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
        row = WriteSection(ws, row, "Age Groups", ["Age Band", "Count"],
            stats.AgeGroups.Select(g => new[] { (object)g.Label, g.Count }));

        row++;
        row = WriteSection(ws, row, "Gender", ["Gender", "Count"],
            stats.GenderBreakdown.Select(g => new[] { (object)g.ConceptName, g.Count }));

        row++;
        WriteSection(ws, row, "Race", ["Race", "Count"],
            stats.RaceBreakdown.Select(g => new[] { (object)g.ConceptName, g.Count }));

        ws.Column(1).Width = 35;
        ws.Column(2).Width = 18;
    }

    private static int WriteSection(ExcelWorksheet ws, int startRow, string title,
        string[] headers, IEnumerable<object[]> rows)
    {
        ws.Cells[startRow, 1].Value = title;
        ws.Cells[startRow, 1].Style.Font.Bold = true;
        startRow++;

        for (int col = 0; col < headers.Length; col++)
        {
            ws.Cells[startRow, col + 1].Value = headers[col];
            ws.Cells[startRow, col + 1].Style.Font.Bold = true;
        }
        startRow++;

        foreach (var row in rows)
        {
            for (int col = 0; col < row.Length; col++)
                ws.Cells[startRow, col + 1].Value = row[col];
            startRow++;
        }

        return startRow;
    }

    private static void WriteRow(ExcelWorksheet ws, int row, string label, object value)
    {
        ws.Cells[row, 1].Value = label;
        ws.Cells[row, 2].Value = value;
    }
}
