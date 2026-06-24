[gicket-bot] PO-critic review contract

Summary
- Ticket 06FF43JEA6C3HNJ6AQA9XY7EC8 is ready for developer handoff: the delivery contract is documentation-scoped, has no open questions, and ties the v0.47 update to concrete repository baselines and canonical evidence-boundary sources.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket 06FF43JEA6C3HNJ6AQA9XY7EC8 keeps `## Open Questions` as `none` and its acceptance criteria explicitly call for `docs/releases/v0.47.0.md`, a new `CHANGELOG.md` v0.47.0 entry, updates to `docs/performance-profiles.md`, updates to `docs/plans/provider-optimization-evidence-matrix.md`, and version alignment to consumer package lines `8.47.0` and `10.47.0`.
- Direct repository listing of `docs/releases` shows release-note files through `docs/releases/v0.46.0.md` and no `docs/releases/v0.47.0.md`, matching the ticket's stated pre-development baseline.
- Direct repository reads show `CHANGELOG.md` currently starts with `## v0.46.0 - Reserved Interactive Provider Optimization`, `docs/performance-profiles.md` currently declares `Status: v0.46.0 provider optimization closure baseline...`, and `docs/releases/v0.46.0.md` states that the visible release label maps to package lines `8.46.0` and `10.46.0`, not `0.46.0`.
- Direct read of `docs/plans/provider-optimization-evidence-matrix.md` preserves the canonical evidence postures (`completed-timing`, `skipped-placeholder`, `diagnostics-only`, `smoke-only`, `storage-footprint`) and explicitly separates PIT maintenance timing evidence from `pit-as-of-read` and `bridge-traversal-read` rows.
- Ticket comments contain the PO refinement contract and no newer human clarification thread; the refinement comment repeats the bounded v0.47 documentation scope and confirms no additional child-ticket or planning-artifact work was added during refinement.
- Interactive session branch context targets `ticket/06FF43JEA6C3HNJ6AQA9XY7EC8-task-update-v0-47-provider-maintenance-release-d` with scratch source ref `303828c5093589614b7f270f25a1806ebc9a6548` and a clean branch snapshot, so the ticket is being reviewed against the current pre-dev branch baseline rather than stale cached evidence.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The incoming release-coordination blockers referenced in the contract will not reopen or materially change the allowed v0.47 maintenance wording during implementation.
- The v0.46.0 release-note documentation list is sufficient as the bounded package-guidance sweep for v0.47, so no broader adopter-doc rewrite is required inside this ticket.

AC / test suggestions
- Verify every touched release-line/package-version reference uses `v0.47.0` with consumer package lines `8.47.0` and `10.47.0`, never a consumer-facing `0.47.0` package version.
- Review updated release-note and changelog prose against `docs/performance-profiles.md` and `docs/plans/provider-optimization-evidence-matrix.md` so maintenance wording does not drift into unsupported benchmark-win claims.

Implementation watchouts
- Use `docs/releases/v0.46.0.md` as the structural template, but do not copy its completed benchmark claims forward as new v0.47 maintenance-timing claims.
- Treat `docs/performance-profiles.md` and `docs/plans/provider-optimization-evidence-matrix.md` as the canonical claim-boundary sources for what can be described as completed timing evidence versus source/test-backed maintenance behavior.

Non-blocking notes
- The current repository baseline is clearly pre-development for this task: `docs/releases/v0.47.0.md` is absent and the major documentation surfaces still advertise v0.46.0.
- This is correctly scoped as a documentation-only handoff; missing implementation artifacts on the branch are expected at PO-critic time and are not a PO blocker.

Split recommendations
- If the package-guidance sweep expands beyond release-line alignment and evidence-boundary consistency into broader README or analyzer-guidance rewrites, split that broader docs cleanup into a separate ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment