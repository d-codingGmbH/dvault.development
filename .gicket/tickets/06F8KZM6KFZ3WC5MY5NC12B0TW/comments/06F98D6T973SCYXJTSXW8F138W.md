[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZM6KFZ3WC5MY5NC12B0TW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZM6KFZ3WC5MY5NC12B0TW`.
- Optimistic claim succeeded (`expectedRevision=06F98B1DPSEVYWX18G1Q9W79YW`, `currentRevision=06F98B6NKHB87KQ1BY5ANQ9A0G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZM6KFZ3WC5MY5NC12B0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails' from source '90b6ec0a4bb9ba6c34c73eaf31060710279c15f2'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails` as `cd3ed433dd13`.

Open questions / Risiken
- Until this closure-only contract replaces the stale split language, automation or humans could still misroute the epic to dev and duplicate already-landed work.
- Future provider or schema-governance expansion could be mistakenly attached to this completed epic instead of being tracked as new work.
- Split recommendation: No new split is recommended; the existing four child tickets already cover and complete the epic scope on develop.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8207`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d0b9b8949fc34171bf79fdf21ed4479d`
- completed-at-utc: `<redacted>-04T19:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/runs/20260604T195552258Z-d0b9b8949fc34171bf79fdf21ed4479d.json`