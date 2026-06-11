using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record OrderRelationshipEvent(
    string OrderBusinessKey,
    string ProductBusinessKey,
    string ProductName,
    DateTimeOffset CreatedAtUtc,
    string RecordSource);
