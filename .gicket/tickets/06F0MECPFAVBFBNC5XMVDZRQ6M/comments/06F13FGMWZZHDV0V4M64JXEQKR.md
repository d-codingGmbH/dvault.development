[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MECPFAVBFBNC5XMVDZRQ6M'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MECPFAVBFBNC5XMVDZRQ6M`.
- Optimistic claim succeeded (`expectedRevision=06F13BW7V708T620A8WY9288AR`, `currentRevision=06F13C3V6MGYSHP9364XAD8F5W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MECPFAVBFBNC5XMVDZRQ6M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MECPFAVBFBNC5XMVDZRQ6M': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p' from source '00d3db273667fe54de336bf51e40981f49b62f69'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p` as `960a5dd0f988`.

Open questions / Risiken
- If implementation projects only from `DataVaultSatelliteReadRecord`, required/null diagnostics will still disappear behind the current silent-skip behavior.
- If explicit and registry-backed typed overloads diverge instead of sharing one projector pipeline, latest/as-of parity or diagnostic wording can drift.
- If reserved-name validation is omitted, a satellite payload or driving key named `HashDiff`, `LoadTimestamp`, `RecordSource`, or `ParentHashKey` will leave the exact-name contract ambiguous.
- If failureKind tokens or message prefix vary across paths, tests and callers lose the deterministic diagnostic contract this ticket is meant to add.
- Split recommendation: No split recommended. Repository evidence still bounds this work to one additive typed-read helper layer, deterministic diagnostics, and tests, and no child tickets or planning documents were materialized in this pass.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9471`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1ef0c1110064487b94505bd330f3156a`
- completed-at-utc: `<redacted>-10T11:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MECPFAVBFBNC5XMVDZRQ6M/runs/20260510T115529934Z-1ef0c1110064487b94505bd330f3156a.json`