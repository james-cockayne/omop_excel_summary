using OfficeOpenXml;
using OfficeOpenXml.Style;
using OmopSummary.Models;

namespace OmopSummary.Rendering;

internal static class DemographicsSheet
{
    internal static void Write(ExcelWorksheet ws, DemographicsStats stats)
    {
        ws.Cells[1, 1].Value = "Patient Demographics";
        ws.Cells[1, 1].Style.Font.Bold = true;
        ws.Cells[1, 1].Style.Font.Size = 14;

        WriteRow(ws, 3, "Total Patients", stats.TotalCount);

        int row = 5;
        row = WriteAgeSection(ws, row, stats.AgeGroups);

        row++;
        row = WriteConceptSection(ws, row, "Gender", stats.GenderBreakdown);

        row++;
        WriteConceptSection(ws, row, "Race", stats.RaceBreakdown);

        ws.Column(1).Width = 35;
        ws.Column(2).Width = 18;
        ws.Column(3).Width = 14;
        ws.Column(4).Width = 20;
        ws.Column(5).Width = 16;
        ws.Column(6).Width = 14;
    }

    private static int WriteAgeSection(ExcelWorksheet ws, int startRow, IEnumerable<AgeGroup> groups)
    {
        ws.Cells[startRow, 1].Value = "Age Groups";
        ws.Cells[startRow, 1].Style.Font.Bold = true;
        startRow++;

        ws.Cells[startRow, 1].Value = "Age Band"; ws.Cells[startRow, 1].Style.Font.Bold = true;
        ws.Cells[startRow, 2].Value = "Count";    ws.Cells[startRow, 2].Style.Font.Bold = true;
        startRow++;

        foreach (var g in groups)
        {
            ws.Cells[startRow, 1].Value = g.Label;
            ws.Cells[startRow, 2].Value = g.Count;
            startRow++;
        }
        return startRow;
    }

    private static int WriteConceptSection(ExcelWorksheet ws, int startRow, string title,
        IEnumerable<ConceptCount> concepts)
    {
        ws.Cells[startRow, 1].Value = title;
        ws.Cells[startRow, 1].Style.Font.Bold = true;
        startRow++;
        return ConceptSheetWriter.WriteConceptTable(ws, startRow, concepts);
    }

    private static void WriteRow(ExcelWorksheet ws, int row, string label, object value)
    {
        ws.Cells[row, 1].Value = label;
        ws.Cells[row, 2].Value = value;
    }
}
