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

        WriteConceptTable(ws, 5, stats.TopConcepts, "Count (rounded to 10)");

        ws.Column(1).Width = 50;
        ws.Column(2).Width = 24;
        ws.Column(3).Width = 14;
        ws.Column(4).Width = 20;
        ws.Column(5).Width = 16;
        ws.Column(6).Width = 14;
    }

    internal static int WriteConceptTable(ExcelWorksheet ws, int startRow, IEnumerable<ConceptCount> concepts,
        string countHeader = "Count")
    {
        ws.Cells[startRow, 1].Value = "Concept";      ws.Cells[startRow, 1].Style.Font.Bold = true;
        ws.Cells[startRow, 2].Value = countHeader;    ws.Cells[startRow, 2].Style.Font.Bold = true;
        ws.Cells[startRow, 3].Value = "Concept ID";   ws.Cells[startRow, 3].Style.Font.Bold = true;
        ws.Cells[startRow, 4].Value = "Concept Code"; ws.Cells[startRow, 4].Style.Font.Bold = true;
        ws.Cells[startRow, 5].Value = "Vocabulary";   ws.Cells[startRow, 5].Style.Font.Bold = true;
        ws.Cells[startRow, 6].Value = "Athena";       ws.Cells[startRow, 6].Style.Font.Bold = true;
        startRow++;

        foreach (var c in concepts)
        {
            ws.Cells[startRow, 1].Value = c.ConceptName;
            ws.Cells[startRow, 2].Value = c.Count;
            if (c.ConceptId > 0)
            {
                ws.Cells[startRow, 3].Value = c.ConceptId;
                ws.Cells[startRow, 4].Value = c.ConceptCode;
                ws.Cells[startRow, 5].Value = c.Vocabulary;
                ws.Cells[startRow, 6].Hyperlink = new Uri($"https://athena.ohdsi.org/search-terms/terms/{c.ConceptId}");
                ws.Cells[startRow, 6].Value = "Athena";
                ws.Cells[startRow, 6].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                ws.Cells[startRow, 6].Style.Font.UnderLine = true;
            }
            startRow++;
        }

        return startRow;
    }
}

