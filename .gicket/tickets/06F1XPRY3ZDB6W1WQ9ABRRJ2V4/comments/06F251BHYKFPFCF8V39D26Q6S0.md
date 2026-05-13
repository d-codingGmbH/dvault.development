[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPRY3ZDB6W1WQ9ABRRJ2V4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPRY3ZDB6W1WQ9ABRRJ2V4`.
- Optimistic claim succeeded (`expectedRevision=06F24Z5V68N6GVVBNAP71P5C84`, `currentRevision=06F24ZSJYHVGQAM6EBE6J22AQ0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPRY3ZDB6W1WQ9ABRRJ2V4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPRY3ZDB6W1WQ9ABRRJ2V4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPRY3ZDB6W1WQ9ABRRJ2V4-epic-ef-core-lifecycle-guardrails' from source '09a77e2324ca19ac705d766154de14d03a2cb734'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F1XPRY3ZDB6W1WQ9ABRRJ2V4-epic-ef-core-lifecycle-guardrails` as `3ca4018aa640`.

Open questions / Risiken
- If later edits remove or materially rewrite docs/releases/v0.8.0.md before closure review completes, the epic would regress against its release-documentation criterion.
- If later docs or ticket text reintroduce shorthand about DVault-owned design-time services, the closure evidence could drift from the ratified consumer-owned preflight boundary.
- Split recommendation: No further split recommended; the remaining closure evidence is already materialized through completed child 06F23Z08K0W49K5JMEHP60WZC0 and tracked artifact docs/releases/v0.8.0.md.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `51128`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0476`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `0ab55733b91a4625a4a11b9ae24a2361`
- completed-at-utc: `<redacted>-13T18:07:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/runs/20260513T180710608Z-0ab55733b91a4625a4a11b9ae24a2361.json`