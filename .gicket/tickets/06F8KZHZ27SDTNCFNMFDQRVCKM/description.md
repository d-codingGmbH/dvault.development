<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story to ratify the existing read-strategy/read-shape evidence surface, keep SQLite as the only repository-proven optimized read baseline, and record the queued stale-blocker cleanup.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The contract should treat the existing `DataVaultDiagnosticsResult.ReadStrategy` plus `ReadShape.provider` surface as the authoritative provider-read evidence boundary for PIT and bridge candidate strategies rather than introducing a parallel diagnostics contract.
- SQLite remains the only repository-proven optimized latest-satellite, PIT, and bridge read provider baseline; MySql, Oracle, Postgres, and SqlServer stay candidate-only until explicit non-SQLite benchmark evidence exists.
- The repository already fixes the bounded vocabularies this ticket should reuse: read-strategy statuses `NotEvaluated`, `ProviderStrategySelected`, and `ProviderNeutralFallback`; read-shape kinds `LatestSatellite`, `PitAsOf`, and `Bridge`; activity/read-mode terms `Current`, `AsOf`, and `Traversal`.

### Scope In
- Define the provider-specific PIT and bridge read-strategy evidence contract for candidate providers beyond SQLite, including eligibility gates and finite fallback causes.
- Define which provider-read facts must appear in diagnostics/support-bundle evidence, including provider name, selected strategy name when present, profile/defaulting facts, status, and fallback causes.
- Define the benchmark/reporting expectation for PIT and bridge provider-strategy claims using the root benchmark artifact triplet and the current documented run-context rules.
- Document bounded non-goals so the story stays on diagnostics/evidence-contract scope instead of widening into runtime feature work.

### Scope Out
- Implementing non-SQLite optimized PIT or bridge reads.
- Changing the existing provider-neutral PIT-backed read request/row contracts or adding a new public read API.
- Exposing raw SQL, provider physical plans, automatic index advice, or automatic PIT/bridge maintenance.
- Benchmark reruns, package publication, release automation, or support-bundle transport automation.

## Acceptance Criteria
- The refinement names the existing read-strategy status vocabulary `NotEvaluated`, `ProviderStrategySelected`, and `ProviderNeutralFallback` as the only v1 status contract for provider-specific PIT and bridge read selection.
- The refinement ratifies the finite fallback-cause contract for provider-specific PIT and bridge reads as `ProviderNameMismatch`, `UnknownOrUnregisteredProviderName`, `NoProviderSpecificStrategyRegistered`, `UnsupportedSatelliteParent`, `MultiActiveSatelliteUnsupported`, `StrategyDeclined`, `UnsupportedPitShape`, and `UnsupportedBridgeShape`.
- The refinement states that authoritative provider-read evidence is emitted through `ReadStrategy` and `ReadShape.provider`, with optional fields such as `selectedStrategyName` omitted when not applicable instead of filled with sentinel values.
- The refinement states that SQLite is the only current repository-proven optimized latest-satellite, PIT, and bridge provider path and that any non-SQLite optimized read claim requires explicit benchmark evidence in the checked-in benchmark artifacts rather than inference from provider packages or write-strategy registrations.
- The refinement states that benchmark/reporting guidance for candidate PIT and bridge strategies must preserve run context and must distinguish provider-specific selection from provider-neutral fallback.
- The refinement records explicit non-goals: no raw-SQL exposure, no automatic PIT/bridge maintenance, no provider physical-plan promise, and no automatic runtime dispatch expansion.

## Definition of Done
- The authoritative refinement output records the bounded provider-read evidence contract and ties it to the existing read diagnostics/read-shape baseline.
- The documented contract keeps the existing PIT-backed read API and typed helper boundaries intact and does not introduce new public runtime shapes.
- The contract aligns wording across diagnostics, support-bundle evidence, telemetry vocabulary, and benchmark guidance for PIT and bridge read strategies.

## Implementation Notes
- Use `docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md` as the primary baseline for `ReadStrategy`, `ReadShape`, provider facts, closed vocabularies, and non-goals; extend that contract instead of creating a parallel evidence surface.
- Keep `docs/plans/pit-backed-as-of-read-api-contract.md` unchanged as the provider-neutral PIT request/row boundary; this story is about selection evidence and eligibility, not about new request types or row records.
- Keep the typed-helper dependency from `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md` intact: provider-read evidence stays value-free, request-bound diagnostics that can flow through the reviewed support-bundle path.
- Align benchmark wording with `docs/releases/v0.26.0.md` and the root benchmark artifacts so future non-SQLite claims only land with explicit row evidence and preserved run context.
- Use the existing `DataVaultActivityTracing` read families and modes (`LatestSatellite`, `Pit`, `Bridge`; `Current`, `AsOf`, `Traversal`) so diagnostics, telemetry, and benchmark language stay on one bounded vocabulary.
- The stale blocker cleanup was not applied directly on this branch; it was queued for replay on the source ticket branch and should be treated as durable pending cleanup rather than a failed write.

## Open Questions
- none

## Follow-Up Questions
- Which non-SQLite provider package should be the first target for an optimized PIT/bridge read prototype after this evidence contract is accepted?
- When non-SQLite candidate implementations exist, should the benchmark artifact schema grow explicit selected-strategy/fallback columns or keep that detail only in companion diagnostics output?

## Risks
- Until queued mutation `mutation-bd0022c7b2e86e2d` replays on `ticket/06F8KZHAB717MJJNAWWK7S0A5W-task-update-v0-27-0-analyzer-and-ef-lifecycle-do`, some views may still show the stale blocker temporarily.
- If future provider work adds new fallback causes, status values, or benchmark-schema fields without coordinated contract updates, diagnostics/support-bundle consumers and typed-helper expectations can drift.
- Non-SQLite benchmark proof depends on provider-specific environment availability, so evidence may lag implementation even when the contract is settled.

## Split Recommendations
- No immediate ticket split is required; this ticket remains a bounded contract-definition story.
- If downstream implementation work is created later, split by provider package and keep benchmark/verifier evidence in separate follow-up tickets so runtime work does not blur the contract baseline.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define the contract for provider-specific PIT and bridge read strategy candidates beyond SQLite, including eligibility gates, fallback causes, benchmark rows, read-shape diagnostics, and non-goals for raw SQL exposure or automatic maintenance.