[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06F7Y0H83H29E1D9K5RK3K7Y9W-story-generate-typed-pit-read-helpers-from-suppo' and commit '7b88eb455693' for ticket '06F7Y0H83H29E1D9K5RK3K7Y9W'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0H83H29E1D9K5RK3K7Y9W`.
- Optimistic claim succeeded (`expectedRevision=06F87ZD7TNDA0P4R1FDBK8A3FC`, `currentRevision=06F87ZR52G63XFRS22AB1SPBQ0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06F7Y0H83H29E1D9K5RK3K7Y9W-story-generate-typed-pit-read-helpers-from-suppo' from source 'ticket/06F7Y0H83H29E1D9K5RK3K7Y9W-story-generate-typed-pit-read-helpers-from-suppo'.
- Planned implementation step: Extended DataVaultTypedReadModelSourceGenerator to collect both satellite and PIT read-model declarations from one authoritative dvault.support-bundle.v1 file.
- Planned implementation step: Added PIT parsing and validation against diagnostics.readShape.pit for PIT identity, parent reference, filter/projected/row-identity column groups, snapshot references, ordinary hub PITs, shared-driving-key multi-active hub PITs, and bounded non-mu...
- Planned implementation step: Added generated PIT records and Read{ProducedName}AsOfAsync extensions that construct DataVaultPitAsOfReadRequest, call IDataVaultReadService.ReadPitRowsAsync, and project ParentHashKey, LoadTimestamp, canonical driving keys, and nullable snapshot-...
- Planned implementation step: Updated analyzer tests for supported ordinary hub PIT, shared-driving-key multi-active hub PIT, link-parent PIT, rejected mismatched driving-key PIT diagnostics, and unaffected satellite generation.
- Planned implementation step: Updated the analyzer README to describe the current bounded PIT helper implementation and remaining bridge boundary.
- Resolved branch route (fallback): base 'develop', work 'ticket/06F7Y0H83H29E1D9K5RK3K7Y9W-story-generate-typed-pit-read-helpers-from-suppo'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06F7Y0H83H29E1D9K5RK3K7Y9W-story-generate-typed-pit-read-helpers-from-suppo'.
- 12 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: Full-solution validation still depends on restoring the missing Microsoft.EntityFrameworkCore.Analyzers 10.0.8 package in CI or a network-enabled developer environment.
- Risk: Generated snapshot-reference timestamp members use the existing PIT read-record surface exposed by ReadPitRowsAsync; this keeps the implementation inside the current runtime boundary but cannot expose raw PIT snapshot-reference cells beyond what that service returns.

Next steps
- Push branch 'ticket/06F7Y0H83H29E1D9K5RK3K7Y9W-story-generate-typed-pit-read-helpers-from-suppo' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9890`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `e8a99c814d0d49f59f7f5a94f7c478a4`
- completed-at-utc: `<redacted>-01T17:29:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0H83H29E1D9K5RK3K7Y9W/runs/20260601T172957556Z-e8a99c814d0d49f59f7f5a94f7c478a4.json`