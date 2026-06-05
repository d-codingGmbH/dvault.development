[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc' and commit 'ab2d0a0649af' for ticket '06F8KZPN02NWFGMRC2Q1PKYKDR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZPN02NWFGMRC2Q1PKYKDR`.
- Optimistic claim succeeded (`expectedRevision=06F99ZCFPQ1WXD35C77EEXQTRR`, `currentRevision=06F99ZKD3CFYK5003985A6PMR4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc' from source 'ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc'.
- Planned implementation step: Added source-boundary classification in the typed read-model generator so raw or residual dvault.model inputs and incompatible dvault.support-bundle schema versions report DMV1960 and suppress generation.
- Planned implementation step: Added analyzer regression tests for raw model files accompanying valid support bundles, incompatible support-bundle versions, ambiguous support bundles, and entity-specific PIT/bridge diagnostic skip behavior that preserves unrelated satellite help...
- Planned implementation step: Updated the typed read-model diagnostic catalog, analyzer README, and v1 typed PIT/bridge helper contract so DMV1960 covers raw/residual model-first source-boundary failures and DMV1968 is documented as reserved.
- Planned implementation step: Ran targeted analyzer tests, format checks, solution build, and solution tests.
- Resolved branch route (fallback): base 'develop', work 'ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc'.
- Continuing with pre-existing repository changes on branch 'ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc' because the active developer transport already materialized in-flight ticket edits: docs/architecture/dvault-v1-typed-pit-bridge-helpe...
- 15 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: dotnet build and dotnet test emit existing NU1900 warnings because the NuGet vulnerability cache path is read-only in this environment; both commands exited successfully.
- Risk: Historical release notes still preserve older DMV1968 wording as release history; the current package README, generator contract, tests, and catalog now match the shipped DMV1960 behavior.

Next steps
- Push branch 'ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9819`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `b3629c85d73b4172958308a5ea1914dc`
- completed-at-utc: `<redacted>-05T00:16:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZPN02NWFGMRC2Q1PKYKDR/runs/20260605T001620465Z-b3629c85d73b4172958308a5ea1914dc.json`