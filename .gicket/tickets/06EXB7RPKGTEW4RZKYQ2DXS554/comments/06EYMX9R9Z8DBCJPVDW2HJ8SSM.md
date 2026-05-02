[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7RPKGTEW4RZKYQ2DXS554'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7RPKGTEW4RZKYQ2DXS554`.
- Optimistic claim succeeded (`expectedRevision=06EYMWAT99C8350SSYKPZXJM90`, `currentRevision=06EYMWENY73Q7GQAXZYQ2RY698`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil' from source 'cc7ef341693844e27693d686b784402d060d2b3d'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil` as `c6b1841f5550`.

Open questions / Risiken
- Blocking finding: Sending this ticket to dev would contradict the current ownership model: both implementation-owning child tickets are already `done`, and the reviewed diff range contains only `.gicket` metadata changes.
- Required PO action: Keep the parent ticket unassigned to development unless a new parent-owned implementation slice is intentionally added to the delivery contract.
- Risky assumption: The current workflow may assume every PO-critic success routes to `dev`, but this ticket's contract explicitly says the correct resolution is umbrella closure/advance rather than developer work.
- Split recommendation: Keep the existing split: parent 06EXB7RPKGTEW4RZKYQ2DXS554 stays coordination-only, child 06EXB7RYFJ3YQDB1E4QHPP8034 owns the plain EF slice, and child 06EXB7S6DB97GVVTS2GGZ3CCX8 owns the DVault slice.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8978`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `343a8fc8957644cd8bd268bcd054646e`
- completed-at-utc: `<redacted>-02T20:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/runs/20260502T205043274Z-343a8fc8957644cd8bd268bcd054646e.json`