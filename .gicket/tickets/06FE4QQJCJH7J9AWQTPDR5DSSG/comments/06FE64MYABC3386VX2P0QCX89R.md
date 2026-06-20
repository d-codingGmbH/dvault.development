[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4QQJCJH7J9AWQTPDR5DSSG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QQJCJH7J9AWQTPDR5DSSG`.
- Optimistic claim succeeded (`expectedRevision=06FE4QT2H3HVDNXGQY14YBJGS0`, `currentRevision=06FE6336QK54E19JQ4P5GEZ9XC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4QQJCJH7J9AWQTPDR5DSSG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4QQJCJH7J9AWQTPDR5DSSG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4QQJCJH7J9AWQTPDR5DSSG-task-investigate-oracle-latest-satellite-evidenc' from source 'b8351124b22acd4da261746b44755f9efecb9868'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FE4QQJCJH7J9AWQTPDR5DSSG-task-investigate-oracle-latest-satellite-evidenc` as `3e401189e557`.

Open questions / Risiken
- If downstream PIT tuning work treats current Oracle latest-satellite capability evidence as equivalent to measured timing evidence, release or performance guidance could overclaim Oracle read performance.
- The historical 2026-06-07 smoke-read artifact still shows provider-neutral fallback for Oracle latest-satellite, so documentation must clearly distinguish that historical configured run from the newer v0.41+ registration and parity baseline.
- Until a configured Oracle latest-satellite benchmark lane exists, provider-specific tuning thresholds for adjacent read models can be justified, but end-to-end Oracle latest-satellite improvement claims remain unproven.
- Split recommendation: No additional split is justified from current evidence; the existing Oracle latest-satellite evidence-gap track is already bounded by docs/plans/provider-optimization-gap-matrix.md P0.04.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8984`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d60579a261c74afb9605af2f4151290c`
- completed-at-utc: `<redacted>-20T03:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QQJCJH7J9AWQTPDR5DSSG/runs/20260620T032828748Z-d60579a261c74afb9605af2f4151290c.json`