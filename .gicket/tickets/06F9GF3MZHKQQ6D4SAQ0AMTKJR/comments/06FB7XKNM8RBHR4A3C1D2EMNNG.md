[gicket-bot] PO-critic review contract

Summary
- Delivery contract is clear and bounded around widening the stable-hash digest contract beyond fixed 64-character SHA-256 while preserving the current `sha256-v1` baseline; no persisted open questions remain, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git diff --name-only develop..HEAD` lists only `.gicket/tickets/06F9GF3MZHKQQ6D4SAQ0AMTKJR/**`, so the story branch currently contains ticket metadata refinement only and no source/doc/test implementation yet.
- `src/DCoding.Data.DVault/StableHashDigest.cs` still throws unless `value.Length == 64` and all characters are lowercase hex, which directly matches the ticket's stated fixed-shape problem.
- `src/DCoding.Data.DVault/DefaultStableHashService.cs`, `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs`, and `src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs` all pin the current default stable-hash baseline to `sha256-v1`, while `DataVaultConventions.cs` separately keeps `DefaultPersistenceContentHashAlgorithm = "sha-256"`.
- `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` currently exposes `StableHashDigest(string algorithmId, string value)` and `IStableHashService.ComputeHash(string normalizedInput)`, so the public contract surface called out in the ticket is directly present in the repo.
- `.gicket/tickets/06F9GF3MZHKQQ6D4SAQ0AMTKJR/comments/06FB7SKB2GFRCPCNEA0NZKW16C.md` repeats `Open questions - none` and marks the ticket `ready_for_po_critic`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The story does not pre-name a concrete shorter digest example in the contract text; developers will still need to choose and document at least one algorithm-specific shorter lowercase-hex shape or equivalent test double to satisfy the Definition of Done.

Risky assumptions
- Approval assumes the truncated SHA-256 candidate ids and sizes can be finalized during implementation/documentation without a separate PO decision, because the only explicit sizing question is deferred under `## Follow-Up Questions` rather than `## Open Questions`.
- Approval assumes the existing child task `06F9GF3TRG65G8MTMG7DH4PREC` remains coordination/history context and not a competing implementation authority for the same scope.
- Approval assumes developers will preserve the split between stable model/key hashing and the fixed persistence `content_hash` contract (`sha-256`, 64-character lowercase hex).

AC / test suggestions
- Add one algorithm-aware regression that accepts a valid non-`sha256-v1` lowercase-hex digest with a shorter length while still rejecting invalid hex and empty/whitespace algorithm ids.
- Keep the published `sha256-v1` vectors unchanged and add regression coverage that `AddDVault()` still resolves `IStableHashService.AlgorithmId == "sha256-v1"` by default.
- If digest-byte access is added, add round-trip and immutability coverage proving the byte view cannot diverge from the canonical hex value.
- Add explicit documentation/test coverage that `DataVaultConventions.PersistenceContentHashAlgorithm` and the separate `content_hash` policy remain fixed and are not widened by this story.

Implementation watchouts
- Any change to `StableHashDigest` or related members will affect the public API snapshot; keep `IStableHashService.ComputeHash(string normalizedInput)` intact unless a broader API change is unavoidable.
- Do not let alternate algorithm ids masquerade as `sha256-v1` or imply that `AddDVault()` auto-enables SHA-1 or truncated variants by default.
- When removing the fixed 64-character assumption, keep canonical lowercase-hex validation intact and make length validation explicitly algorithm-aware rather than unbounded.

Non-blocking notes
- Current repository docs still describe the default stable-hash implementation as 64-character lowercase SHA-256 in `docs/plans/stable-hashing-contract.md`, which is consistent with this being a pre-development contract-widening story rather than completed implementation.
- The story branch history (`git log --oneline --max-count=12 -- ...`) is dominated by PO/PO-critic ticket-management commits (`498e50476`, `3d3538cf3`, `bf87d6bcf`) rather than repository delivery commits, so dev will be starting from a metadata-only handoff.
- The ticket itself is not blocked (`ticket.json: is-blocked false`) even though the story still has a `blocks` relation to child task `06F9GF3TRG65G8MTMG7DH4PREC`.

Split recommendations
- If implementation expands beyond widening the digest contract and preserving `sha256-v1` compatibility, keep built-in SHA-1 or truncated-SHA-256 runtime registrations and their full compatibility-vector coverage in a follow-up ticket, matching the story's own split recommendation.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment