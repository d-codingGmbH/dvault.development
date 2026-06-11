using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record CustomerProfileEvent(
    string CustomerBusinessKey,
    string CustomerName,
    string CustomerStatus,
    DateTimeOffset ChangedAtUtc,
    string RecordSource,
    string HashDiff);
