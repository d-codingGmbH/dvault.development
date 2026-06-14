[gicket-bot] PO refinement contract

Summary
- Repository evidence already ratifies `sha256-v1` plus `HexString` as the existing-project default across `AddDVault()`, `UseDataVault()`, built-in provider capability profiles, diagnostics, docs, and approved API snapshots; this ticket should stay bounded to regression coverage that proves only explicit binary-profile selection changes hash-key storage. No child tickets, relation edits, description updates, attachments, or planning documents were materialized; the live `blocks` relation to `06FBSC0TMZBXVVECGQGESWPCY4` remains unchanged.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- `DataVaultConventions.Default`, `services.AddDVault()`, and `modelBuilder.UseDataVault()` already resolve to `sha256-v1`, 32 digest bytes, and `DataVaultHashKeyStorageProfile.HexString` in the visible source and tests.
- Built-in provider profiles already default hash-key and participant-reference mappings to lowercase-hex text plus `none-string-model`; binary mapping is explicit opt-in through `WithHashKeyStorageProfile(..., DataVaultHashKeyStorageProfile.Binary, ...)`.
- Repository docs already state that logical hash-key values stay lowercase hexadecimal strings, `HexString` is the compatible default, and `Binary` is explicit opt-in physical storage only.
- No bounded planning writes were applied during refinement; the existing `blocks` relation to `06FBSC0TMZBXVVECGQGESWPCY4` stays live.

Scope In
- Add regression coverage for existing-project default startup and model paths so `AddDVault()`, `UseDataVault()`, and default metadata translation keep `HexString`-compatible hash storage.
- Add regression coverage proving that explicit binary-profile selection is the only supported path that flips hash-key storage behavior away from the compatibility default.
- Protect both `HashKey` and `ParticipantReference` mappings so a partial default flip cannot pass unnoticed.
- Keep approved public API snapshot coverage aligned with any public selector or helper involved in the binary-profile story.

Scope Out
- Changing the existing-project default storage profile away from `HexString`.
- Automatic migration, backfill, dual-write, repair, or rehash behavior for persisted hashes.
- New provider-footprint or performance claims beyond the checked-in SQLite-local evidence bundle.
- Unrelated stable-hash algorithm-selection changes or broader storage-profile redesign.

Open questions
- none

Follow-up questions
- When the downstream new-project binary-profile ticket lands, should adopter-facing docs explicitly label that helper as greenfield-only and link back to the compatibility and migration caveats?
- Does the downstream binary-profile work need a separate provider-matrix smoke lane proving the explicit helper selects the same binary mapping facts across every built-in provider profile, beyond this default-preservation ticket?

Risks
- If coverage only exercises one entry point, another default path could still drift; `AddDVault()`, `UseDataVault()`, and default metadata translation all need protection.
- Snapshot approval alone can hide behavioral drift if reviewers accept changed baselines without matching runtime mapping assertions.
- Only asserting primary hash-key columns would miss regressions on participant references, which are part of the same persisted-compatibility contract.
- The live `blocks` relation to `06FBSC0TMZBXVVECGQGESWPCY4` remains until this regression coverage is delivered.

Split recommendations
- No split recommended; the work is already bounded to extending existing unit, integration, and snapshot suites around one compatibility default.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment