<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket into an additive immutable registry contract over the existing metadata model, with exact-name and optional CLR-type lookup expectations; no child tickets, relation edits, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 registry is an additive read model over existing metadata declarations, not a replacement for the current modeling builders or EF translator.
- The registry must preserve the metadata categories already visible in source: hubs, links, satellites, PointInTimeTables, bridges, and Pits, plus provider capability profile metadata, so adapting current metadata loses nothing.
- Registry contents are immutable after construction and preserve canonical declaration order for deterministic iteration.
- Logical-name lookups use exact ordinal matching, consistent with the current validation and translation code paths.
- CLR-type lookup is optional entry metadata: metadata-first adaptation may leave CLR mappings unset, and the registry must not invent CLR associations.

### Scope In
- Define one immutable registry contract and its bounded builder/adapter surface in the DVault modeling layer.
- Define per-kind collections and lookup APIs for hubs, links, satellites, bridges, PointInTimeTables, Pits, and provider capability profiles.
- Define validation rules and diagnostic expectations for duplicate logical names, ambiguous CLR mappings, and missing referenced metadata dependencies.
- Define the no-loss adaptation path from existing DataVaultMetadataModel instances into the registry, including bridge, PIT, and multi-active satellite metadata.

### Scope Out
- Dependency injection or service-registration changes that publish the registry.
- Refactoring existing save or read services to consume the registry.
- Model-first import work or broader metadata-authoring changes.
- Behavioral changes to existing translator or persistence flows beyond the new registry contract itself.

## Acceptance Criteria
- A registry instance can be built from the current DataVaultMetadataModel surface plus provider capability profile metadata without dropped items, reordered items, or inferred replacements.
- The built registry is immutable and exposes deterministic iteration order that matches canonical declaration order.
- The registry provides exact-name lookup for every in-scope metadata kind and parent-scoped lookup where a kind is not globally unique, so valid repeated child names remain representable.
- Where CLR mappings are present, the registry exposes CLR-type lookup; where no CLR mapping is present, lookup returns no match instead of inventing one.
- Registry construction rejects duplicate logical names in the relevant lookup domain, ambiguous CLR mappings, and missing referenced metadata dependencies.
- Validation failures identify the conflicting metadata kind, logical name, and referenced dependency or CLR type precisely enough for callers and tests to pinpoint the offending declaration.

## Definition of Done
- The public contract and placement for the registry, builder or adapter, and lookup surfaces are committed in the existing DVault modeling architecture and follow current naming and layout conventions.
- Automated tests cover deterministic ordering, immutability, exact-name lookup, parent-scoped lookup behavior, CLR ambiguity detection, and missing-dependency diagnostics.
- Automated tests prove no-loss adaptation from the current DataVaultMetadataModel baselines, including bridges, PointInTimeTables, Pits, and multi-active satellite driving keys.
- The ticket completes without adding DI wiring, save-service or read-service rewrites, or model-import work.

## Implementation Notes
- Use the existing DataVaultMetadataModel collections as the bounded v1 metadata source and layer the registry on top as an additive lookup-oriented read model instead of reshaping declaration types first.
- Keep declaration order stable from source lists into registry indexes because current translation code already depends on deterministic ordering across hubs, links, satellites, bridges, and PIT metadata.
- Represent satellite lookup with parent-aware indexing instead of forcing new global satellite-name uniqueness, because current naming and validation behavior already allow child metadata to be scoped by parent.
- Carry provider capability metadata into the same registry family so follow-up translator, save, and read tickets can resolve metadata and provider profile data through one immutable contract.
- Treat CLR mapping as optional entry metadata populated only when upstream configuration supplies it; metadata-first adaptation should leave CLR mapping unset rather than infer types.
- No child tickets, relation changes, or planning documents were materialized in this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- After the registry contract lands, should the older PointInTimeTables naming be publicly deprecated in favor of the newer Pits terminology, or should both remain first-class long-term?
- When the code-first fluent contract is implemented, should link and satellite CLR lookup be exposed only through owning or participating hub CLR types, or should a separate explicit CLR mapping surface be added later?

## Risks
- The repository currently exposes both PointInTimeTables and Pits; weak registry naming could cause one of those existing surfaces to be collapsed or lost during adaptation.
- If the lookup domains are underspecified, implementers may accidentally require global uniqueness for parent-scoped metadata such as satellites, creating a breaking contract the current model does not require.
- If provider capability metadata is omitted from the first registry contract, downstream translator or persistence tickets are likely to create parallel lookup paths and erode the single-source-of-truth goal.

## Split Recommendations
- No additional split is recommended at PO stage; this contract ticket already has four outgoing blocks dependents and should remain the shared contract gate for them.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Define the registry types and lookup contract that become the shared source of truth for configured Data Vault metadata.

## Scope In

- Registry shape for hubs, links, satellites, PIT, bridges, and provider capability metadata.
- Lookup methods by logical name and optional CLR type.
- Validation for duplicate names, ambiguous CLR mappings, and missing dependencies.

## Scope Out

- Service registration wiring.
- Save/read service refactoring.
- Model-first import.

## Acceptance Criteria

- Registry construction is deterministic and immutable after build.
- Existing DataVaultMetadataModel can be adapted into the registry without losing information.
- Validation output identifies the exact conflicting metadata element.