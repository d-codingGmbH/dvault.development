[gicket-bot] PO-critic review contract

Summary
- Contract is now internally consistent, grounded in current repository evidence, and ready for developer handoff; remaining concerns are implementation watchouts, not PO blockers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q90718D21DN1N1Q2AP7YEM/description.md` contains `PO Handoff` -> `ready_for_po_critic` and `## Open Questions` -> `- none`.
- `.gicket/tickets/06F5Q90718D21DN1N1Q2AP7YEM/comments/06F6B0JXMZXGRRW3M30TTB16DC.md` marks prior critic items 1-6 as `answered`, including the blocker-state correction and the provider-specific v0.20.0 hierarchy.
- `git diff --name-status develop...HEAD` shows only `.gicket/tickets/06F5Q90718D21DN1N1Q2AP7YEM/**` metadata/comment/event changes on this branch; no repo docs or product-code files have been edited yet, which is acceptable for this pre-development ticket.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` directly defines `IDataVaultSaveService`, `DataVaultBulkSaveRequest`, and `DataVaultChunkedSaveRequest`, matching the contract's explicit-save baseline.
- `docs/architecture/dvault-v1-explicit-save-service.md` and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` both document the mixed provider boundary: PostgreSQL staged COPY above 60 operations with direct/UNNEST below, MySQL staged bulk above 60 with multi-row between 50 and 60, SQL Server as a single native-bulk lane, and Oracle as retained direct batching.
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs` keeps Oracle on direct batching with `StagedOracleBulkNotSelectedReason` set to `not-selected-no-measured-win`, and `benchmarks/DCoding.Data.DVault.Benchmarks/ProviderNativeBulkIngestionBenchmark.cs` emits the same Oracle boundary text in benchmark execution detail.
- `benchmark-summary.md` shows PostgreSQL, SQL Server, MySQL, and Oracle optional-provider lanes as `skipped` because `DVAULT_TEST_*_CONNECTION_STRING` variables are unset, so the checked-in root triplet defines provider-boundary visibility but not completed live external-provider timings in this checkout.
- `find docs/releases -maxdepth 2 -type f` lists release-note files through `docs/releases/v0.19.0.md`; `docs/releases/v0.20.0.md` does not exist yet, so that deliverable is clearly new work.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Benchmark-facing docs should explicitly preserve the skipped optional-provider case from `benchmark-summary.md` so v0.20.0 prose does not imply completed external-provider timings when connection strings are absent.
- Stored-procedure wording needs a concrete negative example such as 'DVault does not auto-generate or auto-manage stored procedures'; `rg` found no stored-procedure references under `README.md`, `docs`, `benchmarks`, `src`, or `tests`.
- `docs/releases/v0.20.0.md` is absent today, so the release-note part of the ticket is a create-new-file path, not just an edit-existing-file path.

Risky assumptions
- Developers must distinguish repository-supported provider boundaries from completed benchmark measurements, because the checked-in root benchmark triplet currently skips all external providers.
- Readers must not infer an existing stored-procedure public API or automation surface from the new docs; the current repository shows no stored-procedure references.
- README, checklist, benchmark docs, and new v0.20.0 release notes must introduce the v0.20.0 boundary without retroactively rewriting v0.19.0's claim set.

AC / test suggestions
- Add a doc-review check that every updated surface names the same hierarchy: `IDataVaultSaveService` baseline, `DataVaultBulkSaveRequest` compatibility baseline, `DataVaultChunkedSaveRequest` provider-neutral chunked path, and provider-specific optimized exceptions.
- Make the skipped-row rule explicit in one acceptance criterion or review note: optional-provider rows stay authoritative through `executionStatus=skipped` and `skipReason` when the root benchmark triplet is reused.
- Add an explicit wording check that no updated document presents stored procedures as a built-in runtime path or as a replacement for provider-specific optimized saves.

Implementation watchouts
- Create `docs/releases/v0.20.0.md`; it is not present under `docs/releases` yet.
- Keep Oracle wording aligned with both `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs` and `benchmarks/DCoding.Data.DVault.Benchmarks/ProviderNativeBulkIngestionBenchmark.cs`, which still mark staged Oracle bulk as `not-selected-no-measured-win`.
- Keep benchmark-facing prose aligned with `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and `docs/plans/performance-evidence-benchmark-artifact-contract.md`; those sources currently define the PostgreSQL/MySQL staged-versus-small-batch splits and the SQL Server/Oracle exceptions.
- Do not treat the current root `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` triplet as completed external-provider performance proof in this checkout; it currently records skipped optional-provider rows.

Non-blocking notes
- Three downstream blocked tickets remain `todo` (`06F5Q90CSKMGK3NZZ25XTW6W4C`, `06F5Q90KC6JGQPSP285XQYSPK8`, and `06F5Q916BXE2N372SWMH1X776G`), so concise provider-boundary wording here will matter quickly for follow-on work.
- The branch history shows multiple PO and PO-critic passes, but the latest durable refinement comment is the answered contract in `06F6B0JXMZXGRRW3M30TTB16DC.md`.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment