<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the parent Data Vault write-pipeline story around the repository's explicit IDataVaultSaveService baseline, ratified the existing SQLite-first idempotency semantics, and confirmed the already-materialized child-ticket split remains sufficient.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 write pipeline is the explicit DI-resolved IDataVaultSaveService boundary rather than a SaveChanges interceptor.
- A save request carries one shared load timestamp and record source for the batch, and the load timestamp is normalized to UTC before persistence.
- Hub and link idempotency is keyed by deterministic hash keys computed through the registered stable-hash normalizer and hash service; repeat saves reuse existing rows instead of inserting duplicates.
- Satellite change detection is parent-specific and latest-row-based: the service skips an insert only when the newest row for that parent already has the same hash diff; if data changes and later returns to an earlier hash diff, a new historical row is still inserted.
- This parent story is already split through child tickets 06EXB7H6KV753KM125XN3VDRTM, 06EXB7HEJY18HEB5A5MVTN5KZC, and 06EXB7HPGW3Y9MSP10DEC8RBK4.

### Scope In
- Explicit write orchestration for hub, link, and satellite rows in the DCoding.Data.DVault save service surface.
- Deterministic reuse behavior for repeated hub and link saves across repeated calls and across SQLite contexts.
- Satellite history insertion rules driven by parent hash key, caller-supplied hash diff, record source, and load timestamp.
- SQLite-first unit and integration coverage for the default save service behavior.

### Scope Out
- SaveChanges interception or other transparent EF write hooks.
- Changes to the stable-hash algorithm or normalization contract, and automatic computation of satellite hash diffs inside this story.
- Provider-specific upsert behavior, multi-writer concurrency guarantees, retry semantics, or non-SQLite provider commitments.
- Deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, migrations, and broader provider optimizations.

## Acceptance Criteria
- Saving the same hub business-key set again does not insert a duplicate hub row and still returns the same saved hub hash key summary.
- Saving the same link participant hash-key set again does not insert a duplicate link row and still returns the same saved link hash key summary.
- Each newly inserted hub, link, and satellite row persists the request record source and UTC-normalized load timestamp using the existing translated Data Vault table and column naming baseline.
- A satellite insert is skipped only when the newest existing row for the same parent hash key already has the same hash diff; a later return to a prior hash diff is persisted as a new historical row.
- DataVaultSaveResult.RowsWritten counts only rows inserted by the explicit invocation while SavedRecords remain deterministic in hub-then-link-then-satellite request order.

## Definition of Done
- The default AddDVault registration resolves IDataVaultSaveService without requiring SaveChanges interceptors or caller options.
- Repository tests cover representative hub, link, and satellite persistence, replay, and satellite-history scenarios on the SQLite baseline.
- Implementation follows the shared implementation standards plus the referenced MVP Data Vault concepts, default naming policy, stable hashing contract, and explicit save service note.
- The parent story contract continues to reflect the existing child-ticket split instead of reopening that decomposition.

## Implementation Notes
- Use the current DCoding.Data.DVault project and the translated shared-type EF entities produced by ApplyDataVaultMetadata; the visible v1 naming baseline remains HubCustomer, LinkCustomerOrder, and SatCustomerContact-style identifiers.
- Hub hash keys are computed from hub business-key fields and link hash keys from participant hub hash keys through IStableHashNormalizer and IStableHashService; satellite hash diffs are supplied by the caller under the established stable-hashing contract.
- The current provider baseline is DataVaultProviderCapabilityProfiles.Sqlite, whose v1 concurrency support is NoneInV1Unsupported, so idempotency is implemented with deterministic pre-insert reuse lookup rather than provider-specific upsert logic.
- Repository evidence already includes unit coverage in tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs and SQLite integration coverage in tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs.
- Incoming relation evidence shows the stable hashing dependency ticket 06EXB765S2X2MR2K18ZBV8RC38 has already driven follow-up activity for this story; this refinement does not reopen hashing design decisions.

## Open Questions
- none

## Follow-Up Questions
- If later providers need multi-writer safety or native upsert semantics, should that be planned as a separate provider-capability story instead of expanding this SQLite-first parent story?
- Should a later ticket add broader representative coverage for link-attached satellites and non-SQLite provider profiles after the v1 SQLite baseline is accepted?
- If callers want a convenience layer that computes satellite hash diffs for them, should that be introduced as a separate API story rather than folded into this persistence story?

## Risks
- Because the current provider capability profile declares no v1 concurrency signal support, the idempotency contract is suitable for ordinary repeated saves but does not promise race-free behavior under concurrent multi-writer contention.
- Satellite change detection depends on caller-supplied hash diffs, so incorrect upstream hash-diff preparation can create missing or spurious history rows.
- The repository evidence proves the SQLite baseline; behavior for other providers remains deferred and should not be implied by this ticket.

## Split Recommendations
- No new split is recommended. This parent story is already materially decomposed through child tickets 06EXB7H6KV753KM125XN3VDRTM, 06EXB7HEJY18HEB5A5MVTN5KZC, and 06EXB7HPGW3Y9MSP10DEC8RBK4.
- Keep future provider-specific concurrency or upsert work, and advanced Data Vault patterns beyond hubs, links, and single-active satellites, in separate follow-up tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Persist hubs, links, and satellites according to Data Vault-oriented insert/idempotency rules.

## Scope
- Persist hubs and links idempotently.
- Insert satellite rows only when hash diff changes.

## Acceptance Criteria
- Repeated saves do not create duplicate hubs or links.
- Unchanged satellite payloads do not create new rows.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.