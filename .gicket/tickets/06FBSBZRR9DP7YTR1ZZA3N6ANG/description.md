<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- No new child tickets, attachments, or planning documents were created because the existing split children 06FBSBZY1XEJYK1DRV4RV2ZN88, 06FBSC03KAGDABNFGPK9D95QKR, 06FBSC08W24BJGFZ87RSFS21WC, 06FBSC0EJHAY200E7PXNRGV7XR, 06FBSC0MNH0YAWQ4NY2WSC8KJG, and 06FBSC0TMZBXVVECGQGESWPCY4 are already done.

### Scope In
- Reconcile the parent ticket so it authoritatively records the closure-only roll-up of already completed child work.
- Name the repository artifacts that satisfy the original story across APIs, documentation, diagnostics, and benchmark/adoption evidence.
- Preserve the bounded product decision that named binary-first APIs are the new-project path while the compatible default remains HexString unless callers opt in.

### Scope Out
- New product code, new documentation, new diagnostics, new benchmark work, or new migration tooling beyond the already landed child deliverables.
- Reopening the runtime default, promising automatic migration or backfill, promising dual-write behavior, or exposing public byte[] hash-key values.
- Creating further split tickets, attachments, or planning documents for this parent.

## Acceptance Criteria
- The parent description states that this ticket is closure/tracking reconciliation only and that the six split child tickets are complete.
- The parent description names the authoritative evidence surfaces: README.md, docs/getting-started.md, docs/plans/hash-key-storage-profile-contract.md, CHANGELOG.md, docs/releases/v0.36.0.md, hash-key-footprint.md, src/DCoding.Data.DVault/DataVaultOptions.cs, src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs, tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelBuilderExtensionsTests.cs, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs.
- The parent contract states that success is evidence aggregation only and that no separate parent-level developer artifact remains.
- The contract preserves the bounded product decision that new projects use the named binary-first APIs explicitly, existing HexString-compatible stores remain valid until a reviewed migration, reset, or data-move change, public hash-key values remain lowercase hexadecimal strings, and diagnostics surface the selected storage profile.

## Definition of Done
- The authoritative delivery-contract block is persisted on the parent ticket and matches the landed repository evidence.
- The contract makes clear that the executable scope was delivered by the six child tickets and that the parent has no remaining implementation beyond closure reconciliation.
- The contract does not imply automatic migration, runtime default switching, or a public byte[] hash-key value type.

## Implementation Notes
- 06FBSBZY1XEJYK1DRV4RV2ZN88 delivered the named opt-in APIs UseBinaryFirstProfile() and UseDataVaultBinaryFirstProfile().
- 06FBSC03KAGDABNFGPK9D95QKR preserved the compatible default posture; AddDVault() without opt-in remains HexString-compatible in docs and tests.
- 06FBSC08W24BJGFZ87RSFS21WC plus DataVaultDiagnosticsTests.cs carry storage-profile visibility through diagnostics and support surfaces.
- 06FBSC0EJHAY200E7PXNRGV7XR and 06FBSC0TMZBXVVECGQGESWPCY4 landed the quickstart, README, changelog, and release-note guidance for the binary-first new-project recommendation without any automatic migration claim.
- 06FBSC0MNH0YAWQ4NY2WSC8KJG plus hash-key-footprint.md carry the checked-in binary-versus-hex benchmark and adoption evidence.
- No new child tickets, attachments, or planning documents were needed; the queued removal outbox mutation-3e33612cbd428b8c is non-blocking housekeeping only.

## Open Questions
- none

## Follow-Up Questions
- After this parent is closed, decide separately whether historical reporting should restore explicit parent-child roll-up links for the completed split set or continue relying on ticket history plus child descriptions.

## Risks
- none

## Split Recommendations
- No further split is justified. The parent already has six child tickets, and all six are done.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define an explicit new-project profile that selects binary hash-key storage by default without silently changing existing-project behavior. Acceptance: contract names profile entry points, default/compatibility rules, diagnostics, provider responsibilities, non-goals, and caller-owned migration boundaries.