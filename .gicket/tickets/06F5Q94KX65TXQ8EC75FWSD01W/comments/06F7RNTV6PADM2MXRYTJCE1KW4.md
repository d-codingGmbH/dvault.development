[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g' and commit 'dc3f09dc952a' for ticket '06F5Q94KX65TXQ8EC75FWSD01W'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q94KX65TXQ8EC75FWSD01W`.
- Optimistic claim succeeded (`expectedRevision=06F7RCYXXK8RFHS7YAHPY00WS4`, `currentRevision=06F7RG9348602BF9KCWEWXB4G8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g' from source 'ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g'.
- Planned implementation step: Created docs/performance-profiles.md as the canonical detailed adopter guide for small app-local vault, medium chunked ingestion, staged provider ingestion, and read-model-heavy profiles.
- Planned implementation step: Anchored timing claims to the checked-in root benchmark triplet and preserved run context, optional-provider skipped posture, diagnostics surfaces, and rerun triggers.
- Planned implementation step: Added narrow links from README.md, docs/production-adoption-checklist.md, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md to the new guide.
- Resolved branch route (fallback): base 'develop', work 'ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g'.
- Continuing with pre-existing repository changes on branch 'ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g' because the active developer transport already materialized in-flight ticket edits: benchmarks/DCoding.Data.DVault.Benchmarks/README.m...
- Preserved pre-existing materialized artifact 'docs/performance-profiles.md' instead of overwriting it with the model artifact.
- 11 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: External-provider benchmark rows remain skipped in the current artifact triplet, so staged provider guidance is intentionally boundary and eligibility guidance until a configured provider run supplies completed timing rows.
- Risk: dotnet test DVault.slnx --nologo was not run in this dev pass because the implementation is documentation-only and formatting plus build validation passed.

Next steps
- Push branch 'ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9426`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `7a06749112db4511a1b28fdfb49974d7`
- completed-at-utc: `<redacted>-31T04:42:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q94KX65TXQ8EC75FWSD01W/runs/20260531T044240303Z-7a06749112db4511a1b28fdfb49974d7.json`