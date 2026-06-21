[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff' and commit 'a43840e8a956' for ticket '06FE4R1XJVQZTQ8S9WN2YE3ZKW'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4R1XJVQZTQ8S9WN2YE3ZKW`.
- Optimistic claim succeeded (`expectedRevision=06FEG9XTAG8CYC07TN492NC4CR`, `currentRevision=06FEGA54TQFFGV8BSFS4R8REH8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded developer ticket mutation surface enabled: gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-relation.
- Prepared interactive developer scratch worktree for target branch 'ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff' from source 'ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff'.
- Planned implementation step: Added inert allocation profiler internals and scoped measurements around DefaultStableHashNormalizer, BuiltInStableHashService, and DefaultDataVaultSaveService DVault-owned preparation/replay phases.
- Planned implementation step: Added --allocation-hotspots benchmark mode with six bounded SQLite workloads covering canonicalization, digest generation, hub-only save prep, link-bearing save prep, and unchanged/changed satellite replay filtering.
- Planned implementation step: Generated repository-backed evidence under artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-<redacted> with the standard benchmark-summary triplet plus additive allocation-hotspots markdown/csv/json sidecars.
- Planned implementation step: Updated benchmark documentation, integration coverage, benchmark option parsing coverage, and artifact allowlist rules for the ticket evidence bundle.
- Resolved branch route (fallback): base 'develop', work 'ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff'.
- 36 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: The full policy commands dotnet build DVault.slnx --nologo and bash tools/check-format.sh did not complete in this worktree; the format wrapper timed out after 120 seconds with no output, so validation used bounded project-level equivalents with MinVerVersionOverride=0.0.0.
- Risk: NuGet NU1900 warnings appeared because the vulnerability cache is read-only in this sandbox, but they did not fail the successful build or test commands.
- Risk: The ranking is evidence for the required local SQLite sha256-v1/HexString baseline only; provider-specific and non-default hash-key variants still need follow-up validation before generalizing the hotspot order.

Next steps
- Push branch 'ticket/06FE4R1XJVQZTQ8S9WN2YE3ZKW-task-profile-hash-canonicalization-and-hash-diff' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9716`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `623eac205a3a476fbd271c90fa6e371b`
- completed-at-utc: `<redacted>-21T03:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4R1XJVQZTQ8S9WN2YE3ZKW/runs/20260621T035631725Z-623eac205a3a476fbd271c90fa6e371b.json`