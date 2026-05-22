[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F492B9PR036PDNN52S06S9BC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492B9PR036PDNN52S06S9BC`.
- Optimistic claim succeeded (`expectedRevision=06F4TYT7WE93P15FX4E5YKVA5C`, `currentRevision=06F4TZ1C0J73S7H1W4RMQRY98G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F492B9PR036PDNN52S06S9BC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F492B9PR036PDNN52S06S9BC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F492B9PR036PDNN52S06S9BC-story-add-query-shape-diagnostics-for-dvault-rea' from source 'aa8dd7422da6636922c8bc2e5ef777585e3b8092'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F492B9PR036PDNN52S06S9BC-story-add-query-shape-diagnostics-for-dvault-rea` as `f0843243e1c2`.

Open questions / Risiken
- DataVaultDiagnosticsResult and dvault.support-bundle.v1 are stable public surfaces, so the new member and supporting model(s) must remain strictly additive and version-safe.
- If the new payload leaks raw SQL, request hash keys, or payload values, it breaks the existing redaction-safe support-bundle boundary.
- Registry-backed and explicit diagnostics must stay semantically equivalent after normalization or support bundles will diverge for the same logical read.
- Index guidance must stay derived from translated metadata rather than hand-maintained strings or it will drift from actual projected schema.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7528`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `64a917fa274e473a8a5e9f15ad155a4a`
- completed-at-utc: `<redacted>-22T02:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492B9PR036PDNN52S06S9BC/runs/20260522T022718141Z-64a917fa274e473a8a5e9f15ad155a4a.json`