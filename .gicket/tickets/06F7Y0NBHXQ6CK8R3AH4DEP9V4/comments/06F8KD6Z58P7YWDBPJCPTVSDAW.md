[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F7Y0NBHXQ6CK8R3AH4DEP9V4-task-update-v0-26-0-provider-performance-and-sch' for ticket '06F7Y0NBHXQ6CK8R3AH4DEP9V4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0NBHXQ6CK8R3AH4DEP9V4`.
- Optimistic claim succeeded (`expectedRevision=06F8KAZH1YZMWGFQ0J4VZJN72R`, `currentRevision=06F8KBAK2ZCYSQKKJZXJ5DJQBC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F7Y0NBHXQ6CK8R3AH4DEP9V4-task-update-v0-26-0-provider-performance-and-sch' and commit '583a985d5f84' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F7Y0NBHXQ6CK8R3AH4DEP9V4-task-update-v0-26-0-provider-performance-and-sch' from source '583a985d5f84'.
- Interactive tester tool loop completed review for branch 'ticket/06F7Y0NBHXQ6CK8R3AH4DEP9V4-task-update-v0-26-0-provider-performance-and-sch'.
- Evidence: Reviewed commit 583a985d5f84 against develop; git diff --stat on the scoped documentation surfaces shows 12 documentation-file changes and no library/runtime source-file changes.
- Evidence: README.md, docs/performance-profiles.md, docs/production-adoption-checklist.md, and docs/releases/v0.26.0.md now describe v0.26.0 as the current baseline; docs/releases/v0.25.0.md was rewritten to historical wording.
- Evidence: docs/performance-profiles.md links benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json; docs/releases/v0.26.0.md points readers to the same triplet and uses bounded verifier summaries instead of copying raw tables.
- Evidence: docs/architecture/dvault-dotnet-ef-design-time-workflow.md, docs/architecture/dvault-v1-explicit-save-service.md, docs/architecture/dvault-v1-pit-bridge-boundary.md, docs/architecture/dvault-v1-streaming-explicit-save-contract.md, docs/architecture/dvault-v1-typed-pi...
- Evidence: tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs defines the canonical profile categories SmallAppLocalVault, MediumChunkedIngestion, StagedProviderIngestion, and ReadModelHeavy, and those same names appear in docs/performance-profiles.m...
- Evidence: A direct presence check confirmed root benchmark-summary.md, benchmark-summary.json, and benchmark-summary.csv exist.
- 42 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Continue to integrator.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9013`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `3b55d3e47dc44ed892c38b50f5580a7f`
- completed-at-utc: `<redacted>-02T18:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0NBHXQ6CK8R3AH4DEP9V4/runs/20260602T185952743Z-3b55d3e47dc44ed892c38b50f5580a7f.json`