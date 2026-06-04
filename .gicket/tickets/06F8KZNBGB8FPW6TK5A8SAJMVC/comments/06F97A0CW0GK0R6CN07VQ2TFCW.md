[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F8KZNBGB8FPW6TK5A8SAJMVC\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua\u0027 and commit \u00278d4445508e49\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua\u0027 from source \u00278d4445508e49\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua\u0027.",
    "Evidence: \u0060git -C /mnt/c/Projects/DVault rev-parse --verify 8d4445508e49^{commit}\u0060 resolved commit \u00608d4445508e499088b97fb8fb42791f15553177e9\u0060 on branch \u0060ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua\u0060.",
    "Evidence: \u0060git -C /mnt/c/Projects/DVault diff --stat develop..8d4445508e49 -- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs src/DCoding.Data.DVault/DataVaultDiagnostics.cs src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0060 reported 7 changed product/test files with 605 insertions and 55 deletions.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:325-327\u0060 only analyzes a CreateTable primary key when \u0060operation.PrimaryKey\u0060 is non-null.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:818-822\u0060 stores \u0060DataVaultInternalAnnotationNames.ProviderIncludedIndexPropertyNames\u0060; \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:2625-2635\u0060 maps that annotation through \u0060GetPhysicalColumnName(...)\u0060; \u0060src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:673-687\u0060 returns the raw annotation strings during create-index comparison.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:488-558\u0060 adds provider-shape checks for generated load timestamp and PIT snapshot-reference columns, and \u0060:603-687\u0060 adds DVM2010 blocking checks for create-index/add-primary-key/provider-covered-primary-key index mismatches.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:412-505\u0060 adds five-provider index-shape and representative loadTimestampStorage drift coverage, while \u0060:768-940\u0060 builds safe included-index operations only through provider annotations, not the new internal-annotation fallback path.",
    "Evidence: No policy-defined executable verification commands were run in this read-only review session.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/diagnostics, area/ef-core, area/migrations, area/provider-support, area/schema, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua\u0027.",
    "Evidence: Ticket history references implementation commit \u00278d4445508e49\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: A migration that makes generated load timestamp or PIT satellite snapshot reference columns nullable, changes their provider store type, or changes their provider value format away from the selected profile plus loadTimestampStorage contract returns blocking guardrail findings. (Provider-shape checks were added for generated load timestamp and PIT snapshot-reference columns, and the updated unit tests iterate the supported provider profiles plus representative loadTimestampStorage variants.).",
    "AC check passed: Suspicious drop-plus-add or provider-specific replacement patterns affecting DVault-owned generated tables, columns, primary keys, or secondary indexes are reported deterministically through the guardrail report with the offending operation path and remediation guidance. (Suspicious drop-plus-add replacement detection remains deterministic for tables, columns, indexes, and primary keys, and the report tests still assert ordered operation paths and DVM2008 findings.).",
    "DoD check passed: The selected provider capability profile and loadTimestampStorage mapping remain the authoritative source for effective index shape and timestamp storage expectations used by guardrail evaluation. (The schema baseline is still built from DataVaultDiagnosticsResult.Explain, including the selected capability profile, index-coverage flag, store types, and provider value formats.).",
    "DoD check passed: Guardrail and preflight outputs stay bounded, deterministic, and automation-friendly through existing report and command surfaces; DVault still does not apply or rewrite migrations. (The updated guardrail/report logic remains bounded and deterministic, and the implementation still validates and reports without applying or rewriting migrations.).",
    "DoD check passed: The separate downstream documentation task 06F8KZNNS76TD9Z7ESB173FZ68 remains blocked by implementation completion rather than being widened into this story. (The inspected diff stays in product/test code plus ticket artifacts and does not widen the downstream documentation task into this implementation story.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Given any supported provider profile and loadTimestampStorage selection, migration guardrail evaluation uses the same effective generated shape the translator emits for generated secondary indexes and primary keys, including omitted Oracle primary-key-covered secondary indexes, native include columns on SQL Server and Postgres, appended include columns on Sqlite and Oracle, and ignored include columns on MySql. (The baseline still comes from the translator-backed diagnostics explain surface, but create-index comparison can diverge from that effective shape because the new internal included-index fallback is read back as raw annotation strings instead of normalized physical column names.).",
    "AC check failed: A migration that creates, recreates, renames, drops, or replaces a DVault-owned generated secondary index or primary key with provider-incompatible columns, uniqueness, include handling, or duplicate-index behavior returns blocking guardrail findings before migration application. (CreateIndex and AddPrimaryKey mismatches were promoted to blocking DVM2010 findings, but CreateTable primary-key validation still runs only when an inline PrimaryKey object is present, so a DVault-owned table can be created without its generated primary key and avoid an MI-4 blocker.).",
    "AC check failed: Provider-specific guardrail findings name the selected provider/profile and the effective generated object shape needed to explain the failure through the existing diagnostics/report surfaces without executing or rewriting the migration. (DVM2010 messages include provider/profile and expected shape, but the internal included-column fallback path can surface raw property names instead of the effective physical index shape, so the explanatory output is not reliably the same shape the translator emitted.).",
    "DoD check failed: DataVaultMigrationOperationDiagnostics and the central DVM catalog cover the provider-specific migration risks in this story for generated secondary indexes, primary keys, and timestamp storage drift without creating a second provider-facts source of truth. (The central migration DVM catalog gained DVM2010, but the runtime/catalog combination still misses the create-table missing-primary-key case and leaves DVM2004 text describing create/recreate coverage that now lives under DVM2010.).",
    "DoD check failed: Unit tests lock the visible five-provider baseline and the expected safe, risky, and incompatible outcomes for representative generated secondary-index and primary-key migration operations. (The added tests cover the five-provider baseline and representative loadTimestampStorage variants, but they do not lock the observed missing-primary-key CreateTable case or the new internal-annotation-only included-index fallback path.).",
    "Blocking: \u0060src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:325-327\u0060 leaves a gap where \u0060CreateTableOperation\u0060 can omit the DVault-generated primary key entirely and still avoid any MI-4/DVM2010 blocker, which conflicts with the story\u0027s primary-key guardrail scope.",
    "Blocking: by inspection of \u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:818-822\u0060, \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:2625-2635\u0060, and \u0060src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:673-687\u0060, the new internal included-index fallback stores property names but the guardrail comparison reads them back as-is. When provider-native include annotations are unavailable, that can produce false DVM2010 index-shape failures or wrong actual-shape text.",
    "Blocking: \u0060src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs:166-168\u0060 still describes DVM2004 as covering create/recreate key/index mismatches even though the new runtime path moved those failures to DVM2010, leaving the central catalog inconsistent with emitted behavior."
  ],
  "evidence": [
    "\u0060git -C /mnt/c/Projects/DVault rev-parse --verify 8d4445508e49^{commit}\u0060 resolved commit \u00608d4445508e499088b97fb8fb42791f15553177e9\u0060 on branch \u0060ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua\u0060.",
    "\u0060git -C /mnt/c/Projects/DVault diff --stat develop..8d4445508e49 -- src/DCoding.Data.DVault/DataVaultAnnotationNames.cs src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs src/DCoding.Data.DVault/DataVaultDiagnostics.cs src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs\u0060 reported 7 changed product/test files with 605 insertions and 55 deletions.",
    "\u0060src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:325-327\u0060 only analyzes a CreateTable primary key when \u0060operation.PrimaryKey\u0060 is non-null.",
    "\u0060src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:818-822\u0060 stores \u0060DataVaultInternalAnnotationNames.ProviderIncludedIndexPropertyNames\u0060; \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:2625-2635\u0060 maps that annotation through \u0060GetPhysicalColumnName(...)\u0060; \u0060src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:673-687\u0060 returns the raw annotation strings during create-index comparison.",
    "\u0060src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:488-558\u0060 adds provider-shape checks for generated load timestamp and PIT snapshot-reference columns, and \u0060:603-687\u0060 adds DVM2010 blocking checks for create-index/add-primary-key/provider-covered-primary-key index mismatches.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:412-505\u0060 adds five-provider index-shape and representative loadTimestampStorage drift coverage, while \u0060:768-940\u0060 builds safe included-index operations only through provider annotations, not the new internal-annotation fallback path.",
    "No policy-defined executable verification commands were run in this read-only review session.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/ef-core, area/migrations, area/provider-support, area/schema, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua\u0027.",
    "Ticket history references implementation commit \u00278d4445508e49\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Return the ticket to dev and add an MI-4/DVM2010 blocker for DVault-owned CreateTable operations that omit the expected generated primary key.",
    "Normalize \u0060ProviderIncludedIndexPropertyNames\u0060 to effective physical column names during migration-operation comparison, and add a regression test that exercises the internal-annotation-only include fallback path.",
    "Update the DVM2004/DVM2010 catalog text to match the new runtime split, then rerun \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 through legacy verification or another writable verification environment after the fixes."
  ],
  "branchName": "ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua",
  "commitSha": "8d4445508e49"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F8KZNBGB8FPW6TK5A8SAJMVC`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua`