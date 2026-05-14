[gicket-bot] PO refinement contract

Summary
- Refined the ticket to an explicit opt-in SaveChanges metadata interceptor slice for current LoadTimestamp and RecordSource roles; the done incoming blocker is historical and no planning writes were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows the only existing technical metadata roles are LoadTimestamp and RecordSource; this slice should target those current roles and not reopen batch, correlation, or tenant metadata families.
- AddDVault() currently resolves the explicit save-service baseline and registers no ISaveChangesInterceptor; the new interceptor must remain explicit opt-in rather than altering the default write path.
- The incoming blocks relation from epic 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 is historical and non-blocking because that source ticket is already done; no relation cleanup was materialized in this pass.
- No child tickets, relation updates, attachments, or planning documents were created in this pass.

Scope In
- Add an opt-in SaveChanges metadata interceptor slice for Added DVault hub, link, and satellite EF rows that can fill missing LoadTimestamp and RecordSource technical metadata.
- Add explicit configuration and registration surface(s) for enabling the interceptor on a DbContext without changing the default AddDVault() behavior.
- Prove deterministic SQLite integration coverage for automatic population and manual-value preservation across sync and async save paths.
- Honor current DVault metadata annotations and effective column names when deciding which properties to populate.

Scope Out
- No default interceptor registration on the normal AddDVault() path.
- No hash key, hash diff, business-key, participant-reference, or payload derivation in the interceptor.
- No batch id, correlation id, tenant metadata, PIT or bridge projection metadata, or broader audit framework.
- No change to IDataVaultSaveService as the default write boundary.
- No provider-specific behavior claim beyond the existing SQLite proof baseline.

Open questions
- none

Follow-up questions
- If the broader story still needs batch id, correlation id, or tenant or source metadata, should those arrive as separate technical-role additions before any interceptor attempts to populate them?
- After this slice lands, should a later ticket unify interceptor-local metadata sourcing with the broader advanced-configuration hook story for timestamp and record-source defaults?

Risks
- If the interceptor starts computing hash keys, hash diffs, or non-technical values, it will blur the explicit IDataVaultSaveService boundary that the repository has already ratified.
- If sync and async paths resolve values differently, callers could observe inconsistent lineage metadata.
- If the implementation targets literal property names instead of DVault annotations, models with effective-name overrides will regress.

Split recommendations
- No split recommended for the current ticket; repository evidence supports one bounded slice around explicit opt-in population of existing LoadTimestamp and RecordSource roles.
- If development pressure expands into batch, correlation, tenant metadata, or non-Added update behavior, split that work into follow-up tickets instead of widening this slice.

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