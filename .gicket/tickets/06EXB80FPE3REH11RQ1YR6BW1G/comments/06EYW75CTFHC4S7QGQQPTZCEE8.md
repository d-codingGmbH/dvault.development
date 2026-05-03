[gicket-bot] PO refinement contract

Summary
- Refined the contract to use the existing tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj surface as the only required selectable proof, replaced unsupported intra-unit category-filter assumptions with repo-local named group ownership plus xUnit bridges, kept existing ticket relations unchanged, and created no child tickets, relation writes, attachments, or planning documents.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now names one accepted repo-local selection proof: developers must use the existing executable Unit project at tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj as the selectable fast-test surface. Inside that project, the required grouping mechanism is named xUnit test classes or xUnit bridge entrypoints owned by the metadata/model-building, naming/options, hashing, and provider coverage buckets; the ticket no longer requires undocumented runner-selectable Trait or Category filters under xunit.v3.mtp-v1 / Microsoft Testing Platform.
- critic-item-2: `answered` - The contract now adds a concrete discoverability/selectability expectation: a developer must be able to target tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj as the fast local validation surface, and that run must execute the metadata, naming, hashing, provider, and bridged harness coverage without loading tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj.
- critic-item-3: `answered` - The required bridge granularity is now explicit: one xUnit bridge Fact per existing standalone harness or harness family is sufficient if it invokes the underlying Run or equivalent flow and preserves named subcase failure output. Individual internal subcases do not need independent runner-selectability. The existing Modeling bridge is therefore acceptable, and TechnicalMetadataColumnContractTests.cs may follow the same pattern.
- critic-item-4: `answered` - The old blocking ambiguity is removed from risk-only treatment and converted into explicit acceptance language. This ticket no longer asks developers to prove undocumented intra-project category filtering under xunit.v3.mtp-v1. The accepted local proof is the separate Unit project plus deterministic repo-local ownership through named xUnit test classes and bridge entrypoints, with Integration kept in its own csproj.

Clarifications
- The accepted selectable boundary for this ticket is the existing Unit project at tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj; the contract does not depend on undocumented Trait, Category, or Microsoft Testing Platform filter syntax inside that project.
- Inside the Unit project, grouping is a deterministic repo-local ownership rule expressed through named xUnit test classes or xUnit bridge entrypoints for metadata/model-building, naming/options, hashing, and provider registration/capability/strategy coverage.
- The existing modeling harnesses under tests/DCoding.Data.DVault.Tests/Modeling already follow the accepted bridge pattern through tests/DCoding.Data.DVault.Tests/Unit/ConventionFirstEntryPointCoverageTests.cs.
- The fast-only selection proof is the Unit project path itself, not the repository-root dotnet test command, which continues to cover the broader solution surface.
- Relation context is unchanged from the ticket snapshot: this ticket remains under 06EXB807MN08HABHTHVPKKNFMG, still blocks 06EXB80QQHAYH61RY4X3T1E8S0, and upstream hashing context 06EXB76NNRDP7WH1F2R5VYYPMR remains baseline-only input.
- No child tickets, relation writes, attachments, or planning documents were materialized in this refinement run.

Scope In
- Ratify the existing Unit csproj as the required fast local selection surface for this ticket.
- Keep metadata/model-building coverage centered on DataVaultMetadataTests, DataVaultModelBuilderExtensionsTests, DataVaultEfMetadataTranslationTests, and bridged technical metadata contract coverage inside the Unit project.
- Keep naming/options coverage centered on the linked Modeling/DefaultNamingPolicyTests.cs and Modeling/NamingPolicyTests.cs harnesses through the existing xUnit bridge pattern plus ConventionFirstEntryPointCoverageTests.
- Keep hashing coverage centered on StableHashNormalizerTests and StableHashServiceTests, using the completed hashing edge-case baseline as existing context rather than new scope.
- Keep provider registration, provider capability, and provider strategy boundary coverage centered on ExplicitDataVaultSaveServiceTests and DataVaultProviderCapabilityProfileTests.
- Bridge the standalone TechnicalMetadataColumnContractTests.cs harness into the runnable Unit surface using the same accepted bridge style as the existing modeling harnesses.

Scope Out
- Introducing or proving runner-specific Trait, Category, or other intra-project filter syntax under xunit.v3.mtp-v1 / Microsoft Testing Platform.
- SQLite scenario, schema, and raw-storage integration tests under tests/DCoding.Data.DVault.Tests/Integration, including ExplicitDataVaultSaveServiceSqliteTests, SqliteDataVaultSchemaTests, and Integration/SqliteProviderCapabilityProfileTests.
- Opt-in external database checks and configuration gating for PostgreSQL, SQL Server, Oracle, or MySql integration paths; those remain with 06EXB80QQHAYH61RY4X3T1E8S0.
- Production-code behavior changes to save services, provider packages, metadata translation, naming, or hashing implementations.
- Creating extra subprojects, CI changes, or contributor workflow automation solely to achieve per-group CLI filtering inside the current Unit project.

Open questions
- none

Follow-up questions
- If future repo-local documentation or runner proof establishes supported class or test filtering under the current Microsoft Testing Platform setup, do we want a follow-up ticket to publish exact per-group CLI examples for metadata, naming, hashing, and provider coverage?
- If additional provider capability profiles are introduced after sqlite-v1, should each new profile inherit the same unit-group ownership rule plus a matching integration smoke surface by default?

Risks
- If future contributors add new fast tests without keeping them under the named unit-group ownership pattern, group discoverability inside the Unit project may drift even though project-level selectability remains intact.
- When bridging TechnicalMetadataColumnContractTests.cs, the implementation must preserve the existing named subcase visibility so failures still identify the broken metadata-contract expectation clearly.

Split recommendations
- No additional split is recommended; the integration-category boundary is already isolated in 06EXB80QQHAYH61RY4X3T1E8S0.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment