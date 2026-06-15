[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC46047ZF11DR0TTRARM78'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC46047ZF11DR0TTRARM78`.
- Optimistic claim succeeded (`expectedRevision=06FBSCXZE8H3HFY0BPAXB2NVAC`, `currentRevision=06FCPWCAPTPGEPVAT89R2TNQ78`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC46047ZF11DR0TTRARM78': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC46047ZF11DR0TTRARM78': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC46047ZF11DR0TTRARM78-task-add-db2-benchmark-and-test-lane-documentati' from source 'eecde7652b12d401ca1347e3b9ea5a37b542f5c3'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Several canonical docs currently state that no DB2 benchmark lane exists; landing only part of the documentation sweep would leave conflicting guidance.
- DB2 remains an external opt-in dependency, so local or CI environments without a reachable DB2 instance can only validate skipped placeholder behavior, not completed DB2 timing evidence.
- DB2 benchmark execution needs new benchmark-project conditional package restore and a DB2 temp database path; if either is missed, the lane will document support without actually producing matrix rows.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9682`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `3eb03133124d4f4588c508eb15c15663`
- completed-at-utc: `<redacted>-15T13:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC46047ZF11DR0TTRARM78/runs/20260615T133356566Z-3eb03133124d4f4588c508eb15c15663.json`