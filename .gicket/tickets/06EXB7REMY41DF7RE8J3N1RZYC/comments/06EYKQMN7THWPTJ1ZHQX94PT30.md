[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7REMY41DF7RE8J3N1RZYC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7REMY41DF7RE8J3N1RZYC`.
- Optimistic claim succeeded (`expectedRevision=06EYKPR2FKSRB732XN1J0QBV74`, `currentRevision=06EYKPW0ZKXG94XVDQ3FJA3FA4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7REMY41DF7RE8J3N1RZYC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7REMY41DF7RE8J3N1RZYC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7REMY41DF7RE8J3N1RZYC-task-document-project-reference-usage-and-future' from source '1e8cd2f19133ccebe90c127abe648ab7dec2dd3d'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7REMY41DF7RE8J3N1RZYC-task-document-project-reference-usage-and-future` as `e0ad163e65be`.

Open questions / Risiken
- If the future NuGet section includes executable commands before publication, the docs will immediately become false guidance.
- Because the root README is also reused as the package README, project-reference instructions must be clearly framed as pre-publication or from-source guidance so they do not confuse later package consumers.
- Split recommendation: No split recommended; the current evidence supports a single documentation ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8723`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `515f4c12bd014791a79f64e9a1b1789f`
- completed-at-utc: `<redacted>-02T18:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7REMY41DF7RE8J3N1RZYC/runs/20260502T180611136Z-515f4c12bd014791a79f64e9a1b1789f.json`