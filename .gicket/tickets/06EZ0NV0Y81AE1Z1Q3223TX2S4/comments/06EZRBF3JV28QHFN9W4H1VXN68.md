[gicket-bot] PO-critic review contract

Summary
- Current PO refinement resolves the prior undefined cycle case; the ticket is now bounded, has no open questions, and is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NV0Y81AE1Z1Q3223TX2S4/description.md now contains `## Open Questions` = `none` and Acceptance Criteria naming the concrete invalid hierarchy case `EmployeeReportsTo(Employee, Employee)` where selectors `0->0` or `1->1` must fail while distinct selectors are supported.
- git diff 30730da8..251a03d2 -- .gicket/tickets/06EZ0NV0Y81AE1Z1Q3223TX2S4/description.md shows the PO refinement replaced the vague `unsupported metadata-level cycles` language with the explicit same-participant self-cycle rule and the bounded single-recursive-link hierarchy boundary.
- .gicket/tickets/06EZ0NV0Y81AE1Z1Q3223TX2S4/comments/06EZR9KE97H1XFWP3N3Y6W4928.md marks prior critic-item-1/2/3 as `answered` and ties the rejection case to the existing `DataVaultLinkParticipantMetadata` selector baseline.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs currently exposes only `Hubs`, `Links`, and `Satellites`, and src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs exposes public `DataVaultLinkMetadata.Participants`; this directly supports the contract's additive bridge-metadata requirement against the current modeling baseline.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt currently snapshots public `DataVaultMetadataModel`, `DataVaultLinkMetadata`, `DataVaultLinkParticipantMetadata`, and `ApplyDataVaultMetadata(...)`, so the ticket's public-snapshot-or-internal-surface rule is directly auditable.
- git show --stat --format=fuller 251a03d2 and 4cfdd9af shows the active review branch changes are `.gicket` ticket-state updates only; no implementation diff is mixed into this PO-critic decision surface.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The ticket still assumes declaration-order selectors, or an equivalent deterministic selector, can be added without needing a broader public participant-identity redesign beyond the current `DataVaultLinkParticipantMetadata` baseline.
- The ticket still assumes bridge metadata can be introduced additively without changing default `ApplyDataVaultMetadata()` or save-service behavior before sibling mapping ticket 06EZ0NV7KG94MTMNXMGVRYVW9C lands.

AC / test suggestions
- Exercise the concrete recursive-link matrix in unit tests: `EmployeeReportsTo(Employee, Employee)` with `0->1` and `1->0` accepted, `0->0` and `1->1` rejected.
- Add one regression test proving a metadata model with no bridges still produces the existing hub/link/satellite translation shape unchanged.
- If bridge types become public, update `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` in the same change; otherwise keep the first bridge surface internal and leave the approved snapshot unchanged.

Implementation watchouts
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` currently iterates only hubs, links, and satellites, so bridge validation must stay additive and must not retroactively tighten existing non-bridge translation behavior.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs` currently identifies link participants by list position plus hub reference only; bridge endpoint and hierarchy disambiguation must stay bridge-owned and deterministic, not inferred from repeated hub names alone.
- Keep EF bridge-table projection, provider-specific behavior, and user-facing docs/examples out of this ticket per the persisted split to 06EZ0NV7KG94MTMNXMGVRYVW9C and 06EZ0NVE88WW9PMM04NVAZHRG0.

Non-blocking notes
- The previous PO-critic blocker in comment 06EZQ9VSK42MRP80MHKTBHZSXG was materially addressed by the refinement committed in 251a03d2.
- .gicket/tickets/06EZ0NV0Y81AE1Z1Q3223TX2S4/ticket.json is still `todo` and still carries `critic-needed`, `blocked/dev`, and `blocked/test`, which matches a ticket awaiting the current critic decision.

Split recommendations
- If implementation discovers that stable participant identity or multi-link traversal is required beyond the bounded single-link selector model, stop at the minimal bridge contract and raise a follow-up instead of widening this ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment