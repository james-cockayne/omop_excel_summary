using OfficeOpenXml;
using OfficeOpenXml.Style;
using OmopSummary.Models;

namespace OmopSummary.Rendering;

internal static class VisitSheet
{
    internal static void Write(ExcelWorksheet ws, VisitStats stats)
    {
        ws.Cells[1, 1].Value = "Visit Occurrences";
        ws.Cells[1, 1].Style.Font.Bold = true;
        ws.Cells[1, 1].Style.Font.Size = 14;

        ws.Cells[3, 1].Value = "Total Records";
        ws.Cells[3, 2].Value = stats.TotalCount;

        ws.Cells[5, 1].Value = "Visit Type";
        ws.Cells[5, 2].Value = "Count";
        ws.Cells[5, 1].Style.Font.Bold = true;
        ws.Cells[5, 2].Style.Font.Bold = true;

        int row = 6;
        foreach (var v in stats.VisitTypeBreakdown)
        {
            ws.Cells[row, 1].Value = v.ConceptName;
            ws.Cells[row, 2].Value = v.Count;
            row++;
        }

        ws.Column(1).Width = 40;
        ws.Column(2).Width = 18;
    }
}
