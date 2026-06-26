[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43Y6JE9NQWTAQRQXV2YS80'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43Y6JE9NQWTAQRQXV2YS80`.
- Optimistic claim succeeded (`expectedRevision=06FG7C0QKAZ6MX88ECH5ZDTKYR`, `currentRevision=06FG7CAGJKWABGW9KA87TR71WC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43Y6JE9NQWTAQRQXV2YS80': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43Y6JE9NQWTAQRQXV2YS80': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43Y6JE9NQWTAQRQXV2YS80-task-add-support-bundle-facts-for-repeated-same' from source '9ce8b6beee3bfbf8f83f423fe89805b2305b9ae6'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43Y6JE9NQWTAQRQXV2YS80-task-add-support-bundle-facts-for-repeated-same` as `e7db2392accc`.

Open questions / Risiken
- Because the visible public explain surface currently has no ordered participant facts, the additive shape must remain backward compatible for existing support-bundle consumers.
- If implementation exports only produced names and omits logical participant role/name, repeated same-hub links remain ambiguous under future naming-policy changes.
- Current public typed link-mapping evidence is still unique-participant-only, so developers may accidentally widen mapper scope unless this ticket stays fact-only.
- Split recommendation: No split is required for the support-bundle fact work itself; the bounded branch evidence supports one additive explain-contract ticket.
- Split recommendation: If same-hub typed mapper emission is desired later, keep it as a separate follow-up ticket that consumes these new facts and updates mapper or generator contracts independently.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8883`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `43e5abd79e6a4c0a859276ef5dd86639`
- completed-at-utc: `<redacted>-26T11:34:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43Y6JE9NQWTAQRQXV2YS80/runs/20260626T113405426Z-43e5abd79e6a4c0a859276ef5dd86639.json`