[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in' for ticket '06F7Y0KVHGTTVS216ERSG4XNMM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0KVHGTTVS216ERSG4XNMM`.
- Optimistic claim succeeded (`expectedRevision=06F8JCN1EMSS7Q3V8ZRA5FFYK0`, `currentRevision=06F8JG1HPKPTBMXA3A692D0Q7W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in' and commit 'de0963eab7e9' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in' from source 'de0963eab7e9'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Read-only tester inspection confirmed the branch contains the idempotency-preflight implementation, preflight-lane wiring, live-schema index metadata updates, focused unit coverage, SQLite in...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in'.
- Checked out verification commit 'de0963eab7e9'.
- Derived 12 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 12 repository path(s) at commit 'de0963eab7e9'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 144 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to the integrator gate using branch `ticket/06F7Y0KVHGTTVS216ERSG4XNMM-story-add-provider-idempotency-constraint-and-in` at commit `de0963eab7e9`.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8495`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `a91f3a0e02a64b0d8a193116ef54849e`
- completed-at-utc: `<redacted>-02T17:04:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0KVHGTTVS216ERSG4XNMM/runs/20260602T170412251Z-a91f3a0e02a64b0d8a193116ef54849e.json`