[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F492B40K7B0WWPKH8N3PPG3G'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492B40K7B0WWPKH8N3PPG3G`.
- Optimistic claim succeeded (`expectedRevision=06F4NV0JG98PX9D3AF6234JEEW`, `currentRevision=06F4Q2AHYQTWTEF6JBVSFVKP90`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F492B40K7B0WWPKH8N3PPG3G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F492B40K7B0WWPKH8N3PPG3G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F492B40K7B0WWPKH8N3PPG3G-story-expand-provider-capability-and-strategy-ex' from source '40360968780e01b439c08787220aa5b92483f880'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F492B40K7B0WWPKH8N3PPG3G-story-expand-provider-capability-and-strategy-ex` as `0b1f78fbad64`.

Open questions / Risiken
- Because support-bundle JSON already ships the diagnostics sections, expanding explain output is a contract-sensitive change that needs additive-only evolution and deterministic ordering.
- If provider capability or gate descriptions are duplicated instead of derived from existing profiles and evaluators, the explain output can drift from actual dispatch behavior.
- This ticket remains a prerequisite for 06F492BG6BZYYFMBE5WK7CB024, 06F492B9PR036PDNN52S06S9BC, and 06F492BNDPWS9P4EDSV0W7G6VM; underspecified output here would force those stories to infer provider behavior from source code again.
- Split recommendation: No additional split is required at PO refinement time; keep this story focused on the reusable diagnostics and support-bundle contract, while downstream consumption remains in 06F492BG6BZYYFMBE5WK7CB024, 06F492B9PR036PDNN52S06S9BC, and 06F492BNDPWS9P4EDSV...

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9111`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d7e741a2593641d3b459dd56376a3946`
- completed-at-utc: `<redacted>-21T17:38:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492B40K7B0WWPKH8N3PPG3G/runs/20260521T173845007Z-d7e741a2593641d3b459dd56376a3946.json`