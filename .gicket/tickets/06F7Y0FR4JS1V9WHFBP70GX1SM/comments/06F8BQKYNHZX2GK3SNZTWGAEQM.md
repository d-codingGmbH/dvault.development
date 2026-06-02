[gicket-bot] PO-critic review contract

Summary
- Approve: this tracking epic has done child coverage for contract, implementation, tests, and docs on the integrated repository baseline; only archived-duplicate and stale-relation noise remains.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F7Y0FR4JS1V9WHFBP70GX1SM/description.md contains '## Open Questions' -> 'none', so the persisted parent delivery contract has no unresolved open questions.
- git log --oneline --decorate --all --grep '06F7Y0FZXX5J0G7G15681HVEBR|06F7Y0GT7A5QT77TADMRZBVYN8|06F7Y0H83H29E1D9K5RK3K7Y9W|06F7Y0HJ1ZPY7ND9N8RVS92H4C|06F7Y0HZKHBHMYX9EYDYFRYXZ0|06F7Y0GFY7TP3V4B76JB759KB0' -n 30 showed integrated commits 8bc0d6ab8, db1ef4504, 27e4d3020, 0772d4ab1, f089c643a, plus duplicate-close commit 39a5722d5.
- The archived duplicate is directly evidenced by .gicket/relations/B0/BR/06F7Y0GFY7TP3V4B76JB759KB0--06F7Y0FZXX5J0G7G15681HVEBR--duplicates.json and .gicket/archive/06F7Y0GFY7TP3V4B76JB759KB0/comments/06F8ARB2B0HAWYVEJZ9H0XZY8W.md, whose text says the requested read-plan/read-shape work already landed under 06F7Y0FZXX5J0G7G15681HVEBR.
- git show --stat --oneline 39a5722d5 reported '[06F7Y0GFY7TP3V4B76JB759KB0] close duplicate ticket' and moved 06F7Y0GFY7TP3V4B76JB759KB0 from .gicket/tickets/ to .gicket/archive/.
- docs/releases/v0.25.0.md (intended release date 2026-06-02) documents the request-bound IDataVaultReadDiagnosticsService.Analyze(...) / DataVaultDiagnosticsResult.ReadShape baseline, explicit redaction rules, the typed PIT/bridge helper surface, and required maximumDepth for hierarchy helpers.
- README.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/performance-profiles.md, and docs/production-adoption-checklist.md all reference the v0.25.0 ReadShape and support-bundle-driven satellite/PIT/bridge helper baseline rather than the old satellite-only boundary.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs contains ReadDiagnosticsPopulateReadShapeForExplicitRegistryPitAndBridgeRequests (line 71), ReadDiagnosticsPopulateTupleAwarePitReadShapeForMultiActiveRequests (271), ReadPlanExplainContractDocumentNamesAuthoritativeSurfaceAndRedactionBoundary (330), and SupportBundleSerializesReadPlanExplainShapesWithoutRequestValues (366).
- tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs contains GeneratesBridgeReadModelsForSupportedManyToManyAndHierarchyShapes (128), GeneratedBridgeHelpersDelegateThroughRuntimeReadBoundaryWithEquivalentRequestsAndProjection (211), and GeneratesPitReadModelFromRequestBoundSupportBundleReadShapeAndKeepsSatelliteGeneration (727); src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs also shows PIT delegation through ReadPitRowsAsync and hierarchy bridge maximumDepth handling at the rg hits around lines 2007 and <redacted>.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs exposes the authoritative read diagnostics surface directly: rg hits show DataVaultReadShapeKind at line 459, DataVaultDiagnosticsResult.ReadStrategy / ReadShape at lines 644-655, and IDataVaultReadDiagnosticsService at line 885.
- Deferred dependency wait: Child ticket '06F7Y0GFY7TP3V4B76JB759KB0' is 'missing', not 'done'.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No end-to-end consumer sample is tracked in this epic for exporting representative readShape diagnostics and compiling generated PIT/bridge helpers; the parent contract already treats that as follow-up rather than required handoff scope.
- docs/plans/typed-read-model-generator-contract.md is still present as historical satellite-only context; it is marked superseded, but a stronger banner could further reduce reader confusion.

Risky assumptions
- Historical blocks relations into the epic will not be interpreted as live blockers during later workflow steps.

AC / test suggestions
- Optional follow-up only: add a separate sample/docs ticket that demonstrates exporting representative readShape diagnostics and compiling generated PIT/bridge helpers end to end.
- Optional follow-up only: add explicit closure-evidence or relation-audit notes that archived duplicate 06F7Y0GFY7TP3V4B76JB759KB0 is satisfied by 06F7Y0FZXX5J0G7G15681HVEBR so future closure audits do not depend on inferring archive semantics.

Implementation watchouts
- git diff --name-only develop...HEAD shows this review branch differs from develop only in .gicket metadata/comments, so developer handoff should evaluate the already-integrated repository baseline rather than expect fresh feature-code deltas on this branch.
- Do not treat docs/plans/typed-read-model-generator-contract.md as the active PIT/bridge contract; the active boundaries are docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md, docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md, and docs/releases/v0.25.0.md.

Non-blocking notes
- Historical relation files still exist at .gicket/relations/Z0/SM/06F7Y0HZKHBHMYX9EYDYFRYXZ0--06F7Y0FR4JS1V9WHFBP70GX1SM--blocks.json and .gicket/relations/DR/SM/06F7Y0F650KM61BQXMEQPZ86DR--06F7Y0FR4JS1V9WHFBP70GX1SM--blocks.json.
- The latest parent comments under .gicket/tickets/06F7Y0FR4JS1V9WHFBP70GX1SM/comments/ are operational bot lease/run-report entries; one records a prior PO-critic parse failure, but that is not a product-scope blocker.
- Closure is not terminal yet because dependency/child work remains open; preserving downstream blocked labels must prevent immediate role execution after handoff.

Split recommendations
- No new split is required; the epic is already decomposed into contract, implementation, and documentation children with done repository evidence.
- Keep any future consumer sample, relation-cleanup, raw-SQL/plan capture, or support-bundle transport automation work in separate additive follow-up tickets rather than reopening this epic.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment