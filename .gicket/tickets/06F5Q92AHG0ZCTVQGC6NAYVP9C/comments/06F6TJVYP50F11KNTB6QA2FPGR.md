[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' and commit '8e0ea8742ab6' for ticket '06F5Q92AHG0ZCTVQGC6NAYVP9C'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92AHG0ZCTVQGC6NAYVP9C`.
- Optimistic claim succeeded (`expectedRevision=06F6TBRYNWB8K22WV2CW797CBW`, `currentRevision=06F6TC1R79H0P5AP6PM4BDRJQR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' from source 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite'.
- Planned implementation step: Removed direct syntax-tree fallback generation for Code-First ApplyDataVaultMetadata callbacks, literal DataVaultMetadataModel construction, and raw dvault.model.v1 additional files from the typed read-model source generator.
- Planned implementation step: Made DVaultGenerateTypedReadModels=true require exactly one authoritative dvault.support-bundle.v1 additional file and report DMV1960 when no projected support bundle is present.
- Planned implementation step: Kept support-bundle explain generation for hub-parent, link-parent, and multi-active satellite shapes, using projected produced names, metadata names, fingerprints, provider value metadata, CLR type names, and nullability.
- Planned implementation step: Updated analyzer tests to use projected support-bundle fixtures for positive generation and negative diagnostics, including raw dvault.model.v1 rejection, stale fingerprint, non-string payload, nullability fallback, and deterministic name collision.
- Planned implementation step: Updated analyzer README guidance to document the support-bundle-only authoritative input boundary.
- Resolved branch route (fallback): base 'develop', work 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite'.
- 13 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: Local dotnet test/build verification could not be completed: analyzer test and analyzer build commands both timed out after 120 seconds in this sandbox after a NU1900 warning about a read-only NuGet vulnerability-cache path.

Next steps
- Push branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9824`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `88f8142457a94022b4b99e57e5636189`
- completed-at-utc: `<redacted>-28T06:35:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/runs/20260528T063524716Z-88f8142457a94022b4b99e57e5636189.json`