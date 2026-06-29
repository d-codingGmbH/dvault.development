[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FGX5QAZSAB0M0W8FW807GQQR/description.md:7-9 sets PO Handoff to ready_for_po_critic, and :53-54 records ## Open Questions as - none.
- .gicket/tickets/06FGX5QAZSAB0M0W8FW807GQQR/events/06FGX6QD0TSN7QAB3J08VR1ZVR.json records this ticket blocks docs ticket 06FGX5S4FTGBE7YQ897BMY1974, and .gicket/tickets/06FGX5S4FTGBE7YQ897BMY1974/ticket.json:3-8 shows that docs ticket remains todo.
- git diff --name-only from merge-base b15916e00e7d7e8302684e1acfeb3bdbb3352590 to HEAD 131a6f3de6049b525fc11838b47c5e4f8e6862bb listed only .gicket/tickets/06FGX5QAZSAB0M0W8FW807GQQR/**; no src/, tests/, or docs/ files are changed on the current ticket branch yet.
- src/DCoding.Data.DVault.Privacy/DataVaultPrivacyCoverageReporter.cs:16-45 and :55-94 already report alias coverage plus none/marker-only/encrypted-payload-capable key-provider posture, matching the baseline the ticket says to preserve.
- src/DCoding.Data.DVault/IDataVaultPersonalDataCoverageProof.cs:1-12 defines the core abstraction, and src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs:53-68 registers the privacy-package proof implementation without making core depend on privacy concretes.
- src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:<redacted> and :<redacted> already distinguish advisory personal-data-privacy-proof-missing, fail-closed personal-data-privacy-coverage-unusable, metadata-only no observable converter wiring, and converter-alias mismatch via the expected encrypted-payload alias check.
- src/DCoding.Data.DVault/DataVaultSupportBundle.cs:12-41 and src/DCoding.Data.DVault/DataVaultSupportBundleExporter.cs:37-49 show support-bundle export already serializes DataVaultDiagnosticsResult under fixed schema version dvault.support-bundle.v1 with camelCase JSON, which matches the ticket's additive compatibility requirement.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyCoverageReporterTests.cs:15-56 covers covered and registered-but-unmapped alias facts, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:461-576 covers advisory no-proof, unregistered alias, failing proof, metadata-only no-converter, and successful DbContext-backed converter coverage.
- CHANGELOG.md:16-24 records that v0.48 already shipped deterministic alias coverage reporting and fail-closed personal-data diagnostics, while docs/package-compatibility.md:34-36, docs/getting-started.md:233-235, docs/production-adoption-checklist.md:37-42, and docs/architecture/dvault-v1-optional-privacy-extension-boundary.md:99-105 all state provider-native encryption remains guidance-only and must not rely on probing.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract fixes required facts and statuses but leaves the exact property names for the new structured diagnostics/support-bundle fields to existing repository conventions.
- The selected-or-active provider guidance fact is expected to hang off existing diagnostics/support-bundle structures without introducing a new artifact schema, but the precise placement is still an implementation choice.

AC / test suggestions
- Add one explicit object-model assertion and one support-bundle JSON assertion for converter-alias mismatch, because .gicket/tickets/06FGX5QAZSAB0M0W8FW807GQQR/description.md:50-51 calls that lane out as the current missing test emphasis.
- Add a deterministic serialization test for the unmanaged/guidance-only provider-boundary fact to prove it does not depend on live-schema or database probing.

Implementation watchouts
- Keep the seam on IDataVaultPersonalDataCoverageProof and do not leak DCoding.Data.DVault.Privacy concrete types into DCoding.Data.DVault public APIs.
- Reuse DataVaultDiagnosticsResult as the authoritative source for support-bundle export instead of adding a second privacy-only export path.
- When analysis is metadata-only, report converter coverage as not observable/unusable rather than covered if no configured DbContext exposes DataVaultEncryptedPayloadValueConverter.
- Preserve dvault.support-bundle.v1 and camelCase additive compatibility; avoid any schema version bump or non-additive JSON reshaping.

Non-blocking notes
- Current branch head 131a6f3de6049b525fc11838b47c5e4f8e6862bb still contains only ticket metadata changes relative to develop; that is normal at the pre-development PO-critic gate.
- .gicket/tickets/06FGX5QAZSAB0M0W8FW807GQQR/comments/06FGYMNQMA8D27KK43X1GQJ7GR.md records that the prior PO-critic run failed on response parsing, and .gicket/tickets/06FGX5QAZSAB0M0W8FW807GQQR/comments/06FH0JY2BJ9HVV17ZPH3VEKDZ4.md records that escalation was cleared; this was automation transport noise, not a ticket-quality problem.

Split recommendations
- No further split recommended; sibling tickets already isolate provider-boundary work in 06FGX5NTKQX87FWCZ2GDDVCXEW, quickstart work in 06FGX5R67T2G0FEGMWE0JBEKJ8, and docs-alignment work in 06FGX5S4FTGBE7YQ897BMY1974.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment