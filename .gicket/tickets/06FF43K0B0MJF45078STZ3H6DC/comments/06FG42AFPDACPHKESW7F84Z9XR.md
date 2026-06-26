[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FF43K0B0MJF45078STZ3H6DC/description.md contains Open Questions followed by '- none' and acceptance criteria that explicitly name personal-data-privacy-proof-missing, personal-data-privacy-coverage-unusable, exact encryptedPayloadAlias matching, and DataVaultEncryptedPayloadValueConverter evidence.
- git log --oneline develop..HEAD --max-count=20 shows only ticket-lifecycle commits on this branch (9109bc1a74, 66b247732c, 6e30dee476, 47075b0335), and git diff --name-only develop...HEAD shows only .gicket/tickets/06FF43K0B0MJF45078STZ3H6DC/** changed; no src/, tests/, or docs/ files were modified on this branch.
- src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs directly implements the warning/error split: no proof yields personal-data-privacy-proof-missing, configured-but-unusable proof yields personal-data-privacy-coverage-unusable, and usable coverage requires matching DataVaultEncryptedPayloadValueConverter alias wiring on the marked payload field.
- src/DCoding.Data.DVault.Privacy/DataVaultPrivacyPersonalDataCoverageProof.cs and src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadValueConverter.cs show exact alias registration and caller-owned key-provider checks that fail closed.
- src/DCoding.Data.DVault.Privacy/DataVaultPrivacyCoverageReporter.cs plus src/DCoding.Data.DVault.Privacy/DataVaultPrivacyCoverageReport.cs show the coverage report is model-only, deterministic, and redaction-safe: it sorts aliases/properties, reports covered vs registered-but-unmapped, and records key-provider posture without database access.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs includes direct unit evidence for the contract: AnalyzeReportsPersonalDataMarkersAsAdvisoryWithoutPrivacyProof, AnalyzeFailsClosedForMarkedPersonalDataWithUnregisteredEncryptedPayloadAlias, AnalyzeFailsClosedWhenPersonalDataPrivacyProofReturnsNoEvaluation, AnalyzeFailsClosedForMarkedPersonalDataWithoutFieldLevelEncryptedPayloadConverter, and AnalyzeDbContextAcceptsMarkedPersonalDataWithFieldLevelEncryptedPayloadConverter.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyCoverageReporterTests.cs asserts stable coverage output over covered and registered-but-unmapped aliases and verifies no conversion calls are needed for reporting.
- docs/architecture/dvault-v1-optional-privacy-extension-boundary.md, docs/plans/dvault-model-v1-schema-contract.md, and docs/production-adoption-checklist.md all align with the ticket scope: additive personalData[].encryptedPayloadAlias metadata, opt-in provider-neutral privacy proof, no provider-native encryption claims, and model-only coverage review.
- Relation files .gicket/relations/DC/74/06FF43K0B0MJF45078STZ3H6DC--06FF43M7AE9DN3K1YXBPB1R574--parentOf.json, .gicket/relations/DC/S8/06FF43K0B0MJF45078STZ3H6DC--06FF43MQ3AXXK2S5TK65X4Y9S8--parentOf.json, .gicket/relations/DC/M4/06FF43K0B0MJF45078STZ3H6DC--06FF43NAAR3WXH759TVG2RS2M4--parentOf.json, .gicket/relations/DC/GW/06FF43K0B0MJF45078STZ3H6DC--06FF43NJES6S8NBZVWR4FGHWGW--parentOf.json, .gicket/relations/DC/A4/06FF43K0B0MJF45078STZ3H6DC--06FF43PCN26C70DXX326B9VYA4--parentOf.json, .gicket/relations/DC/00/06FF43K0B0MJF45078STZ3H6DC--06FF43QFBQ185N3WPRFD544H00--parentOf.json, and .gicket/relations/DC/A0/06FF43K0B0MJF45078STZ3H6DC--06FF43REXXX4R9WKNCKDXP4RA0--relates.json confirm the persisted six parentOf links and one relates link referenced by the contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- If downstream relies on exact diagnostic text stability, keep explicit examples for the remaining unusable-coverage variants already implied by source: missing EF satellite entity, missing payload property, and wrong converter alias wiring.
- If a machine-readable preflight artifact is later wanted, that export shape is still only a follow-up question, not part of this parent contract.

Risky assumptions
- Downstream routing still intends new developer work on this parent ticket even though the branch diff is metadata-only and the six linked child implementation tickets are already done.
- The existing child tickets fully cover parser, registry, EF translation, and provider follow-through ownership without reopening the parent contract.
- Developers will continue to treat personalData and AddDVaultPrivacy(...) as opt-in preflight evidence, not as compliance or automatic-encryption guarantees.

AC / test suggestions
- Keep the diagnostics matrix explicit for all fail-closed personal-data-privacy-coverage-unusable variants so future refactors do not blur the warning-vs-error boundary.
- Keep one regression test that proves DataVaultPrivacyCoverageReporter.Analyze(...) remains model-only and deterministic across alias ordering and multiple mapped properties for the same alias.

Implementation watchouts
- This branch currently changes only ticket metadata under .gicket/...; any new implementation should be justified against the already-ratified contract rather than reopening parent scope.
- Converter coverage is only provable from Analyze(DbContext) or EF-model inspection; metadata-only analysis intentionally remains insufficient once a privacy proof is configured.
- Alias matching is exact and fail-closed: coverage only counts when the marked field and observed DataVaultEncryptedPayloadValueConverter share the same encryptedPayloadAlias.

Non-blocking notes
- The ticket contract is concrete and has no unresolved Open Questions, so the PO-quality gate itself is satisfied.
- Current ticket comments are operational bot claim/refinement/handover comments; I did not find a newer human comment or closure-evidence amendment that reopens scope.
- Because the linked child tickets are already done, downstream may want to confirm whether the remaining work is true implementation, residual routing, or eventual closure bookkeeping.

Split recommendations
- No new split is needed from a PO-critic perspective; the persisted six parentOf tickets and one relates link already cover the decomposition described in the contract.
- If provider-native encryption or operational lifecycle behavior resurfaces, keep it in separate provider- or workflow-specific tickets instead of widening this parent story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment