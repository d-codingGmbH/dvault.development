[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4QQTS5NFAYN39KP4QW2424'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QQTS5NFAYN39KP4QW2424`.
- Optimistic claim succeeded (`expectedRevision=06FE4QTEDPQ7WW948PG948JVB0`, `currentRevision=06FE7763XREXW4J7B13AREDXNR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4QQTS5NFAYN39KP4QW2424': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4QQTS5NFAYN39KP4QW2424': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4QQTS5NFAYN39KP4QW2424-task-tune-oracle-pit-read-outlier' from source 'cfdb4afb57e20b7f2d83e0c5fc6dca428b738ed6'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- 2 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The current public Oracle PIT claim is based on configured smoke-style artifact triplets with `iterations=1` and `warmupIterations=0`; overclaiming beyond that preserved run context would be misleading.
- Oracle PIT and Oracle bridge share the same strategy registration but not the same hotspot profile; a PIT-only tune must avoid unintentionally changing bridge behavior or widening scope into separate bridge work.
- Because the PIT read implementation is parity-driven across providers, an Oracle-specific optimization could introduce behavior drift unless parity and fallback checks stay green.
- Split recommendation: No new split is needed: Oracle latest-satellite is already separated in done ticket `06FE4QQJCJH7J9AWQTPDR5DSSG`, and coordinated documentation propagation is already separated in blocked ticket `06FE4QRMXVGJVA65ZR5MZ817K8`.
- Split recommendation: Keep this ticket focused on the Oracle PIT hotspot only; do not absorb Oracle bridge or cross-provider tuning unless a later benchmark shows a new distinct hotspot.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9527`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `bd5b5b0ea8a948779db9a1a27da917e1`
- completed-at-utc: `<redacted>-20T06:20:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QQTS5NFAYN39KP4QW2424/runs/20260620T062006563Z-bd5b5b0ea8a948779db9a1a27da917e1.json`