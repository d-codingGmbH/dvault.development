[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F7Y0HJ1ZPY7ND9N8RVS92H4C-story-generate-typed-bridge-read-helpers-from-su' for ticket '06F7Y0HJ1ZPY7ND9N8RVS92H4C'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0HJ1ZPY7ND9N8RVS92H4C`.
- Optimistic claim succeeded (`expectedRevision=06F89E2PS4CWHQ1QFCZ658A5K8`, `currentRevision=06F89ECR57ZNWKH2Y765RF008W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F7Y0HJ1ZPY7ND9N8RVS92H4C-story-generate-typed-bridge-read-helpers-from-su' and commit '658c88f7f0d7' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F7Y0HJ1ZPY7ND9N8RVS92H4C-story-generate-typed-bridge-read-helpers-from-su' from source '658c88f7f0d7'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Acceptance criteria and definition-of-done depend on executing the added analyzer and runtime-oriented bridge helper tests plus the repository format gate, and this read-only tester session c...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06F7Y0HJ1ZPY7ND9N8RVS92H4C-story-generate-typed-bridge-read-helpers-from-su'.
- Checked out verification commit '658c88f7f0d7'.
- Derived 2 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 2 repository path(s) at commit '658c88f7f0d7'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 83 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Route the ticket to integrator using verified commit 658c88f7f0d7.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8496`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `3eb40842c8c448d29f6634145712e800`
- completed-at-utc: `<redacted>-01T19:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0HJ1ZPY7ND9N8RVS92H4C/runs/20260601T195917145Z-3eb40842c8c448d29f6634145712e800.json`