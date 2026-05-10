<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current HEAD already contains the typed row-mapper contracts, registry-backed save request types, and a manual SQLite integration test that assembles DataVaultRegistrySaveRequest directly; this ticket is only the thin helper layer over that baseline.
- V1 helper APIs stay additive in DCoding.Data.DVault and should compose existing IDataVaultHubMapper<TSource>, IDataVaultLinkMapper<TSource>, and IDataVaultSatelliteMapper<TSource> outputs into DataVaultRegistrySaveRequest or DataVaultRegistryBulkSaveRequest before delegating to IDataVaultSaveService.
- LoadTimestamp and RecordSource remain explicit helper inputs at request-assembly time; this ticket does not hide the write boundary behind ambient policies or DbContext.SaveChanges interception.
- For v1 diagnostics, source object means stable source context that at minimum includes the source CLR type and, for bulk helpers, the zero-based batch index; helper errors should not depend on arbitrary ToString output or reflection dumps.
- The bounded satellite helper surface is hub-parent ordinary satellites only; multi-active and link-parent satellite convenience helpers stay out of scope even though the underlying mapper and save-operation contracts already support those shapes.
- Link helpers inherit the v1 typed-link boundary from done task 06F0MEC7FEXAD069AJNYZW0DRM: only links with unique participant hub metadata names are supported, and same-hub or self-link convenience remains deferred.
- No new child tickets were created because the existing story split is already sufficient: this ticket owns typed save helpers, while sibling task 06F0MECPFAVBFBNC5XMVDZRQ6M continues to own typed read projections.

### Scope In
- Add additive typed helper entry points over IDataVaultSaveService for single hub saves from IDataVaultHubMapper<TSource> outputs.
- Add additive typed helper entry points for single link saves from IDataVaultLinkMapper<TSource> outputs within the current unique-participant link boundary.
- Add additive typed helper entry points for single ordinary hub-parent satellite saves from IDataVaultSatelliteMapper<TSource> outputs.
- Add ordered bulk helper coverage for prepared source batches by assembling DataVaultRegistryBulkSaveRequest in caller order.
- Add diagnostic wrapping and regression tests so mapping failures surface logical target plus stable source context, while successful saves preserve the current provider strategy and fallback behavior.

### Scope Out
- SaveChanges interception or any hidden unit-of-work hook around DbContext.SaveChanges.
- New row-mapper contracts, source generators, reflection auto-mapping, or CLR-type discovery beyond the already-landed mapper baseline.
- Composite one-call hub-plus-satellite request mappers or automatic parent-hash-key or participant-hash-key derivation beyond the current row-mapper inputs.
- Multi-active or link-parent satellite save-helper convenience.
- Same-hub or self-link link helper support that would require a new participant identity shape.
- Provider-specific save-strategy changes, IDataVaultSaveService pipeline rewrites, or read-side helper work.

## Acceptance Criteria
- A caller can execute the common hub save and subsequent ordinary satellite save flow through typed helper calls using the existing row-mapper interfaces, without manually assembling raw name/value collections at the call site.
- Helpers build DataVaultRegistrySaveRequest or DataVaultRegistryBulkSaveRequest and delegate to the existing registry-backed IDataVaultSaveService.SaveAsync overloads, preserving provider strategy selection or fallback and current DataVaultSaveResult ordering and RowsWritten semantics.
- Helper entry points keep LoadTimestamp and RecordSource explicit per save or bulk call and do not register or rely on DbContext.SaveChanges interception.
- Helper coverage includes single hub saves, single link saves within the current unique-participant link boundary, single ordinary hub-parent satellite saves, and ordered bulk saves for prepared source batches.
- When mapper invocation or helper request assembly fails, the surfaced exception identifies the logical target and stable source context, including CLR type and zero-based batch index when applicable, while preserving the underlying validation reason.
- Regression coverage proves helper-based calls still exercise the existing save-service pipeline on the current SQLite baseline and do not bypass provider strategy dispatch or fallback.

## Definition of Done
- Public API, XML docs, and snapshot coverage include the new helper entry points and any minimal supporting helper or request types.
- Unit tests cover hub, link, ordinary satellite, and ordered bulk helper assembly, plus wrapped diagnostic failures.
- Integration tests show helper-built requests persist correctly through the existing SQLite baseline and preserve current DataVaultSaveResult behavior.
- At least one strategy-selection or fallback regression test exercises the helper layer and confirms the current provider optimization boundary still applies.
- No ISaveChangesInterceptor registrations, DbContext.SaveChanges hooks, or provider-specific save-strategy changes are introduced by this ticket.

## Implementation Notes
- Mirror the existing registry save and read extension pattern: additive extension methods on IDataVaultSaveService in DCoding.Data.DVault are the default v1 placement.
- Keep the helper surface thin: compose existing IDataVaultHubMapper<TSource>, IDataVaultLinkMapper<TSource>, and IDataVaultSatelliteMapper<TSource> outputs, build registry-backed requests, and call the current SaveAsync overloads rather than adding a new orchestration service.
- Do not add a new composite request-mapper contract in this ticket; callers can compose one-row mappers and chain hub then satellite flows explicitly until a later convenience ticket justifies a broader abstraction.
- For ordinary satellite helpers, the caller-owned source or mapper input still supplies parent hash key and hash diff; helper logic should not invent hidden hash derivation or pre-save metadata discovery rules.
- Bulk helpers must preserve caller order because DataVaultRegistryBulkSaveRequest and the existing save-service behavior already treat order as meaningful.
- Live relation state after cleanup is correct as-is: parent story 06F0MEBV90FB8TQMRXJNH078BM remains the upstream relation and 06F0MECFNF42NK9PND9DWVW9VW continues to block quickstart task 06F0MEDBFZ25YA1M7RJ71Z7ZCM.

## Open Questions
- none

## Follow-Up Questions
- Should a later convenience ticket add a single-call hub-plus-satellite orchestration API that derives the parent hash key from the hub save result instead of requiring two thin helper calls?
- Should a later convenience ticket add multi-active and link-parent satellite helper coverage once the ordinary satellite baseline is proven?
- If users need same-hub or self-link typed link helpers later, what participant identity shape should that follow-up contract adopt?
- After the thin helper layer lands, do we want optional policy wrappers over explicit LoadTimestamp and RecordSource inputs, or should explicit per-call parameters remain the only convenience surface?

## Risks
- If implementation broadens scope into composite mapping or hidden hash-key derivation, the ticket stops being a thin helper layer and may conflict with the already-landed row-mapper contract.
- If wrapped diagnostics omit the original inner validation message, callers will lose the specific duplicate-name or missing-value reason that current registry operations and save-service validation already provide.
- If bulk helpers reorder sources or coalesce requests, they can change DataVaultSaveResult.SavedRecords ordering and provider-strategy evaluation semantics relative to the current explicit bulk contract.
- If ordinary-satellite-only scope is blurred, downstream users may assume multi-active or link-parent helper coverage that this ticket is not meant to ship.

## Split Recommendations
- No additional split is recommended; the current task is bounded once it stays a thin additive helper layer over the existing mapper and registry-save contracts.
- If later demand exists, split follow-up tickets for composite hub-plus-satellite convenience, multi-active or link-parent satellite helpers, and same-hub or self-link link helpers rather than expanding this task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Implement typed explicit save helpers that build and submit save requests while preserving the deliberate explicit write boundary.

## Scope In

- Hub and ordinary satellite save helpers.
- Link save helper for configured relationships.
- Bulk helper for prepared domain batches.
- Regression tests against existing save-service behavior.

## Scope Out

- SaveChanges interception.
- Model-first generation.
- Provider-specific save strategy changes.

## Acceptance Criteria

- Typed save helpers call the existing provider strategy/fallback pipeline.
- Helpers do not hook or override DbContext.SaveChanges.
- Error messages identify the model element and source object that could not be mapped.