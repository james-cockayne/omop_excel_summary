using Dapper;
using DuckDB.NET.Data;
using OmopSummary.Models;
using System.Text.RegularExpressions;

namespace OmopSummary.Extraction;

public class DataExtractor
{
    private static readonly Regex ValidIdentifier = new(@"^\w+$", RegexOptions.Compiled);

    private readonly string _dbPath;
    private readonly string _schema;

    public DataExtractor(string dbPath, string schema)
    {
        if (!ValidIdentifier.IsMatch(schema))
            throw new ArgumentException($"Invalid schema name '{schema}'. Must be a simple SQL identifier (letters, digits, underscores, must not start with a digit).", nameof(schema));

        _dbPath = dbPath;
        _schema = schema;
    }

    public OmopReport ExtractAll()
    {
        using var connection = new DuckDBConnection($"Data Source={_dbPath}");
        connection.Open();

        return new OmopReport
        {
            BasicStats = ExtractBasicStats(connection),
            Demographics = ExtractDemographics(connection),
            Conditions = ExtractConceptStats(connection, "condition_occurrence", "condition_concept_id"),
            Measurements = ExtractConceptStats(connection, "measurement", "measurement_concept_id"),
            Observations = ExtractConceptStats(connection, "observation", "observation_concept_id"),
            DeviceExposures = ExtractConceptStats(connection, "device_exposure", "device_concept_id"),
            ProcedureOccurrences = ExtractConceptStats(connection, "procedure_occurrence", "procedure_concept_id"),
            DrugExposures = ExtractConceptStats(connection, "drug_exposure", "drug_concept_id"),
            Visits = ExtractVisitStats(connection),
            Deaths = ExtractDeathStats(connection),
            DataQuality = ExtractDataQuality(connection),
        };
    }

    // Private Dapper result types — column names must match SQL aliases
    private sealed class AgeBandRow
    {
        public int band_start { get; set; }
        public long cnt { get; set; }
    }

    private sealed class ConceptRow
    {
        public string concept_name { get; set; } = "";
        public long cnt { get; set; }
        public int concept_id { get; set; }
        public string concept_code { get; set; } = "";
        public string vocabulary_id { get; set; } = "";
    }

    private sealed class DomainQualRow
    {
        public long total { get; set; }
        public long? valid { get; set; }
    }

    private BasicStats ExtractBasicStats(DuckDBConnection connection) => new()
    {
        DatabaseName = Path.GetFileName(_dbPath),
        ReportDate = DateTime.UtcNow,
        PersonCount = QueryCount(connection, "person"),
        ObservationCount = QueryCount(connection, "observation"),
        ConditionOccurrenceCount = QueryCount(connection, "condition_occurrence"),
        MeasurementCount = QueryCount(connection, "measurement"),
        DeviceExposureCount = QueryCount(connection, "device_exposure"),
        ProcedureOccurrenceCount = QueryCount(connection, "procedure_occurrence"),
        DrugExposureCount = QueryCount(connection, "drug_exposure"),
        VisitOccurrenceCount = QueryCount(connection, "visit_occurrence"),
        DeathCount = QueryCount(connection, "death"),
    };

    private long QueryCount(DuckDBConnection connection, string table)
        => connection.ExecuteScalar<long>($"SELECT COUNT(*) FROM {_schema}.{table}");

    private DemographicsStats ExtractDemographics(DuckDBConnection connection) => new()
    {
        TotalCount = QueryCount(connection, "person"),
        AgeGroups = ExtractAgeGroups(connection),
        GenderBreakdown = ExtractPersonConceptBreakdown(connection, "gender_concept_id"),
        RaceBreakdown = ExtractPersonConceptBreakdown(connection, "race_concept_id"),
    };

    private List<AgeGroup> ExtractAgeGroups(DuckDBConnection connection)
    {
        var sql = $"""
            SELECT
                CAST(FLOOR((YEAR(CURRENT_DATE) - year_of_birth) / 5) * 5 AS INTEGER) AS band_start,
                COUNT(*) AS cnt
            FROM {_schema}.person
            WHERE year_of_birth IS NOT NULL
            GROUP BY band_start
            ORDER BY band_start
            """;

        return connection.Query<AgeBandRow>(sql)
            .Select(r => new AgeGroup { Label = $"{r.band_start}-{r.band_start + 4}", Count = r.cnt })
            .ToList();
    }

    private List<ConceptCount> ExtractPersonConceptBreakdown(DuckDBConnection connection, string conceptColumn)
    {
        var sql = $"""
            SELECT
                COALESCE(c.concept_name, 'Unknown') AS concept_name,
                COUNT(*) AS cnt,
                COALESCE(c.concept_id, 0) AS concept_id,
                COALESCE(c.concept_code, '') AS concept_code,
                COALESCE(c.vocabulary_id, '') AS vocabulary_id
            FROM {_schema}.person p
            LEFT JOIN {_schema}.concept c ON p.{conceptColumn} = c.concept_id
            GROUP BY c.concept_id, c.concept_name, c.concept_code, c.vocabulary_id
            ORDER BY cnt DESC
            """;

        return MapConceptCounts(connection.Query<ConceptRow>(sql));
    }

