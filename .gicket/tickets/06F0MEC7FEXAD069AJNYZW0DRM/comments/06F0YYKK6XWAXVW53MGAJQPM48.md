[gicket-bot] PO refinement contract

Summary
- Refined the ticket into an additive v1 contract for small typed row mappers that return existing registry-backed hub, link, and satellite save-operation inputs by exact logical name while keeping load timestamp and record source explicit at request assembly time.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The canonical v1 mapper output is the existing registry-backed save-operation family DataVaultRegistryHubSaveOperation, DataVaultRegistryLinkSaveOperation, and DataVaultRegistrySatelliteSaveOperation, not metadata-object-based save operations.
- Use one public interface per row kind in DCoding.Data.DVault: IDataVaultHubMapper<TSource>, IDataVaultLinkMapper<TSource>, and IDataVaultSatelliteMapper<TSource>.
- Each mapper stays intentionally small and manual: one Map(TSource source) call produces one row operation for that row kind.
- Hub mappers output business-key values keyed by exact hub business-key names; link mappers output participant hash keys keyed by exact participant hub metadata names; satellite mappers output parent hash key, payload values, optional driving-key values, and hash diff.
- Satellite mapper target identity follows the existing parent-scoped registry lookup contract: exact DataVaultMetadataReference parent plus exact satellite name, with support for both hub-parent and link-parent satellites.
- Mapper outputs stay string-based because the current save-operation inputs are string-based; typed scalar or provider conversion is not introduced by this contract.
- LoadTimestamp and RecordSource remain explicit request-level inputs on DataVaultRegistrySaveRequest or a later typed helper boundary and are not hidden inside individual row mappers.
- CLR-type metadata lookup remains optional infrastructure in the repository, so v1 typed mappers must not require DataVaultMetadataClrMapping and instead carry their target logical names directly in the returned registry-backed operations.
- Mapped key/value collections are matched by exact logical name; enumeration order is not the canonical order. Canonical order remains the metadata declaration order already used by hubs, links, and multi-active satellites.
- For multi-active satellites, driving-key values remain separate from payload values and hash diff, and the exact driving-key set must match the declared satellite metadata names.

Scope In
- Define the public hub, link, and satellite mapper interface family and its one-source-to-one-row-operation contract.
- Define the exact target-identity convention for typed mappers using logical metadata names and parent-scoped satellite references.
- Define the per-kind mapped value families for business keys, participant hash keys, parent hash key, payload values, driving-key values, and hash diff.
- Define the request-boundary rule that typed save helpers continue to surface LoadTimestamp and RecordSource explicitly when they assemble DataVaultRegistrySaveRequest or DataVaultRegistryBulkSaveRequest.
- Define validation and diagnostic expectations for null source inputs, null mapped values, duplicate output names, missing required values, and missing or extra driving-key names.
- Define the additive placement and API-snapshot expectations for the new mapper contracts in the existing DCoding.Data.DVault public surface.
- Define regression expectations proving manual mapper implementations can feed the existing registry-backed save path without callers hand-assembling raw name/value lists.

Scope Out
- Implementing the typed save helper APIs that submit mapped operations through IDataVaultSaveService.
- Typed latest or as-of satellite read projections, DTO materializers, or read-side mapper contracts.
- Automatic SaveChanges interception.
- Automatic hash-diff generation, participant hash-key derivation from business keys, or other hidden hashing convenience beyond what the mapper explicitly returns.
- Source generation, model-first generation, reflection-based auto-mapping, or convention-only runtime object inspection.
- DI registration or discovery policy for mapper collections beyond whatever minimal wiring later implementation tickets choose.
- Provider-specific save-strategy changes or changes to the existing explicit low-level save contracts.

Open questions
- none

Follow-up questions
- After the row-mapper contract lands, should a later convenience layer add composite request mappers that map one source object to a hub-plus-satellite request, or should v1 stay with one-row mappers composed by the helper?
- Should a later convenience layer add optional participant business-key-to-link hash-key derivation, or should link mappers continue to supply participant hash keys explicitly?
- Should future code-first or model-first tooling auto-emit mapper implementations or registry CLR mappings so callers can opt into metadata lookup by source CLR type without changing this v1 contract?

Risks
- If implementation targets metadata-object-based save operations instead of the chosen registry-backed operation family, typed helpers will couple to metadata construction and drift from the ordinary authoritative-registry path already established in the repository.
- If hidden CLR-type metadata inference is added in v1, metadata-first or code-first registrations without DataVaultMetadataClrMapping will fail unpredictably even though current repository evidence makes CLR mappings optional.
- If row mappers blur driving-key, payload, and hash-diff responsibilities, multi-active satellite behavior will diverge from the existing save contract and create inconsistent persistence semantics.
- If later helper implementations hide LoadTimestamp or RecordSource inside mapper logic, they will violate the explicit save boundary already documented for IDataVaultSaveService.
- Because current operation inputs are string-based, weak coverage around mapper-produced string values could allow inconsistent caller-side business-key or hash-diff formatting unless tests pin the contract down clearly.

Split recommendations
- No split recommended; this ticket should remain the shared contract gate for typed save helpers, while save-helper implementation and typed read projection work stay on 06F0MECFNF42NK9PND9DWVW9VW and 06F0MECPFAVBFBNC5XMVDZRQ6M.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment