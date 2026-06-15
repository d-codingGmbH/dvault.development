[gicket-bot] PO-critic review contract

Summary
- Delivery contract is refinement-complete and the repository already shows the intended binary-first, HexString-compatibility, and no-automatic-migration story across the key documentation baselines, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket snapshot for `06FBSC0TMZBXVVECGQGESWPCY4` marks `PO Handoff` as `ready_for_po_critic`, lists `## Open Questions` as `none`, and shows `Recent comments` as `<none>`.
- `README.md` Quickstart says new projects should use `AddDVault(options => options.UseBinaryFirstProfile())`, that existing databases and configurations are not migrated automatically, that `HexString`-compatible setups remain valid until an owner-planned reviewed migration/reset/data-move change, and that public hash-key values remain lowercase hexadecimal strings.
- `docs/getting-started.md` repeats the explicit `UseBinaryFirstProfile()` / `UseDataVaultBinaryFirstProfile()` opt-in, says existing `HexString`-compatible setups remain valid until a separate reviewed migration/reset/data-move change, and preserves lowercase-hex public hash-key values.
- `docs/releases/v0.36.0.md` states `HexString` is the compatible default profile, `Binary` is explicit opt-in, and DVault does not automatically rehash, backfill, migrate, repair, reconcile, or dual-write persisted keys.
- `docs/releases/v0.37.0.md` carries forward the same baseline: logical hash-key values stay lowercase hexadecimal strings, `HexString` remains the default compatible physical storage profile, and `Binary` remains explicit opt-in physical storage.
- `docs/plans/hash-key-storage-profile-contract.md` defines `HexString` as the default profile, `Binary` as persistence-only opt-in, and lists automatic repair, rehashing, backfill, and dual-write migration behavior as out of scope.
- `git diff --name-only develop..ticket/06FBSC0TMZBXVVECGQGESWPCY4-task-document-binary-first-adoption-and-migratio -- README.md CHANGELOG.md docs/getting-started.md docs/releases/v0.36.0.md docs/releases/v0.37.0.md docs/production-adoption-checklist.md hash-key-footprint.md docs/plans/hash-key-storage-profile-contract.md` returned no paths, so these cited documentation surfaces already match `develop` on this branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A short explicit contrast example between a brand-new project opting into `UseBinaryFirstProfile()` and an unchanged existing `HexString` deployment would make the recommendation-versus-default boundary harder to misread.
- If any wording mentions future algorithm or storage-profile changes, keep the edge case explicit that matching digest widths do not make a persisted compatibility change safe; the contract already treats those changes as separate reviewed migration work.

Risky assumptions
- The ticket assumes the terse `CHANGELOG.md` v0.36.0 summary is sufficient for the changelog surface even though the fuller owner-planned-migration wording currently appears more explicitly in `README.md`, `docs/getting-started.md`, and `docs/releases/v0.36.0.md`.
- The ticket is being routed as pre-development work even though the cited documentation files already match `develop`; if downstream execution decides this is effectively closure-only, that is an execution-path decision rather than a PO-refinement gap.

AC / test suggestions
- Use a closeout checklist that each public surface states the same three points: binary-first is recommended for new projects, existing `HexString` stores remain valid until an owner-planned reviewed migration/reset/data move, and DVault does not auto-rehash/backfill/dual-write/repair/migrate persisted keys.
- When the ticket is closed, compare `README.md`, `docs/getting-started.md`, `CHANGELOG.md`, `docs/releases/v0.36.0.md`, and `docs/releases/v0.37.0.md` side by side to catch one surface using softer wording than the others.

Implementation watchouts
- `CHANGELOG.md` currently summarizes v0.36.0 more tersely than `README.md`, `docs/getting-started.md`, and `docs/releases/v0.36.0.md`; if the acceptance criteria require equally explicit language on every public surface, that is the likeliest drift point.
- Keep `Binary` described as physical storage only; the current repository contract in `README.md`, `docs/getting-started.md`, `docs/plans/hash-key-storage-profile-contract.md`, and `docs/releases/v0.37.0.md` consistently preserves lowercase-hex public values and should not be weakened.

Non-blocking notes
- The persisted contract is internally consistent: scope, acceptance criteria, definition of done, and risks all align around one documentation-alignment task with no unresolved PO questions.
- Current branch history appears to be ticket-routing only; there is no observed diff versus `develop` across the named documentation surfaces yet, which is a delivery watchout but not a PO blocker.

Split recommendations
- No split recommended; the current delivery contract already bounds this as one documentation-alignment task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment