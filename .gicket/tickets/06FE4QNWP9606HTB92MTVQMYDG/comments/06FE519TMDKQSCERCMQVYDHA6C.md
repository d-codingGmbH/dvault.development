[gicket-bot] PO-critic review contract

Summary
- The contract is evidence-backed and has no unresolved open questions, but one current adopter-facing documentation surface is still ambiguous for the v0.42 package/evidence update.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/description.md contains a full delivery contract with 6 acceptance-criteria bullets, 5 definition-of-done bullets, and `## Open Questions` set to `- none`.
- `git log --oneline -n 4` on `ticket/06FE4QNWP9606HTB92MTVQMYDG-story-define-v0-42-provider-evidence-and-tuning` shows the PO handoff commits `f08e24f0a` and `6be8d4d08`, followed by the PO-critic claim `7f593e11c`; `git diff --stat develop..HEAD` touches only `.gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/*`, so this branch is still ticket-refinement-only.
- .gicket/releases/06FE4PMQ8GNKY6X54F8D16AVGC.json is active and says `v0.42.0 - Provider Performance Evidence and Tuning` maps to package versions `8.42.0` and `10.42.0`.
- `benchmark-summary.json` keeps optional-provider rows skipped when `DVAULT_TEST_*` connection strings are unset (context lines 30-56) and preserves `selectedStrategy`/`plannedReadStrategy` guidance with `persistedOutcome=not executed` for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite/PIT/bridge rows (roughly lines <redacted>).
- `docs/performance-profiles.md` and `docs/plans/provider-optimization-gap-matrix.md` already separate completed timing from skipped/diagnostics/smoke posture and record the current PostgreSQL, SQL Server, MySQL, Oracle, and DB2 boundaries the story cites.
- `README.md`, `tools/pack-release-packages.sh`, `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, `docs/package-compatibility.md`, `docs/manual-nuget-publication.md`, `docs/local-validation.md`, `CHANGELOG.md`, and `docs/production-adoption-checklist.md` still reference the current `8.41.0` / `10.41.0` baseline, confirming that the v0.42 package-line/doc move is still downstream work.

Blocking findings
- The coordinated v0.42 documentation/update surface is incomplete: the contract names performance profiles, evidence/gap matrices, release notes/changelog, README, package compatibility, manual publication, and local validation, but it does not say whether `docs/production-adoption-checklist.md` must move too. That file currently carries the same package/evidence baseline (`8.41.0` / `10.41.0` package-line guidance at lines 9-14, performance-evidence guidance at line 98, and v0.41 baseline references at line 123), so downstream work could satisfy the ticket and still leave stale adopter guidance behind.

Required PO actions
- Explicitly include or explicitly exclude `docs/production-adoption-checklist.md` in the v0.42 coordinated documentation scope, then reflect that choice in Acceptance Criteria / Definition of Done so package-line and evidence updates cannot leave that surface stale.
- Clean up the historical-process wording in the contract's Implementation Notes if it is meant to be authoritative: the current note says no ticket-description update was applied in this pass, but `.gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/comments/06FE4YP5D0AW4FSEP0Z2PM8T60.md`, `.gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/comments/06FE4YZKZ2BQH7TTVGBBYNERPR.md`, and `git diff --stat develop..HEAD` all show the description was updated.

Open issues ledger
- critic-item-1 [required-po-action] Explicitly include or explicitly exclude `docs/production-adoption-checklist.md` in the v0.42 coordinated documentation scope, then reflect that choice in Acceptance Criteria / Definition of Done so package-line and evidence updates cannot leave that surface stale.
- critic-item-2 [required-po-action] Clean up the historical-process wording in the contract's Implementation Notes if it is meant to be authoritative: the current note says no ticket-description update was applied in this pass, but `.gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/comments/06FE4YP5D0AW4FSEP0Z2PM8T60.md`, `.gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/comments/06FE4YZKZ2BQH7TTVGBBYNERPR.md`, and `git diff --stat develop..HEAD` all show the description was updated.
- critic-item-3 [blocking-finding] The coordinated v0.42 documentation/update surface is incomplete: the contract names performance profiles, evidence/gap matrices, release notes/changelog, README, package compatibility, manual publication, and local validation, but it does not say whether `docs/production-adoption-checklist.md` must move too. That file currently carries the same package/evidence baseline (`8.41.0` / `10.41.0` package-line guidance at lines 9-14, performance-evidence guidance at line 98, and v0.41 baseline references at line 123), so downstream work could satisfy the ticket and still leave stale adopter guidance behind.

Missing examples / edge cases
- If `docs/production-adoption-checklist.md` stays in scope, define whether it should mirror only the package-line move (`8.42.0` / `10.42.0`) or also the measured-vs-deferred provider-evidence wording used in `docs/performance-profiles.md` and the matrices.
- Clarify how adopter-facing docs should handle superseded v0.32 thresholds after v0.42 evidence lands: keep the historical comparator visible or replace it outright.

Risky assumptions
- The contract currently assumes the named documentation surfaces are exhaustive, but the repository already has another adopter-facing guidance surface (`docs/production-adoption-checklist.md`) that will become stale if it is not updated or intentionally excluded.
- The contract assumes the follow-up question about preserving historical v0.32 thresholds can be deferred without affecting downstream documentation acceptance; that may produce inconsistent release-note vs adopter-guidance wording.

AC / test suggestions
- Add one acceptance criterion or DoD bullet that enumerates every documentation surface expected to move together for the v0.42 package/evidence update, including an explicit decision on `docs/production-adoption-checklist.md`.
- Add a verification suggestion that the final v0.42 doc/update task leaves no unintended `8.41.0`, `10.41.0`, or consumer-facing `0.41.0` guidance in non-historical surfaces after the package-line move.
- Keep the existing evidence rule explicit in downstream acceptance checks: skipped rows with only `selectedStrategy` / `plannedReadStrategy` guidance and `persistedOutcome=not executed` are not promotable to measured timing claims without a provider-configured artifact triplet.

Implementation watchouts
- The root `benchmark-summary.json` intentionally preserves strategy names inside skipped optional-provider rows; downstream tuning/docs work must not treat those rows as completed timings.
- Current repository tooling and adopter docs are still pinned to the v0.41 package lines (`8.41.0` / `10.41.0`), so the eventual v0.42 move spans pack script, package verifier, install guidance, release surfaces, and any other opted-in adopter docs.
- The current branch contains only ticket metadata/history changes, so all repository doc/tool updates still belong to downstream implementation tickets rather than this story branch.

Non-blocking notes
- The live split is already materialized: `.gicket/relations/...` plus related `ticket.json` files show 3 `blocks` tasks (`06FE4QP6FB892E7TJMB47A3MSR`, `06FE4QPEZW97YR6YT7MQD1MXTG`, `06FE4QRC7D55RS8ZZ37ZAEJ98M`) and 7 `relates` tasks covering provider tuning and documentation.
- Historical baseline tickets `06FBSC4QXYQ0SWB1DPMGJJ5XX0`, `06FBSCHBJEYYERDPA7JN34Y8PG`, and `06F8KZTNG44XDPMVTVCV4WJSHG` are all `done`, which matches the contract's claim that they are historical context only.
- No unresolved `## Open Questions` remain in the persisted contract; the remaining questions are tracked under `## Follow-Up Questions`.

Split recommendations
- No additional split is needed beyond the already materialized benchmark-acceptance, provider-tuning, hotspot, and documentation child tickets; the remaining issue is scope clarity on coordinated documentation surfaces.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment