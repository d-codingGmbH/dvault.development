[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7SEAWB2KSBQSHQB2MVV38'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7SEAWB2KSBQSHQB2MVV38`.
- Optimistic claim succeeded (`expectedRevision=06EY1SKP9NH7VA0A5E7N160998`, `currentRevision=06EYKCSPYSNS5ESZDPB7GS87WG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7SEAWB2KSBQSHQB2MVV38': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7SEAWB2KSBQSHQB2MVV38': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7SEAWB2KSBQSHQB2MVV38-story-build-example-scenario-for-orders-and-prod' from source '93e55d1026633e8ebf172f132159761b36055d39'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7SEAWB2KSBQSHQB2MVV38-story-build-example-scenario-for-orders-and-prod` as `13aa0a51493f`.

Open questions / Risiken
- If future edits let the conventional and DVault variants drift to different business facts or payloads, the comparison value of the story will erode.
- Because the structure explanation currently lives in integration tests and schema assertions instead of a dedicated tutorial page, readability depends on keeping that evidence compact and intentional.
- A reader may expect a standalone sample application even though the bounded v1 delivery surface is test-backed documentation rather than a public example app.
- Split recommendation: The story is already split appropriately: 06EXB7SP77MW1HVW7KT4ZFV6G8 covers the normal EF baseline and 06EXB7SY3J6160R9Q35CFN6Q1W covers the DVault link-and-satellite variant.
- Split recommendation: No further split is recommended unless a future ticket explicitly separates standalone sample-app packaging or benchmark harness reuse from the current test-backed example surface.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8454`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `58fea15169214b068f77b4673615eea6`
- completed-at-utc: `<redacted>-02T17:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7SEAWB2KSBQSHQB2MVV38/runs/20260502T172341027Z-58fea15169214b068f77b4673615eea6.json`