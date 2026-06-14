[gicket-bot] PO-critic review contract

Summary
- The earlier PO blocker was addressed and the current contract is now source-backed, bounded, and free of open questions; this story is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSBZY1XEJYK1DRV4RV2ZN88/description.md now states `## Open Questions` as `none` and its Acceptance Criteria/Definition of Done explicitly require an additive named binary-first profile API, unchanged default `sha256-v1` + `HexString` behavior, and focused tests.
- `git diff <redacted>..93090ba04 -- .gicket/tickets/06FBSBZY1XEJYK1DRV4RV2ZN88/description.md .gicket/tickets/06FBSBZY1XEJYK1DRV4RV2ZN88/ticket.json` shows the PO refinement replaced the earlier inferred-existing-API wording with source-backed text that explicitly says no named high-level binary-first selector is visible yet and that this story adds one.
- Previous blocker evidence is resolved in ticket comments: .gicket/tickets/06FBSBZY1XEJYK1DRV4RV2ZN88/comments/06FCC3FR3AX2KP250G1NXBH0X8.md returned the ticket to PO for inferring an unsupported existing API/type, and .gicket/tickets/06FBSBZY1XEJYK1DRV4RV2ZN88/comments/06FCCTR1ZW0F5R5S7NRGM05M6W.md answers critic-item-1/2/3 by restating the work as additive API creation.
- Repository source confirms the current high-level setup families but no named binary-first selector: src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs exposes `AddDVault(...)`, src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs exposes `UseDataVault(...)` and `ApplyDataVaultMetadata(...)`, and src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs exposes `UseDataVaultMetadata(...)`; repo search for `BinaryFirst|UseBinary|WithBinary|UseHashKeyStorage` found only the low-level `WithHashKeyStorageProfile(...)` path.
- Repository source also confirms the baseline the contract depends on: src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs and src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs default to `DataVaultHashKeyStorageProfile.HexString`, tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs asserts `AddDVault()` keeps `sha256-v1` + `HexString`, and src/DCoding.Data.DVault/DataVaultProviderCapabilityProfile.cs plus tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs / DataVaultEfMetadataTranslationTests.cs show low-level `Binary` physical storage already exists while the model boundary stays `string`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The ticket intentionally leaves the canonical quickstart example path unresolved until documentation sibling `06FBSC0EJHAY200E7PXNRGV7XR`; that is noted in Follow-Up Questions and is not a PO blocker for development handoff.

Risky assumptions
- The final public member/type name for the named binary-first selector is still an implementation choice; developers must keep it additive and conventions-owned rather than inventing a separate provider-only lane.
- The contract expects later diagnostics/supportability work to distinguish named binary-first selection from manual explicit binary shaping, but the exact user-facing label text is deferred to sibling ticket `06FBSC08W24BJGFZ87RSFS21WC`.

AC / test suggestions
- Keep one explicit proof that app-level `AddDVault(...)`, registry-backed `UseDataVaultMetadata(...)`, and direct `ApplyDataVaultMetadata(...)` all carry the same selected profile through the shared conventions path.
- Keep one negative/default proof that optionless `AddDVault()` and non-opted-in `UseDataVault()` still resolve to `sha256-v1` + `HexString`.

Implementation watchouts
- Do not satisfy the story by exposing only raw `Binary` storage metadata; the contract explicitly calls out stable named-profile identity as needed for model-cache and later diagnostics flows.
- Do not add the selector on only one public setup family; the refined scope says the shared conventions path must cover app defaults, `UseDataVaultMetadata()` projection, and direct model projection consistently.

Non-blocking notes
- `git log --oneline -n 12` on branch `ticket/06FBSBZY1XEJYK1DRV4RV2ZN88-story-add-explicit-binary-first-profile-api` shows only workflow claim/handoff commits after the PO refinement work; no product diff is expected yet at this pre-development gate.
- Sibling tickets `06FBSC03KAGDABNFGPK9D95QKR`, `06FBSC08W24BJGFZ87RSFS21WC`, `06FBSC0EJHAY200E7PXNRGV7XR`, `06FBSC0MNH0YAWQ4NY2WSC8KJG`, and `06FBSC0TMZBXVVECGQGESWPCY4` remain separate downstream work and do not block this story's developer handoff.

Split recommendations
- No further split recommended; the current diagnostics, compatibility, documentation, and benchmark follow-up work is already separated into sibling tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment