[gicket-bot] PO-critic review contract

Summary
- The AddDVault-only delivery contract is now source-backed and has no unresolved Open Questions, but the persisted ticket still carries dev/test blocker labels. Return to PO for ticket-level label cleanup before developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB6ZMBB97J1Z5TBS29QMGPR/description.md Delivery Contract says Open Questions: none, scopes the test to optionless AddDVault(IServiceCollection), and explicitly excludes UseDataVault, EF provider integration, consuming DbContext, and DbContext-specific startup behavior.
- git rev-parse/git branch observed HEAD 83b41fc5be5dd94bfeda3ca1c70b4a9e80136b0b on ticket/06EXB6ZMBB97J1Z5TBS29QMGPR-task-add-smoke-tests-for-minimal-startup.
- src/DVault/DVaultServiceCollectionExtensions.cs:16 defines public static IServiceCollection AddDVault(this IServiceCollection services); lines 20-21 register DefaultNamingPolicy.Instance and DataVaultConventions.Default, and the method returns services.
- src/DVault/Modeling/DataVaultConventions.cs:51 defines public static DataVaultConventions Default; src/DVault/Modeling/DefaultNamingPolicy.cs:63 defines public static DefaultNamingPolicy Instance.
- tests/DVault.Tests contains DVault.Tests.csproj, Unit, Integration, Shared, and existing xUnit test projects; DVault.Tests.csproj has a VSTest target, and DVault.Tests.csproj at repo root includes Unit and Integration test projects.
- .gicket/relations/T4/PR/06EXB6Z3YMAPSRYRB8NQX3ZST4--06EXB6ZMBB97J1Z5TBS29QMGPR--parentOf.json shows one parentOf relation from 06EXB6Z3YMAPSRYRB8NQX3ZST4 to this ticket.
- PO refinement comment 06EXK6S0XE7BYQ5HJH3HPTDA4C.md marks decision ready_for_po_critic and says prior critic items were answered by narrowing scope to source-backed AddDVault.

Blocking findings
- none

Required PO actions
- After label cleanup, re-handoff for PO-critic review with the AddDVault-only contract unchanged unless the product scope changes.

Open issues ledger
- critic-item-1 [required-po-action] After label cleanup, re-handoff for PO-critic review with the AddDVault-only contract unchanged unless the product scope changes.

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Keep the smoke test focused on public startup behavior: new ServiceCollection().AddDVault(), same IServiceCollection returned, provider builds, and DefaultNamingPolicy/DataVaultConventions resolve to provider-neutral defaults.

Implementation watchouts
- Do not reintroduce UseDataVault, EF provider integration, a consuming DbContext, external database use, or a new public startup API under this ticket.
- Place the test where the branch's normal DVault test command executes it; the repository has multiple DVault test layouts.

Non-blocking notes
- The prior compatibility concern is resolved by direct source evidence for AddDVault(IServiceCollection).
- The Open Questions section contains none, so the guardrail against approving with unresolved open questions is not the blocker here.

Split recommendations
- No split is required for the narrowed AddDVault smoke-test scope.
- Create a separate follow-up only if UseDataVault, EF-specific startup wiring, provider integration, or DbContext-specific startup behavior becomes required.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment