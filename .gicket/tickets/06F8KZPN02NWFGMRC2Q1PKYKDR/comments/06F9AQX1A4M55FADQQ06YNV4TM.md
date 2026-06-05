[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 6/6 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc\u0027 at commit \u0027a634d4bc20eb\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc",
    "commitSha": "a634d4bc20eb",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "With \u0060DVaultGenerateTypedReadModels=true\u0060, resolving anything other than exactly one authoritative \u0060dvault.support-bundle.v1\u0060 additional file results in \u0060DMV1960\u0060 and no generated helpers.",
      "satisfied": true,
      "reason": "Satisfied by the source-boundary logic in DataVaultTypedReadModelSourceGenerator and analyzer tests covering no support bundle, raw dvault.model.v1 input, raw model plus valid support bundle, incompatible dvault.support-bundle.v2 input, and ambiguous multiple bundles; each path reports DMV1960 and suppresses generated sources."
    },
    {
      "expectation": "When \u0060DVaultTypedReadModelMetadataSourceFingerprint\u0060 is configured and does not match the resolved bundle fingerprint, the generator reports \u0060DMV1961\u0060 and suppresses generation.",
      "satisfied": true,
      "reason": "Satisfied by the configured fingerprint gate in the generator and the ReportsStaleConfiguredFingerprintAndSkipsGeneration analyzer test, which asserts DMV1961 and no generated sources on fingerprint drift."
    },
    {
      "expectation": "When PIT explain metadata or request-bound \u0060diagnostics.readShape.pit\u0060 facts are missing, mismatched, or outside the bounded PIT helper contract, the generator reports \u0060DMV1963\u0060 for the affected PIT helper while leaving unrelated supported helpers eligible.",
      "satisfied": true,
      "reason": "Satisfied by explicit PIT readShape validation in the generator for missing or mismatched PIT facts and by analyzer coverage that reports DMV1963 for unsupported PIT helper evidence while preserving unrelated satellite generation."
    },
    {
      "expectation": "When bridge explain metadata or request-bound \u0060diagnostics.readShape.bridge\u0060 facts are missing, mismatched, or outside the bounded bridge helper contract, the generator reports \u0060DMV1964\u0060 or \u0060DMV1967\u0060 as appropriate for the affected bridge helper while leaving unrelated supported helpers eligible.",
      "satisfied": true,
      "reason": "Satisfied by explicit bridge readShape validation in the generator and analyzer coverage that reports DMV1964 for unsupported bridge evidence, DMV1967 for dynamic or unbounded bridge cases, and preserves unrelated supported helper generation."
    },
    {
      "expectation": "A projected model-first support bundle with matching fingerprint and required ReadShape facts continues to generate supported PIT and bridge helpers.",
      "satisfied": true,
      "reason": "Satisfied by passing analyzer tests that still generate supported PIT and bridge helpers from request-bound support bundles, including support bundles sourced from model-first metadata with matching fingerprint and required readShape facts."
    },
    {
      "expectation": "Raw or residual \u0060dvault.model.v1\u0060 artifacts presented outside the projected support-bundle contract report \u0060DMV1960\u0060 under the current source-boundary baseline and do not widen generator inputs.",
      "satisfied": true,
      "reason": "Satisfied by generator logic and analyzer tests that route raw or residual dvault.model.v1 artifacts to DMV1960 without widening inputs, plus README and contract text that keep DMV1968 reserved for future model-first-specific outcomes."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "Generator code paths and analyzer tests cover the \u0060DMV1960\u0060, \u0060DMV1961\u0060, \u0060DMV1963\u0060, \u0060DMV1964\u0060, and \u0060DMV1967\u0060 paths touched by this story, plus the accepted raw-model rejection behavior.",
      "satisfied": true,
      "reason": "Satisfied by committed generator and analyzer-test coverage for DMV1960, DMV1961, DMV1963, DMV1964, and DMV1967, including raw-model rejection behavior, and by successful execution of dotnet test DVault.slnx --nologo."
    },
    {
      "expectation": "README and any in-repo generator contract text that mention these scenarios match the shipped diagnostic mapping.",
      "satisfied": true,
      "reason": "Satisfied because src/DCoding.Data.DVault.Analyzers/README.md, docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md, and docs/plans/typed-read-model-generator-contract.md now consistently describe the shipped DMV1960/1961/1963/1964/1967 mapping and reserved DMV1968 behavior."
    },
    {
      "expectation": "Supported satellite, PIT, and bridge helpers continue generating for unaffected entities in mixed bundles.",
      "satisfied": true,
      "reason": "Satisfied by analyzer tests showing unsupported PIT or bridge helpers emit entity-specific diagnostics while unrelated supported helpers still generate, together with passing supported PIT and bridge generation tests."
    },
    {
      "expectation": "No direct raw-model parsing path or unreviewed metadata-source fallback is introduced.",
      "satisfied": true,
      "reason": "Satisfied because the generator consumes only one authoritative dvault.support-bundle.v1 input, rejects raw dvault.model.v1 files at the source boundary, and the updated contract/docs explicitly forbid direct raw-model parsing or fallback to unreviewed metadata sources."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u0027a634d4bc20eb\u0027 on branch \u0027ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc\u0027.",
    "Committed repository path \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027 exists at verified commit \u0027a634d4bc20eb\u0027.",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: # DVault V1 Typed PIT And Bridge Helper Contract",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: Status: v1 implemented generator contract",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: Ticket: 06F7Y0GT7A5QT77TADMRZBVYN8",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: Current public baseline: [DVault v0.26.0 Release Notes](../releases/v0.26.0.md)",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: Typed helper implementation baseline: [DVault v0.25.0 Release Notes](../releases/v0.25.0.md)",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: ## Decision",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: The support-bundle evidence must identify the PIT parent, produced PIT table, parent hash-key column, PIT \u0060LoadTimestamp\u0060 column, optional canonical driving-key columns, included P...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: - \u0060DateTimeOffset LoadTimestamp\u0060 for the required selected PIT row load timestamp.",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: - nullable \u0060DateTimeOffset?\u0060 snapshot-reference timestamp members per included PIT segment.",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: The implemented typed read-model generator baseline is support-bundle-driven and emits helpers for reviewed satellite, PIT, and bounded bridge read shapes. Satellite helpers contin...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: Generated helpers are ergonomic extension methods over the existing provider-neutral \u0060IDataVaultReadService\u0060 boundary. They construct stable metadata/read request values and projec...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: PIT and bridge helper emission uses support-bundle explain facts because \u0060readShape\u0060 is request-bound. The support bundle must prove the translated table name, produced or mapped c...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: Unsupported PIT or bridge facts produce an entity-specific diagnostic and skip only the affected helper. Other supported satellite, PIT, or bridge helpers in the same support bundl...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: Typed PIT helpers may be emitted only for runtime PIT shapes already proven by the repository PIT boundary:",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: Unsupported PIT residual shapes include missing read-shape evidence, raw support-bundle gaps, link-parent multi-active PITs, incompatible multi-active driving-key families, tuple-f...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: Typed bridge helpers may be emitted only for runtime bridge shapes already proven by the repository bridge boundary:",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: The support-bundle evidence must identify the bridge kind, produced bridge table, endpoint roles, endpoint hash-key columns in generated order, selected filter endpoints, determini...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: Unsupported bridge residual shapes include missing read-shape evidence, endpoint vocabularies outside \u0060From\u0060, \u0060To\u0060, \u0060Ancestor\u0060, and \u0060Descendant\u0060, hierarchy traversal without bounde...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: - \u0060DMV1963\u0060 for PIT metadata that lacks the bounded helper evidence or declares an unsupported PIT shape.",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: - \u0060DMV1964\u0060 for bridge metadata that lacks the bounded helper evidence or declares an unsupported bridge shape.",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: - \u0060DMV1967\u0060 for shapes that require dynamic runtime query construction, provider SQL, runtime projection selection, unbounded traversal, tuple expansion, or payload joins outside t...",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: - \u0060DMV1969\u0060 for valid runtime metadata shapes intentionally skipped because they remain outside the generated helper boundary.",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: - new public runtime read primitives.",
    "Observed committed repository file \u0027docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md\u0027: ## Evidence",
    "Committed repository path \u0027docs/plans/typed-read-model-generator-contract.md\u0027 exists at verified commit \u0027a634d4bc20eb\u0027.",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: # Typed Read Model Generator Contract",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: Status: superseded historical planning context",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: Ticket: 06F5Q922T5B21GJN49FYN6DJH0",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: Historical boundary: v0.22.0 satellite-only typed read helpers",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: Current generator contract: ../architecture/dvault-v1-typed-pit-bridge-helper-contract.md",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: ## Supersession",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: | Load timestamp technical column | non-null \u0060DateTimeOffset LoadTimestamp\u0060 normalized to UTC by the existing read pipeline |",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: The row type must expose exact binding constants for the produced satellite table name and every produced column name used by the generated projection. The generated projector read...",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: Generated extension methods are static extension methods over \u0060IDataVaultReadService\u0060. They keep the caller-owned \u0060DbContext\u0060, hash-key request values, optional as-of timestamp, an...",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: Diagnostics must identify the metadata source kind, metadata source fingerprint when available, logical metadata name, produced entity name, produced property name when relevant, a...",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: - [DVault v0.22.0 Release Notes](../releases/v0.22.0.md).",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: - [Model-First Governance Workflow](../model-first-governance.md).",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: The earlier version of this planning document described generated PIT and bridge helper emission. Epic 06F5Q91V0YGSA6SH9WDS02GH0M explicitly supersedes that design for the shipped ...",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: The current implemented branch baseline has since changed through additive support-bundle-driven work. For current analyzer behavior, use [DVault V1 Typed PIT And Bridge Helper Con...",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: The generator does not parse raw \u0060dvault.model.v1\u0060 additional files, source-visible Code-First callbacks, or literal metadata-first \u0060DataVaultMetadataModel\u0060 declarations directly. ...",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: - provider-specific SQL, query hints, migration operations, or provider-specific performance claims.",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: - automatic support-bundle routing, publication, storage, attachment, or approval workflow.",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: In the historical v0.22 boundary, PIT and bridge shapes had to use existing runtime read surfaces or surface as \u0060DMV196x\u0060 diagnostics when they appeared in generator input. The cur...",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: ## Descriptor And Naming Requirements",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: The support-bundle explain descriptor is an implementation-internal generator model, not a new public artifact format. It must preserve the metadata-source kind and fingerprint, pr...",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: Unsupported satellite inputs include missing produced table or column binding metadata, duplicate or colliding generated property names after deterministic identifier normalization...",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: \u0060Current\u0060 and \u0060Latest\u0060 are equivalent convenience names over \u0060DataVaultLatestSatelliteReadRequest\u0060 with no \u0060asOf\u0060 value. \u0060AsOf\u0060 passes an inclusive UTC cutoff through that same run...",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: | \u0060DMV1963\u0060 | PIT metadata lacks matching request-bound \u0060diagnostics.readShape.pit\u0060 evidence or declares a PIT shape outside the bounded helper contract. |",
    "Observed committed repository file \u0027docs/plans/typed-read-model-generator-contract.md\u0027: | \u0060DMV1964\u0060 | Bridge metadata lacks matching request-bound \u0060diagnostics.readShape.bridge\u0060 evidence or declares a bridge shape outside the bounded helper contract. |",
    "Committed repository path \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027 exists at verified commit \u0027a634d4bc20eb\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: using Microsoft.CodeAnalysis;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: namespace DCoding.Data.DVault.Analyzers;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: internal static class DataVaultTypedReadModelDiagnosticCatalog {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: public const string Category = \u0022SourceGeneration\u0022;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: public static readonly DiagnosticDescriptor MetadataSourceUnavailable = new(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: \u0022DMV1960\u0022,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: DiagnosticSeverity.Error,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: description: \u0022Typed DVault read-model generation requires exactly one deterministic authoritative metadata source.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: public static readonly DiagnosticDescriptor MetadataSourceFingerprintDrift = new(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: description: \u0022Typed DVault read-model generation stops when the configured metadata source fingerprint differs from the resolved source fingerprint.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: public static readonly DiagnosticDescriptor UnsupportedSatelliteShape = new(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: description: \u0022Typed DVault satellite read-model generation supports only deterministic hub-parent or link-parent satellite shapes with string driving keys and payload values.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: public static readonly DiagnosticDescriptor UnsupportedPitShape = new(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: description: \u0022Typed DVault PIT read-model generation supports only the bounded v1 PIT baseline.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: public static readonly DiagnosticDescriptor UnsupportedBridgeShape = new(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: description: \u0022Typed DVault bridge read-model generation supports only the bounded v1 bridge baseline.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: public static readonly DiagnosticDescriptor NameCollision = new(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: description: \u0022Typed DVault read-model generation stops when deterministic generated type, method, or property names collide.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: public static readonly DiagnosticDescriptor PayloadNullabilityFallback = new(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: description: \u0022Typed DVault satellite read-model generation emits nullable payload properties when authoritative CLR or EF nullability cannot be proven.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: public static readonly DiagnosticDescriptor DynamicQueryShapeRequired = new(",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: description: \u0022Typed DVault read-model generation does not emit helpers for shapes that require dynamic runtime query construction, provider SQL, runtime projection selection, or un...",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs\u0027: public static readonly DiagnosticDescriptor UnsupportedModelFirstShape = new(",
    "Committed repository path \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027 exists at verified commit \u0027a634d4bc20eb\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using System.Collections.Immutable;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using System.Text;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using System.Text.Json;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using Microsoft.CodeAnalysis;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using Microsoft.CodeAnalysis.CSharp;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: using Microsoft.CodeAnalysis.Diagnostics;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: !HasSupportBundleTechnicalProperty(entity, \u0022LoadTimestamp\u0022) ||",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: \u0022authoritative explain metadata is missing the PIT parent reference, parent hash key, load timestamp, or satellite snapshot reference binding.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: \u0022PIT driving-key tuple projection requires dynamic runtime query behavior outside the residual generator helper contract.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs\u0027: \u0022the runtime PIT metadata shape is valid for IDataVaultReadService usage but no typed PIT helper is emitted by this diagnostic-only generator path.\u0022);",
    "Committed repository path \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027 exists at verified commit \u0027a634d4bc20eb\u0027.",
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
    "Observed committed repository file \u0027src/DCoding.Data.DVault.Analyzers/README.md\u0027: | \u0060DMV1968\u0060 | Reserved for future model-first-specific typed helper outcomes; current raw or residual model-first additional files are source-boundary failures reported as \u0060DMV1960...",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027 exists at verified commit \u0027a634d4bc20eb\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using System.Collections.Immutable;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using System.Reflection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using DCoding.Data.DVault.Analyzers;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using Microsoft.CodeAnalysis;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using Microsoft.CodeAnalysis.CSharp;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: using Microsoft.CodeAnalysis.Diagnostics;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: RuntimeStubs,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Empty(result.CompilationErrors);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: \u0022SatCustomerProfileRuntime\u0022,",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: var source = AssertGeneratedSource(result, \u0022DVault.GeneratedReadModels.SatCustomerProfileRuntime.g.cs\u0022);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Contains(\u0022public sealed record SatCustomerProfileRuntimeReadModel(\u0022, source, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Contains(\u0022public const string ProducedTableName = \\\u0022SatCustomerProfileRuntime\\\u0022;\u0022, source, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Empty(manyToManyResult.CompilationErrors);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: Assert.Empty(hierarchyResult.CompilationErrors);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs\u0027: public async Task GeneratedBridgeHelpersDelegateThroughRuntimeReadBoundaryWithEquivalentRequestsAndProjection() {",
    "Committed branch delta contains 6 inspectable repository path(s): Modified: docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md, Modified: docs/plans/typed-read-model-generator-contract.md, Modified: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs, Modified: src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs, Modified: src/DCoding.Data.DVault.Analyzers/README.md, Modified: tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 222 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/analyzers, area/developer-experience, area/diagnostics, area/ef-core, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc\u0027.",
    "Ticket history references implementation commit \u0027a634d4bc20eb\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to integrator using branch ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc at commit a634d4bc20eb."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F8KZPN02NWFGMRC2Q1PKYKDR`
- target-role: `integrator`
- verification-summary: Tester verified 6/6 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc' at commit 'a634d4bc20eb'.
- acceptance-criteria: `6/6` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F8KZPN02NWFGMRC2Q1PKYKDR-story-add-generator-diagnostics-for-stale-or-inc`
- implementation-commit: `a634d4bc20eb`
- implementation-pr: `<none>`
- implementation-change: `<none>`