namespace OmopSummary.Models;

public class DemographicsStats
{
    public long TotalCount { get; set; }
    public List<AgeGroup> AgeGroups { get; set; } = [];
    public List<ConceptCount> GenderBreakdown { get; set; } = [];
    public List<ConceptCount> RaceBreakdown { get; set; } = [];
}
