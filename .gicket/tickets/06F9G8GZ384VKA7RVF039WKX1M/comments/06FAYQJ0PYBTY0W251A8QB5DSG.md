[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F9G8GZ384VKA7RVF039WKX1M\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F9G8GZ384VKA7RVF039WKX1M-story-add-dcoding-data-dvault-db2-provider-packa\u0027 and commit \u00278dc38832003f\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F9G8GZ384VKA7RVF039WKX1M-story-add-dcoding-data-dvault-db2-provider-packa\u0027 from source \u00278dc38832003f\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F9G8GZ384VKA7RVF039WKX1M-story-add-dcoding-data-dvault-db2-provider-packa\u0027.",
    "Evidence: git diff --name-only develop...8dc38832003f shows product changes in DVault.slnx, Directory.Build.props, src/DCoding.Data.DVault.Db2/*, core DB2 wiring files, and unit-test/api-snapshot files.",
    "Evidence: src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj targets net8.0;net10.0, sets PackageId to DCoding.Data.DVault.Db2, packs ../../README.md, enables symbols, and references IBM.EntityFrameworkCore 8.0.0.400 and 10.0.0.100 in target-specific ItemGroups.",
    "Evidence: DVault.slnx includes src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj, and Directory.Build.props adds DCoding.Data.DVault.Db2 to the DVaultPackablePackage condition.",
    "Evidence: src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs registers IBM.EntityFrameworkCore to DataVaultProviderCapabilityProfiles.Db2, then calls services.AddDVault(), then registers Db2DataVaultProviderBehavior.",
    "Evidence: src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs, DataVaultProviderCapabilities.cs, DataVaultModelArtifactImporter.cs, DataVaultModelArtifactExporter.cs, and DataVaultDiagnostics.cs each contain new DB2 wiring or a KnownProviderNames.Db2 constant.",
    "Evidence: rg -n \u00228\\.34\\.0|10\\.34\\.0\u0022 against the delivered product diff returned no matches, while README.md, tools/pack-release-packages.sh, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, and tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs still contain 8.33.0 / 10.33.0 package-line references.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactImporterTests.cs adds a db2-v1-loadts-utc-ticks registry assertion, but its BuiltInProfiles() helper still enumerates only SQLite, Oracle, Postgres, SqlServer, and MySql.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/developer-experience, area/ef-core, area/packaging, area/provider-support, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F9G8GZ384VKA7RVF039WKX1M-story-add-dcoding-data-dvault-db2-provider-packa\u0027.",
    "Evidence: Ticket history references implementation commit \u00278dc38832003f\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: The package exposes AddDVaultDb2() and registers IBM.EntityFrameworkCore to the DB2 capability profile before DB2-specific behavior or strategy services are added, without relying on the unknown-provider SQLite fallback to claim DB2 support. (src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs exposes AddDVaultDb2(), registers IBM.EntityFrameworkCore to DataVaultProviderCapabilityProfiles.Db2, then calls AddDVault() before adding Db2DataVaultProviderBehavior.).",
    "AC check passed: The DB2 package pins IBM.EntityFrameworkCore 8.0.0.400 under net8.0 and 10.0.0.100 under net10.0, with no mixed EF Core line references across target frameworks. (src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj pins IBM.EntityFrameworkCore 8.0.0.400 under net8.0 and 10.0.0.100 under net10.0 in separate target-framework ItemGroups.).",
    "AC check passed: The DB2 provider name and capability-profile wiring are reachable from the package-owned runtime surfaces that currently enumerate supported providers, so diagnostics and model-artifact or provider-profile selection can identify DB2 as explicit support rather than unknown fallback. (src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs, DataVaultProviderCapabilityProfileSelection.cs, DataVaultModelArtifactImporter.cs, DataVaultModelArtifactExporter.cs, and DataVaultDiagnostics.cs all add DB2 capability/profile/provider-name wiring so DB2 is treated as explicit support instead of unknown fallback.).",
    "DoD check passed: A consumer can reference DCoding.Data.DVault.Db2, call AddDVaultDb2(), and get explicit DB2 provider registration on both net8.0 and net10.0 without changing the default provider-neutral AddDVault() path. (The new package targets net8.0 and net10.0, exposes AddDVaultDb2(), and adds an explicit DB2 provider behavior without changing the provider-neutral AddDVault() entry point.).",
    "DoD check passed: The solution, project graph, and package metadata surfaces recognize the new DB2 provider package and preserve the existing multi-target package-family boundary. (DVault.slnx, Directory.Build.props, the new DB2 csproj, unit-test project references, and public API snapshots all recognize DCoding.Data.DVault.Db2 as part of the existing multi-target package family.).",
    "DoD check passed: DB2 is represented as an explicit provider in the package-owned registration and selection surfaces, while unsupported schema, live-schema, and read-strategy details remain delegated to the sibling tickets instead of being implied by fallback behavior. (DB2 is explicit in provider behavior/profile selection and model-artifact capability lists, while no DB2 live-schema reader or provider-specific read/save strategy was added in this story.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: The repository contains a new multi-target provider package at src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj, that package is included in DVault.slnx, and its package metadata follows the established provider-package pattern while aligning to the planned 8.34.0 and 10.34.0 package lines. (src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj exists and DVault.slnx includes it, but the delivered repository evidence does not make the planned 8.34.0 / 10.34.0 coordinated package lines explicit for this new package.).",
    "AC check failed: The story leaves a clear package artifact contract for downstream verification: the new DB2 package id, dependency matrix, and expected package lines are explicit enough that task 06F9G8HJJDJH4KF9VK6TZ8B1Z0 can verify them without reopening PO scope. (The new package id and IBM dependency matrix are explicit, but no delivered repository surface records the planned 8.34.0 / 10.34.0 DVault package-line expectation for downstream package verification.).",
    "DoD check failed: The new package\u0027s dependency and artifact expectations are clear enough that downstream package-verification and documentation tickets can complete without reopening DB2 package identity, version, or provider-name decisions. (The delivered work does not make the coordinated 8.34.0 / 10.34.0 package-line contract explicit enough for downstream package-verification and documentation work to proceed without re-deciding version-line expectations.).",
    "The delivered repository does not make the planned 8.34.0 / 10.34.0 DVault package lines explicit for the new DB2 package, so downstream package verification still lacks direct repository evidence for the coordinated package-line expectation.",
    "DB2 verification coverage is incomplete: tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactImporterTests.cs adds one DB2 assertion, but its built-in provider loop still omits DataVaultProviderCapabilityProfiles.Db2."
  ],
  "evidence": [
    "git diff --name-only develop...8dc38832003f shows product changes in DVault.slnx, Directory.Build.props, src/DCoding.Data.DVault.Db2/*, core DB2 wiring files, and unit-test/api-snapshot files.",
    "src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj targets net8.0;net10.0, sets PackageId to DCoding.Data.DVault.Db2, packs ../../README.md, enables symbols, and references IBM.EntityFrameworkCore 8.0.0.400 and 10.0.0.100 in target-specific ItemGroups.",
    "DVault.slnx includes src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj, and Directory.Build.props adds DCoding.Data.DVault.Db2 to the DVaultPackablePackage condition.",
    "src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs registers IBM.EntityFrameworkCore to DataVaultProviderCapabilityProfiles.Db2, then calls services.AddDVault(), then registers Db2DataVaultProviderBehavior.",
    "src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs, DataVaultProviderCapabilities.cs, DataVaultModelArtifactImporter.cs, DataVaultModelArtifactExporter.cs, and DataVaultDiagnostics.cs each contain new DB2 wiring or a KnownProviderNames.Db2 constant.",
    "rg -n \u00228\\.34\\.0|10\\.34\\.0\u0022 against the delivered product diff returned no matches, while README.md, tools/pack-release-packages.sh, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, and tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs still contain 8.33.0 / 10.33.0 package-line references.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactImporterTests.cs adds a db2-v1-loadts-utc-ticks registry assertion, but its BuiltInProfiles() helper still enumerates only SQLite, Oracle, Postgres, SqlServer, and MySql.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/developer-experience, area/ef-core, area/packaging, area/provider-support, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F9G8GZ384VKA7RVF039WKX1M-story-add-dcoding-data-dvault-db2-provider-packa\u0027.",
    "Ticket history references implementation commit \u00278dc38832003f\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add a repository-backed surface in the delivered work that explicitly captures the coordinated 8.34.0 / 10.34.0 DVault package-line expectation for DCoding.Data.DVault.Db2 so downstream package verification does not need to reopen version-line decisions.",
    "Update finite built-in provider verification coverage so DB2 is included wherever this story now treats DB2 as explicit support.",
    "After rework, execute dotnet test DVault.slnx --nologo and bash tools/check-format.sh in the supported verification environment before re-handoff to test."
  ],
  "branchName": "ticket/06F9G8GZ384VKA7RVF039WKX1M-story-add-dcoding-data-dvault-db2-provider-packa",
  "commitSha": "8dc38832003f"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F9G8GZ384VKA7RVF039WKX1M`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F9G8GZ384VKA7RVF039WKX1M-story-add-dcoding-data-dvault-db2-provider-packa`