[gicket-bot] PO-critic review contract

Summary
- Repository and child-ticket evidence largely support the epic, but it is not ready to pass PO-critic because the persisted incoming blocker relation into the epic still exists and the epic contract makes clearing that live blocker part of closure readiness.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- The live parent graph is present in `.gicket/relations/GR/78/06F5Q93R4633D41Z21WQW3SVGR--06F5Q93YXHSKABD2SABWY85S78--parentOf.json`, `.gicket/relations/GR/B0/06F5Q93R4633D41Z21WQW3SVGR--06F5Q9463M0RSHAJJX0F3D1DB0--parentOf.json`, `.gicket/relations/GR/F0/06F5Q93R4633D41Z21WQW3SVGR--06F5Q94D0JDMMWDXSRGWX1E4F0--parentOf.json`, `.gicket/relations/GR/1W/06F5Q93R4633D41Z21WQW3SVGR--06F5Q94KX65TXQ8EC75FWSD01W--parentOf.json`, and `.gicket/relations/GR/94/06F5Q93R4633D41Z21WQW3SVGR--06F5Q94SQ086B2DZ1AKFDXGV94--parentOf.json`.
- `git show HEAD:.gicket/relations/G4/GR/06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.json` still shows a persisted `blocks` relation into the epic, while `git show HEAD:.gicket/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/ticket.json` shows that source blocker ticket is already `done`.
- The epic contract in `.gicket/tickets/06F5Q93R4633D41Z21WQW3SVGR/description.md` still says `The epic is not closed until all five child tickets are complete and the live relation graph no longer leaves the release blocked` and also requires `The incoming blocker relation ... [to be] cleared before final epic closure`.
- Repository baseline evidence is coherent across `docs/architecture/dvault-v1-activity-tracing-contract.md`, `src/DCoding.Data.DVault/DataVaultActivityTracing.cs`, `docs/performance-profiles.md`, `docs/releases/v0.23.0.md`, `README.md`, `docs/production-adoption-checklist.md`, `benchmark-summary.md`, and `benchmark-summary.json`.
- Branch-history check: `git show --stat --summary 3101b84518ea` shows the latest visible handoff commit touched only `.gicket` metadata/comment files, and `git diff --name-only 715d3e6f2b8929e887fba54a41de375a11a4c7aa..ticket/06F5Q93R4633D41Z21WQW3SVGR-epic-tracing-and-performance-guidance` returned no paths.
- `gicket-read-ticket-comments` returned 10 comments for the epic, and the visible comments are bot claim/lease, PO contract, and runtime handoff records; no human clarification comment introduced new scope.

Blocking findings
- The persisted incoming `blocks` relation from `06F5Q93H60W6X8FJ88PWTR6NG4` to the epic still exists in `.gicket/relations/G4/GR/06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.json`, so the epic does not currently satisfy its own closure condition that the live relation graph no longer leaves the release blocked.

Required PO actions
- Clear or explicitly supersede the persisted `blocks` relation `06F5Q93H60W6X8FJ88PWTR6NG4 -> 06F5Q93R4633D41Z21WQW3SVGR`, then update the epic contract/status so the live relation graph is no longer described as blocking closure.
- If the relation is intentionally retained, document the exact remaining blocking condition and point it to an actually open ticket or follow-up instead of the already-`done` `06F5Q93H60W6X8FJ88PWTR6NG4`.

Open issues ledger
- critic-item-1 [required-po-action] Clear or explicitly supersede the persisted `blocks` relation `06F5Q93H60W6X8FJ88PWTR6NG4 -> 06F5Q93R4633D41Z21WQW3SVGR`, then update the epic contract/status so the live relation graph is no longer described as blocking closure.
- critic-item-2 [required-po-action] If the relation is intentionally retained, document the exact remaining blocking condition and point it to an actually open ticket or follow-up instead of the already-`done` `06F5Q93H60W6X8FJ88PWTR6NG4`.
- critic-item-3 [blocking-finding] The persisted incoming `blocks` relation from `06F5Q93H60W6X8FJ88PWTR6NG4` to the epic still exists in `.gicket/relations/G4/GR/06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.json`, so the epic does not currently satisfy its own closure condition that the live relation graph no longer leaves the release blocked.

Missing examples / edge cases
- The contract does not spell out how closure automation should treat a historical `blocks` relation whose source ticket is already `done`.
- The closure path lacks an explicit example of whether relation cleanup is manual, automated, or replaced by another durable closure-evidence amendment.

Risky assumptions
- Assuming a `done` source ticket automatically neutralizes `.gicket/relations/G4/GR/06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.json` would contradict the epic's own `live relation graph` and `incoming blocker relation ... cleared` wording.

AC / test suggestions
- Add a closure-only acceptance note that epic completion requires both all child tickets `done` and the persisted `.gicket/relations/**` graph to be free of active incoming blockers.

Implementation watchouts
- If the relation file remains, downstream automation can continue to treat the epic as blocked even though all five child tickets are done and the repository evidence is landed.
- Because the latest visible branch commit is only gicket metadata and the diff from the supplied scratch ref is empty, the remaining gap is ticket-state hygiene rather than product or documentation implementation work.

Non-blocking notes
- The five-child decomposition still fits the landed repository surfaces; no new product-scope split is evident from the current branch.
- The v0.23.0 repository story is internally consistent: listener-driven tracing, shared tracing vocabulary, benchmark-backed performance guidance, and coordinated docs all point at the same baseline and artifact set.
- No unresolved `## Open Questions` remain in the epic contract, and the visible comment history does not show a new human scope change.

Split recommendations
- No product-scope split recommended.
- If relation cleanup cannot be performed directly on the epic workflow, create a tiny closure-only follow-up dedicated to relation-graph cleanup and epic closeout.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment