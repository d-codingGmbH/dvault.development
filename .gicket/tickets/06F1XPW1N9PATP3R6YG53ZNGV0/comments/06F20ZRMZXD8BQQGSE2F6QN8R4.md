[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F1XPW1N9PATP3R6YG53ZNGV0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPW1N9PATP3R6YG53ZNGV0`.
- Optimistic claim succeeded (`expectedRevision=06F20WDT4QF6TN0HXEQJK0TYT0`, `currentRevision=06F20WQTZP5Q6QXHTBRDEYPM9M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w' from source '2d4ccbcca60dcb2902a88ddce757212a057acaaa'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w` as `1f7d38ea2c68`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Refined :: - Refined the ticket around a focused model-first design-time workflow test and documentation update, using the existing public import, registration, and drift-report APIs with the default SQLite design-time provider...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Refined :: - Refined the ticket around a focused model-first design-time workflow test and documentation update, using the existing public import, registration, and drift-report APIs with the default SQLite ...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8028`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `541d5db449534b1f8bf30c468584b2c2`
- completed-at-utc: `<redacted>-13T08:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPW1N9PATP3R6YG53ZNGV0/runs/20260513T084058897Z-541d5db449534b1f8bf30c468584b2c2.json`