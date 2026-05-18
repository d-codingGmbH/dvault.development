# PIT Maintenance Service V1 Contract

Status: v1 planning contract
Primary ticket: 06F2PGPBRFT48JG57SV57N9TVW
Related tickets: 06F2PGPKXWRFXNPFA1JR0X67XC, 06F2PGPRGN0EVGD6RY5KY9M56W, 06F2PGPXVAYRBC94RQ7X5V4DVG
Baseline references: `README.md`, `docs/plans/deferred-data-vault-capabilities.md`, `docs/plans/pit-backed-as-of-read-api-contract.md`

## Purpose

Define the bounded v1 contract for explicit PIT maintenance so PIT-backed read APIs can rely on one deterministic row-population baseline without introducing provider-specific physical behavior or reopening PIT modeling scope.

## Scope

- Add one explicit PIT maintenance service in `DCoding.Data.DVault` for already projected `DataVaultPitMetadata` tables.
- Support full rebuild for one PIT declaration and bounded incremental maintenance for explicit parent hash keys.
- Keep the baseline provider-neutral and compatible with the existing PIT translation and PIT-backed read contracts.
- Preserve the existing explicit caller-owned workflow. PIT maintenance stays opt-in and is not triggered by `SaveChanges`, interceptors, background jobs, or package-install side effects.

## Supported v1 shape

- `DataVaultPitMetadata` only.
- Parent must resolve to one hub.
- Participating satellites must already be supported by PIT translation: hub-attached, non-multi-active, and unique in declaration order.
- The generated PIT entity keeps the existing `ParentHashKey`, `LoadTimestamp`, and `<Satellite>LoadTimestamp` column contract.

Legacy `DataVaultPointInTimeMetadata`, `DataVaultModelBuilder.PointInTime(...)`, link-based PITs, multi-active PITs, bridge coordination, and provider-specific maintenance optimization are out of scope for this ticket.

## Authoritative row-generation rule

For one PIT and one parent hash key:

1. Collect every visible satellite row from the PIT's declared satellites for that parent.
2. Build the ordered set of distinct satellite `LoadTimestamp` values across those rows.
3. Materialize one PIT row for each distinct timestamp in ascending order.
4. Set the PIT row `LoadTimestamp` to that distinct timestamp.
5. For each declared satellite, set the snapshot column to the latest satellite `LoadTimestamp` at or before the PIT row `LoadTimestamp`, or null when no satellite row is yet visible.

This v1 rule makes rebuild and bounded maintenance deterministic and gives `IDataVaultReadService.ReadPitRowsAsync(...)` a stable historical baseline.

## Maintenance behavior

- Full rebuild recomputes the complete PIT contents for one declared PIT and replaces any existing rows for that PIT table.
- Incremental maintenance accepts explicit parent hash keys and recomputes the complete PIT history for only those parents.
- Incremental maintenance replaces existing PIT rows for the targeted parents before inserting the recomputed rows so late-arriving satellite rows can correct prior PIT history for those parents.
- Empty parent-hash-key inputs are allowed as a no-op for bounded maintenance.
- Parent hash keys use `StringComparer.Ordinal` and reject null, empty, or whitespace values.

## Service boundary

- The maintenance surface stays separate from `IDataVaultReadService` and `IDataVaultSaveService`.
- The service is registered by the core `AddDVault(...)` path beside the existing explicit save/read services.
- Requests use explicit `DataVaultPitMetadata` references rather than inventing a new PIT declaration path.
- The implementation may add request/result types and registry-backed convenience adapters if they stay additive to the explicit service boundary.

## Diagnostics and verification

- Fail before writes when the PIT metadata, generated PIT entity, or participating satellite metadata falls outside the supported PIT baseline.
- Reuse existing produced names, provider capability mappings, and UTC timestamp behavior from the translated PIT and satellite entities.
- Add unit coverage for unsupported shapes and row-generation semantics.
- Add SQLite integration coverage for full rebuild, bounded parent maintenance, missing satellite segments, and late-arriving satellite corrections.
- Update README and the v0.15.0 release-notes task language so PIT-backed reads are documented as consuming explicitly maintained PIT tables.

## Cross-ticket boundaries

- Ticket `06F2PGPKXWRFXNPFA1JR0X67XC` can assume maintained PIT tables exist and should not redefine PIT row-population semantics.
- Ticket `06F2PGPRGN0EVGD6RY5KY9M56W` may optimize reads over maintained PIT tables but does not own PIT maintenance behavior.
- Ticket `06F2PGPXVAYRBC94RQ7X5V4DVG` owns the user-facing doc and release-note follow-through for this contract.
- Provider-specific maintenance strategies, hosted orchestration, link-parent PIT maintenance, and multi-active PIT maintenance remain future work.
