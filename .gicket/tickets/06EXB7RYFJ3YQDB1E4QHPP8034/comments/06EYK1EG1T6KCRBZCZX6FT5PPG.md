[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7RYFJ3YQDB1E4QHPP8034'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7RYFJ3YQDB1E4QHPP8034`.
- Optimistic claim succeeded (`expectedRevision=06EYJEJW4KSP80BYHHEBF06K4G`, `currentRevision=06EYJYMQNECCMM0XDW955HP2CR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7RYFJ3YQDB1E4QHPP8034': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7RYFJ3YQDB1E4QHPP8034': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7RYFJ3YQDB1E4QHPP8034-task-implement-normal-ef-baseline-for-customer-p' from source 'cb4193f9433ab6983db02b0bba55d5d2e61df21e'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7RYFJ3YQDB1E4QHPP8034-task-implement-normal-ef-baseline-for-customer-p` as `73132b9145e4`.

Open questions / Risiken
- Comparison value drops if the plain EF implementation inserts convenience rows or models more than the exact two-event shared baseline.
- If the paired DVault ticket diverges from `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md`, later side-by-side evaluation will lose fidelity.
- Split recommendation: Keep any runnable example or broader demo separate; this ticket should stay focused on the automated plain EF baseline and the locked comparison contract.
- Split recommendation: If stakeholders later want additional change-history variants or replay/deduplication cases, schedule them as separate follow-up tickets instead of widening this v1 baseline.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9586`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `442200af94894f0eb84bebd414cd5ed1`
- completed-at-utc: `<redacted>-02T16:29:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/runs/20260502T162913416Z-442200af94894f0eb84bebd414cd5ed1.json`