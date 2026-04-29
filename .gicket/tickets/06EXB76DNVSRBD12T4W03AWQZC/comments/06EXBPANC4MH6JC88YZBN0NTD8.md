[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06EXB76DNVSRBD12T4W03AWQZC-task-design-stable-hashing-contract' for ticket '06EXB76DNVSRBD12T4W03AWQZC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB76DNVSRBD12T4W03AWQZC`.
- Optimistic claim succeeded (`expectedRevision=06EXBN90BQ8XT1K6AFSSWNBB6M`, `currentRevision=06EXBNRV9M28FB7CFWYBKRYQ9W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06EXB76DNVSRBD12T4W03AWQZC-task-design-stable-hashing-contract' and commit 'a99ca303251c' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06EXB76DNVSRBD12T4W03AWQZC-task-design-stable-hashing-contract' from source 'a99ca303251c'.
- Interactive tester tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy verification.
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06EXB76DNVSRBD12T4W03AWQZC-task-design-stable-hashing-contract'.
- Checked out verification commit 'a99ca303251c'.
- Derived 3 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 3 repository path(s) at commit 'a99ca303251c'.
- Executed tester command `dotnet test --nologo`.
- 70 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Automatic integrator close is not workflow-compatible after tester handoff: No reachable workflow rule allows the transition under context-free evaluation.

Next steps
- Route the ticket to the configured integrator gate for final acceptance review using branch ticket/06EXB76DNVSRBD12T4W03AWQZC-task-design-stable-hashing-contract at commit a99ca303251c.
- Allow the integrator-stage close transition in .gicket/workflow.json or finish the ticket manually from ready-for-integration.

Prompt cache usage
- prompt-tokens: `32859`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0740`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `2b5d65bff2a642de8c64e6f9680c9423`
- completed-at-utc: `<redacted>-28T20:48:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB76DNVSRBD12T4W03AWQZC/runs/20260428T204802785Z-2b5d65bff2a642de8c64e6f9680c9423.json`