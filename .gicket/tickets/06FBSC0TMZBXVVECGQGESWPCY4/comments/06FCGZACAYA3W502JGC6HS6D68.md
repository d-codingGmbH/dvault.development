[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC0TMZBXVVECGQGESWPCY4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC0TMZBXVVECGQGESWPCY4`.
- Optimistic claim succeeded (`expectedRevision=06FBSCXK0CM5GY4E6VPX029Q04`, `currentRevision=06FCGWMMEGDP61YP69R1PY73CC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC0TMZBXVVECGQGESWPCY4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC0TMZBXVVECGQGESWPCY4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC0TMZBXVVECGQGESWPCY4-task-document-binary-first-adoption-and-migratio' from source 'c46b07d8308c0afb16ab29f8d83373b7ce678d13'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSC0TMZBXVVECGQGESWPCY4-task-document-binary-first-adoption-and-migratio` as `ef98ec6563c8`.

Open questions / Risiken
- The biggest documentation risk is collapsing two different ideas into one sentence: binary-first is the recommendation for new projects, but it is not a silent runtime-default change for existing deployments.
- If only the historical v0.36.0 release note carries the strongest adoption language while CHANGELOG.md or the current carried-forward release note stays softer, adopters may still read the public guidance as inconsistent.
- Split recommendation: No split recommended; the current repository evidence already bounds this as one documentation-alignment task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9013`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `475dd61f9476408a814ae71d9182cac1`
- completed-at-utc: `<redacted>-14T23:35:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC0TMZBXVVECGQGESWPCY4/runs/20260614T233515278Z-475dd61f9476408a814ae71d9182cac1.json`