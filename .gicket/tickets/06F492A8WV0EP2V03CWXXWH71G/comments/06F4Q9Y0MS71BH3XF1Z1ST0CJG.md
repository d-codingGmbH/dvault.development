[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff. The ticket contract is detailed, has no open questions, and matches the current DVault migration-guardrail API, test, and relation baseline in the repository.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F492A8WV0EP2V03CWXXWH71G/description.md contains a full Delivery Contract with ## Open Questions set to none, 8 acceptance criteria, 5 definition-of-done items, and explicit scope boundaries for this pre-development story.
- Live relation files .gicket/relations/1G/24/06F492A8WV0EP2V03CWXXWH71G--06F492BG6BZYYFMBE5WK7CB024--blocks.json and .gicket/relations/1G/VM/06F492A8WV0EP2V03CWXXWH71G--06F492BNDPWS9P4EDSV0W7G6VM--blocks.json confirm the current downstream blocks links named in the contract.
- src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs currently exposes only Diagnostics, Issues, IsValid, HasFindings, and ToDisplayString(), and ToDisplayString() renders only valid|invalid plus finding rows; it does not yet expose ordered per-operation safe/risky/incompatible outcomes, so the story addresses a real current gap.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs already locks the DVM2001-DVM2006 catalog, severities, path format, and deterministic ordering, and tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs proves a SQLite-backed diagnostics baseline with provider data such as sqlite-v1 and sqlite-provider-v1, which matches the ticket's additive reporting and provider-aware wording scope.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt already snapshots DataVaultMigrationGuardrailReport and AnalyzeReport(...), so the contract's API-snapshot note is grounded in current repository practice.
- Branch-history inspection shows git log --oneline -n 5 ticket/06F492A8WV0EP2V03CWXXWH71G-story-strengthen-migration-guardrail-reports currently ends at adfa29f39 and git diff --name-only f07f98912..adfa29f39 lists only .gicket/tickets/06F492A8WV0EP2V03CWXXWH71G/... files, so this is still a pre-development metadata-only branch as expected.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- An explicit example for an operation that produces both warning and error findings would lock the intended one-of-three summary outcome more tightly, even though the current wording strongly implies error should win.
- A concrete example of provider-defaulted or provider-neutral wording versus resolved SQLite wording would make the human-readable rendering expectations easier to assert.

Risky assumptions
- The contract implies, but does not literally spell out, that a per-operation summary becomes incompatible whenever any error-severity DVM finding exists, even if warning findings are also attached to the same operation.
- The contract assumes provider-aware wording can be satisfied from the existing DataVaultDiagnosticsResult.Explain surface without introducing any new provider-discovery mechanism; repository evidence supports that assumption today via fields such as provider name, capability profile, provider behavior profile, and value-format metadata.

AC / test suggestions
- Add one assertion that mixed-severity findings on a single operation collapse to one structured outcome with deterministic precedence while preserving the full finding list.
- Add paired human-readable assertions for a real SQLite-backed DbContext and a provider-defaulted/provider-neutral baseline so wording does not regress.
- Update the public API snapshot when additive report DTO members or types are introduced.

Implementation watchouts
- src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs already routes guardrail output through DataVaultMigrationOperationDiagnostics.AnalyzeReport(...) and report.ToDisplayString(), so the change needs to stay additive and keep the current command/exit behavior stable.
- Current tests assert deterministic Issues ordering; any new ordered operation-summary surface must preserve incoming MigrationOperation sequence and stable intra-operation detail ordering rather than re-sorting from merged dictionaries or issue sets.
- Because DataVaultMigrationGuardrailReport.cs currently derives visible findings from existing DVM-coded diagnostics only, safe operations should not be implemented as synthetic DMV/DVM issues.

Non-blocking notes
- The ticket comment directory exists, but the inspected comment files are gicket-bot workflow comments and find .gicket/tickets/06F492A8WV0EP2V03CWXXWH71G -maxdepth 1 -type d shows no attachments directory, which is consistent with the contract's claim that there were no human scope comments or attachments.
- Downstream tickets 06F492BG6BZYYFMBE5WK7CB024 and 06F492BNDPWS9P4EDSV0W7G6VM are still todo in their ticket.json files, so keeping them blocked behind this report-contract story remains coherent.

Split recommendations
- No split recommended. The ticket is already bounded to strengthening one existing report lane while leaving the aggregator story 06F492BG6BZYYFMBE5WK7CB024 and documentation task 06F492BNDPWS9P4EDSV0W7G6VM as downstream consumers.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment