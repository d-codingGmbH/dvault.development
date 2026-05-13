[gicket-bot] PO-critic review contract

Summary
- Delivery contract is source-backed, bounded, and ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XPW1N9PATP3R6YG53ZNGV0/description.md sets `### PO Handoff` to `ready_for_po_critic` and the authoritative contract block has `## Open Questions` -> `none`.
- `src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs` defines public `DataVaultModelArtifactImporter.ImportJson(string json, string? logicalSourcePath = null)`.
- `src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs` defines public `UseDataVaultMetadata(this DbContextOptionsBuilder, DataVaultModelImportResult)`, and `src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs` defines `Compare(DataVaultModelImportResult, DbContext)`.
- `src/DCoding.Data.DVault/DataVaultModelImportResult.cs` exposes public `Diagnostics`, `MetadataModel`, `MetadataRegistry`, and `LoadTimestampStorage`.
- `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj` already references `Microsoft.EntityFrameworkCore.Sqlite`, and `DVault.slnx` already includes the Unit test project.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactImporterTests.cs` already uses `UseSqlite` with `Data Source=:memory:` plus `UseDataVaultMetadata(importResult)` without opening a database, and separately asserts `DMV1002`, logical source path, and `/schemaVersion`.
- `docs/model-first-governance.md` already documents `UseDataVaultMetadata(DataVaultModelImportResult)`, `DataVaultModelDriftReporter.Compare(importResult, context)`, and the `DMV1002` diagnostic example.
- `test -e models/sales-vault.json` and `test -e dvault.model.v1` both returned exit code `1`, matching the contract clarification that no pre-existing artifact file is available.
- `git log --oneline --decorate -n 5` shows HEAD `9242c5a1b` on the claimed ticket branch; the earlier blocking comment `06F20ZRMZXD8BQQGSE2F6QN8R4.md` cited unresolved open questions, and the later PO refinement comment `06F21255R8KY02P0PNE190PM5R.md` resolves that with `Open questions - none`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not force the valid artifact to include more than the minimum projection shape, so reviewers should prefer a non-trivial sample if they want stronger workflow evidence.

Risky assumptions
- The reproduction command filter is intentionally unspecified; the implementation should choose a stable class-level or namespace-level filter rather than a brittle single-test name.
- The design-time-only constraint depends on stopping at model building and drift comparison; opening a connection or initializing a database would exceed scope.

AC / test suggestions
- Prefer one focused unit-test class that contains both the drift-clean path and the unsupported-schemaVersion path so the governance doc can point to one stable `dotnet test DVault.slnx --nologo --filter ...` command.
- Use at least one non-trivial declaration beyond a single hub, such as a satellite or link, so the drift-clean proof demonstrates meaningful projection parity.

Implementation watchouts
- Stay on the public surfaces named in the contract: `ImportJson`, `UseDataVaultMetadata(importResult)`, `Compare(importResult, context)`, and `DataVaultModelImportResult.Diagnostics`.
- Do not assume `models/sales-vault.json` exists; use inline JSON or a bounded new fixture if readability requires it.
- Keep the workflow design-time-only: use SQLite only for provider/model selection and do not open or initialize a database.

Non-blocking notes
- The invalid-artifact proof requested by this ticket overlaps existing `DMV1002` coverage in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactImporterTests.cs`; developers should extend or reuse that pattern rather than duplicate it mechanically.
- The optional narrative path `services.AddDVault(options => options.UseMetadataModel(importResult))` is source-backed in `src/DCoding.Data.DVault/DataVaultOptions.cs`, so mentioning it in docs is safe but not required for the main test proof.

Split recommendations
- No split is required for this ticket as currently bounded to unit-test coverage plus governance-doc updates.
- A runnable quickstart under `examples/`, CI gating, or a broader invalid-model matrix should stay separate follow-up tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment