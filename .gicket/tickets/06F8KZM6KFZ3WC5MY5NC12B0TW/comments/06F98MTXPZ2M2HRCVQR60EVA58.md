[gicket-bot] PO-critic review contract

Summary
- Epic contract is clear as a closure-only roll-up over four done child tickets, so it should return to PO for closure routing and ticket-metadata cleanup rather than be handed to dev.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/description.md lines 4-47 says the epic is a closure-only roll-up, scopes out any dev handoff or new implementation, says no residual developer-owned work remains, and has `## Open Questions` = `- none`.
- .gicket/relations/TW/0G/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZMRXRHRKHV56Y96M4S90G--parentOf.json, .gicket/relations/TW/RG/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZN2BBPB3XFFXEXGX4N4RG--parentOf.json, .gicket/relations/TW/VC/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZNBGB8FPW6TK5A8SAJMVC--parentOf.json, and .gicket/relations/TW/68/06F8KZM6KFZ3WC5MY5NC12B0TW--06F8KZNNS76TD9Z7ESB173FZ68--parentOf.json persist the four child relations.
- `git log --oneline --decorate develop --grep='06F8KZMRXRHRKHV56Y96M4S90G|06F8KZN2BBPB3XFFXEXGX4N4RG|06F8KZNBGB8FPW6TK5A8SAJMVC|06F8KZNNS76TD9Z7ESB173FZ68' -n 20` returned `ef35f304c`, `d23b0e481`, `fa1f7a1f1`, and `826b80b9f` AUTO-INTEGRATION commits on `develop` for the four child tickets.
- `git diff --name-only develop...HEAD -- README.md docs src tests` returned no paths, while `git diff --name-only develop...HEAD` returned only files under `.gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/`; the epic branch contains ticket metadata changes but no unlanded README/docs/src/tests implementation.
- .gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/comments/06F98AWGG8DKF73FBNYDWQY9GR.md lines 17-21 shows the earlier PO-critic blocker was ambiguity about residual dev scope; .gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/comments/06F988SYGAPR1ZSH1JPV6SVZA8.md lines 14-18 shows PO refinement updated the durable contract and handed the ticket back to PO-critic; .gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/comments/06F98AY2QQX2Q63NM69CQ9PT6G.md lines 18-22 marks follow-ups to the child tickets as obsolete because `develop` already contains them as `done`.

Blocking findings
- Approving this ticket for dev would contradict the persisted contract: `.gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/description.md` Scope Out excludes any dev handoff or new implementation, and Acceptance Criteria says no residual developer-owned work remains.

Required PO actions
- Route this epic through the PO-owned closure/completion path instead of handing it to dev.
- In the closure/completion note, cite the four done child tickets and their landed `develop` commits `ef35f304c`, `d23b0e481`, `fa1f7a1f1`, and `826b80b9f` as the implementation coverage for this epic.

Open issues ledger
- critic-item-1 [required-po-action] Route this epic through the PO-owned closure/completion path instead of handing it to dev.
- critic-item-2 [required-po-action] In the closure/completion note, cite the four done child tickets and their landed `develop` commits `ef35f304c`, `d23b0e481`, `fa1f7a1f1`, and `826b80b9f` as the implementation coverage for this epic.
- critic-item-3 [blocking-finding] Approving this ticket for dev would contradict the persisted contract: `.gicket/tickets/06F8KZM6KFZ3WC5MY5NC12B0TW/description.md` Scope Out excludes any dev handoff or new implementation, and Acceptance Criteria says no residual developer-owned work remains.

Missing examples / edge cases
- none

Risky assumptions
- That future provider-expansion or consumer-facing physical naming override work will be opened as new follow-up tickets or epics instead of being appended to this parent, as the current contract expects.

AC / test suggestions
- Add a closure checklist item that the final epic completion record must reference the four child tickets plus the empty `develop...HEAD -- README.md docs src tests` diff as closure evidence.

Implementation watchouts
- Do not reopen implementation under this parent epic; if new provider-specific DDL safety beyond SQLite, Oracle, PostgreSQL, SQL Server, and MySQL is requested, create a new follow-up ticket or epic.
- Do not use this parent epic for a consumer-facing physical naming override API; the contract explicitly treats that as separate follow-up scope.

Non-blocking notes
- The repository anchors named in the contract are present: `docs/plans/provider-identifier-ddl-guardrail-contract.md`, `src/DCoding.Data.DVault/DataVaultProviderIdentifierPreflight.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderIdentifierPreflightTests.cs`, `src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs`, `src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs`, `src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs`, `docs/model-first-governance.md`, `docs/production-adoption-checklist.md`, and `docs/releases/v0.29.0.md`.
- Current branch inspection shows the owner branch is `ticket/06F8KZM6KFZ3WC5MY5NC12B0TW-epic-provider-naming-and-ddl-guardrails`; recent graph output is workflow/ticket history rather than a new implementation branch tip.

Split recommendations
- No new split is recommended; the existing four done child tickets already cover this epic.
- If new provider-expansion or physical naming override work appears later, open a new follow-up ticket or epic instead of reusing this closure-only parent.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment