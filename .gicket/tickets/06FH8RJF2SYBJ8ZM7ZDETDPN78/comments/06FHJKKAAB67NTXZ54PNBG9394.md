[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FH8RJF2SYBJ8ZM7ZDETDPN78-task-expose-provider-crypto-capability-facts-fro' for ticket '06FH8RJF2SYBJ8ZM7ZDETDPN78'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RJF2SYBJ8ZM7ZDETDPN78`.
- Optimistic claim succeeded (`expectedRevision=06FHJGW5QH89Z8Q5B4NDRYVGXW`, `currentRevision=06FHJH956QY8G4MT60XZT1HS8G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FH8RJF2SYBJ8ZM7ZDETDPN78-task-expose-provider-crypto-capability-facts-fro' and commit '215f0ba3f97f' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FH8RJF2SYBJ8ZM7ZDETDPN78-task-expose-provider-crypto-capability-facts-fro' from source '215f0ba3f97f'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: The branch diff and targeted file reads are needed for structural review, but policy-defined executable verification for the claimed implementation still requires deterministic host execution...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06FH8RJF2SYBJ8ZM7ZDETDPN78-task-expose-provider-crypto-capability-facts-fro'.
- Checked out verification commit '215f0ba3f97f'.
- Derived 6 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 6 repository path(s) at commit '215f0ba3f97f'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 106 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Handoff to integrator for final acceptance on commit 215f0ba3f97f.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.5376`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `de5a7df56c6847d1b359c98f53f9a82a`
- completed-at-utc: `<redacted>-30T16:13:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RJF2SYBJ8ZM7ZDETDPN78/runs/20260630T161317261Z-de5a7df56c6847d1b359c98f53f9a82a.json`