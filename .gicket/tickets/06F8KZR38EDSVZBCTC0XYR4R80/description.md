<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement confirms this ticket is the first bounded v0.31.0 performance-guidance child: update `docs/performance-profiles.md` with the authoritative decision-tree contract, keep typed helpers as a support-bundle-driven opt-in branch, and preserve the existing blocked downstream practical-doc task.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- `docs/performance-profiles.md` is already the authoritative performance-profile guide, but it still carries `Status: v0.28.0 adopter guidance`; this story is the bounded v0.31.0 contract uplift for that same document, not a new parallel guide.
- Parent epic `06F8KZQNH8CCMTJW9P95W1N388` explicitly expects this contract first and practical examples second, so the current outgoing `blocks` relation to task `06F8KZRSTHAGSP6GPGFBFQGY08` is the intended child flow.
- Typed helper generation remains outside the four runtime performance profiles: the current repository baseline is the support-bundle-driven opt-in contract in `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md`, and the decision tree should treat helpers as a design-time branch over reviewed `readShape` evidence.
- Observability evidence is already bounded in-repo: write selection uses `IDataVaultDiagnosticsService`, read selection uses `IDataVaultReadDiagnosticsService` plus `ReadShape`, metrics remain opt-in through `AddDVaultTelemetry()`, and tracing remains the sibling `DCoding.Data.DVault` ActivitySource contract.

### Scope In
- Add one explicit v0.31.0 decision-tree contract section to `docs/performance-profiles.md` that tells adopters which question to answer first for write path, read path, typed-helper generation, diagnostics evidence, and stop or fallback handling.
- Preserve and normalize the current four runtime profile families already visible in the guide: small app-local vault, medium chunked ingestion, staged provider ingestion, and read-model heavy.
- Define write-path branching across materialized `DataVaultBulkSaveRequest`, provider-neutral `DataVaultChunkedSaveRequest`, async `IAsyncEnumerable<DataVaultSaveChunk>` sources, and diagnostics-gated staged provider ingestion without inventing new runtime routing.
- Define read-path branching across latest satellite, PIT as-of, and bridge traversal reads, including maintained PIT or bridge prerequisites, provider support limits, and `ReadShape` evidence requirements.
- Add the typed-helper opt-in branch that points to reviewed `dvault.support-bundle.v1` input, `DVaultGenerateTypedReadModels=true`, and request-bound `ReadShape` evidence for PIT or bridge helper emission.
- Link to the existing authoritative detail surfaces for benchmark artifacts, explicit save-service guidance, read-plan explain diagnostics, PIT and bridge boundary guidance, typed helper generation, and activity tracing.

### Scope Out
- New runtime APIs, provider dispatch changes, automatic strategy routing, benchmark reruns, exporter or dashboard work, background PIT or bridge maintenance, or provider-specific SQL artifact generation.
- Rewriting the downstream practical examples task `06F8KZRSTHAGSP6GPGFBFQGY08`, release-note work, or README and navigation refreshes.
- Changing the existing support-bundle, typed-helper, explicit save-service, read-service, telemetry, or activity-tracing contracts beyond clarifying how adopters choose among them.

## Acceptance Criteria
- `docs/performance-profiles.md` contains a clearly labeled v0.31.0 decision-tree contract section that is authoritative for adopter choice order and does not create a second competing decision model elsewhere in the repository.
- The contract gives an ordered write decision path that distinguishes ordinary materialized saves, bounded chunked saves, already-async chunk sources, and staged provider ingestion, with the relevant diagnostics, telemetry, and finite stop or fallback conditions for each branch.
- The contract gives an ordered read decision path that distinguishes latest satellite, PIT as-of, and bridge traversal reads, and it states maintained PIT or bridge freshness, provider support, and incomplete `ReadShape` evidence as explicit fallback or stop conditions.
- The contract includes a separate design-time typed-helper branch that keeps generated satellite, PIT, and bridge helpers behind exactly one authoritative `dvault.support-bundle.v1` input and reviewed request-bound `ReadShape` evidence, rather than presenting helpers as a fifth runtime performance profile.
- The section links to the repository's authoritative detail surfaces for benchmark evidence, explicit save-service boundaries, read-plan explain diagnostics, PIT and bridge boundary guidance, typed helper generation, and activity tracing or metrics guidance.
- Non-SQLite provider claims remain bounded to the existing diagnostics-gated evidence posture, and SQLite remains the only repository-proven optimized latest-satellite read path unless new benchmark evidence is added in another ticket.

## Definition of Done
- The repository diff updates `docs/performance-profiles.md` outside `.gicket` and moves the document to a v0.31.0 contract baseline for this decision-tree section.
- The new section is internally consistent with the existing guide, the parent epic child flow, and blocked task `06F8KZRSTHAGSP6GPGFBFQGY08`, so downstream documentation can elaborate examples without redefining the contract.
- Cross-links point to existing authoritative docs instead of duplicating detailed PIT, bridge, typed-helper, diagnostics, tracing, or benchmark prose and tables.
- The final wording preserves the documented non-goals: no automatic routing, no raw SQL or physical-plan promises, no dashboards or exporters, no automatic PIT or bridge maintenance, and no provider-specific SQL artifact workflow.

## Implementation Notes
- No bounded planning document, attachment, ticket-description update, or relation mutation was applied during this PO refinement; live relation state remains parent epic `06F8KZQNH8CCMTJW9P95W1N388`, outgoing `blocks` to task `06F8KZRSTHAGSP6GPGFBFQGY08`, and historical incoming `blocks` from done task `06F8KZQAWZ7QRGB68KB21C9B0R`.
- Use the existing profile tables and stop-condition prose as inputs; the work is to add an explicit decision-tree contract section, not to replace the four profile sections or restate benchmark tables.
- For write selection, align wording with `docs/architecture/dvault-v1-explicit-save-service.md`: `IDataVaultSaveService` is the public boundary, `DataVaultBulkSaveRequest` stays the materialized baseline, chunked and async save paths remain provider-neutral bounded variants, and staged provider lanes stay diagnostics-gated.
- For read selection, align wording with `docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md` and `docs/architecture/dvault-v1-pit-bridge-boundary.md`: latest satellite, PIT, and bridge choices are request-bound, value-free, and dependent on maintained read-model freshness.
- For typed helpers and observability, point to `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md`, `docs/architecture/dvault-v1-activity-tracing-contract.md`, the root benchmark artifact triplet, and the benchmark README; helper generation is compile-time opt-in over reviewed support-bundle evidence, while telemetry and tracing remain opt-in observability surfaces.

## Open Questions
- none

## Follow-Up Questions
- After this contract lands, should downstream practical-doc task `06F8KZRSTHAGSP6GPGFBFQGY08` add only a short pointer in `docs/production-adoption-checklist.md`, or should that checklist stay unchanged and rely solely on `docs/performance-profiles.md`?

## Risks
- If the new contract over-explains benchmark values instead of choice order, it will duplicate the existing profile tables and compete with the downstream practical-doc task instead of unblocking it.
- Optional PostgreSQL, SQL Server, MySQL, and Oracle provider rows are still evidence-visible but can be skipped when connection strings are unset; the contract must present those lanes as diagnostics-gated starting points, not as repository-proven measured wins.
- Typed-helper wording can regress into a false runtime-profile claim unless the doc keeps helper generation explicitly bound to one authoritative support bundle and reviewed `ReadShape` evidence.
- Read guidance can overpromise if it forgets the maintained PIT or bridge prerequisite or omits fallback handling such as unsupported shape or incomplete evidence from the decision tree.

## Split Recommendations
- No further split is needed. Keep this story as the contract-defining child under epic `06F8KZQNH8CCMTJW9P95W1N388` and leave practical examples, checklist polish, and release-note or navigation updates to the existing downstream tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define the authoritative v0.31.0 performance decision-tree contract as a concrete repository documentation change.

Required repository output
- Update `docs/performance-profiles.md` with an explicit decision-tree contract section for v0.31.0.
- The contract must cover write selection, read selection, typed helper generation, diagnostics evidence, and stop/fallback conditions.
- This ticket must produce a repository diff outside `.gicket`; it is not a tracking-only or ratification-only ticket.

Scope in
- State the ordered questions an adopter should answer before choosing `DataVaultBulkSaveRequest`, `DataVaultChunkedSaveRequest`, async chunk-source saves, staged provider ingestion, latest reads, PIT reads, bridge reads, or typed helper generation.
- Name the evidence surfaces used for decisions: `IDataVaultDiagnosticsService`, `IDataVaultReadDiagnosticsService`, read-shape diagnostics, telemetry summaries, benchmark artifacts, and explicit PIT/bridge maintenance freshness.
- Keep typed helper generation as an opt-in design-time branch over authoritative support bundles, not as a fifth runtime performance profile.
- Link to the existing PIT/bridge, typed helper, activity tracing, and benchmark evidence documents where they are the authoritative detail.

Scope out
- New runtime APIs, provider dispatch changes, automatic strategy routing, benchmark reruns, dashboards, exporters, background maintenance, or provider-specific SQL artifact contracts.
- Rewriting the practical examples or release notes; those are handled by downstream v0.31.0 tasks.

Relation intent
- This story blocks the practical decision-tree documentation task because the downstream task should elaborate the contract, not invent a competing decision model.