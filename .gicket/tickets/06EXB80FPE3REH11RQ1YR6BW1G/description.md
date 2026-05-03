<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the contract to use the existing tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj surface as the only required selectable proof, replaced unsupported intra-unit category-filter assumptions with repo-local named group ownership plus xUnit bridges, kept existing ticket relations unchanged, and created no child tickets, relation writes, attachments, or planning documents.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The accepted selectable boundary for this ticket is the existing Unit project at tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj; the contract does not depend on undocumented Trait, Category, or Microsoft Testing Platform filter syntax inside that project.
- Inside the Unit project, grouping is a deterministic repo-local ownership rule expressed through named xUnit test classes or xUnit bridge entrypoints for metadata/model-building, naming/options, hashing, and provider registration/capability/strategy coverage.
- The existing modeling harnesses under tests/DCoding.Data.DVault.Tests/Modeling already follow the accepted bridge pattern through tests/DCoding.Data.DVault.Tests/Unit/ConventionFirstEntryPointCoverageTests.cs.
- The fast-only selection proof is the Unit project path itself, not the repository-root dotnet test command, which continues to cover the broader solution surface.
- Relation context is unchanged from the ticket snapshot: this ticket remains under 06EXB807MN08HABHTHVPKKNFMG, still blocks 06EXB80QQHAYH61RY4X3T1E8S0, and upstream hashing context 06EXB76NNRDP7WH1F2R5VYYPMR remains baseline-only input.
- No child tickets, relation writes, attachments, or planning documents were materialized in this refinement run.

### Scope In
- Ratify the existing Unit csproj as the required fast local selection surface for this ticket.
- Keep metadata/model-building coverage centered on DataVaultMetadataTests, DataVaultModelBuilderExtensionsTests, DataVaultEfMetadataTranslationTests, and bridged technical metadata contract coverage inside the Unit project.
- Keep naming/options coverage centered on the linked Modeling/DefaultNamingPolicyTests.cs and Modeling/NamingPolicyTests.cs harnesses through the existing xUnit bridge pattern plus ConventionFirstEntryPointCoverageTests.
- Keep hashing coverage centered on StableHashNormalizerTests and StableHashServiceTests, using the completed hashing edge-case baseline as existing context rather than new scope.
- Keep provider registration, provider capability, and provider strategy boundary coverage centered on ExplicitDataVaultSaveServiceTests and DataVaultProviderCapabilityProfileTests.
- Bridge the standalone TechnicalMetadataColumnContractTests.cs harness into the runnable Unit surface using the same accepted bridge style as the existing modeling harnesses.

### Scope Out
- Introducing or proving runner-specific Trait, Category, or other intra-project filter syntax under xunit.v3.mtp-v1 / Microsoft Testing Platform.
- SQLite scenario, schema, and raw-storage integration tests under tests/DCoding.Data.DVault.Tests/Integration, including ExplicitDataVaultSaveServiceSqliteTests, SqliteDataVaultSchemaTests, and Integration/SqliteProviderCapabilityProfileTests.
- Opt-in external database checks and configuration gating for PostgreSQL, SQL Server, Oracle, or MySql integration paths; those remain with 06EXB80QQHAYH61RY4X3T1E8S0.
- Production-code behavior changes to save services, provider packages, metadata translation, naming, or hashing implementations.
- Creating extra subprojects, CI changes, or contributor workflow automation solely to achieve per-group CLI filtering inside the current Unit project.

## Acceptance Criteria
- The only required selectable fast-test proof for this ticket is the existing executable Unit project at tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj; the story does not require new runner-specific Trait or Category filters inside that project.
- Within that Unit project, metadata/model-building, naming/options, hashing, and provider registration/capability/strategy coverage remain discoverable as deterministic repo-local groups through named xUnit test classes or accepted xUnit bridge entrypoints, not through tests/DCoding.Data.DVault.Tests/Integration.
- A unit-only run targeted at tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj executes those fast groups without loading tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj.
- The metadata group includes provider-neutral model and contract coverage for UseDataVault, ApplyDataVaultMetadata, metadata object validation, produced names and ordinals, and the reusable technical metadata column contracts.
- The naming/options group includes the linked Modeling/DefaultNamingPolicyTests.cs and Modeling/NamingPolicyTests.cs harnesses through an xUnit bridge consistent with ConventionFirstEntryPointCoverageTests.
- The hashing group includes stable hash normalizer and hash service determinism, published digest vectors, and the null, culture, order, unsupported-type, and invalid-value edge cases visible in the current repository baseline.
- The provider group verifies the finite current package baseline: AddDVault resolves the core fallback services, PostgreSQL, SQL Server, Oracle, and MySql provider packages do not register an optimized provider strategy, AddDVaultSqlite does, and DataVaultProviderCapabilityProfiles.Sqlite remains covered.
- For standalone harnesses such as Modeling/*.cs and TechnicalMetadataColumnContractTests.cs, one xUnit bridge Fact per harness or harness family is sufficient if it drives the underlying Run or equivalent flow and preserves named internal subcase failure output; independent runner-selectability of every internal subcase is not required.

## Definition of Done
- The agreed grouping is implemented inside tests/DCoding.Data.DVault.Tests/Unit so the Unit project path remains the fast local selection surface for this ticket.
- Existing linked Modeling/*.cs coverage remains connected through the current xUnit bridge pattern instead of becoming an orphaned side harness.
- The standalone tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs harness is folded into the runnable Unit surface through an equivalent xUnit bridge, and its named internal cases remain visible on failure.
- No tests are moved out of tests/DCoding.Data.DVault.Tests/Integration to satisfy this ticket.
- Shared standards from docs/plans/shared-implementation-standards.md are still followed.

## Implementation Notes
- Use tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj as the only required runner-selectable surface for this ticket.
- Do not introduce a contract dependency on undocumented Trait, Category, or Microsoft Testing Platform filter behavior; class or file ownership and bridge entrypoints are the accepted grouping mechanism.
- Reuse the existing bridge style from tests/DCoding.Data.DVault.Tests/Unit/ConventionFirstEntryPointCoverageTests.cs: a bridge xUnit Fact may call a harness Run method that internally covers multiple deterministic subcases.
- Bring tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs behind the Unit project in the same style rather than leaving it as a standalone Main executable harness.
- Preserve the current architectural boundary where unit coverage proves registration and contract behavior, while SQLite runtime behavior and opt-in external-provider behavior stay in tests/DCoding.Data.DVault.Tests/Integration.
- No child tickets, relation updates, attachments, or planning documents were created during this refinement run.

## Open Questions
- none

## Follow-Up Questions
- If future repo-local documentation or runner proof establishes supported class or test filtering under the current Microsoft Testing Platform setup, do we want a follow-up ticket to publish exact per-group CLI examples for metadata, naming, hashing, and provider coverage?
- If additional provider capability profiles are introduced after sqlite-v1, should each new profile inherit the same unit-group ownership rule plus a matching integration smoke surface by default?

## Risks
- If future contributors add new fast tests without keeping them under the named unit-group ownership pattern, group discoverability inside the Unit project may drift even though project-level selectability remains intact.
- When bridging TechnicalMetadataColumnContractTests.cs, the implementation must preserve the existing named subcase visibility so failures still identify the broken metadata-contract expectation clearly.

## Split Recommendations
- No additional split is recommended; the integration-category boundary is already isolated in 06EXB80QQHAYH61RY4X3T1E8S0.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Organize fast unit tests for core behavior and provider package registration.

## Current Baseline
- Core behavior now includes metadata, naming, hashing, options, provider capability contracts, and provider save strategy dispatch.
- Provider packages should be covered by fast registration tests that do not require external database servers.

## Scope
- Tag or group tests for metadata, naming, hashing, options, provider registration, and provider strategy selection boundaries.

## Acceptance Criteria
- Unit tests are fast and deterministic.
- Coverage includes edge cases from modeling stories.
- Provider package registration tests verify expected core fallback behavior and the SQLite optimized strategy registration.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.