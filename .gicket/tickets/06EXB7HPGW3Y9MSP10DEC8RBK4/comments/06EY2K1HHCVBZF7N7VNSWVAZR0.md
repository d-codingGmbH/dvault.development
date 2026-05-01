[gicket-bot] PO refinement contract

Summary
- Refined this ticket as an additive extension of the explicit save-service baseline: persist satellite history only when the latest stored hash diff for the same parent changes; no split or planning artifact was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket extends the existing explicit IDataVaultSaveService and DataVaultSaveRequest write boundary; it does not introduce SaveChanges interception or another implicit persistence path.
- For v1, unchanged means the incoming satellite hash diff matches the latest persisted satellite version for the same parent hash key.
- For v1, changed means the incoming hash diff differs from the latest persisted version for the same parent hash key, so a new historical satellite row is inserted with the request load timestamp and record source.
- A payload that returns to an older historical value after an intervening change still counts as a new version because comparison is against the current latest version, not any historical match.
- Satellite historization in this ticket applies to the existing metadata model for satellites attached to either a hub or a link under the current SQLite-focused v1 baseline.

Scope In
- Add satellite persistence support to the explicit save-service flow alongside the current hub and link save operations.
- Persist parent hash key, payload columns, hash diff, load timestamp, and record source using the repository's existing satellite metadata and naming conventions.
- Suppress insertion when the latest persisted hash diff for the same parent is unchanged.
- Insert a new historical row when the latest persisted hash diff for the same parent changes.
- Add automated SQLite-oriented tests covering changed, unchanged, and parent-scoped historization behavior.

Scope Out
- PIT tables, bridge tables, multi-active satellites, and other deferred post-MVP Data Vault capabilities.
- Provider-specific upsert, retry, or multi-writer concurrency behavior beyond the current SQLite v1 baseline.
- A new implicit persistence mechanism such as EF SaveChanges interception.
- Repository-wide changes to hashing algorithms, naming policy, or non-satellite hub/link behavior beyond what satellite support needs.

Open questions
- none

Follow-up questions
- Should a later ticket add a higher-level caller API for satellite payload submission once the first explicit satellite save path is in place?
- Should multi-active satellite semantics be planned as a separate capability ticket instead of being inferred from this v1 historization rule?
- Should provider-specific conflict handling or SQL-side change detection be revisited once non-SQLite provider profiles are scheduled?

Risks
- The current provider baseline explicitly does not declare multi-writer concurrency support, so unchanged-row suppression relies on deterministic pre-insert lookup rather than provider-neutral conflict handling.
- If upstream save-service request or result contracts change before integration, the satellite operation shape may need small coordinating adjustments.
- If tests do not lock the latest-version comparison rule clearly, future refactors could accidentally treat any historical hash-diff match as unchanged and lose intended history.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment