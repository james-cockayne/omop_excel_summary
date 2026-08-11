using OfficeOpenXml;
using OfficeOpenXml.Style;
using OmopSummary.Models;

namespace OmopSummary.Rendering;

internal static class DeathSheet
{
    internal static void Write(ExcelWorksheet ws, DeathStats stats)
    {
        ws.Cells[1, 1].Value = "Deaths";
        ws.Cells[1, 1].Style.Font.Bold = true;
        ws.Cells[1, 1].Style.Font.Size = 14;

        ws.Cells[3, 1].Value = "Total Deaths";
        ws.Cells[3, 2].Value = stats.TotalCount;

        ws.Cells[5, 1].Value = "Cause of Death";
        ws.Cells[5, 2].Value = "Count";
        ws.Cells[5, 1].Style.Font.Bold = true;
        ws.Cells[5, 2].Style.Font.Bold = true;

        int row = 6;
        foreach (var d in stats.CauseBreakdown)
        {
            ws.Cells[row, 1].Value = d.ConceptName;
            ws.Cells[row, 2].Value = d.Count;
            row++;
        }

        ws.Column(1).Width = 50;
        ws.Column(2).Width = 18;
    }
}
