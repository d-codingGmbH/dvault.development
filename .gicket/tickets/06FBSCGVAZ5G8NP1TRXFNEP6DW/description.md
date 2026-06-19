<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence already supports a bounded MySQL PIT/bridge closure path: the strategy, fallback gates, tests, and a provider-configured 2026-06-07 smoke-read artifact already exist, so refinement should focus on ratifying that evidence lane and aligning any stale gap wording rather than inventing new read behavior.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- AddDVaultMySql() already registers MySqlDataVaultReadStrategy for PIT and bridge reads, so this is not a strategy-invention ticket.
- The checked-in bundle artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.md already contains completed MySQL pit-as-of-read and bridge-traversal-read rows with selectedStrategy=MySqlDataVaultReadStrategy.
- The root benchmark-summary.* MySQL read rows may remain skipped quick-baseline guidance when DVAULT_TEST_MYSQL_CONNECTION_STRING is unset; closure must distinguish that surface from the provider-configured v0.32 smoke-read bundle.
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs still preserves the root MySQL guidance-row contract, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs already proves MySQL provider-mismatch, unsupported-shape, incomplete-evidence, and stale-maintenance fallback behavior.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement run.

### Scope In
- Ratify the existing MySQL PIT and bridge closure evidence using the checked-in provider-configured smoke-read artifact bundle and aligned repository docs/tests.
- Align authoritative evidence and planning surfaces so MySQL pit-as-of-read and bridge-traversal-read are not treated as unresolved gaps once the accepted artifact lane is cited.
- Preserve the explicit-maintenance, incomplete-read-shape-evidence, and stale-read-model-maintenance fallback boundaries for MySQL PIT and bridge reads.
- Keep the ticket bounded to the existing IDataVaultReadService, AddDVaultMySql(), MySqlDataVaultReadStrategy, benchmark artifacts, and documentation surfaces.

### Scope Out
- MySQL latest-satellite optimization remains out of scope; the repository still records providerSpecificReadStrategy=not registered for latest satellite reads.
- No new public API, new read-shape vocabulary, new strategy family, or provider-specific SQL artifact surface is introduced here.
- No PIT or bridge maintenance behavior change, automatic scheduling, SaveChanges hook, or read-time refresh work is included.
- Do not turn the root skipped quick-baseline rows into a requirement to rerun benchmarks if the existing approved v0.32 artifact bundle is accepted as the closure evidence.

## Acceptance Criteria
- The refined ticket explicitly cites the checked-in MySQL provider-configured evidence at artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.md as the closure surface for pit-as-of-read and bridge-traversal-read, with selectedStrategy=MySqlDataVaultReadStrategy for both rows.
- The ticket keeps root benchmark-summary.* MySQL read rows as optional quick-baseline guidance when connection strings are unset and distinguishes them from the completed provider-configured artifact bundle.
- Repository evidence remains aligned that AddDVaultMySql() registers MySqlDataVaultReadStrategy for PIT and bridge reads and that MySQL latest-satellite stays provider-neutral and out of scope.
- Tests and diagnostics boundaries continue to prove provider mismatch, unsupported shape, incomplete read-shape evidence, and stale read-model maintenance fall back through the existing finite causes rather than widening behavior.
- Any documentation or planning updates produced under this ticket align the evidence matrix, gap wording, and performance guidance with the accepted MySQL artifact lane without claiming universal or always-configured MySQL timing.

## Definition of Done
- Reviewers can close the ticket without reopening whether a MySQL PIT/bridge strategy exists; repository evidence already shows registration, diagnostics gates, and completed provider-configured read rows.
- The authoritative closure text differentiates MySQL PIT/bridge completed artifact evidence from MySQL latest-satellite non-support and from root skipped quick-baseline rows.
- No accepted closure text implies automatic PIT or bridge maintenance, raw SQL guarantees, provider plan promises, or broader MySQL read optimization beyond PIT/bridge.
- If evidence or planning surfaces are updated, they no longer describe MySQL pit-as-of-read or bridge-traversal-read as unresolved solely because the root quick baseline is skipped.

## Implementation Notes
- Use src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs and src/DCoding.Data.DVault.MySql/MySqlDataVaultReadStrategy.cs as the authoritative existing MySQL registration and strategy baseline.
- Use artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.md as the provider-configured artifact surface; its 2026-06-07 MySQL rows show completed PIT and bridge execution with MySqlDataVaultReadStrategy selected.
- Use docs/releases/v0.32.0.md and docs/performance-profiles.md to justify that checked-in v0.32 provider bundles are valid repository evidence while still keeping root quick-baseline rows separate from completed provider timing claims.
- Use tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs for MySQL PIT and bridge gate coverage and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs for the current guidance-row contract that must stay internally consistent.
- The visible inconsistency is documentation-level: docs/plans/provider-optimization-gap-matrix.md and docs/plans/provider-optimization-evidence-matrix.md still frame MySQL PIT and bridge root rows as evidence gaps even though an approved provider-configured bundle already exists.
- No bounded planning writes were necessary during refinement; if later materialized, they should stay limited to ticket or planning text rather than product code.

## Open Questions
- none

## Follow-Up Questions
- Should the same artifact-backed closure pattern be applied next to the PostgreSQL and Oracle PIT/bridge gap tickets if their provider-configured smoke-read rows are accepted on the same basis?
- After MySQL PIT/bridge closure is ratified, should the gap matrix keep only MySQL latest-satellite as the remaining MySQL read follow-up?

## Risks
- If reviewers read only the root benchmark-summary.* files, they may incorrectly treat MySQL PIT/bridge as still open because those quick-baseline rows remain skipped when connection strings are unset.
- The same 2026-06-07 smoke-read bundle also contains a completed MySQL latest-satellite row that still selected provider-neutral fallback; closure text must not misread that as MySQL latest-satellite optimization support.
- If evidence-matrix or gap-matrix wording is left unchanged after closure, the repository will keep contradictory signals about whether MySQL PIT/bridge read evidence is already satisfied.

## Split Recommendations
- No split recommended; the visible repository evidence keeps this as one bounded MySQL closure and evidence-alignment ticket.
- Do not create a child ticket for new MySQL PIT/bridge strategy implementation unless someone first disproves the existing 2026-06-07 provider-configured artifact bundle as acceptable closure evidence.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Produce provider-configured PIT/bridge timing evidence for the existing MySQL strategy candidates already registered by `AddDVaultMySql()` and named in benchmark guidance as `MySqlDataVaultReadStrategy`. Acceptance: checked-in evidence covers the MySQL `pit-as-of-read` and `bridge-traversal-read` rows with configured benchmark artifacts or other approved repository evidence; diagnostics, tests, and fallback behavior continue to enforce explicit maintenance plus incomplete/stale evidence fallback boundaries; the ticket does not widen scope into new public API, new read-shape design, or alternative strategy invention.