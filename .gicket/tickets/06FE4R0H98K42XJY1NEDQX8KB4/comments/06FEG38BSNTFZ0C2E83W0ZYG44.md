[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06FE4R0H98K42XJY1NEDQX8KB4-task-write-binary-hash-storage-migration-and-val\u0027 at commit \u0027af89bbeda469\u0027.",
  "implementationReference": {
    "branchName": "ticket/06FE4R0H98K42XJY1NEDQX8KB4-task-write-binary-hash-storage-migration-and-val",
    "commitSha": "af89bbeda469",
    "pullRequestReference": null,
    "changeReference": null,
    "branchOwnerProvenance": {
      "ticketId": "06FE4R0H98K42XJY1NEDQX8KB4",
      "ownerBranch": "ticket/06FE4R0H98K42XJY1NEDQX8KB4-task-write-binary-hash-storage-migration-and-val",
      "sourceCommitSha": "af89bbeda469",
      "baseBranch": "develop",
      "producingRole": "test",
      "producingRunId": "b51b86f0a1ac42e991b13bdd1f76d580",
      "producingInstanceId": "hp-ai-2026-001.1"
    }
  },
  "acceptanceCriteria": [
    {
      "expectation": "A checked-in guide explains that Binary is an explicit opt-in physical storage profile, HexString remains the compatible default, and public, request, and diagnostic hash-key values stay lowercase hexadecimal strings.",
      "satisfied": true,
      "reason": "docs/hash-key-storage-migration.md states that Binary is explicit opt-in physical storage, HexString remains the compatible default, and public, request, read, diagnostic, and support-bundle hash-key values stay canonical lowercase hexadecimal strings; docs/getting-started.md reinforces the same boundary."
    },
    {
      "expectation": "The guide defines a preflight checklist that uses the support-bundle or equivalent translated metadata baseline to compare storage profile, stable-hash algorithm id, digest byte length, provider store type, provider value format, and conversion behavior before any migration step.",
      "satisfied": true,
      "reason": "The guide\u0027s Preflight Checklist requires a support-bundle or equivalent translated metadata baseline and compares storage profile, algorithmId, digestByteLength, provider store type, provider value format, EF CLR type, and conversion behavior before any migration step."
    },
    {
      "expectation": "The guide defines a caller-owned execution and rollback sequence for moving existing persisted data from hex to binary storage, including fail-closed handling when persisted compatibility facts drift or when algorithm changes are mixed into the same change.",
      "satisfied": true,
      "reason": "The guide\u0027s Execution Sequence, Rollback Expectations, and Validation Checkpoints define a caller-owned cutover and rollback flow, require fail-closed handling when compatibility facts drift, and explicitly separate algorithm changes from storage-profile migration."
    },
    {
      "expectation": "The guide states provider caveats using only checked-in evidence: the built-in provider profile baseline is finite, SQLite footprint evidence is the current quantified storage example, and broader provider-specific savings or performance claims are not promised.",
      "satisfied": true,
      "reason": "The Provider Caveats section limits claims to the finite built-in provider profile baseline, treats the SQLite bundle as the current quantified footprint evidence, and explicitly avoids promising provider-specific savings or performance claims beyond that checked-in evidence."
    },
    {
      "expectation": "The guide is discoverable from the current adoption documentation path through cross-links from existing checked-in entry points.",
      "satisfied": true,
      "reason": "The guide is cross-linked from existing adoption entry points in README.md, docs/getting-started.md, docs/production-adoption-checklist.md, and hash-key-footprint.md, making it discoverable from the current adoption path."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The migration and validation guide is checked into the docs set with terminology consistent with the hash-key storage contract and current getting-started or adoption docs.",
      "satisfied": true,
      "reason": "The committed docs/hash-key-storage-migration.md guide uses the same HexString, Binary, lowercase-hex, support-bundle, and fail-closed terminology as the storage-profile contract and the existing adoption documentation."
    },
    {
      "expectation": "The guide includes concrete validation and rollback checkpoints a consumer can follow before, during, and after cutover without implying automatic DVault migration support.",
      "satisfied": true,
      "reason": "Validation Checkpoints and Rollback Expectations provide concrete before, during, and after cutover checks and explicitly state that DVault does not automatically migrate, backfill, repair, reconcile, dual-write, or rehash persisted keys."
    },
    {
      "expectation": "Relevant existing documentation entry points are updated or linked so the guide is discoverable from the current adoption path.",
      "satisfied": true,
      "reason": "Relevant existing documentation entry points were updated so the guide is reachable from the README quickstart/docs map, Getting Started, Production Adoption Checklist, and the hash-key footprint summary."
    },
    {
      "expectation": "Any examples, tables, or caveats in the guide stay aligned with the visible v1 algorithms, storage profiles, and SQLite evidence bundle.",
      "satisfied": true,
      "reason": "The guide\u0027s tables and caveats stay within the visible v1 algorithm set, the HexString and Binary storage profiles, and the SQLite-local footprint evidence bundle, including the same-width sha1-v1 versus sha256-160-v1 incompatibility example."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027af89bbeda469\u0027 on branch \u0027ticket/06FE4R0H98K42XJY1NEDQX8KB4-task-write-binary-hash-storage-migration-and-val\u0027.",
    "Committed repository path \u0027docs/getting-started.md\u0027 exists at verified commit \u0027af89bbeda469\u0027.",
    "Observed committed repository file \u0027docs/getting-started.md\u0027: # Getting Started",
    "Observed committed repository file \u0027docs/getting-started.md\u0027: This guide expands the root README quickstart without turning DVault into an application platform. DVault remains an EF Core library family: applications own provider configuration...",
    "Observed committed repository file \u0027docs/getting-started.md\u0027: ## Choose The Metadata Boundary",
    "Observed committed repository file \u0027docs/getting-started.md\u0027: DVault supports three compatible declaration paths:",
    "Observed committed repository file \u0027docs/getting-started.md\u0027: - Code-First declarations in \u0060OnModelCreating\u0060 for application-local EF models.",
    "Observed committed repository file \u0027docs/getting-started.md\u0027: - Metadata-first declarations through \u0060DataVaultMetadataModel\u0060 or \u0060DataVaultMetadataRegistry\u0060 when one public metadata object should drive schema projection, explicit saves, reads,...",
    "Observed committed repository file \u0027docs/getting-started.md\u0027: \u0060IDataVaultSaveService\u0060 is the public write entry point. The caller supplies load timestamp, record source, ordering, cancellation, and transaction context. DVault does not interce...",
    "Observed committed repository file \u0027docs/getting-started.md\u0027: For shared metadata, build or import a \u0060DataVaultMetadataModel\u0060 and register it with EF options through the documented metadata APIs. For reviewed JSON artifacts, use the model-fir...",
    "Observed committed repository file \u0027docs/getting-started.md\u0027: Hash-key values stay logical lowercase hexadecimal strings in public APIs. \u0060HexString\u0060 is the default compatible physical storage profile. \u0060Binary\u0060 is an opt-in physical storage pr...",
    "Committed repository path \u0027docs/hash-key-storage-migration.md\u0027 exists at verified commit \u0027af89bbeda469\u0027.",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: # Hash-Key Storage Migration Guide",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: Use this guide when an application owner wants to move existing persisted DVault hash-key storage from the default",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: \u0060HexString\u0060 physical profile to the explicit opt-in \u0060Binary\u0060 physical profile. DVault keeps one logical hash-key",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: representation: public APIs, save requests, read requests, diagnostics, explain output, and support bundles continue to use",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: canonical lowercase hexadecimal strings without a prefix.",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: This is an adopter-owned migration plan. DVault does not automatically migrate, backfill, dual-write, repair, reconcile, or",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: 3. Build a provider-specific consumer migration or data-move script that changes the generated hash-key and",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: Provider live-schema evidence is not identical across providers. The support bundle and translated metadata facts are the",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: evidence only when the selected provider exposes them under the consumer application\u0027s operational controls. DB2 live-schema",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: evidence.",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: The checked-in quantified footprint evidence is SQLite-local. The root [hash-key-footprint.md](../hash-key-footprint.md)",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: profiles. Keep storage and lookup/read claims scoped to that bundle unless a future provider-specific evidence bundle is",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: other providers from the SQLite evidence alone.",
    "Committed repository path \u0027docs/production-adoption-checklist.md\u0027 exists at verified commit \u0027af89bbeda469\u0027.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: # Production Adoption Checklist",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: Use this checklist when preparing a DVault-consuming application for production. It is a routing document for adopter readiness; follow the linked source documents for setup exampl...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: ## Package And Provider Baseline",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Install the provider-neutral \u0060DCoding.Data.DVault\u0060 package from NuGet and use the published installation guidance in the [README](../README.md#installation).",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Select the DVault provider package that matches the application database and keep every DVault package on one aligned published release version.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat [v0.42.0 release notes](releases/v0.42.0.md) and [Package Compatibility](package-compatibility.md) as the current public documentation baseline for the coordinated pack...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Review provider schema guardrail facts before applying provider-specific DDL. Keep logical DVault names provider-neutral and traceable through \u0060DataVaultAnnotationNames.Produ...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use \u0060IDataVaultSaveService\u0060 as the default write boundary. Each save request should carry an explicit UTC load timestamp and record source.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Keep \u0060DataVaultBulkSaveRequest\u0060 for already-materialized ordered batches. Use \u0060DataVaultChunkedSaveRequest\u0060 with bounded \u0060DataVaultSaveChunk\u0060 values when the loader needs mat...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat [v0.32.0 release notes](releases/v0.32.0.md) as the historical source for benchmark-driven provider threshold evidence, the review-only provider-specific SQL artifact m...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Choose one consumer package-version line per project: \u00608.42.0\u0060 for \u0060net8.0\u0060 and EF Core 8, or \u006010.42.0\u0060 for \u0060net10.0\u0060 and EF Core 10. Do not use a consumer-facing \u00600.42.0\u0060 pa...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Install \u0060DCoding.Data.DVault.Analyzers\u0060 only in projects that own DVault Code-First declarations, compile-time generated row mapping declarations, or support-bundle-driven ty...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use the runnable SQLite or PostgreSQL quickstarts as setup evidence when a small local proof is useful; see [examples/README.md](../examples/README.md).",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use model-first governance when source-controlled \u0060dvault.model.v1\u0060 JSON artifacts need review, strict import diagnostics, canonical export, projection into EF metadata, drif...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Run DVault diagnostics against the configured design-time model before applying migrations. Use [DVault Dotnet EF Design-Time Workflow](architecture/dvault-dotnet-ef-design-t...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Use \u0060dotnet run --project \u003Cconsumer-project\u003E -- export --output \u003Cpath\u003E\u0060 only for artifact maintenance or reviewed refresh workflows, not as the default blocking CI gate.",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Add \u0060dotnet run --project \u003Cconsumer-project\u003E -- support-bundle --output \u003Cpath\u003E\u0060 as a consumer-invoked troubleshooting artifact when configuration, provider-behavior evidence,...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Review migration guardrail output as operation-level \u0060Safe\u0060, \u0060Risky\u0060, or \u0060Incompatible\u0060 evidence from \u0060DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)\u0060; treat incom...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat unrecognized EF providers as unsupported for provider-specific DDL safety claims. Do not copy SQLite, Oracle, PostgreSQL, SQL Server, DB2, or MySQL guardrail guarantees...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Run idempotency preflight only from consumer-owned live-schema evidence, such as a \u0060DataVaultLiveSchemaReadResult\u0060 supplied to \u0060DataVaultPreflightRequest.IdempotencyLiveSchem...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Do not expect DVault to ship a \u0060dotnet ef\u0060 command shim, intercept EF CLI commands, auto-run migrations, or apply schema repairs. Those behaviors are outside the current v1 w...",
    "Observed committed repository file \u0027docs/production-adoption-checklist.md\u0027: - [ ] Treat provider-specific optimized saves as diagnostics-gated implementations behind the same explicit service. Review \u0060SaveStrategy\u0060 status, provider name, selected strategy ...",
    "Committed repository path \u0027hash-key-footprint.md\u0027 exists at verified commit \u0027af89bbeda469\u0027.",
    "Observed committed repository file \u0027hash-key-footprint.md\u0027: # DVault Hash-Key Footprint Summary",
    "Observed committed repository file \u0027hash-key-footprint.md\u0027: This summary routes v0.36.0 adopter guidance to the checked-in SQLite-local hash-key storage evidence bundle. The detailed artifact sidecars remain authoritative:",
    "Observed committed repository file \u0027hash-key-footprint.md\u0027: - [benchmark-summary.md](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.md)",
    "Observed committed repository file \u0027hash-key-footprint.md\u0027: - [benchmark-summary.csv](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.csv)",
    "Observed committed repository file \u0027hash-key-footprint.md\u0027: - [benchmark-summary.json](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.json)",
    "Observed committed repository file \u0027hash-key-footprint.md\u0027: - [hash-key-footprint.md](artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/hash-key-footprint.md)",
    "Observed committed repository file \u0027hash-key-footprint.md\u0027: ## Evidence Boundary",
    "Observed committed repository file \u0027hash-key-footprint.md\u0027: - Performance and storage claims must stay scoped to this checked-in bundle unless a future provider-specific bundle is added.",
    "Observed committed repository file \u0027hash-key-footprint.md\u0027: For existing persisted databases, plan and validate hex-to-binary adoption with the [Hash-Key Storage Migration Guide](docs/hash-key-storage-migration.md). Keep storage and perform...",
    "Committed repository path \u0027README.md\u0027 exists at verified commit \u0027af89bbeda469\u0027.",
    "Observed committed repository file \u0027README.md\u0027: # DVault",
    "Observed committed repository file \u0027README.md\u0027: DVault is a focused .NET library family for Data Vault 2.x-oriented persistence on Entity Framework Core. It keeps the public surface close to EF Core: model metadata, generated DV...",
    "Observed committed repository file \u0027README.md\u0027: ## Contents",
    "Observed committed repository file \u0027README.md\u0027: - [Installation](#installation)",
    "Observed committed repository file \u0027README.md\u0027: - [Quickstart](#quickstart)",
    "Observed committed repository file \u0027README.md\u0027: - [Package Compatibility](#package-compatibility)",
    "Observed committed repository file \u0027README.md\u0027: Callers own load timestamps, record sources, ordering, transactions, and the moment a DVault write happens.",
    "Observed committed repository file \u0027README.md\u0027: For provider-specific filters, environment variables, benchmark commands, and package-verification details, see [Local Validation](docs/local-validation.md).",
    "Observed committed repository file \u0027README.md\u0027: Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. Use exactly one package line for a consumer p...",
    "Observed committed repository file \u0027README.md\u0027: For runnable examples and fuller workflows, see [Getting Started](docs/getting-started.md), [examples/README.md](examples/README.md), and the current [DVault v0.42.0 Release Notes]...",
    "Observed committed repository file \u0027README.md\u0027: The current coordinated package baseline is documented in [Package Compatibility](docs/package-compatibility.md) and [DVault v0.42.0 Release Notes](docs/releases/v0.42.0.md). DVaul...",
    "Observed committed repository file \u0027README.md\u0027: - \u0060v0.42.0\u0060 is a repository release tag and release-note label, not a NuGet package version.",
    "Observed committed repository file \u0027README.md\u0027: | Release history | [CHANGELOG.md](CHANGELOG.md) and [docs/releases/](docs/releases/) |",
    "Observed committed repository file \u0027README.md\u0027: | Performance evidence and tuning boundaries | [Performance Profiles](docs/performance-profiles.md) and [benchmarks/](benchmarks/) |",
    "Observed committed repository file \u0027README.md\u0027: | Model-first workflow | [Model-First Governance](docs/model-first-governance.md) |",
    "Observed committed repository file \u0027README.md\u0027: - Package publication remains a manual release operation; this repository records package creation and verification, not NuGet publication.",
    "Observed committed repository file \u0027README.md\u0027: - The analyzer package is validated against the \u0060.NET 10 SDK\u0060 build-host baseline for both coordinated package lines; pure \u0060.NET 8 SDK\u0060 analyzer consumption is not a current compat...",
    "Observed committed repository file \u0027README.md\u0027: - DB2 live-schema reading is available as external opt-in evidence through \u0060IBM.EntityFrameworkCore\u0060; DB2 databases, credentials, lifecycle cleanup, and CI isolation remain consume...",
    "Observed committed repository file \u0027README.md\u0027: - \u0060src/DCoding.Data.DVault/\u0060: provider-neutral runtime package.",
    "Observed committed repository file \u0027README.md\u0027: - \u0060docs/\u0060: release notes, architecture, planning, quality, validation, and adoption documentation.",
    "Observed committed repository file \u0027README.md\u0027: bash tools/pack-release-packages.sh",
    "Observed committed repository file \u0027README.md\u0027: \u0060bash tools/pack-release-packages.sh\u0060 creates the two coordinated package lines under \u0060artifacts/packages/\u0060: eight \u00608.42.0\u0060 packages with \u0060net8.0\u0060 assets and EF Core 8 dependency g...",
    "Committed branch delta contains 5 inspectable repository path(s): Modified: docs/getting-started.md, Added: docs/hash-key-storage-migration.md, Modified: docs/production-adoption-checklist.md, Modified: hash-key-footprint.md, Modified: README.md.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: C:\\Projects\\DVault\\examples\\DCoding.Data.DVault.SqliteQuickstart\\DCoding.Data.DVault.SqliteQuickstart.csproj : warning NU1903: Package \u0027SQLitePCLRaw.lib.e_sqlite3\u0027 2.1.11 has a known high severity vulnerability, https://github.com/advisories/GHSA-2m69-gcr7-jv3q [C:\\Projects\\DVault\\DVault.slnx]",
    "Observed stdout: All projects are up-to-date for restore.",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 663 C# files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/documentation, area/hash-storage, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06FE4R0H98K42XJY1NEDQX8KB4-task-write-binary-hash-storage-migration-and-val\u0027.",
    "Ticket history references implementation commit \u0027af89bbeda469\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator for the final acceptance decision."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06FE4R0H98K42XJY1NEDQX8KB4`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06FE4R0H98K42XJY1NEDQX8KB4-task-write-binary-hash-storage-migration-and-val' at commit 'af89bbeda469'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06FE4R0H98K42XJY1NEDQX8KB4-task-write-binary-hash-storage-migration-and-val`
- implementation-commit: `af89bbeda469`
- implementation-pr: `<none>`
- implementation-change: `<none>`