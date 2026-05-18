[gicket-bot] PO refinement contract

Summary
- Verified against develop that provider-native bulk strategy implementation and live bulk-provider coverage are already landed; this ticket should now be treated as closure-only/no-work rather than a fresh dev implementation story.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The native-strategy delivery is already landed on develop. Relative to develop, this branch contains only .gicket metadata, so this story should be re-routed/reclassified as closure-only or no-work, not handed to dev as a fresh implementation story.
- critic-item-2: `answered` - No code delta remains to hand to dev from this ticket branch. Scope In, Acceptance Criteria, and Definition of Done are updated below to match the remaining work: closure/state alignment and ownership reconciliation around already-landed develop code and tests.
- critic-item-3: `answered` - Confirmed. This is not a clean developer handoff branch for implementation work because the branch carries only ticket metadata against develop. The refined contract removes the fresh implementation ask and treats the ticket as closure-only/no-work.
- critic-item-4: `answered` - The ownership split is reconciled by treating this story as historical closure rather than exclusive implementation ownership. Done story 06F2PGMSQ4D4FV8W5ZERD4GS8C already owns the SPI baseline, done task 06F2PGN4GPQCGC5WHZQBGP4SD0 already owns the provider-neutral fallback baseline, and done child 06F2PGNT7DF4DVNKYWDFZC8DEM is the last visible develop integration that added live bulk-provider proof and touched provider strategy surfaces.

Clarifications
- Current ticket comments are bot lease/claim workflow comments only, and the ticket has no persisted attachments.
- Relative to develop, the working branch carries only .gicket metadata for ticket 06F2PGNGVQ3TZZWSABAK5SNFK4; there is no src/, tests/, docs/, or README.md delta to hand to development.
- Develop already contains provider-native strategy and registration surfaces for Postgres, SQL Server, MySQL, and Oracle in the corresponding src/DCoding.Data.DVault.* packages, with shared gate evaluation in src/DCoding.Data.DVault/DataVaultDiagnostics.cs.
- Develop already contains bulk-path proof in tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderBulkSaveAssertions.cs and provider bulk test methods in PostgresOptimizedDataVaultSaveServiceTests.cs, SqlServerDataVaultSmokeTests.cs, MySqlExplicitDataVaultSaveServiceTests.cs, and OracleDataVaultSmokeTests.cs.
- Related ticket state is already done for SPI 06F2PGMSQ4D4FV8W5ZERD4GS8C, fallback 06F2PGN4GPQCGC5WHZQBGP4SD0, and child bulk integration coverage 06F2PGNT7DF4DVNKYWDFZC8DEM; current outgoing relations to benchmark 06F2PGNZBRNCQ1SV2KKP6F3BA8 and docs 06F2PGP2B2RZGGK3CVKK5WRRP8 can remain as closure-ordering context.
- No child ticket was created, no relation was added or removed, no attachment was added, and no planning document was written in this pass.

Scope In
- Reclassify this ticket from pending dev implementation to closure-only acknowledgment that provider-native bulk strategy work is already integrated on develop.
- Capture the exact no-delta state for this branch: no remaining src/, tests/, docs/, or README.md changes exist relative to develop.
- Reconcile delivery ownership against the live done tickets: SPI on 06F2PGMSQ4D4FV8W5ZERD4GS8C, fallback on 06F2PGN4GPQCGC5WHZQBGP4SD0, and live bulk-provider proof plus the last visible develop integration on 06F2PGNT7DF4DVNKYWDFZC8DEM.
- Keep benchmark 06F2PGNZBRNCQ1SV2KKP6F3BA8 and docs 06F2PGP2B2RZGGK3CVKK5WRRP8 aligned with the already-landed develop baseline rather than a nonexistent new implementation branch.

Scope Out
- Any new provider-native strategy code or gate-threshold tuning in the Postgres, SQL Server, MySQL, or Oracle packages.
- Any new external bulk-provider test implementation; the repository already contains that proof on develop.
- Reopening the explicit bulk SPI or the provider-neutral fallback baseline already completed by 06F2PGMSQ4D4FV8W5ZERD4GS8C and 06F2PGN4GPQCGC5WHZQBGP4SD0.
- Benchmark work, comparative performance claims, or broader docs/release-note packaging beyond closure alignment.

Open questions
- none

Follow-up questions
- Should docs task 06F2PGP2B2RZGGK3CVKK5WRRP8 explicitly reconcile older Oracle wording in docs/releases/v0.5.0.md with the current develop bulk-test and strategy baseline?
- Should benchmark story 06F2PGNZBRNCQ1SV2KKP6F3BA8 cite the already-landed provider gates and bulk batch shapes from ExternalProviderBulkSaveAssertions.cs when publishing crossover guidance?

Risks
- Audit/history confusion remains possible because the last visible develop integration for provider bulk proof is done child 06F2PGNT7DF4DVNKYWDFZC8DEM while this story currently claims implementation ownership.
- Documentation can continue to drift from actual shipped behavior until 06F2PGP2B2RZGGK3CVKK5WRRP8 reconciles older release-note wording, especially around Oracle bulk coverage.

Split recommendations
- No additional split is recommended; close or re-route this ticket as already-landed/no-work instead of creating a new child for code that is already on develop.
- If future provider-native bulk changes arise, open a new follow-on story against the concrete missing delta instead of reusing this historical ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment