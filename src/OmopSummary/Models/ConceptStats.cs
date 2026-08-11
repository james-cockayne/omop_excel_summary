namespace OmopSummary.Models;

public class ConceptStats
{
    public long TotalCount { get; set; }
    // Concepts with raw count > 1000, rounded to nearest 10
    public List<ConceptCount> TopConcepts { get; set; } = [];
}
