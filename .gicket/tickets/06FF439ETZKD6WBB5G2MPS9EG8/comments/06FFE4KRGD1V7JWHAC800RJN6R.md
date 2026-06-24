[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF439ETZKD6WBB5G2MPS9EG8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF439ETZKD6WBB5G2MPS9EG8`.
- Optimistic claim succeeded (`expectedRevision=06FF44HNJWM5NAF6DDFA4NJBFM`, `currentRevision=06FFE3EPTNPVPQ8YZ9F60B404C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF439ETZKD6WBB5G2MPS9EG8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF439ETZKD6WBB5G2MPS9EG8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi' from source '409f63421f9aa4a776c43d0ae5986ed9991ddb79'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi` as `fd6b77034bb3`.

Open questions / Risiken
- If only one of the two live docs is updated, the remaining surface can still let readers infer that completed read rows prove provider-maintenance timing.
- Citing the 2026-06-23 provider optimization closure bundle without the maintained-row disclaimer could reintroduce confusion between read-side evidence and maintenance-side evidence.
- Expanding wording beyond the existing v0.45.0 maintenance boundary could accidentally imply benchmark-backed PIT maintenance claims that the repository does not currently prove.
- Split recommendation: No split recommended; the current branch evidence bounds this to one documentation-alignment task across the performance and architecture surfaces.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `87040`
- effective-cache-ratio: `0.4911`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `3f15b37bf7314959b03847fd898a45ce`
- completed-at-utc: `<redacted>-24T00:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF439ETZKD6WBB5G2MPS9EG8/runs/20260624T004043388Z-3f15b37bf7314959b03847fd898a45ce.json`