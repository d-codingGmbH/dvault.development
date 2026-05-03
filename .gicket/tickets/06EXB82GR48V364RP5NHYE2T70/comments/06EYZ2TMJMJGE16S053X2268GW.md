[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB82GR48V364RP5NHYE2T70'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB82GR48V364RP5NHYE2T70`.
- Optimistic claim succeeded (`expectedRevision=06EYZ1192BENYKCPK3FWKTN7TR`, `currentRevision=06EYZ14V3BKXTZSVT2056VXZMC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB82GR48V364RP5NHYE2T70': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB82GR48V364RP5NHYE2T70': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB82GR48V364RP5NHYE2T70-task-document-publication-criteria-and-manual-re' from source '1e36e1de901ff26bae66a710f7fac501a9626823'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB82GR48V364RP5NHYE2T70-task-document-publication-criteria-and-manual-re` as `e0ec42c4a8e1`.

Open questions / Risiken
- If the documentation leaves the release-note or changelog location implicit, manual releases may still diverge even though the rest of the checklist is explicit.
- Because publishing remains manual, any checklist that does not force full-family validation before the first push still leaves room for accidental partial publication.
- Future provider-specific release needs could pressure the coordinated family-release rule, so the documentation should state that the current v1 baseline is synchronized publication across all six packages.
- Split recommendation: No split recommended; the work is a single bounded documentation task for the current manual six-package NuGet release process.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9491`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `58e6211f1bed4195b69ee7b4a27cc7f2`
- completed-at-utc: `<redacted>-03T20:32:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB82GR48V364RP5NHYE2T70/runs/20260503T203258349Z-58e6211f1bed4195b69ee7b4a27cc7f2.json`