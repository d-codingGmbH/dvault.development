[gicket-bot] PO refinement contract

Summary
- Refined the ticket to pin a delegate-based v1 typed-read contract on the existing latest/as-of satellite read path, including exact required/nullability behavior and reserved-name collision rules; no child tickets, relation edits, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - V1 now chooses a delegate-based companion helper surface rather than a second read engine or inferred binder: add `ReadLatestSatelliteAsync<TProjection>(DbContext, DataVaultLatestSatelliteReadRequest, Func<DataVaultSatelliteProjectionRow,TProjection>, CancellationToken)` and a matching overload for `DataVaultRegistryLatestSatelliteReadRequest`. Both latest and as-of reads continue to use the existing request objects, and both explicit-metadata and registry-backed callers pass the same projector delegate.
- critic-item-2: `answered` - The manual projection contract expresses required versus nullable per mapped field by accessor choice on `DataVaultSatelliteProjectionRow`: `RequiredString(...)` and `RequiredDateTimeOffset("LoadTimestamp")` require a non-null projectable value, while `NullableString(...)` accepts an explicit database null and returns `null`. A missing mapped name always fails, even for nullable accessors. Failures throw `InvalidOperationException` with stable prefix `DVault typed satellite projection failed ({failureKind})` and always include the satellite name and mapped name; v1 failureKind tokens are `missing-name`, `null-value`, and `invalid-value`.
- critic-item-3: `answered` - V1 resolves technical-name collisions by reservation, not precedence or aliasing. For typed reads, payload or driving-key names equal by `StringComparer.Ordinal` to `ParentHashKey`, `HashDiff`, `LoadTimestamp`, or `RecordSource` are rejected up front before query execution. Existing payload-versus-driving-key overlap rules stay in place, and v1 does not add alias tokens or fallback precedence.
- critic-item-4: `answered` - The public typed-read contract shape is now pinned: companion extension helpers return `IReadOnlyList<TProjection>` from the existing request types plus a manual projector delegate over `DataVaultSatelliteProjectionRow`. A concrete v1 example is a registry-backed call such as `await readService.ReadLatestSatelliteAsync(context, new DataVaultRegistryLatestSatelliteReadRequest(DataVaultMetadataReference.Hub("Customer"), "Contact", [customerHashKey], cutoffUtc), row => new CustomerContactRead(row.RequiredString("ParentHashKey"), row.RequiredString("HashDiff"), row.RequiredDateTimeOffset("LoadTimestamp"), row.RequiredString("RecordSource"), row.RequiredString("ContactType"), row.NullableString("EmailAddress"))))`; the explicit-metadata overload uses the same projector with `DataVaultLatestSatelliteReadRequest`.
- critic-item-5: `answered` - Nullability behavior is now explicit and no longer left to DTO inference: the projector delegate must opt into each field as required or nullable at the call site. Missing mapped names always fail; present-null values succeed only through `NullableString(...)`; required accessors fail for missing, null, or invalid provider values; and `LoadTimestamp` is obtained only through the normalized required DateTimeOffset accessor. This removes the previous ambiguity about where required versus nullable behavior comes from.
- critic-item-6: `answered` - The exact-name collision gap is closed by a fail-fast reservation rule. The typed projection name space contains only `ParentHashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, declared driving-key names, and declared payload names, all matched by `StringComparer.Ordinal`; if any payload or driving-key name collides with one of the four technical tokens, typed projection is unsupported for that satellite until the metadata names are changed. Developers no longer have to invent precedence between technical and logical names.

Clarifications
- Baseline public caller surface is a manual projector delegate over a new exact-name row accessor type, not reflection, DTO inference, or a second read-service interface. The helper lives as companion extensions beside the current raw `IDataVaultReadService` surface.
- The same typed helper contract must support both `DataVaultLatestSatelliteReadRequest` and `DataVaultRegistryLatestSatelliteReadRequest`; the registry-backed overload resolves metadata once and then delegates to the same typed projection pipeline, mirroring the current raw registry extension.
- The row accessor exact-name space contains only `ParentHashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, declared driving-key names, and declared payload names, all compared by `StringComparer.Ordinal`.
- Required versus nullable is explicit in the mapping call site: missing mapped names always fail, `NullableString(...)` permits only present-null values, and caller-owned parsing beyond raw strings plus normalized `LoadTimestamp` stays outside v1 helper diagnostics.
- For v1 typed reads, satellites whose payload or driving-key names equal `ParentHashKey`, `HashDiff`, `LoadTimestamp`, or `RecordSource` by `StringComparer.Ordinal` are rejected up front for typed projection rather than receiving precedence or aliases.
- No child tickets, relation edits, attachments, or planning documents were created in this refinement pass.

Scope In
- Add companion typed read extensions that return `IReadOnlyList<TProjection>` from existing latest/as-of request objects using a projector delegate.
- Support both explicit-metadata and registry-backed latest/as-of request entry points on the existing provider-neutral read-service path.
- Expose exact-name access to technical satellite fields, declared driving keys, and declared payload names inside the projector row contract.
- Add deterministic projection diagnostics and preflight reserved-name validation, with regression coverage for ordinary and multi-active satellites and load-timestamp storage modes.

Scope Out
- Any reflection-discovered DTO binding, attribute-based projection, source generation, or automatic CLR-to-metadata inference.
- Any new second read engine, widened `IDataVaultReadService` interface requirement, or alternate query-provider surface.
- Automatic scalar conversion beyond raw string access plus normalized `LoadTimestamp`; caller-owned parsing inside the projector delegate remains allowed but out of helper diagnostics.
- Alias or precedence rules for technical-name collisions; v1 rejects those collisions instead of supporting aliasing.
- README and release-note happy-path rewrites beyond code-facing API docs and examples, which remain on ticket 06F0MEDJC732GDD77H60R259P0.

Open questions
- none

Follow-up questions
- After this v1 manual projector contract lands, should PIT-backed typed projections on 06F0MEGYHADPVN575H64D56W2G reuse the same `DataVaultSatelliteProjectionRow` delegate shape?
- Should a later convenience layer add generated or reflection-based binders on top of this manual delegate contract, while keeping the v1 exact-name projector as the stable base?
- Once ticket 06F0MEDJC732GDD77H60R259P0 lands, should README and release docs present the registry-backed typed helper as the happy path and the raw record API as the advanced escape hatch?

Risks
- If implementation projects only from `DataVaultSatelliteReadRecord`, required/null diagnostics will still disappear behind the current silent-skip behavior.
- If explicit and registry-backed typed overloads diverge instead of sharing one projector pipeline, latest/as-of parity or diagnostic wording can drift.
- If reserved-name validation is omitted, a satellite payload or driving key named `HashDiff`, `LoadTimestamp`, `RecordSource`, or `ParentHashKey` will leave the exact-name contract ambiguous.
- If failureKind tokens or message prefix vary across paths, tests and callers lose the deterministic diagnostic contract this ticket is meant to add.

Split recommendations
- No split recommended. Repository evidence still bounds this work to one additive typed-read helper layer, deterministic diagnostics, and tests, and no child tickets or planning documents were materialized in this pass.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment