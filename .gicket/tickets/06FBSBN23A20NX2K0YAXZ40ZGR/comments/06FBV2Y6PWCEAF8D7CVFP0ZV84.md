[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSBN23A20NX2K0YAXZ40ZGR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSBN23A20NX2K0YAXZ40ZGR`.
- Optimistic claim succeeded (`expectedRevision=06FBSD9W6P7X513E9M0JP67SRW`, `currentRevision=06FBTZS3MQVAG5J3RKGYNMZNM4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSBN23A20NX2K0YAXZ40ZGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSBN23A20NX2K0YAXZ40ZGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSBN23A20NX2K0YAXZ40ZGR-story-codify-dependency-line-policy-after-packag' from source 'fbef7290cdd468a9e3761896e8f5d69466d4aeac'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If current-baseline release notes and adoption guidance are not synced to the ratified policy, consumers and release operators will see contradictory dependency guidance even though code, tests, and verifier expectations already agree.
- Because live relation metadata still uses `blocks` for already-done implementation tasks, any workflow that interprets those links literally could misread delivery state until the relations are cleaned up.
- Split recommendation: No new split is needed. The repository already shows the net8 alignment task `06FBSBVGFERJGFF74Y5FC3G7B8` and the net10 alignment task `06FBSBVPAS4XV801DN3J8J3R40` as done; the remaining work is current-baseline documentation consistency.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9131`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `e5867a29c9bb42a0ae5c97cd7782f1e5`
- completed-at-utc: `<redacted>-12T20:35:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSBN23A20NX2K0YAXZ40ZGR/runs/20260612T203514718Z-e5867a29c9bb42a0ae5c97cd7782f1e5.json`