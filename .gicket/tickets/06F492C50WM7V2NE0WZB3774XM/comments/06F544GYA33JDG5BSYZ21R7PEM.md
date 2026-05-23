[gicket-bot] PO-critic review contract

Summary
- Persisted contract is stale against the current repo: it asks for new ReadShape performance diagnostics that current source, release notes, public API snapshot, and tests already contain, so the ticket needs PO re-scoping before developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F492C50WM7V2NE0WZB3774XM/description.md still says the current branch 'does not evidence any existing public performance-stage record type' and scopes in introducing additive performance-stage records under DataVaultDiagnosticsResult.ReadShape.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs already defines DataVaultReadShapeEntity (line 444), DataVaultReadShapeColumnSet (452), DataVaultReadShapeIndexBaseline (459), DataVaultReadShapeProviderDiagnostics (470), DataVaultSatelliteReadShapeDiagnostics (482), DataVaultPitReadShapeDiagnostics (505), DataVaultBridgeReadShapeDiagnostics (527), DataVaultReadShapeDiagnostics (541), and DataVaultDiagnosticsResult.ReadShape (570).
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs already exposes the five IDataVaultReadDiagnosticsService Analyze(DbContext, ...) overloads and normalizes registry latest-satellite and bridge requests to explicit requests before AnalyzeDbContext(...).
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs lines <redacted> already populate latest-satellite, PIT, and bridge ReadShape payloads with filter columns, selection/ordering rules, provider facts, and expected index baselines.
- docs/releases/v0.16.0.md already documents DataVaultDiagnosticsResult.ReadShape as shipped additive behavior describing translated table identity, filter columns, row-selection and ordering rules, key/index baselines, provider fallback caveats, and deterministic camelCase support-bundle output.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt already snapshots the public ReadShape types, including DataVaultSatelliteReadShapeDiagnostics(... FilterColumns ... DeterministicOrdering ... ExpectedIndexBaseline).
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs and tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs already assert explicit and registry latest-satellite, PIT, and bridge ReadShape population, registry equivalence, support-bundle serialization, and provider fallback facts.

Blocking findings
- The persisted delivery contract is factually stale against current source: it says no existing public performance-stage record type is evidenced, but the repo already exposes and snapshots those ReadShape performance records.
- The ticket does not identify a net-new delta from already shipped and tested behavior. Current source, release notes, public API snapshot, and tests already cover the same ReadShape performance/index/provider diagnostics the story asks developers to add.

Required PO actions
- Rewrite the story as a concrete delta from the current ReadShape baseline, naming the exact missing fields or behaviors that are not already present in src/DCoding.Data.DVault/DataVaultDiagnostics.cs and its existing tests/docs.
- If no source-backed gap remains, close or reclassify the ticket as duplicate/obsolete, or convert it into a narrower follow-up with a genuinely new outcome.
- Update acceptance criteria and definition of done so they do not treat already-shipped ReadShape, support-bundle, public API snapshot, and registry-equivalence behavior as new development work.

Open issues ledger
- critic-item-1 [required-po-action] Rewrite the story as a concrete delta from the current ReadShape baseline, naming the exact missing fields or behaviors that are not already present in src/DCoding.Data.DVault/DataVaultDiagnostics.cs and its existing tests/docs.
- critic-item-2 [required-po-action] If no source-backed gap remains, close or reclassify the ticket as duplicate/obsolete, or convert it into a narrower follow-up with a genuinely new outcome.
- critic-item-3 [required-po-action] Update acceptance criteria and definition of done so they do not treat already-shipped ReadShape, support-bundle, public API snapshot, and registry-equivalence behavior as new development work.
- critic-item-4 [blocking-finding] The persisted delivery contract is factually stale against current source: it says no existing public performance-stage record type is evidenced, but the repo already exposes and snapshots those ReadShape performance records.
- critic-item-5 [blocking-finding] The ticket does not identify a net-new delta from already shipped and tested behavior. Current source, release notes, public API snapshot, and tests already cover the same ReadShape performance/index/provider diagnostics the story asks developers to add.

Missing examples / edge cases
- The contract gives no concrete before/after example showing what diagnostic information is still missing from the current latest-satellite, PIT, or bridge ReadShape payloads.
- If the intended gap is multi-stage output, provider-native caveats, or additional payload detail, the ticket does not show a source-backed case where the current records and tests are insufficient.

Risky assumptions
- It assumes the branch lacks performance-stage ReadShape records even though DataVaultDiagnostics.cs, the public API snapshot, and tests already expose them.
- It assumes support-bundle and API snapshot work is still future scope even though DataVaultSupportBundleExporter.ExportJson(...) and the approved API snapshot already cover the existing ReadShape model.
- It assumes registry-backed latest-satellite and bridge equivalence still needs to be introduced even though existing unit and integration tests already assert it.

AC / test suggestions
- If the ticket is kept, add one explicit expected JSON or object-model example for the new diagnostic delta that is missing today and show how it differs from the current ReadShape payload.
- Require any revised acceptance criteria to point at the exact new gap beyond the already-covered cases in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs and tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs.

Implementation watchouts
- Developers working from the current ticket text are likely to duplicate or destabilize existing public ReadShape APIs because the ticket describes already-present records as if they do not exist.
- Because the public API snapshot already includes these ReadShape types, any follow-up must be additive to the current snapshot rather than reintroducing or reshaping the existing model.

Non-blocking notes
- .gicket/tickets/06F492C50WM7V2NE0WZB3774XM/description.md has Open Questions set to none; the blocker is stale or duplicative scope, not unresolved questions.

Split recommendations
- No split until PO identifies a real net-new delta. If only documentation wording or a separate telemetry/summary concept remains, ticket that independently from core read-shape diagnostics.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment