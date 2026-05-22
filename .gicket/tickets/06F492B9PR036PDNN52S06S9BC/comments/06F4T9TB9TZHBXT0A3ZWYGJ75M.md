[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F492B9PR036PDNN52S06S9BC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492B9PR036PDNN52S06S9BC`.
- Optimistic claim succeeded (`expectedRevision=06F4T6WF1TX36YXSKMR34YWX30`, `currentRevision=06F4T73HSFZ0VRH9NPD8N56BX8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F492B9PR036PDNN52S06S9BC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F492B9PR036PDNN52S06S9BC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F492B9PR036PDNN52S06S9BC-story-add-query-shape-diagnostics-for-dvault-rea' from source 'b80b56f3a527912e78a6096e63f2989e7418dc20'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F492B9PR036PDNN52S06S9BC-story-add-query-shape-diagnostics-for-dvault-rea` as `8d2519168262`.

Open questions / Risiken
- DataVaultDiagnosticsResult and dvault.support-bundle.v1 are stable public surfaces, so the new diagnostics member must remain additive and version-safe.
- Because registry-backed and explicit diagnostics normalize through shared request types, any divergence between those paths would create conflicting support-bundle evidence for the same logical read.
- If the payload includes raw SQL, request hash keys, or payload values, it will violate the current redaction-safe support-bundle boundary.
- Index guidance must stay derived from translated EF metadata rather than hand-maintained strings or it will drift from actual projected schema.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9565`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f990a9618eeb4a89aa9ba6199611eb9e`
- completed-at-utc: `<redacted>-22T00:48:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492B9PR036PDNN52S06S9BC/runs/20260522T004801354Z-f990a9618eeb4a89aa9ba6199611eb9e.json`