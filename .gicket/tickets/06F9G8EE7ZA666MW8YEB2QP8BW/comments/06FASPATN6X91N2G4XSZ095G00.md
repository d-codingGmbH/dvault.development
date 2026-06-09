[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F9G8EE7ZA666MW8YEB2QP8BW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9G8EE7ZA666MW8YEB2QP8BW`.
- Optimistic claim succeeded (`expectedRevision=06F9GF35JYK22WHBB8SXQ853N4`, `currentRevision=06FASM3NYGFKRBYJ62MNWH9ASC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F9G8EE7ZA666MW8YEB2QP8BW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F9G8EE7ZA666MW8YEB2QP8BW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F9G8EE7ZA666MW8YEB2QP8BW-epic-net-8-and-ef-core-compatibility-matrix' from source '4eeea2fe583f2138dc183edbd02cc7e9b0dc37e2'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F9G8EE7ZA666MW8YEB2QP8BW-epic-net-8-and-ef-core-compatibility-matrix` as `5e3327400d0a`.

Open questions / Risiken
- The live relation graph still shows this epic blocking 06F9G8GS08VNH0DT09Q4PC2HRC, so workflow views may temporarily overstate an active blocker until relation cleanup is replayed.
- Helper projects and examples remain net10.0-only by design; if later documentation stops restating that boundary, contributors may misread them as accidental compatibility gaps.
- Because the same package IDs span historical 0.x versions and the new 8.x/10.x consumer lines, stale install guidance can still cause line-mixing confusion if docs and verifier rules drift.
- Split recommendation: No further split is recommended. The epic already has the needed materialized child decomposition: 06F9G8EQJGBRSWE96VE028HJYW, 06F9GF2Z4Y7A91ZHG4NW1YTNMC, 06F9G8EXXFJJ1SWWQXC2N9P2X8, 06F9G8F4RQ0T7RV82M3H2H3FVG, 06F9G8FBQTAPXXS1Y4NR5QKVG8, and 06F9G8FJMZ3A...
- Split recommendation: Keep 06F9G8GS08VNH0DT09Q4PC2HRC as downstream follow-on work rather than expanding this epic further.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8898`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `8b43ca517717447998f9ff755de328ba`
- completed-at-utc: `<redacted>-09T14:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9G8EE7ZA666MW8YEB2QP8BW/runs/20260609T144626213Z-8b43ca517717447998f9ff755de328ba.json`