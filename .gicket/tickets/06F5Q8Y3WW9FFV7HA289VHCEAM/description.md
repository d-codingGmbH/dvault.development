<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this as a docs-only v0.19.0 streaming-save documentation and release-note rollout for already-landed chunked-save behavior; no child tickets, relation changes, description updates, attachments, or planning documents were applied.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Verified persisted context: the ticket has no human comments or closure amendments, remains a child of epic 06F5Q8WVYMV8KQPAENPEEE3YM4, is blocked by done stories 06F5Q8XPXEQPJTKGJ7BQGCY438 and 06F5Q8XXSBGW1B8RDRMGVF557W, and blocks future provider-staging work in 06F5Q8YBVRS2EZVMJK5EATV9AR and 06F5Q8YKR31DXGRXVPJ9031BQW.
- Repository evidence already includes the public chunked-save boundary in IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...), DataVaultChunkedSaveRequest, and DataVaultSaveChunk, plus chunked telemetry and SQLite integration coverage.
- The authoritative contract already exists in docs/architecture/dvault-v1-streaming-explicit-save-contract.md and fixes ordering, cancellation, transaction ownership, retained-state fallback, limitations, and non-goals.
- The root benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json triplet already contains customer-profile-streaming-save rows comparing a materialized bulk baseline with chunked-save bounded-10 and bounded-5 runs, including chunk count and retainedStateHighWater in executionDetail.
- No child tickets, relation changes, description updates, attachments, or planning documents were created or queued in this refinement pass.

### Scope In
- Update README to promote v0.19.0 as the current release baseline and add chunked-save usage and migration guidance next to the existing bulk-save guidance.
- Update docs/production-adoption-checklist.md current-baseline and save-boundary guidance to explain when to keep materialized bulk requests versus when to use chunked requests, plus point readers to visible validation and evidence sources.
- Update architecture docs so docs/architecture/dvault-v1-explicit-save-service.md is consistent with the landed chunked-save overload while leaving docs/architecture/dvault-v1-streaming-explicit-save-contract.md as the authoritative detailed contract.
- Add docs/releases/v0.19.0.md using the current release-note structure with package scope, highlights, migration guidance, benchmark evidence links, compatibility notes, limitations, and validation evidence.
- Keep document links anchored to repository-visible evidence: the root benchmark triplet, docs/plans/performance-evidence-benchmark-artifact-contract.md, README local validation commands, and docs/manual-nuget-publication.md.

### Scope Out
- No product-code, benchmark-harness, or test behavior changes.
- No new public API, telemetry type, provider optimization, or benchmark artifact schema work.
- No provider-native chunk execution or staged provider bulk ingestion claims beyond the current provider-neutral boundary.
- No release automation or NuGet publication process changes.
- No relation cleanup, child-ticket split, attachment, or planning-document work unless new evidence appears.

## Acceptance Criteria
- README documents DataVaultChunkedSaveRequest and DataVaultSaveChunk as additive explicit save inputs, preserves DataVaultBulkSaveRequest as the compatibility baseline for already-materialized ordered batches, and gives bounded migration guidance for when to switch.
- docs/production-adoption-checklist.md and the touched architecture docs point to the v0.19.0 streaming-save docs as the current baseline and explain caller-owned transaction, ordering, fallback visibility, and non-goals without contradicting the existing streaming contract.
- A new docs/releases/v0.19.0.md records the coordinated seven-package scope, links the streaming contract and the root benchmark triplet, and cites the existing validation and publication commands already documented in README and docs/manual-nuget-publication.md.
- The docs explicitly keep provider staging and provider-native chunk execution outside the v0.19.0 public claim set.
- Public current-baseline references touched by this story are internally consistent and do not leave README or the production checklist on v0.18.0 after the v0.19.0 release notes are added.

## Definition of Done
- README, the production checklist, the touched architecture docs, and the v0.19.0 release notes present one consistent public story for the current chunked-save surface.
- Versioned current-baseline references touched by this story are updated to v0.19.0 where they claim the current public release baseline; historical release notes remain historical.
- All referenced evidence and commands are repository-visible and align with the root benchmark triplet, docs/plans/performance-evidence-benchmark-artifact-contract.md, README local validation, and docs/manual-nuget-publication.md.
- Formatting or documentation validation passes for the touched files.
- No code, benchmark artifact, or relation-state changes are required to complete this ticket.

## Implementation Notes
- README already documents DataVaultBulkSaveRequest and the v0.18.0 release baseline, so extend those existing sections instead of inventing a parallel documentation path.
- Use docs/architecture/dvault-v1-streaming-explicit-save-contract.md as the source of truth for ordering, cancellation, transaction ownership, the 10000-series retained-state limit, fallback classifications, and non-goals; summarize and cross-link it rather than duplicating its full prose.
- Update docs/architecture/dvault-v1-explicit-save-service.md because it still frames explicit save architecture around single-request and ordered-bulk paths only.
- Use the customer-profile-streaming-save rows in benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json as the visible evidence baseline; they already expose the materialized versus chunked save path, chunk size, chunk count, processed chunk count, and retainedStateHighWater in executionDetail.
- Migration guidance should ratify one default: keep DataVaultBulkSaveRequest when callers already materialize the full ordered request set, and use DataVaultChunkedSaveRequest only when callers need bounded chunking without changing explicit timestamps, record source, ordering, or caller-owned transaction behavior.
- Link existing command sources instead of inventing new command variants: README local validation for build, test, and check-format, and docs/manual-nuget-publication.md for pack and verify-packages.
- No attachments, planning documents, or ticket-description updates were applied during refinement.

## Open Questions
- none

## Follow-Up Questions
- Should a later release add a dedicated checked-in before-and-after streaming-save benchmark bundle once there is a performance-tuning claim beyond the current root summary rows?
- After v0.19.0 docs land, should other public docs that still call v0.18.0 the current baseline be updated in a separate cleanup pass?

## Risks
- Current branch evidence shows streaming rows in the root benchmark triplet but no clearly labeled dedicated streaming before-and-after bundle, so release prose must avoid implying more artifact coverage than is visible.
- Stale v0.18.0 current-baseline references outside the touched docs could leave public guidance inconsistent if the implementation updates only the new release notes.
- The docs must clearly separate current provider-neutral chunked execution from future staged provider bulk ingestion so readers do not infer provider-native chunked optimization has already shipped.

## Split Recommendations
- No split is recommended; the work stays bounded if it is limited to README, docs/production-adoption-checklist.md, the relevant architecture docs, and docs/releases/v0.19.0 aligned to the already-landed contract and benchmark evidence.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Update public docs for the streaming save release.

Acceptance criteria:
- README, production checklist, architecture docs, and release notes describe contract, examples, limitations, and non-goals.
- Documents migration guidance from materialized DataVaultBulkSaveRequest usage.
- Links benchmark artifacts and validation commands.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Updated README, production adoption guidance, explicit-save architecture notes, the streaming explicit-save contract, and the new v0.19.0 release notes for the chunked explicit-save documentation rollout.
- Repaired the streaming contract document so the existing contract-marker unit test remains satisfied while the same paragraph still documents the v0.19.0 landed provider-neutral chunk execution and bounded retained-state diagnostics.
- Kept the work docs-only; no product code, benchmark harness, benchmark artifact, relation-state, or publication automation changes were made.

### Repository Artifacts
- README.md
- docs/production-adoption-checklist.md
- docs/architecture/dvault-v1-explicit-save-service.md
- docs/architecture/dvault-v1-streaming-explicit-save-contract.md
- docs/releases/v0.19.0.md

### Validation
- `dotnet build DVault.slnx --nologo` passed.
- `dotnet test DVault.slnx --nologo` passed.
- `bash tools/check-format.sh` passed.
- Targeted searches over touched current-baseline docs found v0.19.0 routing, chunked-save usage/migration guidance, root benchmark evidence links, and provider-native chunk execution/staged provider bulk ingestion exclusions.

### Notes
- The build and test commands emitted non-fatal `NU1900` warnings because this sandbox could not update NuGet vulnerability-cache files under the local HTTP cache; both commands still completed successfully.
- External provider integration tests remained skipped where their opt-in connection-string environment variables were not configured.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Added a repository-visible v0.19.0 release evidence index under `docs/releases/v0.19.0` so the tester-required path now exists in the repository tree while the ticket remains docs-only.
- The index routes to the authoritative v0.19.0 release notes, streaming save contract, explicit save service notes, root benchmark triplet, benchmark artifact contract, README validation commands, and manual NuGet publication checklist.
- Kept product code, benchmark artifacts, relation state, release automation, and provider staging behavior unchanged.

### Repository Artifacts
- `docs/releases/v0.19.0/README.md`
- `README.md`
- `docs/production-adoption-checklist.md`
- `docs/architecture/dvault-v1-explicit-save-service.md`
- `docs/architecture/dvault-v1-streaming-explicit-save-contract.md`
- `docs/releases/v0.19.0.md`

### Validation
- `dotnet build DVault.slnx --nologo` passed.
- `dotnet test DVault.slnx --nologo` passed.
- `bash tools/check-format.sh` passed.
- `test -d docs/releases/v0.19.0` passed, and `git ls-files --others --exclude-standard docs/releases/v0.19.0` showed `docs/releases/v0.19.0/README.md` before commit/writeback.
- Targeted searches over release documentation found `DataVaultChunkedSaveRequest`, `DataVaultSaveChunk`, `customer-profile-streaming-save` evidence, and provider-native chunk execution/staged provider bulk ingestion exclusions.

### Notes
- Build and test emitted non-fatal `NU1900` warnings because this sandbox could not update NuGet vulnerability-cache files under the local HTTP cache; both commands still exited successfully.
- External PostgreSQL, SQL Server, Oracle, and MySQL live integration tests remained skipped where their opt-in connection-string environment variables were not configured.
<!-- gicket-bot:developer-delivery:v1:end -->