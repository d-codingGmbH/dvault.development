[gicket-bot] PO-critic review contract

Summary
- Refined parent contract is now closure-only, cites the landed repository evidence, and has no unresolved open questions; approve for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSBZRR9DP7YTR1ZZA3N6ANG/description.md now contains the authoritative delivery-contract block, explicitly says the parent is closure/tracking reconciliation only, names the repo evidence surfaces, and has `## Open Questions` -> `- none`.
- README.md:62-64,73,85,137,163 and docs/getting-started.md:20,27,36,73 directly recommend `UseBinaryFirstProfile()` / `UseDataVaultBinaryFirstProfile()` for new projects while keeping existing `HexString`-compatible setups valid and public hash-key values as lowercase hexadecimal strings.
- src/DCoding.Data.DVault/DataVaultOptions.cs:84-89 defines `UseBinaryFirstProfile()`, and src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:26-27,53-62,296-310 defines `UseDataVaultBinaryFirstProfile()` with `DataVaultHashKeyStorageProfile.Binary` and `DataVaultConventions.BinaryFirstProfileName`.
- docs/plans/hash-key-storage-profile-contract.md:23-27,52-68, CHANGELOG.md:27-29, docs/releases/v0.36.0.md:45-65,105-119,170, and hash-key-footprint.md:25-34 all align on the same bounded product contract: `HexString` remains the compatible default, `Binary` is explicit opt-in, public values stay lowercase hex strings, diagnostics expose the selected profile, and DVault does not auto-migrate/backfill/dual-write.
- tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs:94-128, tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelBuilderExtensionsTests.cs:58-70, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:146-243 assert binary-first conventions and diagnostics visibility for the storage profile.
- `git log --all --grep='06FBSBZRR9DP7YTR1ZZA3N6ANG|06FBSBZY1XEJYK1DRV4RV2ZN88|06FBSC03KAGDABNFGPK9D95QKR|06FBSC08W24BJGFZ87RSFS21WC|06FBSC0EJHAY200E7PXNRGV7XR|06FBSC0MNH0YAWQ4NY2WSC8KJG|06FBSC0TMZBXVVECGQGESWPCY4' --all --max-count=30` shows child `AUTO-INTEGRATION squash into develop` commits `0353d7d50`, `464c307d0`, `57a0f0c94`, `c9404808b`, `177a7f8de`, and `cdb9f223e`.
- `git diff --stat develop...HEAD` only lists `.gicket/tickets/06FBSBZRR9DP7YTR1ZZA3N6ANG/**` changes and no diffs in the named README/docs/src/tests evidence surfaces, matching the contract statement that the parent has no separate implementation artifact beyond evidence aggregation.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Keep any future parent-level acceptance scoped to evidence reconciliation over the named repository surfaces; new implementation work should be opened as a separate ticket, not added back onto this parent.

Implementation watchouts
- Do not treat parent approval as permission to change the runtime default: repository evidence still keeps `AddDVault()` on the compatible `HexString` default unless callers opt into the named binary-first profile.
- Do not imply automatic migration, backfill, dual-write, or public `byte[]` hash-key values; the contract and cited docs/tests consistently keep migration caller-owned and public values on lowercase hexadecimal `string`.

Non-blocking notes
- none

Split recommendations
- No further split is justified; the parent already rolls up six completed child tickets and its remaining scope is closure/tracking reconciliation only.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment