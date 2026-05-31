[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q9463M0RSHAJJX0F3D1DB0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q9463M0RSHAJJX0F3D1DB0`.
- Optimistic claim succeeded (`expectedRevision=06F72ZPVTQPHG8YQXHVZPPTXB8`, `currentRevision=06F7QD7J69FBEHBSSZTEV0DYA4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q9463M0RSHAJJX0F3D1DB0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q9463M0RSHAJJX0F3D1DB0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope' from source 'bd7deb707900eb5a639d88669662d5e4b6228eb7'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope` as `be4cc4f0941e`.

Open questions / Risiken
- If bridge tracing is added only inside DefaultDataVaultReadService, callers that hit the bridge extension fallback branch could miss spans; the implementation should explicitly verify both public bridge helper paths.
- If tag/event construction happens before listener or sampling checks, the implementation can violate the contract's minimal-overhead no-listener baseline even when StartActivity returns null.
- A public ActivitySource holder would change the core package API snapshot and should be avoided unless repository API policy requires public exposure.
- Split recommendation: No further split is recommended; save/read tracing is already cleanly separated from PIT and bridge maintenance tracing in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8229`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `2f62db597f4b41acace5cba36862e54c`
- completed-at-utc: `<redacted>-31T01:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/runs/20260531T015648514Z-2f62db597f4b41acace5cba36862e54c.json`