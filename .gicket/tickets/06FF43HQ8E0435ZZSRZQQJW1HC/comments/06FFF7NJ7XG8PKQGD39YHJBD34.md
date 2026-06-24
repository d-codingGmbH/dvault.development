[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43HQ8E0435ZZSRZQQJW1HC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43HQ8E0435ZZSRZQQJW1HC`.
- Optimistic claim succeeded (`expectedRevision=06FF44KFVG9XKJAGAGJG8V8A68`, `currentRevision=06FFF55WSTT14SJ8XWA02RQEE0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43HQ8E0435ZZSRZQQJW1HC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43HQ8E0435ZZSRZQQJW1HC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa' from source '4fb2e9625b89ee473d3ae584335166e93bd5aa96'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43HQ8E0435ZZSRZQQJW1HC-task-harden-pit-maintenance-unsupported-shape-fa` as `10e88b8bd4a0`.

Open questions / Risiken
- If PostgreSQL fallback proof relies only on opt-in live-provider integration, CI signal will remain weaker than deterministic unit coverage.
- SQL Server uses service replacement while PostgreSQL uses strategy registration, so missing-registration coverage must be set up deliberately or tests may accidentally recheck provider mismatch instead of the intended registration boundary.
- The PIT maintenance code exposes fewer established diagnostics hooks for missing registration than save and read flows, so test design should avoid accidental scope creep into new public diagnostics behavior.
- Split recommendation: No split recommended; the remaining work is a bounded hardening pass across already-existing PIT maintenance test surfaces.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8806`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f61efea9def443eb87ad35bd7563fda4`
- completed-at-utc: `<redacted>-24T03:13:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43HQ8E0435ZZSRZQQJW1HC/runs/20260624T031353207Z-f61efea9def443eb87ad35bd7563fda4.json`