[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q9463M0RSHAJJX0F3D1DB0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q9463M0RSHAJJX0F3D1DB0`.
- Optimistic claim succeeded (`expectedRevision=06F7S7VT8TZXX9772SH0F8RC4C`, `currentRevision=06F7S851RE2YKD9WXGEEVGGJZW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q9463M0RSHAJJX0F3D1DB0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q9463M0RSHAJJX0F3D1DB0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope' from source '6c0b284f3acd5da8878864f479be8d533946a9a0'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope` as `2c03a9b83093`.

Open questions / Risiken
- If tracing is added only around ReadLatestSatelliteRowsAsync, typed or helper-based latest-satellite executions can bypass that hook and miss spans because they may execute through IDataVaultSatelliteProjectionReadService or DataVaultSatelliteReadPipeline.
- If bridge tracing is added only inside one branch, callers that hit the other branch in DataVaultReadServiceBridgeExtensions can miss dvault.read.bridge spans.
- If helper layers add wrapper spans instead of reusing the underlying explicit or projection execution path, duplicate root spans can leak into listener output.
- If tag or event construction happens before listener or sampling checks, the implementation can violate the no-listener minimal-overhead baseline even when StartActivity returns null.
- Split recommendation: No split is recommended; current branch evidence supports one bounded story for explicit save/read tracing, while PIT and bridge maintenance tracing remains separate in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `81501`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0298`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1c0cee1119e74592819a84c273de150e`
- completed-at-utc: `<redacted>-31T06:13:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/runs/20260531T061328943Z-1c0cee1119e74592819a84c273de150e.json`