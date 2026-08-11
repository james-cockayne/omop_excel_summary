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

        ConceptSheetWriter.WriteConceptTable(ws, 5, stats.CauseBreakdown);

        ws.Column(1).Width = 50;
        ws.Column(2).Width = 18;
        ws.Column(3).Width = 14;
        ws.Column(4).Width = 20;
        ws.Column(5).Width = 16;
        ws.Column(6).Width = 14;
    }
}
