namespace OmopSummary.Models;

public class DomainCoverage
{
    public string DomainName { get; set; } = string.Empty;
    public long TotalRecords { get; set; }
    public long RecordsWithValidConcept { get; set; }
    public double CoveragePercent => TotalRecords == 0 ? 0 : RecordsWithValidConcept * 100.0 / TotalRecords;
}

public class DataQualityStats
{
    public List<DomainCoverage> DomainCoverages { get; set; } = [];
}
