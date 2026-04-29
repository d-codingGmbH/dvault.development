[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB6ZMBB97J1Z5TBS29QMGPR/description.md Delivery Contract says PO Handoff decision ready_for_po_critic and ## Open Questions contains '- none'.
- The same persisted contract scopes this ticket to one self-contained smoke test for optionless AddDVault(IServiceCollection), explicitly excluding UseDataVault, EF provider integration, a consuming DbContext, and DbContext-specific startup behavior.
- src/DVault/DVaultServiceCollectionExtensions.cs defines public static IServiceCollection AddDVault(this IServiceCollection services), validates null services, registers DefaultNamingPolicy.Instance and DataVaultConventions.Default, and returns services.
- src/DVault/Modeling/DefaultNamingPolicy.cs defines public sealed class DefaultNamingPolicy with public static DefaultNamingPolicy Instance; src/DVault/Modeling/DataVaultConventions.cs defines public sealed class DataVaultConventions with public static DataVaultConventions Default.
- tests/DVault.Tests exists with DVault.Tests.csproj, Modeling, Unit, Integration, and Shared files; git ls-files shows tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs and tests/DVault.Tests/Unit/TestDiscoverySmokeTests.cs.
- .gicket/relations/T4/PR/06EXB6Z3YMAPSRYRB8NQX3ZST4--06EXB6ZMBB97J1Z5TBS29QMGPR--parentOf.json shows one parentOf relation from 06EXB6Z3YMAPSRYRB8NQX3ZST4 to this ticket.

Blocking findings
- none

Required PO actions
- Reconcile the PO Summary/PO-critic checklist response with the actual ticket labels so the handoff evidence no longer claims completed label cleanup while blocker labels remain.

Open issues ledger
- critic-item-1 [required-po-action] Reconcile the PO Summary/PO-critic checklist response with the actual ticket labels so the handoff evidence no longer claims completed label cleanup while blocker labels remain.

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Keep the AC focused on public startup behavior: new ServiceCollection().AddDVault(), BuildServiceProvider(), and resolving DefaultNamingPolicy and DataVaultConventions from the provider.
- Avoid AC that requires inspecting private descriptor ordering or creating DbContext/EF/provider infrastructure.

Implementation watchouts
- Place the smoke test where the existing tests/DVault.Tests command path will execute it.
- Do not introduce a new public startup API, solution/project scaffold changes, external database dependency, or UseDataVault/DbContext behavior under this ticket.

Non-blocking notes
- The AddDVault API, provider-neutral defaults, and DVault test structure are directly present in source, so no product-scope blocker remains once labels are corrected.
- The persisted Open Questions section contains only '- none', so the return is due to ticket labels rather than unresolved product questions.

Split recommendations
- No split is needed for the narrowed AddDVault smoke-test scope once the stale blocked labels are removed.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment