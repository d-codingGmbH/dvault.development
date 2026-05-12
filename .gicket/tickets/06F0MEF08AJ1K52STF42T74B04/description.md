<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the repo and ticket state: this ticket remains the import/projection child under story 06F0MEE0NC2009J73PP0ATE6YW, is correctly blocked by completed schema/parser tickets 06F0MEE8T9PKPKQH8EPWNQ2CRW and 06F0MEEGJE9QCHC8YN4FEXYX10, and still correctly blocks export 06F0MEFHKF04B746X7GJKRVT04 and drift 06F0MEFX5M9V9SA25N76CPGT4M; no new split, relation change, or planning document was needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Treat 06F0MEE8T9PKPKQH8EPWNQ2CRW and 06F0MEEGJE9QCHC8YN4FEXYX10 as completed prerequisites and authoritative baselines for dvault.model.v1 shape, strict JSON parsing, and base validation diagnostics; this ticket does not reopen schemaVersion policy, YAML boundary, or validation taxonomy.
- Current repository evidence already provides the reusable registry and EF projection path through DataVaultMetadataRegistry, AddDVault(...UseMetadataModel/UseMetadataRegistry...), UseDataVaultMetadata(...), and ApplyDataVaultMetadata(...); this ticket should connect imported artifacts to that existing path rather than introduce a parallel projection stack.
- Current branch evidence already includes an internal DataVaultModelArtifactParser that can build a DataVaultMetadataModel and DataVaultMetadataRegistry from dvault.model.v1 input; this ticket owns turning that imported-artifact result into a supported import-to-registry/import-to-EF projection workflow.
- Code-First parity is bounded to the currently implemented fluent baseline documented in the repo: hubs, ordered business keys, hub-parent satellites, multi-active driving keys, and ordered hub links. Link-parent satellites, PIT, bridges, and role-bearing recursive link cases should compare against metadata-first baseline instead of reopening Code-First scope.
- No new child tickets, relation writes, or planning attachments were materialized during refinement. Existing dependency relations remain consistent with the intended delivery order.

### Scope In
- Add the additive imported-model entry surface that accepts a dvault.model.v1 JSON artifact plus optional logical source path and returns structured diagnostics together with a usable DataVaultMetadataModel and DataVaultMetadataRegistry result.
- Connect successful imports to the existing registry-backed and model-builder-backed EF metadata flow so callers can reuse AddDVault, UseDataVaultMetadata, and ApplyDataVaultMetadata without manually re-declaring the imported model.
- Carry imported loadTimestampStorage into provider capability selection so built-in provider profiles preserve the current provider-aware timestamp and index behavior when projection runs from an imported artifact.
- Map registry-build and EF projection failures back to the originating logical declaration and artifact path or JSON Pointer rather than surfacing root-level failures only.
- Add parity coverage for the shared imported-model/code-first/metadata-first subset and metadata-first parity coverage for advanced imported-model shapes that current Code-First APIs do not expose.

### Scope Out
- Redefining dvault.model.v1 fields, tokens, defaults, or validation categories already delivered by 06F0MEE8T9PKPKQH8EPWNQ2CRW and 06F0MEEGJE9QCHC8YN4FEXYX10.
- YAML ingestion work owned by 06F0MEERJ7D5Q4WYBQAJD3GFVC.
- Export tooling, drift reporting, or governance documentation owned by 06F0MEFHKF04B746X7GJKRVT04, 06F0MEFX5M9V9SA25N76CPGT4M, and 06F0MEGAGJCEHQ8QRHGH8W7804.
- New read-service APIs or runtime model mutation beyond imported-model-to-registry and imported-model-to-EF projection.
- Expanding the public Code-First surface to cover link-parent satellites, PIT, bridges, or role-bearing recursive link declarations.
- Provider-specific translator behavior outside the existing provider capability profile mechanism.

## Acceptance Criteria
- A valid dvault.model.v1 artifact can be consumed through a supported import surface that yields a DataVaultMetadataModel and DataVaultMetadataRegistry usable by existing AddDVault, UseDataVaultMetadata, and ApplyDataVaultMetadata flows without duplicate manual metadata declarations.
- Imported loadTimestampStorage values provider-default, iso-8601-utc-text, and utc-ticks are honored through the existing built-in provider capability profiles, and comparable imported-model projection keeps the same provider-aware timestamp storage and index behavior as the established metadata-first baseline.
- For the currently shared surface of hubs, ordered business keys, hub-parent satellites, multi-active driving keys, and ordered hub links, imported-model EF projection matches both metadata-first and Code-First relational shape across the built-in provider profile matrix already exercised by the repository.
- For shapes outside the current Code-First surface, including link-parent satellites, PIT metadata, bridges, and role-bearing recursive link scenarios, imported-model projection matches the metadata-first baseline or uses a narrow additive model-first adapter permitted by the schema contract without expanding Code-First scope in this ticket.
- When import-to-registry or import-to-EF projection fails after JSON parsing, diagnostics identify the failing logical declaration and the originating artifact path or JSON Pointer so the caller can trace the source model element that caused the issue.
- Imported-model projection preserves the existing authoritative-source conflict behavior when combined with explicit metadata-model or registry configuration on the same EF model or DbContext options path.

## Definition of Done
- Supported import entry points exist for dvault.model.v1 artifacts and are covered by unit tests for successful registry/model creation and for post-parse projection failures.
- Imported registries can drive both model-builder projection and DbContext registry opt-in through the existing DVault registration/projection path without bypassing metadata-source fingerprint and conflict behavior.
- Parity tests cover the shared Code-First subset against metadata-first and imported-model baselines, and separate tests cover advanced imported-model shapes against metadata-first baseline where Code-First has no current surface.
- Relevant DVault test suites for the new import/projection path pass in the ticket branch, including provider-profile-sensitive projection checks where the repository already has matrix coverage.
- The implementation remains additive and keeps export, drift, governance docs, YAML, and read-service work on their existing tickets.

## Implementation Notes
- Build on the existing internal parser baseline instead of duplicating schema parsing or semantic validation logic. The new work should wrap or expose the imported result in a way that fits the current public registry and EF projection architecture.
- Populate the imported registry with provider capability profiles that reflect the artifact's loadTimestampStorage choice so existing registry-based projection can select the correct per-provider storage shape without a second manual override.
- Route EF projection through the existing ApplyDataVaultMetadata(DataVaultMetadataRegistry) or equivalent registry-backed path so metadata-source annotations, fingerprinting, and source-conflict handling stay consistent with current metadata-first behavior.
- Use existing repository parity-test patterns as the baseline: imported-model versus metadata-first versus Code-First only for the shared fluent surface, and imported-model versus metadata-first for link-parent satellite, PIT, bridge, and recursive-role cases.
- Keep any extra model-first-only adapter state narrow and additive where current public metadata abstractions do not retain enough information, especially for role-bearing recursive links and hierarchy bridge endpoint binding. Failure mapping should still point back to the source artifact path for the offending declaration.

## Open Questions
- none

## Follow-Up Questions
- After this ticket lands, should 06F0MEGAGJCEHQ8QRHGH8W7804 document the new imported-model entry point and the recommended choice between model-first, metadata-first, and Code-First flows?
- Should downstream export and drift tickets 06F0MEFHKF04B746X7GJKRVT04 and 06F0MEFX5M9V9SA25N76CPGT4M consume the same public import result surface directly so artifact normalization and parity logic stay centralized?

## Risks
- If imported loadTimestampStorage is not carried into registry provider profiles, imported-model projection can silently diverge from metadata-first and Code-First provider behavior even when the logical model matches.
- If post-parse mapping and translator failures are surfaced only as generic metadata exceptions, the ticket's source-path diagnostic requirement will not be met and imported artifacts will be hard to debug.
- Recursive-role and hierarchy bridge cases remain sensitive because current public link metadata does not carry participant roles; imported-model projection must preserve that extra binding information narrowly enough to avoid collapsing distinct recursive participants into the same EF shape.

## Split Recommendations
- No new split is recommended. The remaining work is already bounded once schema/parser/YAML stay on their completed sibling tickets and export/drift/governance remain on their existing downstream tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Connect imported model-first artifacts to the registry and EF metadata projection pipeline used by v0.6 Code-First models.

## Scope In

- Conversion from imported model document to registry entries.
- EF metadata projection through existing provider capability profiles.
- Tests comparing imported model, Code-First model, and metadata-first model equivalence.

## Scope Out

- Export tooling.
- Read service implementation.

## Acceptance Criteria

- Imported models can drive schema projection without duplicate manual metadata declarations.
- Existing provider-aware timestamp/index behavior still applies.
- Projection failures identify the source model path that caused the issue.