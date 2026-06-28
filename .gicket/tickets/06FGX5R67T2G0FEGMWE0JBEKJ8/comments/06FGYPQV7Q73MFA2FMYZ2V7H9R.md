[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FGX5R67T2G0FEGMWE0JBEKJ8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX5R67T2G0FEGMWE0JBEKJ8`.
- Optimistic claim succeeded (`expectedRevision=06FGX6QK712Z8XM1KYCT5GQZCG`, `currentRevision=06FGYN3VYHV3HR610TYD3S98C8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FGX5R67T2G0FEGMWE0JBEKJ8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FGX5R67T2G0FEGMWE0JBEKJ8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FGX5R67T2G0FEGMWE0JBEKJ8-task-add-privacy-quickstart-for-caller-owned-key' from source '595dd4f2858e9c72ca1bdbf0bba6137b1eb7d8df'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FGX5R67T2G0FEGMWE0JBEKJ8-task-add-privacy-quickstart-for-caller-owned-key` as `67f87b2b2941`.

Open questions / Risiken
- Because the privacy caveat appears in several documentation surfaces, partial edits could reintroduce contradictory claims about provider-native encryption or compliance automation if downstream docs work does not keep them aligned.
- Readers may over-interpret the demo key provider as a supported key-management lifecycle unless the caller-owned boundary remains explicit wherever the quickstart is referenced.
- Split recommendation: No split is needed for this ticket at refinement time. Any future native-encryption feature should be created as a separate provider-specific ticket for one exact capability at a time.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8644`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `e09e13d0750e4abe9cf74208dd905588`
- completed-at-utc: `<redacted>-28T17:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX5R67T2G0FEGMWE0JBEKJ8/runs/20260628T175048633Z-e09e13d0750e4abe9cf74208dd905588.json`