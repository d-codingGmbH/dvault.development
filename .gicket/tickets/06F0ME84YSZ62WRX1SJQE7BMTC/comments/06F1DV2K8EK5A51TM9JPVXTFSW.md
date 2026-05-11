[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F0ME84YSZ62WRX1SJQE7BMTC-epic-code-first-and-typed-workflow-usability' for ticket '06F0ME84YSZ62WRX1SJQE7BMTC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0ME84YSZ62WRX1SJQE7BMTC`.
- Optimistic claim succeeded (`expectedRevision=06F1DSJSJEJVT4S67M2QT6HMSG`, `currentRevision=06F1DSRM5XR76G8M894XM6B120`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Selected verification source branch 'ticket/06F0ME84YSZ62WRX1SJQE7BMTC-epic-code-first-and-typed-workflow-usability' (developer-delivery-outcome contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F0ME84YSZ62WRX1SJQE7BMTC-epic-code-first-and-typed-workflow-usability' from source 'ticket/06F0ME84YSZ62WRX1SJQE7BMTC-epic-code-first-and-typed-workflow-usability'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Read-only tester review found the claimed handoff is a no-repository-change epic branch: git diff develop...ticket/06F0ME84YSZ62WRX1SJQE7BMTC-epic-code-first-and-typed-workflow-usability show...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06F0ME84YSZ62WRX1SJQE7BMTC-epic-code-first-and-typed-workflow-usability'.
- Expanded deterministic verification evidence using 5 developer verification hint(s) across 3 hinted repository path(s) at commit '35bd23bbcaa5'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- Executed tester command `bash tools/check-format.sh`.
- Restored verification branch 'ticket/06F0ME84YSZ62WRX1SJQE7BMTC-epic-code-first-and-typed-workflow-usability' after tester verification.
- 107 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Committed branch delta against base branch 'develop' did not contain non-ticket repository paths to inspect.

Next steps
- Hand off to integrator according to the configured tester success route.

Prompt cache usage
- prompt-tokens: `27500`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0884`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `d19c4c08f06046829136b62b7c635801`
- completed-at-utc: `<redacted>-11T12:04:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0ME84YSZ62WRX1SJQE7BMTC/runs/20260511T120406252Z-d19c4c08f06046829136b62b7c635801.json`