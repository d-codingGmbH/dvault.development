[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8RGQZA7D9JZSTSAJEM9B3M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RGQZA7D9JZSTSAJEM9B3M`.
- Optimistic claim succeeded (`expectedRevision=06FH8SM8N2EAP6HJ3X80KS5K4R`, `currentRevision=06FHGWH62HZR3BNXS7VMK8AYQC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8RGQZA7D9JZSTSAJEM9B3M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8RGQZA7D9JZSTSAJEM9B3M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8RGQZA7D9JZSTSAJEM9B3M-task-define-provider-native-crypto-capability-co' from source 'f1c4b84bf7530885f920ebfff8bb30c1c5d12566'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FH8RGQZA7D9JZSTSAJEM9B3M-task-define-provider-native-crypto-capability-co` as `2988ad4c15bb`.

Open questions / Risiken
- The ticket title can invite over-scoping; without the documented boundary, implementers may incorrectly treat it as approval for shared provider-native encryption support.
- Documentation drift between architecture, release notes, and diagnostics could reintroduce accidental compliance or key-lifecycle claims even though the repository baseline rejects them.
- A future provider-specific optimization could accidentally bypass fail-closed behavior or alias-driven ownership unless it stays behind a separate provider ticket with its own diagnostics and evidence.
- Split recommendation: Keep any future provider-native encryption work split into one provider and one exact capability per ticket, with its own provider package surface, fallback rules, tests, and evidence.
- Split recommendation: If broader privacy workflow APIs are still desired, split them from this contract ticket into separate explicit capabilities such as read-helper redaction, pseudonymization, or retention metadata review rather than widening the shared v1 boundary.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9006`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `5a8f95822b3343e2acdfacce025ceca5`
- completed-at-utc: `<redacted>-30T12:19:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RGQZA7D9JZSTSAJEM9B3M/runs/20260630T121959836Z-5a8f95822b3343e2acdfacce025ceca5.json`