[gicket-bot] PO-critic review contract

Summary
- The ticket is now coherent as a closure-only/no-work story: the branch is metadata-only against `develop`, `develop` already contains the provider-native strategy and bulk-test surfaces, and the latest PO refinement resolved the earlier PO-critic blockers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- On branch `ticket/06F2PGNGVQ3TZZWSABAK5SNFK4-story-add-provider-native-bulk-ingestion-strateg`, `git rev-parse HEAD` returned `25c5b930683556962ac99cb473110e1f6ed1ef33` and `git rev-parse develop` returned `b95ad09f91694f638b51911850d687c6765a195e`.
- `git diff --stat develop...HEAD -- src tests docs README.md` returned no output, and `git diff --name-status develop...HEAD` listed only `.gicket/tickets/06F2PGNGVQ3TZZWSABAK5SNFK4/{description.md,ticket.json,comments/*,events/*}`.
- `.gicket/tickets/06F2PGNGVQ3TZZWSABAK5SNFK4/description.md` now states the story should be treated as `closure-only/no-work`, and its `## Open Questions` section is `none`.
- Ticket comments show the prior blocker was explicitly answered: `.gicket/tickets/06F2PGNGVQ3TZZWSABAK5SNFK4/comments/06F3P4QE1PRYT93BWTZZH889TM.md` is the earlier PO-critic return-to-PO contract, and `.gicket/tickets/06F2PGNGVQ3TZZWSABAK5SNFK4/comments/06F3P7K5B69RW4QX86NCTGFSHC.md` answers `critic-item-1` through `critic-item-4` by reclassifying the story to closure-only/no-work.
- `git show --name-only --format='%h %s' b95ad09f91694f638b51911850d687c6765a195e` shows `develop` already integrated `[06F2PGNT7DF4DVNKYWDFZC8DEM] AUTO-INTEGRATION squash into develop` with `README.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, `src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs`, `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs`, and the provider bulk integration test files.
- `git grep` on `develop` directly shows the already-landed provider-native surfaces: `AddDVaultPostgres`, `AddDVaultSqlServer`, `AddDVaultMySql`, and `AddDVaultOracle` in provider service-collection extensions; `EvaluatePostgres`, `EvaluateSqlServer`, `EvaluateMySql`, and `EvaluateOracle` in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs`; and bulk coverage in `tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderBulkSaveAssertions.cs` plus the Postgres/SQL Server/MySQL/Oracle bulk test methods.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Approval assumes downstream automation interprets this as a closure-only/no-work story and does not expect a fresh implementation delta from this branch, because the branch carries no `src/`, `tests/`, `docs/`, or `README.md` changes relative to `develop`.
- Approval assumes `06F2PGNT7DF4DVNKYWDFZC8DEM` remains acceptable as the last visible `develop` integration evidence for the already-landed provider bulk proof referenced by this story.

AC / test suggestions
- If the workflow needs a stronger machine-readable closure cue, add an acceptance item or label convention stating that this story carries no remaining dev code delta and exists only to reconcile ownership/history.
- Keep downstream benchmark `06F2PGNZBRNCQ1SV2KKP6F3BA8` and docs `06F2PGP2B2RZGGK3CVKK5WRRP8` tied to the already-landed `develop` baseline rather than this metadata-only branch.

Implementation watchouts
- Do not reopen provider strategy or provider bulk test files from this ticket branch; `develop...HEAD` is metadata-only for `src/`, `tests/`, `docs/`, and `README.md`.
- `develop:docs/architecture/dvault-v1-explicit-save-service.md` says Oracle optimized coverage includes eligible ordinary satellite batches, while `develop:docs/releases/v0.5.0.md` still says Oracle is hub/link-only; keep that divergence isolated to downstream docs follow-up `06F2PGP2B2RZGGK3CVKK5WRRP8`.

Non-blocking notes
- The contract's implementation note cites historical branch commit `116dd999cc5a61b186d8f34e19c12f739d975dfe`, but current branch HEAD is `25c5b930683556962ac99cb473110e1f6ed1ef33`; the later commits are still ticket-metadata-only.

Split recommendations
- No additional split is needed for this ticket's current state; keep it as closure-only/no-work with benchmark and docs follow-ons remaining separate.
- If future provider-native bulk work appears after this review, open a new ticket against the concrete missing delta instead of reopening this historical closure story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment