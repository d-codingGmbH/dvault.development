[gicket-bot] PO refinement contract

Summary
- Refined as an evidence-only performance task on the existing SQLite/`sha256-v1`/`HexString` baseline: rank DVault allocation hotspots in stable-hash canonicalization, digest generation, satellite hash-diff replay filtering, and pre-write save preparation before any optimization ticket starts.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- DVault already treats satellite `HashDiff` as caller-supplied input; this ticket profiles the DVault-owned latest-hash-diff lookup/filter and replay-dedup path rather than introducing an internal payload-hash generator.
- The default v1 evidence baseline is the required SQLite benchmark lane with `sha256-v1` and `HexString` hash-key storage, which matches the checked-in root benchmark triplet and current stable-hash/storage contracts.
- Existing benchmark artifacts already capture whole-scenario mean allocated bytes; this ticket must add finer-grained hotspot ranking evidence before any allocation-reduction implementation is attempted.

Scope In
- Profile DVault-owned allocation hotspots in `DefaultStableHashNormalizer` canonicalization and `BuiltInStableHashService` digest generation on the default hash-key baseline.
- Profile provider-neutral/common save preparation in `DefaultDataVaultSaveService`, including request resolution, hub/link hash-key save-plan creation, unique-row dedupe preparation, and row materialization before database writes.
- Profile the satellite latest-hash-diff path for unchanged and changed replay workloads, including latest-state lookup/filtering and chunked continuity-state handling where that path is exercised.
- Preserve ticket-scoped evidence and a ranked hotspot summary that identifies the measured workloads, the highest-allocation steps or methods, and the recommended optimization order.

Scope Out
- Implementing allocation optimizations or changing runtime behavior in the same ticket; this ticket stops at measured hotspot ranking.
- Changing stable-hash algorithm ids, hash-key storage-profile contracts, or caller-facing lowercase-hex hash-key boundaries.
- Reopening hash-diff ownership; caller code continues to supply the deterministic satellite `HashDiff` value.
- Provider-specific SQL/save/read tuning, external-provider timing collection, or full hash-key variant-matrix validation beyond the default baseline.

Open questions
- none

Follow-up questions
- After the ranking lands, should actual optimization work be split into separate implementation tickets for stable-hash canonicalization/hash generation versus satellite replay/save-preparation allocations?
- Once a hotspot fix is implemented, which non-default validation lanes need reruns beyond the default baseline: the bounded hash-key storage matrix, optional provider save lanes, or both?
- Should any focused hotspot lane become part of the default benchmark report after this evidence ticket, or remain opt-in like the current `--latest-indexes` mode?

Risks
- Whole-scenario allocation numbers can hide the true in-memory hotspot order if the evidence does not separately isolate DB/EF overhead from DVault-owned canonicalization and save-preparation work.
- Because satellite `HashDiff` values are caller-supplied, the ticket can be overread if upstream payload-hash generation costs are mixed into the DVault hotspot summary.
- A ranking taken only on the default SQLite/`sha256-v1`/`HexString` baseline should not be generalized to provider-specific or non-default hash-key variants without follow-up validation.

Split recommendations
- Keep this ticket evidence-only. If the ranking surfaces independent hotspot families, land follow-up implementation tickets separately for stable-hash canonicalization/hash generation and for satellite replay/save-preparation allocation reduction.

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