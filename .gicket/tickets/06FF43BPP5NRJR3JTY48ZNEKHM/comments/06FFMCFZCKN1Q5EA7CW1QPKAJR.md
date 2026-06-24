[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FF43BPP5NRJR3JTY48ZNEKHM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43BPP5NRJR3JTY48ZNEKHM`.
- Optimistic claim succeeded (`expectedRevision=06FFM16GSY374K7P4QS5T5JW0M`, `currentRevision=06FFMAHFVX1X4YYR0BFYHZYBTM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43BPP5NRJR3JTY48ZNEKHM-task-normalize-provider-neutral-pit-maintenance' from source 'c4eb9e27999384b4d209395f33cbd1d13b792d1a'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FF43BPP5NRJR3JTY48ZNEKHM-task-normalize-provider-neutral-pit-maintenance` as `31bb67c05243`.

Open questions / Risiken
- Blocking finding: For the supplied closure-only review path, the ticket is not yet closeable: its own Definition of Done still requires benchmark-generation/normalization logic and regression coverage, but current branch history and repository artifacts show only ticket-metada...
- Required PO action: Fix the routing/contract mismatch: either re-route this as a normal pre-development developer handoff ticket, or keep closure-only routing and rewrite the contract so it refers only to already-landed evidence.
- Risky assumption: Assuming the runtime intended a closure-only audit even though the persisted contract reads like a normal pre-development implementation task.
- Risky assumption: Assuming sibling pit-full-rebuild-maintenance benchmark artifacts can land without any further contract/schema clarification; current repo docs still treat those rows as unlanded contract-only guidance.
- Split recommendation: No split is needed if PO re-routes this as a normal developer-hand-off ticket.
- Split recommendation: If the ticket must remain closure-only, split out the future implementation/coverage work and leave this ticket limited to already-landed contract/evidence updates.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8835`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `e087f41bbca74d8788f72128ee7f2453`
- completed-at-utc: `<redacted>-24T15:14:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43BPP5NRJR3JTY48ZNEKHM/runs/20260624T151401184Z-e087f41bbca74d8788f72128ee7f2453.json`