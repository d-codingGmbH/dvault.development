[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F5Q91V0YGSA6SH9WDS02GH0M-epic-typed-read-models-and-hash-governance' for ticket '06F5Q91V0YGSA6SH9WDS02GH0M'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q91V0YGSA6SH9WDS02GH0M`.
- Optimistic claim succeeded (`expectedRevision=06F6XWSK8E9WJ12DRQ8J31NDFG`, `currentRevision=06F6XX34J8CKQDD3MAKKGPXYCM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F5Q91V0YGSA6SH9WDS02GH0M-epic-typed-read-models-and-hash-governance' and commit 'e5ad28a237f6' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F5Q91V0YGSA6SH9WDS02GH0M-epic-typed-read-models-and-hash-governance' from source 'e5ad28a237f6'.
- Interactive tester tool loop completed review for branch 'ticket/06F5Q91V0YGSA6SH9WDS02GH0M-epic-typed-read-models-and-hash-governance'.
- Evidence: git diff --name-only develop...e5ad28a237f6 -- ':(exclude).gicket/**' returned only docs/plans/README.md and docs/plans/typed-read-model-generator-contract.md.
- Evidence: git diff --check develop...e5ad28a237f6 -- docs/plans/README.md docs/plans/typed-read-model-generator-contract.md returned no output.
- Evidence: docs/plans/README.md now has a Superseded Planning Context section and no longer lists typed-read-model-generator-contract.md under Current Contracts.
- Evidence: docs/plans/typed-read-model-generator-contract.md now says Status: superseded historical planning context, points to docs/releases/v0.22.0.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/model-first-governance.md, tests/DCoding.Data.DVault.Tests/Analyzers/DataV...
- Evidence: docs/releases/v0.22.0.md lines 26, 30, 38, 40, 52, 75, 83, and 95 describe opt-in support-bundle-driven satellite-only helpers, no PIT or bridge helper emission, dynamic IDataVaultReadService as the default runtime path, compiled EF queries as the fixed-shape alterna...
- Evidence: src/DCoding.Data.DVault.Analyzers/README.md lines 54, 56, 58, and 64-73 describe one authoritative support bundle input, satellite-only helper generation over IDataVaultReadService, and DMV1960-DMV1969 unsupported-shape diagnostics.
- 48 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Hand off to integrator.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9268`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `37f4a169ff0a45b5ab37fe5a36ea83a3`
- completed-at-utc: `<redacted>-28T14:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q91V0YGSA6SH9WDS02GH0M/runs/20260528T142614384Z-37f4a169ff0a45b5ab37fe5a36ea83a3.json`