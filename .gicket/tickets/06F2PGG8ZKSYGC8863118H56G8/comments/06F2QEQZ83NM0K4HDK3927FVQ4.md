[gicket-bot] PO refinement contract

Summary
- Refinement resets this ticket to pending developer implementation for PostgreSQL, SQL Server, Oracle, and MySQL live-schema readers; current branch evidence is still SQLite-only with no src/ or tests/ diff versus develop, so it is not ready to return to PO-critic.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The prior closure implication is removed. This ticket remains unfinished developer work; the branch has no provider-reader product or test implementation evidence, so it is not being treated as ready for closure or handoff.
- critic-item-2: `answered` - No alternate implementation branch, ref, commit, or test evidence is present in the supplied ticket context. Treat the work as unimplemented developer scope and do not return it to PO-critic until src/ and tests/ evidence exists.
- critic-item-3: `answered` - This refinement keeps PostgreSQL, SQL Server, Oracle, and MySQL live-schema readers as future developer work rather than implying they already landed. Current repository docs remain SQLite-first today, so this ticket must earn broader provider support through new code and tests instead of assuming it on this branch.
- critic-item-4: `answered` - Confirmed. Compared with develop, the branch contains no src/ or tests/ changes, so a closure-only or implementation-complete handoff is unsupported by repository evidence.
- critic-item-5: `answered` - Confirmed. DataVaultLiveSchemaReader.ReadAsync(...) still dispatches only SQLite and returns UnsupportedProvider for non-SQLite providers, so the non-SQLite reader acceptance criteria remain target behavior and are not yet satisfied.
- critic-item-6: `answered` - Confirmed. The current branch provides no direct PostgreSQL, SQL Server, Oracle, or MySQL live-schema integration evidence; provider-specific success-path coverage in tests is still required before Definition of Done is met.

Clarifications
- Current branch evidence versus develop shows no product or test changes under src/ or tests; this ticket remains actual developer work.
- DataVaultLiveSchemaReader.ReadAsync(...) still routes only Microsoft.EntityFrameworkCore.Sqlite; recognized non-SQLite providers still resolve to UnsupportedProvider today.
- No alternate implementation branch, commit, comment, or attachment evidence was supplied in the current ticket context.
- Current repository docs still describe SQLite as the present first-class live-schema reader boundary; broader provider support must be established by new code and tests on this ticket.
- The parent story relation from 06F2PGFZWC5PXSDH46RCZPN1CG remains in place, and the incoming blocks relation from 06F2PGG57K3S7CJQP5QX9AWW3G is historical prerequisite context rather than evidence that this ticket is implemented.
- No child tickets, relation changes, or planning documents were materialized in this refinement run.

Scope In
- Implement built-in live-schema reader dispatch and catalog readers for PostgreSQL, SQL Server, Oracle, and MySQL in DataVaultLiveSchemaReader.ReadAsync(...).
- Preserve the existing public DataVaultLiveSchemaReadResult contract and snapshot object model while extending non-SQLite provider support.
- Add provider-specific external opt-in integration coverage that provisions isolated provider objects through the existing fixture helpers and verifies the canonical expected snapshot.
- Keep deterministic ordering and existing provider-name baselines consistent with current drift-comparison and provider-capability infrastructure.

Scope Out
- Claiming non-SQLite provider-reader support is already delivered without src/ and tests/ evidence on this branch.
- Adding new public API types, statuses, snapshot fields, or drift-difference semantics.
- Expanding live-schema reading to foreign keys, views, sequences, triggers, arbitrary non-DVault objects, migration repair, or automatic schema changes.
- Broad CI, README, or release-note rollout beyond minimal coherence updates here or explicit handoff to 06F2PGHA0EXJRGDHM4GQM7NPYR.

Open questions
- What exact branch, ref, or commit and corresponding src/ plus tests/ evidence will be attached once the non-SQLite provider-reader implementation exists for re-review?

Follow-up questions
- After first-class readers land, should 06F2PGHA0EXJRGDHM4GQM7NPYR add SQL Server, Oracle, and MySQL fixture walkthroughs comparable to the existing PostgreSQL guidance?
- Do later CI or tooling tickets want opt-in smoke commands for the new provider readers, or should live-provider verification remain developer-managed and off by default?

Risks
- Provider catalogs differ in schema scoping, identifier casing, index metadata shape, and storage-type text, so normalization mistakes can create false drift or hide real mismatches.
- Oracle physical-name limits and MySQL dual provider-name support make identifier mapping and dispatch handling more brittle than the existing SQLite path.
- External opt-in coverage depends on configured provider packages and create or drop permissions in developer-managed databases, so some defects can remain latent when those lanes are not exercised.
- If the ticket is sent back to PO-critic again without non-ticket src/ and tests/ evidence, it will fail on the same repository-gap findings.

Split recommendations
- No child tickets or relation changes were materialized in this run. Keep this as one bounded implementation ticket unless product explicitly narrows current release scope back to SQLite-only; if that happens, move first-class PostgreSQL, SQL Server, Oracle, and MySQL readers into a separate implementation ticket instead of leaving them implied here.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment