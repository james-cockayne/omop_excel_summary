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
                new() { ConceptName = "FEMALE", Count = 26200, ConceptId = 8532, ConceptCode = "F", Vocabulary = "Gender" },
                new() { ConceptName = "MALE",   Count = 23800, ConceptId = 8507, ConceptCode = "M", Vocabulary = "Gender" },
            ],
            RaceBreakdown =
            [
                new() { ConceptName = "White",                     Count = 32000, ConceptId = 8527, ConceptCode = "5", Vocabulary = "Race" },
                new() { ConceptName = "Black or African American", Count = 8500,  ConceptId = 8516, ConceptCode = "3", Vocabulary = "Race" },
                new() { ConceptName = "Asian",                     Count = 4200,  ConceptId = 8515, ConceptCode = "2", Vocabulary = "Race" },
                new() { ConceptName = "Other Race",                Count = 5300,  ConceptId = 8522, ConceptCode = "6", Vocabulary = "Race" },
            ],
        },
        Conditions = new ConceptStats
        {
            TotalCount = 320000,
            TopConcepts =
            [
                new() { ConceptName = "Essential hypertension",         Count = 42000, ConceptId = 316866, ConceptCode = "38341003",  Vocabulary = "SNOMED" },
                new() { ConceptName = "Type 2 diabetes mellitus",       Count = 28000, ConceptId = 201826, ConceptCode = "44054006",  Vocabulary = "SNOMED" },
                new() { ConceptName = "Hyperlipidemia",                 Count = 24000, ConceptId = 432867, ConceptCode = "55822004",  Vocabulary = "SNOMED" },
                new() { ConceptName = "Obesity",                        Count = 19000, ConceptId = 433736, ConceptCode = "414916001", Vocabulary = "SNOMED" },
                new() { ConceptName = "Depressive disorder",            Count = 15000, ConceptId = 440383, ConceptCode = "35489007",  Vocabulary = "SNOMED" },
                new() { ConceptName = "Chronic kidney disease stage 3", Count = 12000, ConceptId = 443601, ConceptCode = "433144002", Vocabulary = "SNOMED" },
                new() { ConceptName = "Asthma",                         Count = 11000, ConceptId = 317009, ConceptCode = "195967001", Vocabulary = "SNOMED" },
                new() { ConceptName = "Atrial fibrillation",            Count = 9000,  ConceptId = 313217, ConceptCode = "49436004",  Vocabulary = "SNOMED" },
            ],
        },
        Measurements = new ConceptStats
        {
            TotalCount = 1100000,
            TopConcepts =
            [
                new() { ConceptName = "Hemoglobin A1c/Hemoglobin.total in Blood",        Count = 180000, ConceptId = 3004410, ConceptCode = "4548-4",   Vocabulary = "LOINC" },
                new() { ConceptName = "Creatinine [Mass/volume] in Serum or Plasma",     Count = 160000, ConceptId = 3016723, ConceptCode = "2160-0",   Vocabulary = "LOINC" },
                new() { ConceptName = "Glucose [Mass/volume] in Blood",                  Count = 140000, ConceptId = 3004501, ConceptCode = "2345-7",   Vocabulary = "LOINC" },
                new() { ConceptName = "Body mass index (BMI) [Ratio]",                   Count = 130000, ConceptId = 3038553, ConceptCode = "39156-5",  Vocabulary = "LOINC" },
            ],
        },
        Observations = new ConceptStats
        {
            TotalCount = 85000,
            TopConcepts =
            [
                new() { ConceptName = "Tobacco smoking status", Count = 32000, ConceptId = 4005823, ConceptCode = "229819007", Vocabulary = "SNOMED" },
                new() { ConceptName = "Body weight",            Count = 18000, ConceptId = 3025315, ConceptCode = "29463-7",   Vocabulary = "LOINC" },
                new() { ConceptName = "Alcohol use",            Count = 12000, ConceptId = 4238768, ConceptCode = "160573003", Vocabulary = "SNOMED" },
            ],
        },
        DeviceExposures = new ConceptStats
        {
            TotalCount = 12000,
            TopConcepts =
            [
                new() { ConceptName = "Cardiac pacemaker",          Count = 4200, ConceptId = 4119932, ConceptCode = "14106009",  Vocabulary = "SNOMED" },
                new() { ConceptName = "Continuous glucose monitor",  Count = 3100, ConceptId = 4163246, ConceptCode = "700422006", Vocabulary = "SNOMED" },
            ],
        },
        ProcedureOccurrences = new ConceptStats
        {
            TotalCount = 220000,
            TopConcepts =
            [
                new() { ConceptName = "Colonoscopy",       Count = 28000, ConceptId = 4263110, ConceptCode = "73761001",  Vocabulary = "SNOMED" },
                new() { ConceptName = "Electrocardiogram", Count = 22000, ConceptId = 4023672, ConceptCode = "29303009",  Vocabulary = "SNOMED" },
                new() { ConceptName = "Plain chest X-ray", Count = 19000, ConceptId = 4097276, ConceptCode = "399208008", Vocabulary = "SNOMED" },
            ],
        },
        DrugExposures = new ConceptStats
        {
            TotalCount = 430000,
            TopConcepts =
            [
                new() { ConceptName = "Atorvastatin", Count = 62000, ConceptId = 1545958, ConceptCode = "83367",  Vocabulary = "RxNorm" },
                new() { ConceptName = "Metformin",    Count = 55000, ConceptId = 1503297, ConceptCode = "6809",   Vocabulary = "RxNorm" },
                new() { ConceptName = "Lisinopril",   Count = 48000, ConceptId = 1308216, ConceptCode = "29046",  Vocabulary = "RxNorm" },
                new() { ConceptName = "Amlodipine",   Count = 37000, ConceptId = 1332418, ConceptCode = "17767",  Vocabulary = "RxNorm" },
            ],
        },
        Visits = new VisitStats
        {
            TotalCount = 190000,
            VisitTypeBreakdown =
            [
                new() { ConceptName = "Outpatient Visit",     Count = 130000, ConceptId = 9202, ConceptCode = "AMB",  Vocabulary = "Visit" },
                new() { ConceptName = "Inpatient Visit",      Count = 42000,  ConceptId = 9201, ConceptCode = "IMP",  Vocabulary = "Visit" },
                new() { ConceptName = "Emergency Room Visit", Count = 18000,  ConceptId = 9203, ConceptCode = "EMER", Vocabulary = "Visit" },
            ],
        },
        Deaths = new DeathStats
        {
            TotalCount = 3200,
            CauseBreakdown =
            [
                new() { ConceptName = "Malignant neoplasm of trachea, bronchus and lung", Count = 320, ConceptId = 4291005, ConceptCode = "363358000", Vocabulary = "SNOMED" },
                new() { ConceptName = "Acute myocardial infarction",                      Count = 280, ConceptId = 312327,  ConceptCode = "57054005",  Vocabulary = "SNOMED" },
                new() { ConceptName = "Cerebrovascular disease",                          Count = 210, ConceptId = 381591,  ConceptCode = "62914000",  Vocabulary = "SNOMED" },
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
