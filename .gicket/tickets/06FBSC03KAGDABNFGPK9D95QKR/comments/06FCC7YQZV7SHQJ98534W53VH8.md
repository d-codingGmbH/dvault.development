[gicket-bot] PO-critic review contract

Summary
- Persisted contract is resolved, repository evidence already proves the existing HexString default, and the branch still contains only PO/lease metadata changes, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSC03KAGDABNFGPK9D95QKR/description.md contains a Delivery Contract with 4 acceptance criteria, focused regression-test scope, and '## Open Questions' set to 'none'.
- git log on ticket/06FBSC03KAGDABNFGPK9D95QKR-task-preserve-existing-project-hex-compatibility shows aafb3817d '[06FBSC03KAGDABNFGPK9D95QKR] handoff po->po-critic' followed by d7180c831 '[06FBSC03KAGDABNFGPK9D95QKR] lease claim po-critic'.
- git diff --name-only 4b781d67ce2082ffaf77f709b8162ccbe7448447..HEAD lists only .gicket/tickets/06FBSC03KAGDABNFGPK9D95QKR/** files, so no implementation files have changed on the owner branch yet.
- src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs defines DataVaultConventions.Default with stable hash 'sha256-v1', digest byte length 32, and DataVaultHashKeyStorageProfile.HexString; src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers DataVaultConventions.Default in AddDVault().
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs routes UseDataVault() through UseDataVaultCore(...), which falls back to DataVaultConventions.Default hash settings when no override is present.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs maps both HashKey and ParticipantReference through HashKeyText(...), which carries LowercaseHexText, HexString, 'sha256-v1', 32, 'lowercase-hex-no-prefix', and 'none-string-model' across built-in provider profiles.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs shows WithHashKeyStorageProfile(Binary, algorithmId, digestByteLength) is the explicit path that switches mappings to LowercaseHexBinary and 'lowercase-hex-string-to-bytes'.
- tests/DCoding.Data.DVault.Tests/Modeling/DefaultNamingPolicyTests.cs already asserts AddDVault() and UseDataVault() resolve to DataVaultConventions.Default and that the default conventions expose 'sha256-v1', 32, and HexString; tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs already cover HexString vs Binary mapping facts.
- tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs plus tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt already include public snapshot coverage for AddDVault, UseDataVault, and WithHashKeyStorageProfile(...).
- hash-key-footprint.md, docs/plans/hash-key-storage-profile-contract.md, and CHANGELOG.md all state that HexString is the compatible/default storage profile and Binary is explicit opt-in physical storage only.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- If a new public greenfield/new-project binary helper lands during implementation, the ticket should explicitly cover that helper as the only supported path that changes the default hash-key storage profile.
- Regression coverage should keep HashKey and ParticipantReference assertions paired across default and explicit Binary paths so a partial default flip cannot slip through.

Risky assumptions
- The ticket assumes the current public binary-selection surface remains WithHashKeyStorageProfile(...); if a different public selector/helper is introduced concurrently, the same change must update both behavior coverage and approved API snapshots.
- The ticket assumes SQLite-local integration evidence plus existing provider-profile unit tests are sufficient for this compatibility-default task; a broader multi-provider smoke lane is explicitly deferred to follow-up discussion.

AC / test suggestions
- Add at least one negative regression that proves Binary mapping facts do not appear on default AddDVault(), UseDataVault(), or default ApplyDataVaultMetadata(...) paths without explicit selection.
- Keep API snapshot coverage around any public binary-selection surface so default-shape changes require intentional review.
- Assert compatibility facts at the persisted-shape level: storage profile, algorithm id, digest byte length, provider store type, provider value format, and conversion behavior.

Implementation watchouts
- The current owner branch is still metadata-only, so the developer handoff starts from zero implementation changes.
- Snapshot-only updates are insufficient for this ticket; the contract already warns that behavioral mapping assertions must accompany any approved baseline changes.
- Even on the Binary path, the EF/public model boundary must remain lowercase hexadecimal string values rather than raw byte arrays.

Non-blocking notes
- The persisted contract is specific enough for developer handoff because Open Questions are closed and the acceptance criteria are bounded to existing regression anchors.
- No split is needed from a PO perspective; the work is already scoped to extending existing unit, integration, and snapshot suites.

Split recommendations
- No split recommended; keep this as one bounded regression-coverage task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment