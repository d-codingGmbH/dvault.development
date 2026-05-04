[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the existing external opt-in PostgreSQL test harness and the already-proven SQLite save semantics; no blocking PO questions remain.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The opt-in live PostgreSQL contract is already established in the repository: `DVAULT_TEST_POSTGRES_CONNECTION_STRING` enables the tests and the existing configuration helper provides the skip reason when it is absent.
- Default local validation already has a bounded no-Postgres path through the conditional Npgsql package reference and reflection-based provider loader in the integration test project; this ticket should preserve that pattern rather than introduce a mandatory local dependency.
- For this ticket, `optimized strategy` means the PostgreSQL-specific provider path owned by the parent PostgreSQL optimization story, not the current provider-neutral fallback path and not a rewrite of dispatch semantics.
- The behavioral baseline is already defined by the existing SQLite explicit-save coverage: insert-only writes, unchanged satellite hash-diff suppression, and changed satellite append-only insertion.

Scope In
- Add opt-in PostgreSQL integration tests in the existing integration test suite for the PostgreSQL optimized save path.
- Verify representative hub, link, and satellite save behavior through PostgreSQL when the optimized provider path is available.
- Assert unchanged satellite replays do not insert a new row and changed satellite hash diffs do insert a new history row.
- Reuse the existing PostgreSQL configuration, skip, and `ProviderIntegration.ExternalOptIn` test conventions so unattended default runs stay unaffected.

Scope Out
- Implementing the PostgreSQL optimized strategy itself beyond any minimal test-only wiring needed to exercise it.
- Benchmark work, performance claims, or benchmark-result publication.
- Docker, container, or database provisioning automation for PostgreSQL environments.
- Required-local PostgreSQL validation or any change that makes the default local suite depend on PostgreSQL.
- Reopening provider-neutral fallback-selection or unknown-provider dispatch coverage already handled by the existing strategy-selection tests.

Open questions
- none

Follow-up questions
- After the behavior suite lands, should the parent PostgreSQL optimization story also require benchmark evidence or documented smoke results from the same opt-in environment?
- Should later non-SQLite provider optimization stories reuse this same external opt-in integration harness pattern once their strategies exist?

Risks
- Current repository evidence shows the PostgreSQL provider package is still compatibility-only today; if the optimized path is not available in the same workstream, these tests cannot become green yet.
- Live PostgreSQL validation depends on externally supplied connectivity and clean isolation, so tests must create deterministic per-run schema or data boundaries to avoid flaky results.
- If the coverage starts asserting provider-specific SQL text instead of persisted behavior, it will become brittle without materially improving the product contract.

Split recommendations
- No split recommended; keep this ticket limited to opt-in PostgreSQL integration coverage and leave provider implementation, benchmarks, and broader provider rollout concerns to the parent story or separate follow-up tickets.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment