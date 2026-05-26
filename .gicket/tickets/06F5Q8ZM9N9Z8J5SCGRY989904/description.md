<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the Oracle staged bulk story against the visible Oracle array-binding baseline, the completed shared staging SPI contract, and the existing opt-in Oracle test and benchmark surfaces; no PO-blocking questions remain.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The visible Oracle baseline is already bounded in source: `AddDVaultOracle()` selects `OracleDataVaultSaveStrategy` only for clean `Oracle.EntityFrameworkCore` batches with exact provider-name match, no pending tracked changes, no multi-active satellites, at least 50 total operations, and at most 10000 satellite operations.
- The current Oracle optimized path is not staged ingestion; it currently uses direct Oracle batching, including array binding when available and direct insert batching otherwise. This ticket evaluates staged Oracle work against that existing Oracle-optimized baseline, not only against the provider-neutral fallback.
- Done story `06F5Q8YKR31DXGRXVPJ9031BQW` already settled the internal staging SPI and caller-owned transaction/cancellation contract. This ticket should consume that contract rather than reopen public API or transaction-boundary decisions.
- Oracle live integration tests and provider-native benchmark rows remain opt-in behind `DVAULT_TEST_ORACLE_CONNECTION_STRING`; absent Oracle configuration should continue to produce explicit skipped evidence rather than silent omission.
- Broader staged-bulk diagnostics and broader benchmark-matrix or regression-budget work are already separated into `06F5Q8Z0Y0ADE5H37DAPA1ADQM` and `06F5Q900FC0P3HBZP81CVK7264`, so this story stays focused on Oracle evaluation and implementation.

### Scope In
- Evaluate Oracle batch shapes against the current direct Oracle strategy and record the boundary between retained direct execution, staged execution, and provider-neutral fallback.
- Implement an Oracle staged bulk path only for shapes where evidence shows a measurable win over the current Oracle direct path and where cleanup, transaction participation, and batch-size behavior are reliable under Oracle limits.
- Keep the work behind the existing `AddDVaultOracle()` and `IDataVaultSaveService` boundary while consuming the shared internal staging SPI from `06F5Q8YKR31DXGRXVPJ9031BQW`.
- Preserve or narrow Oracle-specific gates for clean contexts, supported satellite shapes, and bounded batch sizes so unsupported shapes still decline safely before or during Oracle-specific execution.
- Extend Oracle-focused unit, integration, and benchmark coverage enough to prove path selection, correctness, and cleanup behavior for the selected Oracle approach.

### Scope Out
- Adding a new public staged-save API, new public staging types, or any consumer-visible write boundary beyond `IDataVaultSaveService`.
- Cross-provider staged-bulk fallback diagnostics or remediation messaging beyond what this Oracle implementation needs locally; that belongs to `06F5Q8Z0Y0ADE5H37DAPA1ADQM`.
- Broad staged benchmark-matrix expansion or regression budgets across providers; that belongs to `06F5Q900FC0P3HBZP81CVK7264`.
- Non-Oracle staged implementations for PostgreSQL, SQL Server, or MySQL.
- Treating generated stored procedures or other Oracle-specific escape hatches as the default architecture.

## Acceptance Criteria
- The repository records the Oracle decision boundary between the existing direct Oracle path, a staged Oracle path, and provider-neutral fallback for eligible ordered bulk batches, including the conditions under which each path is selected or declined.
- Any Oracle staged path stays behind `AddDVaultOracle()` and the existing `IDataVaultSaveService` contract, uses the internal staging SPI from `06F5Q8YKR31DXGRXVPJ9031BQW`, and does not introduce new public save APIs or public staging types.
- Staged Oracle execution is enabled only for shapes where evidence shows a net benefit over the current Oracle direct path and where stage creation, population, execution, cleanup, cancellation, and failure handling are deterministic under the caller-owned transaction boundary and Oracle limits.
- Shapes that do not beat or cannot safely satisfy the staged path keep the current Oracle direct path or provider-neutral fallback, and unsupported shapes such as dirty contexts, multi-active satellites, oversized batches, or missing Oracle prerequisites are declined deterministically.
- Oracle-focused tests cover staged-path selection, retained direct-path selection, fallback behavior, persisted-row correctness for hub, link, and ordinary satellite batches, and cleanup or failure handling in the Oracle opt-in lane.
- When Oracle is configured, benchmark evidence is captured through the existing optional-provider artifact contract with visible Oracle rows comparing the retained direct path and any staged path; when Oracle is not configured, the harness still preserves deterministic skipped Oracle rows instead of silently dropping the Oracle boundary.

## Definition of Done
- Code and any supporting internal documentation make the retained-versus-staged Oracle boundary explicit enough that downstream benchmark work can extend evidence without reopening Oracle path-selection rules.
- The Oracle implementation reuses the completed shared staging SPI contract and does not change the settled public `IDataVaultSaveService` or caller-owned transaction and cancellation semantics.
- Oracle unit tests and opt-in Oracle integration tests cover selected, declined, and fallback shapes for hub, link, and ordinary satellite batches, including cleanup and failure behavior for the staged path when that path is implemented.
- Any Oracle benchmark evidence or planned skipped Oracle rows remains compatible with the existing benchmark summary triplet and the shared performance-evidence artifact contract.

## Implementation Notes
- Use the current `OracleDataVaultSaveStrategy` as the comparison baseline. The visible implementation already performs Oracle-specific direct batching, including array binding when supported, so staged ingestion must justify itself against that path rather than against a naive provider-neutral baseline.
- Preserve the existing Oracle gate baseline unless evidence supports a narrower staged gate: exact `Oracle.EntityFrameworkCore` provider match, clean change tracker, no multi-active satellites, minimum 50 total operations, and maximum 10000 satellite operations.
- Consume the internal staging SPI and transaction contract already settled by done story `06F5Q8YKR31DXGRXVPJ9031BQW`; do not redefine transaction ownership, cancellation ownership, or public save-service semantics here.
- Reuse the existing Oracle opt-in validation surfaces driven by `DVAULT_TEST_ORACLE_CONNECTION_STRING` for live integration proof and optional provider benchmark rows instead of inventing new provisioning or CI assumptions.
- Keep Oracle benchmark evidence aligned with `docs/plans/performance-evidence-benchmark-artifact-contract.md`; broader benchmark-matrix expansion and regression budgets remain downstream in `06F5Q900FC0P3HBZP81CVK7264`.
- Do not widen the current public release posture from `README.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and `docs/releases/v0.19.0.md`; those sources still treat staged provider bulk ingestion as outside the current public claim set.

## Open Questions
- none

## Follow-Up Questions
- After Oracle evaluation lands, should release-facing docs later publish an explicit staged-provider coverage matrix once the broader benchmark ticket is complete?
- If the Oracle implementation needs new adopter-facing staged fallback explanations, should those messages be delivered through `06F5Q8Z0Y0ADE5H37DAPA1ADQM` rather than expanded ad hoc in this story?
- If Oracle evidence supports only a very narrow staged window, should a later follow-up ticket expose that window more explicitly in diagnostics or release notes?

## Risks
- Oracle staging-object cleanup and privilege behavior may vary by environment; if cleanup is not deterministic under cancellation or failure, the staged path must stay narrower than initially hoped or remain disabled for those shapes.
- The current Oracle direct path already uses Oracle-specific batching, so a staged implementation may add complexity without enough benefit unless the benchmark evidence is materially better for specific shapes.
- Oracle quantitative proof remains opt-in behind external provider configuration, so missing local Oracle access can delay completed evidence even though the harness already supports deterministic skipped rows.

## Split Recommendations
- No additional split is recommended now. Oracle implementation scope, staged-bulk diagnostics scope, and broader benchmark-matrix scope are already separated across `06F5Q8ZM9N9Z8J5SCGRY989904`, `06F5Q8Z0Y0ADE5H37DAPA1ADQM`, and `06F5Q900FC0P3HBZP81CVK7264`.
- If Oracle evaluation reveals two materially different viable staging mechanisms with different privilege or cleanup assumptions, create an Oracle-specific follow-up for the secondary mechanism instead of widening this ticket beyond one evidence-backed approach.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: Evaluate and implement an Oracle staged bulk path only for shapes where evidence beats the existing gated strategy.

Acceptance criteria:
- Records the decision boundary between array binding, staging, and provider-neutral fallback.
- Implements staging only when cleanup, transaction, and batch-size behavior are reliable under Oracle limits.
- Adds Oracle-gated integration tests and benchmark evidence when configured.