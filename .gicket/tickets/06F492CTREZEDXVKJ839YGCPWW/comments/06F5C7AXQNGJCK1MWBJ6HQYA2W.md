[gicket-bot] PO refinement contract

Summary
- Existing repository evidence already bounds this story: the shared performance artifact contract is in place, SQLite is the required completed local baseline, optional PostgreSQL/SQL Server/MySQL/Oracle provider rows stay visible with normalized skipped reasons when not configured, and no ticket-side write was needed during PO refinement.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Verified live relation state: 06F492CTREZEDXVKJ839YGCPWW is a child of 06F492BTNHRPBC7D24E13ECFKM, is currently blocked by 06F492CAB2293R7BGJWMWMRKT4, 06F492CFSJHN0RGXXRG3KT63FM, and 06F492CN76GS3CKM8EFD0C20XM, and currently blocks 06F492D05THPGQVT3B3K7853A0; no relation cleanup was materialized in this PO pass.
- Current ticket context has no recent human comments and no persisted attachments that add extra scope or constraints.
- docs/plans/performance-evidence-benchmark-artifact-contract.md already defines the authoritative benchmark artifact set, run-context fields, row fields, required SQLite baseline, and optional provider matrix for this story.
- benchmark-summary.csv already demonstrates the intended v1 evidence shape: completed SQLite optimized-versus-fallback rows for core scenarios plus visible provider-native-bulk-ingestion rows for PostgreSQL, SQL Server, MySQL, and Oracle with normalized skipped reasons when those providers are not configured.

Scope In
- Extend or reuse the existing benchmark harness and shared artifact contract to persist provider optimization regression baselines instead of inventing a new benchmark format.
- Keep SQLite local temporary files as the required always-completed provider baseline for provider-neutral fallback versus provider-optimized DVault strategies across the provider-sensitive scenarios already covered by the harness.
- Preserve optional external-provider baseline rows for PostgreSQL, SQL Server, MySQL, and Oracle, producing completed optimized-versus-fallback evidence when configured and explicit skipped rows when not configured.
- Capture deterministic provider execution detail for each optimized scenario so reviewers can tell whether the optimized path or the provider-neutral fallback path ran, using generated SQL when stable and practical or equivalent provider-native execution detail otherwise.

Scope Out
- Adding new providers beyond SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Replacing the shared performance-evidence artifact contract or creating a parallel ticket-specific artifact schema.
- Guaranteeing external provider availability in every local developer environment.
- Broad non-provider optimization performance work such as expanding compiled-model, compiled-query, or DbContext-pooling benchmarks into a new cross-provider matrix.

Open questions
- none

Follow-up questions
- Should a later ticket promote the optional external-provider evidence lanes into provisioned CI or nightly regression gates instead of release or manually collected evidence only?
- Should a later performance ticket expand the external-provider matrix beyond save-strategy baselines into compiled-model, compiled-query, or DbContext-pooling comparisons once provider environments are provisioned more reliably?

Risks
- Completed external-provider baselines remain environment-dependent because PostgreSQL, SQL Server, MySQL, and Oracle rows only execute when their configured connection strings and backing services are available.
- Generated SQL and other low-level provider execution details can drift across EF or provider-version changes, so capture needs normalization to avoid noisy false regressions.
- Small cross-provider timing deltas can be masked by machine variance, so allocation metrics and strict run-context parity remain necessary to keep the evidence interpretable.

Split recommendations
- No split recommended; current repository evidence already bounds this story to extending the existing performance-evidence contract and provider matrix.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment