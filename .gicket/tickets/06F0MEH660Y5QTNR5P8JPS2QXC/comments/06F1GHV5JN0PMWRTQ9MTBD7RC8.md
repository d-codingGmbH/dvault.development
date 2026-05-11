[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F0MEH660Y5QTNR5P8JPS2QXC/description.md:1-69` contains the persisted delivery contract with explicit Scope In/Out, 6 acceptance criteria, 4 definition-of-done items, 5 implementation notes, and `## Open Questions` = `none`.
- `.gicket/tickets/06F0MEH660Y5QTNR5P8JPS2QXC/comments/06F1GF30K43RFNGYRAJSNCMS4W.md:6-22` records PO handoff decision `ready_for_po_critic` and ratifies `docs/plans/06F0MEGYHADPVN575H64D56W2G-pit-backed-as-of-read-api-contract.md` as the source of truth for the public PIT read surface.
- `.gicket/tickets/06F0MEGYHADPVN575H64D56W2G/ticket.json:3-20` shows the contract-defining ticket is already `done`.
- `docs/plans/06F0MEGYHADPVN575H64D56W2G-pit-backed-as-of-read-api-contract.md:27-226` defines the supported PIT v1 shape, `DataVaultPitAsOfReadRequest`, `IDataVaultReadService.ReadPitRowsAsync(...)`, `ReadPitAsync<TProjection>(...)`, missing-PIT-row behavior, missing-snapshot behavior, and deterministic diagnostics.
- `src/DCoding.Data.DVault/IDataVaultReadService.cs:8-19` confirms the current public read boundary still only exposes `ReadLatestSatelliteRowsAsync(...)`, so the ticket's additive PIT API change is concrete and testable.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:545-564,899-944`, `Modeling/DataVaultMetadataModel.cs:68-160`, and `Modeling/DataVaultMetadataRegistry.cs:239-251` already provide `DataVaultPitSatelliteReferenceMetadata`, `DataVaultPitMetadata`, PIT slots in the metadata model, and PIT registry lookup.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:402-447` already projects PIT entities with `DataVaultTableKind.Pit`, `SnapshotReference` properties, and a PK over parent hash key + PIT load timestamp, which matches the provider-neutral read-service baseline.
- `docs/releases/v0.6.0.md:39-47` still states `IDataVaultReadService` is limited to latest/as-of satellite rows and that PIT-backed read APIs are not delivered in v0.6.0, matching the ticket's DoD to update public API/docs when implementation lands.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract examples do not pin behavior when the wider model contains same-named satellites under different parents. `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataRegistryTests.cs:77-90` treats parent-scoped duplicate satellite names as valid, while `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:498-508` currently resolves PIT satellites by global name before parent checks.
- The PIT contract does not include an explicit empty-`parentHashKeys` example. Current latest/as-of request normalization allows empty-after-dedupe input in `src/DCoding.Data.DVault/DataVaultLatestSatelliteReadRequest.cs:53-62`.

Risky assumptions
- Assumes PIT v1 can intentionally rely on the existing generated-entity and translator baseline for supported shapes without adding a new PO-level clarification for same-named satellites under different parents.
- Assumes the public PIT read surface is intentionally limited to explicit metadata requests plus the typed projector helper, even though latest and bridge reads already expose registry-backed adapters in `src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs:26-172`.

AC / test suggestions
- Add a PIT coverage case for same-named satellites on different hubs so support vs. deterministic rejection is explicit.
- Add an empty-request test to pin whether PIT reads short-circuit to `[]` after parent-hash-key dedupe.
- Update the public API snapshot together with the contract fixture so the new `IDataVaultReadService` method and PIT row types cannot drift from `docs/plans/06F0MEGYHADPVN575H64D56W2G-pit-backed-as-of-read-api-contract.md`.
- Include malformed generated PIT-entity tests for missing or wrong `EntityKind`, `MetadataName`, or `SnapshotReference` annotations because the implementation notes depend on those bindings.

Implementation watchouts
- Adding `ReadPitRowsAsync` changes the public interface; `DefaultDataVaultReadService`, DI wiring, and PublicApi snapshots need to move together without regressing latest/as-of reads.
- Keep missing PIT rows and missing satellite snapshots as normal read outcomes, not diagnostics and not fallback latest-satellite reads.
- Reuse existing provider-neutral timestamp conversion and annotation-based column discovery instead of reconstructing PIT names independently.
- Be careful with satellite resolution when duplicate logical satellite names exist elsewhere in the model; the registry is parent-scoped but `ResolvePitSatellites(...)` currently matches PIT satellites by global name first.

Non-blocking notes
- The ticket is already well-bounded to one hub-parent PIT read shape and explicitly excludes bridge traversal, link-parent PITs, link-attached satellites, multi-active shapes, PIT maintenance, and provider-specific optimizations.
- Release-note and README follow-up is already acknowledged by the current DoD and risk text, so doc work can stay coupled to implementation rather than blocking developer handoff now.

Split recommendations
- No split recommended; the contract is already narrow and the authoritative planning ticket `06F0MEGYHADPVN575H64D56W2G` is done.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment