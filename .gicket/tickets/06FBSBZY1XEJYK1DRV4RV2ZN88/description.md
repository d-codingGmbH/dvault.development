<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Contract-only refinement: source-backed the existing low-level binary hash-key storage support, explicitly restated that no named high-level binary-first public API is visible in the current branch, and bounded this story to adding that additive API plus focused tests while existing sibling tickets keep diagnostics, compatibility, docs, and benchmark follow-up work.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current branch evidence already proves the low-level binary storage baseline: DataVaultHashKeyStorageProfile.Binary keeps the logical/public model boundary as lowercase hexadecimal string values, while provider mappings project byte storage.
- Current branch evidence also shows the missing product surface: AddDVault(), UseDataVault(), UseDataVaultMetadata(), and ApplyDataVaultMetadata(...) are visible high-level setup families, but the inspected public setup files do not yet show a named binary-first selector on those surfaces.
- DataVaultConventions.Default and the built-in provider profiles keep the current default bounded to sha256-v1 plus HexString; this story must preserve that default for non-opted-in callers.
- The new binary-first profile changes only the physical default for generated hash-key and hash-key-reference storage in opted-in models. Logical/public hash-key values remain lowercase hexadecimal strings per docs/plans/hash-key-storage-profile-contract.md.
- No child tickets, relation changes, attachments, or planning documents were materialized in this refinement run because the existing sibling split already bounds the remaining diagnostics, compatibility, documentation, and benchmark work.

### Scope In
- Add one additive named binary-first profile-selection API on the existing conventions-owning high-level setup path, so common new-project configuration does not require callers to manually compose provider capabilities with WithHashKeyStorageProfile(...).
- Carry the selected profile through the shared conventions path used by AddDVault() app defaults, UseDataVaultMetadata() projection, and direct model projection so the same opted-in profile reaches EF metadata translation consistently.
- Project Binary storage for generated hash-key and participant-reference columns through the existing provider capability pipeline while preserving lowercase-hex string values at public, request, metadata, diagnostics, and support-bundle boundaries.
- Add focused tests and any required public API snapshot coverage for the new named profile path and unchanged default behavior.

### Scope Out
- Changing the logical/public hash-key value type or format away from lowercase hexadecimal strings.
- Automatic migration, backfill, dual-write, repair, or silent persisted-data compatibility handling for existing HexString data.
- New diagnostics/support-bundle UX, broader hex-compatibility regression matrices, quickstart or release-note docs, and benchmark evidence beyond the minimal code/test work needed to land the profile-selection API.
- Redesigning or removing the existing low-level provider-capability path; advanced callers can keep using explicit WithHashKeyStorageProfile(...) shaping independently.

## Acceptance Criteria
- A caller can opt into a named binary-first DVault profile through the additive high-level conventions/setup API introduced by this story, without directly mutating provider capability profiles in the common path.
- Opting into that profile projects DataVaultHashKeyStorageProfile.Binary for generated hash-key and participant-reference columns through the existing provider capability pipeline while logical/public hash-key values remain lowercase hexadecimal strings.
- Existing non-opted-in setup continues to resolve to the current sha256-v1 plus HexString default behavior.
- Advanced callers can still use the existing low-level provider-capability binary shaping path independently of the new named profile.
- Tests cover the opted-in profile behavior, unchanged default behavior, and any public API snapshot expectations for the new additive API.

## Definition of Done
- The named binary-first profile-selection surface is implemented as an additive public API and any approved public API snapshot artifacts are updated.
- Representative tests prove the selected profile reaches EF metadata translation through the shared conventions path and produces Binary storage metadata for hash-key and participant-reference columns without changing the logical string boundary.
- Representative tests prove the default non-opted-in path still resolves to sha256-v1 plus HexString behavior.
- No documentation, benchmark, migration-automation, or broader diagnostics UX work is folded into this story beyond any minimal internal profile-identity plumbing needed to keep later sibling tickets coherent.

## Implementation Notes
- Reuse the existing conventions path rather than inventing a provider-only switch: AddDVault() registers DataVaultConventions, DataVaultModelCustomizer resolves app-level conventions for UseDataVaultMetadata(), and UseDataVaultCore already projects conventions into provider capabilities before EF metadata translation.
- Reuse the existing binary storage machinery instead of duplicating mapping logic: DataVaultProviderCapabilityProfile.WithHashKeyStorageProfile(...) already maps Binary to lowercase-hex-string-to-bytes provider conversion and the translator already installs the corresponding EF value converter.
- The currently inspected public setup surfaces do not expose a named binary-first selector; it is in scope for this story to add the missing public member or type needed to express that profile explicitly.
- Keep the selected profile visible to model-cache and later diagnostics flows. DataVaultModelCacheKeyFactory already keys on hash-key storage profile, and later diagnostics work will need a stable way to distinguish named binary-first selection from manual explicit binary shaping.

## Open Questions
- none

## Follow-Up Questions
- When the documentation sibling ticket lands, choose which supported high-level setup entry point becomes the canonical quickstart example for recommending the binary-first profile.
- When the diagnostics sibling ticket lands, decide the exact user-facing label text that distinguishes the named binary-first profile from manual explicit binary provider shaping.

## Risks
- If the implementation exposes only raw Binary storage metadata and no stable named profile identity, later diagnostics and supportability work cannot reliably distinguish named binary-first selection from manual explicit binary shaping.
- If the new selector lands on only one conventions-owning setup path, some common new-project setups will still require low-level provider-capability plumbing and the story will under-deliver on the API goal.
- If the selected profile bypasses existing conventions and model-cache plumbing, different contexts could reuse the wrong cached EF model shape for hash-key storage.

## Split Recommendations
- No further split is needed. Existing sibling tickets already own diagnostics/support-bundle work (06FBSC08W24BJGFZ87RSFS21WC), broader compatibility regression (06FBSC03KAGDABNFGPK9D95QKR), quickstart/release documentation (06FBSC0EJHAY200E7PXNRGV7XR and 06FBSC0TMZBXVVECGQGESWPCY4), and benchmark evidence (06FBSC0MNH0YAWQ4NY2WSC8KJG).

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Implement a small public API or options entry point for new projects to select the binary-first DVault profile. Acceptance: existing HexString-compatible behavior remains the default for existing configuration paths, and tests cover the new profile selection.