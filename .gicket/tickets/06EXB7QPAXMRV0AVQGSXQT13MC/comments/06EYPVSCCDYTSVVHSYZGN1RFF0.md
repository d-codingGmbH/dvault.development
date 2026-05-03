[gicket-bot] PO-critic review contract

Summary
- Contract and repo evidence are coherent, but this epic is explicitly a closure-only umbrella with no developer-owned implementation slice, so it should return to PO for closure-path/status cleanup rather than go to dev.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `README.md` directly documents the approved quickstart surface: source consumption from `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`, `.NET 10`, `services.AddDVault()`, `modelBuilder.ApplyDataVaultMetadata(...)`, explicit `IDataVaultSaveService`/`DataVaultSaveRequest` writes, and EF shared-type reads via `Set<Dictionary<string, object>>("LinkCustomerOrder")`.
- Direct source inspection confirms the cited API surface exists in repo code: `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` exposes public `AddDVault`, `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` exposes public `ApplyDataVaultMetadata`, `src/DCoding.Data.DVault/DataVaultSaveService.cs` defines public `IDataVaultSaveService` and `DataVaultSaveRequest`, and `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` sets `DataVaultProviderCapabilityProfiles.Sqlite`.
- `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs` enumerates four baselines (`CustomerProfilePlainEfBenchmark`, `CustomerProfileDataVaultBenchmark`, `OrderProductPlainEfBenchmark`, `OrderProductDataVaultBenchmark`), `BenchmarkArtifacts.cs` writes deterministic `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`, and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` says the harness uses SQLite temporary files only and no Postgres/Docker/secrets.
- `git rev-parse HEAD` and `git rev-parse ticket/06EXB7QPAXMRV0AVQGSXQT13MC-epic-examples-documentation-and-benchmarks` both returned `8bf9c6eaf0cd8c025cc1b4473568afe1cb316e08`; `git diff --stat 8c1a79e68267..8bf9c6eaf0cd8c025cc1b4473568afe1cb316e08` touched only `.gicket/comments`, `.gicket/events`, and `.gicket/tickets/*/ticket.json`, with no parent-level product-code or docs changes after PO handoff.

Blocking findings
- none

Required PO actions
- Keep the parent epic as coordination-only; if any residual scope remains, place it on the existing child tickets or a new follow-up ticket rather than reopening parent-owned implementation work.

Open issues ledger
- critic-item-1 [required-po-action] Keep the parent epic as coordination-only; if any residual scope remains, place it on the existing child tickets or a new follow-up ticket rather than reopening parent-owned implementation work.

Missing examples / edge cases
- none

Risky assumptions
- This contract assumes the workflow can close or advance a closure-only epic without forcing a `dev` phase; the current runtime success path (`po-critic` -> `dev`) conflicts with the no-parent-implementation contract.

AC / test suggestions
- For future closure-only parent tickets, add an explicit acceptance/workflow statement that PO-critic approval routes to closure/aggregation handling rather than `dev`.
- If the board template allows it, add a ticket-level closure checklist item that all linked child tickets are `done` before the parent leaves PO-critic.

Implementation watchouts
- Do not reopen parent-owned README/benchmark/product-code work on this epic; the contract and branch history both show the parent is only a coordination record.
- When benchmark results are cited, keep the generated provider/runtime/hardware context from `benchmark-summary.md` or `benchmark-summary.json` attached to the copied numbers.

Non-blocking notes
- `README.md` lists `examples/` as future runnable examples, and the repo currently contains `examples/.gitkeep`; that is consistent with the epic's follow-up-only stance rather than a current closure requirement.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` targets `net10.0` and packs `../../README.md` as the package README, so the canonical beginner surface is aligned with the contract.

Split recommendations
- No additional split is needed at the epic level; keep future standalone `examples/`, provider-specific docs, or benchmark-publication work as separate follow-up tickets or epics.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment