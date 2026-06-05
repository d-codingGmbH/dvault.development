[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the delivery contract is specific, `## Open Questions` is `none`, repository docs/tests already define the support-bundle, fingerprint, and helper-skip boundaries, and the remaining blocker-relation drift is documented as non-blocking ticket metadata cleanup.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZPZZE8VZEBANP5MPN8HH8/description.md contains the authoritative Delivery Contract with 5 acceptance criteria, implementation notes pointing to `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs`, and `## Open Questions` set to `none`.
- docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md states that typed helpers consume exactly one authoritative `dvault.support-bundle.v1`, incompatible or ambiguous bundle input stays on `DMV1960`, fingerprint drift stays on `DMV1961`, and unsupported PIT/bridge facts skip only the affected helper while other helpers continue to generate.
- docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md defines request-bound `readShape` evidence for PIT and bridge helpers, matching the ticket's clarified boundary that PIT/bridge helper generation depends on reviewed support-bundle read-shape facts.
- tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs already covers the static contract baseline: `GeneratesBridgeReadModelsForSupportedManyToManyAndHierarchyShapes` (line 128), `ReportsUnavailableSourceForIncompatibleSupportBundleVersion` (line 527), `ReportsStaleConfiguredFingerprintAndSkipsGeneration` (line 586), `ReportsUnsupportedPitShapeAndKeepsUnrelatedSatelliteGeneration` (line 683), `ReportsUnsupportedBridgeShapeAndKeepsUnrelatedSatelliteGeneration` (line 745), and `GeneratesPitReadModelFromRequestBoundSupportBundleReadShapeAndKeepsSatelliteGeneration` (line 947).
- .gicket/tickets/06F8KZPN02NWFGMRC2Q1PKYKDR/ticket.json shows the stale upstream blocker ticket is already `done`, while `.gicket/relations/DR/H8/06F8KZPN02NWFGMRC2Q1PKYKDR--06F8KZPZZE8VZEBANP5MPN8HH8--blocks.json` still exists; `.gicket/tickets/06F8KZPZZE8VZEBANP5MPN8HH8/comments/06F9AY7WBF9RD7XT5P50B8WAVG.md` records that blocked-by follow-up as `base-terminal-dropped`, and the target ticket file currently has `is-blocked: false`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Keep at least one transition test as a true degrade-and-recover sequence for the same helper family so the acceptance criteria prove both stale-output removal and restoration, not just isolated one-pass outcomes.
- When covering partial PIT or bridge skip behavior, assert both sides of the boundary in the same scenario: the affected helper is removed or skipped with the expected diagnostic while an unrelated supported helper from the same bundle remains generated.

Implementation watchouts
- The existing test file mostly exercises one-pass generator inputs; the freshness story needs successive generator-run evidence so the test must prove stale generated outputs are cleared between runs rather than only comparing separate fresh compilations.
- Stay on the documented diagnostic boundary: fingerprint drift is `DMV1961`, incompatible or schema-version-mismatched support-bundle input is `DMV1960`, and PIT or bridge-specific unsupported shapes should keep using helper-specific skip diagnostics without broadening the contract.

Non-blocking notes
- Downstream task `.gicket/tickets/06F8KZQAWZ7QRGB68KB21C9B0R/ticket.json` remains `todo` and is still blocked by this story via `.gicket/relations/H8/0R/06F8KZPZZE8VZEBANP5MPN8HH8--06F8KZQAWZ7QRGB68KB21C9B0R--blocks.json`, which is consistent with the delivery contract risk section.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment