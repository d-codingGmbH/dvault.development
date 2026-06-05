[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the persisted contract is specific, `## Open Questions` is `none`, and the named repository doc/source surfaces directly exist; remaining risk is keeping the new v0.31.0 section authoritative without widening into unrelated baseline harmonization.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The live relation set matches the contract: `.gicket/relations/88/80/06F8KZQNH8CCMTJW9P95W1N388--06F8KZR38EDSVZBCTC0XYR4R80--parentOf.json` makes epic `06F8KZQNH8CCMTJW9P95W1N388` the parent, `.gicket/relations/80/08/06F8KZR38EDSVZBCTC0XYR4R80--06F8KZRSTHAGSP6GPGFBFQGY08--blocks.json` preserves the downstream blocked task, and related `ticket.json` files show `06F8KZRSTHAGSP6GPGFBFQGY08` is still `todo` while historical incoming blocker `06F8KZQAWZ7QRGB68KB21C9B0R` is `done`.
- `docs/performance-profiles.md:3-57` is still `Status: v0.28.0 adopter guidance`, cites the root benchmark artifact triplet, and already carries the four profile families plus an existing `## Profile Selection` table that this story can lift into an explicit v0.31.0 contract.
- `docs/architecture/dvault-v1-explicit-save-service.md:8-10,29-42` and `src/DCoding.Data.DVault/DataVaultSaveService.cs:13-60,507-568` directly prove the named write boundary: `IDataVaultSaveService`, `DataVaultBulkSaveRequest`, `DataVaultChunkedSaveRequest`, and the async `IAsyncEnumerable<DataVaultSaveChunk>` save path.
- `docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md:9-17,40-52,92-128`, `docs/architecture/dvault-v1-pit-bridge-boundary.md:10-12,57-63`, and `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:159-208,<redacted>` directly prove request-bound read diagnostics, finite fallback causes, maintained PIT/bridge prerequisites, and the SQLite-only optimized latest-satellite posture.
- `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md:10-22`, `src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs:43-46`, `src/DCoding.Data.DVault/DataVaultTelemetryServiceCollectionExtensions.cs:8-25`, and `docs/architecture/dvault-v1-activity-tracing-contract.md:11-25` directly prove the typed-helper, `CreateSupportBundleDiagnostics`, telemetry, and tracing boundaries referenced by the ticket.
- `git diff --stat develop...HEAD` and `git diff --name-only develop...HEAD` on branch `ticket/06F8KZR38EDSVZBCTC0XYR4R80-story-define-performance-decision-tree-contract` at `8e400acc2` show only `.gicket/tickets/06F8KZR38EDSVZBCTC0XYR4R80/**` metadata changes; no repository documentation work has started yet.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Make the branch between materialized `DataVaultChunkedSaveRequest` and already-async `IAsyncEnumerable<DataVaultSaveChunk>` explicit so adopters do not treat them as interchangeable defaults.
- Call out `IncompleteReadShapeEvidence` and `StaleReadModelMaintenance` as distinct read stop/fallback conditions rather than a single generic fallback bucket.
- State plainly that skipped optional-provider benchmark rows are evidence-visible starting points, not measured PostgreSQL/SQL Server/MySQL/Oracle wins.

Risky assumptions
- The developer will treat the existing `## Profile Selection` table as supporting input and not leave it as a competing authoritative decision model after the new v0.31.0 section is added.
- The developer will keep this story scoped to `docs/performance-profiles.md` even though repo-wide current-baseline statements are mixed today: `docs/performance-profiles.md` still says v0.28.0, `docs/production-adoption-checklist.md:9` says v0.29.0, and `README.md:25` says v0.30.0.
- Downstream task `06F8KZRSTHAGSP6GPGFBFQGY08` will own practical examples and any optional checklist pointer, so this story can stay contract-only.

AC / test suggestions
- Acceptance review should confirm one clearly labeled v0.31.0 decision-tree contract section exists in `docs/performance-profiles.md` and no second authoritative decision model remains elsewhere in that document.
- Verify the write branches map back to source-backed public surfaces in `src/DCoding.Data.DVault/DataVaultSaveService.cs:13-60,507-568` and `docs/architecture/dvault-v1-explicit-save-service.md:8-10,29-42`.
- Verify read fallback wording reuses the finite `DataVaultReadStrategyFallbackCauseKind` values in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:159-208` and maintained PIT/bridge requirements in `docs/architecture/dvault-v1-pit-bridge-boundary.md:10-12,57-63`.
- Verify typed-helper and observability wording stays aligned with `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md:10-22`, `src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs:43-46`, `src/DCoding.Data.DVault/DataVaultTelemetryServiceCollectionExtensions.cs:8-25`, and `docs/architecture/dvault-v1-activity-tracing-contract.md:11-25`.

Implementation watchouts
- The current branch is metadata-only; the first developer diff still needs to satisfy the definition of done by changing `docs/performance-profiles.md` outside `.gicket`.
- Do not reframe typed helpers as a fifth runtime performance profile; keep them design-time, `dvault.support-bundle.v1`-backed, and request-bound via reviewed `ReadShape` evidence.
- Do not overstate non-SQLite provider read performance; SQLite remains the only repository-proven optimized latest-satellite path unless another ticket adds new benchmark evidence.
- Do not imply read-time PIT/bridge maintenance, automatic strategy routing, dashboards, exporters, or provider-specific SQL artifact workflow.

Non-blocking notes
- No repository implementation has started yet; for this pre-development gate that is expected and not a reason to return the ticket to PO.
- The contract is already specific enough that missing code/test diffs on the branch are a developer-handoff watchout, not a PO-level blocker.

Split recommendations
- No split recommended. Keep the contract definition in `06F8KZR38EDSVZBCTC0XYR4R80` and leave practical examples, optional checklist pointer work, and release-doc follow-through to the existing downstream tickets under epic `06F8KZQNH8CCMTJW9P95W1N388`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment