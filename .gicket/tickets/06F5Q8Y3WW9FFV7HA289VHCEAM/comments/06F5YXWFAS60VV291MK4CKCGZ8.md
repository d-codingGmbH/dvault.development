[gicket-bot] PO refinement contract

Summary
- Refined this as a docs-only v0.19.0 streaming-save documentation and release-note rollout for already-landed chunked-save behavior; no child tickets, relation changes, description updates, attachments, or planning documents were applied.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Verified persisted context: the ticket has no human comments or closure amendments, remains a child of epic 06F5Q8WVYMV8KQPAENPEEE3YM4, is blocked by done stories 06F5Q8XPXEQPJTKGJ7BQGCY438 and 06F5Q8XXSBGW1B8RDRMGVF557W, and blocks future provider-staging work in 06F5Q8YBVRS2EZVMJK5EATV9AR and 06F5Q8YKR31DXGRXVPJ9031BQW.
- Repository evidence already includes the public chunked-save boundary in IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...), DataVaultChunkedSaveRequest, and DataVaultSaveChunk, plus chunked telemetry and SQLite integration coverage.
- The authoritative contract already exists in docs/architecture/dvault-v1-streaming-explicit-save-contract.md and fixes ordering, cancellation, transaction ownership, retained-state fallback, limitations, and non-goals.
- The root benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json triplet already contains customer-profile-streaming-save rows comparing a materialized bulk baseline with chunked-save bounded-10 and bounded-5 runs, including chunk count and retainedStateHighWater in executionDetail.
- No child tickets, relation changes, description updates, attachments, or planning documents were created or queued in this refinement pass.

Scope In
- Update README to promote v0.19.0 as the current release baseline and add chunked-save usage and migration guidance next to the existing bulk-save guidance.
- Update docs/production-adoption-checklist.md current-baseline and save-boundary guidance to explain when to keep materialized bulk requests versus when to use chunked requests, plus point readers to visible validation and evidence sources.
- Update architecture docs so docs/architecture/dvault-v1-explicit-save-service.md is consistent with the landed chunked-save overload while leaving docs/architecture/dvault-v1-streaming-explicit-save-contract.md as the authoritative detailed contract.
- Add docs/releases/v0.19.0.md using the current release-note structure with package scope, highlights, migration guidance, benchmark evidence links, compatibility notes, limitations, and validation evidence.
- Keep document links anchored to repository-visible evidence: the root benchmark triplet, docs/plans/performance-evidence-benchmark-artifact-contract.md, README local validation commands, and docs/manual-nuget-publication.md.

Scope Out
- No product-code, benchmark-harness, or test behavior changes.
- No new public API, telemetry type, provider optimization, or benchmark artifact schema work.
- No provider-native chunk execution or staged provider bulk ingestion claims beyond the current provider-neutral boundary.
- No release automation or NuGet publication process changes.
- No relation cleanup, child-ticket split, attachment, or planning-document work unless new evidence appears.

Open questions
- none

Follow-up questions
- Should a later release add a dedicated checked-in before-and-after streaming-save benchmark bundle once there is a performance-tuning claim beyond the current root summary rows?
- After v0.19.0 docs land, should other public docs that still call v0.18.0 the current baseline be updated in a separate cleanup pass?

Risks
- Current branch evidence shows streaming rows in the root benchmark triplet but no clearly labeled dedicated streaming before-and-after bundle, so release prose must avoid implying more artifact coverage than is visible.
- Stale v0.18.0 current-baseline references outside the touched docs could leave public guidance inconsistent if the implementation updates only the new release notes.
- The docs must clearly separate current provider-neutral chunked execution from future staged provider bulk ingestion so readers do not infer provider-native chunked optimization has already shipped.

Split recommendations
- No split is recommended; the work stays bounded if it is limited to README, docs/production-adoption-checklist.md, the relevant architecture docs, and docs/releases/v0.19.0 aligned to the already-landed contract and benchmark evidence.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment