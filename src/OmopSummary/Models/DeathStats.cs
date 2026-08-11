namespace OmopSummary.Models;

public class DeathStats
{
    public long TotalCount { get; set; }
    public List<ConceptCount> CauseBreakdown { get; set; } = [];
}
