[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is well bounded overall, but it does not define the minimal observable EF-model effect that proves the new ModelBuilder entry point applies default DVault conventions without overlapping the downstream EF metadata-translation ticket.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7FPZRCFC33RF2M5SXZTK4/description.md` contains a persisted delivery contract with `## Open Questions` set to `- none`, and it says this ticket blocks `06EXB7FYXNBPMH8VGQCGP2R41R`.
- `.gicket/tickets/06EXB7FPZRCFC33RF2M5SXZTK4/comments/*.md` are all bot-generated claim/lease/handover/refinement comments; no human clarification comment was present in the local comment history.
- `src/DCoding.Data.DVault/Modeling/DataVaultModelBuilderExtensions.cs` exposes only `UseDataVault(this DataVaultModelBuilder modelBuilder)`, and `src/DCoding.Data.DVault/Modeling/DataVaultModel.cs` shows the current observable behavior is setting `Conventions` to `DataVaultConventions.Default`.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` already provides a root-namespace optionless entry point and registers `DefaultNamingPolicy.Instance`, `DataVaultConventions.Default`, `DefaultStableHashService.Instance`, and `DefaultStableHashNormalizer.Instance`.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` targets `net10.0`, enables `GenerateDocumentationFile`, treats `CS1591` as warnings-as-errors, and only references `Microsoft.Extensions.DependencyInjection.Abstractions` Version `10.0.0`; `rg -n "Microsoft\.EntityFrameworkCore|EntityFramework" -g '*.csproj' src tests` found no EF Core package references in the repo projects.
- `tests/DCoding.Data.DVault.Tests/Modeling/DefaultNamingPolicyTests.cs` covers `AddDVault()` and the internal `DataVaultModelBuilder.UseDataVault()` path, but repository search `rg -n "EntityFrameworkCore|ModelBuilder|Annotation|SetAnnotation|HasAnnotation|IModel" src tests README.md docs` found no EF `ModelBuilder` or annotation assertion surface in source or tests.
- `.gicket/tickets/06EXB7FYXNBPMH8VGQCGP2R41R/description.md` says the blocked follow-up ticket owns mapping hubs, links, satellites, keys, indexes, and technical columns and that tests there inspect the generated EF model.
- `git log --oneline --decorate -n 8 -- .gicket/tickets/06EXB7FPZRCFC33RF2M5SXZTK4 src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests` shows HEAD `7a41ff44` is only the po-critic lease-claim commit, and `git diff --name-only b853d26d..HEAD -- src/DCoding.Data.DVault tests/DCoding.Data.DVault.Tests` returned no source or test file changes.

Blocking findings
- The contract requires the EF `ModelBuilder` extension to `apply/record` the default DVault conventions, but the repository currently has no EF Core package reference, no EF-model metadata contract, and no existing test/assertion surface that makes that behavior concrete. Because blocked ticket `06EXB7FYXNBPMH8VGQCGP2R41R` separately owns EF model metadata translation, this ticket needs an explicit definition of the minimal observable EF-model effect it must create.

Required PO actions
- Amend the delivery contract to name the exact EF-model artifact this ticket must create or mutate to prove `ModelBuilder.UseDataVault()` is wired to `DataVaultConventions.Default` (for example, specific DVault-owned model annotation key/value(s) or another public inspection surface).
- State explicitly whether this ticket does or does not perform EF model metadata translation for hubs, links, satellites, keys, indexes, and technical columns, so its boundary is clean against blocked ticket `06EXB7FYXNBPMH8VGQCGP2R41R`.
- Update the acceptance criteria and test expectations to match that chosen contract, so dev does not have to guess between a near-no-op fluent wrapper and prematurely implementing downstream metadata work.

Open issues ledger
- critic-item-1 [required-po-action] Amend the delivery contract to name the exact EF-model artifact this ticket must create or mutate to prove `ModelBuilder.UseDataVault()` is wired to `DataVaultConventions.Default` (for example, specific DVault-owned model annotation key/value(s) or another public inspection surface).
- critic-item-2 [required-po-action] State explicitly whether this ticket does or does not perform EF model metadata translation for hubs, links, satellites, keys, indexes, and technical columns, so its boundary is clean against blocked ticket `06EXB7FYXNBPMH8VGQCGP2R41R`.
- critic-item-3 [required-po-action] Update the acceptance criteria and test expectations to match that chosen contract, so dev does not have to guess between a near-no-op fluent wrapper and prematurely implementing downstream metadata work.
- critic-item-4 [blocking-finding] The contract requires the EF `ModelBuilder` extension to `apply/record` the default DVault conventions, but the repository currently has no EF Core package reference, no EF-model metadata contract, and no existing test/assertion surface that makes that behavior concrete. Because blocked ticket `06EXB7FYXNBPMH8VGQCGP2R41R` separately owns EF model metadata translation, this ticket needs an explicit definition of the minimal observable EF-model effect it must create.

Missing examples / edge cases
- No concrete example shows what `DbContext.OnModelCreating` should be able to inspect or observe immediately after `modelBuilder.UseDataVault()` in this ticket.
- Repeat-call or idempotence behavior for `modelBuilder.UseDataVault()` is not stated.
- Interaction with ordinary non-DVault EF configuration in the same `OnModelCreating` method is not stated for this task.

Risky assumptions
- Assumes the EF Core package chosen for the `net10.0` baseline exposes a public, provider-neutral `ModelBuilder` construction path suitable for focused unit tests; the repo currently has no EF Core reference to verify that locally.
- Assumes a root-namespace `ModelBuilder.UseDataVault()` overload will remain unambiguous beside `DCoding.Data.DVault.Modeling.DataVaultModelBuilderExtensions.UseDataVault()` purely through parameter-type distinction.

AC / test suggestions
- Add one acceptance criterion that names the public observable EF-model outcome, not just that conventions are `applied/recorded`.
- If safe repeated calls are intended, add one explicit test expectation for idempotence.
- If preserving ordinary EF configuration is expected at this layer, add one explicit test that combines `UseDataVault()` with unrelated EF configuration on the same builder.

Implementation watchouts
- Keep the EF Core package version aligned with the repo's `net10.0` / `10.0.0` package baseline to avoid restore drift.
- Avoid provider-specific relational annotations or schema behavior in this entry-point ticket; the contract already scopes those out and the blocked follow-up ticket owns later EF metadata work.
- Place the overload under `DCoding.Data.DVault` so consumers do not need to import `DCoding.Data.DVault.Modeling`.

Non-blocking notes
- The persisted delivery contract's `## Open Questions` section is already resolved to `none`.
- The existing repository baseline for root namespace, docs generation, `CS1591`, default conventions, and the internal `DataVaultModelBuilder.UseDataVault()` path was directly confirmed from source.

Split recommendations
- Keep the existing split: this ticket for the public EF `ModelBuilder` entry point, and `06EXB7FYXNBPMH8VGQCGP2R41R` for richer EF model metadata translation. The needed change is scope clarification, not another child ticket.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment