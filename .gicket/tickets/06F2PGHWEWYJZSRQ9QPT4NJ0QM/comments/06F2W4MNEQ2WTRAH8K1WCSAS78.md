[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGHWEWYJZSRQ9QPT4NJ0QM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGHWEWYJZSRQ9QPT4NJ0QM`.
- Optimistic claim succeeded (`expectedRevision=06F2PNJQXR1DN60AWP11BHVHFC`, `currentRevision=06F2W2QT8PQ6QX0NC9TWS1ERTR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGHWEWYJZSRQ9QPT4NJ0QM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGHWEWYJZSRQ9QPT4NJ0QM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGHWEWYJZSRQ9QPT4NJ0QM-task-implement-high-confidence-analyzer-rules' from source '00f41117660f67e9451c68ac505324a867cc41ca'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGHWEWYJZSRQ9QPT4NJ0QM-task-implement-high-confidence-analyzer-rules` as `53a7443497df`.

Open questions / Risiken
- Because the repository already contains this analyzer slice while the ticket is still planned under the `v0.12.0` graph, later work could accidentally reopen or over-expand a slice that is already historically documented unless the two-diagnostic boundary stays explicit.
- No `docs/releases/v0.12.0.md` file exists yet, so future release-note work could omit or duplicate the analyzer baseline if documentation ownership is not kept with the downstream docs ticket.
- If a developer treats this task as an invitation to add code fixes, broader model validation, or dataflow analysis, the current high-confidence, low-noise analyzer contract will expand beyond the intended scope.
- Split recommendation: No additional split is recommended; the current task is already a bounded implementation slice and adjacent work is already separated into analyzer documentation task `06F2PGJ28KVSZAAFRA40D94128`, downstream code-fix story `06F2PGJBRXFCP038CN6XVAYSZM`, an...
- Split recommendation: Do not create child tickets for broader diagnostics, code fixes, or generator work from this task; keep those as separate follow-on scopes.
- Split recommendation: Do not materialize relation cleanup from this pass; the live relation state is already coherent for a closure-style refinement of this ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9371`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b8d593efdc4e49e983b81188022c9b61`
- completed-at-utc: `<redacted>-15T23:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGHWEWYJZSRQ9QPT4NJ0QM/runs/20260515T235710191Z-b8d593efdc4e49e983b81188022c9b61.json`