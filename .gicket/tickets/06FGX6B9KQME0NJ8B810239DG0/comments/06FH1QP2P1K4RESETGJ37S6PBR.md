[gicket-bot] PO-critic review contract

Summary
- Fresh ticket, comment, branch, and repository inspection supports developer handoff: the contract is specific, Open Questions is none, and the requested manifest-validation lane aligns with existing preflight, guardrail, and redaction surfaces.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestValidator.cs:7-49 exposes ValidateJson(string) for the current hash-key storage migration dry-run manifest, and src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationValidationResult.cs:22-30 exposes deterministic redacted findings with IsValid based on error severity.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs:293-340 verifies the hash-key-storage-migration command emits schemaVersion=dvault.hash-key-storage-migration.v1 plus dryRun, source, target, comparison, and entries.
- src/DCoding.Data.DVault/DataVaultPreflightRequest.cs:69-90, src/DCoding.Data.DVault/DataVaultPreflight.cs:17-176, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultPreflightTests.cs:14-39 and 123-148 show the existing aggregate preflight pattern is explicit and caller-owned, skips omitted optional lanes, and keeps migration-guardrail as a separate lane with blocking semantics.
- src/DCoding.Data.DVault/DataVaultPreflightReport.cs:20-137 and src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs:23-105 preserve lane-by-lane reporting and a distinct DataVaultMigrationGuardrailReport, matching the ticket's separation requirement.
- src/DCoding.Data.DVault/DataVaultSupportBundleExporter.cs:20-27 serializes support-bundle JSON through DataVaultSupportBundleRedactor.Redact(node), which matches the ticket's structural-only redaction boundary.
- git show --stat --summary --format=fuller 618f32e81100afbec9b73de01a902f7b812dabd2 shows the branch head touches only .gicket ticket metadata, comment, and event files, with no src/ or tests/ product changes yet; that is consistent with a pre-development review branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developers must wire the current validator/exporter manifest shape, not the older conceptual field names still described in some repository docs; the done upstream ticket 06FGX69QJYHGNKBV8MJ1HG7MMG records that mismatch risk.
- If this work extends diagnostics or support-bundle output, the exact projection shape is intentionally left additive; only the redaction boundary and lane separation are fixed by the contract.

AC / test suggestions
- Keep public API snapshot coverage in scope because tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt already snapshots DataVaultPreflight and the manifest-validation result types.
- Preserve explicit coverage for omitted input => skipped lane, warnings/info without errors => non-blocking lane, errors => blocked lane, and display output that keeps manifest validation separate from migration guardrails.

Implementation watchouts
- Use the existing DataVaultHashKeyStorageMigrationManifestValidator and DataVaultHashKeyStorageMigrationValidationResult directly; the ticket is integration over an existing validator, not a new manifest contract.
- Do not fold manifest findings into DataVaultMigrationGuardrailReport or emit raw manifest/support-bundle payload data; the current preflight/report surfaces already model separate lanes and redacted artifacts.

Non-blocking notes
- The current branch head is a lease-claim metadata commit only; no product implementation has started yet, which is normal for this gate.
- The follow-up question about the incoming blocks relation from 06FGX69QJYHGNKBV8MJ1HG7MMG is historical scheduling context only because the related ticket is already done.

Split recommendations
- No split recommended: direct repository evidence shows the validator, preflight scaffolding, migration-guardrail lane, and support-bundle redaction baseline already exist, so the remaining work is bounded integration and test wiring.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment