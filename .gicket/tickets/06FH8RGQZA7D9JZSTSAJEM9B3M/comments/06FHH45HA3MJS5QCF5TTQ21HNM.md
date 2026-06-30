[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8RGQZA7D9JZSTSAJEM9B3M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RGQZA7D9JZSTSAJEM9B3M`.
- Optimistic claim succeeded (`expectedRevision=06FHH1TYWS6B6HMTXP9D202738`, `currentRevision=06FHH26WA1WB8SDXDRX7X7MC4G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8RGQZA7D9JZSTSAJEM9B3M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8RGQZA7D9JZSTSAJEM9B3M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8RGQZA7D9JZSTSAJEM9B3M-task-define-provider-native-crypto-capability-co' from source '9ca9a0936e6552caf3046c7aa9d6278a02033f77'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FH8RGQZA7D9JZSTSAJEM9B3M-task-define-provider-native-crypto-capability-co` as `8ea5d3768476`.

Open questions / Risiken
- The ticket title can still invite over-scoping unless implementers treat DataVaultProviderNativeEncryptionBoundaryFact as evidence-only and keep provider-native work split into separate provider tickets.
- Documentation drift between architecture, release notes, and code-surface naming could reintroduce the old incorrect privacy-package path or imply managed native encryption support.
- A future provider optimization could bypass alias ownership or fail-closed posture if it is added outside a separate provider-scoped capability ticket.
- Split recommendation: Keep any future provider-native encryption work split to one provider and one exact capability per ticket, with its own provider package surface, fallback rules, tests, and evidence.
- Split recommendation: Split broader privacy workflow APIs such as read-helper redaction, pseudonymization flows, or retention metadata review into separate tickets instead of widening this contract.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `41143`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0591`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `41e61e65e0554461949e28051fefd440`
- completed-at-utc: `<redacted>-30T12:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RGQZA7D9JZSTSAJEM9B3M/runs/20260630T124603596Z-41e61e65e0554461949e28051fefd440.json`