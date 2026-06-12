[gicket-bot] PO-critic review contract

Summary
- Ticket contract is repository-aligned, bounded to provider-specific mapping work, has no open questions, and is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9GF5TNAXBCKN5BD9CKD7WVG/description.md:29-51 contains 5 acceptance criteria, 4 definition-of-done items, and ## Open Questions -> - none.
- .gicket/tickets/06F9GF5N4N3Q685XQPKTM5EC00/ticket.json:7-17 shows the provider-neutral predecessor story is done and not blocked.
- .gicket/tickets/06F9GF60BKEW0CC9FCZRPVX0SR/description.md:1 scopes end-to-end schema/save/read coverage into the downstream task, and .gicket/relations/VG/SR/06F9GF5TNAXBCKN5BD9CKD7WVG--06F9GF60BKEW0CC9FCZRPVX0SR--blocks.json records this story blocks that task.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:11-20 registers the exact built-in provider-name baseline used by the contract: Sqlite, SqlServer, Postgres, Oracle, IBM DB2, and MySql/Pomelo.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs:238-247 and 332-353 define Binary as explicit opt-in with string model CLR type and provider store types BLOB, RAW(n), bytea, varbinary(n), VARBINARY(n), and varbinary(n).
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:940-949 and 968-974 write ProviderProfile, ProviderStorageType, ProviderValueFormat, HashKeyStorageProfile, StableHashAlgorithmId, StableHashDigestByteLength, StableHashDigestEncoding, and HashKeyConversionBehavior, and only apply the string->byte[] converter when LowercaseHexBinary is selected.
- src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:326-343 preserves the bounded capability-profile-defaulted and provider-behavior-defaulted warning surfaces for unresolved provider selection; src/DCoding.Data.DVault/DataVaultSupportBundleExporter.cs:37-41 exports diagnostics through DataVaultSupportBundle JSON.
- src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs:18-25 and 27-35 routes DB2 to UnsupportedDataVaultLiveSchemaReader, and tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs:72-82 asserts DB2 remains explicitly unsupported for live-schema reads.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:460-483, tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:<redacted>, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:520-600 already verify six-provider Binary mappings, string CLR boundary, digest metadata, provider byte[] conversion, and fail-closed drift checks.
- git log --oneline -n 12 on the ticket branch shows only ticket workflow commits after develop (b39171c73, 198c7684a, a2fb08e50, 3bfd047db), and git diff --name-only e63027b01..HEAD returned only files under .gicket/tickets/06F9GF5TNAXBCKN5BD9CKD7WVG/; no src/, tests/, or docs/ implementation changes are on the branch yet.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A worked example for same-width incompatibility (sha1-v1 vs sha256-160-v1) would make the fail-closed algorithmId rule easier to test.
- A worked example for the unresolved-provider warning path would make the defaulted-diagnostics boundary easier to validate.

Risky assumptions
- This story assumes unsupported or unregistered providers may still fall through the existing SQLite capability path as long as capability-profile-defaulted and provider-behavior-defaulted warnings stay visible.
- This story assumes DB2 live-schema drift validation remains intentionally unsupported even though DB2 provider profile registration and provider packages exist.
- This story assumes downstream task 06F9GF60BKEW0CC9FCZRPVX0SR will carry the cross-provider schema/save/read integration proof, so this ticket can stop at capability-profile, translator, diagnostics, and guardrail surfaces.

AC / test suggestions
- Keep explicit assertions for both HashKey and ParticipantReference under Binary across all six built-in provider profiles.
- Add or preserve guardrail tests that fail closed on same-width algorithm drift and on storage-profile drift for Binary mappings.
- Add diagnostics/support-bundle assertions that exported property facts include ProviderStorageType, ProviderValueFormat, HashKeyStorageProfile, StableHashAlgorithmId, StableHashDigestByteLength, StableHashDigestEncoding, and HashKeyConversionBehavior.

Implementation watchouts
- Preserve the exact provider-name selection table in src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs; do not add heuristic provider matching beyond the existing bounded mechanism.
- Keep the model/public hash-key boundary as string; the byte[] conversion belongs only at the provider layer in DataVaultEfMetadataTranslator.
- Preserve DefaultDataVaultDiagnosticsService defaulted-warning behavior for unresolved providers.
- Do not expand this story into DB2 live-schema reader work or the end-to-end provider integration coverage already owned by 06F9GF60BKEW0CC9FCZRPVX0SR.

Non-blocking notes
- The branch is still pre-development: git diff --name-only e63027b01..HEAD lists only files under .gicket/tickets/06F9GF5TNAXBCKN5BD9CKD7WVG/.
- The relation .gicket/relations/VG/SR/06F9GF5TNAXBCKN5BD9CKD7WVG--06F9GF60BKEW0CC9FCZRPVX0SR--blocks.json already documents that this story gates the downstream integration-test task.

Split recommendations
- No split recommended; the story is already bounded between done predecessor 06F9GF5N4N3Q685XQPKTM5EC00 and downstream integration task 06F9GF60BKEW0CC9FCZRPVX0SR.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment