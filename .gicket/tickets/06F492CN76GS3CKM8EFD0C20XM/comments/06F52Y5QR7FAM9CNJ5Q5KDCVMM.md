[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06F492CN76GS3CKM8EFD0C20XM-story-add-compiled-model-compiled-query-and-dbco' and commit '901ccc2af893' for ticket '06F492CN76GS3CKM8EFD0C20XM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492CN76GS3CKM8EFD0C20XM`.
- Optimistic claim succeeded (`expectedRevision=06F52GP8N7HGV9RNS5Y7D85HF8`, `currentRevision=06F52HVA66PSTJFTTAK7RK511C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06F492CN76GS3CKM8EFD0C20XM-story-add-compiled-model-compiled-query-and-dbco' from source 'ticket/06F492CN76GS3CKM8EFD0C20XM-story-add-compiled-model-compiled-query-and-dbco'.
- Planned implementation step: Added benchmark scenarios for compiled-model startup, compiled-query hub reads, and pooled versus non-pooled DbContext DVault operations in the existing benchmark harness.
- Planned implementation step: Extended the default SQLite benchmark matrix and stable artifact/test expectations from 26 to 32 rows, including the new six completed SQLite rows.
- Planned implementation step: Updated benchmark and architecture documentation to state the bounded SQLite-only performance evidence and pooled-context guardrails.
- Planned implementation step: Generated repository-root benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json using the standard benchmark artifact writer.
- Resolved branch route (fallback): base 'develop', work 'ticket/06F492CN76GS3CKM8EFD0C20XM-story-add-compiled-model-compiled-query-and-dbco'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06F492CN76GS3CKM8EFD0C20XM-story-add-compiled-model-compiled-query-and-dbco'.
- Continuing with pre-existing repository changes on branch 'ticket/06F492CN76GS3CKM8EFD0C20XM-story-add-compiled-model-compiled-query-and-dbco' because the active developer transport already materialized in-flight ticket edits: benchmark-summary.csv, benchmark-summary.json, ben...
- 16 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: Benchmark timings are machine-specific; the generated artifacts preserve OS, runtime, processor, provider filter, and iteration context for downstream interpretation.
- Risk: No SQL capture was added because the docs and benchmark rows limit the claim to timing/allocation evidence, not emitted SQL shape, index usage, or batching behavior.

Next steps
- Push branch 'ticket/06F492CN76GS3CKM8EFD0C20XM-story-add-compiled-model-compiled-query-and-dbco' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9918`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `fc882e2466b14dbcb0ea3200cb3e1d35`
- completed-at-utc: `<redacted>-22T20:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492CN76GS3CKM8EFD0C20XM/runs/20260522T205526369Z-fc882e2466b14dbcb0ea3200cb3e1d35.json`