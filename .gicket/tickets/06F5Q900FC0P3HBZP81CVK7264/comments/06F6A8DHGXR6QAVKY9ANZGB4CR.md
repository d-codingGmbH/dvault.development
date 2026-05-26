[gicket-bot] PO-critic review contract

Summary
- Delivery contract aligns with current repo evidence, `## Open Questions` is `none`, and the branch contains only PO refinement metadata; this ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q900FC0P3HBZP81CVK7264/description.md` contains the authoritative delivery contract, a `PO Handoff` decision of `ready_for_po_critic`, and `## Open Questions` set to `none`.
- Comment `.gicket/tickets/06F5Q900FC0P3HBZP81CVK7264/comments/06F6A6AXBEY5XHWRTJE44MY3A8.md` records the PO refinement contract and explicitly says no human comments or attachments added scope; comment `06F6A6QJDD3701HN0HWAG26BV4.md` reports PO refinement outcome `po-refinement-ready`.
- Current branch `ticket/06F5Q900FC0P3HBZP81CVK7264-story-add-staged-bulk-benchmark-matrix-and-regre` points to `2372a536d601e3f1bd0a1beb7333fd04dc0288ec`; `git diff --name-only 0be48f938..2372a536d601e3f1bd0a1beb7333fd04dc0288ec` shows only `.gicket/tickets/06F5Q900FC0P3HBZP81CVK7264/**` changes and no `src/`, `tests/`, or benchmark artifact edits yet.
- `benchmark-summary.md:60-67` and matching `benchmark-summary.csv`/`benchmark-summary.json` rows show the current `provider-native-bulk-ingestion` surface already keeps optional-provider rows visible when skipped, with PostgreSQL `smallBatchBoundary=direct-or-unnest`, SQL Server `transfer=SqlBulkCopy`, MySQL staged strategy identity, and Oracle `stagedOracleBulk=not-selected-no-measured-win` in `executionDetail`.
- `docs/architecture/dvault-v1-explicit-save-service.md:60-86` states the existing provider boundaries the ticket relies on: PostgreSQL staged bulk at 60+ operations with retained direct-or-UNNEST below that threshold, SQL Server native gate at 50 operations, MySQL native gate at 50 with staged bulk for larger eligible batches, and Oracle's retained direct bulk boundary.
- `src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs:17` and `src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs:20` define `MinimumStagedBulkOperationCount = 60`; `tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs` directly verify the below-threshold retained-path behavior and Oracle `not-selected-no-measured-win` decision.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:9-43` defines the existing public `IDataVaultSaveService` boundary with `SaveAsync` overloads for `DataVaultSaveRequest`, `DataVaultBulkSaveRequest`, and `DataVaultChunkedSaveRequest`, matching the ticket's explicit scope-out on new public save APIs.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:185-245,442-455` still expect only the current fallback-plus-optimized external-provider rows and assert `executionDetail` tokens for the existing staged/native rows, which matches the ticket's stated gap: the staged/direct comparison matrix and its artifact-contract tests are not implemented yet.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No additional PO example is required for handoff: the contract already names the staged-eligible existing batch, the PostgreSQL below-60 retained path, the MySQL 50-59 retained path, and the Oracle direct/no-staged-win boundary.

Risky assumptions
- The contract assumes the targeted regression rows are the new staged-bulk comparison rows added to `provider-native-bulk-ingestion`; it does not enumerate final baseline ids, so dev/test must keep row identities explicit in the artifact set.
- The contract assumes unattended runs may still produce skipped optional-provider rows only, consistent with the current artifact contract; live provider regression claims will still depend on configured external database lanes.

AC / test suggestions
- Keep acceptance and artifact-test assertions anchored to row identity, skip visibility, and deterministic `executionDetail` tokens across markdown/CSV/JSON, because the contract explicitly forbids adding new artifact columns.
- Treat PostgreSQL retained direct-or-UNNEST, MySQL retained multi-row, SQL Server native bulk, and Oracle retained direct/no-staged-win as separate observable matrix cases so regression budgets compare the intended execution paths.

Implementation watchouts
- The current checked-in benchmark matrix exposes fallback plus one optimized row per optional provider; developer work must expand that matrix without changing the shared artifact schema, provider filter, or run-context contract.
- Because the root benchmark triplet currently keeps skipped optional-provider rows visible, omitting any new staged/direct comparison row when providers are unconfigured would break the existing artifact-contract behavior.

Non-blocking notes
- The branch currently contains PO refinement metadata rather than implementation work; that is expected for this pre-development review and is not a PO blocker.
- `## Follow-Up Questions` remain in the contract, but `## Open Questions` is `none`, so the follow-ups do not block developer handoff under the stated gate.

Split recommendations
- No split is needed for PO refinement if the work stays on benchmark harness, benchmark evidence, and benchmark-contract documentation for staged-bulk comparisons.
- If future work wants cross-scenario regression-budget policy changes beyond `provider-native-bulk-ingestion`, keep that as a separate artifact-governance ticket instead of widening this story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment