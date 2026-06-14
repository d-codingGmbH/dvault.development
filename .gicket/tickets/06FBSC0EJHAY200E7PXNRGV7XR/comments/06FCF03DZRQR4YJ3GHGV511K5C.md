[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the ticket is now clearly framed as normal implementation work, the scope and acceptance criteria are concrete, the named APIs are verified in source, and the contract has no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSC0EJHAY200E7PXNRGV7XR/description.md:17-45` defines the exact delivery surfaces and API split, and `description.md:47-48` records `## Open Questions` as `none`.
- `.gicket/tickets/06FBSC0EJHAY200E7PXNRGV7XR/comments/06FCEY5P0Z09HC3H8H8VWZTDQG.md:10-16` explicitly marks prior PO-critic items 1-6 as answered and reaffirms this is normal implementation work, not closure-only routing.
- `src/DCoding.Data.DVault/DataVaultOptions.cs:83-91` defines `UseBinaryFirstProfile()`, and `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:22-64` defines `UseDataVaultBinaryFirstProfile(...)` for the code-first path.
- `README.md:60-76`, `docs/getting-started.md:15-25`, `examples/README.md:57-68`, `examples/DCoding.Data.DVault.SqliteQuickstart/Program.cs:11-17`, and `examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs:16-22` still show the current default-path setup the ticket is meant to update, so the requested work is concrete and locally verifiable.
- `docs/getting-started.md:66-70` and `docs/releases/v0.37.0.md:93-95` already anchor the compatibility baseline: public hash-key values stay lowercase hexadecimal strings, `HexString` remains the compatible default, and `Binary` is explicit opt-in.
- `git -C /mnt/c/Projects/DVault log --oneline --max-count=6` shows HEAD `7c8b4efdc` on `ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi`, and `git show --name-only HEAD` lists only `.gicket/...` files; branch history matches a pre-development handoff rather than a closure-ready implementation branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Assuming the compatibility caveat can be phrased loosely; the contract requires it to stay visible in the primary quickstart path and consistent across the named surfaces.
- Assuming the two binary-first APIs are interchangeable; the contract expects `UseBinaryFirstProfile()` for registry-backed `AddDVault(...)` quickstarts and `UseDataVaultBinaryFirstProfile()` for direct `ModelBuilder` guidance.

AC / test suggestions
- In dev/test review, check each scoped surface for either the binary-first recommendation or explicit existing-project compatibility framing, and ensure no snippet still presents default-only `AddDVault()` as the recommended new-project path.
- Verify the final wording preserves the storage contract from `docs/getting-started.md:66-70` and `docs/releases/v0.37.0.md:95`: logical/public hash keys remain lowercase hexadecimal strings even when binary physical storage is recommended.

Implementation watchouts
- Do not imply automatic migration, data backfill, provider DDL changes, or seamless switching for existing HexString-backed databases.
- Use the shipped named APIs instead of low-level provider capability composition in the quickstart guidance.
- Keep `README.md`, `docs/getting-started.md`, `examples/README.md`, and both runnable quickstart programs aligned; the contract explicitly treats mixed guidance as a risk.

Non-blocking notes
- The scoped repo files are intentionally still unmodified on this branch; for this pre-development gate that is expected and not a reason to return the ticket to PO.

Split recommendations
- No split recommended. The enabling API story `06FBSBZY1XEJYK1DRV4RV2ZN88` is already done, and sibling ticket `06FBSC0TMZBXVVECGQGESWPCY4` already owns broader adoption/migration documentation outside the quickstart path.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment