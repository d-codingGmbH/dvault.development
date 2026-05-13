# PIT-Backed As-Of Read API Contract

Status: v1 planning contract
Ticket: 06F0MEGYHADPVN575H64D56W2G
Baseline references: `README.md`, `docs/releases/v0.6.0.md`, `docs/plans/deferred-data-vault-capabilities.md`

## Purpose

Define the bounded v1 PIT-backed as-of read contract before runtime implementation. The contract extends the existing `IDataVaultReadService` latest/as-of satellite read pattern with one provider-neutral PIT request, one raw PIT read-record shape, and caller-owned typed projectors.

This document is a contract and fixture target. It does not implement PIT row querying, PIT refresh, provider-specific SQL, or PIT maintenance.

## Compatibility Baseline

PIT-backed reads stay on the existing public read boundary:

- `IDataVaultReadService` remains the service entry point.
- Request objects describe explicit metadata and parent hash keys.
- Raw row APIs remain available for advanced callers.
- Typed read models are built by caller-owned projector delegates.
- The lower-level provider capability pipeline continues to hide timestamp storage details from callers.

The current latest/as-of satellite behavior is unchanged. PIT-backed reads add a separate request and raw-record family for `DataVaultPitMetadata`; they do not change `DataVaultLatestSatelliteReadRequest`, `DataVaultSatelliteReadRecord`, or latest-satellite fallback behavior.

The v1 PIT vocabulary is the newer `DataVaultPitMetadata` baseline. The older `DataVaultPointInTimeMetadata` and `DataVaultModelBuilder.PointInTime(...)` surface remains historical and out of scope for this contract.

## Supported V1 Shape

One read request targets exactly one `DataVaultPitMetadata` declaration. The declaration must describe:

- one hub parent through `DataVaultMetadataReference.Hub(...)`
- one or more ordered hub-attached satellite references
- ordinary non-multi-active satellite references only
- no bridge traversal, link parent, link-attached satellite, or provider-specific optimization requirement

The declaration order of `DataVaultPitMetadata.Satellites` is the contract order for read-record satellite segments and typed projection access.

## Request Contract

The public request shape is provider-neutral:

```csharp
public sealed class DataVaultPitAsOfReadRequest {
  public DataVaultPitAsOfReadRequest(
      DataVaultPitMetadata pit,
      IEnumerable<string> parentHashKeys,
      DateTimeOffset asOf);

  public DataVaultPitMetadata Pit { get; }
  public IReadOnlyList<string> ParentHashKeys { get; }
  public DateTimeOffset AsOf { get; }
}
```

Constructor rules:

- `pit` is required.
- `parentHashKeys` is required, deduplicated with `StringComparer.Ordinal`, and rejects null, empty, or whitespace values.
- `asOf` is normalized with `ToUniversalTime()` and remains a caller-facing `DateTimeOffset`.

For each requested parent hash key, the service resolves the latest PIT row whose PIT row `LoadTimestamp` is visible at or before `AsOf`. A missing PIT row yields no projected record for that parent.

## Read-Service Contract

The interface-level raw row method is:

```csharp
public interface IDataVaultReadService {
  Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request,
      CancellationToken cancellationToken = default);
}
```

The typed projector helper mirrors the latest/as-of satellite projection pattern:

```csharp
public static Task<IReadOnlyList<TProjection>> ReadPitAsync<TProjection>(
    this IDataVaultReadService readService,
    DbContext dbContext,
    DataVaultPitAsOfReadRequest request,
    Func<DataVaultPitProjectionRow, TProjection> projector,
    CancellationToken cancellationToken = default);
```

`ReadPitRowsAsync(...)` is the raw escape hatch. `ReadPitAsync(...)` maps selected rows through caller-owned projector delegates and does not use reflection-based DTO binding.

## Raw Record Shape

The raw record exposes the matched PIT row and satellite snapshots in declaration order:

```csharp
public sealed class DataVaultPitReadRecord {
  public string ParentHashKey { get; }
  public DateTimeOffset LoadTimestamp { get; }
  public IReadOnlyList<DataVaultPitSatelliteSnapshot> SatelliteSnapshots { get; }
  public IReadOnlyDictionary<string, DataVaultPitSatelliteSnapshot> SatelliteSnapshotsByName { get; }
}

public sealed class DataVaultPitSatelliteSnapshot {
  public string SatelliteName { get; }
  public int Ordinal { get; }
  public bool IsPresent { get; }
  public DateTimeOffset? SnapshotLoadTimestamp { get; }
  public string? HashDiff { get; }
  public string? RecordSource { get; }
  public IReadOnlyDictionary<string, string?> PayloadValues { get; }
}
```

Record rules:

- `ParentHashKey` is the requested hub hash key that matched a PIT row.
- `LoadTimestamp` is the PIT row load timestamp normalized to UTC.
- `SatelliteSnapshots` preserves `DataVaultPitMetadata.Satellites` order.
- `SatelliteSnapshotsByName` uses the declared satellite name with `StringComparer.Ordinal`.
- `SnapshotLoadTimestamp` is the satellite row load timestamp referenced by the PIT row.
- `IsPresent` is false when the PIT row has no snapshot value for the satellite or the snapshot cannot materialize a satellite segment.
- Absent satellite segments keep `IsPresent == false`, `SnapshotLoadTimestamp == null`, `HashDiff == null`, `RecordSource == null`, and an empty payload dictionary.
- Absent satellite segments never trigger a non-PIT latest/as-of fallback read.

The typed projection row uses exact names like the latest satellite projection row. Technical values include `ParentHashKey` and `LoadTimestamp`; satellite payload values are scoped behind the declared satellite name so multi-satellite projectors remain deterministic.

## Multi-Satellite Typed Projection Example

```csharp
var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
var profile = new DataVaultSatelliteMetadata(
    "Profile",
    customer.ToReference(),
    ["Customer Name", "Customer Tier"]);
var status = new DataVaultSatelliteMetadata(
    "Status",
    customer.ToReference(),
    ["Status Code"]);
var customerPit = new DataVaultPitMetadata(
    customer.ToReference(),
    ["Profile", "Status"]);

var snapshots = await readService.ReadPitAsync(
    context,
    new DataVaultPitAsOfReadRequest(
        customerPit,
        [aliceCustomerHashKey, bobCustomerHashKey],
        new DateTimeOffset(2026, 5, 11, 12, 0, 0, TimeSpan.Zero)),
    row => {
      var profileSnapshot = row.RequiredSatellite("Profile");
      var statusSnapshot = row.OptionalSatellite("Status");

      return new CustomerSnapshotRead(
          row.RequiredString("ParentHashKey"),
          row.RequiredDateTimeOffset("LoadTimestamp"),
          profileSnapshot.RequiredString("Customer Name"),
          profileSnapshot.RequiredString("Customer Tier"),
          statusSnapshot?.NullableString("Status Code"));
    },
    cancellationToken);

public sealed record CustomerSnapshotRead(
    string ParentHashKey,
    DateTimeOffset LoadTimestamp,
    string CustomerName,
    string CustomerTier,
    string? StatusCode);
```

Expected behavior:

- `Profile` is read before `Status` because the PIT declaration orders satellites that way.
- `row.RequiredDateTimeOffset("LoadTimestamp")` returns the PIT row load timestamp as `DateTimeOffset`.
- Provider storage modes such as ISO 8601 text or UTC ticks do not change the caller-facing API.
- If a matched PIT row has no `Status` snapshot value, `OptionalSatellite("Status")` is absent and `StatusCode` is null in the typed projection.

## Missing PIT Row Example

Request:

```csharp
new DataVaultPitAsOfReadRequest(
    customerPit,
    ["customer-hash-001", "customer-hash-missing"],
    new DateTimeOffset(2026, 5, 11, 12, 0, 0, TimeSpan.Zero));
```

PIT table state:

| Parent hash key | PIT row LoadTimestamp | Profile snapshot | Status snapshot |
| --- | --- | --- | --- |
| `customer-hash-001` | `2026-05-11T10:00:00Z` | `2026-05-11T09:58:00Z` | `2026-05-11T09:59:00Z` |
| `customer-hash-missing` | none | none | none |

Expected result:

- one `DataVaultPitReadRecord` for `customer-hash-001`
- no placeholder, null record, or exception for `customer-hash-missing`

## Missing Satellite Snapshot Example

If the matched PIT row exists but the `Status` snapshot value is absent, the result still contains the parent row:

```text
ParentHashKey: customer-hash-001
LoadTimestamp: 2026-05-11T10:00:00Z
SatelliteSnapshots:
  [0] Profile IsPresent=true SnapshotLoadTimestamp=2026-05-11T09:58:00Z
  [1] Status  IsPresent=false SnapshotLoadTimestamp=null
```

The `Status` segment is absent. The service must not read the latest non-PIT `Status` row to fill the gap.

## Deterministic Diagnostics

Unsupported or inconsistent metadata fails before row materialization with deterministic diagnostics. The diagnostic must name the requested PIT declaration and the unsupported shape.

Required v1 diagnostic cases:

| Case | Diagnostic expectation |
| --- | --- |
| `DataVaultPitMetadata.Parent.Kind != Hub` | reject link-based PIT parents and non-hub parents |
| satellite reference marked multi-active | reject multi-active satellite references |
| satellite attached to a link | reject link-attached PIT satellites |
| satellite attached to a different hub | reject inconsistent parent/satellite shape |
| bridge-driven read request | reject bridge traversal reads as outside the PIT baseline |
| `DataVaultPointInTimeMetadata` request | reject legacy point-in-time modeling as outside this contract |
| request outside one declared `DataVaultPitMetadata` | reject reflection DTO binding or ad hoc table-name reads |

These diagnostics are separate from missing data behavior. Missing PIT rows and missing satellite snapshots are normal read results, not metadata diagnostics.

## Fixture Expectations

The approved fixture for this contract is `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/Contracts/PitBackedAsOfReadContract.approved.txt`.

The fixture captures:

- request shape and `DateTimeOffset` timestamp behavior
- raw PIT record and per-satellite snapshot shape
- declaration-order preservation for multiple satellites
- missing PIT row omission
- missing satellite snapshot absence
- unsupported multi-active, bridge, link-based, and legacy `PointInTime` diagnostics

Downstream implementation tickets should update implementation code to satisfy the fixture. This ticket does not add runtime PIT query behavior.
