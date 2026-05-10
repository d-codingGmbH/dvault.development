<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to pin a delegate-based v1 typed-read contract on the existing latest/as-of satellite read path, including exact required/nullability behavior and reserved-name collision rules; no child tickets, relation edits, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Baseline public caller surface is a manual projector delegate over a new exact-name row accessor type, not reflection, DTO inference, or a second read-service interface. The helper lives as companion extensions beside the current raw `IDataVaultReadService` surface.
- The same typed helper contract must support both `DataVaultLatestSatelliteReadRequest` and `DataVaultRegistryLatestSatelliteReadRequest`; the registry-backed overload resolves metadata once and then delegates to the same typed projection pipeline, mirroring the current raw registry extension.
- The row accessor exact-name space contains only `ParentHashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, declared driving-key names, and declared payload names, all compared by `StringComparer.Ordinal`.
- Required versus nullable is explicit in the mapping call site: missing mapped names always fail, `NullableString(...)` permits only present-null values, and caller-owned parsing beyond raw strings plus normalized `LoadTimestamp` stays outside v1 helper diagnostics.
- For v1 typed reads, satellites whose payload or driving-key names equal `ParentHashKey`, `HashDiff`, `LoadTimestamp`, or `RecordSource` by `StringComparer.Ordinal` are rejected up front for typed projection rather than receiving precedence or aliases.
- No child tickets, relation edits, attachments, or planning documents were created in this refinement pass.

### Scope In
- Add companion typed read extensions that return `IReadOnlyList<TProjection>` from existing latest/as-of request objects using a projector delegate.
- Support both explicit-metadata and registry-backed latest/as-of request entry points on the existing provider-neutral read-service path.
- Expose exact-name access to technical satellite fields, declared driving keys, and declared payload names inside the projector row contract.
- Add deterministic projection diagnostics and preflight reserved-name validation, with regression coverage for ordinary and multi-active satellites and load-timestamp storage modes.

### Scope Out
- Any reflection-discovered DTO binding, attribute-based projection, source generation, or automatic CLR-to-metadata inference.
- Any new second read engine, widened `IDataVaultReadService` interface requirement, or alternate query-provider surface.
- Automatic scalar conversion beyond raw string access plus normalized `LoadTimestamp`; caller-owned parsing inside the projector delegate remains allowed but out of helper diagnostics.
- Alias or precedence rules for technical-name collisions; v1 rejects those collisions instead of supporting aliasing.
- README and release-note happy-path rewrites beyond code-facing API docs and examples, which remain on ticket 06F0MEDJC732GDD77H60R259P0.

## Acceptance Criteria
- Callers can use `ReadLatestSatelliteAsync<TProjection>(DbContext, DataVaultLatestSatelliteReadRequest, Func<DataVaultSatelliteProjectionRow,TProjection>, CancellationToken)` and the matching overload for `DataVaultRegistryLatestSatelliteReadRequest`, while the existing raw `ReadLatestSatelliteRowsAsync` API remains source-compatible.
- The same projector delegate contract works for both explicit-metadata and registry-backed request paths, and the registry-backed overload resolves metadata once then reuses the same typed projection pipeline.
- Inside `DataVaultSatelliteProjectionRow`, exact-name access supports `ParentHashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, declared driving-key names, and declared payload names using `StringComparer.Ordinal`, while preserving current latest/as-of selection and multi-active series semantics.
- Required versus nullable behavior is explicit at the mapping call site: `RequiredString(...)` and `RequiredDateTimeOffset("LoadTimestamp")` fail on missing, null, or invalid values; `NullableString(...)` returns `null` only for an existing mapped name whose provider value is null; a missing mapped name always fails.
- Projection failures throw `InvalidOperationException` with deterministic prefix `DVault typed satellite projection failed ({failureKind})` and include the satellite metadata name and offending mapped name, where v1 `failureKind` tokens are `missing-name`, `null-value`, or `invalid-value`.
- Before any row materialization, the typed helper rejects satellites whose payload or driving-key names equal `ParentHashKey`, `HashDiff`, `LoadTimestamp`, or `RecordSource` by `StringComparer.Ordinal`.
- Automated tests cover explicit and registry-backed latest/as-of parity, ordinary and multi-active projections, required-versus-nullable behavior, reserved-name rejection, and `LoadTimestamp` normalization across provider-default, ISO 8601 UTC text, and UTC-ticks storage.

