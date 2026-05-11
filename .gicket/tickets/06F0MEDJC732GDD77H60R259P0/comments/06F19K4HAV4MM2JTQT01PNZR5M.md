[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEDJC732GDD77H60R259P0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEDJC732GDD77H60R259P0`.
- Optimistic claim succeeded (`expectedRevision=06F19JMNTW7PPGPWJM5RAJB4F8`, `currentRevision=06F19JQKBD0YBTVQYT7YG36JTW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEDJC732GDD77H60R259P0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEDJC732GDD77H60R259P0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u' from source '924fd02172e0402d13d289c9d8b27fee3c908de8'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u` as `0930df53cbf3`.

Open questions / Risiken
- Returning directly to tester would repeat the known package-verification blocker.
- Running dev again in the same restricted runner will not satisfy the validation gate.
- Approving a generic dev handoff without a capable-runner guarantee could loop back to PO-critic.
- Bypassing the validation gate through repository edits would violate the approved scope.
- Split recommendation: No split recommended now. Split only if capable-runner output proves a real packaging defect that needs separate remediation.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `56415`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0431`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `675c796a754a43538fd4838fad079acd`
- completed-at-utc: `<redacted>-11T02:10:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEDJC732GDD77H60R259P0/runs/20260511T021010774Z-675c796a754a43538fd4838fad079acd.json`