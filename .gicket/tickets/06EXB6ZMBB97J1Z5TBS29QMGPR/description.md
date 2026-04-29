<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- The PO-critic label-cleanup evidence issue is reconciled: the live ticket still has blocker/workflow labels, so the product contract must not claim label cleanup was completed; the AddDVault smoke-test scope remains ready to continue.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The AddDVault-only delivery contract remains the intended implementation scope.
- The v1 startup surface for this ticket remains AddDVault(IServiceCollection) in src/DVault/DVaultServiceCollectionExtensions.cs.
- The smoke test must verify the optionless AddDVault path and must not require UseDataVault, EF provider integration, a consuming DbContext, or DbContext-specific startup behavior.
- tests/DVault.Tests is the existing DVault test area for this ticket, with DVault.Tests.csproj plus Modeling, Unit, Integration, and Shared test areas visible in branch evidence.
- No child tickets, outgoing relations, attachments, or planning documents were created in this pass.

### Scope In
- Add one self-contained smoke test under the existing tests/DVault.Tests structure for the default optionless AddDVault startup path.
- Use the branch's existing .NET test style to create a minimal ServiceCollection or equivalent startup path and call AddDVault with no DVault-specific configuration.
- Assert observable public startup success: AddDVault returns the same IServiceCollection, the service provider can be built, and provider-neutral defaults registered by AddDVault can be resolved.
- Keep the test deterministic and free of external databases, network services, and machine-specific infrastructure.

### Scope Out
- Creating or renaming solution files, source projects, package metadata, repository build configuration, or test scaffolding.
- Adding a new public startup API as part of this ticket.
- Creating, requiring, or testing UseDataVault, EF provider integration, a consuming DbContext, or DbContext-specific startup behavior.
- Provider-matrix coverage, broad configuration regression testing, SQLite integration behavior, and external database integration tests.

## Acceptance Criteria
- A smoke test in the existing DVault test suite exercises new ServiceCollection().AddDVault() or the branch-equivalent optionless AddDVault path and passes with default DVault configuration.
- The test fails if the current AddDVault startup path begins requiring mandatory DVault-specific configuration beyond convention-first defaults.
- The test validates public AddDVault behavior through service collection/provider behavior rather than private DI descriptor ordering.
- The test runs without external databases, network services, or machine-specific infrastructure.
- The test is discoverable and executable through the existing DVault .NET test project or solution command for the branch.

## Definition of Done
- The smoke test is implemented under the existing tests/DVault.Tests structure using the current repository test pattern.
- The relevant existing DVault test project or solution test command passes for the affected suite.
- The test uses the source-backed public AddDVault(IServiceCollection) startup surface currently defined in src/DVault/DVaultServiceCollectionExtensions.cs.
- No repository scaffold or new public startup API is introduced by this ticket.
- Repository formatting expectations remain satisfied for any touched test files.

## Implementation Notes
- Start from src/DVault/DVaultServiceCollectionExtensions.cs, which currently defines AddDVault(IServiceCollection), validates null services, registers DefaultNamingPolicy.Instance and DataVaultConventions.Default, and returns the same IServiceCollection.
- Use tests/DVault.Tests/DVault.Tests.csproj and nearby tests/DVault.Tests/Modeling or tests/DVault.Tests/Unit patterns to place the smoke test where the branch's normal DVault test command will execute it.
- For the current v1 target, a minimal ServiceCollection plus service provider build is sufficient; do not introduce host, EF, provider, or database dependencies solely for this ticket.
- Reasonable observable assertions are that AddDVault returns the same IServiceCollection and the built provider resolves DefaultNamingPolicy and DataVaultConventions to the provider-neutral defaults registered by the visible AddDVault implementation.
- If the branch changes before development and AddDVault(IServiceCollection) is no longer present, return to PO refinement rather than creating a different startup API under this ticket.

## Open Questions
- none

## Follow-Up Questions
- Decide separately whether DVault needs a public UseDataVault, EF provider integration, or DbContext-specific startup API and create a dedicated implementation ticket if so.
- After the AddDVault smoke test lands, decide separately whether provider-specific startup coverage or broader configuration regression tests are needed.

## Risks
- A test that asserts private DI registration mechanics instead of public startup success may become brittle.
- Because the repository contains multiple DVault test layouts, the developer must place the smoke test where the branch's normal test command will actually execute it.

## Split Recommendations
- No prerequisite setup split is required for the current narrowed AddDVault smoke-test scope because the branch contains the source project, source-backed AddDVault entry point, and DVault test structure.
- Create a separate follow-up ticket only if UseDataVault, EF-specific startup wiring, a consuming DbContext path, provider integration, or a new public startup surface becomes required.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Protect the minimal configuration experience with tests.

## Scope
- Create a small consuming DbContext and verify startup succeeds with defaults.

## Acceptance Criteria
- Smoke tests fail if required setup becomes noisy.
- The test does not require external databases.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.