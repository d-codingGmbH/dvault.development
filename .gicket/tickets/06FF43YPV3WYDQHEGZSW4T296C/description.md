<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Fresh repo inspection shows repeated same-hub links already work in metadata/runtime when participant roles are explicit; this ticket is refined to generated typed link-mapper parity, explicit-save-path verification, and doc/diagnostic alignment.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current runtime save path already supports repeated same-hub links when link metadata carries distinct produced participant names such as SourceCustomer and MatchedCustomer; the resolved save path reads participant values by produced participant name.
- This ticket does not need a new persistence boundary. The additive goal is generated typed link-mapper parity so same-hub role-bearing links flow through the existing IDataVaultLinkMapper<TSource> and IDataVaultSaveService helper path.
- For this ticket, DataVaultLinkParticipantBindingAttribute should be treated as binding the produced participant name: the hub name for ordinary links, or the explicit participant role name for repeated same-hub links.

### Scope In
- Generated typed link-mapper support for repeated same-hub links when every participant name is explicit, non-blank, and unique by StringComparer.Ordinal.
- Alignment of analyzer diagnostics, XML/docs/contract text, and generated helper metadata so the supported shape is described as role-bearing produced participant names rather than distinct hub types.
- Verification that generated same-hub link mappers work through the existing explicit save-service helper flow without introducing implicit persistence behavior.

### Scope Out
- Ambiguous repeated same-hub links without explicit distinct participant names or roles.
- New implicit persistence, SaveChanges interception, or a replacement for the existing IDataVaultSaveService boundary.
- Broader typed-helper parity work outside repeated same-hub links, including typed read-model generation, link-parent satellite helper expansion, effectivity-specific APIs, or model-first support-bundle changes.

## Acceptance Criteria
- A compile-time generated link mapper can be declared for a repeated same-hub link by using distinct produced participant names in declaration order, and the generated mapper emits a registry-backed link operation that preserves those exact names.
- Generated same-hub link mappers work with the existing typed link save helpers and explicit save-service flow, with callers still supplying loadTimestamp and recordSource and without any new implicit write path.
- Repeated same-hub generation is rejected only when participant names are ambiguous or duplicate; ordinary distinct-hub generated link mappings keep current behavior.
- Tests cover a successful role-bearing repeated same-hub generated mapping and at least one failing ambiguous or duplicate declaration, plus save-path verification against existing runtime/link metadata behavior.

## Definition of Done
- Repository test coverage demonstrates generated same-hub link mapper creation, request assembly, and persistence through the existing explicit save boundary.
- Contract and package docs no longer state that all repeated same-hub typed link mappings are unsupported; they document the explicit-role and unique-produced-name boundary instead.
- No new public persistence abstraction is introduced beyond additive generated-mapper parity and any minimal supporting contract or documentation updates.

## Implementation Notes
- Prefer the smallest additive path: reuse IDataVaultLinkMapper<TSource>, DataVaultRegistryLinkSaveOperation, and the existing single, bulk, and async SaveLink* helpers instead of introducing a separate same-hub-specific save contract.
- Fresh code inspection shows DataVaultRegistryLinkSaveOperation and the resolved save path already key link participants by produced participant name (SourceEndpointName), so the main work is generator declaration semantics, diagnostics, tests, and contract alignment rather than a new save pipeline.
- Update generator and analyzer copy that currently says participant hub name or claims repeated same-hub typed mappings are unsupported; the effective uniqueness rule for generated links is the produced participant name set.
- Use the existing role-bearing same-hub baseline CustomerIdentityMatch with SourceCustomer and MatchedCustomer as the representative parity example.

## Open Questions
- none

## Follow-Up Questions
- Should a later compatibility cleanup add a clearer additive alias or obsoletion path for ParticipantHubName-style naming, which now semantically represents a produced participant name when same-hub roles are explicit?

## Risks
- The public names ParticipantHubName and ParticipantHubNames are misleading for same-hub role-bearing mappings; incomplete doc alignment could leave the supported pattern hard to discover.
- If implementation expands into a new public same-hub-specific save contract instead of reusing the current mapper and save-service path, scope and compatibility risk grow unnecessarily.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add source-generated typed mapper or save helper support for repeated same-hub links where roles are explicit and unambiguous. Acceptance: generated code preserves participant role identity and explicit save-service semantics.