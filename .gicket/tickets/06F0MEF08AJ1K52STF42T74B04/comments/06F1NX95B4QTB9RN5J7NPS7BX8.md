[gicket-bot] PO refinement contract

Summary
- Verified the repo and ticket state: this ticket remains the import/projection child under story 06F0MEE0NC2009J73PP0ATE6YW, is correctly blocked by completed schema/parser tickets 06F0MEE8T9PKPKQH8EPWNQ2CRW and 06F0MEEGJE9QCHC8YN4FEXYX10, and still correctly blocks export 06F0MEFHKF04B746X7GJKRVT04 and drift 06F0MEFX5M9V9SA25N76CPGT4M; no new split, relation change, or planning document was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Treat 06F0MEE8T9PKPKQH8EPWNQ2CRW and 06F0MEEGJE9QCHC8YN4FEXYX10 as completed prerequisites and authoritative baselines for dvault.model.v1 shape, strict JSON parsing, and base validation diagnostics; this ticket does not reopen schemaVersion policy, YAML boundary, or validation taxonomy.
- Current repository evidence already provides the reusable registry and EF projection path through DataVaultMetadataRegistry, AddDVault(...UseMetadataModel/UseMetadataRegistry...), UseDataVaultMetadata(...), and ApplyDataVaultMetadata(...); this ticket should connect imported artifacts to that existing path rather than introduce a parallel projection stack.
- Current branch evidence already includes an internal DataVaultModelArtifactParser that can build a DataVaultMetadataModel and DataVaultMetadataRegistry from dvault.model.v1 input; this ticket owns turning that imported-artifact result into a supported import-to-registry/import-to-EF projection workflow.
- Code-First parity is bounded to the currently implemented fluent baseline documented in the repo: hubs, ordered business keys, hub-parent satellites, multi-active driving keys, and ordered hub links. Link-parent satellites, PIT, bridges, and role-bearing recursive link cases should compare against metadata-first baseline instead of reopening Code-First scope.
- No new child tickets, relation writes, or planning attachments were materialized during refinement. Existing dependency relations remain consistent with the intended delivery order.

Scope In
- Add the additive imported-model entry surface that accepts a dvault.model.v1 JSON artifact plus optional logical source path and returns structured diagnostics together with a usable DataVaultMetadataModel and DataVaultMetadataRegistry result.
- Connect successful imports to the existing registry-backed and model-builder-backed EF metadata flow so callers can reuse AddDVault, UseDataVaultMetadata, and ApplyDataVaultMetadata without manually re-declaring the imported model.
- Carry imported loadTimestampStorage into provider capability selection so built-in provider profiles preserve the current provider-aware timestamp and index behavior when projection runs from an imported artifact.
- Map registry-build and EF projection failures back to the originating logical declaration and artifact path or JSON Pointer rather than surfacing root-level failures only.
- Add parity coverage for the shared imported-model/code-first/metadata-first subset and metadata-first parity coverage for advanced imported-model shapes that current Code-First APIs do not expose.

Scope Out
- Redefining dvault.model.v1 fields, tokens, defaults, or validation categories already delivered by 06F0MEE8T9PKPKQH8EPWNQ2CRW and 06F0MEEGJE9QCHC8YN4FEXYX10.
- YAML ingestion work owned by 06F0MEERJ7D5Q4WYBQAJD3GFVC.
- Export tooling, drift reporting, or governance documentation owned by 06F0MEFHKF04B746X7GJKRVT04, 06F0MEFX5M9V9SA25N76CPGT4M, and 06F0MEGAGJCEHQ8QRHGH8W7804.
- New read-service APIs or runtime model mutation beyond imported-model-to-registry and imported-model-to-EF projection.
- Expanding the public Code-First surface to cover link-parent satellites, PIT, bridges, or role-bearing recursive link declarations.
- Provider-specific translator behavior outside the existing provider capability profile mechanism.

Open questions
- none

Follow-up questions
- After this ticket lands, should 06F0MEGAGJCEHQ8QRHGH8W7804 document the new imported-model entry point and the recommended choice between model-first, metadata-first, and Code-First flows?
- Should downstream export and drift tickets 06F0MEFHKF04B746X7GJKRVT04 and 06F0MEFX5M9V9SA25N76CPGT4M consume the same public import result surface directly so artifact normalization and parity logic stay centralized?

Risks
- If imported loadTimestampStorage is not carried into registry provider profiles, imported-model projection can silently diverge from metadata-first and Code-First provider behavior even when the logical model matches.
- If post-parse mapping and translator failures are surfaced only as generic metadata exceptions, the ticket's source-path diagnostic requirement will not be met and imported artifacts will be hard to debug.
- Recursive-role and hierarchy bridge cases remain sensitive because current public link metadata does not carry participant roles; imported-model projection must preserve that extra binding information narrowly enough to avoid collapsing distinct recursive participants into the same EF shape.

Split recommendations
- No new split is recommended. The remaining work is already bounded once schema/parser/YAML stay on their completed sibling tickets and export/drift/governance remain on their existing downstream tickets.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment