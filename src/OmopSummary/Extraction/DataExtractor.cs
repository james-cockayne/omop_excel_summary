using DuckDB.NET.Data;
using OmopSummary.Models;

namespace OmopSummary.Extraction;

public class DataExtractor
{
    private readonly string _dbPath;
    private readonly string _schema;

    public DataExtractor(string dbPath, string schema)
    {
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

    private BasicStats ExtractBasicStats(DuckDBConnection connection)
    {
        return new BasicStats
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
    }

    private long QueryCount(DuckDBConnection connection, string table)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT COUNT(*) FROM {_schema}.{table}";
        return (long)cmd.ExecuteScalar()!;
    }

    private DemographicsStats ExtractDemographics(DuckDBConnection connection)
    {
        return new DemographicsStats
        {
            TotalCount = QueryCount(connection, "person"),
            AgeGroups = ExtractAgeGroups(connection),
            GenderBreakdown = ExtractPersonConceptBreakdown(connection, "gender_concept_id"),
            RaceBreakdown = ExtractPersonConceptBreakdown(connection, "race_concept_id"),
        };
    }

    private List<AgeGroup> ExtractAgeGroups(DuckDBConnection connection)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"""
            SELECT
                CAST(FLOOR((YEAR(CURRENT_DATE) - year_of_birth) / 5) * 5 AS INTEGER) AS band_start,
                COUNT(*) AS cnt
            FROM {_schema}.person
            WHERE year_of_birth IS NOT NULL
            GROUP BY band_start
            ORDER BY band_start
            """;

        var groups = new List<AgeGroup>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int start = reader.GetInt32(0);
            long count = reader.GetInt64(1);
            groups.Add(new AgeGroup { Label = $"{start}-{start + 4}", Count = count });
        }
        return groups;
    }

    private List<ConceptCount> ExtractPersonConceptBreakdown(DuckDBConnection connection, string conceptColumn)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"""
            SELECT
                COALESCE(c.concept_name, 'Unknown') AS concept_name,
                COUNT(*) AS cnt
            FROM {_schema}.person p
            LEFT JOIN {_schema}.concept c ON p.{conceptColumn} = c.concept_id
            WHERE p.{conceptColumn} != 0
            GROUP BY c.concept_name
            ORDER BY cnt DESC
            """;

        return ReadConceptCounts(cmd);
    }

    private ConceptStats ExtractConceptStats(DuckDBConnection connection, string table, string conceptColumn)
    {
        var total = QueryCount(connection, table);

        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"""
            SELECT
                COALESCE(c.concept_name, 'Unknown') AS concept_name,
                COUNT(*) AS cnt
            FROM {_schema}.{table} t
            LEFT JOIN {_schema}.concept c ON t.{conceptColumn} = c.concept_id
            WHERE t.{conceptColumn} != 0
            GROUP BY c.concept_name
            HAVING COUNT(*) > 1000
            ORDER BY cnt DESC
            """;

        var concepts = ReadConceptCounts(cmd);
        // Round each count to nearest 10
        foreach (var cc in concepts)
            cc.Count = (long)(Math.Round(cc.Count / 10.0) * 10);

        return new ConceptStats { TotalCount = total, TopConcepts = concepts };
    }

    private VisitStats ExtractVisitStats(DuckDBConnection connection)
    {
        var total = QueryCount(connection, "visit_occurrence");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"""
            SELECT
                COALESCE(c.concept_name, 'Unknown') AS concept_name,
                COUNT(*) AS cnt
            FROM {_schema}.visit_occurrence v
            LEFT JOIN {_schema}.concept c ON v.visit_type_concept_id = c.concept_id
            WHERE v.visit_type_concept_id != 0
            GROUP BY c.concept_name
            ORDER BY cnt DESC
            """;

        return new VisitStats { TotalCount = total, VisitTypeBreakdown = ReadConceptCounts(cmd) };
    }

    private DeathStats ExtractDeathStats(DuckDBConnection connection)
    {
        var total = QueryCount(connection, "death");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"""
            SELECT
                COALESCE(c.concept_name, 'Unknown') AS concept_name,
                COUNT(*) AS cnt
            FROM {_schema}.death d
            LEFT JOIN {_schema}.concept c ON d.cause_concept_id = c.concept_id
            WHERE d.cause_concept_id != 0
            GROUP BY c.concept_name
            HAVING COUNT(*) > 10
            ORDER BY cnt DESC
            """;

        return new DeathStats { TotalCount = total, CauseBreakdown = ReadConceptCounts(cmd) };
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
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"""
                SELECT
                    COUNT(*) AS total,
                    SUM(CASE WHEN {column} != 0 AND {column} IS NOT NULL THEN 1 ELSE 0 END) AS valid
                FROM {_schema}.{table}
                """;
            using var reader = cmd.ExecuteReader();
            reader.Read();
            coverages.Add(new DomainCoverage
            {
                DomainName = name,
                TotalRecords = reader.IsDBNull(0) ? 0L : reader.GetInt64(0),
                RecordsWithValidConcept = reader.IsDBNull(1) ? 0L : reader.GetInt64(1),
            });
        }

        return new DataQualityStats { DomainCoverages = coverages };
    }

    private static List<ConceptCount> ReadConceptCounts(DuckDBCommand cmd)
    {
        var results = new List<ConceptCount>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            results.Add(new ConceptCount { ConceptName = reader.GetString(0), Count = reader.GetInt64(1) });
        return results;
    }
}
