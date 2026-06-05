[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZPZZE8VZEBANP5MPN8HH8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZPZZE8VZEBANP5MPN8HH8`.
- Optimistic claim succeeded (`expectedRevision=06F8M01XNYRBZ7QCQVE3864C44`, `currentRevision=06F9ARGB74839QE69ETR958004`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZPZZE8VZEBANP5MPN8HH8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZPZZE8VZEBANP5MPN8HH8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZPZZE8VZEBANP5MPN8HH8-story-add-typed-helper-freshness-transition-test' from source '4c5520d3a1dd4fda6b2c3f89a96809c2a80d97dc'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZPZZE8VZEBANP5MPN8HH8-story-add-typed-helper-freshness-transition-test` as `0ef804c3aa13`.

Open questions / Risiken
- The stale incoming `blocks` relation from `06F8KZPN02NWFGMRC2Q1PKYKDR` remains visible in the latest live relation read until the queued replay runs on that ticket's owner branch.
- Ticket `06F8KZQAWZ7QRGB68KB21C9B0R` remains blocked by this story until the transition-test coverage is delivered.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `28624`
- cached-tokens: `7552`
- effective-cache-ratio: `0.2638`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `346d0a6358464460a4eea6db812c2788`
- completed-at-utc: `<redacted>-05T01:49:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZPZZE8VZEBANP5MPN8HH8/runs/20260605T014941783Z-346d0a6358464460a4eea6db812c2788.json`