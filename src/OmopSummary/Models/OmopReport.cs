namespace OmopSummary.Models;

public class OmopReport
{
    public BasicStats BasicStats { get; set; } = new();
    public DemographicsStats Demographics { get; set; } = new();
    public ConceptStats Conditions { get; set; } = new();
    public ConceptStats Measurements { get; set; } = new();
    public ConceptStats Observations { get; set; } = new();
    public ConceptStats DeviceExposures { get; set; } = new();
    public ConceptStats ProcedureOccurrences { get; set; } = new();
    public ConceptStats DrugExposures { get; set; } = new();
    public VisitStats Visits { get; set; } = new();
    public DeathStats Deaths { get; set; } = new();
    public DataQualityStats DataQuality { get; set; } = new();
}
