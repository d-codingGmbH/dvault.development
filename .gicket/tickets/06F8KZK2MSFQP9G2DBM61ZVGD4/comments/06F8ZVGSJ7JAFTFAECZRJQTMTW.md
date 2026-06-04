[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZK2MSFQP9G2DBM61ZVGD4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZK2MSFQP9G2DBM61ZVGD4`.
- Optimistic claim succeeded (`expectedRevision=06F8M0156PSJGZXN4JSNXCDS7M`, `currentRevision=06F8ZSYPS0N1DNJYWEJG4XQ738`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZK2MSFQP9G2DBM61ZVGD4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZK2MSFQP9G2DBM61ZVGD4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZK2MSFQP9G2DBM61ZVGD4-task-add-provider-read-benchmark-rows-and-verifi' from source 'fc5b6d054c5743601a036509cbee772f1ce4eb3f'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZK2MSFQP9G2DBM61ZVGD4-task-add-provider-read-benchmark-rows-and-verifi` as `7122a76d2b64`.

Open questions / Risiken
- Execution sequencing still depends on blocking ticket 06F8KZJNZ999C8NKY0S92VBDN0; refinement is complete, but delivery ordering remains constrained by the live relation state.
- If skipped optional-provider rows are not verifier-protected, later artifact refreshes can silently drop those rows and make provider coverage look narrower than the documented contract.
- If row labels or summaries blur the difference between measured SQLite results and skipped optional-provider lanes, downstream docs or release notes may overstate provider-read optimization evidence.
- Split recommendation: No split recommended; the repository already fixes the scenario baseline, provider posture, and artifact contract, so benchmark-row extension and verifier coverage remain one bounded task.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `41942`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1801`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6cba4b70ebe141ab942448f871455eb7`
- completed-at-utc: `<redacted>-04T00:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZK2MSFQP9G2DBM61ZVGD4/runs/20260604T000006537Z-6cba4b70ebe141ab942448f871455eb7.json`