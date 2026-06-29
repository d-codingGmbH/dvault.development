[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 4/4 acceptance criteria and 3/3 definition-of-done expectations on branch \u0027ticket/06FGX6CRPG02ZWGE62QWSG42EC-task-document-binary-migration-manifest-validati\u0027 at commit \u0027760979ede822\u0027.",
  "implementationReference": {
    "branchName": "ticket/06FGX6CRPG02ZWGE62QWSG42EC-task-document-binary-migration-manifest-validati",
    "commitSha": "760979ede822",
    "pullRequestReference": null,
    "changeReference": null,
    "branchOwnerProvenance": {
      "ticketId": "06FGX6CRPG02ZWGE62QWSG42EC",
      "ownerBranch": "ticket/06FGX6CRPG02ZWGE62QWSG42EC-task-document-binary-migration-manifest-validati",
      "sourceCommitSha": "760979ede822",
      "baseBranch": "develop",
      "producingRole": "test",
      "producingRunId": "7dfc82e34144468b80691ebe653dc5d9",
      "producingInstanceId": "hp-ai-2026-001.1"
    }
  },
  "acceptanceCriteria": [
    {
      "expectation": "The migration documentation shows a complete pre-change sequence for existing persisted \u0060HexString\u0060 storage: capture reviewed source support-bundle or equivalent metadata evidence, run the \u0060hash-key-storage-migration\u0060 dry-run export, and validate/review the resulting \u0060dvault.hash-key-storage-migration.v1\u0060 manifest before any EF migration or data conversion is attempted.",
      "satisfied": true,
      "reason": "\u0060docs/hash-key-storage-migration.md\u0060 at commit \u0060760979ede822\u0060 now requires reviewed source evidence, the \u0060hash-key-storage-migration\u0060 dry-run export, and manifest validation before schema or data conversion, and \u0060docs/releases/v0.49.0.md\u0060 repeats the same pre-change sequence for existing \u0060HexString\u0060 storage."
    },
    {
      "expectation": "The documented validation flow makes the machine-checkable boundary explicit: structural or compatibility drift blocks the flow, warnings are non-structural only, and the same docs make clear that DVault is validating a review artifact rather than executing the migration.",
      "satisfied": true,
      "reason": "The migration guide makes the machine-checkable boundary explicit: it names the producer and validator surfaces, states the manifest is a review artifact rather than a migration runner, lists blocking \u0060error\u0060 cases, and states that warning-only manifests are non-structural and non-blocking."
    },
    {
      "expectation": "README and current release notes explicitly route existing \u0060HexString\u0060 users to the validated dry-run manifest path and preserve the separate message that binary-first is the recommendation for new schemas only.",
      "satisfied": true,
      "reason": "\u0060README.md\u0060 and \u0060docs/releases/v0.49.0.md\u0060 both preserve binary-first guidance for new schemas and explicitly route existing persisted \u0060HexString\u0060 users through reviewed source capture, dry-run manifest export, and manifest validation."
    },
    {
      "expectation": "If README wording changes touch packaged guidance, package-verifier expectations remain aligned; if packaged README assertions are unaffected, no verifier expectation change is required.",
      "satisfied": true,
      "reason": "\u0060README.md\u0060 changed, but the verified diff touched migration guidance rather than the packaged install/analyzer-host fragments enforced by \u0060tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0060; that verifier file stayed unchanged, so no expectation update was required."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "A reader can determine from the updated docs which command or API surfaces produce the manifest, which surface validates it, what facts are checked, and what work remains caller-owned after validation.",
      "satisfied": true,
      "reason": "The updated migration guide identifies the manifest producer (\u0060hash-key-storage-migration\u0060), the validator surfaces (\u0060DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(...)\u0060 and \u0060DataVaultPreflight.Run(...)\u0060), the checked facts (\u0060schemaVersion\u0060, \u0060dryRun\u0060, \u0060source\u0060, \u0060target\u0060, \u0060comparison\u0060, \u0060entries\u0060, provider/hash facts, and coverage), and the caller-owned work that remains after validation."
    },
    {
      "expectation": "The migration guide, README, and current release notes tell one consistent story about binary-first for new schemas, reviewed dry-run validation for existing persisted \u0060HexString\u0060 storage, and DVault\u0027s explicit non-goals.",
      "satisfied": true,
      "reason": "The migration guide, \u0060README.md\u0060, and \u0060docs/releases/v0.49.0.md\u0060 now tell the same story: binary-first is for new schemas, existing persisted \u0060HexString\u0060 storage must use the reviewed dry-run validation path first, and DVault does not execute the migration or rewrite persisted keys automatically."
    },
    {
      "expectation": "Any touched README-backed package-verification assertions or related documentation checks are updated to match the final wording.",
      "satisfied": true,
      "reason": "The README-backed verifier assertions did not need code changes because the README edits did not alter the packaged installation guidance that \u0060PackageVerifier.cs\u0060 checks, and the provided deterministic verification evidence shows \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 both passed."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027760979ede822\u0027 on branch \u0027ticket/06FGX6CRPG02ZWGE62QWSG42EC-task-document-binary-migration-manifest-validati\u0027.",
    "Committed repository path \u0027docs/hash-key-storage-migration.md\u0027 exists at verified commit \u0027760979ede822\u0027.",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: # Hash-Key Storage Migration Guide",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: Use this guide when an application owner wants to move existing persisted DVault hash-key storage from the default",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: \u0060HexString\u0060 physical profile to the explicit opt-in \u0060Binary\u0060 physical profile. DVault keeps one logical hash-key",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: representation: public APIs, save requests, read requests, diagnostics, explain output, and support bundles continue to use",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: canonical lowercase hexadecimal strings without a prefix.",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: This is an adopter-owned migration plan. DVault does not automatically migrate, backfill, dual-write, repair, reconcile, or",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: The validator emits blocking \u0060error\u0060 findings for:",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: sorts findings by severity rank (\u0060error\u0060, then \u0060warning\u0060, then \u0060info\u0060), stable code, table name, column name, and JSON path",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: | One hub, link, satellite, PIT, or bridge hash-key/reference column from the reviewed boundary is absent or the comparison counts do not match the entries | Invalid; emit \u0060error\u0060 ...",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: | The same \u0060tableName\u0060 and \u0060propertyName\u0060 identity appears more than once | Invalid; emit \u0060error\u0060 for duplicate coverage. |",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: | Source entries mix \u0060HexString\u0060 and \u0060Binary\u0060, or target entries mix profiles inside the selected boundary | Invalid; emit \u0060error\u0060 for mixed profile facts. |",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: | Provider or capability profile is not in the built-in v1 baseline | Invalid; emit \u0060error\u0060 for unsupported provider or profile values. |",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: | Source and target use different \u0060algorithmId\u0060, \u0060digestByteLength\u0060, or digest encoding | Invalid; emit \u0060error\u0060 for hash-fact drift. |",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: | \u0060sha1-v1\u0060 source is compared with \u0060sha256-160-v1\u0060 target and both report 20 digest bytes | Invalid; emit \u0060error\u0060 for algorithm drift despite equal byte length. |",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: \u0060DataVaultPreflight.Run(...)\u0060 through \u0060DataVaultPreflightRequest.HashKeyStorageMigrationManifestJson\u0060. Treat any error",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: 5. Build a provider-specific consumer migration or data-move script that changes the generated hash-key and",
    "Observed committed repository file \u0027docs/hash-key-storage-migration.md\u0027: | Before migration | The \u0060dvault.hash-key-storage-migration.v1\u0060 manifest validates without error find",
    "Committed repository path \u0027docs/releases/v0.49.0.md\u0027 exists at verified commit \u0027760979ede822\u0027.",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: # DVault v0.49.0 Release Notes",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: Release: \u0060v0.49.0 - Modeling and Generator Parity Refinement\u0060",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: Release date: 2026-06-28",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: These notes record the coordinated v0.49.0 modeling, generator, diagnostics, documentation, validation, and package-line baseline for DVault. The public release label maps to the c...",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: ## Package Lines",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: | Package version line | Target framework | EF Core line |",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: - Generated same-hub link mappers remain thin \u0060IDataVaultLinkMapper\u003CTSource\u003E\u0060 implementations and still flow through the existing explicit \u0060IDataVaultSaveService\u0060 path with caller-...",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: These facts make repeated same-hub roles reviewable by support-bundle-driven tooling. They do not create raw model-first mapper generation, provider-specific SQL, implicit persiste...",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: - Treat structural or compatibility errors as blocking. Warning-only manifests are non-structural and still require review.",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: Effectivity remains the existing caller-owned link-parent satellite pattern. No effectivity-specific fluent API or table family is added in this release.",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: The release validation lane is:",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: bash tools/pack-release-packages.sh",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: Package validation checks the expected \u00608.50.0\u0060 and \u006010.50.0\u0060 artifact set, package metadata, dependency groups, analyzer asset placement, XML documentation, symbol packages, READM...",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: v0.49.0 does not publish packages, record final publish approval, record package hashes, add package-signing evidence, or add release automation.",
    "Observed committed repository file \u0027docs/releases/v0.49.0.md\u0027: v0.49.0 does not add dependent child key modeling, model-first same-hub typed mapper generation, effectivity-specific fluent APIs, implicit persistence, automatic PIT or bridge mai...",
    "Committed repository path \u0027README.md\u0027 exists at verified commit \u0027760979ede822\u0027.",
    "Observed committed repository file \u0027README.md\u0027: # DVault",
    "Observed committed repository file \u0027README.md\u0027: DVault is a focused .NET library family for Data Vault 2.x-oriented persistence on Entity Framework Core. It keeps the public surface close to EF Core: model metadata, generated DV...",
    "Observed committed repository file \u0027README.md\u0027: ## Contents",
    "Observed committed repository file \u0027README.md\u0027: - [Installation](#installation)",
    "Observed committed repository file \u0027README.md\u0027: - [Quickstart](#quickstart)",
    "Observed committed repository file \u0027README.md\u0027: - [Package Compatibility](#package-compatibility)",
    "Observed committed repository file \u0027README.md\u0027: Callers own load timestamps, record sources, ordering, transactions, deterministic satellite hash diffs, and the moment a DVault write happens.",
    "Observed committed repository file \u0027README.md\u0027: var loadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);",
    "Observed committed repository file \u0027README.md\u0027: loadTimestamp,",
    "Observed committed repository file \u0027README.md\u0027: LoadTimestamp = row.RequiredDateTimeOffset(\u0022LoadTimestamp\u0022),",
    "Observed committed repository file \u0027README.md\u0027: Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The blocks below list the full coordinated pa...",
    "Observed committed repository file \u0027README.md\u0027: Privacy provider caveats stay inside the finite repository-backed provider baseline: SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2. MySQL means the repository MySQL profil...",
    "Observed committed repository file \u0027README.md\u0027: The binary-first profile is the recommended physical storage profile for new projects. Existing databases and configurations are not migrated automatically; \u0060HexString\u0060-compatible ...",
    "Observed committed repository file \u0027README.md\u0027: For runnable examples, the optional privacy proof, and fuller workflows, see [Getting Started](docs/getting-started.md), [examples/README.md](examples/README.md), and the current [...",
    "Observed committed repository file \u0027README.md\u0027: The current coordinated package baseline is documented in [Package Compatibility](docs/package-compatibility.md) and [DVault v0.49.0 Release Notes](docs/releases/v0.49.0.md). DVaul...",
    "Observed committed repository file \u0027README.md\u0027: - \u0060v0.49.0\u0060 is a repository release tag and release-note label, not a NuGet package version.",
    "Observed committed repository file \u0027README.md\u0027: - \u0060DCoding.Data.DVault.Privacy\u0060 remains optional and opt-in; it provides registration and alias-driven encrypted payload conversion seams over ordinary EF Core mapped payload prope...",
    "Observed committed repository file \u0027README.md\u0027: | Release history | [CHANGELOG.md](CHANGELOG.md) and [docs/releases/](docs/releases/) |",
    "Observed committed repository file \u0027README.md\u0027: | Performance evidence and tuning boundaries | [Performance Profiles](docs/performance-profiles.md) and [benchmarks/](benchmarks/) |",
    "Observed committed repository file \u0027README.md\u0027: | Model-first workflow | [Model-First Governance](docs/model-first-governance.md) |",
    "Observed committed repository file \u0027README.md\u0027: - Package publication remains a manual release operation; this repository records package creation and verification, not NuGet publication.",
    "Observed committed repository file \u0027README.md\u0027: - The analyzer package is validated against the \u0060.NET 10 SDK\u0060 build-host baseline for both coordinated package lines; pure \u0060.NET 8 SDK\u0060 analyzer consumption is not a current compat...",
    "Committed repository path \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027 exists at verified commit \u0027760979ede822\u0027.",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: using System.IO.Compression;",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: using System.Xml.Linq;",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: namespace DCoding.Data.DVault.PackageVerification;",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: public sealed class PackageVerifier {",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: private const string CorePackageId = \u0022DCoding.Data.DVault\u0022;",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: private const string Db2PackageId = \u0022DCoding.Data.DVault.Db2\u0022;",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: public PackageVerificationResult Verify(PackageVerificationOptions options) {",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: var issues = new List\u003CPackageVerificationIssue\u003E();",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: issues.Add(new PackageVerificationIssue(",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: PackageVerificationOptions.DefaultPackageDirectory,",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: \u0022Package directory does not exist at \u0027\u0022 \u002B options.PackageDirectory \u002B \u0022\u0027. Run \u0027bash tools/pack-release-packages.sh\u0027 from the repository root first.\u0022));",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: return new PackageVerificationResult(issues);",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: \u0022Unexpected file artifact in package directory. Expected only the \u0022 \u002B expectedPackageArtifactCount \u002B \u0022 .nupkg files and \u0022 \u002B expectedSymbolsArtifactCount \u002B \u0022 .snupkg files produced ...",
    "Observed committed repository file \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027: List\u003CPackageVerificationIssue\u003E issues) {",
    "Committed branch delta contains 3 inspectable repository path(s): Modified: docs/hash-key-storage-migration.md, Modified: docs/releases/v0.49.0.md, Modified: README.md.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: C:\\Projects\\DVault\\examples\\DCoding.Data.DVault.SqliteQuickstart\\DCoding.Data.DVault.SqliteQuickstart.csproj : warning NU1903: Package \u0027SQLitePCLRaw.lib.e_sqlite3\u0027 2.1.11 has a known high severity vulnerability, https://github.com/advisories/GHSA-2m69-gcr7-jv3q [C:\\Projects\\DVault\\DVault.slnx]",
    "Observed stdout: All projects are up-to-date for restore.",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 735 C# files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/documentation, area/hashing, area/migrations, area/package, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FGX6CRPG02ZWGE62QWSG42EC-task-document-binary-migration-manifest-validati\u0027.",
    "Ticket history references implementation commit \u0027760979ede822\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off commit \u0060760979ede822\u0060 to the integrator gate.",
    "Use the existing green verification evidence from \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 for the integrator review."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06FGX6CRPG02ZWGE62QWSG42EC`
- target-role: `integrator`
- verification-summary: Tester verified 4/4 acceptance criteria and 3/3 definition-of-done expectations on branch 'ticket/06FGX6CRPG02ZWGE62QWSG42EC-task-document-binary-migration-manifest-validati' at commit '760979ede822'.
- acceptance-criteria: `4/4` satisfied
- definition-of-done: `3/3` satisfied
- implementation-branch: `ticket/06FGX6CRPG02ZWGE62QWSG42EC-task-document-binary-migration-manifest-validati`
- implementation-commit: `760979ede822`
- implementation-pr: `<none>`
- implementation-change: `<none>`