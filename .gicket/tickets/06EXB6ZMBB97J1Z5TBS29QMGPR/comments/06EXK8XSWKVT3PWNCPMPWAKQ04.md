[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB6ZMBB97J1Z5TBS29QMGPR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6ZMBB97J1Z5TBS29QMGPR`.
- Optimistic claim succeeded (`expectedRevision=06EXK6TAYZN3A8TWWN69RMFM00`, `currentRevision=06EXK7SZ2NAWYWGEMSC84E2DDR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB6ZMBB97J1Z5TBS29QMGPR-task-add-smoke-tests-for-minimal-startup' from source '83b41fc5be5dd94bfeda3ca1c70b4a9e80136b0b'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB6ZMBB97J1Z5TBS29QMGPR-task-add-smoke-tests-for-minimal-startup` as `57960f55cf33`.

Open questions / Risiken
- Required PO action: After label cleanup, re-handoff for PO-critic review with the AddDVault-only contract unchanged unless the product scope changes.
- Split recommendation: No split is required for the narrowed AddDVault smoke-test scope.
- Split recommendation: Create a separate follow-up only if UseDataVault, EF-specific startup wiring, provider integration, or DbContext-specific startup behavior becomes required.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8010`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `254a2327465f4a629e197c67c9329863`
- completed-at-utc: `<redacted>-29T14:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6ZMBB97J1Z5TBS29QMGPR/runs/20260429T142758464Z-254a2327465f4a629e197c67c9329863.json`