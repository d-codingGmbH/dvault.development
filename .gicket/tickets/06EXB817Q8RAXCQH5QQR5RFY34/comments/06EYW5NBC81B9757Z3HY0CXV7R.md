[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract is bounded to six packable DVault packages, anchors the relevant public API surface in source, and leaves no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB817Q8RAXCQH5QQR5RFY34/description.md` contains `## Open Questions` -> `none`, scopes the work to `src/DCoding.Data.DVault/`, `src/DCoding.Data.DVault.Sqlite/`, `src/DCoding.Data.DVault.Postgres/`, `src/DCoding.Data.DVault.SqlServer/`, `src/DCoding.Data.DVault.Oracle/`, and `src/DCoding.Data.DVault.MySql/`, and explicitly scopes out `src/DCoding.Data/DCoding.Data.csproj`, tests, and benchmarks.
- `rg -n '^.?\[gicket-bot\]' .gicket/tickets/06EXB817Q8RAXCQH5QQR5RFY34/comments/*.md` matched every local comment file; the latest files `06EYW43RH107G2XZPAT3ZRPK50.md`, `06EYW48FEVWHN7B565B6QRBYDM.md`, and `06EYW48KZ7Q7EAZ1QD4PSBVX7R.md` are bot run/claim/lease comments rather than human scope changes.
- Relation files `.gicket/relations/0M/34/06EXB80ZNQTTGT6VN2DKEDGB0M--06EXB817Q8RAXCQH5QQR5RFY34--parentOf.json`, `.gicket/relations/34/8G/06EXB817Q8RAXCQH5QQR5RFY34--06EXB81FSWAA6N1HMYQ0CM4S8G--blocks.json`, `.gicket/relations/K4/34/06EXB7HPGW3Y9MSP10DEC8RBK4--06EXB817Q8RAXCQH5QQR5RFY34--blocks.json`, and `.gicket/relations/J0/34/06EXB7J6HCA9QZ3DPP5Z03YGJ0--06EXB817Q8RAXCQH5QQR5RFY34--blocks.json` show parent story `06EXB80ZNQTTGT6VN2DKEDGB0M`, downstream blocked ticket `06EXB81FSWAA6N1HMYQ0CM4S8G`, and incoming blockers `06EXB7HPGW3Y9MSP10DEC8RBK4` and `06EXB7J6HCA9QZ3DPP5Z03YGJ0`; the related `ticket.json` files show both incoming blockers are already `done`.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` and the five provider project files each declare `GenerateDocumentationFile>true`, `PackageOutputPath $(MSBuildThisFileDirectory)../../bin/packages/`, and `WarningsAsErrors $(WarningsAsErrors);CS1591` (`src/DCoding.Data.DVault/DCoding.Data.DVault.csproj:7,17,20`; provider project files at `:7,17,20`).
- `src/DCoding.Data/DCoding.Data.csproj:6` and the benchmark/test project files (`benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj:8`, `tests/DCoding.Data.DVault.Tests/*/*.csproj`) mark the non-release surfaces `IsPackable>false`, matching the contract's scope-out.
- Source/API evidence matches the named acceptance-criteria surface: `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16` (`AddDVault`), provider extension files `src/DCoding.Data.DVault.{Sqlite,Postgres,SqlServer,Oracle,MySql}/*ServiceCollectionExtensions.cs:14`, `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:15,29` (`UseDataVault`, `ApplyDataVaultMetadata`), `src/DCoding.Data.DVault/DataVaultSaveService.cs:10` (`IDataVaultSaveService`), and `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:129,229` (provider capability contracts); the same surface is referenced in `README.md:30,50,77,117-121` and `docs/architecture/dvault-v1-explicit-save-service.md:8,10,14,25`.
- Branch history is limited to ticket orchestration/refinement commits: `git log --oneline` shows `373a2cff` -> `452bdc1d` -> `70b4a8cb` -> `<redacted>`, and `git diff --name-only 373a2cff..<redacted>` changes only `.gicket/**` files, with no source-tree drift during PO refinement.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No direct source matches for `protected` were found under the six packable `src/DCoding.Data.DVault*` roots (`rg -n '\bprotected\b' ...` returned no matches), so developer verification should state whether the gate is effectively public-only on this branch.
- `README.md:163` shows a `dotnet pack` example only for `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`; implementation proof should explicitly cover the five provider packages as well.

Risky assumptions
- Assuming the existing `GenerateDocumentationFile` plus `WarningsAsErrors ... CS1591` settings are sufficient without extra analyzers; the current ticket is correct only if developers verify missing-doc failures and do not rely on property presence alone.
- Assuming package-shipped XML docs will be proven from real pack artifacts in `bin/packages/` or package contents, not inferred from `PackageOutputPath`/`GenerateDocumentationFile` settings.

AC / test suggestions
- Require a verification pass that packs all six packable projects, not just the core package shown in `README.md:163`.
- Capture artifact-level proof for each package that the generated `.xml` documentation file is present with the pack output.
- Capture proof that no broad `CS1591` suppression was introduced; current repo search found only per-project `WarningsAsErrors ... CS1591` entries and no `NoWarn` or `pragma` suppressions under `src/`.

Implementation watchouts
- The current public surface is broader than the README examples; `rg` also found public modeling and save-contract types under `src/DCoding.Data.DVault/Modeling/*.cs` and `src/DCoding.Data.DVault/DataVaultSaveService.cs`, so CS1591 work may extend beyond the named examples.
- If the XML-doc settings are centralized, keep the condition scoped to the six packable packages so `src/DCoding.Data/DCoding.Data.csproj`, tests, and `benchmarks/DCoding.Data.DVault.Benchmarks/` do not start failing unintentionally.
- Do not treat current project-file settings as completion by themselves; the contract still requires build-gate proof and pack-output proof.

Non-blocking notes
- The ticket already has the refined PO contract persisted and `## Open Questions` is `none`, so there is no PO-level dependency on further clarification before dev pickup.
- All local comments on this ticket are bot-authored automation comments; no human comment or attachment changed scope after the persisted contract was written.
- The parent quality story `06EXB80ZNQTTGT6VN2DKEDGB0M` remains `todo`, but that does not block this child task from moving to dev.

Split recommendations
- No split recommended; the downstream API approval/snapshot testing work is already separated into ticket `06EXB81FSWAA6N1HMYQ0CM4S8G`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment