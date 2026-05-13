# Fluent link child boundary addendum

Status: authoritative child addendum
Child ticket: 06F0MEA1FF743S14XQW02H4A3W
Parent ticket: 06F0ME976PM5455JK04S6GPNNW
Parent contract: docs/plans/fluent-code-first-api-contract.md

## Boundary

- Ticket 06F0ME976PM5455JK04S6GPNNW and docs/plans/fluent-code-first-api-contract.md are the authoritative boundary for this child.
- This child owns only link and relationship projection. Hub-parent satellite projection, including `DrivingKey(...)` multi-active opt-in capture and validation, remains on 06F0ME9PM8KXH3VP59TQR0ETA8. Parity coverage remains on 06F0MEAD1BAA5QEVM3F9QJA38G.

## Required scope

- Declare links only from previously configured hubs.
- Support explicit relationship names and the derived default when no name is supplied.
- Require at least two participants, preserve declaration-order participant ordering, and fail clearly when participant hubs are missing, ambiguous, or otherwise unsupported.
- Project to `DataVaultLinkMetadata` and the existing provider-aware EF shared-type translator.

## Scope guardrails

- Hub business-key capture, satellite payload capture, `DrivingKey(...)` selector capture, and save-service behavior stay out of scope for this child.
