[gicket-bot] PO-critic review contract

Summary
- Approved for dev: the ticket is bounded to the existing SQLite benchmark harness, the fallback and optimized APIs exist in source, and the persisted contract has no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted contract at .gicket/tickets/06EZ0NCGYCADKEYGR16J5PJFS0/description.md contains `## Open Questions` -> `none` and explicitly ties the work to BenchmarkRunner, BenchmarkArtifacts, the benchmark README, and automated tests.
- Ticket comments under .gicket/tickets/06EZ0NCGYCADKEYGR16J5PJFS0/comments/ are bot workflow/contract records only; 06EZ4D6N9VRC5BJTAC7N92WEZG.md restates the refinement contract and later comments are claim/lease/handover records with no unresolved human discussion.
- `git log --oneline --first-parent` on ticket/06EZ0NCGYCADKEYGR16J5PJFS0-task-emit-provider-comparison-benchmark-artifact shows only workflow commits after develop (`b04a1d90`, `f95fdcd6`, `<redacted>`, `ccad24b3` before `26988d0c`), so PO-critic is reviewing the refined ticket against the repo baseline rather than against hidden implementation work.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs currently runs 6 baselines: customer-profile-history (`conventional-ef`, `dvault-explicit-save`), customer-profile-bulk-history (`conventional-ef-bulk`, `dvault-bulk-save`), and order-product-fulfillment-history (`conventional-ef`, `dvault-explicit-save`).
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs currently emits only scenario/baseline/iterations/timing/persisted-outcome columns plus BenchmarkArtifactDocument(Context, Results), which matches the ticket's scope to extend an existing artifact schema rather than invent a new pipeline.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs exposes provider-neutral `AddDVault()`, while src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs calls `AddDVault()` and then registers `IDataVaultProviderSaveStrategy` via `AddDVaultSqlite()`.
- src/DCoding.Data.DVault/DataVaultSaveService.cs orders `IDataVaultProviderSaveStrategy` registrations by descending `Priority` and falls back to the built-in writer when none can save, directly supporting the ticket's fallback-vs-optimized comparison model.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs already proves both provider-neutral `AddDVault()` fallback and SQLite-optimized `AddDVaultSqlite()` selection on the same SQLite provider.
- benchmarks/DCoding.Data.DVault.Benchmarks/README.md and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs already cover one-run markdown/CSV/JSON artifact emission and the current SQLite-only benchmark harness.
- `git -C /mnt/c/Projects/DVault ls-files benchmarks/DCoding.Data.DVault.Benchmarks/bin benchmarks/DCoding.Data.DVault.Benchmarks/obj artifacts` returned no tracked files, which aligns with the ticket's output-path constraint.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not give example dataset-size and change-ratio tokens for the retained smaller scenarios (`customer-profile-history` and `order-product-fulfillment-history`); implementation will need to choose stable labels and lock them down in tests/docs.
- The contract does not include a sample artifact row showing how provider, strategy family, dataset size, and change ratio should read together across markdown, CSV, and JSON.

Risky assumptions
- The ticket assumes `represented SQLite scenario` means every scenario left in the runner/README, not only the two large customer-profile workloads.
- The ticket assumes the parent story's skipped-external-provider reporting remains intentionally deferred and will not be expected from this SQLite-only child slice.

AC / test suggestions
- Have automated artifact assertions compare the same logical row across markdown, CSV, and JSON for at least one fallback/optimized/classic triplet, not just row counts.
- Add explicit assertions that the same SQLite provider can yield both `AddDVault()` fallback rows and `AddDVaultSqlite()` optimized rows, so strategy family is proven independently of provider name.

Implementation watchouts
- Current benchmark docs and tests assume 6 baselines; adding fallback plus the large insert-only scenario will change total row counts and README wording in benchmarks/DCoding.Data.DVault.Benchmarks/README.md and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs.
- Current artifact schema in benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs only models scenario/baseline/timing/persisted outcome, so the new metadata must be carried consistently in markdown, CSV, and JSON without dropping the existing run-context fields.
- Current README text says the executable registers the SQLite package so DVault timings include the SQLite provider save strategy; documentation must be updated so fallback `AddDVault()` rows are not obscured by SQLite-only optimized wording.

Non-blocking notes
- The benchmark README, runner, artifact writer, and integration tests already exist in one cohesive harness, so this is a bounded extension rather than a greenfield benchmark pipeline.
- The persisted contract already names the exact large change-heavy baseline (`customer-profile-bulk-history`, 100 customers, 10 profile states each) and explicitly scopes live external providers out.

Split recommendations
- No additional split is needed for this ticket. Keep skipped-provider reporting and any non-SQLite provider expansion in the follow-up story/tickets already referenced by the contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment