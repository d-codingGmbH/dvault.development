[gicket-bot] PO refinement contract

Summary
- Refined the ticket into a bounded v1 generator contract: keep generator work in the existing analyzer package, use compile-time declarative mappings as input, and emit additive metadata and row-operation helpers against the current explicit save boundary. No child tickets, relation writes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows `DCoding.Data.DVault.Analyzers` is the existing optional developer-tooling package and there is no separate generator package or generator implementation on the branch, so v1 generator work should extend that package rather than add a new package family.
- Existing typed save contracts in `DCoding.Data.DVault` already center on `DataVaultRegistryHubSaveOperation`, `DataVaultRegistryLinkSaveOperation`, `DataVaultRegistrySatelliteSaveOperation`, `IDataVaultHubMapper<TSource>`, `IDataVaultLinkMapper<TSource>`, and `IDataVaultSatelliteMapper<TSource>`; the generator contract reuses that boundary instead of inventing a new runtime persistence API.
- `loadTimestamp` and `recordSource` remain caller-owned at `IDataVaultSaveService` request time; the generator contract does not hide `SaveAsync`, intercept `SaveChanges`, or create a new metadata authority alongside code-first, metadata-first, and model-first.
- Incoming `blocks` relations from done story `06F2PGJBRXFCP038CN6XVAYSZM` and done epic `06F2PGFT8Z406HFBJGQSY7YRJ0` are satisfied historical dependencies, while this ticket still blocks implementation task `06F2PGJSXP18VKKV52QZA4NP30`.
- No child tickets, relation writes, attachments, or planning documents were created in this refinement run.

Scope In
- Define one bounded v1 source-generator contract inside the existing `DCoding.Data.DVault.Analyzers` developer-tooling package.
- Ratify compile-time C# mapping declarations as the generator input boundary, with each declaration binding one source CLR type to one logical DVault hub, link, or hub-parent satellite target by exact metadata names and ordered member bindings.
- Define generated outputs as additive metadata helpers plus generated row-mapping code that returns existing `DataVaultRegistry*SaveOperation` types and plugs into existing typed mapper and save-service flows.
- Support hub mappings, link mappings whose participant hub names are unique by `StringComparer.Ordinal`, ordinary hub-parent satellite mappings, and hub-parent multi-active satellite mappings.

Scope Out
- No implementation of the generator itself; that remains with `06F2PGJSXP18VKKV52QZA4NP30`.
- No new authoritative metadata path, no generator-time execution of EF models, no design-time CLI integration, and no parsing of external model artifacts as the default v1 input.
- No new runtime save orchestration API, automatic `SaveChanges` persistence, automatic load-timestamp generation, or automatic record-source generation.
- No link-parent satellite generation, no same-hub repeated-participant link generation, and no broader reflection or discovery surface.
- No new package family or `docs/releases/v0.12.0.md` work; coordinated documentation remains with `06F2PGJYY6S97B4Z8044D34K5C`.

Open questions
- none

Follow-up questions
- After the bounded v1 generator lands, should a separate follow-on ticket cover link-parent satellites or self-link and repeated-participant link mappings?
- Should a later ergonomics ticket add generated bulk-save adapters or generated wrappers around `SaveHubAsync(...)`, `SaveLinkAsync(...)`, and future non-ordinary satellite helper paths once the row-operation baseline is proven?
- When `06F2PGJYY6S97B4Z8044D34K5C` runs, should the v0.12 documentation explicitly compare manual typed mappers versus generated helpers for the same `DataVaultRegistry*SaveOperation` boundary?

Risks
- If implementation treats the generator input as a new authoritative metadata declaration system instead of a helper layer over existing logical names, it will reopen code-first, metadata-first, and model-first ownership and expand scope.
- If generated output hides `loadTimestamp`, `recordSource`, or save orchestration, it can violate the explicit `IDataVaultSaveService` boundary already ratified elsewhere in the repository.
- Satellite or link scope can sprawl quickly if v1 tries to absorb link-parent satellites or same-hub repeated-participant links that current typed-mapper ergonomics already constrain.
- `docs/releases/v0.12.0.md` is still absent on the branch snapshot, so public communication of this generator contract remains a downstream documentation dependency until `06F2PGJYY6S97B4Z8044D34K5C` lands.

Split recommendations
- No additional split is required for this contract ticket; the existing story already separates contract definition, implementation (`06F2PGJSXP18VKKV52QZA4NP30`), and release and documentation (`06F2PGJYY6S97B4Z8044D34K5C`).
- If implementation work proves too large, split follow-on generator support by excluded shape families such as link-parent satellites, repeated-participant and self-link handling, or higher-level save wrappers instead of widening the initial v1 contract.

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