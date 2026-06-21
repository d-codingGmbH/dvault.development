[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff' for ticket '06FE4R1XJVQZTQ8S9WN2YE3ZKW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4R1XJVQZTQ8S9WN2YE3ZKW`.
- Optimistic claim succeeded (`expectedRevision=06FEGMT3V2WCZGJMV6K8G65DRR`, `currentRevision=06FEGRP5R0XDAMY5NVPVZA345W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff' and commit 'a43840e8a956' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff' from source 'a43840e8a956'.
- Interactive tester assessment detected an environment-bound executable verification blocker and fell back to legacy verification.
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff'.
- Checked out verification commit 'a43840e8a956'.
- Derived 12 committed repository path(s) from branch delta against base branch 'develop'.
- Expanded committed repository inspection with 12 branch-delta path(s) beyond the 2 ticket-declared path(s).
- Inspected committed repository state for 14 repository path(s) at commit 'a43840e8a956'.
- 210 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator using commit a43840e8a956 and the committed evidence bundle under artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-<redacted>/.
- Use the report's recommended optimization order to scope follow-up implementation tickets; no tester rework is indicated by the current evidence.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `54400`
- effective-cache-ratio: `0.4642`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `3b165135b9dc495b92c6270585940ea7`
- completed-at-utc: `<redacted>-21T04:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4R1XJVQZTQ8S9WN2YE3ZKW/runs/20260621T042837805Z-3b165135b9dc495b92c6270585940ea7.json`