[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSBN23A20NX2K0YAXZ40ZGR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSBN23A20NX2K0YAXZ40ZGR`.
- Optimistic claim succeeded (`expectedRevision=06FBVV0YW9YRSNCT5MHRF2XPKW`, `currentRevision=06FBVV73TJ7GDMX2JHG26T8JF4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSBN23A20NX2K0YAXZ40ZGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSBN23A20NX2K0YAXZ40ZGR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSBN23A20NX2K0YAXZ40ZGR-story-codify-dependency-line-policy-after-packag' from source '8511f32c6e8af58dd2d6b44a9600de6b2a38236c'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSBN23A20NX2K0YAXZ40ZGR-story-codify-dependency-line-policy-after-packag` as `e13536922fbf`.

Open questions / Risiken
- Until the three named documentation surfaces are aligned, release operators and consumers will continue to see guidance that contradicts the already-landed project, test, and package-verifier baseline.
- If historical done-task blocks relations remain in live ticket metadata, automation or humans may misread delivery state even after the documentation work is complete.
- Split recommendation: No split recommended. The remaining work is a bounded documentation-alignment pass across docs/plans/shared-implementation-standards.md, docs/releases/v0.36.0.md, and docs/production-adoption-checklist.md.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9270`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `c9dc6271c4004db88c694977a9a23700`
- completed-at-utc: `<redacted>-12T22:30:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSBN23A20NX2K0YAXZ40ZGR/runs/20260612T223002727Z-c9dc6271c4004db88c694977a9a23700.json`