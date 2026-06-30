[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8RJF2SYBJ8ZM7ZDETDPN78'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RJF2SYBJ8ZM7ZDETDPN78`.
- Optimistic claim succeeded (`expectedRevision=06FH8SMPA3TP2KVYZ4TP5A5Z4C`, `currentRevision=06FHHXW94VKQKERE21W5WZH3KR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8RJF2SYBJ8ZM7ZDETDPN78': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8RJF2SYBJ8ZM7ZDETDPN78': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8RJF2SYBJ8ZM7ZDETDPN78-task-expose-provider-crypto-capability-facts-fro' from source '11fd879c002b3744a31178ad7af9161b3b3e4b0c'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FH8RJF2SYBJ8ZM7ZDETDPN78-task-expose-provider-crypto-capability-facts-fro` as `7eb27fe66292`.

Open questions / Risiken
- Consumers may misread a supported capability row as DVault runtime support or environment activation unless the fact model and docs keep the unmanaged guidance-only boundary explicit.
- Generic diagnostics already default unknown providers to the SQLite storage profile for some explain paths; this ticket must avoid reusing that fallback for provider-native crypto facts.
- Mixing deployment-at-rest features and SQL-function features in one static matrix can create false equivalence unless the reported capability family makes the distinction explicit.
- Provider docs or package baselines can drift over time; without checked-in tests per provider row, the static matrix could become stale or contradictory.
- Split recommendation: Keep provider-native runtime activation or conversion behavior split by one provider and one exact capability per ticket after this discovery/reporting slice.
- Split recommendation: Keep consumer-facing configuration and selection behavior in existing ticket 06FH8RKDJTS3BB11J6J6QJVVD4 rather than expanding this ticket.
- Split recommendation: Keep docs rollout in existing ticket 06FH8RMZPSZ7H3AQRP8FX72S08 rather than widening this diagnostics ticket.
- Split recommendation: If optional live probing is ever desired, split it into a later opt-in diagnostics ticket with its own redaction and secret-handling review.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9090`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4eb0415baf614418a351ee9e69a0371f`
- completed-at-utc: `<redacted>-30T14:48:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RJF2SYBJ8ZM7ZDETDPN78/runs/20260630T144831905Z-4eb0415baf614418a351ee9e69a0371f.json`