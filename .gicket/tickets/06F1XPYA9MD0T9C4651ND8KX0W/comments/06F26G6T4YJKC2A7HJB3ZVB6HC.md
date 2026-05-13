[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F1XPYA9MD0T9C4651ND8KX0W-story-prove-compiled-model-and-compiled-query-co' for ticket '06F1XPYA9MD0T9C4651ND8KX0W'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XPYA9MD0T9C4651ND8KX0W`.
- Optimistic claim succeeded (`expectedRevision=06F26E39W4HWPZN3SXF3TNVZKC`, `currentRevision=06F26EGH230Y0VDEGBKD1V8874`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Selected verification source branch 'ticket/06F1XPYA9MD0T9C4651ND8KX0W-story-prove-compiled-model-and-compiled-query-co' and commit '3994ffb54356' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F1XPYA9MD0T9C4651ND8KX0W-story-prove-compiled-model-and-compiled-query-co' from source '3994ffb54356'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Static read-only review observed the claimed commit 3994ffb54356, the added docs/architecture/dvault-ef-compiled-compatibility.md artifact, and the existing compiled compatibility test wiring...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06F1XPYA9MD0T9C4651ND8KX0W-story-prove-compiled-model-and-compiled-query-co'.
- Checked out verification commit '3994ffb54356'.
- Derived 1 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 1 repository path(s) at commit '3994ffb54356'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 72 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator for the configured final acceptance gate.

Prompt cache usage
- prompt-tokens: `25641`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0948`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `3e5b4cdfe02f440498223dd3a79c2a35`
- completed-at-utc: `<redacted>-13T21:31:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XPYA9MD0T9C4651ND8KX0W/runs/20260513T213152392Z-3e5b4cdfe02f440498223dd3a79c2a35.json`