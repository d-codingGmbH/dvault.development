[gicket-bot] PO refinement contract

Summary
- Refined the SQLite relational-schema story as an umbrella over the already-materialized child tasks 06EXB7GESWZZTZG7XYAKTTKQRW and 06EXB7GPRGEJHKFMJ8MVAVF8ZG; repository evidence fixes the v1 baseline to SQLite schema creation via EnsureCreated plus committed schema snapshot coverage, so no new split or planning artifact was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current split is already materialized: child ticket 06EXB7GESWZZTZG7XYAKTTKQRW ('Task: Map hubs, links, and satellites to Sqlite tables') and child ticket 06EXB7GPRGEJHKFMJ8MVAVF8ZG ('Task: Add schema and migration snapshot tests') are both linked from this story through existing parentOf relations and both are already done.
- Repository evidence already fixes the bounded v1 provider baseline to the EF Core 10 SQLite path in src/DCoding.Data.DVault and tests/DCoding.Data.DVault.Tests, including ApplyDataVaultMetadata, DataVaultProviderCapabilityProfiles.Sqlite, SqliteDataVaultSchemaTests, and the committed SqliteDataVaultSchemaSnapshot.txt artifact.
- The story's 'migration or create-database flow' acceptance point resolves to the create-database flow via context.Database.EnsureCreated(); the repository does not yet carry an approved migration framework or design-time migration baseline, so migration artifacts are out of scope for this story.
- No new child tickets, relations, attachments, or planning documents were created in this refinement pass because the necessary split and repository baseline already exist.

Scope In
- Keep this ticket as the story-level umbrella/tracking item for generating usable SQLite relational schema from the existing DVault EF metadata baseline.
- Treat 06EXB7GESWZZTZG7XYAKTTKQRW as the implementation owner for SQLite table, column, primary-key, and index mapping for hubs, links, and satellites.
- Treat 06EXB7GPRGEJHKFMJ8MVAVF8ZG as the implementation owner for schema-regression coverage and the reviewable SQLite schema snapshot baseline.
- Keep the story aligned with the current repository surface in src/DCoding.Data.DVault and tests/DCoding.Data.DVault.Tests, including the deterministic naming and ordering already fixed by source and tests.

Scope Out
- New developer-owned implementation on the parent story beyond the already materialized child tickets.
- EF migration infrastructure, design-time migration services, committed migration artifacts, or migration-specific snapshot baselines.
- Non-SQLite providers, provider-specific optimizations, or advanced configuration hooks.
- Foreign keys, navigations, runtime data-loading behavior, or broader persistence features beyond the current provider-neutral metadata translation and SQLite schema baseline.

Open questions
- none

Follow-up questions
- If the project later approves EF migration behavior or design-time services, should migration-specific output move to a separate follow-up ticket rather than reopening this umbrella story?
- Once this story advances, should the existing blocks relations from 06EXB7G6YE4X0GA0CT7EPEFMPR to downstream example stories 06EXB7RPKGTEW4RZKYQ2DXS554 and 06EXB7SEAWB2KSBQSHQB2MVV38 remain as story-level dependencies or be re-pointed to a more concrete still-open ticket if one exists?

Risks
- If the parent story drifts back toward executable developer scope, automation can duplicate work that is already completed in child tickets 06EXB7GESWZZTZG7XYAKTTKQRW and 06EXB7GPRGEJHKFMJ8MVAVF8ZG.
- If later work treats migration support as implied by this story title, scope can leak into design-time or provider-specific infrastructure that the current repository baseline does not support.
- Downstream example tickets may carry stale blocker relations if relation hygiene is not reviewed after the umbrella story advances.

Split recommendations
- No additional split is recommended; the concrete implementation and regression-coverage slices are already materialized through child tickets 06EXB7GESWZZTZG7XYAKTTKQRW and 06EXB7GPRGEJHKFMJ8MVAVF8ZG and their existing sequencing relation.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment