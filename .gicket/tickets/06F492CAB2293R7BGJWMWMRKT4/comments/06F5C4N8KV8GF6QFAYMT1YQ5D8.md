[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F492CAB2293R7BGJWMWMRKT4-story-measure-and-tune-provider-neutral-read-all' for ticket '06F492CAB2293R7BGJWMWMRKT4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492CAB2293R7BGJWMWMRKT4`.
- Optimistic claim succeeded (`expectedRevision=06F5C2GXX26CNSY5D05CSJ1CRW`, `currentRevision=06F5C2ZR9N2XJEMZ75ZTFTNBR8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F492CAB2293R7BGJWMWMRKT4-story-measure-and-tune-provider-neutral-read-all' and commit '83b9e1ea241c' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F492CAB2293R7BGJWMWMRKT4-story-measure-and-tune-provider-neutral-read-all' from source '83b9e1ea241c'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Definition of Done item 1 requires executable evidence that the bounded branch build, test, and format checks pass. This interactive read-only review path cannot run those commands directly, ...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06F492CAB2293R7BGJWMWMRKT4-story-measure-and-tune-provider-neutral-read-all'.
- Checked out verification commit '83b9e1ea241c'.
- Derived 12 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 12 repository path(s) at commit '83b9e1ea241c'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 215 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator using branch ticket/06F492CAB2293R7BGJWMWMRKT4-story-measure-and-tune-provider-neutral-read-all at commit 83b9e1ea241c.

Prompt cache usage
- prompt-tokens: `60537`
- cached-tokens: `32000`
- effective-cache-ratio: `0.5286`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `0ac43b33ece54734ba44be444bf5e7ff`
- completed-at-utc: `<redacted>-23T18:22:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492CAB2293R7BGJWMWMRKT4/runs/20260523T182203928Z-0ac43b33ece54734ba44be444bf5e7ff.json`