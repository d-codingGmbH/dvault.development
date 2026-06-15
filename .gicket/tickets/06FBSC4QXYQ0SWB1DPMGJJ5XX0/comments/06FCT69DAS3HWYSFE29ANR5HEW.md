[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC4QXYQ0SWB1DPMGJJ5XX0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC4QXYQ0SWB1DPMGJJ5XX0`.
- Optimistic claim succeeded (`expectedRevision=06FBSCZAVNBH5068NJSCNRVXT0`, `currentRevision=06FCT4DXTK5PJ5NBJ5EX3W1RJM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC4QXYQ0SWB1DPMGJJ5XX0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC4QXYQ0SWB1DPMGJJ5XX0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC4QXYQ0SWB1DPMGJJ5XX0-task-update-performance-docs-with-provider-evide' from source 'e18b2f7dbaf13bdaf363f2a32d2c2b60372d5583'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Detected directly written bounded PO planning artifact for transactional writeback: docs/plans/provider-optimization-evidence-docs-v0.39-refinement.md.
- 3 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The live gicket comment and relation reads were trust-policy blocked in this run, so duplicate and relation decisions rely on the provided ticket snapshot and repository evidence rather than a fresh persisted relation read.
- If separate release-planning work changes the established dual package-version-line pattern for v0.39, the new release note wording will need to be adjusted to match that later release decision.
- Split recommendation: No split recommended. The visible repository evidence supports one bounded documentation task across `docs/performance-profiles.md`, `docs/releases/v0.39.0.md`, and `CHANGELOG.md`, and the ticket-bound refinement note has already been materialized to pres...

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9172`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `16fee485e29143589def3c314e70cc3b`
- completed-at-utc: `<redacted>-15T21:03:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC4QXYQ0SWB1DPMGJJ5XX0/runs/20260615T210359695Z-16fee485e29143589def3c314e70cc3b.json`