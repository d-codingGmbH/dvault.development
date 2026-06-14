[gicket-bot] PO refinement contract

Summary
- Contract-only refinement: source-backed the existing low-level binary hash-key storage support, explicitly restated that no named high-level binary-first public API is visible in the current branch, and bounded this story to adding that additive API plus focused tests while existing sibling tickets keep diagnostics, compatibility, docs, and benchmark follow-up work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is restated against source-backed evidence: the current branch already exposes low-level binary storage support through DataVaultHashKeyStorageProfile.Binary, DataVaultProviderCapabilityProfile.WithHashKeyStorageProfile(...), and EF metadata translation tests, but it does not visibly expose a named high-level binary-first public API yet. This story therefore adds that API explicitly instead of assuming it already exists.
- critic-item-2: `answered` - The refined contract no longer infers an existing public binary-first API or type. The visible high-level setup families in source are AddDVault(), UseDataVault(), UseDataVaultMetadata(), and ApplyDataVaultMetadata(...); the missing additive work is to extend that conventions path so callers can opt into a named binary-first profile without manual provider-capability mutation.
- critic-item-3: `answered` - The unsupported summary claim is replaced with: current source already supports low-level binary hash-key projection, and this story adds the missing additive named high-level binary-first profile-selection API plus focused tests while diagnostics, compatibility regression, docs, and benchmark evidence remain in sibling tickets.

Clarifications
- Current branch evidence already proves the low-level binary storage baseline: DataVaultHashKeyStorageProfile.Binary keeps the logical/public model boundary as lowercase hexadecimal string values, while provider mappings project byte storage.
- Current branch evidence also shows the missing product surface: AddDVault(), UseDataVault(), UseDataVaultMetadata(), and ApplyDataVaultMetadata(...) are visible high-level setup families, but the inspected public setup files do not yet show a named binary-first selector on those surfaces.
- DataVaultConventions.Default and the built-in provider profiles keep the current default bounded to sha256-v1 plus HexString; this story must preserve that default for non-opted-in callers.
- The new binary-first profile changes only the physical default for generated hash-key and hash-key-reference storage in opted-in models. Logical/public hash-key values remain lowercase hexadecimal strings per docs/plans/hash-key-storage-profile-contract.md.
- No child tickets, relation changes, attachments, or planning documents were materialized in this refinement run because the existing sibling split already bounds the remaining diagnostics, compatibility, documentation, and benchmark work.

Scope In
- Add one additive named binary-first profile-selection API on the existing conventions-owning high-level setup path, so common new-project configuration does not require callers to manually compose provider capabilities with WithHashKeyStorageProfile(...).
- Carry the selected profile through the shared conventions path used by AddDVault() app defaults, UseDataVaultMetadata() projection, and direct model projection so the same opted-in profile reaches EF metadata translation consistently.
- Project Binary storage for generated hash-key and participant-reference columns through the existing provider capability pipeline while preserving lowercase-hex string values at public, request, metadata, diagnostics, and support-bundle boundaries.
- Add focused tests and any required public API snapshot coverage for the new named profile path and unchanged default behavior.

Scope Out
- Changing the logical/public hash-key value type or format away from lowercase hexadecimal strings.
- Automatic migration, backfill, dual-write, repair, or silent persisted-data compatibility handling for existing HexString data.
- New diagnostics/support-bundle UX, broader hex-compatibility regression matrices, quickstart or release-note docs, and benchmark evidence beyond the minimal code/test work needed to land the profile-selection API.
- Redesigning or removing the existing low-level provider-capability path; advanced callers can keep using explicit WithHashKeyStorageProfile(...) shaping independently.

Open questions
- none

Follow-up questions
- When the documentation sibling ticket lands, choose which supported high-level setup entry point becomes the canonical quickstart example for recommending the binary-first profile.
- When the diagnostics sibling ticket lands, decide the exact user-facing label text that distinguishes the named binary-first profile from manual explicit binary provider shaping.

Risks
- If the implementation exposes only raw Binary storage metadata and no stable named profile identity, later diagnostics and supportability work cannot reliably distinguish named binary-first selection from manual explicit binary shaping.
- If the new selector lands on only one conventions-owning setup path, some common new-project setups will still require low-level provider-capability plumbing and the story will under-deliver on the API goal.
- If the selected profile bypasses existing conventions and model-cache plumbing, different contexts could reuse the wrong cached EF model shape for hash-key storage.

Split recommendations
- No further split is needed. Existing sibling tickets already own diagnostics/support-bundle work (06FBSC08W24BJGFZ87RSFS21WC), broader compatibility regression (06FBSC03KAGDABNFGPK9D95QKR), quickstart/release documentation (06FBSC0EJHAY200E7PXNRGV7XR and 06FBSC0TMZBXVVECGQGESWPCY4), and benchmark evidence (06FBSC0MNH0YAWQ4NY2WSC8KJG).

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment