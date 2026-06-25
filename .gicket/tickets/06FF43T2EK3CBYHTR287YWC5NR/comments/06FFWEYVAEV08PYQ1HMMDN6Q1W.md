[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43T2EK3CBYHTR287YWC5NR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43T2EK3CBYHTR287YWC5NR`.
- Optimistic claim succeeded (`expectedRevision=06FF44Q3MSKYGQGBYDP3FFJH2W`, `currentRevision=06FFWDBQDB714GYRYZJN6BWV6M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43T2EK3CBYHTR287YWC5NR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43T2EK3CBYHTR287YWC5NR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43T2EK3CBYHTR287YWC5NR-task-add-postgresql-binary-first-provider-quicks' from source '08ceffb1d50ef66f5e113965a03fd83af8f44eba'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43T2EK3CBYHTR287YWC5NR-task-add-postgresql-binary-first-provider-quicks` as `2d2a779358be`.

Open questions / Risiken
- Quickstart docs currently duplicate install/version text across multiple files; if touched surfaces are not aligned in this ticket, stale package numbers can remain visible to adopters.
- If the PostgreSQL parity note drifts from the runnable example or local-validation command, readers may incorrectly infer that external PostgreSQL setup is required for default DVault validation.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8066`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `c003fd12af344ba9a50f6a5520e1e9a4`
- completed-at-utc: `<redacted>-25T10:03:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43T2EK3CBYHTR287YWC5NR/runs/20260625T100316172Z-c003fd12af344ba9a50f6a5520e1e9a4.json`