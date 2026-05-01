[gicket-bot] PO refinement contract

Summary
- Refined the parent Data Vault write-pipeline story around the repository's explicit IDataVaultSaveService baseline, ratified the existing SQLite-first idempotency semantics, and confirmed the already-materialized child-ticket split remains sufficient.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 write pipeline is the explicit DI-resolved IDataVaultSaveService boundary rather than a SaveChanges interceptor.
- A save request carries one shared load timestamp and record source for the batch, and the load timestamp is normalized to UTC before persistence.
- Hub and link idempotency is keyed by deterministic hash keys computed through the registered stable-hash normalizer and hash service; repeat saves reuse existing rows instead of inserting duplicates.
- Satellite change detection is parent-specific and latest-row-based: the service skips an insert only when the newest row for that parent already has the same hash diff; if data changes and later returns to an earlier hash diff, a new historical row is still inserted.
- This parent story is already split through child tickets 06EXB7H6KV753KM125XN3VDRTM, 06EXB7HEJY18HEB5A5MVTN5KZC, and 06EXB7HPGW3Y9MSP10DEC8RBK4.

Scope In
- Explicit write orchestration for hub, link, and satellite rows in the DCoding.Data.DVault save service surface.
- Deterministic reuse behavior for repeated hub and link saves across repeated calls and across SQLite contexts.
- Satellite history insertion rules driven by parent hash key, caller-supplied hash diff, record source, and load timestamp.
- SQLite-first unit and integration coverage for the default save service behavior.

Scope Out
- SaveChanges interception or other transparent EF write hooks.
- Changes to the stable-hash algorithm or normalization contract, and automatic computation of satellite hash diffs inside this story.
- Provider-specific upsert behavior, multi-writer concurrency guarantees, retry semantics, or non-SQLite provider commitments.
- Deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, migrations, and broader provider optimizations.

Open questions
- none

Follow-up questions
- If later providers need multi-writer safety or native upsert semantics, should that be planned as a separate provider-capability story instead of expanding this SQLite-first parent story?
- Should a later ticket add broader representative coverage for link-attached satellites and non-SQLite provider profiles after the v1 SQLite baseline is accepted?
- If callers want a convenience layer that computes satellite hash diffs for them, should that be introduced as a separate API story rather than folded into this persistence story?

Risks
- Because the current provider capability profile declares no v1 concurrency signal support, the idempotency contract is suitable for ordinary repeated saves but does not promise race-free behavior under concurrent multi-writer contention.
- Satellite change detection depends on caller-supplied hash diffs, so incorrect upstream hash-diff preparation can create missing or spurious history rows.
- The repository evidence proves the SQLite baseline; behavior for other providers remains deferred and should not be implied by this ticket.

Split recommendations
- No new split is recommended. This parent story is already materially decomposed through child tickets 06EXB7H6KV753KM125XN3VDRTM, 06EXB7HEJY18HEB5A5MVTN5KZC, and 06EXB7HPGW3Y9MSP10DEC8RBK4.
- Keep future provider-specific concurrency or upsert work, and advanced Data Vault patterns beyond hubs, links, and single-active satellites, in separate follow-up tickets.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment