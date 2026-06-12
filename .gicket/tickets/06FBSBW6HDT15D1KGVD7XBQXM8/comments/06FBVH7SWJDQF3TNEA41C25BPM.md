[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06FBSBW6HDT15D1KGVD7XBQXM8-story-audit-analyzer-package-compatibility-for-n\u0027 at commit \u0027243be40a1a2e\u0027.",
  "implementationReference": {
    "branchName": "ticket/06FBSBW6HDT15D1KGVD7XBQXM8-story-audit-analyzer-package-compatibility-for-n",
    "commitSha": "243be40a1a2e",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "The ticket records an explicit compatibility decision for DCoding.Data.DVault.Analyzers on the 8.36.0 line, including whether the package remains a net10.0 analyzer asset or must be retargeted.",
      "satisfied": true,
      "reason": "docs/plans/analyzer-package-compatibility-audit.md records the explicit decision to keep one net10.0 analyzer asset and require a .NET 10 SDK host for the 8.36.0 and 10.36.0 package lines."
    },
    {
      "expectation": "The decision cites concrete local proof from the analyzer csproj, pack script, package verification code/tests, integration-project analyzer reference, and repository validation surfaces.",
      "satisfied": true,
      "reason": "The audit note cites the analyzer csproj, analyzers/dotnet/cs packaging, tools/pack-release-packages.sh, PackageVerifier.cs, the net8.0;net10.0 integration project with SetTargetFramework=net10.0, and repository validation surfaces; PackageVerifierTests.cs plus a successful dotnet test run provide additional checked-in verification."
    },
    {
      "expectation": "If the analyzer stays net10.0-only, README.md, src/DCoding.Data.DVault.Analyzers/README.md, and package-verification expectations explicitly state the supported build-host SDK baseline for net8.0 consumers.",
      "satisfied": true,
      "reason": "README.md and src/DCoding.Data.DVault.Analyzers/README.md now state the .NET 10 SDK host baseline for net8.0 consumers on 8.36.0, and PackageVerifier.cs with PackageVerifierTests.cs enforces that wording."
    },
    {
      "expectation": "If the product requirement is instead net8.0-project plus .NET 8 SDK compatibility, follow-up implementation retargets the analyzer assets and adds verification that proves that exact baseline.",
      "satisfied": true,
      "reason": "The accepted outcome is not a pure .NET 8 SDK compatibility claim; the audit note explicitly treats analyzer retargeting and exact-baseline verification as future work only if that different product requirement is chosen."
    },
    {
      "expectation": "The final install guidance and verification lane do not promise a broader compatibility story than the repository actually validates.",
      "satisfied": true,
      "reason": "The install guidance now disclaims pure .NET 8 SDK analyzer consumption, the package-verification lane enforces the host-SDK wording, and dotnet test plus bash tools/check-format.sh both succeeded on commit 243be40a1a2e."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The audit decision and proof are preserved in the ticket handoff and the planning note at docs/plans/analyzer-package-compatibility-audit.md.",
      "satisfied": true,
      "reason": "The ticket description preserves the delivery contract and docs/plans/analyzer-package-compatibility-audit.md preserves the decision and proof, with traceable handoff branch and commit evidence in ticket history."
    },
    {
      "expectation": "The accepted compatibility claim is reflected in analyzer installation guidance and in the package-verification or smoke-test lane that enforces that claim.",
      "satisfied": true,
      "reason": "The accepted compatibility claim is reflected in the root and analyzer installation guidance, and PackageVerifier.cs now fails packaged READMEs that omit the required host-SDK statement; PackageVerifierTests.cs covers both runtime and analyzer README enforcement."
    },
    {
      "expectation": "Existing follow-up tasks stay aligned with the chosen outcome: 06FBSBWBT33K7Y1Z6NM71GAQ68 for implementation or SDK gating, and 06FBSBWH9F415E12VRHRYQ2JJM for documentation and verification alignment.",
      "satisfied": true,
      "reason": "The authoritative delivery contract and persisted workflow comments keep 06FBSBWBT33K7Y1Z6NM71GAQ68 aligned to retargeting or SDK-gating work and 06FBSBWH9F415E12VRHRYQ2JJM aligned to documentation and verification alignment, with no conflicting tester evidence."
    },
    {
      "expectation": "A reviewer can trace the chosen baseline to checked-in repository evidence without reopening method-level implementation questions.",
      "satisfied": true,
      "reason": "A reviewer can trace the chosen baseline through the audit note, README updates, package-verifier changes, unit tests, and verified branch and commit 243be40a1a2e without reopening method-level implementation questions."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027243be40a1a2e\u0027 on branch \u0027ticket/06FBSBW6HDT15D1KGVD7XBQXM8-story-audit-analyzer-package-compatibility-for-n\u0027.",
    "Committed repository path \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027 exists at verified commit \u0027243be40a1a2e\u0027.",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: # Analyzer Package Compatibility Audit",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: Ticket: \u006006FBSBW6HDT15D1KGVD7XBQXM8\u0060",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: ## Decision",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: For the current v0.36.x compatibility contract, keep \u0060DCoding.Data.DVault.Analyzers\u0060 on one \u0060net10.0\u0060 analyzer asset and treat the \u0060.NET 10 SDK\u0060 as the supported build-host baselin...",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: The current repository evidence does not prove support for consuming the analyzer package from a pure \u0060.NET 8 SDK\u0060 baseline. If that baseline becomes a product requirement, the ana...",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: ## Proof",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: - The same project packs its payload under \u0060analyzers/dotnet/cs/\u0060, not under \u0060lib/net8.0\u0060 or \u0060lib/net10.0\u0060, so the package does not expose consumer-target-specific runtime assets.",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: - \u0060tools/pack-release-packages.sh\u0060 packs the analyzer project once for \u00608.36.0\u0060 and once for \u006010.36.0\u0060 without changing the analyzer target framework, so both package lines current...",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: - \u0060tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0060 verifies analyzer asset presence, XML docs, symbols, and README guidance, but it does not require a separate \u0060n...",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: - \u0060README.md\u0060, \u0060docs/local-validation.md\u0060, \u0060docs/manual-nuget-publication.md\u0060, and \u0060.github/workflows/ci.yml\u0060 all set \u0060.NET 10 SDK\u0060 as the current validation and publication baseli...",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: - \u0060docs/plans/shared-implementation-standards.md\u0060 explicitly allows analyzer, tooling, benchmark, and repository helper projects to stay on \u0060net10.0\u0060 when they are not consumer run...",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: - If the product requirement is instead \u0022net8 target project plus .NET 8 SDK\u0022 compatibility, retarget the analyzer assets and add a verification lane that proves that exact baselin...",
    "Observed committed repository file \u0027docs/plans/analyzer-package-compatibility-audit.md\u0027: - Keep package verification and install guidance aligned with whichever compatibility claim is accepted so the \u00608.36.0\u0060 analyzer package is not documented more broadly than it is v...",
    "Committed repository path \u0027docs/plans/shared-implementation-standards.md\u0027 exists at verified commit \u0027243be40a1a2e\u0027.",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: # Shared Implementation Standards",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: Status: v1 shared standards",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: Ticket: 06EXB6NWYVB37D7S74VB3PVTCC",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: Milestone: Foundation and architecture",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: ## Purpose",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: This document is the shared implementation standards artifact for DVault foundation work. Downstream tickets should reference this document when they need repository formatting, la...",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: - hash key, hash diff, load timestamp, and record source technical columns",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: The v1 conceptual baseline includes hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources. Early examples may stay conceptual and SQLite-oriented, but...",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: These standards consolidate existing repository decisions. They do not replace the referenced source documents, and they do not introduce product-code behavior, provider-specific p...",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: Those tickets should reference this artifact instead of copying standards into their own descriptions or implementation notes. Future governance work may attach this document to th...",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: Manual review is not an accepted substitute for the gate. The first CI workflow or application build definition added to the repository must run \u0060bash tools/check-format.sh\u0060 as a b...",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: - packable runtime/provider projects that ship to consumers target \u0060net8.0;net10.0\u0060",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: - analyzer, tooling, benchmark, or repository helper projects may stay on \u0060net10.0\u0060 when they are not consumer runtime packages",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: Planning release \u0060v0.36.0\u0060 defines the current dual consumer package-line contract and carries forward the DB2 provider package baseline and stable hash algorithm-selection guidanc...",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: The planning release number is not the consumer-facing NuGet package version. \u0060v0.36.0\u0060 produces exactly two aligned package-version lines:",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: Do not publish or document a consumer-facing \u00600.36.0\u0060 DVault package version for this planning release. Do not combine \u00608.36.0\u0060 and \u006010.36.0\u0060 packages in one published artifact fam...",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: Each resolved target must use exactly one compatible EF/provider dependency line. Runtime, provider, integration-test, benchmark, example, and verifier project files may use condit...",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: The required provider package evidence for the compatibility lines is:",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: \u0060DCoding.Data.DVault.Analyzers\u0060 remains coordinated family tooling, not a runtime dependency. Consuming projects should keep analyzer/source-generator references local with \u0060Privat...",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: Downstream package verification, matrix tests, release notes, README guidance, and CI documentation are incomplete if they blur planning release \u0060v0.36.0\u0060 with package versions \u00608....",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: This historical section records the previous v0.33 compatibility contract for release-note links and audit context. Planning release \u0060v0.33.0\u0060 defined a dual consumer package-line ...",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: The planning release number is not the consumer-facing NuGet package version. \u0060v0.33.0\u0060 produces exactly two aligned package-version lines:",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: Do not publish or document a consumer-facing \u00600.33.0\u0060 DVault package version for this planning release. Do not combine \u00608.33.0\u0060 and \u006010.33.0\u0060 packages in one published artifact fam...",
    "Observed committed repository file \u0027docs/plans/shared-implementation-standards.md\u0027: Each resolved target must use exactly one compatible EF/provider dependency line. Runtime, provider, integration-test, and verifier project files may use conditional \u0060PackageRefere...",
    "Committed repository path \u0027README.md\u0027 exists at verified commit \u0027243be40a1a2e\u0027.",
    "Observed committed repository file \u0027README.md\u0027: # DVault",
    "Observed committed repository file \u0027README.md\u0027: DVault is a focused .NET library family for Data Vault 2.x-oriented persistence on Entity Framework Core. It keeps the public surface close to EF Core: model metadata, generated DV...",
    "Observed committed repository file \u0027README.md\u0027: ## Contents",
    "Observed committed repository file \u0027README.md\u0027: - [Installation](#installation)",
    "Observed committed repository file \u0027README.md\u0027: - [Quickstart](#quickstart)",
    "Observed committed repository file \u0027README.md\u0027: - [Current v0.36.0 Hash-Key Storage Guidance Baseline](#current-v0360-hash-key-storage-guidance-baseline)",
    "Observed committed repository file \u0027README.md\u0027: Callers own load timestamps, record sources, ordering, transactions, and the moment a DVault write happens.",
    "Observed committed repository file \u0027README.md\u0027: For provider-specific filters, environment variables, benchmark commands, and package-verification details, see [Local Validation](docs/local-validation.md).",
    "Observed committed repository file \u0027README.md\u0027: For runnable examples and fuller workflows, see [Getting Started](docs/getting-started.md), [examples/README.md](examples/README.md), and the current [DVault v0.36.0 Release Notes]...",
    "Observed committed repository file \u0027README.md\u0027: The v0.36.0 release record is the current coordinated eight-package documentation baseline for the dual consumer package-version lines. See [DVault v0.36.0 Release Notes](docs/rele...",
    "Observed committed repository file \u0027README.md\u0027: Hash-key values stay logical lowercase hexadecimal strings at public request, save, read, diagnostics, and support-bundle boundaries. \u0060HexString\u0060 remains the default compatible phy...",
    "Observed committed repository file \u0027README.md\u0027: Changing stable hash algorithm id, digest length, or hash-key storage profile after values are persisted is caller-owned compatibility work. DVault does not add automatic rehashing...",
    "Observed committed repository file \u0027README.md\u0027: | Release history | [CHANGELOG.md](CHANGELOG.md) and [docs/releases/](docs/releases/) |",
    "Observed committed repository file \u0027README.md\u0027: | Performance evidence and tuning boundaries | [Performance Profiles](docs/performance-profiles.md) and [benchmarks/](benchmarks/) |",
    "Observed committed repository file \u0027README.md\u0027: | Model-first workflow | [Model-First Governance](docs/model-first-governance.md) |",
    "Observed committed repository file \u0027README.md\u0027: - Package publication remains a manual release operation; this repository records package creation and verification, not NuGet publication.",
    "Observed committed repository file \u0027README.md\u0027: - \u0060src/DCoding.Data.DVault/\u0060: provider-neutral runtime package.",
    "Observed committed repository file \u0027README.md\u0027: - \u0060docs/\u0060: release notes, architecture, planning, quality, validation, and adoption documentation.",
    "Observed committed repository file \u0027README.md\u0027: bash tools/pack-release-packages.sh",
    "Observed committed repository file \u0027README.md\u0027: \u0060bash tools/pack-release-packages.sh\u0060 creates the two coordinated package lines under \u0060artifacts/packages/\u0060: eight \u00608.36.0\u0060 packages with \u0060net8.0\u0060 assets and EF Core 8 dependency g...",
    "Committed repository path \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027 exists at verified commit \u0027243be40a1a2e\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: # DCoding.Data.DVault.Analyzers",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: Roslyn analyzers and source generators for DVault compile-time metadata declarations. The v1 package reports:",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: - \u0060DMV1901\u0060 for unsupported \u0060BusinessKey(...)\u0060, \u0060Payload(...)\u0060, or \u0060DrivingKey(...)\u0060 selector shapes.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: - \u0060DMV1902\u0060 for duplicate logical member declarations inside the same applicable Code-First builder lambda scope.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: - \u0060DMV1910\u0060 for exposing DVault generated shared-type tables as \u0060DbSet\u003CDictionary\u003Cstring, object\u003E\u003E\u0060 members on a \u0060DbContext\u0060.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: - \u0060DMV1911\u0060 for direct EF write calls against DVault generated shared-type \u0060DbSet\u003CDictionary\u003Cstring, object\u003E\u003E\u0060 sets.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The package also provides bounded code fixes for DMV1901 anonymous-object direct-member expansion and DMV1902 later-duplicate removal. Its mapping source generator emits registry-b...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: Generated code implements the existing \u0060IDataVaultHubMapper\u003CTSource\u003E\u0060, \u0060IDataVaultLinkMapper\u003CTSource\u003E\u0060, or \u0060IDataVaultSatelliteMapper\u003CTSource\u003E\u0060 contracts and constructs \u0060DataVaultR...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: For each supported PIT, the generator emits \u0060{PitProducedName}ReadModel\u0060 and \u0060{PitProducedName}ReadExtensions\u0060 with \u0060Read...AsOfAsync\u0060 over \u0060IDataVaultReadService\u0060. The helper cons...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The lifecycle diagnostics \u0060DMV1912\u0060 through \u0060DMV1914\u0060 are high-confidence EF Core misuse diagnostics for direct source-visible evidence only. They align with the root README sectio...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: Install the analyzer package in projects that declare DVault Code-First metadata, compile-time generated row mappings, or support-bundle-driven typed satellite, PIT, and bridge rea...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: \u0060PrivateAssets=\u0022all\u0022\u0060 keeps the analyzer local to the project that owns the declarations. The package supplies analyzer assets and does not require a runtime reference from applica...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The analyzer does not attempt whole-application DI inference and does not treat \u0060UseDataVaultSaveChangesMetadataInterceptor(...)\u0060 as a replacement for the explicit save boundary. T...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: \u0060DMV1912\u0060 reports when direct \u0060ApplyDataVaultMetadata(...)\u0060 model projection visibly varies by caller-owned context state or branches and the directly visible \u0060IModelCacheKeyFactor...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: \u0060DMV1913\u0060 reports direct \u0060UseModel(...)\u0060 compiled-model selection when the same source visibly applies that runtime model to a DVault context whose realized model shape can vary. F...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The lifecycle diagnostics are intentionally limited to direct syntax and semantic facts in the analyzed source: visible instance members read in \u0060OnModelCreating(...)\u0060, direct bran...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The source generator recognizes mapping declarations from \u0060DCoding.Data.DVault\u0060 runtime attributes on one source type:",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The typed read-model source generator emits satellite latest/current/as-of helpers, bounded PIT as-of helpers, and bounded bridge traversal helpers from one authoritative \u0060dvault.s...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: For each supported bridge, the generator emits \u0060{BridgeProducedName}ReadModel\u0060 and \u0060{BridgeProducedName}ReadExtensions\u0060 over \u0060IDataVaultReadService\u0060. Many-to-many bridges emit \u0060Rea...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: The v1 generator supports hub-parent, link-parent, and deterministic multi-active satellites whose driving keys and payload values are strings after projection into the support-bun...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: | \u0060DMV1963\u0060 | PIT metadata or request-bound PIT read-shape evidence is incomplete or outside the bounded generated-helper baseline. |",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: | \u0060DMV1964\u0060 | Bridge metadata or request-bound bridge read-shape evidence is incomplete or outside the bounded generated-helper baseline. |",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: | \u0060DMV1966\u0060 | Payload nullability cannot be proven from the support-bundle descriptor, so the generated payload property falls back to nullable. |",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: | \u0060DMV1967\u0060 | The shape would require dynamic runtime query construction, provider SQL, runtime projection selection, or unbounded traversal. |",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027 exists at verified commit \u0027243be40a1a2e\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: using System.IO.Compression;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: using System.Text;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: using System.Xml.Linq;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: using DCoding.Data.DVault.PackageVerification;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: namespace DCoding.Data.DVault.Tests.Unit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: public void RuntimeReadmeMustContainBothPackageLineInstallGuides() {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: CreateRuntimePackageReadme([new PackageLine(Net8PackageLineVersion, Net8TargetFramework, \u0022EF Core 8\u0022)]);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: public void RuntimeReadmeMustStateAnalyzerBuildHostSdkBaseline() {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: CreateRuntimePackageReadme().Replace(ExpectedAnalyzerBuildHostGuidance, string.Empty, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: public void ReadmeMustNotUseStaleOrPlanningReleaseInstallVersions() {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: CreateRuntimePackageReadme() \u002B",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0027: issue.Message.Contains(\u0022must not document stale or planning-release install version fragment\u0022, StringComparison.Ordinal) \u0026\u0026",
    "Committed repository path \u0027tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs\u0027 exists at verified commit \u0027243be40a1a2e\u0027.",
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
    "Committed branch delta contains 6 inspectable repository path(s): Added: docs/plans/analyzer-package-compatibility-audit.md, Modified: docs/plans/shared-implementation-standards.md, Modified: README.md, Modified: src/DCoding.Data.DVault.Analyzers/README.md, Modified: tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs, Modified: tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault.Analyzers -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault.Analyzers\\bin\\Debug\\net10.0\\DCoding.Data.DVault.Analyzers.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 657 C# files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/api-compatibility, area/developer-experience, area/diagnostics, area/packaging, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FBSBW6HDT15D1KGVD7XBQXM8-story-audit-analyzer-package-compatibility-for-n\u0027.",
    "Ticket history references implementation commit \u0027243be40a1a2e\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator using branch ticket/06FBSBW6HDT15D1KGVD7XBQXM8-story-audit-analyzer-package-compatibility-for-n at commit 243be40a1a2e."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06FBSBW6HDT15D1KGVD7XBQXM8`
- target-role: `integrator`
- verification-summary: Tester verified 5/5 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06FBSBW6HDT15D1KGVD7XBQXM8-story-audit-analyzer-package-compatibility-for-n' at commit '243be40a1a2e'.
- acceptance-criteria: `5/5` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06FBSBW6HDT15D1KGVD7XBQXM8-story-audit-analyzer-package-compatibility-for-n`
- implementation-commit: `243be40a1a2e`
- implementation-pr: `<none>`
- implementation-change: `<none>`