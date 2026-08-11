using OfficeOpenXml;
using OfficeOpenXml.Style;
using OmopSummary.Models;

namespace OmopSummary.Rendering;

internal static class ConceptSheetWriter
{
    internal static void Write(ExcelWorksheet ws, string title, ConceptStats stats)
    {
        ws.Cells[1, 1].Value = title;
        ws.Cells[1, 1].Style.Font.Bold = true;
        ws.Cells[1, 1].Style.Font.Size = 14;

        ws.Cells[3, 1].Value = "Total Records";
        ws.Cells[3, 2].Value = stats.TotalCount;

        ws.Cells[5, 1].Value = "Concept";
        ws.Cells[5, 2].Value = "Count (rounded to 10)";
        ws.Cells[5, 1].Style.Font.Bold = true;
        ws.Cells[5, 2].Style.Font.Bold = true;

        int row = 6;
        foreach (var concept in stats.TopConcepts)
        {
            ws.Cells[row, 1].Value = concept.ConceptName;
            ws.Cells[row, 2].Value = concept.Count;
            row++;
        }

        ws.Column(1).Width = 50;
        ws.Column(2).Width = 24;
    }
}
