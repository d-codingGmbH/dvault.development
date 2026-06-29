[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the delivery contract is source-backed, documentation-only, and has no open PO questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FGX6CRPG02ZWGE62QWSG42EC/description.md` contains `## Open Questions` -> `none` and scopes the ticket to documentation alignment only; product-code changes are explicitly out of scope.
- `git -C /mnt/c/Projects/DVault diff --name-only develop...HEAD` lists only `.gicket/tickets/06FGX6CRPG02ZWGE62QWSG42EC/**`, so no documentation or source implementation work has started on the branch yet.
- `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs` dispatches the `hash-key-storage-migration` verb, and `src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestExporter.cs` sets `CurrentSchemaVersion = "dvault.hash-key-storage-migration.v1"`.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs:300-340` asserts the exported manifest root contains `schemaVersion`, `dryRun`, `source`, `target`, `comparison`, and `entries`, proving the dry-run artifact shape is already machine-checkable.
- `src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestValidator.cs` and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultHashKeyStorageMigrationManifestValidatorTests.cs` prove built-in provider-profile and stable-hash validation, including the `sha1-v1` versus `sha256-160-v1` incompatibility case and non-blocking warnings for `capabilityProfileDefaulted`.
- `src/DCoding.Data.DVault/DataVaultPreflight.cs` plus `tests/DCoding.Data.DVault.Tests/Unit/DataVaultPreflightTests.cs` show the manifest-validation lane is optional, passes when `report.IsValid`, blocks when manifest errors exist, and keeps warning-only manifests non-blocking.
- `README.md:68-70`, `docs/hash-key-storage-migration.md:61-62,177`, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:205-223`, and `docs/production-adoption-checklist.md:53-55` already document most of the exporter/validator/preflight flow, while `rg -n "hash-key-storage-migration|HexString|UseBinaryFirstProfile|Binary" docs/releases/v0.49.0.md` returned no matches, confirming the current release-notes baseline still lacks the migration-flow pointer the ticket calls out.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A redacted sample manifest or sample preflight output is still only a follow-up question, not a handoff blocker, but the eventual docs should make the warning-vs-error distinction concrete for readers.
- The finished docs should include at least one non-blocking warning example, because current tests prove warnings are allowed while structural/compatibility defects are blocking.

Risky assumptions
- Assumes implementation will align documentation to the source-backed manifest shape already asserted in `DataVaultDesignTimeCommandTests.cs` (`schemaVersion`/`dryRun`/`source`/`target`/`comparison`/`entries`) instead of the older abstract field names currently listed in `docs/hash-key-storage-migration.md:95-107`.
- Assumes any root `README.md` wording change either preserves the packaged README assertions enforced in `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:533-619` or updates those assertions intentionally.
- Assumes `docs/releases/v0.49.0.md` remains the intended current public release-notes baseline, with `docs/releases/v0.43.0.md` treated only as historical context.

AC / test suggestions
- Verify the finished docs explicitly name the producing surface (`hash-key-storage-migration` command), the validating surfaces (`DataVaultHashKeyStorageMigrationManifestValidator.ValidateJson(...)` and optional `DataVaultPreflight.Run(...)` lane), and the caller-owned work that remains after validation.
- Add a docs review check that the release notes, README, and migration guide all distinguish `binary-first for new schemas` from `validated dry-run path for existing persisted HexString storage`.
- Spot-check the final wording against `DataVaultDesignTimeCommandTests.cs`, `DataVaultHashKeyStorageMigrationManifestValidatorTests.cs`, and `DataVaultPreflightTests.cs` so the docs match the actual warning/error behavior and manifest field names.

Implementation watchouts
- Current `docs/hash-key-storage-migration.md` describes top-level fields such as `selectedModelBoundary`, `reviewedSourceEvidence`, and `coverage`, but the exported/tested manifest shape is `schemaVersion`, `dryRun`, `source`, `target`, `comparison`, and `entries`; the update should not document a schema the exporter does not emit.
- `docs/releases/v0.49.0.md` currently has no `hash-key-storage-migration`, `HexString`, or `Binary` guidance, so release-note alignment is still real work, not already satisfied.
- README edits are coupled to package verification because the root `README.md` is checked inside `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` for exact package-line and analyzer-guidance fragments.

Non-blocking notes
- Comment history on the ticket is bot orchestration only; no human comment thread introduced new unresolved scope.
- The branch history (`git log --oneline --decorate -n 8`) shows only ticket lease and handover commits after `develop`, which is consistent with a clean pre-development handoff.

Split recommendations
- No split recommended; repository evidence shows the exporter, validator, preflight lane, and most surrounding docs already exist, so the remaining work is a bounded docs-alignment task across the migration guide, README, and current release notes.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment