[gicket-bot] PO-critic review contract

Summary
- The previous package-version blocker is explicitly resolved in the persisted contract. The ticket is now a bounded docs-only `v0.39.0` handoff with `## Open Questions` = `none` and clear repository-backed evidence boundaries.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSC4QXYQ0SWB1DPMGJJ5XX0/description.md` now says `PO Handoff` = `ready_for_po_critic`, `## Open Questions` = `none`, and explicitly allows a docs-only `v0.39.0` release note/changelog entry that must not claim `8.39.0`, `10.39.0`, or a consumer-facing `0.39.0` package version.
- `.gicket/tickets/06FBSC4QXYQ0SWB1DPMGJJ5XX0/comments/06FCT8SKY6DG69AF6Z00HTM5H8.md` answers prior critic-item-1/2/3 as `answered` and says the developer should proceed with a docs-only `v0.39.0` note while explicitly avoiding package-line claims.
- `git diff ee9d52278b88d5e1dc2202826f639429344d7855..HEAD -- .gicket/tickets/06FBSC4QXYQ0SWB1DPMGJJ5XX0/description.md` shows the earlier `8.39.0` / `10.39.0` assumption was replaced with explicit docs-only wording and package-line scope-out.
- `git log --oneline --max-count=8` shows the branch at `028ed6ac11f987f86bb678a56ef710a0f7ad6b03`, and `git diff --name-only ee9d52278b88d5e1dc2202826f639429344d7855..HEAD` listed only `.gicket/tickets/06FBSC4QXYQ0SWB1DPMGJJ5XX0/...` files, so the branch changes since the prior block are ticket refinement metadata only; no implementation diff has started yet.
- `CHANGELOG.md` currently starts with `## v0.38.0 - Binary-First New Project Profile`, `docs/releases/v0.39.0.md` is absent, and `docs/performance-profiles.md` still carries `Status: v0.32.0 ...`; those repo facts match the remaining docs-only work named in the contract.
- `docs/performance-profiles.md` already links `docs/plans/provider-optimization-evidence-matrix.md`, while `rg -n "provider-optimization-gap-matrix|Gap Matrix|gap matrix" docs/performance-profiles.md` returned no matches, so the acceptance criterion to add a direct gap-matrix handoff is concrete and bounded.
- `rg -n "v0\.39\.0|8\.39\.0|10\.39\.0|0\.39\.0|8\.38\.0|10\.38\.0"` across `CHANGELOG.md`, the package-guidance docs, and `tools/pack-release-packages.sh` found only `8.38.0` / `10.38.0` in the package/version surfaces, which is consistent with the refined rule that this ticket must not claim new consumer package lines.
- `benchmark-summary.md` shows SQLite completed rows and PostgreSQL/SQL Server/MySQL/Oracle/DB2 optional-provider rows skipped because the `DVAULT_TEST_*_CONNECTION_STRING` variables were unset, and `docs/plans/provider-optimization-evidence-matrix.md` plus `docs/plans/provider-optimization-gap-matrix.md` remain the canonical fact and follow-up sources named by the contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A one-line example of acceptable docs-only `v0.39.0` release-note wording would improve consistency, but the current contract is already specific enough for developer handoff.

Risky assumptions
- Implementation must treat `.gicket/tickets/06FBSC4QXYQ0SWB1DPMGJJ5XX0/description.md` and comment `06FCT8SKY6DG69AF6Z00HTM5H8.md` as authoritative over the stale `8.39.0` / `10.39.0` sentence that still remains in `docs/plans/provider-optimization-evidence-docs-v0.39-refinement.md`.

AC / test suggestions
- Review `docs/performance-profiles.md` for an explicit evidence-vs-follow-up split and for direct links to both `docs/plans/provider-optimization-evidence-matrix.md` and `docs/plans/provider-optimization-gap-matrix.md`.
- Review `docs/releases/v0.39.0.md` and `CHANGELOG.md` for any accidental `8.39.0`, `10.39.0`, or consumer-facing `0.39.0` package-version wording.

Implementation watchouts
- Do not let the new `v0.39.0` docs imply consumer package-version movement; current repo-backed version surfaces still stop at `8.38.0` / `10.38.0`.
- Keep provider claims bounded to the checked-in evidence posture: SQLite timings where completed, skipped-placeholder wording for unset external-provider lanes, and narrower DB2 diagnostics or smoke caveats.
- Do not copy raw benchmark tables into the docs when the contract says to cite matrix row identity and posture semantics instead.

Non-blocking notes
- No split is needed as long as developers keep the work bounded to `docs/performance-profiles.md`, `docs/releases/v0.39.0.md`, and `CHANGELOG.md`.

Split recommendations
- No split recommended.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment