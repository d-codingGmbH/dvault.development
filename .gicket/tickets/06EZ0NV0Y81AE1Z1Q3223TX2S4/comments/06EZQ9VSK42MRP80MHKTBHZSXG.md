[gicket-bot] PO-critic review contract

Summary
- The ticket is largely well-scoped, but the rejected bridge-cycle case is still too vague for deterministic developer/test handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket returned ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 at revision 06EZQ8N7EBJP4GBGXFXQ5GSFD0 with `PO Handoff` = `ready_for_po_critic` and `## Open Questions` = `none`.
- gicket-read-ticket-comments returned the PO refinement comment and handoff comments stating EF mapping work was split to 06EZ0NV7KG94MTMNXMGVRYVW9C and docs/examples work was split to 06EZ0NVE88WW9PMM04NVAZHRG0.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs` currently exposes only `Hubs`, `Links`, and `Satellites`, confirming bridge metadata would be an additive extension to the existing aggregate metadata model rather than an already-defined bridge subsystem.
- `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` currently includes public `DataVaultMetadataModel`, `DataVaultLinkMetadata`, `DataVaultLinkParticipantMetadata`, and `ApplyDataVaultMetadata(...)`, so the ticket's public-snapshot-or-internal-surface rule is directly auditable against a real baseline.
- `git show --stat --format=fuller 8194713b81e0a152988367f42b7b209fef26cf0a` shows the latest branch-history evidence on this review surface is the PO-critic lease-claim commit touching `.gicket` artifacts and ticket state only, not an implementation change.

Blocking findings
- The contract requires bridge validation to reject an `unsupported metadata-level cycle`, but neither the persisted contract nor the observed repository baseline defines one concrete cycle shape that must fail. Because the planned bridge model is only anchored to the existing hub/link/satellite aggregate in `src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs`, the negative cycle case is not specific enough to make the required DoD tests objectively auditable.

Required PO actions
- Add at least one concrete invalid-cycle example to the acceptance criteria or implementation notes and name the exact metadata pattern that must be rejected.
- State the boundary between the one supported bounded hierarchy traversal and the first disallowed cyclical/traversal shape so developers can write deterministic negative tests without inventing policy.

Open issues ledger
- critic-item-1 [required-po-action] Add at least one concrete invalid-cycle example to the acceptance criteria or implementation notes and name the exact metadata pattern that must be rejected.
- critic-item-2 [required-po-action] State the boundary between the one supported bounded hierarchy traversal and the first disallowed cyclical/traversal shape so developers can write deterministic negative tests without inventing policy.
- critic-item-3 [blocking-finding] The contract requires bridge validation to reject an `unsupported metadata-level cycle`, but neither the persisted contract nor the observed repository baseline defines one concrete cycle shape that must fail. Because the planned bridge model is only anchored to the existing hub/link/satellite aggregate in `src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs`, the negative cycle case is not specific enough to make the required DoD tests objectively auditable.

Missing examples / edge cases
- A concrete recursive-link example showing how ancestor-side and descendant-side selectors disambiguate a hierarchy when the same hub type appears on both sides of the traversed link is still missing.
- A concrete ambiguous-endpoint example is missing for a traversed link that repeats the same hub more than once or otherwise requires selector-based disambiguation.

Risky assumptions
- The ticket assumes declaration-order selectors or an equivalent selector will be enough without later needing a broader participant-identity surface beyond the current public `DataVaultLinkParticipantMetadata` baseline in the API snapshot.
- The ticket assumes bridge metadata can be added without forcing behavioral changes in `ApplyDataVaultMetadata()` before the separate EF-mapping ticket 06EZ0NV7KG94MTMNXMGVRYVW9C lands.

AC / test suggestions
- Add one explicit rejected-case acceptance example for the exact cycle shape that must fail.
- Anchor the required unit tests with one concrete recursive-hierarchy success example and one duplicate-participant ambiguity failure example.

Implementation watchouts
- The current public API snapshot shows `DataVaultMetadataModel` with the existing hubs/links/satellites aggregate and public `ApplyDataVaultMetadata(...)`; any public bridge surface must stay additive or update `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` in the same delivery.
- The persisted contract explicitly says bridge validation must not retroactively tighten current non-bridge behavior such as satellite metadata translation cases that allow unresolved parent names.

Non-blocking notes
- `## Open Questions` is `none`, so the ticket passes the explicit open-question gate once the cycle rule is clarified.
- The sibling-ticket split is already clear and consistent across the persisted contract and comment history: metadata/validation stay here, EF mapping is 06EZ0NV7KG94MTMNXMGVRYVW9C, and docs/examples are 06EZ0NVE88WW9PMM04NVAZHRG0.

Split recommendations
- If making the cycle rule concrete would require bridge composition semantics or a broader public participant-identity redesign, keep this ticket on the minimal bridge metadata contract and open the follow-up already anticipated in `## Split Recommendations`.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment