[gicket-bot] PO refinement contract

Summary
- Refined the binary-first profile API story against repository and ticket evidence. Low-level binary hash-key projection already exists; this story is bounded to a named high-level binary-first selection surface plus focused tests, while diagnostics, compatibility regression, docs, and benchmark follow-ups stay in sibling tickets.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The done contract ticket 06F9GF5FV54DGWY9GA8ZEZWM5R and docs/plans/hash-key-storage-profile-contract.md already fix the baseline: Binary changes physical storage only, logical/public hash-key values stay lowercase hexadecimal strings, and DVault does not add automatic migration or backfill behavior. This ticket should inherit that baseline rather than reopen it.
- Current code already supports binary physical hash-key projection through explicit provider-capability shaping and provider mapping tests. The missing product surface is a small named new-project selection path, not the underlying provider mapping work.
- The new profile should keep the current default stable hash algorithm baseline sha256-v1 and change only the generated hash-key storage default for the opted-in path to binary storage.
- Existing default paths remain HexString-compatible unless the new profile is explicitly selected. This story does not silently change current AddDVault(), UseDataVault(), or direct metadata projection defaults.
- Current active ticket relations show this story blocks 06FBSC0EJHAY200E7PXNRGV7XR (quickstart update) and 06FBSC0MNH0YAWQ4NY2WSC8KJG (benchmark evidence); those remain downstream work, not blockers for this refinement.

Scope In
- Add one named high-level binary-first DVault profile-selection surface for new projects on the existing public configuration families used to establish DVault conventions.
- Make the opted-in profile project the same Binary hash-key storage behavior already supported by provider capability profiles while preserving lowercase-hex string behavior at public, request, metadata, and diagnostics boundaries.
- Keep lower-level explicit provider-capability storage shaping supported for advanced/manual cases without redefining it as the default path.
- Add focused tests and public-API coverage for the new profile selection path.

Scope Out
- Diagnostics, support-bundle, or explain output work tracked by 06FBSC08W24BJGFZ87RSFS21WC.
- Broader hex-compatibility regression coverage tracked by 06FBSC03KAGDABNFGPK9D95QKR.
- Quickstart, adoption, or release-note documentation updates tracked by 06FBSC0EJHAY200E7PXNRGV7XR and 06FBSC0TMZBXVVECGQGESWPCY4.
- Benchmark or footprint evidence work tracked by 06FBSC0MNH0YAWQ4NY2WSC8KJG.
- Automatic migration, backfill, dual-write, or logical hash-key type changes.

Open questions
- none

Follow-up questions
- When the documentation tickets land, choose which supported high-level entry point becomes the canonical quickstart example for recommending the binary-first profile.
- When the diagnostics ticket lands, decide the exact user-facing label text for binary-first versus manual explicit binary configuration while reusing the same underlying storage metadata.

Risks
- If the implementation records only Binary storage and not the named profile identity, downstream diagnostics and supportability work cannot reliably distinguish binary-first selection from manual explicit binary provider shaping.
- If the convenience surface is added on only one configuration family, some new-project setup paths will still require low-level provider-capability plumbing and the story will under-deliver on the API goal.

Split recommendations
- No further split is needed. The surrounding diagnostics, compatibility, documentation, and benchmark work is already separated into sibling tickets.

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