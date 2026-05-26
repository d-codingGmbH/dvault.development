<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already fixes the bounded v1 baseline: IDataVaultSaveService remains the public write boundary, and staged provider bulk ingestion is additive future work around that boundary rather than a new save API.
- Existing bounded save explainability is already centered on DataVaultSaveStrategyDiagnostics, DataVaultSaveStrategyCandidateDiagnostics, DataVaultSaveTelemetrySummary, and the deterministic fallback-explanation catalog; this story should extend that vocabulary for staged-provider fallback instead of introducing a separate diagnostics channel.
- Done story 06F5Q8YKR31DXGRXVPJ9031BQW already settled the internal staging SPI, lifecycle, and caller-owned transaction contract; this ticket should make those staged decline and fallback outcomes actionable for adopters rather than reopen the architecture decision.
- Live relation evidence shows this ticket is a child of epic 06F5Q8YBVRS2EZVMJK5EATV9AR and blocks documentation task 06F5Q90718D21DN1N1Q2AP7YEM; stale relation 06F5Q8YKR31DXGRXVPJ9031BQW -> 06F5Q8Z0Y0ADE5H37DAPA1ADQM already has queued removal mutation-d7bd529c93873885.
- Current ticket comments are bot claim and lease comments only; there are no human comments adding extra scope or blockers.

### Scope In
- Add finite staged-provider fallback cause kinds, explanations, and remediation hints on the existing save diagnostics and telemetry surfaces.
- Surface staged lifecycle phase, selected strategy, evaluated candidates, provider caveat classification, and request, hub, link, and satellite operation counts for staged decline or fallback decisions without raw values.
- Keep request-bound support-bundle and design-time explain output aligned with the same staged fallback vocabulary when caller-supplied representative save diagnostics are present.
- Add automated coverage for staged fallback causes, explanation text, candidate and selected-strategy reporting, operation-count reporting, and redaction boundaries.

### Scope Out
- Implementing staged provider bulk save strategies or changing provider-neutral save behavior.
- Introducing a new public save entrypoint, automatic stored-procedure path, or staging-management API.
- Reopening the staging SPI or caller-owned transaction contract already settled by 06F5Q8YKR31DXGRXVPJ9031BQW.
- Benchmark expansion and release or documentation rollout beyond the already separated benchmark and docs tickets.
- Emitting raw SQL, credentials, hash keys, payload values, transient stage object contents, or other unbounded per-row diagnostics.

## Acceptance Criteria
- Representative save diagnostics can distinguish ordinary provider-neutral fallback from staged-provider fallback or decline using finite machine-readable cause kinds with deterministic human-readable explanation and remediation text.
- For staged-provider evaluation, surfaced diagnostics preserve candidate ordering, selected-strategy identity when applicable, request count, total operation count, hub, link, and satellite operation counts, and relevant staged lifecycle or provider-caveat classification while staying redacted and bounded.
- Additive diagnostics align with the settled staging contract from 06F5Q8YKR31DXGRXVPJ9031BQW by reporting dirty-context, unsupported-shape, transaction-participation, cleanup, or provider-limitation outcomes without introducing a new save contract.
- When representative request-bound save diagnostics are supplied, support-bundle or equivalent explain output reuses the same staged fallback vocabulary and does not emit raw SQL, credentials, hash keys, payload values, or stage-row contents.
- Automated tests cover new staged fallback cause kinds, explanation and remediation text, candidate and selected-strategy reporting, operation-count reporting, and redaction behavior.

## Definition of Done
- The ticket has one authoritative refinement contract that treats this as additive save explainability work over existing diagnostics, telemetry, and support-bundle surfaces.
- Downstream provider, benchmark, and documentation tickets can rely on one finite staged fallback vocabulary and one redaction policy without reopening save-boundary or transaction-contract questions.
- Any public additions remain additive extensions to existing diagnostics types rather than a new persistence API or staging-management contract.
- Tests prove the new staged fallback reporting paths touched by the implementation.

## Implementation Notes
- Use the current diagnostics baseline instead of inventing a new artifact: DataVaultSaveStrategyDiagnostics already carries ProviderName, SelectedStrategyName, ordered Candidates, and finite FallbackCauses, while DataVaultSaveTelemetrySummary already carries request and operation counts plus fallback explanations.
- Keep the v0.19.0 baseline from README, docs/releases/v0.19.0.md, and docs/architecture/dvault-v1-explicit-save-service.md: staged provider bulk ingestion remains additive future work around the same IDataVaultSaveService boundary, and declined staged execution must still resolve to bounded provider-neutral fallback.
- If staged-provider caveats need new fields or enum members, keep them finite, additive, and reusable across runtime telemetry, request-bound diagnostics, and caller-supplied support-bundle diagnostics instead of provider-specific free-form text.
- Preserve the consumer-owned support-bundle rule: the command host does not invent representative requests, so staged fallback data serialized there must come from caller-supplied request-bound diagnostics and stay redacted.
- No child tickets, attachments, planning documents, or ticket-description updates were materialized in this pass; stale blocker cleanup for relation 06F5Q8YKR31DXGRXVPJ9031BQW--06F5Q8Z0Y0ADE5H37DAPA1ADQM--blocks was already queued earlier in the session as mutation-d7bd529c93873885.

## Open Questions
- none

## Follow-Up Questions
- After implementation evidence exists, should the documentation task publish a compact provider caveat matrix for staged fallback causes and remediation guidance?
- Should the benchmark story reuse the same staged lifecycle and cleanup outcome vocabulary in its execution detail rows so diagnostics and performance evidence stay aligned?
- After provider implementations land, do we want a separate rollout ticket for any additional metric or tag names beyond the current bounded telemetry fields?

## Risks
- If staged fallback causes are emitted as provider-specific free-form text instead of one shared finite catalog, provider packages will drift and downstream docs will not have a stable vocabulary.
- If the story broadens into new save APIs or stage-management surface area, it will reopen the already-closed staging SPI and transaction-contract decision.
- If staged diagnostics leak transient stage object details, SQL text, or row values, they will violate the existing bounded telemetry and support-bundle redaction posture.

## Split Recommendations
- No additional split is recommended; the epic already separates staging contract, provider implementations, benchmarks, documentation, and this bounded diagnostics story.
- If later implementation evidence shows materially different caveat taxonomies per provider, create provider-specific follow-up tickets rather than widening this shared diagnostics contract.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Make staged provider bulk fallback causes actionable for adopters.

Acceptance criteria:
- Adds deterministic remediation hints for staging SPI declines and provider-native fallback paths.
- Reports staging lifecycle, selected strategy, candidates, operation counts, and provider caveats without leaking values.
- Includes unit tests for new fallback causes and remediation messages.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

- decision: `implemented`
- scope: additive staged-provider bulk fallback diagnostics over existing save diagnostics, telemetry, and support-bundle surfaces
- repository evidence:
  - `src/DCoding.Data.DVault/DataVaultStagedProviderBulkDiagnostics.cs`
  - `src/DCoding.Data.DVault/IDataVaultProviderStagedBulkSaveDiagnostics.cs`
  - `src/DCoding.Data.DVault/DataVaultDiagnostics.cs`
  - `src/DCoding.Data.DVault/DataVaultSaveTelemetrySummary.cs`
  - `tests/DCoding.Data.DVault.Tests/Unit/DataVaultTelemetryTests.cs`
  - `tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs`
- validation:
  - `dotnet build DVault.slnx --nologo` passed
  - `dotnet test DVault.slnx --nologo` passed with external-provider live tests skipped by missing opt-in connection strings
  - `bash tools/check-format.sh` passed
<!-- gicket-bot:developer-delivery:v1:end -->