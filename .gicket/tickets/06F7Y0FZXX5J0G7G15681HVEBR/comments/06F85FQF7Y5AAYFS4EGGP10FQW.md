[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0FZXX5J0G7G15681HVEBR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0FZXX5J0G7G15681HVEBR`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0Y20HCHQKAGK15PPPDGXW`, `currentRevision=06F85DRVFBF0B77XGWPYD8W91C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0FZXX5J0G7G15681HVEBR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0FZXX5J0G7G15681HVEBR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0FZXX5J0G7G15681HVEBR-story-define-redacted-read-plan-explain-v2-contr' from source '5e9fcf2b3175e636089beec0835e79e805cd62ff'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F7Y0FZXX5J0G7G15681HVEBR-story-define-redacted-read-plan-explain-v2-contr` as `1a707dc1e771`.

Open questions / Risiken
- Several rule members such as SeriesSelectionRule, PitRowSelectionRule, SnapshotLookupBehavior, and SupportedEndpointRules are literal explanatory strings; the contract should describe their meaning and bounded purpose without over-promising exact prose stability unless the tea...
- ExpectedIndexBaseline reflects translated metadata baselines, not observed provider execution plans; unclear wording could cause consumers to infer unsupported physical-plan guarantees.
- Current repository evidence shows strong SQLite and provider-neutral fallback coverage; if the public contract starts making stronger cross-provider wording guarantees, additional provider-specific verification may be needed.
- Split recommendation: No split recommended for this ticket. The current repository already provides a bounded baseline, and future raw-SQL capture, automatic-index advisory behavior, or broader generated-helper work should stay in separate additive tickets.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8280`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `0515d0265fea47ff8a3fcb13dc0ce1fa`
- completed-at-utc: `<redacted>-01T10:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0FZXX5J0G7G15681HVEBR/runs/20260601T103331698Z-0515d0265fea47ff8a3fcb13dc0ce1fa.json`