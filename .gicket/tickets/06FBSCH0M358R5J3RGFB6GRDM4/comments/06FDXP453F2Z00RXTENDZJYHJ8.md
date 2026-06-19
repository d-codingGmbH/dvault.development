[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FBSCH0M358R5J3RGFB6GRDM4-task-close-oracle-pit-and-bridge-read-gaps' for ticket '06FBSCH0M358R5J3RGFB6GRDM4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCH0M358R5J3RGFB6GRDM4`.
- Optimistic claim succeeded (`expectedRevision=06FDTKPPA00BS8R95N4RZJBWGR`, `currentRevision=06FDXJX90VG5TR6MADAG33VK6G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FBSCH0M358R5J3RGFB6GRDM4-task-close-oracle-pit-and-bridge-read-gaps' and commit '803a21c486a5' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FBSCH0M358R5J3RGFB6GRDM4-task-close-oracle-pit-and-bridge-read-gaps' from source '803a21c486a5'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Static review of commit 803a21c486a5 found the Oracle PIT/bridge evidence, documentation, and verifier updates aligned with the ticket, but the changed integration test and documentation stil...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FBSCH0M358R5J3RGFB6GRDM4-task-close-oracle-pit-and-bridge-read-gaps'.
- Checked out verification commit '803a21c486a5'.
- Derived 6 committed repository path(s) from branch delta against base branch 'develop'.
- Expanded committed repository inspection with 4 branch-delta path(s) beyond the 4 ticket-declared path(s).
- Inspected committed repository state for 8 repository path(s) at commit '803a21c486a5'.
- 208 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Route the ticket to integrator using branch ticket/06FBSCH0M358R5J3RGFB6GRDM4-task-close-oracle-pit-and-bridge-read-gaps at commit 803a21c486a5.
- Use the passing dotnet test DVault.slnx --nologo and bash tools/check-format.sh results together with the checked-in Oracle smoke-read evidence references during integrator review.

Prompt cache usage
- prompt-tokens: `65797`
- cached-tokens: `31488`
- effective-cache-ratio: `0.4786`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `675c37baab9847e38f78fae3b52e6194`
- completed-at-utc: `<redacted>-19T07:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCH0M358R5J3RGFB6GRDM4/runs/20260619T074632282Z-675c37baab9847e38f78fae3b52e6194.json`