[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the contract is specific, `## Open Questions` is resolved to `none`, and the remaining work is a bounded documentation/release-note update against directly observed repo gaps.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGP2B2RZGGK3CVKK5WRRP8/description.md` contains the delivery contract, `## Open Questions` with `- none`, and acceptance criteria naming `docs/releases/v0.14.0.md`, `README.md`, `examples/README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, and `docs/architecture/dvault-v1-explicit-save-service.md`.
- `ls docs/releases` lists `v0.5.0.md` through `v0.13.0.md` and no `v0.14.0.md`.
- `README.md` still uses `0.13.0` package versions and has `## v0.13.0 Release Notes` / `## Current v0.13.0 Limitations`; `examples/README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, and `docs/model-first-governance.md` also still present `0.13.0` as the current public baseline.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` already exposes `IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest)` and the registry-backed `SaveAsync(..., DataVaultRegistryBulkSaveRequest)`, so the public bulk surface exists and the ticket is documenting shipped behavior rather than inventing new API.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` is a direct source of the native-save gates: dirty `DbContext` and multi-active satellite batches decline, SQL Server requires at least `50` total operations and at most `500` satellite operations, MySQL requires at least `50`, Oracle requires at least `50`, and MySQL provider-name matching accepts both `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`.
- `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkArtifacts.cs` confirm the artifact contract `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`, with preserved `executionStatus`, `skipReason`, provider, and hardware/runtime context.
- `docs/architecture/dvault-v1-explicit-save-service.md` still says the matrix is `release-scoped to v0.5` and that benchmark artifact scope is SQLite plus optional PostgreSQL only, which now lags the benchmark README's optional SQL Server/MySQL/Oracle bulk rows.
- Branch history is still metadata-only: `git show --stat 978be6e33162` and `git show --stat 97471f300fc6563b5f012c55291fd660c66ff078` touch only `.gicket/...`, and `git diff --name-only develop...HEAD -- README.md docs examples benchmarks src/DCoding.Data.DVault.Analyzers/README.md` returned no paths.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Public docs should explicitly state that optional-provider benchmark rows may appear as `executionStatus=skipped` with a `skipReason`, not disappear from the artifact set.
- Docs should call out dirty `DbContext` state and multi-active satellite batches as fallback-to-provider-neutral cases, not only the happy-path native bulk batches.
- Provider-eligibility wording should avoid implying the SQL Server/MySQL/Oracle thresholds also apply to PostgreSQL.

Risky assumptions
- The PO note says the README MySQL section still needs parity, but direct repo inspection shows a live MySQL opt-in lane already exists in `README.md`; the remaining MySQL delta may be elsewhere or already satisfied.
- The named current-baseline docs in the contract are not exhaustive; `docs/model-first-governance.md` still declares `Status: v0.13.0 public guidance` and will also need alignment.

AC / test suggestions
- Use `docs/releases/v0.13.0.md` as the structural template for `docs/releases/v0.14.0.md`, including the validation-evidence command set (`dotnet build`, `dotnet test`, `dotnet pack`, `bash tools/verify-packages.sh`, `bash tools/check-format.sh`).
- Source all provider-native eligibility wording from `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` rather than from older prose.
- Cross-check performance/release-note wording against `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and `BenchmarkArtifacts.cs` so copied results preserve provider, skip, and hardware/runtime context.

Implementation watchouts
- Add `docs/releases/v0.14.0.md` as a new historical release record; do not rewrite historical notes such as `docs/releases/v0.5.0.md`.
- Update current-guidance files together; `docs/architecture/dvault-v1-explicit-save-service.md` currently contradicts the benchmark README on optional-provider benchmark scope.
- Do not generalize the 50-operation native-save threshold to PostgreSQL; `DataVaultDiagnostics.cs` applies minimum-operation gates to SQL Server, MySQL, and Oracle only.
- Do not regress the existing MySQL opt-in command and conditional restore-marker wording already present in `README.md`.

Non-blocking notes
- Comment history under `.gicket/tickets/06F2PGP2B2RZGGK3CVKK5WRRP8/comments/` is automation-only plus the PO refinement contract; no conflicting human clarification was found.
- Current HEAD `97471f300fc6563b5f012c55291fd660c66ff078` matches the supplied scratch-source-ref and only adds po/po-critic claim metadata, so the branch still needs the actual documentation edits.

Split recommendations
- No split recommended; bulk SPI, provider-native strategy, external-provider coverage, and benchmark work are already separated into done sibling tickets `06F2PGMSQ4D4FV8W5ZERD4GS8C`, `06F2PGNGVQ3TZZWSABAK5SNFK4`, `06F2PGNT7DF4DVNKYWDFZC8DEM`, and `06F2PGNZBRNCQ1SV2KKP6F3BA8`.
- If later desired, open a follow-up docs/example ticket for runnable bulk quickstarts or checked-in benchmark artifacts instead of widening this release-note closure task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment