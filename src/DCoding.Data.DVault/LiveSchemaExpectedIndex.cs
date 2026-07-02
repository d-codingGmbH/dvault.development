namespace DCoding.Data.DVault;

internal sealed record LiveSchemaExpectedIndex(
    string IndexName,
    IReadOnlyList<string> ColumnNames,
    IReadOnlyList<string> DescendingColumnNames);