    private ConceptStats ExtractConceptStats(DuckDBConnection connection, string table, string conceptColumn)
    {
        var total = QueryCount(connection, table);

        var sql = $"""
            SELECT
                COALESCE(c.concept_name, 'Unknown') AS concept_name,
                COUNT(*) AS cnt,
                COALESCE(c.concept_id, 0) AS concept_id,
                COALESCE(c.concept_code, '') AS concept_code,
                COALESCE(c.vocabulary_id, '') AS vocabulary_id
            FROM {_schema}.{table} t
            LEFT JOIN {_schema}.concept c ON t.{conceptColumn} = c.concept_id
            WHERE t.{conceptColumn} != 0
            GROUP BY c.concept_id, c.concept_name, c.concept_code, c.vocabulary_id
            HAVING COUNT(*) > 10
            ORDER BY cnt DESC
            """;

        var concepts = MapConceptCounts(connection.Query<ConceptRow>(sql));
        // Round each count to nearest 10
        foreach (var cc in concepts)
            cc.Count = (long)(Math.Round(cc.Count / 10.0) * 10);

        return new ConceptStats { TotalCount = total, TopConcepts = concepts };
    }

    private VisitStats ExtractVisitStats(DuckDBConnection connection)
    {
        var total = QueryCount(connection, "visit_occurrence");

        var sql = $"""
            SELECT
                COALESCE(c.concept_name, 'Unknown') AS concept_name,
                COUNT(*) AS cnt,
                COALESCE(c.concept_id, 0) AS concept_id,
                COALESCE(c.concept_code, '') AS concept_code,
                COALESCE(c.vocabulary_id, '') AS vocabulary_id
            FROM {_schema}.visit_occurrence v
            LEFT JOIN {_schema}.concept c ON v.visit_type_concept_id = c.concept_id
            WHERE v.visit_type_concept_id != 0
            GROUP BY c.concept_id, c.concept_name, c.concept_code, c.vocabulary_id
            ORDER BY cnt DESC
            """;

        return new VisitStats { TotalCount = total, VisitTypeBreakdown = MapConceptCounts(connection.Query<ConceptRow>(sql)) };
    }

    private DeathStats ExtractDeathStats(DuckDBConnection connection)
    {
        var total = QueryCount(connection, "death");

        var sql = $"""
            SELECT
                COALESCE(c.concept_name, 'Unknown') AS concept_name,
                COUNT(*) AS cnt,
                COALESCE(c.concept_id, 0) AS concept_id,
                COALESCE(c.concept_code, '') AS concept_code,
                COALESCE(c.vocabulary_id, '') AS vocabulary_id
            FROM {_schema}.death d
            LEFT JOIN {_schema}.concept c ON d.cause_concept_id = c.concept_id
            WHERE d.cause_concept_id != 0
            GROUP BY c.concept_id, c.concept_name, c.concept_code, c.vocabulary_id
            HAVING COUNT(*) > 10
            ORDER BY cnt DESC
            """;

        return new DeathStats { TotalCount = total, CauseBreakdown = MapConceptCounts(connection.Query<ConceptRow>(sql)) };
    }

    private DataQualityStats ExtractDataQuality(DuckDBConnection connection)
    {
        var domains = new[]
        {
            ("Conditions",           "condition_occurrence",  "condition_concept_id"),
            ("Measurements",         "measurement",           "measurement_concept_id"),
            ("Observations",         "observation",           "observation_concept_id"),
            ("Device Exposures",     "device_exposure",       "device_concept_id"),
            ("Procedure Occurrences","procedure_occurrence",  "procedure_concept_id"),
            ("Drug Exposures",       "drug_exposure",         "drug_concept_id"),
            ("Visit Occurrences",    "visit_occurrence",      "visit_concept_id"),
        };

        var coverages = new List<DomainCoverage>();
        foreach (var (name, table, column) in domains)
        {
            var sql = $"""
                SELECT
                    COUNT(*) AS total,
                    CAST(SUM(CASE WHEN {column} != 0 AND {column} IS NOT NULL THEN 1 ELSE 0 END) AS BIGINT) AS valid
                FROM {_schema}.{table}
                """;
            var row = connection.QuerySingle<DomainQualRow>(sql);
            coverages.Add(new DomainCoverage
            {
                DomainName = name,
                TotalRecords = row.total,
                RecordsWithValidConcept = row.valid ?? 0L,
            });
        }

        return new DataQualityStats { DomainCoverages = coverages };
    }

    private static List<ConceptCount> MapConceptCounts(IEnumerable<ConceptRow> rows)
        => rows.Select(r => new ConceptCount
        {
            ConceptName = r.concept_name,
            Count = r.cnt,
            ConceptId = r.concept_id,
            ConceptCode = r.concept_code,
            Vocabulary = r.vocabulary_id,
        }).ToList();
}
