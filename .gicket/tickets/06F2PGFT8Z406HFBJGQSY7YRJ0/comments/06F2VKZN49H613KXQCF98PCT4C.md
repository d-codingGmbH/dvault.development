[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGFT8Z406HFBJGQSY7YRJ0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGFT8Z406HFBJGQSY7YRJ0`.
- Optimistic claim succeeded (`expectedRevision=06F2PNGNHRA8TR14BTBQ18E2E4`, `currentRevision=06F2VHEAV8RD0XMH5DKX0RGHYG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGFT8Z406HFBJGQSY7YRJ0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGFT8Z406HFBJGQSY7YRJ0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGFT8Z406HFBJGQSY7YRJ0-epic-design-time-drift-and-ci-guardrails' from source 'ab31a74aeb86d61314ce3b5b9d74f3e53f1fd74d'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGFT8Z406HFBJGQSY7YRJ0-epic-design-time-drift-and-ci-guardrails` as `0632912d352b`.

Open questions / Risiken
- Non-SQLite live-schema validation remains environment-dependent and can regress unnoticed unless consuming applications explicitly enable the opt-in provider lanes.
- If later docs or downstream tickets blur the boundary, users may misread built-in provider readers as DVault-managed database provisioning, CI infrastructure, or automatic migration behavior.
- Future analyzer/generator work could accidentally reopen the v0.11.0 scope unless the explicit consumer-owned command-host and preflight model stays fixed.
- Split recommendation: No additional split is recommended; the epic already has the correct bounded child set in `06F2PGFZWC5PXSDH46RCZPN1CG`, `06F2PGGEY26Y65G97NGFKH381M`, `06F2PGGW8ZBW80V6B8RPWNVM70`, and `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- Split recommendation: Keep the existing downstream analyzer/code-fix/source-generator tickets blocked and separate instead of widening this v0.11.0 design-time/drift epic.
- Split recommendation: Do not materialize new planning docs or relation rewrites unless a later ticket intentionally re-scopes the downstream dependency graph.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9060`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `43e8b0f5715b4f2ba494940492eda455`
- completed-at-utc: `<redacted>-15T22:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGFT8Z406HFBJGQSY7YRJ0/runs/20260515T224423814Z-43e8b0f5715b4f2ba494940492eda455.json`