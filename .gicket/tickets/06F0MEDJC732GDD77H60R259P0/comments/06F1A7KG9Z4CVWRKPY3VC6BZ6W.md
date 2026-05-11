[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEDJC732GDD77H60R259P0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEDJC732GDD77H60R259P0`.
- Optimistic claim succeeded (`expectedRevision=06F1A6420VYR8SNZWDMZR97M0G`, `currentRevision=06F1A6NAPNCFRWVVPGGAGM0YSW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEDJC732GDD77H60R259P0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEDJC732GDD77H60R259P0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEDJC732GDD77H60R259P0-task-update-readme-and-release-docs-for-v0-6-0-u' from source '9f098d7bc5216a8b1c51e0327f8d89638d7df95f'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Routing package validation back to a restricted cache-incomplete runner would repeat the known blocker.
- Reviewers may confuse forward-looking README 0.6.0 install guidance with pre-tag MinVer prerelease artifact filenames; the contract separates those concerns.
- Final package publication remains outside this ticket and still requires the release operator's audited approval.
- Split recommendation: No split is recommended now because capable-runner validation already exists and satisfies the current pre-tag package-validation contract.
- Split recommendation: Split only a future non-MinVer packaging or verifier defect with concrete failing capable-runner output.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `54584`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0446`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1af61acd0a6e4b148073f118f8adc65e`
- completed-at-utc: `<redacted>-11T03:39:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEDJC732GDD77H60R259P0/runs/20260511T033935987Z-1af61acd0a6e4b148073f118f8adc65e.json`