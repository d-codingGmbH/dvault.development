[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FE4RJP5KG02DF7AEMCQYGNVW-task-prototype-postgresql-pit-rebuild-insert-sel' for ticket '06FE4RJP5KG02DF7AEMCQYGNVW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RJP5KG02DF7AEMCQYGNVW`.
- Optimistic claim succeeded (`expectedRevision=06FF0R59DXYQRW0J9MYKN9N654`, `currentRevision=06FF0TTBYKJ0G9BKCDKSBTGG2G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FE4RJP5KG02DF7AEMCQYGNVW-task-prototype-postgresql-pit-rebuild-insert-sel' and commit 'bc0c80245ef1' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FE4RJP5KG02DF7AEMCQYGNVW-task-prototype-postgresql-pit-rebuild-insert-sel' from source 'bc0c80245ef1'.
- Interactive tester tool loop fell back to legacy verification after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FE4RJP5KG02DF7AEMCQYGNVW-task-prototype-postgresql-pit-rebuild-insert-sel'.
- Checked out verification commit 'bc0c80245ef1'.
- Derived 12 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 12 repository path(s) at commit 'bc0c80245ef1'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 197 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator for final acceptance review of commit bc0c80245ef1 on branch ticket/06FE4RJP5KG02DF7AEMCQYGNVW-task-prototype-postgresql-pit-rebuild-insert-sel.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8985`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `49600a755bc443a6bdd76c157ef3e1cc`
- completed-at-utc: `<redacted>-22T17:49:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RJP5KG02DF7AEMCQYGNVW/runs/20260622T174938346Z-49600a755bc443a6bdd76c157ef3e1cc.json`