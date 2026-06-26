[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43Y6JE9NQWTAQRQXV2YS80'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43Y6JE9NQWTAQRQXV2YS80`.
- Optimistic claim succeeded (`expectedRevision=06FG6FVSN939AD4KDZ63EGG6MR`, `currentRevision=06FG75HBSJY0V88ZRF442NZRF4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43Y6JE9NQWTAQRQXV2YS80': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43Y6JE9NQWTAQRQXV2YS80': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43Y6JE9NQWTAQRQXV2YS80-task-add-support-bundle-facts-for-repeated-same' from source '05527103020e43eae35dac3fb4874ee9bd190425'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43Y6JE9NQWTAQRQXV2YS80-task-add-support-bundle-facts-for-repeated-same` as `ca496c6bc5b2`.

Open questions / Risiken
- Because the current public explain surface has no participant descriptor, the new additive shape must remain backward compatible for existing support-bundle consumers.
- If implementation keys only off produced property names and omits logical participant identity, repeated same-hub roles remain ambiguous under future naming-policy changes.
- Current `IDataVaultLinkMapper` and `DataVaultLinkParticipantBindingAttribute` evidence still models unique participant hub names, so developers may accidentally widen mapper scope unless this ticket stays fact-only.
- Split recommendation: No split is required for the support-bundle fact work itself; the bounded branch evidence supports one additive explain-contract ticket.
- Split recommendation: If same-hub typed mapper emission is desired later, keep it as a separate follow-up ticket that consumes these new facts and updates mapper or generator contracts independently.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8906`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d72e0a8b207d4a1dae67b8c86c5cc3c4`
- completed-at-utc: `<redacted>-26T11:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43Y6JE9NQWTAQRQXV2YS80/runs/20260626T110625093Z-d72e0a8b207d4a1dae67b8c86c5cc3c4.json`