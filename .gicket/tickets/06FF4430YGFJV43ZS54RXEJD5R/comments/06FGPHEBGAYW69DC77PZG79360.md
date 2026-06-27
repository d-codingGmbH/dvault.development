[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF4430YGFJV43ZS54RXEJD5R'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF4430YGFJV43ZS54RXEJD5R`.
- Optimistic claim succeeded (`expectedRevision=06FGJ1R5G6AZ3QGAPTKAA331AG`, `currentRevision=06FGPEF40PZ44YE07WE31G8J3M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF4430YGFJV43ZS54RXEJD5R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF4430YGFJV43ZS54RXEJD5R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF4430YGFJV43ZS54RXEJD5R-task-update-v0-49-modeling-parity-release-docs' from source 'a457604ab31c42f45ab1c70e8e675c7b7800ec11'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF4430YGFJV43ZS54RXEJD5R-task-update-v0-49-modeling-parity-release-docs` as `8bc6f09edfeb`.

Open questions / Risiken
- The v0.48 baseline is repeated across multiple docs, so a partial rollover could leave contradictory 8.48.0 and 10.48.0 versus 8.49.0 and 10.49.0 guidance.
- Readers may conflate typed read helpers with typed save mappers or source-generator parity unless the v0.49 docs keep the support-bundle-driven read-helper limits and the same-hub mapper support clearly separated.
- If analyzer wording is relaxed beyond the audit evidence, consumers could infer unsupported pure .NET 8 SDK analyzer compatibility.
- If dependent child and effectivity-specific API caveats are dropped during the refresh, the release could appear to claim broader modeling parity than the repository currently proves.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7210`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1a1347f90c5a41e897f96b95c9c5bcc1`
- completed-at-utc: `<redacted>-27T22:49:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF4430YGFJV43ZS54RXEJD5R/runs/20260627T224911283Z-1a1347f90c5a41e897f96b95c9c5bcc1.json`