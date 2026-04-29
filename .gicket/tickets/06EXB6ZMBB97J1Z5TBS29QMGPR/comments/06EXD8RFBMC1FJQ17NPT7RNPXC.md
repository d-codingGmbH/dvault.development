[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB6ZMBB97J1Z5TBS29QMGPR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6ZMBB97J1Z5TBS29QMGPR`.
- Optimistic claim succeeded (`expectedRevision=06EXC38PD2NANBXGSYKJ106Y0G`, `currentRevision=06EXD88NCH0HA16VM07CC6JCC8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB6ZMBB97J1Z5TBS29QMGPR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB6ZMBB97J1Z5TBS29QMGPR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB6ZMBB97J1Z5TBS29QMGPR-task-add-smoke-tests-for-minimal-startup' from source 'd7755c52fc0a5bacb265becd1010e2ca8d166324'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB6ZMBB97J1Z5TBS29QMGPR-task-add-smoke-tests-for-minimal-startup` as `17d5bfb25606`.

Open questions / Risiken
- Implementing this ticket before the public API and test scaffold exist would expand it into implicit architecture and repository setup work.
- A smoke test that asserts internal registration or EF convention details instead of observable minimal startup success may become brittle.
- Split recommendation: No split is needed for the smoke-test itself once prerequisites exist.
- Split recommendation: If the repository still has no solution, source project, test project, or test command at dev start, split or block on a separate foundation/setup ticket rather than broadening this task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `29843`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0815`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1e05f23fde304aa09b8ae7f3a70a6c6b`
- completed-at-utc: `<redacted>-29T00:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6ZMBB97J1Z5TBS29QMGPR/runs/20260429T002823174Z-1e05f23fde304aa09b8ae7f3a70a6c6b.json`