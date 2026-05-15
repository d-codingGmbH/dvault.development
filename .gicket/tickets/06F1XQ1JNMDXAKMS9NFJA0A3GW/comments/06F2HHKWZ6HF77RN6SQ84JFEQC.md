[gicket-bot] PO-critic review contract

Summary
- Rule scope and analyzer-test expectations are well bounded, but the ticket still leaves stable analyzer diagnostic-id allocation and catalog ownership unspecified, so it should return to PO once more before developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted contract in .gicket/tickets/06F1XQ1JNMDXAKMS9NFJA0A3GW/description.md has '## Open Questions - none', scopes the work to two analyzer rules plus minimal harness, and requires each rule to have a stable DVault diagnostic id/category/title/message/remediation.
- git log --oneline -n 4 on branch ticket/06F1XQ1JNMDXAKMS9NFJA0A3GW-task-implement-first-analyzer-rules-and-tests shows only workflow commits e6851bfc7, ffae082dc, 04eb23338, and 7e84e2d7a for PO/PO-critic claim and handoff.
- git diff --name-only cc7e30ee2..HEAD lists only .gicket/tickets/06F1XQ1JNMDXAKMS9NFJA0A3GW/* files, so this branch adds ticket metadata only and no analyzer scaffolding or source changes yet.
- Repository search found no existing analyzer/Roslyn project: rg on src/, tests/, and DVault.slnx for 'Microsoft.CodeAnalysis|Analyzer|Roslyn' returned only the existing RunAnalyzers=false settings in tests/DCoding.Data.DVault.Tests/{Unit,Modeling,Integration}/*.csproj and no analyzer project references.
- src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs currently defines only DMV1001-DMV1801 model-artifact entries and DVM2001-DVM2006 migration entries.
- src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs and src/DCoding.Data.DVault/DataVaultDiagnosticDefinition.cs are internal, and src/DCoding.Data.DVault/Properties/AssemblyInfo.cs grants InternalsVisibleTo only to provider and test assemblies, not to a future analyzer assembly.
- Related blocker .gicket/tickets/06F1XPS7KGKBP5SVMQPJC49J2G/ticket.json is done, but its description still leaves as a follow-up question whether later diagnostic families should keep the same DMV#### prefix with reserved bands or get an explicit cross-family allocation policy.

Blocking findings
- The task requires stable analyzer diagnostic ids, but the ticket does not reserve concrete ids or a code band for the two rules. Existing repository evidence stops at DMV1001-DMV1801 and DVM2001-DVM2006, and the completed diagnostic-code story still leaves future-family allocation open. That leaves a user-visible stable-id decision to implementation.
- The ticket says the analyzer work should consume the established DVault diagnostic contract, but the only concrete catalog types are internal to src/DCoding.Data.DVault and are not shared with a future analyzer project by any directly observed public or InternalsVisibleTo boundary. The package/catalog ownership needed to satisfy the acceptance criteria is therefore not explicit.

Required PO actions
- Assign the two first analyzer diagnostics explicit stable ids, or at minimum reserve a concrete analyzer code band and category convention that this ticket must use.
- Clarify where analyzer diagnostic metadata is supposed to live: extend the core-package catalog, add a documented shared/public contract for analyzer assemblies, or explicitly allow analyzer-local definitions that mirror the established fields.
- If the intended package-boundary decision belongs to parent story 06F1XQ15J5JEC92T1QCE9TABBM, copy that decision into this task or refine the parent before re-handing this task to dev.

Open issues ledger
- critic-item-1 [required-po-action] Assign the two first analyzer diagnostics explicit stable ids, or at minimum reserve a concrete analyzer code band and category convention that this ticket must use.
- critic-item-2 [required-po-action] Clarify where analyzer diagnostic metadata is supposed to live: extend the core-package catalog, add a documented shared/public contract for analyzer assemblies, or explicitly allow analyzer-local definitions that mirror the established fields.
- critic-item-3 [required-po-action] If the intended package-boundary decision belongs to parent story 06F1XQ15J5JEC92T1QCE9TABBM, copy that decision into this task or refine the parent before re-handing this task to dev.
- critic-item-4 [blocking-finding] The task requires stable analyzer diagnostic ids, but the ticket does not reserve concrete ids or a code band for the two rules. Existing repository evidence stops at DMV1001-DMV1801 and DVM2001-DVM2006, and the completed diagnostic-code story still leaves future-family allocation open. That leaves a user-visible stable-id decision to implementation.
- critic-item-5 [blocking-finding] The ticket says the analyzer work should consume the established DVault diagnostic contract, but the only concrete catalog types are internal to src/DCoding.Data.DVault and are not shared with a future analyzer project by any directly observed public or InternalsVisibleTo boundary. The package/catalog ownership needed to satisfy the acceptance criteria is therefore not explicit.

Missing examples / edge cases
- A concrete false-positive guard for valid direct field selectors, not only property selectors, if analyzer behavior is meant to match the current runtime selector helpers.
- A concrete example for implicit conversion or boxing selectors such as x => (object)x.Member, because current runtime selector handling is split between DataVaultCodeFirstHubBuilder.cs and DataVaultCodeFirstSelector.cs.
- A concrete sample showing what 'generated-code-safe' and 'direct user-authored invocation chains only' means in practice.

Risky assumptions
- Assuming developers can pick analyzer ids ad hoc now and still satisfy the 'stable diagnostic id' promise without later churn or collisions.
- Assuming a separate analyzer package can 'consume' the existing catalog conventions without an explicitly shared/public catalog boundary.
- Assuming minimal analyzer scaffolding can be decided locally without unresolved parent-story package-boundary choices leaking into this task.

AC / test suggestions
- Add the approved ids/categories for the two diagnostics directly to the acceptance criteria once PO assigns them.
- Keep the existing true-positive/false-positive rule coverage, and add one explicit non-DVault/noise guard fixture to back the 'direct invocation chains only' scope.
- If field selectors are intended to stay supported, add one named analyzer fixture that proves no diagnostic on a valid field member.

Implementation watchouts
- All existing test projects explicitly set RunAnalyzers=false, so normal build/test execution will not prove analyzer behavior.
- There is no existing analyzer/Roslyn project in src/, tests/, or DVault.slnx, so even minimal scaffolding is net-new on this ticket.
- Selector validation is not centralized in one runtime helper: BusinessKey validation lives partly in DataVaultCodeFirstHubBuilder.cs, while Payload/DrivingKey use DataVaultCodeFirstSelector.cs.
- The current branch history and diff show ticket metadata changes only; developers will start from repository baseline rather than from any prepared analyzer scaffold.

Non-blocking notes
- The rule slice itself is well bounded: the contract stays on two high-confidence Code-First rules and explicitly excludes broader analyzer coverage and code fixes.
- Repository evidence for the target semantics is strong: docs/plans/fluent-code-first-api-contract.md plus tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs and DataVaultCodeFirstLinkTests.cs already define the relevant selector and duplicate-member behaviors.

Split recommendations
- No functional split of the two-rule implementation is needed after clarification; the current slice remains a good dev-sized ticket.
- If PO does not want to refine parent story 06F1XQ15J5JEC92T1QCE9TABBM yet, create a tiny prerequisite clarification ticket for analyzer diagnostic-id allocation and catalog ownership, then resend this task unchanged.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment