[gicket-bot] PO refinement contract

Summary
- Refined the ticket to deliver reviewable SQLite schema snapshot regression coverage against the current DVault metadata-translation baseline and to defer migration-specific snapshot coverage to later scoped work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- For this ticket, the bounded v1 provider baseline is the existing SQLite integration test path under tests/DCoding.Data.DVault.Tests/Integration using the current EF Core 10 SQLite dependency.
- "Representative models" means the currently visible metadata surface: hubs, links, hub satellites, link satellites, deterministic table and column naming, primary keys, and indexes.
- The current repository does not yet carry a migration framework or design-time migration baseline, so the required deliverable for this ticket is schema snapshot coverage, not migration snapshot coverage.

Scope In
- Add repeatable regression tests that compare canonical generated SQLite schema output for representative DVault metadata models against committed expected baselines.
- Cover the schema elements currently produced by ApplyDataVaultMetadata, including table names, column names and order, primary key names and columns, and index names, columns, and uniqueness.
- Include representative fixtures that exercise at least one hub, one multi-participant link, one hub satellite, one link satellite, and a business-key ordering case that can detect unintended naming or ordering drift.
- Keep the work inside the existing DVault test layout and repository standards so it runs through DVault.slnx and the shared formatting gate.

Scope Out
- Migration-script or EF migration snapshot coverage for providers that do not already have an approved migration baseline on this branch.
- New provider support beyond the existing SQLite test baseline.
- A public migration framework, committed EF migration artifacts, design-time tooling, or broader provider-specific migration contracts.
- Broader schema-generation features, runtime configuration APIs, or advanced customization hooks not already present on the branch.

Open questions
- none

Follow-up questions
- Once DVault intentionally scopes migration behavior or adds EF design-time support, should provider-specific migration-script snapshots be tracked in a separate follow-up ticket?
- If future naming or provider override hooks land, should additional snapshot fixtures be added to cover customized conventions separately from the default baseline?

Risks
- Snapshot coverage that stores raw provider DDL without enough canonicalization may become brittle across EF Core or SQLite version changes.
- The ticket title mentions migrations, so an implementer could accidentally expand scope into deferred provider-specific migration infrastructure unless the refined scope is followed.
- Replacing all structural assertions with a single opaque blob snapshot could make failures harder to diagnose if the canonical output is not kept focused and readable.

Split recommendations
- If migration-specific output requires Microsoft.EntityFrameworkCore.Design, committed model snapshots, or design-time services, split that work into a separate ticket after migration behavior is intentionally scoped.
- If additional database providers need equivalent coverage later, create provider-specific snapshot tickets rather than widening this ticket beyond the current SQLite baseline.

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