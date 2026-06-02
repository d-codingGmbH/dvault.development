<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Epic scope is already decomposed into seven persisted child tickets and matches the checked-in v0.26.0 documentation boundary for provider diagnostics, benchmark evidence, migration/idempotency guardrails, and the stored-procedure artifact boundary; no new bounded writes were needed in this refinement pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- No new child tickets, relation edits, description updates, attachments, or planning documents were materialized because the existing split and repository evidence already bound the scope tightly enough for PO-critic review.
- Treat the checked-in v0.26.0 documentation set as the authoritative baseline for this epic rather than reopening naming or boundary decisions already ratified in `README.md`, `docs/performance-profiles.md`, and `docs/releases/v0.26.0.md`.

### Scope In
- Provider performance explainability through the existing documentation and diagnostics boundary: benchmark artifact verifier evidence, bounded provider-eligibility facts, and performance-profile guidance.
- Schema-safety guidance through documented migration guardrails and idempotency preflight expectations, not automated repair or deployment flows.
- The documented high-performance artifact boundary for future stored-procedure or provider-specific SQL proposals, including the explicit decision gate that keeps those artifacts outside the default runtime path.
- Epic-level coordination across the already materialized child-ticket split until the linked slices are completed and closure evidence is coherent.

### Scope Out
- Automatic stored-procedure deployment, default stored-procedure runtime execution, or provider-specific SQL artifact generation.
- DBA workflow automation, dashboards, hosted observability, platform schedulers, or background-work orchestration.
- New runtime behavior, new diagnostics payload fields, new benchmark artifact schema, new benchmark rows, or package-publication/release automation claims under this epic.
- Non-SQLite optimized latest-satellite, PIT, or bridge read performance claims without new benchmark evidence.

## Acceptance Criteria
- Epic handoff text and child-ticket completion evidence stay aligned with the checked-in v0.26.0 boundary: provider-tuning diagnostics, benchmark verifier evidence, migration guardrails, idempotency preflight, and the stored-procedure artifact boundary are in scope, while package publication and new runtime behavior are not.
- Performance guidance remains anchored to the checked-in `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` triplet, including the four profile categories `SmallAppLocalVault`, `MediumChunkedIngestion`, `StagedProviderIngestion`, and `ReadModelHeavy` plus the run-context caveats documented in `docs/performance-profiles.md`.
- Provider guidance continues to present SQLite as the only repository-proven optimized latest-satellite, PIT, and bridge read path, while other provider rows remain documented as skipped or diagnostics-gated until new evidence exists.
- Schema guardrail and artifact-boundary guidance explicitly preserves the no-automation boundary: no implicit stored-procedure runtime path, no automatic deployment, and no schema repair or maintenance automation are introduced by this epic.
- Epic closure can be evaluated from the existing child-ticket graph and closure evidence rather than from additional PO scoping work.

## Definition of Done
- All existing child tickets under this epic are completed, re-scoped, or otherwise closed with repository evidence that remains consistent with the v0.26.0 documentation baseline.
- Any live blocking relations are either satisfied by completed dependency work or explicitly cleaned up if they become stale before epic closure.
- No residual PO-level uncertainty remains about the benchmark evidence baseline, provider-performance claim boundary, or stored-procedure/schema-automation exclusions.
- Closure evidence for the epic references the already-landed documentation and benchmark artifacts instead of implying unpublished runtime or release behavior.

## Implementation Notes
- The current authoritative evidence is documentation-first: `README.md`, `docs/performance-profiles.md`, and `docs/releases/v0.26.0.md` already define the public boundary for this epic.
- The checked-in benchmark baseline is bounded and finite: 3 iterations, 1 warmup iteration, `ProviderDefault` load timestamp storage, provider filter `all`, Debian GNU/Linux 13 x64, .NET 10.0.8, required SQLite local temporary files, and optional PostgreSQL, SQL Server, MySQL, and Oracle rows preserved as skipped when connection strings are unset.
- Existing explainability already has a concrete code surface in `DataVaultActivityTracing`: provider, outcome, failure-kind/class, duration-bucket, maintenance-kind, and read-model-kind tags/events are the runtime evidence surface, not a dashboard obligation.
- Use this epic as a coordination parent only; the materialized child split is already present and no additional PO-side decomposition is justified from the current evidence.
- No planning document or ticket-description rewrite was applied in this pass because the existing docs plus live relation graph already provide the necessary authoritative boundary.

## Open Questions
- none

## Follow-Up Questions
- When the linked implementation/documentation tickets finish, should epic closure also remove the two remaining incoming `blocks` relations if they are no longer semantically needed?
- After v0.26.0, is there a separate backlog need to gather repository-backed optimized read evidence for non-SQLite providers, or should those claims remain intentionally deferred?

## Risks
- The benchmark artifacts are observational, not SLA commitments; if later closure evidence drops the documented run-context caveats, adopters may overread the timing numbers.
- The live relation graph still shows two incoming blockers, so epic closure could stall operationally even though no additional PO clarification is required.

## Split Recommendations
- No further split recommended. The epic already has seven persisted child tickets and should remain a coordination/closure parent.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Improve provider performance explainability and schema safety through evidence-backed diagnostics, migration checks, and a documented high-performance artifact boundary.

# Scope Out
No automatic stored-procedure deployment, no default stored-procedure runtime path, no DBA workflow automation, no dashboard, and no platform scheduler.