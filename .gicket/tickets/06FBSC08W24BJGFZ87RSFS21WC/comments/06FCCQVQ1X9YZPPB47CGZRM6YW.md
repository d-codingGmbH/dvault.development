[gicket-bot] PO-critic review contract

Summary
- Ticket 06FBSC08W24BJGFZ87RSFS21WC is repo-grounded, bounded, and ready for developer handoff; the contract names the existing diagnostics surfaces, fixes the HexString/Binary vocabulary, and leaves no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSC08W24BJGFZ87RSFS21WC/description.md contains the authoritative Delivery Contract with Open Questions: none and acceptance criteria that explicitly require structured explain, human-readable diagnostics, support-bundle parity, and scenario-specific tests.
- docs/plans/hash-key-storage-profile-contract.md bounds the v1 storage-profile vocabulary to HexString and Binary and requires diagnostics and dvault.support-bundle.v1 to carry storage profile, store type, provider value format, stableHash algorithm facts, digest sizing, digest encoding, and conversion behavior without raw values.
- src/DCoding.Data.DVault/DataVaultProviderTypeMappingExplain.cs and src/DCoding.Data.DVault/DataVaultPropertyExplain.cs already expose HashKeyStorageProfile, StableHashAlgorithmId, DigestByteLength, DigestEncoding, and ConversionBehavior on the explain surfaces named by the ticket.
- src/DCoding.Data.DVault/DataVaultDiagnosticsResult.cs already renders a human-readable hash-key storage summary as hash-key storage <profile>/<storeType>/<conversionBehavior> when hash-key mappings are present.
- src/DCoding.Data.DVault/DataVaultSupportBundleExporter.cs exports the diagnostics payload, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs method AnalyzeReportsSelectedStableHashMetadataInExplainDisplayAndSupportBundle already asserts support-bundle stable-hash facts, hashKeyStorageProfile = HexString, and absence of raw business-key and raw digest values.
- Binary is direct repository-supported scope, not invented scope: src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs exposes WithHashKeyStorageProfile(...), tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs method BinaryHashKeyStorageProfileIsExplicitOptInAndKeepsStringModelBoundary verifies Binary mappings, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs method ApplyDataVaultMetadataBinaryHashKeyProfileAppliesProviderNeutralConversionToKeysAndReferences verifies translated key/reference projection.
- Branch history is still pre-development: git log shows head 5718889fcc3e20a9ed7bb4d09b0eec20b09405e6 after handoff commit af5888188b51547c9d5e423db8b7f017b463845f, and git diff --name-only b040df049e3a7905cf37705d5d1f6a0cc00c412a..af5888188b51 plus af5888188b51..5718889fcc3e20a9ed7bb4d09b0eec20b09405e6 touches only .gicket/tickets/06FBSC08W24BJGFZ87RSFS21WC/** files, not src/**, tests/**, or docs/** implementation files.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract names a third scenario bucket for any existing provider/profile-preselected Binary path, but it does not pin one concrete named example from the current repo.

Risky assumptions
- Current repo evidence directly proves explicit Binary opt-in via DataVaultProviderCapabilityProfile.WithHashKeyStorageProfile(...); developers should verify whether any separate preselected Binary path exists before claiming that third scenario is covered.
- Human-readable diagnostics currently summarize hash-key storage from the HashKey type mapping in src/DCoding.Data.DVault/DataVaultDiagnosticsResult.cs, so parity with structured explain and support-bundle output must be validated rather than assumed.

AC / test suggestions
- Assert the same storage-fact set for HashKey and ParticipantReference surfaces: hashKeyStorageProfile, provider store type, provider value format, stableHashAlgorithmId, digestByteLength, digestEncoding, and conversionBehavior.
- Keep redaction assertions on both DataVaultDiagnosticsResult.ToDisplayString() output and dvault.support-bundle.v1 JSON so raw business keys and raw digest values remain absent in HexString and Binary scenarios.
- Use the three scenario buckets exactly as written in the contract: default HexString, explicit Binary, and any existing Binary-preselected path that is directly visible in the current public API or tests.

Implementation watchouts
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs method AnalyzeReportsSelectedStableHashMetadataInExplainDisplayAndSupportBundle currently proves HexString diagnostics/support-bundle behavior only; Binary diagnostics coverage still needs to be added or expanded.
- src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs method CreateHashKeyCapabilityProfile reprojects diagnostics using the current hash-key mapping's HashKeyStorageProfile, so tests should verify that Binary selections survive that reprojection unchanged.
- Do not widen the storage vocabulary beyond HexString and Binary; docs/plans/hash-key-storage-profile-contract.md explicitly bounds v1 to those two tokens.

Non-blocking notes
- Persisted comments show a clean PO handoff: .gicket/tickets/06FBSC08W24BJGFZ87RSFS21WC/comments/06FCCJCKKBE3RN4AYZGYD34Z3M.md records ready_for_po_critic and no conflicting follow-up discussion reopened the contract.
- The current branch contains ticket-state and orchestration metadata only, which is normal for a pre-development PO-critic gate and is not a blocker by itself.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment