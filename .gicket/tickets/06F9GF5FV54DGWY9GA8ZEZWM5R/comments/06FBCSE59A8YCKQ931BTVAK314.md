[gicket-bot] PO-critic review contract

Summary
- The ticket is mostly well-refined and aligned to current repository surfaces, but one contract gap remains: it does not say how to handle stable-hash algorithm-id changes that keep the same digest length and store shape.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `gicket-read-ticket` returned ticket `06F9GF5FV54DGWY9GA8ZEZWM5R` revision `06FBCQM4QJN76MK8SCJ6PNM0M4` with `## Open Questions` = `none` and a detailed Delivery Contract.
- `gicket-read-ticket-comments` returned 10 comments and they are automation claim/lease/handover/report entries; the ticket snapshot itself lists `Recent comments: <none>`.
- `git rev-parse HEAD` returned `cf9e7a6fea15eb9d62e10084fb1cbbc00d929772`, matching the provided `scratch-source-ref`, and `git diff --stat cf9e7a6fea15eb9d62e10084fb1cbbc00d929772..HEAD` returned no output.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs` defines six built-in profiles: `sqlite-v1`, `oracle-v1`, `postgres-v1`, `db2-v1`, `sqlserver-v1`, and `mysql-pomelo-v1`.
- `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` maps `IBM.EntityFrameworkCore` to `UnsupportedDataVaultLiveSchemaReader`, matching the ticket's stated DB2 live-schema unsupported boundary.
- `src/DCoding.Data.DVault/BuiltInStableHashService.cs`, `docs/plans/stable-hashing-contract.md`, and `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs` show `sha1-v1` and `sha256-160-v1` both use 20 digest bytes / 40 lowercase-hex characters.
- `src/DCoding.Data.DVault/DataVaultStableHashExplain.cs`, `src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs` show diagnostics/support-bundle already expose `algorithmId`, `digestByteLength`, and `digestEncoding` without emitting raw digest values.
- `src/DCoding.Data.DVault/DataVaultLatestSatelliteReadRequest.cs`, `src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs`, `src/DCoding.Data.DVault/DataVaultBridgeEndpointReadValue.cs`, and `src/DCoding.Data.DVault/StableHashDigest.cs` keep hash-key request/read/result surfaces string-based and canonical lowercase-hex.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` writes `ProviderLogicalPropertyKind`, `ProviderStorageType`, and `ProviderValueFormat`; `src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs` currently applies provider-column-shape guardrails only to `LoadTimestamp` and `SatelliteSnapshotReference`.
- `docs/plans/provider-identifier-ddl-guardrail-contract.md` still documents only five supported provider profiles, while `docs/production-adoption-checklist.md` and `DataVaultProviderCapabilityProfiles.cs` already show six including DB2.

Blocking findings
- none

Required PO actions
- Amend the Delivery Contract/Acceptance Criteria to state the expected behavior when the stable hash `algorithmId` changes but digest length and store type do not, using the concrete `sha1-v1` versus `sha256-160-v1` case.
- If the expected behavior is fail-closed, name the authoritative comparison surface for that check, e.g. support-bundle drift, reviewed artifact, EF annotation, or preflight baseline, so downstream implementation and tests are unambiguous.

Open issues ledger
- critic-item-1 [required-po-action] Amend the Delivery Contract/Acceptance Criteria to state the expected behavior when the stable hash `algorithmId` changes but digest length and store type do not, using the concrete `sha1-v1` versus `sha256-160-v1` case.
- critic-item-2 [required-po-action] If the expected behavior is fail-closed, name the authoritative comparison surface for that check, e.g. support-bundle drift, reviewed artifact, EF annotation, or preflight baseline, so downstream implementation and tests are unambiguous.

Missing examples / edge cases
- Same-length algorithm swap: `sha1-v1` <-> `sha256-160-v1` under `HexString`.
- Same-length algorithm swap: `sha1-v1` <-> `sha256-160-v1` under `Binary` where physical bytes change semantics but persisted shape does not.
- Binary round-trip examples for both 20-byte and 32-byte algorithms that prove logical lowercase-hex strings stay unchanged at API, read, and diagnostic boundaries.

Risky assumptions
- Assuming `digestByteLength` uniquely identifies hash semantics is unsafe in this repository because two built-in ids already share the same 20-byte length.
- Assuming implementers will ignore older five-provider planning text is risky; the repository currently contains both five-profile and six-profile documentation surfaces.

AC / test suggestions
- Add an explicit acceptance/test requirement for algorithm-id drift handling when persisted shape is unchanged.
- Add a provider/profile matrix that exercises `HexString` and `Binary` against all four built-in algorithm ids and asserts diagnostics expose `algorithmId`, `digestByteLength`, `digestEncoding`, storage profile, and provider store type without raw digests.
- Add a fail-closed or explicitly-not-detectable example for DB2 live-schema unsupported mode so reviewers know which drift lanes are expected to stop at explain/preflight only.

Implementation watchouts
- Current built-in provider mappings in `DataVaultProviderCapabilityProfiles.cs` are all text-oriented for `HashKey` and `ParticipantReference`, so Binary opt-in requires new provider mappings rather than a doc-only rename.
- Current provider-column-shape drift checks in `DataVaultMigrationOperationDiagnostics.cs` only special-case `LoadTimestamp` and `SatelliteSnapshotReference`; hash-key columns and hash-key references will need equivalent guardrail coverage to meet the ticket intent.
- The older planning doc `docs/plans/provider-identifier-ddl-guardrail-contract.md` still lists only five provider profiles and could mislead downstream reviewers unless the final contract explicitly supersedes that baseline.

Non-blocking notes
- The branch review surface is clean for this ticket contract: `HEAD` equals the provided scratch source ref and the diff stat to that ref is empty.
- Aside from the algorithm-id gap above, the ticket is otherwise well-scoped: open questions are explicitly `none`, scope in/out is concrete, and the DB2 unsupported live-schema boundary is already aligned to current source.

Split recommendations
- If the team wants to reduce scope, keep the ticket's existing split: separate provider-profile/annotation/storage-profile contract work from migration/live-schema/explain guardrail work, but resolve the same-length algorithm-id compatibility rule in the parent contract before handoff.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment