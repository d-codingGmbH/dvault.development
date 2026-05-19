[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06F2PGPKXWRFXNPFA1JR0X67XC-story-improve-current-and-as-of-query-apis' and commit '9cc510706b33' for ticket '06F2PGPKXWRFXNPFA1JR0X67XC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGPKXWRFXNPFA1JR0X67XC`.
- Optimistic claim succeeded (`expectedRevision=06F40ARXHA8MFYJZFCHJZ995G4`, `currentRevision=06F40AW6GFPJARZ9T2GC5H6S9C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06F2PGPKXWRFXNPFA1JR0X67XC-story-improve-current-and-as-of-query-apis' from source 'ticket/06F2PGPKXWRFXNPFA1JR0X67XC-story-improve-current-and-as-of-query-apis'.
- Planned implementation step: Added DataVaultReadServiceCurrentSatelliteExtensions with explicit-metadata and registry-backed ReadCurrentSatelliteRowsAsync, ReadCurrentSatelliteAsync, ReadAsOfSatelliteRowsAsync, and ReadAsOfSatelliteAsync overloads.
- Planned implementation step: Kept all new overloads delegating through DataVaultLatestSatelliteReadRequest or DataVaultRegistryLatestSatelliteReadRequest and the existing ReadLatestSatelliteRowsAsync / ReadLatestSatelliteAsync pipeline.
- Planned implementation step: Expanded SQLite integration coverage for explicit current/as-of raw reads, registry current/as-of raw reads, typed explicit current/as-of reads, typed registry current/as-of reads, missing-parent empty results, missing registry metadata errors, lin...
- Planned implementation step: Updated README and v0.15.0 release notes to describe current as an additive convenience name over latest-satellite reads and to keep PIT-backed history on the separate PIT request surface.
- Planned implementation step: Updated the core public API approved snapshot for the new overload family.
- Planned implementation step: Removed the existing UTF-8 BOM from src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs so the repository format gate passes.
- Resolved branch route (fallback): base 'develop', work 'ticket/06F2PGPKXWRFXNPFA1JR0X67XC-story-improve-current-and-as-of-query-apis'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- 17 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: Build and test output still includes existing NU1900 warnings because NuGet vulnerability-cache writes target a read-only home cache path in this sandbox; commands completed successfully.

Next steps
- Push branch 'ticket/06F2PGPKXWRFXNPFA1JR0X67XC-story-improve-current-and-as-of-query-apis' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9752`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `739ccd924a6845619352782fd170179e`
- completed-at-utc: `<redacted>-19T13:01:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGPKXWRFXNPFA1JR0X67XC/runs/20260519T130143819Z-739ccd924a6845619352782fd170179e.json`