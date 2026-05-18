<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence already contains the PIT maintenance contract, split tickets, and blocking chain; this story is refined to the provider-neutral explicit PIT maintenance baseline with no new planning writes needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already contains docs/plans/pit-maintenance-service-v1-contract.md with this ticket as the primary ticket and 06F2PGPKXWRFXNPFA1JR0X67XC, 06F2PGPRGN0EVGD6RY5KY9M56W, and 06F2PGPXVAYRBC94RQ7X5V4DVG as related follow-on tickets.
- Persisted relation events already model the delivery chain: 06F2PGPBRFT48JG57SV57N9TVW blocks 06F2PGPKXWRFXNPFA1JR0X67XC and 06F2PGPXVAYRBC94RQ7X5V4DVG; 06F2PGPKXWRFXNPFA1JR0X67XC blocks 06F2PGPRGN0EVGD6RY5KY9M56W; 06F2PGPRGN0EVGD6RY5KY9M56W blocks 06F2PGPXVAYRBC94RQ7X5V4DVG.
- Ticket comments contain only bot claim and lease entries; there are no human comments adding scope or blocking questions.
- Existing README and docs/releases/v0.7.0.md already ratify that PIT reads consume already materialized PIT tables and do not refresh or maintain them implicitly.

### Scope In
- Add one additive explicit PIT maintenance service in DCoding.Data.DVault, registered beside the existing explicit save and read services through AddDVault(...).
- Support a full rebuild for one DataVaultPitMetadata declaration and bounded incremental maintenance for explicit parent hash keys.
- Keep the maintenance baseline provider-neutral and deterministic by recomputing PIT rows from distinct satellite LoadTimestamp values in ascending order and populating each <Satellite>LoadTimestamp snapshot from the latest visible satellite row at or before each PIT timestamp.
- Validate and fail before writes when PIT metadata, generated PIT entities, or participating satellites fall outside the supported v1 PIT shape.
- Add unit and SQLite integration coverage for rebuild semantics, bounded parent maintenance, missing satellite segments, and late-arriving satellite corrections.

### Scope Out
- Changing IDataVaultReadService PIT query semantics, typed PIT projection ergonomics, or current/as-of query API shape; that follow-on work stays in 06F2PGPKXWRFXNPFA1JR0X67XC.
- Provider-aware PIT or bridge read optimization; that stays in 06F2PGPRGN0EVGD6RY5KY9M56W.
- User-facing README and v0.15.0 release-note follow-through beyond the contract handoff; that stays in 06F2PGPXVAYRBC94RQ7X5V4DVG.
- Legacy DataVaultPointInTimeMetadata and DataVaultModelBuilder.PointInTime(...), link-parent PITs, multi-active PITs, bridge maintenance, hosted or background orchestration, and provider-specific maintenance strategies.

## Acceptance Criteria
- The core package exposes an additive explicit PIT maintenance surface for one DataVaultPitMetadata target without hiding PIT refresh behind SaveChanges, interceptors, or background automation.
- Full rebuild deletes and regenerates the complete PIT contents for one declared PIT table using the authoritative row-generation rule from docs/plans/pit-maintenance-service-v1-contract.md.
- Incremental maintenance accepts explicit parent hash keys, treats empty input as a no-op, recomputes complete PIT history for only those parents, and replaces existing PIT rows for those parents so late-arriving satellite rows correct prior PIT history.
- The supported v1 shape is enforced before writes: hub-parent DataVaultPitMetadata only, non-multi-active hub-attached satellites only, unique declared satellites, and the translated PIT entity and columns must match the existing ParentHashKey, LoadTimestamp, and <Satellite>LoadTimestamp contract.
- The result remains compatible with the existing PIT read contract so downstream tickets can assume maintained PIT tables without redefining PIT row-population semantics.

## Definition of Done
- Public DI registration, request and result types, and any convenience adapters are additive, snapshot-covered as needed, and do not break the existing explicit save and read surface.
- Unit tests cover validation failures and deterministic row-generation behavior.
- SQLite integration tests cover full rebuild, bounded parent maintenance, missing satellite snapshots, and late-arriving satellite corrections.
- The implementation leaves clear handoff evidence for 06F2PGPKXWRFXNPFA1JR0X67XC, 06F2PGPRGN0EVGD6RY5KY9M56W, and 06F2PGPXVAYRBC94RQ7X5V4DVG without reopening PIT maintenance semantics.

## Implementation Notes
- AddDVault(...) currently registers IDataVaultSaveService and IDataVaultReadService as explicit services; PIT maintenance should be introduced as a sibling explicit service, not folded into either existing interface.
- README.md and docs/releases/v0.7.0.md already describe PIT reads as operating over already materialized PIT tables, so the new service should preserve that explicit separation and only change the source of PIT row population.
- Reuse the existing translated PIT naming and column contract: PIT table name produced from the metadata name, plus ParentHashKey, LoadTimestamp, and one <Satellite>LoadTimestamp snapshot column per declared satellite.
- No new child tickets, relation changes, attachments, or planning documents were materialized in this run because the repository already contains docs/plans/pit-maintenance-service-v1-contract.md and the related ticket chain above.

## Open Questions
- none

## Follow-Up Questions
- When 06F2PGPXVAYRBC94RQ7X5V4DVG is implemented, should the v0.15.0 release notes create a new docs/releases/v0.15.0.md file, since the repository currently stops at v0.14.0?
- After the provider-neutral baseline lands, which providers justify dedicated PIT maintenance performance work beyond the existing read-optimization ticket?
- Should the legacy PointInTime surface be formally deprecated in a later release once the DataVaultPitMetadata maintenance and read flow is complete?

## Risks
- Provider-neutral full rebuild and parent-scoped recomputation can be expensive on large PIT tables, so v1 should not imply provider-specific physical tuning or hosted orchestration.
- The repository still contains both legacy PointInTime and newer DataVaultPitMetadata surfaces, so the implementation must avoid accidentally merging or renaming those contracts.
- If the downstream documentation ticket does not update README and v0.15.0 release notes promptly, public guidance will still describe PIT reads without the new maintained-PIT baseline.

## Split Recommendations
- No further split is recommended; the repository already contains the durable planning split through docs/plans/pit-maintenance-service-v1-contract.md and tickets 06F2PGPKXWRFXNPFA1JR0X67XC, 06F2PGPRGN0EVGD6RY5KY9M56W, and 06F2PGPXVAYRBC94RQ7X5V4DVG.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add explicit rebuild and incremental maintenance for PIT tables.

## Scope
- Refine and complete the work for "Add PIT maintenance service" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.