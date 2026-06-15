[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FBSC4BEBGSVVTJSQXM1Z74CC-task-collect-baseline-provider-optimization-evid' for ticket '06FBSC4BEBGSVVTJSQXM1Z74CC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC4BEBGSVVTJSQXM1Z74CC`.
- Optimistic claim succeeded (`expectedRevision=06FCSRVDN7BXVPYB0VNYPEFSH4`, `currentRevision=06FCSRYA48SV6D951JXQVSAVBG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FBSC4BEBGSVVTJSQXM1Z74CC-task-collect-baseline-provider-optimization-evid' and commit 'be5c01a3719e' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FBSC4BEBGSVVTJSQXM1Z74CC-task-collect-baseline-provider-optimization-evid' from source 'be5c01a3719e'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Repository inspection supports the claimed provider-evidence baseline, but final tester pass requires deterministic execution of the declared verification commands outside this read-only sess...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FBSC4BEBGSVVTJSQXM1Z74CC-task-collect-baseline-provider-optimization-evid'.
- Checked out verification commit 'be5c01a3719e'.
- Inspected committed repository state for 2 repository path(s) at commit 'be5c01a3719e'.
- Expanded deterministic verification evidence using 4 developer verification hint(s) across 2 hinted repository path(s) at commit 'be5c01a3719e'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 137 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator using branch ticket/06FBSC4BEBGSVVTJSQXM1Z74CC-task-collect-baseline-provider-optimization-evid at commit be5c01a3719e.
- Keep completed external-provider timing claims anchored to the checked-in v0.32 bundles and not to the root skipped-placeholder rows.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `80384`
- effective-cache-ratio: `0.7489`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `11ca8a3cd4bd4e4291e5dec93cc7348f`
- completed-at-utc: `<redacted>-15T20:15:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC4BEBGSVVTJSQXM1Z74CC/runs/20260615T201538521Z-11ca8a3cd4bd4e4291e5dec93cc7348f.json`