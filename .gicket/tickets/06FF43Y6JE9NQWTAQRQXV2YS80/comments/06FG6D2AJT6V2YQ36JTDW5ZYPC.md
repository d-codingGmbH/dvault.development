[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43Y6JE9NQWTAQRQXV2YS80'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43Y6JE9NQWTAQRQXV2YS80`.
- Optimistic claim succeeded (`expectedRevision=06FF44R63B183R2JJK81KG1QMG`, `currentRevision=06FG6ARPA6S5PWWWSQ7ZHF2EZ0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43Y6JE9NQWTAQRQXV2YS80': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43Y6JE9NQWTAQRQXV2YS80': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43Y6JE9NQWTAQRQXV2YS80-task-add-support-bundle-facts-for-repeated-same' from source '8ce484d59e5de21e6b3363388f9e26ac7cbf046a'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43Y6JE9NQWTAQRQXV2YS80-task-add-support-bundle-facts-for-repeated-same` as `8b091799d86b`.

Open questions / Risiken
- Because current typed mapper docs and generator diagnostics still enforce unique participant names, implementers may accidentally widen mapper or runtime behavior inside this ticket; keeping this ticket fact-only avoids mixing support-bundle contract work with public mapper co...
- Additive support-bundle changes must remain backward compatible for existing consumers that only read current `diagnostics.explain.entities[].properties[]` shapes.
- If exported facts rely on provider-specific produced names without also preserving logical participant identity, downstream generator work could still be ambiguous for repeated same-hub roles or future naming-policy changes.
- Split recommendation: No split is required for the support-bundle fact work itself; it is a bounded additive explain-contract change with focused tests.
- Split recommendation: If same-hub typed mapper emission is also desired, keep it as a separate child or follow-up ticket that consumes these new facts and updates the public mapper or generator contract independently.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9451`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `5df2f8d8b6ec4b289eb80ad454afbbed`
- completed-at-utc: `<redacted>-26T09:13:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43Y6JE9NQWTAQRQXV2YS80/runs/20260626T091306439Z-5df2f8d8b6ec4b289eb80ad454afbbed.json`