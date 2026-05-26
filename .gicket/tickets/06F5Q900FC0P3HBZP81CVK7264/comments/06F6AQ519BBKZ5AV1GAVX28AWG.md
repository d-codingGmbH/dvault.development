[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F5Q900FC0P3HBZP81CVK7264-story-add-staged-bulk-benchmark-matrix-and-regre' for ticket '06F5Q900FC0P3HBZP81CVK7264'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q900FC0P3HBZP81CVK7264`.
- Optimistic claim succeeded (`expectedRevision=06F6AMNNCK2NYB4QFSSK7BCNJM`, `currentRevision=06F6AMYMT6Z7E96JZ3S5N3E1TC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F5Q900FC0P3HBZP81CVK7264-story-add-staged-bulk-benchmark-matrix-and-regre' and commit '704cb327a219' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F5Q900FC0P3HBZP81CVK7264-story-add-staged-bulk-benchmark-matrix-and-regre' from source '704cb327a219'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Repository review confirms the staged-bulk matrix implementation, checked-in before/after artifact triplets, docs, and benchmark contract tests are wired into the branch, but the tester gate ...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06F5Q900FC0P3HBZP81CVK7264-story-add-staged-bulk-benchmark-matrix-and-regre'.
- Checked out verification commit '704cb327a219'.
- Derived 12 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 12 repository path(s) at commit '704cb327a219'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 219 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off the ticket to integrator using branch ticket/06F5Q900FC0P3HBZP81CVK7264-story-add-staged-bulk-benchmark-matrix-and-regre at commit 704cb327a219.
- If later release evidence needs live optional-provider timings, rerun the benchmark with configured provider connection strings to replace skipped rows.

Prompt cache usage
- prompt-tokens: `30341`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0802`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `dde9d5f37b9745369a5d25977152cf95`
- completed-at-utc: `<redacted>-26T17:37:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q900FC0P3HBZP81CVK7264/runs/20260526T173709955Z-dde9d5f37b9745369a5d25977152cf95.json`