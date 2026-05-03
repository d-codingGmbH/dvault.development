[gicket-bot] PO refinement contract

Summary
- Refined the unit-test-categories task against the current repo test layout; the ticket remains a child of 06EXB807MN08HABHTHVPKKNFMG, still blocks 06EXB80QQHAYH61RY4X3T1E8S0, upstream hashing ticket 06EXB76NNRDP7WH1F2R5VYYPMR is already done, and no child tickets, relation changes, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The runnable fast local unit surface already exists at tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj; it references the core package plus the current provider packages Sqlite, Postgres, SqlServer, Oracle, and MySql, and links the Modeling/*.cs harness files into the unit assembly.
- Integration coverage is already separated into tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj, and README.md keeps PostgreSQL integration opt-in through DVAULT_TEST_POSTGRES_CONNECTION_STRING; this ticket should refine only the local unit category surface.
- The visible provider-capability baseline is finite: DataVaultProviderCapabilityProfiles.Sqlite is the only built-in capability profile, AddDVault() is the provider-neutral fallback registration path, and AddDVaultSqlite() is the only visible optimized provider-strategy registration path.
- No recent human comments or ticket attachments add extra scope; the existing relation set is sufficient context for refinement.

Scope In
- Classify the fast local metadata and model-building tests already centered on DataVaultMetadataTests, DataVaultModelBuilderExtensionsTests, DataVaultEfMetadataTranslationTests, and the technical metadata contract coverage.
- Classify the fast local naming and options tests already centered on the linked Modeling/DefaultNamingPolicyTests.cs and Modeling/NamingPolicyTests.cs harnesses plus ConventionFirstEntryPointCoverageTests.
- Classify the fast local hashing tests already centered on StableHashNormalizerTests and StableHashServiceTests, using the completed edge-case ticket 06EXB76NNRDP7WH1F2R5VYYPMR as established baseline context rather than new scope.
- Classify the fast local provider registration, provider capability, and provider strategy boundary tests already centered on ExplicitDataVaultSaveServiceTests and DataVaultProviderCapabilityProfileTests.

Scope Out
- SQLite scenario, schema, and raw-storage integration tests under tests/DCoding.Data.DVault.Tests/Integration, including ExplicitDataVaultSaveServiceSqliteTests, SqliteDataVaultSchemaTests, and Integration/SqliteProviderCapabilityProfileTests.
- Opt-in external database checks and configuration gating for PostgreSQL, SQL Server, Oracle, or MySql integration paths; those remain in the broader integration-category work tracked by 06EXB80QQHAYH61RY4X3T1E8S0.
- Production-code behavior changes to save services, provider packages, metadata translation, naming, or hashing implementations.
- New provider packages, CI infrastructure changes, benchmarks, or workflow-metadata updates.

Open questions
- none

Follow-up questions
- After this ticket and 06EXB80QQHAYH61RY4X3T1E8S0 both land, do we want a short contributor-facing note or helper command that shows how to select only unit categories versus only integration categories under the current dotnet and xUnit v3 runner?
- If additional provider capability profiles are introduced after sqlite-v1, should each new profile inherit the same unit-category vocabulary plus a matching integration smoke category by default?

Risks
- If the category mechanism does not match the capabilities of the current xUnit v3 Microsoft Testing Platform runner, the grouping may exist in code but remain difficult to select in local automation.
- SQLite has both unit registration tests and local integration tests; weak naming or grouping could blur the intended boundary between this unit-only ticket and the downstream integration-category ticket.
- Leaving the existing technical metadata contract harness outside the runnable unit surface would make part of the metadata category easy to miss despite other tickets and docs relying on that coverage.

Split recommendations
- No additional split is recommended; the integration-category boundary is already isolated in child task 06EXB80QQHAYH61RY4X3T1E8S0.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment