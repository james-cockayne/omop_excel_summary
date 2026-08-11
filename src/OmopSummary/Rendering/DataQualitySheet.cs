using OfficeOpenXml;
using OfficeOpenXml.Style;
using OmopSummary.Models;

namespace OmopSummary.Rendering;

internal static class DataQualitySheet
{
    internal static void Write(ExcelWorksheet ws, DataQualityStats stats)
    {
        ws.Cells[1, 1].Value = "Data Quality";
        ws.Cells[1, 1].Style.Font.Bold = true;
        ws.Cells[1, 1].Style.Font.Size = 14;

        ws.Cells[3, 1].Value = "Domain";
        ws.Cells[3, 2].Value = "Total Records";
        ws.Cells[3, 3].Value = "Records with Valid Concept";
        ws.Cells[3, 4].Value = "Coverage %";
        ws.Cells[3, 1].Style.Font.Bold = true;
        ws.Cells[3, 2].Style.Font.Bold = true;
        ws.Cells[3, 3].Style.Font.Bold = true;
        ws.Cells[3, 4].Style.Font.Bold = true;

        int row = 4;
        foreach (var d in stats.DomainCoverages)
        {
            ws.Cells[row, 1].Value = d.DomainName;
            ws.Cells[row, 2].Value = d.TotalRecords;
            ws.Cells[row, 3].Value = d.RecordsWithValidConcept;
            ws.Cells[row, 4].Value = Math.Round(d.CoveragePercent, 1);
            row++;
        }

        ws.Column(1).Width = 28;
        ws.Column(2).Width = 18;
        ws.Column(3).Width = 30;
        ws.Column(4).Width = 16;
    }
}
