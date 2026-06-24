[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43JEA6C3HNJ6AQA9XY7EC8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43JEA6C3HNJ6AQA9XY7EC8`.
- Optimistic claim succeeded (`expectedRevision=06FF43JZ037RZ3KJFYHGHZ4RG8`, `currentRevision=06FFPX10622JF6E42X9DRXGX9M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43JEA6C3HNJ6AQA9XY7EC8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43JEA6C3HNJ6AQA9XY7EC8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43JEA6C3HNJ6AQA9XY7EC8-task-update-v0-47-provider-maintenance-release-d' from source 'f85901103bbfe07dc0b2fbbcd5503f0177ab6658'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43JEA6C3HNJ6AQA9XY7EC8-task-update-v0-47-provider-maintenance-release-d` as `8595b7df1b98`.

Open questions / Risiken
- Incoming blocker tickets may still shift the exact v0.47 maintenance wording, which can force a final documentation pass even though the refinement boundary is clear.
- The repository currently advertises v0.46.0 across multiple guidance surfaces, so an incomplete sweep can leave mismatched release-line or package-version references.
- The existing evidence contract forbids treating source and test backed PIT maintenance work as completed timing evidence, so careless release-note language can create a documentation regression.
- Split recommendation: If package-guidance edits expand beyond release-line alignment and evidence-boundary consistency, split broader adopter-document rewrites into a separate docs ticket so this ticket stays bounded to the v0.47 release-doc sweep.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `44872`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0542`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `7955dac88fea4b2783aeae2e5641e798`
- completed-at-utc: `<redacted>-24T21:14:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43JEA6C3HNJ6AQA9XY7EC8/runs/20260624T211410009Z-7955dac88fea4b2783aeae2e5641e798.json`