namespace OmopSummary.Models;

public class ConceptCount
{
    public string ConceptName { get; set; } = string.Empty;
    public long Count { get; set; }
    public long ConceptId { get; set; }
    public string ConceptCode { get; set; } = string.Empty;
    public string Vocabulary { get; set; } = string.Empty;
}
