[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry' for ticket '06F0MEFHKF04B746X7GJKRVT04'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEFHKF04B746X7GJKRVT04`.
- Optimistic claim succeeded (`expectedRevision=06F1TJKK1V1A9KR35CMK63573C`, `currentRevision=06F1TJV4HT53XSKQY5E16B002C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Selected verification source branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry' and commit 'ade2e9fbd2e7' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry' from source 'ade2e9fbd2e7'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Read-only tester review found the claimed implementation wired into source and tests, but the persisted Definition of Done requires executable verification and the developer-declared commands...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry'.
- Checked out verification commit 'ade2e9fbd2e7'.
- Derived 3 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 3 repository path(s) at commit 'ade2e9fbd2e7'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 111 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Route the ticket to integrator for final gate review using the verified branch ticket/06F0MEFHKF04B746X7GJKRVT04-task-add-model-export-from-code-first-registry at commit ade2e9fbd2e7.

Prompt cache usage
- prompt-tokens: `25674`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0947`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `c7f5897443d74c6b961b283375a8cfd5`
- completed-at-utc: `<redacted>-12T17:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEFHKF04B746X7GJKRVT04/runs/20260512T175538876Z-c7f5897443d74c6b961b283375a8cfd5.json`