[gicket-bot] PO refinement contract

Summary
- Refined the story to ratify the existing read-strategy/read-shape evidence surface, keep SQLite as the only repository-proven optimized read baseline, and record the queued stale-blocker cleanup.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The contract should treat the existing `DataVaultDiagnosticsResult.ReadStrategy` plus `ReadShape.provider` surface as the authoritative provider-read evidence boundary for PIT and bridge candidate strategies rather than introducing a parallel diagnostics contract.
- SQLite remains the only repository-proven optimized latest-satellite, PIT, and bridge read provider baseline; MySql, Oracle, Postgres, and SqlServer stay candidate-only until explicit non-SQLite benchmark evidence exists.
- The repository already fixes the bounded vocabularies this ticket should reuse: read-strategy statuses `NotEvaluated`, `ProviderStrategySelected`, and `ProviderNeutralFallback`; read-shape kinds `LatestSatellite`, `PitAsOf`, and `Bridge`; activity/read-mode terms `Current`, `AsOf`, and `Traversal`.

Scope In
- Define the provider-specific PIT and bridge read-strategy evidence contract for candidate providers beyond SQLite, including eligibility gates and finite fallback causes.
- Define which provider-read facts must appear in diagnostics/support-bundle evidence, including provider name, selected strategy name when present, profile/defaulting facts, status, and fallback causes.
- Define the benchmark/reporting expectation for PIT and bridge provider-strategy claims using the root benchmark artifact triplet and the current documented run-context rules.
- Document bounded non-goals so the story stays on diagnostics/evidence-contract scope instead of widening into runtime feature work.

Scope Out
- Implementing non-SQLite optimized PIT or bridge reads.
- Changing the existing provider-neutral PIT-backed read request/row contracts or adding a new public read API.
- Exposing raw SQL, provider physical plans, automatic index advice, or automatic PIT/bridge maintenance.
- Benchmark reruns, package publication, release automation, or support-bundle transport automation.

Open questions
- none

Follow-up questions
- Which non-SQLite provider package should be the first target for an optimized PIT/bridge read prototype after this evidence contract is accepted?
- When non-SQLite candidate implementations exist, should the benchmark artifact schema grow explicit selected-strategy/fallback columns or keep that detail only in companion diagnostics output?

Risks
- Until queued mutation `mutation-bd0022c7b2e86e2d` replays on `ticket/06F8KZHAB717MJJNAWWK7S0A5W-task-update-v0-27-0-analyzer-and-ef-lifecycle-do`, some views may still show the stale blocker temporarily.
- If future provider work adds new fallback causes, status values, or benchmark-schema fields without coordinated contract updates, diagnostics/support-bundle consumers and typed-helper expectations can drift.
- Non-SQLite benchmark proof depends on provider-specific environment availability, so evidence may lag implementation even when the contract is settled.

Split recommendations
- No immediate ticket split is required; this ticket remains a bounded contract-definition story.
- If downstream implementation work is created later, split by provider package and keep benchmark/verifier evidence in separate follow-up tickets so runtime work does not blur the contract baseline.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 3
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment