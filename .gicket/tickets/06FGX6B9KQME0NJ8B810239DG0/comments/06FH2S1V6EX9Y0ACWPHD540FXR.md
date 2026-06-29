[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FGX6B9KQME0NJ8B810239DG0\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FGX6B9KQME0NJ8B810239DG0-task-wire-migration-manifest-validation-into-pre\u0027 and commit \u0027af2404fd699a\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FGX6B9KQME0NJ8B810239DG0-task-wire-migration-manifest-validation-into-pre\u0027 from source \u0027af2404fd699a\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FGX6B9KQME0NJ8B810239DG0-task-wire-migration-manifest-validation-into-pre\u0027.",
    "Evidence: \u0060git rev-parse --verify af2404fd699a^{commit}\u0060 resolved to \u0060af2404fd699a10f1d1d8ba6fda5f5186566ee022\u0060.",
    "Evidence: \u0060git diff --name-only develop...af2404fd699a\u0060 shows product changes in \u0060src/DCoding.Data.DVault/DataVaultPreflight.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultPreflightRequest.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPreflightTests.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060, \u0060docs/architecture/dvault-dotnet-ef-design-time-workflow.md\u0060, and \u0060docs/production-adoption-checklist.md\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultPreflight.cs\u0060 creates a \u0060hash-key-storage-migration-manifest\u0060 section and calls \u0060DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(request.HashKeyStorageMigrationManifestJson)\u0060 when input is supplied.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0060 adds the public \u0060HashKeyStorageMigrationManifest\u0060 section, includes it in overall section counting, and renders it in \u0060ToDisplayString()\u0060.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPreflightTests.cs\u0060 adds explicit manifest-lane tests for skipped input, valid input, blocking invalid input, warning-only input, JSON serialization, and separation from migration guardrails.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestValidator.cs\u0060 returns raw string content from \u0060FormatJsonValue(...)\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultHashKeyStorageMigrationManifestValidatorTests.cs\u0060 asserts literal fingerprint strings are preserved in \u0060ExpectedValue\u0060 and \u0060ActualValue\u0060.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/diagnostics, area/hashing, area/migrations, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06FGX6B9KQME0NJ8B810239DG0-task-wire-migration-manifest-validation-into-pre\u0027.",
    "Evidence: Ticket history references implementation commit \u0027af2404fd699a\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: Consumers can pass a serialized dvault.hash-key-storage-migration.v1 manifest through the existing preflight-style request or equivalent diagnostics path, and the library validates it with DataVaultHashKeyStorageMigrationManifestValidator. (\u0060src/DCoding.Data.DVault/DataVaultPreflightRequest.cs\u0060 adds \u0060HashKeyStorageMigrationManifestJson\u0060, and \u0060src/DCoding.Data.DVault/DataVaultPreflight.cs\u0060 validates that input through \u0060DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(...)\u0060 in a dedicated preflight section.).",
    "AC check passed: Aggregate preflight reports manifest validation in a dedicated lane that is distinct from migration-guardrail, with blocking behavior when manifest findings include one or more error severities. (\u0060src/DCoding.Data.DVault/DataVaultPreflight.cs\u0060 creates a separate \u0060hash-key-storage-migration-manifest\u0060 lane, and \u0060src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0060 keeps it distinct from \u0060MigrationGuardrail\u0060; manifest errors set the lane to \u0060Blocked\u0060 while \u0060MigrationGuardrail\u0060 remains independently evaluated.).",
    "AC check passed: When manifest input is omitted, the preflight lane behaves like other optional lanes and reports a deterministic skipped or no-input outcome instead of inventing discovery behavior. (When \u0060HashKeyStorageMigrationManifestJson\u0060 is null, \u0060src/DCoding.Data.DVault/DataVaultPreflight.cs\u0060 returns a skipped manifest lane with the deterministic reason \u0022No hash-key storage migration manifest JSON was provided.\u0022 rather than discovering files or other inputs.).",
    "AC check passed: Tests cover valid manifests, invalid manifests, deterministic display or serialization, and clear separation between manifest-validation results and EF migration-guardrail results. (\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPreflightTests.cs\u0060 adds coverage for skipped input, valid manifests, blocking invalid manifests, non-blocking warnings, serialization output, and separation from migration-guardrail results.).",
    "DoD check passed: The public preflight request/report surface includes an explicit optional manifest-validation path with deterministic status and display behavior. (The public preflight surface now has an explicit optional manifest input on \u0060DataVaultPreflightRequest\u0060 and a corresponding \u0060HashKeyStorageMigrationManifest\u0060 section on \u0060DataVaultPreflightReport\u0060, with deterministic skipped/passed/blocked display output.).",
    "DoD check passed: The diagnostics result shape can carry the manifest-validation outcome as a separate structured section when this lane is used. (\u0060src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060 show the diagnostics report shape now carries manifest-validation as its own structured section.).",
    "DoD check passed: Existing standalone manifest-validator behavior and the hash-key-storage-migration design-time command remain compatible. (\u0060git diff --name-only develop...af2404fd699a -- src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestValidator.cs src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationValidationResult.cs tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0060 returned no paths, so the standalone command and validator surfaces were left untouched by this branch.).",
    "DoD check passed: Relevant workflow/documentation text for design-time preflight/diagnostics is updated if the public surface changes. (\u0060git diff --name-only develop...af2404fd699a\u0060 includes \u0060docs/architecture/dvault-dotnet-ef-design-time-workflow.md\u0060 and \u0060docs/production-adoption-checklist.md\u0060, and the updated text now documents aggregate preflight manifest validation and explicit caller-owned manifest input.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: If diagnostics or support-bundle output is extended for this lane, it preserves only structural manifest-validation facts or findings and emits no raw hash-key values or other secret-bearing data. (The new diagnostics lane serializes \u0060DataVaultHashKeyStorageMigrationValidationResult\u0060 directly, but \u0060src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestValidator.cs\u0060 still records verbatim string values through \u0060FormatJsonValue(...)\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultHashKeyStorageMigrationManifestValidatorTests.cs\u0060 explicitly expects literal \u0060ActualValue\u0060/\u0060ExpectedValue\u0060 strings such as \u0060target-fingerprint\u0060. That means manifest-validation diagnostics are not guaranteed structural-only or redacted.).",
    "DoD check failed: Unit tests cover lane skipping, blocking errors, non-blocking warnings/info, and any diagnostics/support-bundle serialization touched by the change. (The added unit tests cover skipped, blocking, warning/info, and one serialization case, but the serialization path touched by this change is still missing a regression that proves invalid required string fields are redacted. The only new serialization test injects an ignored extra \u0060rawHashKey\u0060 property, which does not exercise the observed verbatim-string finding path.).",
    "Blocking: the new preflight diagnostics lane reuses manifest-validator findings verbatim, but those findings still preserve caller-supplied string values. That violates the ticket\u0027s structural-only/redaction boundary for manifest-validation diagnostics.",
    "Coverage gap: the added serialization test only proves an ignored extra \u0060rawHashKey\u0060 property is absent from serialized output; it does not cover malformed required string fields, which are the path that currently exposes verbatim values."
  ],
  "evidence": [
    "\u0060git rev-parse --verify af2404fd699a^{commit}\u0060 resolved to \u0060af2404fd699a10f1d1d8ba6fda5f5186566ee022\u0060.",
    "\u0060git diff --name-only develop...af2404fd699a\u0060 shows product changes in \u0060src/DCoding.Data.DVault/DataVaultPreflight.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultPreflightRequest.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPreflightTests.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060, \u0060docs/architecture/dvault-dotnet-ef-design-time-workflow.md\u0060, and \u0060docs/production-adoption-checklist.md\u0060.",
    "\u0060src/DCoding.Data.DVault/DataVaultPreflight.cs\u0060 creates a \u0060hash-key-storage-migration-manifest\u0060 section and calls \u0060DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(request.HashKeyStorageMigrationManifestJson)\u0060 when input is supplied.",
    "\u0060src/DCoding.Data.DVault/DataVaultPreflightReport.cs\u0060 adds the public \u0060HashKeyStorageMigrationManifest\u0060 section, includes it in overall section counting, and renders it in \u0060ToDisplayString()\u0060.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPreflightTests.cs\u0060 adds explicit manifest-lane tests for skipped input, valid input, blocking invalid input, warning-only input, JSON serialization, and separation from migration guardrails.",
    "\u0060src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestValidator.cs\u0060 returns raw string content from \u0060FormatJsonValue(...)\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultHashKeyStorageMigrationManifestValidatorTests.cs\u0060 asserts literal fingerprint strings are preserved in \u0060ExpectedValue\u0060 and \u0060ActualValue\u0060.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/hashing, area/migrations, area/tests, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FGX6B9KQME0NJ8B810239DG0-task-wire-migration-manifest-validation-into-pre\u0027.",
    "Ticket history references implementation commit \u0027af2404fd699a\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Redact or structurally normalize manifest-validation finding values before they are exposed through \u0060DataVaultPreflightReport\u0060, or harden the canonical validator finding shape so invalid string-bearing fields do not echo caller-supplied content.",
    "Add a regression that passes secret-like strings through expected manifest fields and asserts both \u0060JsonSerializer.Serialize(report)\u0060 and \u0060ToDisplayString()\u0060 omit the raw value while preserving structural diagnostics.",
    "After fixing the redaction issue, run the required verification commands in the supported tester environment: \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060."
  ],
  "branchName": "ticket/06FGX6B9KQME0NJ8B810239DG0-task-wire-migration-manifest-validation-into-pre",
  "commitSha": "af2404fd699a"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FGX6B9KQME0NJ8B810239DG0`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FGX6B9KQME0NJ8B810239DG0-task-wire-migration-manifest-validation-into-pre`