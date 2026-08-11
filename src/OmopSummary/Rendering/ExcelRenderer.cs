using OfficeOpenXml;
using OmopSummary.Models;

namespace OmopSummary.Rendering;

public static class ExcelRenderer
{
    public static void Render(OmopReport report, string outputPath)
    {
        using var package = new ExcelPackage();

        SummarySheet.Write(package.Workbook.Worksheets.Add("Summary"), report);
        DemographicsSheet.Write(package.Workbook.Worksheets.Add("Demographics"), report.Demographics);
        ConceptSheetWriter.Write(package.Workbook.Worksheets.Add("Conditions"), "Conditions", report.Conditions);
        ConceptSheetWriter.Write(package.Workbook.Worksheets.Add("Measurements"), "Measurements", report.Measurements);
        ConceptSheetWriter.Write(package.Workbook.Worksheets.Add("Observations"), "Observations", report.Observations);
        ConceptSheetWriter.Write(package.Workbook.Worksheets.Add("Device Exposures"), "Device Exposures", report.DeviceExposures);
        ConceptSheetWriter.Write(package.Workbook.Worksheets.Add("Procedure Occurrences"), "Procedure Occurrences", report.ProcedureOccurrences);
        ConceptSheetWriter.Write(package.Workbook.Worksheets.Add("Drug Exposures"), "Drug Exposures", report.DrugExposures);
        VisitSheet.Write(package.Workbook.Worksheets.Add("Visit Occurrences"), report.Visits);
        DeathSheet.Write(package.Workbook.Worksheets.Add("Deaths"), report.Deaths);
        DataQualitySheet.Write(package.Workbook.Worksheets.Add("Data Quality"), report.DataQuality);

        package.SaveAs(new FileInfo(outputPath));
    }
}
