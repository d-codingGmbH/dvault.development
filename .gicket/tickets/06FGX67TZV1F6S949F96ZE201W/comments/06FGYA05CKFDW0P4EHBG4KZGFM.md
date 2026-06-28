[gicket-bot] PO-critic review contract

Summary
- PO contract is ready for developer handoff: the persisted ticket has no open questions, the scope is bounded to the v1 manifest validation contract, and the required vocabulary is anchored in current repo docs and source.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FGX67TZV1F6S949F96ZE201W/description.md contains `PO Handoff` decision `ready_for_po_critic` and `## Open Questions` = `none`.
- `git log --oneline` for the ticket paths shows head `f11663bca` (po-critic claim) and PO handoff commit `e447784db`; `git show --name-only e447784dbd42` changed only ticket description/comment/event metadata files, so the current branch state is a clean ticket-handoff surface.
- .gicket/relations/1W/MG/06FGX67TZV1F6S949F96ZE201W--06FGX69QJYHGNKBV8MJ1HG7MMG--blocks.json keeps this ticket blocking `06FGX69QJYHGNKBV8MJ1HG7MMG`, and .gicket/tickets/06FGX69QJYHGNKBV8MJ1HG7MMG/description.md says the downstream task will `Implement a small parser/validator ... according to the accepted contract`, so the dependency line is coherent.
- docs/hash-key-storage-migration.md defines the same v1 boundary the ticket asks for: adopter-owned `HexString` to `Binary`, authoritative `dvault.support-bundle.v1` or equivalent translated EF metadata, full `HashKey`/`ParticipantReference` coverage across hubs, links, satellites, PITs, and bridges, and fail-closed drift rules.
- docs/plans/hash-key-storage-profile-contract.md fixes the provider baseline (`sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, `mysql-pomelo-v1`), required per-column facts, and the `sha1-v1` versus `sha256-160-v1` same-size incompatibility case referenced by the ticket.
- src/DCoding.Data.DVault/BuiltInStableHashService.cs exposes built-in ids `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1` with digest lengths 32/20/16/20, matching the ratified stable-hash baseline.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs and src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs encode the provider profile ids plus HexString/Binary store and conversion projections, and docs/architecture/dvault-dotnet-ef-design-time-workflow.md plus tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs already use `dvault.hash-key-storage-migration.v1`, deterministic ordering, and hub/link/satellite/PIT/bridge coverage vocabulary that matches this contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add one explicit invalid example where coverage totals look correct but the same `tableName/propertyName` pair appears twice, so duplicate coverage handling is unambiguous.
- Add one explicit invalid example for unsupported or defaulted provider-profile identity even when store types and digest sizes otherwise look compatible.
- Add one explicit invalid example for selected-boundary provenance drift such as `metadataSourceFingerprint` or equivalent boundary identity changing while per-column facts stay the same.
- Add one explicit warning-only example for unavailable supplemental live-schema evidence so the error/warning/info split is concrete.

Risky assumptions
- The ticket implies fail-closed handling for unsupported provider/profile values, but it does not name whether a defaulted capability profile must be treated as a blocking unsupported-profile case.
- The ticket requires selected model boundary and reviewed source evidence provenance, but it intentionally leaves the concrete field naming open; downstream work will need to keep that aligned with existing metadata-source vocabulary rather than inventing a parallel identity scheme.
- The ticket reserves warnings for non-blocking evidence gaps; downstream work should not widen warning usage to structural manifest defects or profile/algorithm drift.

AC / test suggestions
- Keep deterministic negative coverage for missing coverage, duplicate coverage, mixed-profile rejection, algorithm or digest drift, and the `sha1-v1` versus `sha256-160-v1` same-size incompatibility case already called out by the contract.
- Add a boundary-drift case where `providerName`, `capabilityProfile`, or `metadataSourceFingerprint` changes while column facts remain otherwise compatible.
- Add a warning-only fixture for unavailable supplemental live-schema evidence so classification stays testable and deterministic.
- Assert stable finding ordering for semantically identical manifests even when input object/member order differs.

Implementation watchouts
- Fail closed on the full persisted compatibility fact set; equal width or equal store type alone is not proof of compatibility.
- Coverage must stay complete across hubs, links, satellites, PITs, and bridges, including every `ParticipantReference` column in the selected boundary.
- `dvault.support-bundle.v1` or equivalent translated EF metadata should remain the authoritative baseline; live-schema evidence stays supplemental only.
- The v1 path is storage-only `HexString` to `Binary`; caller-facing hash-key values and EF CLR exposure remain lowercase hexadecimal strings.

Non-blocking notes
- The repo already contains adjacent dry-run/export vocabulary in docs/architecture/dvault-dotnet-ef-design-time-workflow.md and src/DCoding.Data.DVault/DataVaultHashKeyStorageMigrationManifestExporter.cs, so this ticket is grounded in existing repository terminology rather than speculative naming.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs already exercises deterministic manifest export and algorithm/digest drift rejection, which lowers terminology risk for the downstream validator ticket without turning this ticket into a post-development review.
- The existing blocks dependency on 06FGX69QJYHGNKBV8MJ1HG7MMG remains appropriate: this ticket defines the contract, the downstream ticket implements the validator against it.

Split recommendations
- No split recommended; the ticket is already bounded to the v1 manifest validation contract and separated from downstream implementation by the existing blocks relation to 06FGX69QJYHGNKBV8MJ1HG7MMG.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment