[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB75NX7Z0DY7X0BD0YFZECM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB75NX7Z0DY7X0BD0YFZECM`.
- Optimistic claim succeeded (`expectedRevision=06EXBF7D6VD3VST4PYQBA3ZTD0`, `currentRevision=06EXBQNYH03DY7FEHNANH2JRFR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB75NX7Z0DY7X0BD0YFZECM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB75NX7Z0DY7X0BD0YFZECM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB75NX7Z0DY7X0BD0YFZECM-task-define-default-table-and-column-naming-poli' from source 'b8fd4e59ca571dc1aa0b179ed4a92805c702a30e'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB75NX7Z0DY7X0BD0YFZECM-task-define-default-table-and-column-naming-poli` as `29096d6e7b57`.

Open questions / Risiken
- The technical metadata column contract is a sibling ticket; if it changes the canonical technical fields, the naming examples and tests here must be kept aligned.
- Expanding reserved-word handling into provider-specific catalogs or full SQL quoting would broaden this task beyond the provider-neutral v1 policy.
- Full linguistic singularization can introduce surprising behavior; documenting finite rules keeps v1 deterministic but may require follow-up for irregular domain terms.
- Split recommendation: No split recommended for this ticket. Override points and technical metadata contracts already exist as separate sibling tasks, so this ticket should stay focused on the default policy and its examples/tests.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8394`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1e9aeb0e045347e389d3fffbf9c26fb6`
- completed-at-utc: `<redacted>-28T20:58:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB75NX7Z0DY7X0BD0YFZECM/runs/20260428T205836840Z-1e9aeb0e045347e389d3fffbf9c26fb6.json`