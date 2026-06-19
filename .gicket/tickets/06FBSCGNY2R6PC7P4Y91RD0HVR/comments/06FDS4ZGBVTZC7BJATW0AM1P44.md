[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCGNY2R6PC7P4Y91RD0HVR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCGNY2R6PC7P4Y91RD0HVR`.
- Optimistic claim succeeded (`expectedRevision=06FD6E1ZQ596CP7A1BBDXA7WKW`, `currentRevision=06FDS2086FA4KR9HMEBW5SYZER`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCGNY2R6PC7P4Y91RD0HVR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCGNY2R6PC7P4Y91RD0HVR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCGNY2R6PC7P4Y91RD0HVR-task-close-sql-server-pit-and-bridge-read-gaps' from source '356bd23b500aa8701f692c2bbfaa6db218ae817f'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSCGNY2R6PC7P4Y91RD0HVR-task-close-sql-server-pit-and-bridge-read-gaps` as `cb621a806f15`.

Open questions / Risiken
- If documentation updates only add the v0.32 smoke-read link without clearing `P2.02` and `P3.02`, the repository will continue to publish contradictory evidence posture for the same SQL Server rows.
- If reviewers insist that only the root quick triplet can close a read-evidence gap, this ticket will need an explicit policy decision because the existing artifact contract and v0.32 evidence bundle already preserve completed SQL Server PIT/bridge timing evidence.
- Split recommendation: No split recommended. SQL Server PIT and bridge closure share one provider, one existing artifact bundle, one strategy name, and one documentation-consistency problem; keep SQL Server latest-satellite evidence as its separate existing follow-up.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9733`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a7a192e8f1ba4896ac1bedf39667fe22`
- completed-at-utc: `<redacted>-18T21:12:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCGNY2R6PC7P4Y91RD0HVR/runs/20260618T211223385Z-a7a192e8f1ba4896ac1bedf39667fe22.json`