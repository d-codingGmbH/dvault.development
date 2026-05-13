[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEDTB8496GYVM9K42F9VPG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEDTB8496GYVM9K42F9VPG`.
- Optimistic claim succeeded (`expectedRevision=06F0QH499S5PQFBGG4E386M610`, `currentRevision=06F1X7TFDPM2Z2YFY0AJ6Z7CK4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEDTB8496GYVM9K42F9VPG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEDTB8496GYVM9K42F9VPG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEDTB8496GYVM9K42F9VPG-epic-model-first-specifications-and-advanced-rea' from source '7e80fc80c7aa871a214ab4006cc261abeab18edd'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEDTB8496GYVM9K42F9VPG-epic-model-first-specifications-and-advanced-rea` as `8ce3057ed31d`.

Open questions / Risiken
- Historical docs such as docs/releases/v0.6.0.md can still be misread as current capability posture unless reviewers anchor on docs/releases/v0.7.0.md and the refined child-ticket contracts.
- Consumers may overread the advanced read-model baseline as including PIT refresh, bridge maintenance, or unbounded traversal unless downstream docs continue to keep those boundaries explicit.
- Provider-aware optimization claims should stay tied to benchmarked, branch-visible evidence so the epic is not interpreted as blanket optimization coverage for every provider or read shape.
- Split recommendation: No further split is recommended. The epic is already decomposed into four done child tickets covering model-first import, export/drift tooling, PIT/bridge read helpers, and provider-aware optimization follow-up.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `49562`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0491`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `063998d063b847fd8edf44ad4c3099a9`
- completed-at-utc: `<redacted>-13T00:05:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEDTB8496GYVM9K42F9VPG/runs/20260513T000523337Z-063998d063b847fd8edf44ad4c3099a9.json`