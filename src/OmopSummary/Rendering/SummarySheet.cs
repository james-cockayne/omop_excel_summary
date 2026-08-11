using OfficeOpenXml;
using OfficeOpenXml.Style;
using OmopSummary.Models;

namespace OmopSummary.Rendering;

internal static class SummarySheet
{
    internal static void Write(ExcelWorksheet ws, OmopReport report)
    {
        var stats = report.BasicStats;

        ws.Cells[1, 1].Value = "OMOP Database Summary";
        ws.Cells[1, 1].Style.Font.Bold = true;
        ws.Cells[1, 1].Style.Font.Size = 14;

        WriteRow(ws, 3, "Database", stats.DatabaseName);
        WriteRow(ws, 4, "Report Date", stats.ReportDate.ToString("yyyy-MM-dd"));

        ws.Cells[6, 1].Value = "Table";
        ws.Cells[6, 2].Value = "Record Count";
        ws.Cells[6, 1].Style.Font.Bold = true;
        ws.Cells[6, 2].Style.Font.Bold = true;

        int row = 7;
        WriteRow(ws, row++, "Persons",                stats.PersonCount);
        WriteRow(ws, row++, "Conditions",             stats.ConditionOccurrenceCount);
        WriteRow(ws, row++, "Measurements",           stats.MeasurementCount);
        WriteRow(ws, row++, "Observations",           stats.ObservationCount);
        WriteRow(ws, row++, "Device Exposures",       stats.DeviceExposureCount);
        WriteRow(ws, row++, "Procedure Occurrences",  stats.ProcedureOccurrenceCount);
        WriteRow(ws, row++, "Drug Exposures",         stats.DrugExposureCount);
        WriteRow(ws, row++, "Visit Occurrences",      stats.VisitOccurrenceCount);
        WriteRow(ws, row++, "Deaths",                 stats.DeathCount);

        ws.Column(1).Width = 28;
        ws.Column(2).Width = 18;
    }

    private static void WriteRow(ExcelWorksheet ws, int row, string label, object value)
    {
        ws.Cells[row, 1].Value = label;
        ws.Cells[row, 2].Value = value;
    }
}
