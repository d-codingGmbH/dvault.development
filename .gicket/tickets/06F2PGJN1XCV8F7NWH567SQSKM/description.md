<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket into a bounded v1 generator contract: keep generator work in the existing analyzer package, use compile-time declarative mappings as input, and emit additive metadata and row-operation helpers against the current explicit save boundary. No child tickets, relation writes, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence shows `DCoding.Data.DVault.Analyzers` is the existing optional developer-tooling package and there is no separate generator package or generator implementation on the branch, so v1 generator work should extend that package rather than add a new package family.
- Existing typed save contracts in `DCoding.Data.DVault` already center on `DataVaultRegistryHubSaveOperation`, `DataVaultRegistryLinkSaveOperation`, `DataVaultRegistrySatelliteSaveOperation`, `IDataVaultHubMapper<TSource>`, `IDataVaultLinkMapper<TSource>`, and `IDataVaultSatelliteMapper<TSource>`; the generator contract reuses that boundary instead of inventing a new runtime persistence API.
- `loadTimestamp` and `recordSource` remain caller-owned at `IDataVaultSaveService` request time; the generator contract does not hide `SaveAsync`, intercept `SaveChanges`, or create a new metadata authority alongside code-first, metadata-first, and model-first.
- Incoming `blocks` relations from done story `06F2PGJBRXFCP038CN6XVAYSZM` and done epic `06F2PGFT8Z406HFBJGQSY7YRJ0` are satisfied historical dependencies, while this ticket still blocks implementation task `06F2PGJSXP18VKKV52QZA4NP30`.
- No child tickets, relation writes, attachments, or planning documents were created in this refinement run.

### Scope In
- Define one bounded v1 source-generator contract inside the existing `DCoding.Data.DVault.Analyzers` developer-tooling package.
- Ratify compile-time C# mapping declarations as the generator input boundary, with each declaration binding one source CLR type to one logical DVault hub, link, or hub-parent satellite target by exact metadata names and ordered member bindings.
- Define generated outputs as additive metadata helpers plus generated row-mapping code that returns existing `DataVaultRegistry*SaveOperation` types and plugs into existing typed mapper and save-service flows.
- Support hub mappings, link mappings whose participant hub names are unique by `StringComparer.Ordinal`, ordinary hub-parent satellite mappings, and hub-parent multi-active satellite mappings.

### Scope Out
- No implementation of the generator itself; that remains with `06F2PGJSXP18VKKV52QZA4NP30`.
- No new authoritative metadata path, no generator-time execution of EF models, no design-time CLI integration, and no parsing of external model artifacts as the default v1 input.
- No new runtime save orchestration API, automatic `SaveChanges` persistence, automatic load-timestamp generation, or automatic record-source generation.
- No link-parent satellite generation, no same-hub repeated-participant link generation, and no broader reflection or discovery surface.
- No new package family or `docs/releases/v0.12.0.md` work; coordinated documentation remains with `06F2PGJYY6S97B4Z8044D34K5C`.

## Acceptance Criteria
- The ticket ratifies a single v1 generator input and output contract that keeps generator work inside `DCoding.Data.DVault.Analyzers` and keeps `DCoding.Data.DVault` as the runtime API boundary.
- The input contract states that consumers provide compile-time declarative C# mappings, each targeting exactly one hub, link, or hub-parent satellite and naming the exact logical DVault metadata identifiers and ordered source-member bindings to use.
- The output contract states that valid inputs generate deterministic metadata-helper information and row-mapping code that produces the correct existing registry-backed save-operation type for the target shape.
- The contract explicitly limits v1 support to hubs, unique-participant links, ordinary hub-parent satellites, and hub-parent multi-active satellites, and explicitly excludes link-parent satellites and same-hub repeated-participant links.
- The contract assigns malformed declaration detection to compile-time diagnostics while leaving logical metadata resolution, missing required values, and save-request validation on the existing operation constructors and `IDataVaultSaveService` pipeline.
- Downstream implementation and documentation tickets can proceed without reopening package placement, metadata-authority ownership, or explicit-save-boundary decisions.

## Definition of Done
- An authoritative ticket contract records package placement, supported input shapes, generated output shapes, validation ownership, and non-goals for the v1 source-generator slice.
- The contract is concrete enough that `06F2PGJSXP18VKKV52QZA4NP30` can implement the generator without inventing a second package, a fourth metadata authority, or a new persistence boundary.
- The ticket leaves no blocking PO-level ambiguity about supported DVault target shapes, runtime integration boundary, or release-note ownership.
- Any public documentation and release-note follow-through stays delegated to `06F2PGJYY6S97B4Z8044D34K5C` rather than widening this contract ticket.

## Implementation Notes
- Reuse the existing developer-tooling package boundary from `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj`; the generator should remain optional tooling and continue to fit the current `PrivateAssets=all` guidance.
- Keep the input surface compile-time inspectable and local to consumer C# source so the generator does not need to execute `ApplyDataVaultMetadata(...)`, `DataVaultMetadataModel`, `DataVaultModelArtifactImporter`, or consumer design-time command hosts during compilation.
- Generated row mappings should target `DataVaultRegistryHubSaveOperation`, `DataVaultRegistryLinkSaveOperation`, and `DataVaultRegistrySatelliteSaveOperation` and preserve existing logical-name semantics and canonical ordering rules.
- Preserve the existing caller-owned save boundary: generated code may construct row operations or implement existing mapper interfaces, but it must not create hidden `SaveAsync` orchestration, automatic record-source policies, or automatic load-timestamp policies.
- Carry forward current typed-mapper limitations from repository evidence: unique participant hub names for generated links, no link-parent satellites in v1, and no automatic expansion into unsupported runtime shapes.
- Keep public docs and release-note closure in downstream task `06F2PGJYY6S97B4Z8044D34K5C`; this contract ticket should not widen into v0.12 documentation aggregation.

## Open Questions
- none

## Follow-Up Questions
- After the bounded v1 generator lands, should a separate follow-on ticket cover link-parent satellites or self-link and repeated-participant link mappings?
- Should a later ergonomics ticket add generated bulk-save adapters or generated wrappers around `SaveHubAsync(...)`, `SaveLinkAsync(...)`, and future non-ordinary satellite helper paths once the row-operation baseline is proven?
- When `06F2PGJYY6S97B4Z8044D34K5C` runs, should the v0.12 documentation explicitly compare manual typed mappers versus generated helpers for the same `DataVaultRegistry*SaveOperation` boundary?

## Risks
- If implementation treats the generator input as a new authoritative metadata declaration system instead of a helper layer over existing logical names, it will reopen code-first, metadata-first, and model-first ownership and expand scope.
- If generated output hides `loadTimestamp`, `recordSource`, or save orchestration, it can violate the explicit `IDataVaultSaveService` boundary already ratified elsewhere in the repository.
- Satellite or link scope can sprawl quickly if v1 tries to absorb link-parent satellites or same-hub repeated-participant links that current typed-mapper ergonomics already constrain.
- `docs/releases/v0.12.0.md` is still absent on the branch snapshot, so public communication of this generator contract remains a downstream documentation dependency until `06F2PGJYY6S97B4Z8044D34K5C` lands.

## Split Recommendations
- No additional split is required for this contract ticket; the existing story already separates contract definition, implementation (`06F2PGJSXP18VKKV52QZA4NP30`), and release and documentation (`06F2PGJYY6S97B4Z8044D34K5C`).
- If implementation work proves too large, split follow-on generator support by excluded shape families such as link-parent satellites, repeated-participant and self-link handling, or higher-level save wrappers instead of widening the initial v1 contract.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Specify the minimal generator contract before implementation.

## Scope
- Refine and complete the work for "Define source generator input and output contract" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.