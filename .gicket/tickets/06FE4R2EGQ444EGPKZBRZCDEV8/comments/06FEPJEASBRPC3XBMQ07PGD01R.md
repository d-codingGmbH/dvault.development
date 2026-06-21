[gicket-bot] PO-critic review contract

Summary
- Closure-only approval is not supported: related evidence tickets are done, but this ticket still requires unreleased docs work and the branch only contains ticket metadata changes.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FE4R2EGQ444EGPKZBRZCDEV8/description.md` says the ticket must add `docs/releases/v0.43.0.md`, add a `CHANGELOG.md` v0.43.0 entry, and update current-baseline docs; its `## Open Questions` section is `- none`.
- The PO refinement comment `.gicket/tickets/06FE4R2EGQ444EGPKZBRZCDEV8/comments/06FEPGHXVA638CPY2WTCD4Q1Q4.md` explicitly says current docs still advertise the v0.42.0 baseline and `docs/releases/v0.43.0.md` does not yet exist.
- `git diff --name-only develop...HEAD` lists only `.gicket/tickets/06FE4R2EGQ444EGPKZBRZCDEV8/...` metadata/comment files; no `README.md`, `CHANGELOG.md`, `docs/...`, or `src/DCoding.Data.DVault.Analyzers/README.md` changes are on the branch.
- `test -f docs/releases/v0.43.0.md` returned `missing`.
- `CHANGELOG.md` currently begins with `## v0.42.0 - Provider Performance Evidence and Tuning`; there is no v0.43.0 section.
- `README.md`, `docs/production-adoption-checklist.md`, `docs/performance-profiles.md`, and `src/DCoding.Data.DVault.Analyzers/README.md` still describe the current public baseline as v0.42.0 / `8.42.0` / `10.42.0`.

Blocking findings
- This review was routed as closure-only, but the persisted contract still requires new release-note/changelog/current-baseline documentation updates. That is remaining implementation work, not landed closure evidence.
- There is no branch implementation for the required docs work. Relative to `develop`, only `.gicket` ticket metadata changed.
- The contract's required deliverables are still absent in the repository: `docs/releases/v0.43.0.md` is missing and `CHANGELOG.md` has no v0.43.0 entry.

Required PO actions
- Fix the ticket routing. Either convert this back to a normal pre-development docs task for `dev`, or rewrite it into a true closure-only/no-work-required ticket backed by repository evidence that already satisfies the deliverables.
- If the intended path is dev handoff, keep the refined contract but remove the closure-only assumption from the workflow context.
- If the intended path is closure-only, replace the current scope/acceptance/DoD language that requires adding docs with auditable landed paths showing those exact updates already exist.

Open issues ledger
- critic-item-1 [required-po-action] Fix the ticket routing. Either convert this back to a normal pre-development docs task for `dev`, or rewrite it into a true closure-only/no-work-required ticket backed by repository evidence that already satisfies the deliverables.
- critic-item-2 [required-po-action] If the intended path is dev handoff, keep the refined contract but remove the closure-only assumption from the workflow context.
- critic-item-3 [required-po-action] If the intended path is closure-only, replace the current scope/acceptance/DoD language that requires adding docs with auditable landed paths showing those exact updates already exist.
- critic-item-4 [blocking-finding] This review was routed as closure-only, but the persisted contract still requires new release-note/changelog/current-baseline documentation updates. That is remaining implementation work, not landed closure evidence.
- critic-item-5 [blocking-finding] There is no branch implementation for the required docs work. Relative to `develop`, only `.gicket` ticket metadata changed.
- critic-item-6 [blocking-finding] The contract's required deliverables are still absent in the repository: `docs/releases/v0.43.0.md` is missing and `CHANGELOG.md` has no v0.43.0 entry.

Missing examples / edge cases
- none

Risky assumptions
- Assuming the upstream benchmark/migration/allocation tickets being `done` is enough to close this downstream docs-consolidation ticket without actually updating the release notes and current-baseline docs.
- Assuming the PO handoff state `ready_for_po_critic` converts remaining documentation work into closure evidence.

AC / test suggestions
- Add an explicit routing note that this is a normal dev ticket unless the owner branch already contains `docs/releases/v0.43.0.md`, a `CHANGELOG.md` v0.43.0 section, and the listed current-baseline doc updates.
- If closure-only is ever intended for docs consolidation, require acceptance criteria to cite the exact landed doc paths and branch diff expected to be present.

Implementation watchouts
- When this returns to normal dev routing, the touched README/release/performance/analyzer/adoption surfaces need one coherent v0.43 baseline; mixed v0.42/v0.43 guidance will confuse consumers.
- Binary-storage guidance must keep skipped/failed provider rows as caveats only and keep post-persistence migration caller-owned.

Non-blocking notes
- none

Split recommendations
- No split is needed if this is routed back to a normal dev docs task.
- If PO wants a separate closure-only record of already-landed upstream evidence, split that from the actual v0.43 documentation implementation ticket.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment