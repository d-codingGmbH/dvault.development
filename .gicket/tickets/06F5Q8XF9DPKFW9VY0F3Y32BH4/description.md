<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this story as the request-scoped retained-state and bounded diagnostics slice of the streaming-save epic; no child tickets, relation changes, description updates, attachments, or planning documents were materialized because the existing epic split already covers contract, execution, fallback/remediation, and benchmark work.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Done ticket 06F5Q8X261DQHG7N1445NGXB5W already fixes the additive chunked-save boundary, caller-owned transaction and cancellation rules, and the non-goals for background ingestion and implicit SaveChanges; this story should not reopen API-shape or orchestration questions.
- The retained continuity state for this story is the per-attempt state needed to preserve hash-key and hash-diff behavior across chunk boundaries; its visible scope is per satellite table or metadata declaration, parent hash key, and canonical multi-active driving-key values when applicable.
- The outgoing blocks relations to 06F5Q8XPXEQPJTKGJ7BQGCY438 and 06F5Q8XXSBGW1B8RDRMGVF557W make this story the source of raw bounded state and memory diagnostics that later fallback/remediation and benchmark work depend on, not the place for remediation prose or benchmark evidence.
- Repository diagnostics baselines already require deterministic redacted output and finite fallback vocabularies; this story should extend those bounded patterns for streaming continuity state instead of introducing ad-hoc logs or raw value dumps.

### Scope In
- Implement the retained-state layer needed to preserve chunk-boundary hash-key and hash-diff continuity for chunked saves without materializing the full logical load.
- Bound that retained state for one explicit chunked-save attempt and release it deterministically on success, failure, or cancellation.
- Emit bounded deterministic redacted diagnostics for chunk counts, retained-state usage, state-bound fallback or rejection causes, and unsupported memory-sensitive shapes.
- Add unit and integration coverage for continuity preservation, deterministic release, and bounded-diagnostics behavior.

### Scope Out
- Redefining the public chunked-save contract or its non-goals; that is already owned by done ticket 06F5Q8X261DQHG7N1445NGXB5W.
- Owning the general provider-neutral chunk execution pipeline or ordinary chunk and request ordering behavior beyond the retained-state and state-diagnostics slice.
- Developer-facing remediation guidance for provider mismatch, dirty-context, transaction, or chunk-sizing issues; that remains with 06F5Q8XPXEQPJTKGJ7BQGCY438.
- Benchmark artifact production or release evidence; that remains with 06F5Q8XXSBGW1B8RDRMGVF557W.
- New advanced-configuration hook families or provider-specific tuning matrices for memory policy in v1.

## Acceptance Criteria
- Chunked saves preserve the ordered-bulk hash-key and hash-diff continuity baseline by retaining only the per-attempt satellite continuity state needed across chunk boundaries, keyed by satellite table or metadata, parent hash key, and canonical multi-active driving-key values when relevant.
- Retained continuity state is bounded for v1, grows only within one explicit chunked-save attempt, and is deterministically released when the save completes, fails, or is canceled so no state leaks into later calls or caller-owned DbContext lifetime.
- When a request shape would require unsupported or unbounded retained state, the implementation deterministically rejects it or routes it through a documented bounded fallback with a finite cause classification instead of silently consuming unbounded memory.
- The diagnostics surface reports only bounded deterministic redacted summaries, including total and processed chunk counts, retained-state current and high-water counts, finite fallback or rejection cause kinds, and unsupported-shape classification, without raw hash keys, payload values, or unbounded per-parent listings.
- Any additive diagnostic summary reuses the repository's finite strategy-status and fallback-cause conventions where applicable and keeps outputs stable across equivalent ordered inputs.
- Tests cover retained-state release on success, failure, and cancellation; continuity preservation across chunk boundaries; deterministic behavior for unsupported memory-sensitive shapes; and redaction or boundedness of emitted diagnostics.

