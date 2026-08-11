using OmopSummary.Extraction;
using OmopSummary.Rendering;

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: OmopSummary <db-path> <report-name> [schema]");
    return 1;
}

string dbPath = args[0];
string reportName = args[1];
string schema = args.Length >= 3 ? args[2] : "cdm";

string outputPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(dbPath))!, $"{reportName}.xlsx");

Console.WriteLine($"Extracting data from {dbPath} (schema: {schema})...");
var report = new DataExtractor(dbPath, schema).ExtractAll();

Console.WriteLine($"Rendering report to {outputPath}...");
ExcelRenderer.Render(report, outputPath);

Console.WriteLine("Done.");
return 0;
