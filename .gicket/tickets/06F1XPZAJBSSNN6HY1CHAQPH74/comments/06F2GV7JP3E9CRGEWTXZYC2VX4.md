[gicket-bot] PO refinement contract

Summary
- Aligned the story to the repository's bounded interceptor baseline: explicit opt-in SaveChanges metadata population for LoadTimestamp and RecordSource only, with broader lineage metadata and consumer-guide work kept separate.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the v1 technical metadata baseline as HashKey, HashDiff, LoadTimestamp, and RecordSource; this story's interceptor scope auto-populates only missing LoadTimestamp and RecordSource values.
- The concrete implementation slice is already represented by done child 06F1XPZS9SNK93JNKC02B63QG4, so this parent story should not reopen a second overlapping dev slice.
- Opt-in happens per DbContext through UseDataVaultSaveChangesMetadataInterceptor(...); the default AddDVault() path remains interceptor-free and IDataVaultSaveService remains the default DVault write boundary.
- Target-column discovery is annotation-driven through DVault technical metadata annotations, so effective-name overrides such as LoadedAtUtc or SourceSystem remain in scope without property-name branching.
- Batch id, correlation id, tenant-bound lineage metadata, and overwrite-on-opt-in behavior are not part of this story's bounded v1 slice.

Scope In
- Bound the parent story to an explicit opt-in SaveChanges interceptor for Added DVault hub, link, and satellite rows.
- Expose and preserve the public DbContext opt-in configuration surface for interceptor registration and value supply.
- Populate configured LoadTimestamp and RecordSource values only when those targeted metadata values are absent.
- Preserve caller-supplied manual LoadTimestamp and RecordSource values by default.
- Use DVault metadata annotations to identify eligible technical columns, including renamed effective column names.
- Treat the existing child implementation, API snapshot, and SQLite tests as the concrete delivery slice for this story.

Scope Out
- No default interceptor registration on the normal AddDVault() path.
- No HashKey or HashDiff computation, mutation, or backfill inside SaveChanges interception.
- No batch id, correlation id, tenant inference, source trust classification, or broader audit metadata work.
- No update, delete, or non-Added-row interception behavior.
- No replacement or obscuring of the explicit IDataVaultSaveService write path.
- No broad README, examples, or adoption-guide refresh in this story.
- No broader verification claim than the current SQLite proof baseline for behavior coverage.

Open questions
- none

Follow-up questions
- Should a separate follow-up story cover batch id, correlation id, tenant-bound lineage metadata, or other technical roles beyond LoadTimestamp and RecordSource?
- If callers later need explicit overwrite modes for LoadTimestamp or RecordSource, should that be a separate story instead of broadening this safe-default slice?
- Should the documentation story add consumer guidance on when to choose the explicit save service versus the opt-in SaveChanges interceptor?

Risks
- If the parent story text stays broad, downstream reviewers may incorrectly assume this ticket delivers batch, correlation, tenant, or overwrite-mode behavior that the repository does not support.
- If SaveChanges interception expands beyond LoadTimestamp and RecordSource without a separate contract, ownership of HashKey and HashDiff behavior can become ambiguous.
- Current repository docs still emphasize the explicit save-service path; if later documentation is not updated carefully, consumer guidance can drift from the new optional opt-in behavior.
- Claiming broad provider validation beyond the SQLite proof baseline would overstate the repository evidence.

Split recommendations
- No new split is needed for the implemented interceptor slice; use done child 06F1XPZS9SNK93JNKC02B63QG4 as the concrete implementation record for this story.
- Keep broader lineage metadata families such as batch, correlation, tenant, or governance-specific source metadata in separate follow-up tickets.
- Keep README and adoption-guide expansion in the existing documentation lane instead of reopening this story's core implementation scope.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment