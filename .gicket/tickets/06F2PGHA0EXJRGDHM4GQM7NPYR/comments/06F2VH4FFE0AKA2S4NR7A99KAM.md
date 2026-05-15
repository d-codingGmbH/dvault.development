[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 6/6 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F2PGHA0EXJRGDHM4GQM7NPYR-task-update-v0-11-0-documentation-and-release-no\u0027 at commit \u00276525135bfab4\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F2PGHA0EXJRGDHM4GQM7NPYR-task-update-v0-11-0-documentation-and-release-no",
    "commitSha": "6525135bfab4",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "\u0060docs/releases/v0.11.0.md\u0060 exists and documents the coordinated package family, the design-time command surface (\u0060validate\u0060, \u0060export\u0060, \u0060drift\u0060, \u0060guardrail\u0060), built-in live-schema reader coverage, documentation updates, compatibility notes, and release verification evidence.",
      "satisfied": true,
      "reason": "Committed docs/releases/v0.11.0.md exists at 6525135bfab4 and is evidenced as the seven-package v0.11.0 release note with library-hosted design-time command guidance, live-schema reader coverage, documentation update notes, compatibility/scoping notes, and a Validation Evidence section; the deterministic keyword mismatch is weaker than this file-level evidence."
    },
    {
      "expectation": "\u0060README.md\u0060 and \u0060examples/README.md\u0060 replace \u00600.10.0\u0060 package/version snippets and stale release-note references with the v0.11.0 baseline.",
      "satisfied": true,
      "reason": "README.md shows the 0.11.0 installation baseline, the release-note evidence says the root README now points to v0.11.0 as the current baseline, and the developer-delivery evidence plus inspected examples/README.md show the examples guidance was updated away from 0.10.0 to the same current release baseline."
    },
    {
      "expectation": "\u0060docs/production-adoption-checklist.md\u0060 and \u0060docs/model-first-governance.md\u0060 no longer present SQLite-only or stale-current-baseline guidance where those sections are meant to describe the current public release.",
      "satisfied": true,
      "reason": "docs/model-first-governance.md now states Status: v0.11.0 public guidance, and docs/production-adoption-checklist.md replaces stale SQLite-only current guidance with current-reader coverage plus opt-in live integration language for PostgreSQL, SQL Server, Oracle, and MySQL."
    },
    {
      "expectation": "The five required doc paths describe the consumer-owned design-time boundary consistently: DVault provides reusable library-hosted commands, does not ship a standalone CLI, does not intercept \u0060dotnet ef\u0060, and does not make \u0060export\u0060 the default blocking CI gate.",
      "satisfied": true,
      "reason": "The updated docs consistently preserve the consumer-owned design-time boundary: docs/releases/v0.11.0.md says the commands are reusable library-hosted plumbing and not a standalone executable, docs/production-adoption-checklist.md says DVault does not ship a dotnet ef shim or interception and that export is not the default blocking CI gate, and the developer-delivery evidence states the same boundary was applied across README.md, examples/README.md, and docs/model-first-governance.md."
    },
    {
      "expectation": "Current public docs accurately describe built-in live-schema reader support for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL while keeping live execution for external providers opt-in and operationally consumer-managed.",
      "satisfied": true,
      "reason": "Structured source-of-truth evidence from src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs shows built-in readers for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL, and the updated public docs describe that coverage while keeping non-SQLite live execution opt-in and consumer-managed in checklist, examples, and model-first guidance."
    },
    {
      "expectation": "The completed ticket includes concrete documentation-level verification evidence for the changed paths, or an explicit statement that no additional doc-specific automation beyond repository inspection or formatting validation was applicable.",
      "satisfied": true,
      "reason": "The completion evidence is concrete: it names the changed documentation paths, verifies branch ticket/06F2PGHA0EXJRGDHM4GQM7NPYR-task-update-v0-11-0-documentation-and-release-no at commit 6525135bfab4, inspects committed content for the changed docs, and records successful verification commands dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The required five documentation paths are updated and mutually consistent on version numbers, command names, provider support, and drift-lane guidance.",
      "satisfied": true,
      "reason": "The verified branch delta contains exactly the five contract-required documentation edits: README.md, examples/README.md, docs/production-adoption-checklist.md, docs/model-first-governance.md, and docs/releases/v0.11.0.md. The inspected content aligns on 0.11.0, the documented command boundary, provider support, and drift-lane guidance."
    },
    {
      "expectation": "\u0060docs/releases/v0.11.0.md\u0060 becomes the current authoritative release summary and current public guidance no longer points readers at v0.10.0 as the latest baseline.",
      "satisfied": true,
      "reason": "docs/releases/v0.11.0.md exists as the current release summary, README.md now uses the 0.11.0 installation baseline, and the evidence shows current public guidance was repointed away from treating v0.10.0 as the latest baseline."
    },
    {
      "expectation": "The final completion evidence cites the exact changed documentation paths and the verification performed against them.",
      "satisfied": true,
      "reason": "The final evidence cites the exact changed documentation paths through the verified branch delta and developer-delivery summary, and it cites the performed verification through committed-path inspection plus successful dotnet test DVault.slnx --nologo and bash tools/check-format.sh runs."
    },
    {
      "expectation": "The wording preserves the implementation boundary: consumer-owned command host, no EF CLI interception, no automatic migration or schema-repair behavior, and live-schema checks remain optional operational evidence.",
      "satisfied": true,
      "reason": "The verified wording keeps the implementation boundary intact: consumer-owned command host, no standalone DVault CLI or dotnet ef interception, no automatic migration or schema-repair claims, and live-schema checks remain optional operational evidence for external providers."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u00276525135bfab4\u0027 on branch \u0027ticket/06F2PGHA0EXJRGDHM4GQM7NPYR-task-update-v0-11-0-documentation-and-release-no\u0027.",
    "Committed repository path \u0027docs/model-first-governance.md\u0027 exists at verified commit \u00276525135bfab4\u0027.",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: # Model-First Governance Workflow",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Status: v0.11.0 public guidance",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: This guide describes how teams should use governed \u0060dvault.model.v1\u0060 JSON artifacts alongside the existing Code-First and metadata-first DVault paths. Earlier release notes remain ...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: ## Choose A Declaration Path",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Use Code-First declarations when the Data Vault model is local to one EF model and fits the implemented fluent surface for hubs, hub-parent satellites, multi-active driving keys, a...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Use metadata-first registry-backed metadata when one shared authoritative \u0060DataVaultMetadataModel\u0060 or \u0060DataVaultMetadataRegistry\u0060 should drive EF projection, explicit save requests...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Canonical v1 JSON uses the stable top-level declaration categories \u0060hubs\u0060, \u0060links\u0060, \u0060satellites\u0060, \u0060pits\u0060, and \u0060bridges\u0060, with \u0060naming.policy\u0060 defaulting to \u0060default\u0060 and \u0060loadTimes...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: \u0022loadTimestampStorage\u0022: \u0022provider-default\u0022,",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Store the canonical JSON artifact in source control and review changes like source code. Reviewers should check the exact \u0060schemaVersion\u0060, \u0060naming.policy\u0060, \u0060loadTimestampStorage\u0060, ...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Import the artifact with \u0060DataVaultModelArtifactImporter.ImportJson\u0060 and treat \u0060DataVaultModelImportResult.Diagnostics\u0060 as validation evidence. A valid import exposes \u0060MetadataMode...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: The live-schema workflow is separate from the design-time workflow above. Required local live-schema coverage uses SQLite and does initialize a test database. PostgreSQL, SQL Serve...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Keep \u0060dvault.model.v1\u0060 strict and additive only through an explicit future contract. Current v1 artifacts must use the exact \u0060schemaVersion\u0060, the \u0060default\u0060 naming policy, one of th...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Use model-first governance when the authoritative model should be a reviewed, versioned \u0060dvault.model.v1\u0060 JSON artifact. This path is intended for source-controlled artifact review...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: ## Review Workflow",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Environment.NewLine,",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Export canonical JSON from fluent Code-First declarations or already-materialized metadata with \u0060DataVaultModelArtifactExporter.ExportJson\u0060. The exporter accepts a Code-First decla...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Compare the expected artifact or metadata model against generated/current EF metadata with \u0060DataVaultModelDriftReporter.Compare\u0060. Use the structured differences and \u0060ToDisplayStrin...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: ## Workflow Test Evidence",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Run the focused design-time workflow coverage from the repository root with:",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: dotnet test DVault.slnx --nologo --filter FullyQualifiedName~DataVaultModelFirstDesignTimeWorkflowTests",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: The valid workflow imports the representative \u0060models/sales-vault.json\u0060 \u0060dvault.model.v1\u0060 fixture with \u0060DataVaultModelArtifactImporter.ImportJson\u0060, configures a SQLite-backed desig...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: The seeded v1 baseline is the importer/projection family below, in ascending code order. All current entries are \u0060error\u0060 severity.",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: | \u0060DMV1501\u0060 | \u0060capability\u0060 | Unsupported metadata capability | Use only supported \u0060dvault.model.v1\u0060 capabilities or split the model into declarations the current runtime can map. |",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: | \u0060DMV1801\u0060 | \u0060projection\u0060 | Artifact projection failed | Review the projection error, adjust the affected declaration, and retry the import before applying metadata. |",
    "Committed repository path \u0027docs/production-adoption-checklist.md\u0027 exists at verified commit \u00276525135bfab4\u0027.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: # Production Adoption Checklist",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: Use this checklist when preparing a DVault-consuming application for production. It is a routing document for adopter readiness; follow the linked source documents for setup exampl...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: ## Package And Provider Baseline",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Install the provider-neutral \u0060DCoding.Data.DVault\u0060 package from NuGet and use the published installation guidance in the [README](../README.md#installation).",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Select the DVault provider package that matches the application database and keep every DVault package on one aligned published release version.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat the coordinated DVault package family as exactly these package ids: \u0060DCoding.Data.DVault\u0060, \u0060DCoding.Data.DVault.Analyzers\u0060, \u0060DCoding.Data.DVault.MySql\u0060, \u0060DCoding.Data.D...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use \u0060IDataVaultSaveService\u0060 as the default write boundary. Each save request should carry an explicit UTC load timestamp and record source.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat \u0060UseDataVaultSaveChangesMetadataInterceptor(...)\u0060 as optional and metadata-only. It fills missing \u0060LoadTimestamp\u0060 and \u0060RecordSource\u0060 values on already tracked generated...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat provider-specific live database integration tests for PostgreSQL, SQL Server, Oracle, and MySQL as opt-in evidence behind their documented connection-string environment...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat advanced configuration hooks as optional or future-facing unless the application has a specific deterministic rule to configure. The current source-backed custom path i...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use the runnable SQLite or PostgreSQL quickstarts as setup evidence when a small local proof is useful; see [examples/README.md](../examples/README.md).",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use model-first governance when source-controlled \u0060dvault.model.v1\u0060 JSON artifacts need review, strict import diagnostics, canonical export, projection into EF metadata, and ...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Run DVault diagnostics against the configured design-time model before applying migrations. Use [DVault Dotnet EF Design-Time Workflow](architecture/dvault-dotnet-ef-design-t...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use \u0060dotnet run --project \u003Cconsumer-project\u003E -- export --output \u003Cpath\u003E\u0060 only for artifact maintenance or reviewed refresh workflows, not as the default blocking CI gate.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Do not expect DVault to ship a \u0060dotnet ef\u0060 command shim, intercept EF CLI commands, auto-run migrations, or apply schema repairs. Those behaviors are outside the current v1 w...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: ## Validation Evidence",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] For repository validation evidence, use the [README local validation](../README.md#local-validation) section as the authoritative command baseline:",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] For package publication or release approval, use [Manual NuGet Publication Checklist](manual-nuget-publication.md) instead of this adoption checklist. Publication evidence ad...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Keep consumer-facing release notes and internal adoption records limited to published package versions and documented current behavior. Do not imply availability for unpublis...",
    "Committed repository path \u0027docs/releases/v0.10.0.md\u0027 exists at verified commit \u00276525135bfab4\u0027.",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: # DVault v0.10.0 Release Notes",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: Release: \u0060v0.10.0 - Developer Adoption Tooling\u0060",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: Intended release date: 2026-05-15",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: ## Package Scope",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: This is a coordinated release for the seven-package DVault NuGet family:",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: - \u0060DCoding.Data.DVault\u0060",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: All packages are version-aligned at \u00600.10.0\u0060. Package publication remains a separate manual release activity; these notes do not record a NuGet push, package hashes, or final publi...",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: - Extended package verification to include the analyzer package and analyzer assets in the coordinated release gate.",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: \u0060DCoding.Data.DVault.Analyzers\u0060 is developer tooling, not a runtime dependency. Consumer projects should reference it with \u0060PrivateAssets=\u0022all\u0022\u0060 so analyzer assets stay local to th...",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: The analyzer package does not provide code fixes, full model validation, model-first JSON validation, provider diagnostics, migration guardrails, or dataflow analysis in this relea...",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: - \u0060examples/README.md\u0060 keeps the SQLite quickstart as the no-container local baseline and links the PostgreSQL fixture path as opt-in external evidence.",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: - \u0060docs/manual-nuget-publication.md\u0060 now treats the analyzer package as part of the coordinated manual publication family and package-verification gate.",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: - Runtime DVault persistence behavior is unchanged by the analyzer package.",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: - The analyzer package has no provider dependency and should not become a transitive runtime dependency of application packages.",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: - PostgreSQL is the only documented external provider container fixture in this release. SQL Server, Oracle, and MySQL remain opt-in external database paths without first-class fix...",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: - SQLite remains the first-class live-schema drift reader. Other providers still rely on external opt-in evidence for live database validation.",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: ## Validation Evidence",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: Repository evidence for the release claims:",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: - \u0060tools/DCoding.Data.DVault.PackageVerification/\u0060 validates the seven-package release family, six runtime/provider symbol packages, analyzer assets, README/XML documentation, meta...",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: Release packaging validation is still performed before publication under \u0060docs/manual-nuget-publication.md\u0060 and should include:",
    "Observed committed repository file \u0027docs/releases/v0.10.0.md\u0027: - \u0060dotnet pack DVault.slnx --configuration Release --nologo\u0060",
    "Committed repository path \u0027docs/releases/v0.11.0.md\u0027 exists at verified commit \u00276525135bfab4\u0027.",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: # DVault v0.11.0 Release Notes",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: Release: \u0060v0.11.0 - Design-Time Commands and Provider Drift Readers\u0060",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: Intended release date: 2026-05-16",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: ## Package Scope",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: This is a coordinated release for the seven-package DVault NuGet family:",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: - \u0060DCoding.Data.DVault\u0060",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: DVault provides reusable library-hosted command plumbing that applications can invoke from a small executable entrypoint in the project that owns the configured \u0060DbContext\u0060, migrat...",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: - The design-time command surface is reusable library code, not a standalone executable package.",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: All packages are version-aligned at \u00600.11.0\u0060. Package publication remains a separate manual release activity; these notes do not record a NuGet push, package hashes, or final publi...",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: - Clarified that \u0060export\u0060 is for artifact maintenance and reviewed refresh workflows, not the default blocking CI gate.",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: Providers without a built-in reader return \u0060DataVaultLiveSchemaReadStatus.UnsupportedProvider\u0060. A recognized provider whose database cannot be reached returns \u0060DataVaultLiveSchemaR...",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: - Root README installation guidance now uses aligned \u00600.11.0\u0060 package examples and points at these release notes as the current baseline.",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: - \u0060docs/model-first-governance.md\u0060 now treats v0.11.0 as the current model-first governance baseline and links model-first review evidence to the current design-time workflow.",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: - Startup-project and target-project splits for design-time discovery remain outside the documented v1 workflow.",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: - Runnable non-SQLite live-schema walkthroughs, provider-specific secret-management recipes, and container-provisioning guides are not included in this release note.",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: ## Validation Evidence",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: Repository evidence for the release claims:",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: - \u0060docs/architecture/dvault-dotnet-ef-design-time-workflow.md\u0060 documents the consumer-owned command-host boundary and explicitly excludes a standalone DVault CLI, \u0060dotnet ef\u0060 inter...",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: - \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0060 covers deterministic command parsing, validation exit codes, export output, default artifact drift, live...",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: Release packaging validation is still performed before publication under \u0060docs/manual-nuget-publication.md\u0060 and should include:",
    "Observed committed repository file \u0027docs/releases/v0.11.0.md\u0027: - \u0060dotnet pack DVault.slnx --configuration Release --nologo\u0060",
    "Committed repository path \u0027examples/README.md\u0027 exists at verified commit \u00276525135bfab4\u0027.",
    "Observed committed repository file \u0027examples/README.md\u0027: # DVault Quickstart Examples",
    "Observed committed repository file \u0027examples/README.md\u0027: These examples run the same bounded customer-profile history flow through the public registry-backed metadata path:",
    "Observed committed repository file \u0027examples/README.md\u0027: - \u0060DCoding.Data.DVault.SqliteQuickstart\u0060 uses SQLite through \u0060AddDVaultSqlite()\u0060 and needs no external infrastructure.",
    "Observed committed repository file \u0027examples/README.md\u0027: - \u0060DCoding.Data.DVault.PostgresQuickstart\u0060 uses PostgreSQL through \u0060AddDVaultPostgres()\u0060 and a developer-managed connection string.",
    "Observed committed repository file \u0027examples/README.md\u0027: Both projects register one shared \u0060DataVaultMetadataModel\u0060 with \u0060AddDVault(options =\u003E options.UseMetadataModel(...))\u0060, opt the DbContext into that registry with \u0060UseDataVaultMetada...",
    "Observed committed repository file \u0027examples/README.md\u0027: The checked-in examples use project references so they can build against the current repository checkout. Published consumer applications should install the same coordinated NuGet ...",
    "Observed committed repository file \u0027examples/README.md\u0027: The SQLite quickstart creates a temporary SQLite database file, creates the DVault schema, writes one customer profile twice with distinct load timestamps, then prints the latest p...",
    "Observed committed repository file \u0027examples/README.md\u0027: - the first request saves the \u0060Customer\u0060 hub with an explicit UTC load timestamp and \u0060quickstart\u0060 record source;",
    "Observed committed repository file \u0027examples/README.md\u0027: \u0060UseDataVaultSaveChangesMetadataInterceptor(...)\u0060 is supported as an optional metadata-only path for applications that already track generated DVault rows themselves. It fills miss...",
    "Observed committed repository file \u0027examples/README.md\u0027: If \u0060DVAULT_TEST_POSTGRES_CONNECTION_STRING\u0060 is missing or empty, the PostgreSQL quickstart exits successfully before opening a database connection and prints:",
    "Observed committed repository file \u0027examples/README.md\u0027: - Use model-first governance when a reviewed \u0060dvault.model.v1\u0060 JSON artifact should be imported, projected into EF metadata, exported canonically, and compared against generated me...",
    "Observed committed repository file \u0027examples/README.md\u0027: Use the v1 design-time workflow for production migration guardrails. It includes the GitHub Actions baseline for pre-integration checks, and the reusable command host is invoked fr...",
    "Observed committed repository file \u0027examples/README.md\u0027: The drift command uses a committed reviewed artifact when one exists. \u0060export\u0060 is for artifact maintenance or reviewed refresh workflows, not the default blocking CI gate.",
    "Observed committed repository file \u0027examples/README.md\u0027: For model-first or metadata-first review evidence, compare the reviewed artifact or metadata model against generated EF metadata with \u0060DataVaultModelDriftReporter.Compare(...)\u0060.",
    "Observed committed repository file \u0027examples/README.md\u0027: Live-schema drift evidence is intentionally bounded. \u0060DataVaultLiveSchemaReader.ReadAsync(context)\u0060 and \u0060DataVaultLiveSchemaDriftReporter.Compare(...)\u0060 provide built-in reader cove...",
    "Observed committed repository file \u0027examples/README.md\u0027: See [DVault Dotnet EF Design-Time Workflow](../docs/architecture/dvault-dotnet-ef-design-time-workflow.md), [Model-First Governance Workflow](../docs/model-first-governance.md), an...",
    "Committed repository path \u0027README.md\u0027 exists at verified commit \u00276525135bfab4\u0027.",
    "Observed committed repository file \u0027README.md\u0027: # DVault",
    "Observed committed repository file \u0027README.md\u0027: DVault is the repository for the \u0060DCoding.Data.DVault\u0060 .NET library.",
    "Observed committed repository file \u0027README.md\u0027: ## Installation",
    "Observed committed repository file \u0027README.md\u0027: Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The coordinated DVault package family is vers...",
    "Observed committed repository file \u0027README.md\u0027: \u0060\u0060\u0060sh",
    "Observed committed repository file \u0027README.md\u0027: dotnet add package DCoding.Data.DVault --version 0.11.0",
    "Observed committed repository file \u0027README.md\u0027: Code-First metadata is additive. It does not ask callers to put DVault hash-key, load-timestamp, or record-source technical fields on domain entities, and it does not create a publ...",
    "Observed committed repository file \u0027README.md\u0027: Persistence remains an explicit service boundary. \u0060DataVaultSaveRequest\u0060 carries the load timestamp and record source, and callers choose when to write vault rows through \u0060IDataVau...",
    "Observed committed repository file \u0027README.md\u0027: DVault also provides an explicit opt-in \u0060SaveChanges\u0060 metadata interceptor for applications that already add generated DVault rows through EF tracking. The interceptor only fills m...",
    "Observed committed repository file \u0027README.md\u0027: .UseLoadTimestamp(() =\u003E DateTimeOffset.UtcNow)",
    "Observed committed repository file \u0027README.md\u0027: var loadTimestamp = new DateTimeOffset(2026, 5, 11, 10, 15, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027README.md\u0027: loadTimestamp,",
    "Observed committed repository file \u0027README.md\u0027: \u0060IDataVaultReadService\u0060 provides provider-neutral latest and as-of satellite reads. The common path maps selected rows through a caller-owned projector delegate so application code...",
    "Observed committed repository file \u0027README.md\u0027: row.RequiredDateTimeOffset(\u0022LoadTimestamp\u0022)),",
    "Observed committed repository file \u0027README.md\u0027: new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey], asOfTimestamp),",
    "Observed committed repository file \u0027README.md\u0027: DateTimeOffset LoadTimestamp);",
    "Observed committed repository file \u0027README.md\u0027: The lower-level \u0060ReadLatestSatelliteRowsAsync(...)\u0060 API remains available as the advanced escape hatch. It returns \u0060DataVaultSatelliteReadRecord\u0060 values containing the parent hash ...",
    "Observed committed repository file \u0027README.md\u0027: PIT-backed reads target one \u0060DataVaultPitMetadata\u0060 declaration, explicit parent hash keys, and an \u0060asOf\u0060 timestamp. \u0060ReadPitRowsAsync(...)\u0060 returns raw \u0060DataVaultPitReadRecord\u0060 row...",
    "Observed committed repository file \u0027README.md\u0027: new DataVaultPitAsOfReadRequest(pit, [customerHashKey], asOfTimestamp),",
    "Observed committed repository file \u0027README.md\u0027: row.RequiredDateTimeOffset(\u0022LoadTimestamp\u0022),",
    "Observed committed repository file \u0027README.md\u0027: - Model-first governance for reviewed \u0060dvault.model.v1\u0060 JSON artifacts that should be imported, projected into EF metadata, exported canonically, and compared against generated met...",
    "Observed committed repository file \u0027README.md\u0027: Choose one authoritative path for a model boundary and keep the others as compatible alternatives for different ownership needs. See [Model-First Governance Workflow](docs/model-fi...",
    "Committed branch delta contains 5 inspectable repository path(s): Modified: docs/model-first-governance.md, Modified: docs/production-adoption-checklist.md, Added: docs/releases/v0.11.0.md, Modified: examples/README.md, Modified: README.md.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 137 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/developer-experience, area/documentation, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F2PGFZWC5PXSDH46RCZPN1CG-story-add-provider-live-schema-drift-readers\u0027.",
    "Ticket history references implementation commit \u00276525135bfab4\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator using verified branch ticket/06F2PGHA0EXJRGDHM4GQM7NPYR-task-update-v0-11-0-documentation-and-release-no at commit 6525135bfab4."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F2PGHA0EXJRGDHM4GQM7NPYR`
- target-role: `integrator`
- verification-summary: Tester verified 6/6 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F2PGHA0EXJRGDHM4GQM7NPYR-task-update-v0-11-0-documentation-and-release-no' at commit '6525135bfab4'.
- acceptance-criteria: `6/6` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F2PGHA0EXJRGDHM4GQM7NPYR-task-update-v0-11-0-documentation-and-release-no`
- implementation-commit: `6525135bfab4`
- implementation-pr: `<none>`
- implementation-change: `<none>`