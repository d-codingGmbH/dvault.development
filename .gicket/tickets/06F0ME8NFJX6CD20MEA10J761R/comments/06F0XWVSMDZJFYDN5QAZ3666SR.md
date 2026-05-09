[gicket-bot] PO-critic review contract

Summary
- The ticket is internally consistent, grounded in current repository and ticket state, and has no unresolved PO questions; it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0ME8NFJX6CD20MEA10J761R/description.md contains `PO Handoff` decision `ready_for_po_critic` and `## Open Questions` set to `none`.
- .gicket/relations/1R/NW/06F0ME8NFJX6CD20MEA10J761R--06F0ME976PM5455JK04S6GPNNW--parentOf.json, .gicket/relations/1R/A8/06F0ME8NFJX6CD20MEA10J761R--06F0ME9PM8KXH3VP59TQR0ETA8--parentOf.json, .gicket/relations/1R/3W/06F0ME8NFJX6CD20MEA10J761R--06F0MEA1FF743S14XQW02H4A3W--parentOf.json, and .gicket/relations/1R/8G/06F0ME8NFJX6CD20MEA10J761R--06F0MEAD1BAA5QEVM3F9QJA38G--parentOf.json persist the live story-to-child `parentOf` links referenced by the contract.
- docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md exists and matches the bounded story scope: additive `ModelBuilder.ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>)`, hub-parent satellites only, `DrivingKey(...)` as the only fluent multi-active opt-in, and link-parent satellites kept out of v1.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs exposes the root-namespace `ApplyDataVaultMetadata(this ModelBuilder, Action<DataVaultCodeFirstModelBuilder>)` overload, and src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs builds fluent declarations into `DataVaultMetadataModel` before translation.
- src/DCoding.Data.DVault/DataVaultCodeFirstHubBuilder.cs, DataVaultCodeFirstSatelliteBuilder.cs, DataVaultCodeFirstSelector.cs, and DataVaultCodeFirstModelBuilder.cs enforce the contract behaviors the story calls out: direct readable scalar selectors, duplicate logical-member rejection, declaration ordering, previously configured hub participants, at least two link participants, and rejection of repeated same-hub link participants.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstSchemaParityTests.cs, tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt cover the fluent API surface, actionable validation failures, hub/satellite/link translation parity, built-in provider-profile parity, SQLite parity, and public API exposure.
- `git log --oneline --decorate -n 8 ticket/06F0ME8NFJX6CD20MEA10J761R-story-add-fluent-ef-code-first-modeling-api` shows the story branch sitting on top of develop commits 086e6e962, ce62043eb, c5400b329, and b09a17277 for the design and implementation children, while `git diff --name-only develop..ticket/06F0ME8NFJX6CD20MEA10J761R-story-add-fluent-ef-code-first-modeling-api` lists only `.gicket/tickets/06F0ME8NFJX6CD20MEA10J761R/{ticket.json,description.md,comments/*,events/*}`, confirming the story branch is carrying refinement/handoff state rather than reopening implementation scope.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract intentionally does not include fluent link-parent satellite examples; that shape remains metadata-first-only in v1.
- The contract intentionally does not include same-hub or recursive self-link examples; those require a follow-up surface with participant-role or alias support.
- The contract intentionally does not include a code-first hub-name override example; non-CLR logical hub names still rely on the metadata-first path.

Risky assumptions
- Consumers who need non-CLR logical hub names can tolerate the temporary metadata-first fallback until a dedicated follow-up ticket exists.
- Future work will not silently widen selector parsing beyond direct readable scalar members without updating both the contract and parity coverage.
- Any future need for link-parent satellites or same-hub/recursive participant roles will be split into dedicated tickets instead of being added implicitly under this story.

AC / test suggestions
- Keep the AC language tied to the existing verification surfaces already present in the repo: `DataVaultCodeFirstMetadataTranslationTests`, `DataVaultCodeFirstLinkTests`, `DataVaultCodeFirstSchemaParityTests`, and `Integration/SqliteDataVaultSchemaTests`.
- Retain the explicit built-in provider matrix naming in the ticket wording rather than collapsing parity expectations to a generic provider statement.

Implementation watchouts
- Do not bypass `DataVaultMetadataModel` when translating the fluent API; the current root overload in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` depends on flowing through the metadata-first translator path.
- Do not widen v1 to repeated same-hub link participants or link-parent satellites without a separate ticket; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs` already treats those shapes as unsupported.
- Preserve declaration order and duplicate logical-member validation semantics already enforced by `src/DCoding.Data.DVault/DataVaultCodeFirstHubBuilder.cs` and `src/DCoding.Data.DVault/DataVaultCodeFirstSelector.cs`.

Non-blocking notes
- The latest substantive story comment is `.gicket/tickets/06F0ME8NFJX6CD20MEA10J761R/comments/06F0XV0Q0Q0RAW7XSSB8S3G1NC.md`, which records `decision: ready_for_po_critic`; later comments on this ticket are lease and orchestration bookkeeping.
- The story is still unassigned in `.gicket/tickets/06F0ME8NFJX6CD20MEA10J761R/ticket.json`, but no ticket-level evidence suggests that is blocking refinement completeness.

Split recommendations
- No additional split is needed for this story; keep the existing parentOf structure to 06F0ME976PM5455JK04S6GPNNW, 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G.
- If scope later expands to hub-name overrides, link-parent satellites, or same-hub/recursive participant roles, create dedicated follow-up tickets instead of reopening this bounded story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment