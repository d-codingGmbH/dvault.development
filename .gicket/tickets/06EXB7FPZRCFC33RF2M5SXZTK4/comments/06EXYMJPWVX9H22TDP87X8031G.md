[gicket-bot] PO-critic review contract

Summary
- Ticket 06EXB7FPZRCFC33RF2M5SXZTK4 is sufficiently refined for developer handoff: the contract is explicit about the single EF model-annotation behavior, the non-goals are bounded, open questions are resolved, and the split to the downstream EF-translation ticket is backed by repository and gicket evidence.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted delivery contract explicitly scopes this ticket to a root-namespace `ModelBuilder.UseDataVault()` that sets model annotation `DCoding.Data.DVault:Conventions` to `DataVaultConventions.Default`, excludes EF entity/property/key/index translation, and shows `## Open Questions` as `- none` in `.gicket/tickets/06EXB7FPZRCFC33RF2M5SXZTK4/description.md:18-67` and `:55-56`.
- Repository source confirms the existing public default payload exists today: `DataVaultConventions.Default` is defined in `src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs:8-55` with the documented default naming policy, MVP concept set, hash ids, and logical object names.
- Repository source confirms the existing non-EF convention wiring pattern the ticket wants to mirror: `src/DCoding.Data.DVault/Modeling/DataVaultModelBuilderExtensions.cs:13-18` applies `DataVaultConventions.Default`, and `src/DCoding.Data.DVault/Modeling/DataVaultModel.cs:53-61` exposes `Conventions` and `IsDataVaultEnabled` on `DataVaultModelBuilder`.
- Current tests already validate the non-EF same-instance convention wiring, which gives a concrete local precedent for the new EF-facing tests: `tests/DCoding.Data.DVault.Tests/Modeling/DefaultNamingPolicyTests.cs:167-198` checks fluent return, enablement, `Same(DataVaultConventions.Default, modelBuilder.Conventions)`, and default convention values.
- The main library project is `net10.0`, generates XML docs, and treats `CS1591` as errors in `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj:3-29`; the same file currently references only `Microsoft.Extensions.DependencyInjection.Abstractions`, and a repo-wide grep over `src/` and `tests/` returned no existing `Microsoft.EntityFrameworkCore` or `DCoding.Data.DVault:Conventions` matches, so the contract correctly identifies missing EF-package and EF-annotation work.
- The split boundary is real in persisted gicket data: `.gicket/relations/K4/1R/06EXB7FPZRCFC33RF2M5SXZTK4--06EXB7FYXNBPMH8VGQCGP2R41R--blocks.json` records this ticket blocking `06EXB7FYXNBPMH8VGQCGP2R41R`, and that blocked ticket still owns mapping hubs/links/satellites/keys/indexes in `.gicket/tickets/06EXB7FYXNBPMH8VGQCGP2R41R/description.md:1-9`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not explicitly say whether repeated `ModelBuilder.UseDataVault()` calls must be idempotent beyond ending with the same `DataVaultConventions.Default` instance on the model annotation.
- The negative-translation AC is clear at a scope level, but it does not give a concrete example model with an ordinary entity already present; adding that example would make the absence-of-extra-EF-metadata expectation more executable.

Risky assumptions
- Implementation will need to choose a net10-compatible EF Core package/version even though the contract intentionally leaves the exact package id/version to repository-aligned developer judgment.
- Consumers may call `UseDataVault()` from multiple places in `OnModelCreating`; the contract assumes overwrite/idempotent behavior will be handled consistently even though repeated-call semantics are not spelled out.

AC / test suggestions
- Add one focused test that calls `UseDataVault()` twice and asserts the model annotation still points to the same `DataVaultConventions.Default` instance.
- Add one focused EF test with a normal entity in the model and assert that a bare `UseDataVault()` call adds only the model-level `DCoding.Data.DVault:Conventions` annotation and no DVault-specific entity/property/key/index metadata.

Implementation watchouts
- Keep the new extension in namespace `DCoding.Data.DVault` and typed specifically to `Microsoft.EntityFrameworkCore.ModelBuilder` so it does not collide with the existing `DCoding.Data.DVault.Modeling.DataVaultModelBuilderExtensions.UseDataVault()` surface.
- Do not let this ticket absorb hub/link/satellite/key/index/technical-column translation work; that boundary is explicitly deferred to ticket `06EXB7FYXNBPMH8VGQCGP2R41R`.
- Because `DCoding.Data.DVault:Conventions` becomes a public inspection key, renaming the annotation key or changing the payload away from `DataVaultConventions.Default` would break the refined contract.

Non-blocking notes
- The branch tip inspected for this review is `40a8d013225bd3cf6cae6f2685e01c7bffca8f66`, and the last visible history entries for this ticket are workflow handoff/lease commits rather than new product-code commits.
- The repo already has a root-namespace convention-first precedent in `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:4-25`, which fits the requested root-namespace EF entry point style.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment