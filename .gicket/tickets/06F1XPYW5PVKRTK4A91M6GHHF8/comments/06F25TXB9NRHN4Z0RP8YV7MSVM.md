[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F1XPYW5PVKRTK4A91M6GHHF8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XPYW5PVKRTK4A91M6GHHF8`.
- Optimistic claim succeeded (`expectedRevision=06F25SJ5QC9M60GKFNJHDJAD30`, `currentRevision=06F25SQD0VJ00K8E60EQVBS7H0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XPYW5PVKRTK4A91M6GHHF8-task-add-compiled-query-model-compatibility-test' from source '800f96856abbd7cfbd0dc74cf8e0a7f534e7733c'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F1XPYW5PVKRTK4A91M6GHHF8-task-add-compiled-query-model-compatibility-test` as `e65bf2cf9307`.

Open questions / Risiken
- Risky assumption: The developer must choose a read path that is actually expressible through EF Core compiled query APIs; the contract permits this by requiring an already-supported deterministic read/query surface and forbidding new query APIs.
- Risky assumption: The compiled model proof should use a checked-in deterministic test fixture path, not EF CLI generated artifacts, because the contract explicitly scopes out design-time compiled-model generation.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8736`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `21b46dcda0c64e959f876a00ac89bb38`
- completed-at-utc: `<redacted>-13T19:58:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XPYW5PVKRTK4A91M6GHHF8/runs/20260513T195850022Z-21b46dcda0c64e959f876a00ac89bb38.json`