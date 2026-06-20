[gicket-bot] PO-critic review contract

Summary
- The persisted delivery contract now matches direct repository and ticket evidence, has `## Open Questions` set to `- none`, and gives downstream developers clear v0.42 evidence, threshold, fallback, documentation, and package-line gates.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/description.md contains `PO Handoff` decision `ready_for_po_critic` and `## Open Questions` -> `- none`, so the persisted contract has no unresolved open-question blocker.
- Latest PO refinement comment `.gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/comments/06FE58FS701XKK3HNQTXGF15TG.md` explicitly answers the prior relation-state blocker and states the live split as `3` active `blocks`, `10` active `relates`, and `0` active `parentOf` files.
- `find .gicket/relations -type f | grep 06FE4QNWP9606HTB92MTVQMYDG | wc -l` returned `13`; the corresponding active relation-file counts for `--blocks.json`, `--relates.json`, and `--parentOf.json` were `3`, `10`, and `0`. Examples include `.gicket/relations/DG/SR/06FE4QNWP9606HTB92MTVQMYDG--06FE4QP6FB892E7TJMB47A3MSR--blocks.json` and `.gicket/relations/DG/TG/06FE4QNWP9606HTB92MTVQMYDG--06FE4QPEZW97YR6YT7MQD1MXTG--relates.json`.
- `rg -l TicketRelationRemoved .gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/events | wc -l` returned `10`, matching the contract's historical `parentOf` removal count.
- .gicket/releases/06FE4PMQ8GNKY6X54F8D16AVGC.json names release `v0.42.0 - Provider Performance Evidence and Tuning` and says it maps to package versions `8.42.0` and `10.42.0`.
- `test -f docs/releases/v0.42.0.md; echo $?` returned `1`, so the release-note file is still absent and clearly remains downstream implementation work, which is consistent with the story's implementation note and not a pre-development PO blocker.
- `rg -n` across `README.md`, `CHANGELOG.md`, `docs/package-compatibility.md`, `docs/manual-nuget-publication.md`, `docs/local-validation.md`, `docs/production-adoption-checklist.md`, `tools/pack-release-packages.sh`, and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` shows the current repository is still on the `8.41.0` / `10.41.0` baseline, which matches the contract's statement that the coordinated `8.42.0` / `10.42.0` move is downstream work.
- `benchmark-summary.json` keeps PostgreSQL, SQL Server, MySQL, Oracle, and DB2 lanes behind `DVAULT_TEST_*` gates as `executionStatus=skipped` with `persistedOutcome=not executed`, while `docs/plans/provider-optimization-evidence-matrix.md` and `docs/plans/provider-optimization-gap-matrix.md` already define the completed-timing vs skipped-placeholder/diagnostics-only/smoke-only boundaries and the current threshold/fallback posture this story is ratifying.
- `git diff --name-only 3f9468f01..HEAD` lists only `.gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG` artifacts, so this branch remains refinement-only rather than mixing implementation work into pre-development review.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- If later v0.42 evidence materially changes a threshold, downstream docs should explicitly state how much superseded v0.32 comparator detail remains adopter-facing versus moving to release history only.
- If a configured external-provider lane produces only diagnostics-only or smoke-only evidence, downstream docs should show that posture explicitly instead of implying a completed timing promotion.

Risky assumptions
- Later tooling will treat the current `blocks` + `relates` graph as authoritative until a separate relation-normalization ticket intentionally changes the live relation model.

AC / test suggestions
- When downstream work closes, verify that every coordinated v0.42 surface uses `8.42.0` or `10.42.0` and that no non-historical `8.41.0` or `10.41.0` install guidance remains in README, release notes, CHANGELOG, package compatibility, manual publication, local validation, or production adoption docs.
- Require promoted timing claims to cite the exact matrix row identity `scenario`, `provider`, `baseline`, and `posture` plus the matching benchmark artifact triplet, especially for external-provider latest-satellite, PIT, and bridge rows.

Implementation watchouts
- `docs/releases/v0.42.0.md` does not exist yet; downstream work must add it instead of assuming release-note alignment is already landed.
- Current repository tooling and adopter-facing docs are still on `8.41.0` / `10.41.0`; the package-line move has to update pack, verify, install, validation, README, changelog, and adopter guidance together.
- Root `benchmark-summary.json` preserves provider strategy names even when external-provider rows are skipped; those rows remain non-timing evidence until a provider-configured artifact triplet completes them.
- The live split model is active `blocks` plus `relates`, not active `parentOf`; downstream workflow or closure automation should not infer hierarchy semantics unless a later normalization ticket changes the graph.

Non-blocking notes
- Latest PO refinement comment `06FE58FS701XKK3HNQTXGF15TG.md` explicitly supersedes the earlier incorrect parentOf wording seen in prior ticket commentary.
- Previous PO-critic blockers about documentation scope and relation-state wording are resolved in the persisted description and latest PO refinement comment.

Split recommendations
- No additional split recommended; the story already has `3` active `blocks` relations and `10` active `relates` relations to downstream tickets, and the contract now states that live model explicitly.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment