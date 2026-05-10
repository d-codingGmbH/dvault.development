[gicket-bot] PO-critic review contract

Summary
- The PO refinement closed the earlier blocker gaps; the ticket is now consistent with the existing registry-backed save surface and is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEC7FEXAD069AJNYZW0DRM/description.md:13-16 and 39-42 now explicitly limit IDataVaultLinkMapper<TSource> to links with unique participant hub metadata names, reject repeated same-hub or self-link typed link shapes in v1, and place missing required hub/link/payload-name failures at the existing save-service boundary; description.md:61-62 records ## Open Questions as '- none'.
- .gicket/tickets/06F0MEC7FEXAD069AJNYZW0DRM/comments/06F0Z6RREEJQ2C55Q453EGKH48.md:10-14 marks critic-item-1 through critic-item-4 answered and states that the prior same-hub/self-link ambiguity and validation-ownership ambiguity were both resolved before handoff back to po-critic.
- git log --oneline -- .gicket/tickets/06F0MEC7FEXAD069AJNYZW0DRM shows 499678a89 (handoff po-critic->po), 8c311ba1e (handoff po->po-critic), and HEAD 663f928b6 (current po-critic claim); git diff --word-diff=plain ae8a12507bd15d3c269ad643d79fcf91292e5fec..663f928b6bad856def5aa8487fda6ddba6c1ed7a on the ticket description shows the contract change from ambiguous typed-link support and earlier-validation wording to explicit rejection plus save-service-boundary wording.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:296-313, 540-557, and <redacted> confirm the repository baseline the ticket now matches: registry-backed link operations are keyed by participant hub metadata name, duplicate names are rejected, and link save-plan creation reads required participant values back by participant name in declaration order.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:171-186 and 317-362, src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs:166-176, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs:160-168 confirm that LoadTimestamp and RecordSource stay on DataVaultRegistrySaveRequest, satellites resolve by exact parent reference plus exact satellite name, and link-parent satellites are already a supported repository shape.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:644-715 shows DataVaultLinkMetadata accepts repeated hub endpoints, while src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs:101-130 creates registries with Array.Empty<DataVaultMetadataClrMapping>(); that matches the refined contract's deliberate v1 typed-link rejection boundary and its rule that mapper contracts must not require CLR mappings.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A small API example for null source handling on Map(TSource source), including nullable annotations for reference-type TSource, would make the contract easier to implement consistently.
- A concrete manual link-parent-satellite mapper example would help later dev/test tickets, although the parent-scoped rule itself is already specified.

Risky assumptions
- The contract now requires null-source failures, but the eventual public nullable annotations or generic constraints on Map(TSource source) still need to be pinned in API snapshot/tests.
- Duplicate-name diagnostics and missing-required-name diagnostics occur at different layers in the current save pipeline; tests need to keep those two failure families distinct so the boundary does not drift again.

AC / test suggestions
- Add API snapshot assertions for the nullable annotations on Map(TSource source).
- Add one contract test that proves repeated same-hub/self-link typed link mapping fails through the documented duplicate-name path for the registry-backed link operation shape.
- Add one explicit test that distinguishes constructor-time duplicate-name failure from save-plan-time missing-required-value failure for satellites and links.

Implementation watchouts
- Do not reintroduce same-hub typed-link support without a separate participant identity shape; the current registry-backed link operation still collapses by participant hub metadata name.
- Keep LoadTimestamp and RecordSource outside the mapper interfaces and on DataVaultRegistrySaveRequest or DataVaultRegistryBulkSaveRequest.
- Keep exact StringComparer.Ordinal logical-name matching and parent-scoped satellite resolution; do not make CLR mappings a hidden prerequisite for mapper use.

Non-blocking notes
- The refined contract stays additive and continues to align with the already-split follow-up tickets for typed save helpers and typed read projections.

Split recommendations
- No additional split is needed for this v1 mapper-contract ticket.
- Keep any future same-hub or self-link typed-link participant-role/ordinal/alias work on a separate follow-up ticket, as the refined contract already scopes that out of v1.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment