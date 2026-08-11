using OmopSummary.Models;

namespace OmopSummary.Tests;

internal static class TestData
{
    internal static OmopReport BuildReport() => new()
    {
        BasicStats = new BasicStats
        {
            DatabaseName = "test.db",
            ReportDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc),
            PersonCount = 50000,
            ConditionOccurrenceCount = 320000,
            MeasurementCount = 1100000,
            ObservationCount = 85000,
            DeviceExposureCount = 12000,
            ProcedureOccurrenceCount = 220000,
            DrugExposureCount = 430000,
            VisitOccurrenceCount = 190000,
            DeathCount = 3200,
        },
        Demographics = new DemographicsStats
        {
            TotalCount = 50000,
            AgeGroups =
            [
                new() { Label = "0-4",   Count = 1800 },
                new() { Label = "5-9",   Count = 2100 },
                new() { Label = "10-14", Count = 2400 },
                new() { Label = "15-19", Count = 2700 },
                new() { Label = "20-24", Count = 3100 },
                new() { Label = "25-29", Count = 3400 },
                new() { Label = "30-34", Count = 3600 },
                new() { Label = "35-39", Count = 3500 },
                new() { Label = "40-44", Count = 3300 },
                new() { Label = "45-49", Count = 3200 },
                new() { Label = "50-54", Count = 3000 },
                new() { Label = "55-59", Count = 2900 },
                new() { Label = "60-64", Count = 2800 },
                new() { Label = "65-69", Count = 2600 },
                new() { Label = "70-74", Count = 2200 },
                new() { Label = "75-79", Count = 1900 },
                new() { Label = "80-84", Count = 1500 },
                new() { Label = "85-89", Count = 900 },
                new() { Label = "90-94", Count = 400 },
            ],
            GenderBreakdown =
            [
                new() { ConceptName = "FEMALE", Count = 26200 },
                new() { ConceptName = "MALE",   Count = 23800 },
            ],
            RaceBreakdown =
            [
                new() { ConceptName = "White",                     Count = 32000 },
                new() { ConceptName = "Black or African American", Count = 8500 },
                new() { ConceptName = "Asian",                     Count = 4200 },
                new() { ConceptName = "Other Race",                Count = 5300 },
            ],
        },
        Conditions = new ConceptStats
        {
            TotalCount = 320000,
            TopConcepts =
            [
                new() { ConceptName = "Essential hypertension",         Count = 42000 },
                new() { ConceptName = "Type 2 diabetes mellitus",       Count = 28000 },
                new() { ConceptName = "Hyperlipidemia",                 Count = 24000 },
                new() { ConceptName = "Obesity",                        Count = 19000 },
                new() { ConceptName = "Depressive disorder",            Count = 15000 },
                new() { ConceptName = "Chronic kidney disease stage 3", Count = 12000 },
                new() { ConceptName = "Asthma",                         Count = 11000 },
                new() { ConceptName = "Atrial fibrillation",            Count = 9000 },
            ],
        },
        Measurements = new ConceptStats
        {
            TotalCount = 1100000,
            TopConcepts =
            [
                new() { ConceptName = "Hemoglobin A1c/Hemoglobin.total in Blood",        Count = 180000 },
                new() { ConceptName = "Creatinine [Mass/volume] in Serum or Plasma",     Count = 160000 },
                new() { ConceptName = "Glucose [Mass/volume] in Blood",                  Count = 140000 },
                new() { ConceptName = "Body mass index (BMI) [Ratio]",                   Count = 130000 },
            ],
        },
        Observations = new ConceptStats
        {
            TotalCount = 85000,
            TopConcepts =
            [
                new() { ConceptName = "Tobacco smoking status", Count = 32000 },
                new() { ConceptName = "Body weight",            Count = 18000 },
                new() { ConceptName = "Alcohol use",            Count = 12000 },
            ],
        },
        DeviceExposures = new ConceptStats
        {
            TotalCount = 12000,
            TopConcepts =
            [
                new() { ConceptName = "Cardiac pacemaker",          Count = 4200 },
                new() { ConceptName = "Continuous glucose monitor",  Count = 3100 },
            ],
        },
        ProcedureOccurrences = new ConceptStats
        {
            TotalCount = 220000,
            TopConcepts =
            [
                new() { ConceptName = "Colonoscopy",       Count = 28000 },
                new() { ConceptName = "Electrocardiogram", Count = 22000 },
                new() { ConceptName = "Plain chest X-ray", Count = 19000 },
            ],
        },
        DrugExposures = new ConceptStats
        {
            TotalCount = 430000,
            TopConcepts =
            [
                new() { ConceptName = "Atorvastatin", Count = 62000 },
                new() { ConceptName = "Metformin",    Count = 55000 },
                new() { ConceptName = "Lisinopril",   Count = 48000 },
                new() { ConceptName = "Amlodipine",   Count = 37000 },
            ],
        },
        Visits = new VisitStats
        {
            TotalCount = 190000,
            VisitTypeBreakdown =
            [
                new() { ConceptName = "Outpatient Visit",     Count = 130000 },
                new() { ConceptName = "Inpatient Visit",      Count = 42000 },
                new() { ConceptName = "Emergency Room Visit", Count = 18000 },
            ],
        },
        Deaths = new DeathStats
        {
            TotalCount = 3200,
            CauseBreakdown =
            [
                new() { ConceptName = "Malignant neoplasm of trachea, bronchus and lung", Count = 320 },
                new() { ConceptName = "Acute myocardial infarction",                      Count = 280 },
                new() { ConceptName = "Cerebrovascular disease",                          Count = 210 },
            ],
        },
        DataQuality = new DataQualityStats
        {
            DomainCoverages =
            [
                new() { DomainName = "Conditions",            TotalRecords = 320000,  RecordsWithValidConcept = 314000 },
                new() { DomainName = "Measurements",          TotalRecords = 1100000, RecordsWithValidConcept = 1088000 },
                new() { DomainName = "Observations",          TotalRecords = 85000,   RecordsWithValidConcept = 82000 },
                new() { DomainName = "Device Exposures",      TotalRecords = 12000,   RecordsWithValidConcept = 11800 },
                new() { DomainName = "Procedure Occurrences", TotalRecords = 220000,  RecordsWithValidConcept = 218000 },
                new() { DomainName = "Drug Exposures",        TotalRecords = 430000,  RecordsWithValidConcept = 425000 },
                new() { DomainName = "Visit Occurrences",     TotalRecords = 190000,  RecordsWithValidConcept = 189000 },
            ],
        },
    };
}
