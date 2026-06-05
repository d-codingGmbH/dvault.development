<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket as a bounded typed-helper generator transition-test story with no split or description update required; queued stale blocker cleanup is the only materialized planning action from this run.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository baseline already fixes the v1 helper contract: typed helpers consume exactly one authoritative `dvault.support-bundle.v1`, satellite helpers remain in scope, and PIT/bridge helpers require reviewed request-bound `readShape` evidence from the support bundle.
- For this ticket, schema-version or other incompatible support-bundle input stays on the existing `DMV1960` source-boundary path, while metadata-source fingerprint drift stays on `DMV1961`.
- Documented skip behavior is bounded: unsupported PIT or bridge facts skip only the affected helper and preserve other supported satellite, PIT, or bridge helpers from the same bundle.
- A stale incoming `blocks` relation from `06F8KZPN02NWFGMRC2Q1PKYKDR` was already targeted for cleanup; the removal is queued for replay on that ticket's owner branch, so the latest live relation read still shows it until replay.

### Scope In
- Add analyzer/source-generator regression tests for support-bundle freshness transitions across successive runs of the typed read-model generator.
- Cover satellite, PIT, and bridge helper generation/removal when authoritative support-bundle evidence is refreshed, becomes stale, or becomes incompatible.
- Assert generator behavior for fingerprint mismatch (`DMV1961`) and schema-version or other incompatible support-bundle input (`DMV1960`).
- Assert partial-generation skip behavior so unsupported PIT/bridge facts suppress only the affected helper while other supported helpers continue to generate.

### Scope Out
- Changing the typed helper public contract, naming pattern, or helper method signatures documented for v1.
- Adding new runtime read semantics, PIT/bridge maintenance behavior, or provider-specific SQL/read-strategy features.
- Changing support-bundle export workflows, raw `dvault.model.v1` parsing, or broader metadata/import architecture.
- General analyzer diagnostic redesign outside the existing `DMV1960`/`DMV1961` boundary for these transition scenarios.

## Acceptance Criteria
- The existing typed read-model generator test suite includes transition scenarios where a valid support bundle changes between generator runs and the resulting satellite, PIT, and bridge helper outputs update to match the newest authoritative bundle instead of retaining stale generated members.
- A transition from valid helper-generating input to fingerprint-mismatched input reports `DMV1961` and removes or suppresses previously generated typed helpers as required by the current generator contract.
- A transition from valid helper-generating input to schema-version-mismatched or otherwise incompatible support-bundle input reports `DMV1960` and does not leave stale helper output behind.
- A transition that makes one PIT or bridge helper unsupported verifies the documented skip boundary: the affected helper is skipped or removed with the expected diagnostic while other supported helpers from the same bundle remain generated.
- At least one transition scenario verifies recovery in the opposite direction, showing that refreshed authoritative bundle evidence restores the expected helper output after a prior stale or incompatible state.

## Definition of Done
- Deterministic analyzer/source-generator tests are added under the existing typed read-model generator test area and cover both degradation and recovery transitions.
- The tests assert generated source presence or absence and diagnostic ids at the contract boundary rather than relying on implementation-only side effects.
- No new PO clarification is required because the repository documents already define the authoritative helper, freshness, fingerprint, and skip-behavior boundaries for v1.

## Implementation Notes
- Use the existing `DataVaultTypedReadModelSourceGeneratorTests` coverage area as the implementation home rather than introducing a separate test harness.
- Model each scenario as successive generator inputs so the assertions prove stale outputs are cleared when the authoritative support bundle changes.
- Use repository-documented helper categories and diagnostics as the baseline: satellite helpers from the v0.22.0 contract, PIT/bridge helpers from the v1 typed-helper contract, `DMV1960` for incompatible support-bundle input, and `DMV1961` for metadata-source fingerprint drift.
- Keep the partial-skip assertions narrow to the documented boundary that only unsupported PIT/bridge helpers are skipped while unrelated supported helpers continue to emit.
- Relation cleanup was materialized during refinement as a queued `remove-relation` mutation for `06F8KZPN02NWFGMRC2Q1PKYKDR --blocks--> 06F8KZPZZE8VZEBANP5MPN8HH8`; no child tickets, description updates, attachments, or planning documents were created.

## Open Questions
- none

## Follow-Up Questions
- After the queued stale-blocker replay completes on `06F8KZPN02NWFGMRC2Q1PKYKDR`, confirm that shared board or reporting views no longer surface the old incoming blocker edge.

## Risks
- The stale incoming `blocks` relation from `06F8KZPN02NWFGMRC2Q1PKYKDR` remains visible in the latest live relation read until the queued replay runs on that ticket's owner branch.
- Ticket `06F8KZQAWZ7QRGB68KB21C9B0R` remains blocked by this story until the transition-test coverage is delivered.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add tests for refreshed and stale support-bundle transitions, including satellite, PIT, bridge, fingerprint mismatch, schema-version mismatch, and documented skip behavior.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Added typed helper support-bundle transition tests in `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs`.
- Covered refresh, stale fingerprint (`DMV1961`), incompatible schema (`DMV1960`), recovery, and PIT/bridge partial skip boundaries (`DMV1963`/`DMV1964`).

Verification
- `dotnet test tests/DCoding.Data.DVault.Tests/Analyzers/DCoding.Data.DVault.Tests.Analyzers.csproj --nologo --no-restore --filter FullyQualifiedName~DataVaultTypedReadModelSourceGeneratorTests` passed.
- `dotnet build DVault.slnx --nologo --no-restore` passed.
- `dotnet test DVault.slnx --nologo --no-restore` passed; external provider integration tests without configured connection strings were skipped.
- `bash tools/check-format.sh` passed.
<!-- gicket-bot:developer-delivery:v1:end -->