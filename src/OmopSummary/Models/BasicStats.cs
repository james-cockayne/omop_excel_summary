namespace OmopSummary.Models;

public class BasicStats
{
    public string DatabaseName { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public long PersonCount { get; set; }
    public long ObservationCount { get; set; }
    public long ConditionOccurrenceCount { get; set; }
    public long MeasurementCount { get; set; }
    public long DeviceExposureCount { get; set; }
    public long ProcedureOccurrenceCount { get; set; }
    public long DrugExposureCount { get; set; }
    public long VisitOccurrenceCount { get; set; }
    public long DeathCount { get; set; }
}
