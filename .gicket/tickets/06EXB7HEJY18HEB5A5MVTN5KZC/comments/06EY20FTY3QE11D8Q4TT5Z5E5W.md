[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7HEJY18HEB5A5MVTN5KZC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7HEJY18HEB5A5MVTN5KZC`.
- Optimistic claim succeeded (`expectedRevision=06EY1Y1M0HCKA9QQVHZV7027BG`, `currentRevision=06EY1ZF85Q3EK73WQZT3B05HY8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7HEJY18HEB5A5MVTN5KZC-task-persist-hubs-and-links-idempotently' from source 'eacb8ea4cd2fb8b4b21cf906530f0182ce4ccbba'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7HEJY18HEB5A5MVTN5KZC-task-persist-hubs-and-links-idempotently` as `51004eb1be4c`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Amended :: - Amended the ticket contract to preserve first-persisted hub and link lineage metadata on reuse and to define IDataVaultSaveService reuse results on the existing public API.
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Amended :: - Amended the ticket contract to preserve first-persisted hub and link lineage metadata on reuse and to define IDataVaultSaveService reuse results on the existing public API.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8831`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `bbf95b7404e042c88e40fd565b5e2874`
- completed-at-utc: `<redacted>-01T00:48:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/runs/20260501T004815949Z-bbf95b7404e042c88e40fd565b5e2874.json`