## Definition of Done
- Public API surface, XML docs, and snapshot coverage include the typed helper overloads, `DataVaultSatelliteProjectionRow`, the required/nullability accessors, and one visible registry-backed example plus the explicit-metadata variant.
- Implementation remains additive to the current raw read service and reuses existing batching, satellite resolution, latest/as-of series selection, and timestamp normalization logic instead of introducing a second provider-neutral read engine.
- The typed path validates reserved technical-name collisions before query execution and never relies on the current silent-skip behavior in `DefaultDataVaultReadService` for required/null diagnostics.
- Tests prove parity for hub-parent, link-parent, ordinary, and multi-active satellite reads and prove the typed `LoadTimestamp` accessor returns the same UTC values across supported storage modes.
- Existing raw `DataVaultSatelliteReadRecord` reads remain available and source-compatible as the advanced escape hatch.

## Implementation Notes
- Concrete v1 example: `public sealed record CustomerContactRead(string ParentHashKey, string HashDiff, DateTimeOffset LoadTimestamp, string RecordSource, string ContactType, string? EmailAddress); var rows = await readService.ReadLatestSatelliteAsync(context, new DataVaultRegistryLatestSatelliteReadRequest(DataVaultMetadataReference.Hub("Customer"), "Contact", [customerHashKey], cutoffUtc), row => new CustomerContactRead(row.RequiredString("ParentHashKey"), row.RequiredString("HashDiff"), row.RequiredDateTimeOffset("LoadTimestamp"), row.RequiredString("RecordSource"), row.RequiredString("ContactType"), row.NullableString("EmailAddress"))));` The explicit-metadata overload uses the same projector delegate with `new DataVaultLatestSatelliteReadRequest(contactSatellite, [customerHashKey], cutoffUtc)`.
- Keep the helper as companion extensions rather than new members on `IDataVaultReadService`, matching the current explicit-vs-registry split where registry helpers resolve metadata and then delegate to the explicit request path.
- Implement projection against a pre-silent-drop row/view that can distinguish missing names, null provider values, invalid timestamp storage values, and valid strings; do not map from already-materialized `DataVaultSatelliteReadRecord` alone.
- Keep projection names exact and ordinal-sensitive; do not introduce case-insensitive matching, convention aliases, or collision precedence. The typed helper owns the additional reservation check for `ParentHashKey`, `HashDiff`, `LoadTimestamp`, and `RecordSource`.
- V1 helper-owned type conversion is limited to normalized `LoadTimestamp`; payload, driving-key, `ParentHashKey`, `HashDiff`, and `RecordSource` are exposed as strings and any further parsing inside the projector delegate is caller-owned.

## Open Questions
- none

## Follow-Up Questions
- After this v1 manual projector contract lands, should PIT-backed typed projections on 06F0MEGYHADPVN575H64D56W2G reuse the same `DataVaultSatelliteProjectionRow` delegate shape?
- Should a later convenience layer add generated or reflection-based binders on top of this manual delegate contract, while keeping the v1 exact-name projector as the stable base?
- Once ticket 06F0MEDJC732GDD77H60R259P0 lands, should README and release docs present the registry-backed typed helper as the happy path and the raw record API as the advanced escape hatch?

## Risks
- If implementation projects only from `DataVaultSatelliteReadRecord`, required/null diagnostics will still disappear behind the current silent-skip behavior.
- If explicit and registry-backed typed overloads diverge instead of sharing one projector pipeline, latest/as-of parity or diagnostic wording can drift.
- If reserved-name validation is omitted, a satellite payload or driving key named `HashDiff`, `LoadTimestamp`, `RecordSource`, or `ParentHashKey` will leave the exact-name contract ambiguous.
- If failureKind tokens or message prefix vary across paths, tests and callers lose the deterministic diagnostic contract this ticket is meant to add.

## Split Recommendations
- No split recommended. Repository evidence still bounds this work to one additive typed-read helper layer, deterministic diagnostics, and tests, and no child tickets or planning documents were materialized in this pass.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Add typed read projections for latest/as-of satellite rows on top of IDataVaultReadService so users can consume DTOs rather than dictionaries or raw records for common reads.

## Scope In

- Projection contract for typed satellite DTOs.
- Latest and as-of reads by parent hash key.
- Tests for missing payload columns, nullable payloads, driving keys, and timestamp storage modes.

## Scope Out

- PIT-backed read models.
- Bridge traversal reads.
- Provider-specific read strategy tuning.

## Acceptance Criteria

- Typed reads reuse the existing provider-neutral read service path.
- Projection failures are deterministic and actionable.
- Existing DataVaultSatelliteReadRecord reads remain available.