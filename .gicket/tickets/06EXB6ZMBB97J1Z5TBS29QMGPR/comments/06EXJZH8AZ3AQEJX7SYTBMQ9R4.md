[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB6ZMBB97J1Z5TBS29QMGPR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6ZMBB97J1Z5TBS29QMGPR`.
- Optimistic claim succeeded (`expectedRevision=06EXJS5GA2YDB7A2CR5QHDY43G`, `currentRevision=06EXJYH8QXSWVKYYSK52152WF4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB6ZMBB97J1Z5TBS29QMGPR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB6ZMBB97J1Z5TBS29QMGPR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB6ZMBB97J1Z5TBS29QMGPR-task-add-smoke-tests-for-minimal-startup' from source '33e014d0752db18a9c3d6b6e5bfbd3baf729416c'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB6ZMBB97J1Z5TBS29QMGPR-task-add-smoke-tests-for-minimal-startup` as `e3ca25624581`.

Open questions / Risiken
- A test that asserts private DI registration mechanics instead of public startup success may become brittle.
- If later product direction makes UseDataVault mandatory for the minimal startup story, that should be handled as a separate API/integration ticket before expanding this smoke test.
- Split recommendation: No prerequisite setup split is required for the current narrowed AddDVault smoke-test scope because the branch now contains the source project, AddDVault entry point, and test structure.
- Split recommendation: Create a separate follow-up ticket only if UseDataVault, EF-specific startup wiring, or provider integration becomes part of the required public startup surface.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `53975`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0451`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `09235876dd28407cb50cd048564186b7`
- completed-at-utc: `<redacted>-29T13:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6ZMBB97J1Z5TBS29QMGPR/runs/20260429T134656363Z-09235876dd28407cb50cd048564186b7.json`