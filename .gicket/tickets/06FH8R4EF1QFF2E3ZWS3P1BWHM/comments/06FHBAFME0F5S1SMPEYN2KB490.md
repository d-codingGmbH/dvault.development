[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester verification detected blocking repository findings on branch 'ticket/06FH8R4EF1QFF2E3ZWS3P1BWHM-task-add-net-8-sdk-analyzer-smoke-ci-and-package'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8R4EF1QFF2E3ZWS3P1BWHM`.
- Optimistic claim succeeded (`expectedRevision=06FHB30H2D94JKYFQH71EDPGHC`, `currentRevision=06FHB6WMNRM0CVAD6W46YQK68C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FH8R4EF1QFF2E3ZWS3P1BWHM-task-add-net-8-sdk-analyzer-smoke-ci-and-package' and commit '3e1fe45851510e776c894d73871cb2aebd7856f6' (developer-delivery-outcome contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FH8R4EF1QFF2E3ZWS3P1BWHM-task-add-net-8-sdk-analyzer-smoke-ci-and-package' from source '3e1fe45851510e776c894d73871cb2aebd7856f6'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Structural review shows the branch retargets the analyzer project to netstandard2.0, wires package-managed Roslyn/System.Composition/System.Text.Json dependencies, removes the old integration...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FH8R4EF1QFF2E3ZWS3P1BWHM-task-add-net-8-sdk-analyzer-smoke-ci-and-package'.
- Checked out verification commit '3e1fe45851510e776c894d73871cb2aebd7856f6'.
- Derived 12 committed repository path(s) from branch delta against base branch 'develop'.
- Expanded committed repository inspection with 10 branch-delta path(s) beyond the 4 ticket-declared path(s).
- Inspected committed repository state for 14 repository path(s) at commit '3e1fe4585151'.
- 338 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Expected repository path 'analyzers/dotnet/cs' is absent from the verified committed repository state.
- Trust audit: Trust policy audit
- policy-version: 2026-03-25
- active-mode: trust/repo
- [allowed] command: git checkout ticket/06FH8R4EF1QFF2E3ZWS3P1BWHM-task-add-net-8-sdk-analyzer-smoke-ci-and-package (allow: git checkout*) (approval-hook)
- [allowed] command: git check...
- Acceptance-criteria comparison is incomplete: 6 item(s) could not be confirmed due to verification failures.
- Definition-of-done comparison is incomplete: 5 item(s) could not be confirmed due to verification failures.

Next steps
- Inspect bot logs and retry tester verification.
- Hand off to integrator using branch ticket/06FH8R4EF1QFF2E3ZWS3P1BWHM-task-add-net-8-sdk-analyzer-smoke-ci-and-package at commit 3e1fe45851510e776c894d73871cb2aebd7856f6.
- If integrator wants a fresh executable spot-check, rerun the repository-root validation sequence documented in docs/local-validation.md for the same commit.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9235`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `6c35be3175e84744ad59108365784cb3`
- completed-at-utc: `<redacted>-29T23:14:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8R4EF1QFF2E3ZWS3P1BWHM/runs/20260629T231447494Z-6c35be3175e84744ad59108365784cb3.json`