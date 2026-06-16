[gicket-bot] PO refinement contract

Summary
- Repository evidence already defines a bounded provider-bulk documentation baseline, so this ticket can move to PO critic as a documentation-alignment contract rather than a new benchmark or provider-implementation task.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already aligns the core interpretation surfaces: README.md, docs/performance-profiles.md, docs/releases/v0.39.0.md, CHANGELOG.md, docs/architecture/dvault-v1-explicit-save-service.md, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md.
- The safe v1 default is to keep README.md high-level and route detailed provider-bulk thresholds, skipped-row caveats, and artifact interpretation to the performance guide, benchmark README, and evidence matrix instead of duplicating raw benchmark detail in the root README.
- Provider bulk documentation must preserve explicit no-op and fallback semantics already established in the save-service contract: reused rows remain deterministic, empty chunks are no-ops, unsupported or declined provider batches fall back through the provider-neutral writer, and provider-specific strategies do not change caller-visible ordering or transaction ownership.

Scope In
- Document the provider-native bulk-ingestion posture for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 using the existing diagnostics-gated save-service boundary and checked-in benchmark evidence.
- Document root benchmark-triplet caveats: SQLite local rows are completed where present, while optional external-provider rows may remain skipped placeholders when connection-string environment variables are unset.
- Document provider-neutral fallback and declined-gate behavior for dirty contexts, unsupported multi-active batches, provider-name mismatch, and out-of-gate operation counts.
- Document the accepted no-op and reuse boundary for ordered bulk and chunked saves without widening runtime behavior.
- Keep release-note, performance-guide, and benchmark-guidance wording aligned with the explicit save-service architecture note.

Scope Out
- Rerunning benchmarks or generating new benchmark artifact triplets.
- Changing provider implementations, strategy gates, diagnostics behavior, or benchmark schema.
- Claiming completed external-provider timing from skipped root rows.
- Adding provider-native chunk execution, automatic routing, stored-procedure runtime dispatch, or broader DB2 bulk claims.
- Expanding non-SQLite latest-satellite optimization or read-strategy scope; that remains separate evidence or capability work.

Open questions
- none

Follow-up questions
- If a later release wants completed PostgreSQL, SQL Server, MySQL, Oracle, or DB2 timing in the root quick baseline instead of skipped placeholders, which provider-configured benchmark tickets should own those reruns and artifact bundles?
- Should any future root README expansion summarize provider bulk gates directly, or should README remain a high-level entry point that links to the performance guide and benchmark README for threshold details?

Risks
- Future doc edits can accidentally overclaim external-provider performance if they cite skipped root rows without the evidence-matrix posture or the linked provider-specific artifact bundle.
- Provider thresholds in the v0.32 evidence bundles are run-context-bound; copying their numbers without preserving hardware, runtime, iteration, warmup, and provider-configuration context would create misleading guidance.

Split recommendations
- No split recommended; current repository evidence already bounds this work to documentation alignment and claim hygiene across existing bulk-save evidence surfaces.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment