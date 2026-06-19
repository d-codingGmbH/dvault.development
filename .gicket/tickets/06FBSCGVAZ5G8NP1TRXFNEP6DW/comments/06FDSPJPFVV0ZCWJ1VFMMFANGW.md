[gicket-bot] PO refinement contract

Summary
- Repository evidence already supports a bounded MySQL PIT/bridge closure path: the strategy, fallback gates, tests, and a provider-configured 2026-06-07 smoke-read artifact already exist, so refinement should focus on ratifying that evidence lane and aligning any stale gap wording rather than inventing new read behavior.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- AddDVaultMySql() already registers MySqlDataVaultReadStrategy for PIT and bridge reads, so this is not a strategy-invention ticket.
- The checked-in bundle artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.md already contains completed MySQL pit-as-of-read and bridge-traversal-read rows with selectedStrategy=MySqlDataVaultReadStrategy.
- The root benchmark-summary.* MySQL read rows may remain skipped quick-baseline guidance when DVAULT_TEST_MYSQL_CONNECTION_STRING is unset; closure must distinguish that surface from the provider-configured v0.32 smoke-read bundle.
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs still preserves the root MySQL guidance-row contract, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs already proves MySQL provider-mismatch, unsupported-shape, incomplete-evidence, and stale-maintenance fallback behavior.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement run.

Scope In
- Ratify the existing MySQL PIT and bridge closure evidence using the checked-in provider-configured smoke-read artifact bundle and aligned repository docs/tests.
- Align authoritative evidence and planning surfaces so MySQL pit-as-of-read and bridge-traversal-read are not treated as unresolved gaps once the accepted artifact lane is cited.
- Preserve the explicit-maintenance, incomplete-read-shape-evidence, and stale-read-model-maintenance fallback boundaries for MySQL PIT and bridge reads.
- Keep the ticket bounded to the existing IDataVaultReadService, AddDVaultMySql(), MySqlDataVaultReadStrategy, benchmark artifacts, and documentation surfaces.

Scope Out
- MySQL latest-satellite optimization remains out of scope; the repository still records providerSpecificReadStrategy=not registered for latest satellite reads.
- No new public API, new read-shape vocabulary, new strategy family, or provider-specific SQL artifact surface is introduced here.
- No PIT or bridge maintenance behavior change, automatic scheduling, SaveChanges hook, or read-time refresh work is included.
- Do not turn the root skipped quick-baseline rows into a requirement to rerun benchmarks if the existing approved v0.32 artifact bundle is accepted as the closure evidence.

Open questions
- none

Follow-up questions
- Should the same artifact-backed closure pattern be applied next to the PostgreSQL and Oracle PIT/bridge gap tickets if their provider-configured smoke-read rows are accepted on the same basis?
- After MySQL PIT/bridge closure is ratified, should the gap matrix keep only MySQL latest-satellite as the remaining MySQL read follow-up?

Risks
- If reviewers read only the root benchmark-summary.* files, they may incorrectly treat MySQL PIT/bridge as still open because those quick-baseline rows remain skipped when connection strings are unset.
- The same 2026-06-07 smoke-read bundle also contains a completed MySQL latest-satellite row that still selected provider-neutral fallback; closure text must not misread that as MySQL latest-satellite optimization support.
- If evidence-matrix or gap-matrix wording is left unchanged after closure, the repository will keep contradictory signals about whether MySQL PIT/bridge read evidence is already satisfied.

Split recommendations
- No split recommended; the visible repository evidence keeps this as one bounded MySQL closure and evidence-alignment ticket.
- Do not create a child ticket for new MySQL PIT/bridge strategy implementation unless someone first disproves the existing 2026-06-07 provider-configured artifact bundle as acceptable closure evidence.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment