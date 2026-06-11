using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record OrderFulfillmentEvent(
    string AllocationStatus,
    string WarehouseCode,
    DateTimeOffset ChangedAtUtc,
    string RecordSource,
    string HashDiff);
