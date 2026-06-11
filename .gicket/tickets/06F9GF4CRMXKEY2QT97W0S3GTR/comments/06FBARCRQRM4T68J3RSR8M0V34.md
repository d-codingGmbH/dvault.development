[gicket-bot] PO-critic review contract

Summary
- Delivery contract is bounded, evidence-backed, and has no open questions; this ticket is ready for developer handoff as a documentation task.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket `06F9GF4CRMXKEY2QT97W0S3GTR` persists `## Open Questions` as `none`, so the contract is not blocked by unresolved PO questions.
- `README.md` already contains a `Govern stable hashes` section stating `AddDVault()` defaults to `sha256-v1`, hashes UTF-8 without BOM, emits lowercase 64-character digests, and points readers to `docs/plans/stable-hashing-contract.md` and `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs`.
- `docs/plans/stable-hashing-contract.md` defines the exact built-in set and caveats: default `sha256-v1`, opt-in `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`, no auto-registration of non-default ids, and caller-owned compatibility work for post-persistence algorithm changes.
- `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs` proves the default `AddDVault()` selection, explicit built-in selection, UTF-8-without-BOM behavior, deterministic vectors, truncated SHA-256 leading-byte behavior, and the exact digest lengths for all four built-in ids.
- `src/DCoding.Data.DVault/BuiltInStableHashService.cs` and `src/DCoding.Data.DVault/StableHashDigest.cs` lock the same four ids and algorithm-aware digest lengths that the ticket asks docs to describe.
- `docs/releases/v0.22.0.md` already carries the stable-hash governance baseline and explicitly says the default algorithm remains `sha256-v1`, so this ticket is a documentation refresh rather than a new algorithm decision.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No quantitative sizing or collision-budget example is included for choosing `sha256-128-v1` versus `sha256-160-v1`; the ticket already treats that as later follow-up, so this is non-blocking.
- No concrete caller example illustrates the operational impact of changing stable-hash algorithm or truncation after persisted hub keys, link keys, or hash diffs already exist; keep this as documentation nuance or later follow-up, not a handoff blocker.

Risky assumptions
- Assumes the completed diagnostics/support-bundle ticket `06F9GF46KZYRKR1EGEPR3TV824` will stay closed and will not reopen with wording changes that materially affect the final v0.35.0 documentation slice.
- Assumes the planned v0.35.0 documentation will keep the existing versioning pattern already visible in `README.md` (`8.34.0` and `10.34.0` today) when it introduces `8.35.0` and `10.35.0` examples.

AC / test suggestions
- Keep an explicit review check that the final docs list all four built-in ids with exact digest shapes: `sha256-v1` 64 hex / 32 bytes, `sha1-v1` 40 / 20, `sha256-128-v1` 32 / 16, and `sha256-160-v1` 40 / 20.
- Keep a wording check that the docs point back to `docs/plans/stable-hashing-contract.md` and `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs` as the proof surfaces for vectors, determinism, explicit selection, and truncated-digest behavior.
- Keep a compatibility review check that the docs do not blur stable-hash guidance with `DataVaultConventions.PersistenceContentHashAlgorithm` / `content_hash` storage semantics described in `docs/plans/stable-hashing-contract.md`.

Implementation watchouts
- `README.md` currently documents the default `sha256-v1` posture; the new documentation must add opt-in guidance without implying that `AddDVault()` auto-registers non-default ids.
- `README.md` still points readers to the current published baseline `docs/releases/v0.34.0.md` and package examples `8.34.0` / `10.34.0`, so the v0.35.0 release-note wording must advance those docs consistently.
- Preserve the current contract boundary that provider packages may optimize transport or batching but must not silently replace the shared stable-hash normalizer and service semantics.

Non-blocking notes
- The ticket is already bounded as documentation-only: scope out explicitly excludes runtime default changes, migration tooling, new crypto/compliance claims, and provider-side hashing behavior.
- The current README baseline is consistent with this being planned next-step documentation work rather than already-landed release work.

Split recommendations
- No split recommended; the persisted contract already narrows this to one evidence-backed documentation slice and the related diagnostics dependency is now completed.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment