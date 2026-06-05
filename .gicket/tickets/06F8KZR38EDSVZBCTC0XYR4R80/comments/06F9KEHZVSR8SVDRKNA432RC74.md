[gicket-bot] PO refinement contract

Summary
- Refinement confirms this ticket is the first bounded v0.31.0 performance-guidance child: update `docs/performance-profiles.md` with the authoritative decision-tree contract, keep typed helpers as a support-bundle-driven opt-in branch, and preserve the existing blocked downstream practical-doc task.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- `docs/performance-profiles.md` is already the authoritative performance-profile guide, but it still carries `Status: v0.28.0 adopter guidance`; this story is the bounded v0.31.0 contract uplift for that same document, not a new parallel guide.
- Parent epic `06F8KZQNH8CCMTJW9P95W1N388` explicitly expects this contract first and practical examples second, so the current outgoing `blocks` relation to task `06F8KZRSTHAGSP6GPGFBFQGY08` is the intended child flow.
- Typed helper generation remains outside the four runtime performance profiles: the current repository baseline is the support-bundle-driven opt-in contract in `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md`, and the decision tree should treat helpers as a design-time branch over reviewed `readShape` evidence.
- Observability evidence is already bounded in-repo: write selection uses `IDataVaultDiagnosticsService`, read selection uses `IDataVaultReadDiagnosticsService` plus `ReadShape`, metrics remain opt-in through `AddDVaultTelemetry()`, and tracing remains the sibling `DCoding.Data.DVault` ActivitySource contract.

Scope In
- Add one explicit v0.31.0 decision-tree contract section to `docs/performance-profiles.md` that tells adopters which question to answer first for write path, read path, typed-helper generation, diagnostics evidence, and stop or fallback handling.
- Preserve and normalize the current four runtime profile families already visible in the guide: small app-local vault, medium chunked ingestion, staged provider ingestion, and read-model heavy.
- Define write-path branching across materialized `DataVaultBulkSaveRequest`, provider-neutral `DataVaultChunkedSaveRequest`, async `IAsyncEnumerable<DataVaultSaveChunk>` sources, and diagnostics-gated staged provider ingestion without inventing new runtime routing.
- Define read-path branching across latest satellite, PIT as-of, and bridge traversal reads, including maintained PIT or bridge prerequisites, provider support limits, and `ReadShape` evidence requirements.
- Add the typed-helper opt-in branch that points to reviewed `dvault.support-bundle.v1` input, `DVaultGenerateTypedReadModels=true`, and request-bound `ReadShape` evidence for PIT or bridge helper emission.
- Link to the existing authoritative detail surfaces for benchmark artifacts, explicit save-service guidance, read-plan explain diagnostics, PIT and bridge boundary guidance, typed helper generation, and activity tracing.

Scope Out
- New runtime APIs, provider dispatch changes, automatic strategy routing, benchmark reruns, exporter or dashboard work, background PIT or bridge maintenance, or provider-specific SQL artifact generation.
- Rewriting the downstream practical examples task `06F8KZRSTHAGSP6GPGFBFQGY08`, release-note work, or README and navigation refreshes.
- Changing the existing support-bundle, typed-helper, explicit save-service, read-service, telemetry, or activity-tracing contracts beyond clarifying how adopters choose among them.

Open questions
- none

Follow-up questions
- After this contract lands, should downstream practical-doc task `06F8KZRSTHAGSP6GPGFBFQGY08` add only a short pointer in `docs/production-adoption-checklist.md`, or should that checklist stay unchanged and rely solely on `docs/performance-profiles.md`?

Risks
- If the new contract over-explains benchmark values instead of choice order, it will duplicate the existing profile tables and compete with the downstream practical-doc task instead of unblocking it.
- Optional PostgreSQL, SQL Server, MySQL, and Oracle provider rows are still evidence-visible but can be skipped when connection strings are unset; the contract must present those lanes as diagnostics-gated starting points, not as repository-proven measured wins.
- Typed-helper wording can regress into a false runtime-profile claim unless the doc keeps helper generation explicitly bound to one authoritative support bundle and reviewed `ReadShape` evidence.
- Read guidance can overpromise if it forgets the maintained PIT or bridge prerequisite or omits fallback handling such as unsupported shape or incomplete evidence from the decision tree.

Split recommendations
- No further split is needed. Keep this story as the contract-defining child under epic `06F8KZQNH8CCMTJW9P95W1N388` and leave practical examples, checklist polish, and release-note or navigation updates to the existing downstream tickets.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment