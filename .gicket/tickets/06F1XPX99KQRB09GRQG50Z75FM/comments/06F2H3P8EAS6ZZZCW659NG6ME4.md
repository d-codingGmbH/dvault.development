[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git -C /mnt/c/Projects/DVault rev-parse --abbrev-ref HEAD` returned `ticket/06F1XPX99KQRB09GRQG50Z75FM-epic-read-and-runtime-performance-ergonomics`, and `git log --oneline -n 4` on that branch shows HEAD `940ddd439` after PO handoff commits `abcf4f4cd` and `d71efb237`.
- `git -C /mnt/c/Projects/DVault diff --name-status develop...ticket/06F1XPX99KQRB09GRQG50Z75FM-epic-read-and-runtime-performance-ergonomics` lists only `.gicket/tickets/06F1XPX99KQRB09GRQG50Z75FM/{description.md,ticket.json,comments/*,events/*}` changes; there are no `src/`, `tests/`, or benchmark-file changes on the epic branch itself.
- `.gicket/tickets/06F1XPX99KQRB09GRQG50Z75FM/description.md` contains `PO Handoff decision: ready_for_po_critic` and `## Open Questions` with `- none`.
- Persisted relation files `.gicket/relations/FM/6M/06F1XPX99KQRB09GRQG50Z75FM--06F1XPXJW79K94G4WG86AG2X6M--parentOf.json`, `FM/0W/...--06F1XPYA9MD0T9C4651ND8KX0W--parentOf.json`, `FM/74/...--06F1XPZAJBSSNN6HY1CHAQPH74--parentOf.json`, and `FM/44/...--06F1XQ03MADSPQD0AJN6R50D44--parentOf.json` confirm the four-child split; the repo also contains seven outgoing `blocks` files for the epic and one incoming `blocks` file from done epic `06F1XPRY3ZDB6W1WQ9ABRRJ2V4`.
- Latest child comments show completion evidence beyond status alone: `06F25RRGN53M9R2Y3XBPBQRJKR.md` records PO-critic closure-only approval for `06F1XPXJW79K94G4WG86AG2X6M`; `06F26G8MBCKQK3TZ20B2QDQ09G.md`, `06F2H01JH9QPMYY44M3P5QRCQ4.md`, and `06F25XVK96W5WY049AABVKDF54.md` are integrator `ACCEPT` decisions for the other three child tickets.
- Direct source evidence matches the contract boundary: `src/DCoding.Data.DVault/IDataVaultReadService.cs` exposes `ReadLatestSatelliteRowsAsync(...)` and `ReadPitRowsAsync(...)`; `src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs` exposes `ReadLatestSatelliteAsync(...)`; `src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs` exposes `ReadBridgeRowsAsync(...)` and `ReadBridgeAsync(...)`.
- The explicit-save and optional-hook boundaries are directly present in source: `src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs` exposes opt-in `UseDataVaultSaveChangesMetadataInterceptor(...)`, while `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs` and `src/DCoding.Data.DVault/DataVaultSaveService.cs` show optional provider save strategies ordered by `Priority` with provider-neutral fallback when no strategy accepts the batch.
- Repository docs and tests already anchor the epic's acceptance language: `README.md` documents explicit `IDataVaultSaveService` persistence plus `ReadLatestSatelliteAsync(...)`, `ReadPitAsync(...)`, and `ReadBridgeAsync(...)`; `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs`, `DataVaultBridgeReadServiceSqliteTests.cs`, `DataVaultSaveChangesMetadataInterceptorSqliteTests.cs`, and `BenchmarkScenarioExecutionTests.cs` provide checked-in compatibility and performance evidence.
- Attachment metadata is real, not just prose: `.gicket/tickets/06F1XPX99KQRB09GRQG50Z75FM/attachments/manifest.json` references `v0.9.0-read-runtime-performance-plan.md`, and the blob exists at `.gicket/attachments/blobs/06ef9cf424da59c6530e25d5babbf0abce00eebfab4edc030312bc06b375fdd1` with the four planned slices matching the four child tickets.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not give one explicit example that maps the word `current` to the existing `ReadLatestSatelliteAsync(...)` call pattern without an `asOf` timestamp; the repository README implies that behavior, but the epic text does not say it outright.
- The contract does not call out edge-case expectations for hierarchy bridge `maximumDepth` boundaries or empty endpoint-hash-key batches, although `tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs` covers bounded depth and empty-result behavior.

Risky assumptions
- Approval assumes the SQLite-first benchmark and compatibility evidence is sufficient for this epic's bounded performance claim; `README.md` and `BenchmarkScenarioExecutionTests.cs` do not establish a broad cross-provider benchmark matrix.
- Approval assumes the incoming `blocks` relation from done epic `06F1XPRY3ZDB6W1WQ9ABRRJ2V4` remains historical/non-blocking unless a later ticket/comment explicitly reopens it.
- Approval assumes downstream `dev` handling will be validation/closure-oriented, because the current epic branch diff versus `develop` contains only ticket artifacts and no implementation changes.

AC / test suggestions
- When the epic is closed, keep acceptance traceability pointed at `DataVaultCompiledCompatibilitySqliteTests.cs`, `DataVaultBridgeReadServiceSqliteTests.cs`, `DataVaultSaveChangesMetadataInterceptorSqliteTests.cs`, and `BenchmarkScenarioExecutionTests.cs` so the checked-in evidence stays easy to audit.
- If a handoff comment is added downstream, make it explicit that the implementation evidence already lives on `develop` via completed child-ticket work and that this epic branch is a ticket-only refinement branch.

Implementation watchouts
- Do not expect fresh product code on `ticket/06F1XPX99KQRB09GRQG50Z75FM-epic-read-and-runtime-performance-ergonomics`; `git diff` against `develop` shows only `.gicket` artifacts on this branch.
- Keep bridge scope bounded to generated endpoint hash-key columns plus hierarchy `TraversalDepth`, matching `src/DCoding.Data.DVault/DataVaultBridgeReadPipeline.cs` and `DataVaultBridgeProjectionRow.cs`.
- Keep any SaveChanges convenience additive and opt-in; the repository baseline still anchors default writes on explicit `IDataVaultSaveService`.

Non-blocking notes
- The persisted plan artifact is attachment-backed rather than a tracked repository-root file; that is consistent with the ticket comment and attachment manifest evidence.
- The epic already blocks several follow-on adoption/tooling tickets via persisted `blocks` relations, so its relation graph is broader than the four-child execution split without contradicting the contract.

Split recommendations
- No additional split is recommended; the persisted four-child execution split matches both the relation graph and current child-ticket completion state.
- If compiled-query/model work ever expands into provider-by-provider certification, split that into a separate follow-up instead of reopening this epic's bounded scope.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment