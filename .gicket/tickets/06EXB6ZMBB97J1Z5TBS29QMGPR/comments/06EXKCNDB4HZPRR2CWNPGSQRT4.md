[gicket-bot] PO-critic review contract

Summary
- The narrowed AddDVault smoke-test ticket is ready for developer handoff; the persisted contract has no open questions and repository evidence confirms the public AddDVault surface and existing DVault test structure.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket description contains ## Open Questions with only '- none', so the contract has no unresolved open questions blocking approve_for_dev.
- Persisted delivery contract Scope In asks for one self-contained smoke test under tests/DVault.Tests for the default optionless AddDVault startup path, with same IServiceCollection return, service-provider build, and provider-neutral defaults resolution.
- Persisted delivery contract Scope Out explicitly excludes UseDataVault, EF provider integration, consuming DbContext behavior, repository scaffold changes, and new public startup APIs.
- repository-list-directory tests/DVault.Tests returned DVault.Tests.csproj plus Integration, Modeling, Shared, and Unit directories, including tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs and tests/DVault.Tests/Unit/TestDiscoverySmokeTests.cs.
- repository-list-directory src/DVault returned src/DVault/DVaultServiceCollectionExtensions.cs plus Modeling files including DataVaultConventions.cs and DefaultNamingPolicy.cs.
- repository-read-text src/DVault/DVaultServiceCollectionExtensions.cs shows public static IServiceCollection AddDVault(this IServiceCollection services), null validation, registrations for DefaultNamingPolicy.Instance and DataVaultConventions.Default, and return services.
- repository-read-text tests/DVault.Tests/DVault.Tests.csproj shows net10.0 executable test project with ProjectReference to ..\..\src\DVault\DVault.csproj and VSTest target executing dotnet "$(TargetPath)".
- repository-read-text tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs shows existing executable test style with internal static class, Main test runner, local assertion helpers, and current AddDVault-related tests.
- git rev-parse HEAD returned 8662ca70f1e59a80d41cf6bc52713e1a01600d5b, matching the scratch-source-ref, and git show identifies it as the ticket branch lease-claim commit on ticket/06EXB6ZMBB97J1Z5TBS29QMGPR-task-add-smoke-tests-for-minimal-startup.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Keep the acceptance criteria focused on observable public AddDVault behavior: optionless call, same IServiceCollection returned, ServiceProvider builds, and DefaultNamingPolicy/DataVaultConventions resolve to provider-neutral defaults.
- Avoid descriptor-order assertions; provider behavior and resolved public defaults are the right level for this smoke test.

Implementation watchouts
- Place the test where the existing tests/DVault.Tests execution path will actually discover or run it; the repository has executable-style Modeling tests and separate Unit/Integration test projects.
- Do not introduce UseDataVault, EF provider wiring, a consuming DbContext, external database dependencies, solution/project scaffolding, or a new public startup API under this ticket.
- If AddDVault(IServiceCollection) disappears before development, the persisted contract says to return to PO refinement rather than inventing a replacement startup surface.

Non-blocking notes
- Existing tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs already contains AddDVault descriptor-level coverage, so the requested work should add the missing service-provider smoke coverage rather than broadening scope.
- Assurance is low and gated roles are dev and test; no PO-critic satisfied label is configured.

Split recommendations
- No split is needed for the current narrowed AddDVault smoke-test scope.
- Create follow-up tickets only for UseDataVault, EF-specific startup wiring, consuming DbContext behavior, provider integration, or broader startup/configuration regression coverage.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment