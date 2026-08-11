# OMOP Excel Summary

A command-line tool that connects to a DuckDB database containing an OMOP CDM 5.4 dataset, extracts patient and clinical statistics across key domains, and produces a multi-sheet Excel workbook summarising the database contents.

## Prerequisites

- Docker

## Usage

```
docker run --rm \
  -v /path/to/your/data:/data \
  ghcr.io/your-org/omop-summary:latest \
  /data/<db-filename> <report-name> [schema]
```


| Argument | Required | Description |
|---|---|---|
| `<db-filename>` | Yes | Full path to the DuckDB file inside the container (e.g. `/data/cibuild.db`) |
| `<report-name>` | Yes | Base name for the output file (written as `<report-name>.xlsx` alongside the database) |
| `[schema]` | No | OMOP CDM schema name (default: `cdm`) |

## Example

```
docker run --rm \
  -v /home/user/omop-data:/data \
  ghcr.io/your-org/omop-summary:latest \
  /data/cibuild.db MyReport cdm
```

This reads `omop.db` from `/home/user/omop-data` and writes `MyReport.xlsx` to the same directory.

## Output

The workbook contains one sheet per domain:

| Sheet | Contents |
|---|---|
| Summary | Record counts for all tables, database name, report date |
| Demographics | Patient count, age bands, gender breakdown, race breakdown |
| Conditions | Total count + top conditions (frequency > 1000, rounded to 10) |
| Measurements | Total count + top measurements |
| Observations | Total count + top observations |
| Device Exposures | Total count + top devices |
| Procedure Occurrences | Total count + top procedures |
| Drug Exposures | Total count + top drugs |
| Visit Occurrences | Total count + visit type breakdown |
| Deaths | Total count + cause of death breakdown |
| Data Quality | Per-domain concept coverage percentage |
