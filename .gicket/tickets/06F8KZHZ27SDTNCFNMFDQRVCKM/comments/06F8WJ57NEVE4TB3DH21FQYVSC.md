[gicket-bot] PO-critic review contract

Summary
- Persisted delivery contract is internally consistent, anchored to existing repo contracts and benchmark artifacts, and has no unresolved PO questions; ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F8KZHZ27SDTNCFNMFDQRVCKM/description.md:28-50` contains 6 acceptance criteria plus `## Open Questions` = `none`, so the persisted contract is complete enough for dev handoff.
- `.gicket/tickets/06F8KZHZ27SDTNCFNMFDQRVCKM/description.md:12-20,41-46` explicitly binds the story to existing surfaces: `DataVaultDiagnosticsResult.ReadStrategy`, `ReadShape.provider`, `docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md`, `docs/plans/pit-backed-as-of-read-api-contract.md`, `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md`, and `DataVaultActivityTracing` vocabularies.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:139-199,520-531` already defines the exact read-strategy statuses, finite fallback-cause enum, and `DataVaultReadShapeProviderDiagnostics.SelectedStrategyName` shape that the ticket is ratifying.
- `docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md:11-13,34-70` already documents `ReadStrategy` as the authoritative provider strategy section, `readShape.provider` as the provider-facts surface, and omission of `selectedStrategyName` when no provider-specific strategy is selected.
- `docs/plans/pit-backed-as-of-read-api-contract.md:9,15,23` preserves the existing provider-neutral `IDataVaultReadService` PIT boundary, and `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md:10-14` keeps PIT/bridge typed helpers support-bundle-driven without widening runtime read semantics.
- `docs/releases/v0.26.0.md:39-47` ties performance claims to `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`, requires preserved run context, and states SQLite is the only repository-proven optimized latest-satellite/PIT/bridge read provider path.
- `benchmark-summary.csv:19-23` and `benchmark-summary.md:50-54` show the checked-in latest-satellite, PIT as-of, and bridge traversal read rows only have SQLite optimized variants, with `selectedStrategy=SqliteDataVaultReadStrategy` and `readShapeProviderStatus=ProviderStrategySelected` on the `dvault-adddvaultsqlite-optimized` rows.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Keep one acceptance/example check that fallback omits `selectedStrategyName` in both `readStrategy` and `readShape.provider`, matching `docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md:57-70` and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:<redacted>`.
- Keep one regression check that unsupported link-parent PIT requests surface `UnsupportedPitShape` in both `ReadStrategy` and `ReadShape.provider`, matching `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:250-258`.
- Keep benchmark-verifier expectations tied to the root artifact triplet and preserved run context from `docs/releases/v0.26.0.md:39-47` and `docs/plans/performance-evidence-benchmark-artifact-contract.md:16-33,94`.

Implementation watchouts
- `git diff --name-only develop..ticket/06F8KZHZ27SDTNCFNMFDQRVCKM-story-define-provider-read-strategy-evidence-con` currently lists only `.gicket/tickets/06F8KZHZ27SDTNCFNMFDQRVCKM/**`, so the actual contract/documentation work is still future developer work; that is normal for this pre-development gate and not a PO blocker.
- Keep the implementation bounded to the existing contract surfaces: `ReadStrategy`, `ReadShape.provider`, the existing provider-neutral `IDataVaultReadService` PIT boundary, and support-bundle-driven typed helpers; do not let the story expand into new public runtime APIs or runtime dispatch behavior.

Non-blocking notes
- Current benchmark evidence already supports the ticket's wording that strategy selection evidence must stay distinct from performance claims: `benchmark-summary.csv:20-21` shows the SQLite PIT row selected `SqliteDataVaultReadStrategy` even though its mean time is slightly above the fallback row in that checked-in run.
- PO refinement comment `.gicket/tickets/06F8KZHZ27SDTNCFNMFDQRVCKM/comments/06F8WFZJTC4NQBW6SS8DT0R8T8.md` reports persisted coverage of 6 acceptance-criteria items, 3 definition-of-done items, and 6 implementation notes.

Split recommendations
- No immediate split needed; keep this as the bounded contract-definition story described in `.gicket/tickets/06F8KZHZ27SDTNCFNMFDQRVCKM/description.md:61-63`.
- If downstream implementation tickets are opened later, split by provider package and keep benchmark/verifier evidence in separate follow-up tickets, consistent with `.gicket/tickets/06F8KZHZ27SDTNCFNMFDQRVCKM/description.md:61-63`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment