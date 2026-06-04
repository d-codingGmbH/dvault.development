[gicket-bot] PO-critic review contract

Summary
- Child-ticket coverage and landed repository evidence satisfy the epic scope, but the live parent ticket is still persisted as active dev/test work, so it should return to PO for closure-path ticket-field cleanup rather than hand off to dev.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/description.md states the epic is a closure-only roll-up, says 06F8KZMRXRHRKHV56Y96M4S90G, 06F8KZN2BBPB3XFFXEXGX4N4RG, 06F8KZNBGB8FPW6TK5A8SAJMVC, and 06F8KZNNS76TD9Z7ESB173FZ68 collectively satisfy scope, says no residual developer-owned work remains, and shows `## Open Questions` -> `- none`.
- The parent-child relation set is present in `.gicket/relations/TW/0G/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZMRXRHRKHV56Y96M4S90G--parentOf.json`, `.gicket/relations/TW/RG/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZN2BBPB3XFFXEXGX4N4RG--parentOf.json`, `.gicket/relations/TW/VC/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZNBGB8FPW6TK5A8SAJMVC--parentOf.json`, and `.gicket/relations/TW/68/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZNNS76TD9Z7ESB173FZ68--parentOf.json`.
- `git log --oneline --decorate develop --grep '<child ids>' -n 20` returned auto-integration commits `ef35f304c`, `d23b0e481`, `fa1f7a1f1`, and `826b80b9f` on `develop` for those four child tickets.
- `git diff --name-only develop...HEAD -- docs src tests` returned no paths, and `git diff --name-only develop..HEAD` shows only `.gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/**`, so the epic branch carries ticket metadata only and no unlanded product work.
- Repository anchors cited by the contract exist and are populated: `docs/plans/provider-identifier-ddl-guardrail-contract.md` names ticket 06F8KZMRXRHRKHV56Y96M4S90G; `src/DCoding.Data.DVault/DataVaultProviderIdentifierPreflight.cs` emits `DVM2009`; `src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs` defines `DVM2009` and `DVM2010`; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs` asserts `DVM2010`; and `docs/releases/v0.29.0.md`, `docs/production-adoption-checklist.md`, and `docs/model-first-governance.md` all reference the v0.29.0 provider schema guardrail baseline.

Blocking findings
- This epic owns no residual developer slice; the correct next step is closure-path/status cleanup, not a dev handoff. Under the allowed decision enum, that requires `return_to_po` even though child coverage is otherwise sufficient.

Required PO actions
- Move the ticket onto the correct closure/completion path if the workflow cannot represent a closure-only epic on the normal `po-critic -> dev` route.
- Keep any future provider-expansion or physical-naming-override work as new scope on follow-up tickets or epics rather than reopening this parent.

Open issues ledger
- critic-item-1 [required-po-action] Move the ticket onto the correct closure/completion path if the workflow cannot represent a closure-only epic on the normal `po-critic -> dev` route.
- critic-item-2 [required-po-action] Keep any future provider-expansion or physical-naming-override work as new scope on follow-up tickets or epics rather than reopening this parent.
- critic-item-3 [blocking-finding] This epic owns no residual developer slice; the correct next step is closure-path/status cleanup, not a dev handoff. Under the allowed decision enum, that requires `return_to_po` even though child coverage is otherwise sufficient.

Missing examples / edge cases
- none

Risky assumptions
- That the historical child-to-parent `blocks` relation in `.gicket/relations/68/TW/06F8KZNNS76TD9Z7ESB173FZ68--06F8KZM6KFZ3WC5MY5NC12B0TW--blocks.json` is harmless housekeeping and will not reopen parent routing.

AC / test suggestions
- Add a workflow statement that PO-critic approval for closure-only umbrella tickets routes to closure or aggregation handling rather than `dev`.

Implementation watchouts
- Do not reopen parent-owned docs, code, or test work on 06F8KZM6KFZ3WC5MY5NC12B0TW; the landed scope is already covered by the four done children and future work should be new tickets.
- Do not treat the empty `docs/src/tests` diff on this epic branch as a missing-implementation problem; it is consistent with a metadata-only closure branch.

Non-blocking notes
- The current branch tip `269a8a4be` sits above earlier PO and PO-critic handoff commits and only changes `.gicket` metadata; that matches the closure-only audit posture.
- The remaining child-to-parent `blocks` relation from 06F8KZNNS76TD9Z7ESB173FZ68 is ticket-hygiene follow-up, not a scope blocker, per the prompt's audit rule.

Split recommendations
- No new split is needed for the completed epic.
- If provider baselines expand beyond SQLite, Oracle, PostgreSQL, SQL Server, and MySQL, or if a consumer-facing physical naming override API is wanted later, track that as a new epic or follow-up ticket instead of reopening 06F8KZM6KFZ3WC5MY5NC12B0TW.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment