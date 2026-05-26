[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already fixes the bounded v1 baseline: IDataVaultSaveService remains the public write boundary, and staged provider bulk ingestion is additive future work around that boundary rather than a new save API.
- Existing bounded save explainability is already centered on DataVaultSaveStrategyDiagnostics, DataVaultSaveStrategyCandidateDiagnostics, DataVaultSaveTelemetrySummary, and the deterministic fallback-explanation catalog; this story should extend that vocabulary for staged-provider fallback instead of introducing a separate diagnostics channel.
- Done story 06F5Q8YKR31DXGRXVPJ9031BQW already settled the internal staging SPI, lifecycle, and caller-owned transaction contract; this ticket should make those staged decline and fallback outcomes actionable for adopters rather than reopen the architecture decision.
- Live relation evidence shows this ticket is a child of epic 06F5Q8YBVRS2EZVMJK5EATV9AR and blocks documentation task 06F5Q90718D21DN1N1Q2AP7YEM; stale relation 06F5Q8YKR31DXGRXVPJ9031BQW -> 06F5Q8Z0Y0ADE5H37DAPA1ADQM already has queued removal mutation-d7bd529c93873885.
- Current ticket comments are bot claim and lease comments only; there are no human comments adding extra scope or blockers.

Scope In
- Add finite staged-provider fallback cause kinds, explanations, and remediation hints on the existing save diagnostics and telemetry surfaces.
- Surface staged lifecycle phase, selected strategy, evaluated candidates, provider caveat classification, and request, hub, link, and satellite operation counts for staged decline or fallback decisions without raw values.
- Keep request-bound support-bundle and design-time explain output aligned with the same staged fallback vocabulary when caller-supplied representative save diagnostics are present.
- Add automated coverage for staged fallback causes, explanation text, candidate and selected-strategy reporting, operation-count reporting, and redaction boundaries.

Scope Out
- Implementing staged provider bulk save strategies or changing provider-neutral save behavior.
- Introducing a new public save entrypoint, automatic stored-procedure path, or staging-management API.
- Reopening the staging SPI or caller-owned transaction contract already settled by 06F5Q8YKR31DXGRXVPJ9031BQW.
- Benchmark expansion and release or documentation rollout beyond the already separated benchmark and docs tickets.
- Emitting raw SQL, credentials, hash keys, payload values, transient stage object contents, or other unbounded per-row diagnostics.

Open questions
- none

Follow-up questions
- After implementation evidence exists, should the documentation task publish a compact provider caveat matrix for staged fallback causes and remediation guidance?
- Should the benchmark story reuse the same staged lifecycle and cleanup outcome vocabulary in its execution detail rows so diagnostics and performance evidence stay aligned?
- After provider implementations land, do we want a separate rollout ticket for any additional metric or tag names beyond the current bounded telemetry fields?

Risks
- If staged fallback causes are emitted as provider-specific free-form text instead of one shared finite catalog, provider packages will drift and downstream docs will not have a stable vocabulary.
- If the story broadens into new save APIs or stage-management surface area, it will reopen the already-closed staging SPI and transaction-contract decision.
- If staged diagnostics leak transient stage object details, SQL text, or row values, they will violate the existing bounded telemetry and support-bundle redaction posture.

Split recommendations
- No additional split is recommended; the epic already separates staging contract, provider implementations, benchmarks, documentation, and this bounded diagnostics story.
- If later implementation evidence shows materially different caveat taxonomies per provider, create provider-specific follow-up tickets rather than widening this shared diagnostics contract.

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