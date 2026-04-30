[gicket-bot] PO-critic review contract

Summary
- Ticket contract is ready for developer handoff; it has no unresolved Open Questions and the persisted scope is supported by direct repository evidence.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket 06EXB6X2YG4RW5JTSYH2FENJK0 delivery contract states PO Handoff decision ready_for_po_critic and ## Open Questions contains only 'none'.
- git status --short returned no output, so the review worktree was clean at HEAD b3b490c.
- git show --stat 32eebdf227bc shows the PO handoff commit for ticket 06EXB6X2YG4RW5JTSYH2FENJK0 updated the ticket description/comments/events only, with 220 insertions and 6 deletions.
- git ls-files *.slnx src/**/*.csproj tests/**/*.csproj returned DVault.slnx, src/DCoding.Data.DVault/DCoding.Data.DVault.csproj, and Integration/Shared/Unit test projects under tests/DCoding.Data.DVault.Tests.
- README.md documents DVault.slnx as the canonical root build/test entry point, src/DCoding.Data.DVault as the main library, tests/DCoding.Data.DVault.Tests as the test root, and validation commands dotnet build, dotnet test, dotnet pack, and bash tools/check-format.sh.
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj targets net10.0 and declares RootNamespace and PackageId as DCoding.Data.DVault, with Apache-2.0 license metadata, repository metadata, README packing, symbols, snupkg, and package output path.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs directly defines public static IServiceCollection AddDVault(this IServiceCollection services), calls ArgumentNullException.ThrowIfNull, registers DefaultNamingPolicy.Instance and DataVaultConventions.Default via TryAddSingleton, and returns the same service collection.
- tests/DCoding.Data.DVault.Tests/Modeling/DefaultNamingPolicyTests.cs includes tests for AddDVault no-option overload discoverability, fluent return, default registrations, UseDataVault no-option behavior, and default conventions.
- src/DCoding.Data.DVault/TechnicalMetadataColumnRole.cs defines exactly HashKey, HashDiff, LoadTimestamp, and RecordSource; TechnicalMetadataColumnRequiredness.cs defines RequiredWhenDeclared; TechnicalMetadataColumnContract.cs builds the four default contracts with the expected names.
- tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs covers the closed four-role set, default/effective names, override preservation, and RequiredWhenDeclared semantics.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Build, test, pack, and formatting command results were not present in the available tool results; the contract already allows environment-only blockers to be recorded by implementation if local .NET 10 or .slnx-capable tooling is unavailable.

AC / test suggestions
- Keep the current AC requiring dotnet build, dotnet test, dotnet pack, and bash tools/check-format.sh results or a concrete environment-only blocker in the developer handoff evidence.

Implementation watchouts
- Do not expand this epic into provider-specific persistence, migrations, schema generation, CI publishing, or advanced configuration hooks; those are explicitly out of scope.
- Preserve AddDVault as optionless for v1 and keep DefaultNamingPolicy.Instance plus DataVaultConventions.Default registration idempotent with existing default registrations.
- Use current README/csproj paths as authoritative over historical child-story references to src/DVault.

Non-blocking notes
- No new split is needed; the contract already maps existing child tickets 06EXB6XBV95E08R2W9ZQ1PRDPM, 06EXB6YBXPDBPWZPNV89A9F9AM, and 06EXB6Z3YMAPSRYRB8NQX3ZST4 to skeleton, package metadata, and convention-first entry-point slices.
- The historical child-ticket path cleanup can remain a later hygiene item and should not block this epic handoff.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment