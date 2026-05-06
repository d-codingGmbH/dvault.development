[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NVE88WW9PMM04NVAZHRG0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NVE88WW9PMM04NVAZHRG0`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y4MY60FB3HPN5900BCT4C`, `currentRevision=06EZQEVHK6YWFD9HNAZ3ABKAHR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NVE88WW9PMM04NVAZHRG0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NVE88WW9PMM04NVAZHRG0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar' from source '62a1d8a493188f3bc3c2d4939ccbba7a6758cbca'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar` as `2f737c2057f8`.

Open questions / Risiken
- If the parent bridge implementation changes its generated shape late in development, the documentation example and unsupported-pattern notes will need a final consistency pass.
- Because bridge behavior is explicitly deferred and opt-in at v0.5, the documentation must avoid overstating hierarchy depth, maintenance semantics, or provider-specific guarantees that the implementation does not prove.
- Split recommendation: No split recommended; the current task is already a bounded documentation/example child under bridge story `06EZ0NTV4SVAKV98C418T8A3CC`.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `47197`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0515`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f8201e9d01e24caabaf0056b9398dcb9`
- completed-at-utc: `<redacted>-06T05:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/runs/20260506T052640456Z-f8201e9d01e24caabaf0056b9398dcb9.json`