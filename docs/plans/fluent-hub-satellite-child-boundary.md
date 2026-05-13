# Fluent hub and satellite child boundary addendum

Status: authoritative child addendum
Child ticket: 06F0ME9PM8KXH3VP59TQR0ETA8
Parent ticket: 06F0ME976PM5455JK04S6GPNNW
Parent contract: docs/plans/fluent-code-first-api-contract.md
Driving-key contract: docs/plans/multi-active-satellite-driving-key-contract.md

## Boundary

- Ticket 06F0ME976PM5455JK04S6GPNNW and docs/plans/fluent-code-first-api-contract.md are the authoritative boundary for this child.
- This child owns hub and hub-parent satellite projection only. Link declarations remain on 06F0MEA1FF743S14XQW02H4A3W and parity coverage remains on 06F0MEAD1BAA5QEVM3F9QJA38G.

## Required scope

- Capture repeated direct scalar `BusinessKey(...)`, `Payload(...)`, and `DrivingKey(...)` selectors in declaration order.
- Treat `DrivingKey(...)` as the only fluent multi-active opt-in for hub-parent satellites and project the covered shape into `DataVaultSatelliteMetadata` driving-key names.
- Reject unsupported selector shapes for `BusinessKey(...)`, `Payload(...)`, and `DrivingKey(...)` with actionable repeated-single-member guidance.

## Acceptance additions

- One or more `DrivingKey(...)` calls produce the covered multi-active metadata shape defined by docs/plans/multi-active-satellite-driving-key-contract.md and remain schema-equivalent to the metadata-first baseline for the covered scenario.
- Link declarations, link-parent satellites, save-service behavior, and typed save helpers stay out of scope for this child.
