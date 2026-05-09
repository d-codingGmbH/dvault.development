[gicket-bot] PO refinement contract

Summary
- Refined the ticket into an additive immutable registry contract over the existing metadata model, with exact-name and optional CLR-type lookup expectations; no child tickets, relation edits, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 registry is an additive read model over existing metadata declarations, not a replacement for the current modeling builders or EF translator.
- The registry must preserve the metadata categories already visible in source: hubs, links, satellites, PointInTimeTables, bridges, and Pits, plus provider capability profile metadata, so adapting current metadata loses nothing.
- Registry contents are immutable after construction and preserve canonical declaration order for deterministic iteration.
- Logical-name lookups use exact ordinal matching, consistent with the current validation and translation code paths.
- CLR-type lookup is optional entry metadata: metadata-first adaptation may leave CLR mappings unset, and the registry must not invent CLR associations.

Scope In
- Define one immutable registry contract and its bounded builder/adapter surface in the DVault modeling layer.
- Define per-kind collections and lookup APIs for hubs, links, satellites, bridges, PointInTimeTables, Pits, and provider capability profiles.
- Define validation rules and diagnostic expectations for duplicate logical names, ambiguous CLR mappings, and missing referenced metadata dependencies.
- Define the no-loss adaptation path from existing DataVaultMetadataModel instances into the registry, including bridge, PIT, and multi-active satellite metadata.

Scope Out
- Dependency injection or service-registration changes that publish the registry.
- Refactoring existing save or read services to consume the registry.
- Model-first import work or broader metadata-authoring changes.
- Behavioral changes to existing translator or persistence flows beyond the new registry contract itself.

Open questions
- none

Follow-up questions
- After the registry contract lands, should the older PointInTimeTables naming be publicly deprecated in favor of the newer Pits terminology, or should both remain first-class long-term?
- When the code-first fluent contract is implemented, should link and satellite CLR lookup be exposed only through owning or participating hub CLR types, or should a separate explicit CLR mapping surface be added later?

Risks
- The repository currently exposes both PointInTimeTables and Pits; weak registry naming could cause one of those existing surfaces to be collapsed or lost during adaptation.
- If the lookup domains are underspecified, implementers may accidentally require global uniqueness for parent-scoped metadata such as satellites, creating a breaking contract the current model does not require.
- If provider capability metadata is omitted from the first registry contract, downstream translator or persistence tickets are likely to create parallel lookup paths and erode the single-source-of-truth goal.

Split recommendations
- No additional split is recommended at PO stage; this contract ticket already has four outgoing blocks dependents and should remain the shared contract gate for them.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment