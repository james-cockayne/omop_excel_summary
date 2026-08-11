using OfficeOpenXml;
using OmopSummary.Rendering;

namespace OmopSummary.Tests;

[TestClass]
public class ExcelRenderingTests
{
    private static readonly string OutputPath = Path.Combine(
        Path.GetDirectoryName(typeof(ExcelRenderingTests).Assembly.Location)!,
        "TestReport.xlsx");

    private static readonly string[] ExpectedSheets =
    [
        "Summary",
        "Demographics",
        "Conditions",
        "Measurements",
        "Observations",
        "Device Exposures",
        "Procedure Occurrences",
        "Drug Exposures",
        "Visit Occurrences",
        "Deaths",
        "Data Quality",
    ];

    [ClassInitialize]
    public static void ClassInitialize(TestContext _)
    {
        ExcelRenderer.Render(TestData.BuildReport(), Path.GetFullPath(OutputPath));
    }

    private ExcelPackage OpenPackage() => new(new FileInfo(Path.GetFullPath(OutputPath)));

    [TestMethod]
    public void OutputFile_Exists_And_HasContent()
    {
        Assert.IsTrue(File.Exists(Path.GetFullPath(OutputPath)));
        Assert.IsGreaterThan(0L, new FileInfo(Path.GetFullPath(OutputPath)).Length);
    }

    [TestMethod]
    public void Workbook_Has_Eleven_Sheets()
    {
        using var pkg = OpenPackage();
        Assert.AreEqual(11, pkg.Workbook.Worksheets.Count);
    }

    [TestMethod]
    public void Workbook_SheetNames_Match_Expected()
    {
        using var pkg = OpenPackage();
        var actual = pkg.Workbook.Worksheets.Select(ws => ws.Name).ToArray();
        CollectionAssert.AreEqual(ExpectedSheets, actual);
    }

    [TestMethod]
    public void SummarySheet_DatabaseName_IsCorrect()
    {
        using var pkg = OpenPackage();
        Assert.AreEqual("test.db", pkg.Workbook.Worksheets["Summary"].Cells[3, 2].Value?.ToString());
    }

    [TestMethod]
    public void SummarySheet_PersonCount_IsCorrect()
    {
        using var pkg = OpenPackage();
        // row 7 = Persons (first data row after the header at row 6)
        Assert.AreEqual(50000L, Convert.ToInt64(pkg.Workbook.Worksheets["Summary"].Cells[7, 2].Value));
    }

    [TestMethod]
    public void DemographicsSheet_TotalCount_IsCorrect()
    {
        using var pkg = OpenPackage();
        Assert.AreEqual(50000L, Convert.ToInt64(pkg.Workbook.Worksheets["Demographics"].Cells[3, 2].Value));
    }

    [TestMethod]
    public void DemographicsSheet_FirstAgeGroup_IsCorrect()
    {
        using var pkg = OpenPackage();
        // Age Groups section: title at row 5, header at row 6, first data at row 7
        Assert.AreEqual("0-4", pkg.Workbook.Worksheets["Demographics"].Cells[7, 1].Value?.ToString());
    }

    [TestMethod]
    public void ConditionsSheet_TotalCount_IsCorrect()
    {
        using var pkg = OpenPackage();
        Assert.AreEqual(320000L, Convert.ToInt64(pkg.Workbook.Worksheets["Conditions"].Cells[3, 2].Value));
    }

    [TestMethod]
    public void ConditionsSheet_FirstConcept_IsCorrect()
    {
        using var pkg = OpenPackage();
        Assert.AreEqual("Essential hypertension", pkg.Workbook.Worksheets["Conditions"].Cells[6, 1].Value?.ToString());
    }

    [TestMethod]
    public void DataQualitySheet_HasDomainRows()
    {
        using var pkg = OpenPackage();
        var ws = pkg.Workbook.Worksheets["Data Quality"];
        Assert.AreEqual("Conditions", ws.Cells[4, 1].Value?.ToString());
        Assert.IsNotNull(ws.Cells[10, 1].Value); // last domain row
    }

    [TestMethod]
    public void VisitSheet_TotalCount_IsCorrect()
    {
        using var pkg = OpenPackage();
        Assert.AreEqual(190000L, Convert.ToInt64(pkg.Workbook.Worksheets["Visit Occurrences"].Cells[3, 2].Value));
    }

    [TestMethod]
    public void DeathSheet_TotalCount_IsCorrect()
    {
        using var pkg = OpenPackage();
        Assert.AreEqual(3200L, Convert.ToInt64(pkg.Workbook.Worksheets["Deaths"].Cells[3, 2].Value));
    }
}
