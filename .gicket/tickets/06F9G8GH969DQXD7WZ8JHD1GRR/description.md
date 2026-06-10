<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the DB2 provider support epic as an already-split tracking parent: repository evidence and completed child tickets cover the contract, package, schema/guardrails, integration, package-verification, and documentation lanes, and no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this PO pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Existing epic children already cover the full bounded DB2 slice: 06F9G8GS08VNH0DT09Q4PC2HRC (contract), 06F9G8GZ384VKA7RVF039WKX1M (provider package), 06F9G8H5HE1CJHQXGC2C2YK7P8 (schema and live-schema guardrails), 06F9G8HBXS7Y42J7XFSQKZ2AZ8 (save/read integration), 06F9G8HJJDJH4KF9VK6TZ8B1Z0 (package verification), and 06F9G8HRZ72XP5Z7FNWM6MBMQC (documentation).
- Repository evidence now includes the concrete DB2 package surface, AddDVaultDb2 registration, explicit DB2 live-schema unsupported handling, opt-in DB2 save/read smoke coverage, and v0.34.0 DB2 release documentation, so the epic no longer needs another scope split.
- Direct read of child ticket 06F9G8GS08VNH0DT09Q4PC2HRC exceeded the local result-byte cap in this slice, but multiple completed child contracts consistently cite it as the authoritative DB2 baseline and the repository state matches that baseline.
- No bounded writes were applied in this refinement run: no new child tickets, relation updates, description updates, attachments, or planning documents.

### Scope In
- Track the bounded DB2 provider-support slice across provider package registration, provider capability and schema guardrails, provider-neutral save/read compatibility evidence, package verification, and release/documentation alignment.
- Use DCoding.Data.DVault.Db2 plus AddDVaultDb2 as the explicit consumer-facing DB2 entry point for the DVault package family.
- Keep DB2 validation opt-in and externally provisioned through DVAULT_TEST_DB2_CONNECTION_STRING rather than making DB2 part of the default local repository baseline.

### Scope Out
- DB2 provisioning, deployment orchestration, container recipes, CI-owned DB2 infrastructure, credentials, or default local DB2 requirements.
- Provider-native DB2 save/read optimization, DB2 live-schema reader implementation, or platform/tooling work beyond the bounded provider-support lanes already planned.
- Broader provider-matrix expansion, DB2-specific performance claims, or SQL-artifact automation beyond the documented v1 boundaries.

## Acceptance Criteria
- The epic is fully decomposed into bounded child lanes covering DB2 contract, provider package registration, schema/guardrails, integration coverage, package verification, and documentation/release notes, with no missing unowned scope inside the original epic boundary.
- Repository evidence shows an actual DB2 provider package and registration surface: src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj exists, AddDVaultDb2 is present, and IBM.EntityFrameworkCore is pinned for both net8.0 and net10.0.
- Repository evidence shows DB2 schema/read/save compatibility is explicit and bounded: DB2 opt-in smoke coverage exists for representative saves and reads, and live-schema handling for IBM.EntityFrameworkCore is explicitly unsupported rather than implicit.
- Repository documentation reflects the DB2 baseline through the v0.34.0 release note and related package-family guidance without expanding scope to DB2 provisioning or default external-database requirements.
- No additional child-ticket split is required to represent the epic's planned v1 DB2 scope.

## Definition of Done
- All DB2 epic lanes remain represented by the existing child tickets and repository evidence without reopening provider-name, package-line, schema-guardrail, integration, verification, or documentation decisions.
- The epic has no remaining PO-blocking scope ambiguity or missing child ownership within the original boundary.
- Any residual relation housekeeping is tracker maintenance rather than missing product-scope definition.

## Implementation Notes
- src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj shows a packable multi-target DB2 provider package with target frameworks net8.0;net10.0 and IBM.EntityFrameworkCore pins 8.0.0.400 and 10.0.0.100.
- src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs shows AddDVaultDb2 registering IBM.EntityFrameworkCore against DataVaultProviderCapabilityProfiles.Db2, then calling AddDVault and adding DB2 provider behavior.
- tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs covers representative DB2 hub/link/satellite saves plus latest/as-of satellite, PIT, and bridge reads through provider-neutral fallback behind the DB2 opt-in test gate.
- src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs keeps IBM.EntityFrameworkCore on an explicit unsupported live-schema reader path, which matches the bounded DB2 baseline instead of implying reader parity with other providers.
- docs/releases/v0.34.0.md records the eight-package family, DB2 consumer package lines 8.34.0 and 10.34.0, the manual publication boundary, and the opt-in external DB2 evidence posture.
- Completed child tickets already cover the epic lanes for package, schema/guardrails, integration, verification, and documentation; no further child-ticket materialization was justified by the current evidence.

## Open Questions
- none

## Follow-Up Questions
- none

## Risks
- The live relation graph still contains an incoming blocks edge from done documentation ticket 06F9G8HRZ72XP5Z7FNWM6MBMQC, so tracker closure automation may need relation cleanup even though scope evidence is complete.
- Direct read of ticket 06F9G8GS08VNH0DT09Q4PC2HRC exceeded the local result-byte cap in this slice, so this refinement relies on corroborating completed child contracts and repository state for that authoritative DB2 baseline.

## Split Recommendations
- No additional split recommended; the epic already has six child lanes covering contract, package, schema/guardrails, integration, verification, and documentation, and repository evidence shows those lanes are complete.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add focused DB2 support as another EF Core provider package for DVault. Keep scope to provider registration, capabilities, schema/read/save compatibility, tests, docs, and package verification. Do not add DB2 provisioning, deployment orchestration, platform tooling, or default provider-specific runtime behavior beyond supported DVault provider patterns.