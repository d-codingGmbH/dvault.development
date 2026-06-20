[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FE4QRMXVGJVA65ZR5MZ817K8-task-update-provider-performance-matrices-and-v0' for ticket '06FE4QRMXVGJVA65ZR5MZ817K8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QRMXVGJVA65ZR5MZ817K8`.
- Optimistic claim succeeded (`expectedRevision=06FEET527FTGAZTH9WDCAGZD3M`, `currentRevision=06FEETC70XQFRYY7DDBHKKZN2M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FE4QRMXVGJVA65ZR5MZ817K8-task-update-provider-performance-matrices-and-v0' and commit '64a8e92be5c3' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FE4QRMXVGJVA65ZR5MZ817K8-task-update-provider-performance-matrices-and-v0' from source '64a8e92be5c3'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Repository inspection shows the scoped documentation outputs already exist and are unchanged relative to develop, but final tester sign-off still needs deterministic execution of the declared...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FE4QRMXVGJVA65ZR5MZ817K8-task-update-provider-performance-matrices-and-v0'.
- Checked out verification commit '64a8e92be5c3'.
- Inspected committed repository state for 3 repository path(s) at commit '64a8e92be5c3'.
- Expanded deterministic verification evidence using 4 developer verification hint(s) across 15 hinted repository path(s) at commit '64a8e92be5c3'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 310 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Developer verification hint references repository path '0/.42/.0', but that path is absent from the verified committed repository state.
- Developer verification hint references repository path '10/.42/.0', but that path is absent from the verified committed repository state.
- Developer verification hint references repository path '8/.42/.0', but that path is absent from the verified committed repository state.
- Developer verification hint references repository path 'build/test', but that path is absent from the verified committed repository state.
- Developer verification hint references repository path 'v0/.42/.0', but that path is absent from the verified committed repository state.
- Non-blocking: the verifier's missing-path findings for v0.42.0, 8.42.0, 10.42.0, 0.42.0, and build/test came from semantic version or command tokens in developer hints, not from contract-declared repository outputs.

Next steps
- Hand off to integrator using branch ticket/06FE4QRMXVGJVA65ZR5MZ817K8-task-update-provider-performance-matrices-and-v0 at commit 64a8e92be5c3.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7708`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `8164d0739af84f9b97de0628b67f0cbc`
- completed-at-utc: `<redacted>-20T23:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QRMXVGJVA65ZR5MZ817K8/runs/20260620T235214900Z-8164d0739af84f9b97de0628b67f0cbc.json`