namespace OmopSummary.Models;

public class VisitStats
{
    public long TotalCount { get; set; }
    public List<ConceptCount> VisitTypeBreakdown { get; set; } = [];
}
