[gicket-bot] PO refinement contract

Summary
- Refined current child ticket under story 06EZ0NCAFFJSSRFFEG66AYG8XC around the existing SQLite benchmark harness: compare classic EF, provider-neutral AddDVault fallback, and SQLite AddDVaultSqlite optimized rows in documentation artifacts, keep artifacts/benchmarks as the bounded output root, and leave external-provider expansion to follow-up work. No new attachments, child tickets, relations, or planning documents were created in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ticket 06EZ0NCGYCADKEYGR16J5PJFS0 remains a parentOf child of story 06EZ0NCAFFJSSRFFEG66AYG8XC and refines only the benchmark-artifact slice of that story.
- The current repository baseline is the repo-local SQLite harness in benchmarks/DCoding.Data.DVault.Benchmarks; v0.5 required benchmark coverage is SQLite-only, so this ticket does not need live Postgres, MySQL, Oracle, or SQL Server execution.
- The comparison set for this ticket is classic EF, provider-neutral AddDVault fallback, and the SQLite AddDVaultSqlite path that currently registers SqliteDataVaultSaveStrategy on the same local SQLite provider.
- The existing large change-heavy baseline is customer-profile-bulk-history with 100 customers and 10 profile states each.
- The large insert-only baseline should be the matching customer-profile bulk shape with one initial profile state per customer so the dataset family stays comparable inside the current benchmark contract.
- Benchmark artifact output stays under artifacts/, with artifacts/benchmarks ratified as the default bounded output location.
- No ticket attachments, child tickets, or planning documents were created in this refinement run.

Scope In
- Extend the existing benchmark runner and artifact schema so each emitted comparison row carries provider, strategy family, dataset-size metadata, and change-ratio metadata alongside timing and persisted-outcome fields.
- Add provider-neutral fallback benchmark rows alongside the existing classic EF and SQLite optimized DVault rows.
- Represent one large insert-only customer-profile bulk scenario and the existing large change-heavy customer-profile bulk scenario.
- Keep the existing smaller customer-profile and order-product scenarios if they continue to provide comparison context and persisted-outcome proof.
- Update benchmark documentation and automated coverage for the expanded comparison artifacts.

Scope Out
- Live external-provider benchmark execution or provider-package matrices beyond the current SQLite local temporary-file harness.
- Changing production save-service architecture, provider strategy dispatch contracts, or non-benchmark product behavior.
- CI performance gates, historical trend storage, dashboard publication, or checked-in benchmark result snapshots.
- Tracked bin/ or obj/ outputs or artifact paths outside artifacts/.

Open questions
- none

Follow-up questions
- After additional provider benchmarks exist, should a follow-up ticket add explicit skipped-provider rows and opt-in external-provider comparisons without changing the current SQLite-required baseline?
- When ticket 06EZ0NA7CWDYJ7ZS3K5GM0187M or later provider-optimization work lands, should the benchmark artifact schema add PostgreSQL or other provider rows as a separate expansion ticket?
- Should later documentation work check in one sample provider-comparison artifact or attach one to a release ticket once the first reference-machine run is accepted?

Risks
- If documentation copies benchmark numbers without the emitted provider, runtime, and hardware context, readers may overgeneralize SQLite local measurements.
- If the new dataset-size or change-ratio labels diverge between markdown, CSV, and JSON, the comparison artifacts stop being machine- and doc-friendly.
- If the artifact schema hardcodes SQLite-only assumptions too tightly, later provider-expansion tickets may need a format revision.

Split recommendations
- No additional split is required for this ticket after bounding it to the existing SQLite benchmark harness, fallback/optimized/classic strategy comparison, and one added large insert-only scenario.
- If stakeholders later want live external-provider execution or skipped-provider reporting, handle that as separate provider-expansion tickets instead of widening this task.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment