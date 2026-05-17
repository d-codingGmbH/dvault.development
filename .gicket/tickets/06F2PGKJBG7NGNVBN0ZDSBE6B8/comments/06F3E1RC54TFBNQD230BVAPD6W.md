[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGKJBG7NGNVBN0ZDSBE6B8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKJBG7NGNVBN0ZDSBE6B8`.
- Optimistic claim succeeded (`expectedRevision=06F3DZSJXK32BHM795RBPA0A14`, `currentRevision=06F3DZZR2S8K5K7G9NAVA4WQZ4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGKJBG7NGNVBN0ZDSBE6B8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGKJBG7NGNVBN0ZDSBE6B8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGKJBG7NGNVBN0ZDSBE6B8-task-test-link-parent-satellite-metadata-project' from source '0156c75b5ff7e77f56e5b728ab581a8ca92b2a58'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGKJBG7NGNVBN0ZDSBE6B8-task-test-link-parent-satellite-metadata-project` as `34632e1aea66`.

Open questions / Risiken
- The only material risk is intent mismatch: if the original human intent was new code-first capability rather than projection-test coverage, closing this ticket will not deliver that feature.
- If this ticket is kept open instead of closed, the historical code-first routing context may keep sending reviewers toward an unsupported surface.
- Split recommendation: No split on this ticket. Close it as already covered. If needed later, open separate tickets for code-first link-parent satellite declarations and for any broader coverage expansion.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7707`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6de92bf6e18048f28261f21d9d2c8e74`
- completed-at-utc: `<redacted>-17T17:41:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/runs/20260517T174109106Z-6de92bf6e18048f28261f21d9d2c8e74.json`