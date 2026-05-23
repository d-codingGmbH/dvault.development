[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F492C50WM7V2NE0WZB3774XM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492C50WM7V2NE0WZB3774XM`.
- Optimistic claim succeeded (`expectedRevision=06F5AJGYRJ5ZJDSG1H1N097N4M`, `currentRevision=06F5AJZ3GF181QTGH30PW9QFG0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F492C50WM7V2NE0WZB3774XM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F492C50WM7V2NE0WZB3774XM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' from source '23c13d24f66be4c34fa4363ef643ea67919c1b68'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an` as `f82145c7fabe`.

Open questions / Risiken
- If implementation routes request-bound work through the wrong service or replaces the existing ReadShape carrier instead of extending it additively, current consumers may break.
- If projection role names vary by provider or request path, explicit and registry-backed diagnostics become harder to compare; keep role names deterministic and provider-neutral.
- If downstream readers follow the unchanged title instead of the delivery contract, they may assume net-new index-hint work that is out of scope until metadata cleanup occurs.
- Split recommendation: No split is required. This remains one bounded additive read-shape diagnostics extension on top of the existing request-bound baseline already visible in the current branch.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `62994`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0386`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `ac767b2146994f5e957147abc5bb7b0e`
- completed-at-utc: `<redacted>-23T14:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492C50WM7V2NE0WZB3774XM/runs/20260523T145509806Z-ac767b2146994f5e957147abc5bb7b0e.json`