## Definition of Done
- The retained-state ownership, keying, bounding, and release rules for chunked-save continuity are documented clearly enough that the sibling execution, fallback, and benchmark stories do not need to reopen them.
- Repository tests prove chunk-boundary continuity behavior still matches the existing ordered-bulk baseline while the new bounded-state rules hold under success, failure, and cancellation paths.
- Any new diagnostic contract stays deterministic, redacted, low-cardinality, and consistent with existing DVault diagnostics vocabulary.
- No blocking ambiguity remains about this story's split boundary versus the done contract ticket and the existing fallback/remediation and benchmark child stories.

## Implementation Notes
- Use the existing contract note and tests as the semantic baseline: docs/architecture/dvault-v1-streaming-explicit-save-contract.md, StreamingExplicitSaveContractSnapshotTests, and the ExplicitDataVaultSaveServiceSqliteTests chunked-contract fixtures already pin the required continuity and ordering rules.
- Prefer extending existing bounded diagnostics surfaces and vocabulary such as DataVaultSaveTelemetrySummary, DataVaultSaveStrategyDiagnosticsStatus, DataVaultSaveStrategyFallbackCauseKind, and the repository's deterministic redaction patterns instead of adding ad-hoc verbose logging.
- Keep v1 on one deterministic default bounding policy; if later consumers need threshold tuning or custom memory-policy hooks, that is follow-up work rather than a blocker for this story.
- This story should emit raw bounded state facts such as counts, high-water marks, and finite cause kinds, while 06F5Q8XPXEQPJTKGJ7BQGCY438 owns the later developer-facing explanation and remediation layer built on top of those facts.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this pass because the current epic split already provides dedicated stories for contract, execution, fallback/remediation, and benchmark work.

## Open Questions
- none

## Follow-Up Questions
- After the deterministic v1 default ships, do consumers need an optional advanced hook for retained-state thresholds or eviction policy, or is the fixed bounded policy sufficient for the planned release window?
- Once this story lands, should support-bundle or diagnostics-report surfaces gain a representative chunked-save memory and state section, or is telemetry-only coverage enough for v1?
- Do any provider packages need provider-specific optimized continuity-state diagnostics after the provider-neutral bounded-state baseline is in place?

## Risks
- If continuity state is keyed too broadly, released too late, or allowed to outlive the explicit save attempt, the implementation can leak memory or contaminate later saves while appearing semantically correct in happy-path tests.
- If diagnostics emit raw hash keys, payload values, or unbounded per-parent detail, the story will violate the repository's deterministic redaction baseline and create supportability noise rather than bounded diagnostics.
- If this story absorbs remediation text or benchmark evidence work, it will duplicate the already-related sibling tickets and make ownership across the epic harder to reason about.

## Split Recommendations
- No additional split is recommended; the current epic already separates contract (06F5Q8X261DQHG7N1445NGXB5W), provider-neutral execution (06F5Q8X8Q72TQ5B7F2JSAJWPR8), fallback/remediation (06F5Q8XPXEQPJTKGJ7BQGCY438), and benchmark evidence (06F5Q8XXSBGW1B8RDRMGVF557W).

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Add bounded state handling and diagnostics for streaming hash-key/hash-diff continuity.

Acceptance criteria:
- Defines how per-parent satellite state is scoped, bounded, and released across chunks.
- Adds diagnostics for retained state, chunk counts, fallback causes, and unsupported memory-sensitive shapes.
- Keeps diagnostics deterministic and redacted.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

- decision: `implemented`
- summary: Added the public chunked-save API plus request-scoped retained satellite continuity state and bounded telemetry diagnostics for chunked explicit saves.
- repository paths: `src/DCoding.Data.DVault/DataVaultSaveService.cs`, `src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs`, `src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs`, `docs/architecture/dvault-v1-streaming-explicit-save-contract.md`, and focused unit/integration test updates.
- verification: `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo --no-build`, and `bash tools/check-format.sh` passed. Build emitted existing/environmental warnings including NuGet vulnerability-cache read-only warnings, but no errors.
- notes: The retained-state fallback uses finite cause kind `RetainedSatelliteSeriesLimitReached` and unsupported-shape kind `RetainedSatelliteSeriesLimitExceeded`; diagnostics expose counts and enum values only, not raw hash keys or payload values.

<!-- gicket-bot:developer-delivery:v1:end -->