[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi' and commit 'b5349e23c670' for ticket '06FBSC0EJHAY200E7PXNRGV7XR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC0EJHAY200E7PXNRGV7XR`.
- Optimistic claim succeeded (`expectedRevision=06FCF0FVQC69NNKBKHCX0MYCDW`, `currentRevision=06FCF0P8SZ0BXDJ9YHY8Y4F0R0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi' from source 'ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi'.
- Planned implementation step: Verified the shipped binary-first APIs in source: UseBinaryFirstProfile() on DataVaultOptions and UseDataVaultBinaryFirstProfile() on ModelBuilder.
- Planned implementation step: Updated the root README and docs/getting-started quickstart text/code so new projects select binary-first storage and direct Code-First model projection records UseDataVaultBinaryFirstProfile().
- Planned implementation step: Updated examples/README.md plus both runnable SQLite/PostgreSQL quickstart programs so registry-backed metadata setup chains UseBinaryFirstProfile() with UseMetadataModel(...).
- Planned implementation step: Updated the PostgreSQL fixture README snippet to describe the same binary-first new-project quickstart behavior and compatibility caveat.
- Planned implementation step: Ran build, test, and format validation from the repository root.
- Resolved branch route (fallback): base 'develop', work 'ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi'.
- 15 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: Live external-provider behavior was not exercised because the optional provider connection strings are not configured in this workspace; the default test suite verified the normal skip boundary.
- Risk: docs/releases/v0.37.0.md was left unchanged because it already anchors the carried-forward compatibility baseline, and broader release-note expansion is scoped out for this ticket.

Next steps
- Push branch 'ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8977`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `acf3986c2c0d4def81cb341672661171`
- completed-at-utc: `<redacted>-14T20:19:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC0EJHAY200E7PXNRGV7XR/runs/20260614T201957592Z-acf3986c2c0d4def81cb341672661171.json`