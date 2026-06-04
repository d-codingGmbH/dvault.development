[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/description.md` has `## Open Questions` = `none`, but its `## Follow-Up Questions` still asks whether the epic should be materialized into child implementation tickets and its `## Split Recommendations` still lists four child-ticket splits.
- The epic already has four live child relations in `.gicket/relations/TW/0G/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZMRXRHRKHV56Y96M4S90G--parentOf.json`, `.gicket/relations/TW/68/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZNNS76TD9Z7ESB173FZ68--parentOf.json`, `.gicket/relations/TW/RG/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZN2BBPB3XFFXEXGX4N4RG--parentOf.json`, and `.gicket/relations/TW/VC/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZNBGB8FPW6TK5A8SAJMVC--parentOf.json`.
- Those four child tickets are persisted as done in `.gicket/tickets/<id>/ticket.json`: 06F8KZMRXRHRKHV56Y96M4S90G (`Story: Define provider identifier and DDL guardrail contract`), 06F8KZN2BBPB3XFFXEXGX4N4RG (`Story: Add provider identifier preflight checks`), 06F8KZNBGB8FPW6TK5A8SAJMVC (`Story: Strengthen provider-specific migration guardrails`), and 06F8KZNNS76TD9Z7ESB173FZ68 (`Task: Update v0.29.0 provider schema guardrail documentation`).
- `git log --oneline --decorate develop -n 20` shows auto-integration commits on `develop` for those child tickets: `ef35f304c` (06F8KZMRXRHRKHV56Y96M4S90G), `d23b0e481` (06F8KZN2BBPB3XFFXEXGX4N4RG), `fa1f7a1f1` (06F8KZNBGB8FPW6TK5A8SAJMVC), and `826b80b9f` (06F8KZNNS76TD9Z7ESB173FZ68).
- `git diff --name-only develop..HEAD` lists only `.gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/**`, and `git log --graph -n 12` shows HEAD `3b9190179` is the ticket-claim branch tip for `ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails` rather than a new implementation branch tip.

Blocking findings
- The epic does not state what developer-owned work remains after those four child tickets landed on `develop`; sending this ticket to `dev` now would be ambiguous and risks duplicate or no-op execution.

Required PO actions
- Decide whether 06F8KZM6KFZ3WC5MY5NC12B0TW is now a closure-only/roll-up ticket over 06F8KZMRXRHRKHV56Y96M4S90G, 06F8KZN2BBPB3XFFXEXGX4N4RG, 06F8KZNBGB8FPW6TK5A8SAJMVC, and 06F8KZNNS76TD9Z7ESB173FZ68, or whether there is specific residual scope that still needs developer work.

Open issues ledger
- critic-item-1 [required-po-action] Decide whether 06F8KZM6KFZ3WC5MY5NC12B0TW is now a closure-only/roll-up ticket over 06F8KZMRXRHRKHV56Y96M4S90G, 06F8KZN2BBPB3XFFXEXGX4N4RG, 06F8KZNBGB8FPW6TK5A8SAJMVC, and 06F8KZNNS76TD9Z7ESB173FZ68, or whether there is specific residual scope that still needs developer work.
- critic-item-2 [blocking-finding] The epic does not state what developer-owned work remains after those four child tickets landed on `develop`; sending this ticket to `dev` now would be ambiguous and risks duplicate or no-op execution.

Missing examples / edge cases
- If the epic is meant to stay open after all listed child tickets are done, add one explicit example of what residual epic-level work or closure evidence remains.

Risky assumptions
- That the four done child tickets fully satisfy the epic even though the epic never explicitly states whether any cross-ticket integration or closure task remains.

AC / test suggestions
- Add an epic-level acceptance item stating whether completion of the four existing child tickets is sufficient to finish the epic, or list any residual deliverable that is not already owned by one of those child tickets.

Implementation watchouts
- none

Non-blocking notes
- The refined contract is otherwise concrete: `.gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/description.md` has `Open Questions` set to `none`, and its implementation notes anchor scope to `docs/plans/provider-identifier-ddl-guardrail-contract.md`, `docs/naming/default-naming-policy.md`, `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs`, `src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs`, `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs`, and `src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs`.

Split recommendations
- No new split is recommended right now; first reconcile the epic against the already-existing done child tickets, and only create another child ticket if PO can name residual work that is not already covered by those four tickets.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment