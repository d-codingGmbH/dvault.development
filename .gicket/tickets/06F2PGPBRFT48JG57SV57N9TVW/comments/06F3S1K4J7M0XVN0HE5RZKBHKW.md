[gicket-bot] PO refinement contract

Summary
- Repository evidence already contains the PIT maintenance contract, split tickets, and blocking chain; this story is refined to the provider-neutral explicit PIT maintenance baseline with no new planning writes needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already contains docs/plans/pit-maintenance-service-v1-contract.md with this ticket as the primary ticket and 06F2PGPKXWRFXNPFA1JR0X67XC, 06F2PGPRGN0EVGD6RY5KY9M56W, and 06F2PGPXVAYRBC94RQ7X5V4DVG as related follow-on tickets.
- Persisted relation events already model the delivery chain: 06F2PGPBRFT48JG57SV57N9TVW blocks 06F2PGPKXWRFXNPFA1JR0X67XC and 06F2PGPXVAYRBC94RQ7X5V4DVG; 06F2PGPKXWRFXNPFA1JR0X67XC blocks 06F2PGPRGN0EVGD6RY5KY9M56W; 06F2PGPRGN0EVGD6RY5KY9M56W blocks 06F2PGPXVAYRBC94RQ7X5V4DVG.
- Ticket comments contain only bot claim and lease entries; there are no human comments adding scope or blocking questions.
- Existing README and docs/releases/v0.7.0.md already ratify that PIT reads consume already materialized PIT tables and do not refresh or maintain them implicitly.

Scope In
- Add one additive explicit PIT maintenance service in DCoding.Data.DVault, registered beside the existing explicit save and read services through AddDVault(...).
- Support a full rebuild for one DataVaultPitMetadata declaration and bounded incremental maintenance for explicit parent hash keys.
- Keep the maintenance baseline provider-neutral and deterministic by recomputing PIT rows from distinct satellite LoadTimestamp values in ascending order and populating each <Satellite>LoadTimestamp snapshot from the latest visible satellite row at or before each PIT timestamp.
- Validate and fail before writes when PIT metadata, generated PIT entities, or participating satellites fall outside the supported v1 PIT shape.
- Add unit and SQLite integration coverage for rebuild semantics, bounded parent maintenance, missing satellite segments, and late-arriving satellite corrections.

Scope Out
- Changing IDataVaultReadService PIT query semantics, typed PIT projection ergonomics, or current/as-of query API shape; that follow-on work stays in 06F2PGPKXWRFXNPFA1JR0X67XC.
- Provider-aware PIT or bridge read optimization; that stays in 06F2PGPRGN0EVGD6RY5KY9M56W.
- User-facing README and v0.15.0 release-note follow-through beyond the contract handoff; that stays in 06F2PGPXVAYRBC94RQ7X5V4DVG.
- Legacy DataVaultPointInTimeMetadata and DataVaultModelBuilder.PointInTime(...), link-parent PITs, multi-active PITs, bridge maintenance, hosted or background orchestration, and provider-specific maintenance strategies.

Open questions
- none

Follow-up questions
- When 06F2PGPXVAYRBC94RQ7X5V4DVG is implemented, should the v0.15.0 release notes create a new docs/releases/v0.15.0.md file, since the repository currently stops at v0.14.0?
- After the provider-neutral baseline lands, which providers justify dedicated PIT maintenance performance work beyond the existing read-optimization ticket?
- Should the legacy PointInTime surface be formally deprecated in a later release once the DataVaultPitMetadata maintenance and read flow is complete?

Risks
- Provider-neutral full rebuild and parent-scoped recomputation can be expensive on large PIT tables, so v1 should not imply provider-specific physical tuning or hosted orchestration.
- The repository still contains both legacy PointInTime and newer DataVaultPitMetadata surfaces, so the implementation must avoid accidentally merging or renaming those contracts.
- If the downstream documentation ticket does not update README and v0.15.0 release notes promptly, public guidance will still describe PIT reads without the new maintained-PIT baseline.

Split recommendations
- No further split is recommended; the repository already contains the durable planning split through docs/plans/pit-maintenance-service-v1-contract.md and tickets 06F2PGPKXWRFXNPFA1JR0X67XC, 06F2PGPRGN0EVGD6RY5KY9M56W, and 06F2PGPXVAYRBC94RQ7X5V4DVG.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment