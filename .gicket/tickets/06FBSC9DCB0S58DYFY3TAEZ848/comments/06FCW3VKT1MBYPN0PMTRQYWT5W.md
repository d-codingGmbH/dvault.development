[gicket-bot] PO refinement contract

Summary
- Repository and documentation evidence show PostgreSQL bulk support is already implemented and threshold-bounded; this ticket should refine to an evaluation outcome that defers code or threshold changes until new provider-configured benchmark evidence exists.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The supplied ticket snapshot already shows no recent comments.
- PostgreSQL bulk support is present today: `AddDVaultPostgres()` registers `PostgresDataVaultSaveStrategy`, with retained direct or UNNEST work below 60 operations and staged `COPY` at 60-plus operations.
- The exact direct-versus-UNNEST crossover is already repository-defined inside the current implementation (`PostgresUnnestInsertMinimumRowCount = 32`) and is not an open PO-level decision.
- The checked-in v0.32 PostgreSQL evidence bundle already records completed wins for both lanes, while the v0.39 root benchmark triplet still skips PostgreSQL provider rows when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

Scope In
- Ratify the current PostgreSQL save-path baseline from code, tests, and documentation.
- Record the bounded recommendation for this ticket as `defer with reason` on implementation or threshold changes.
- Point delivery to the authoritative evidence surfaces that distinguish existing implementation from missing provider-configured timing evidence.

Scope Out
- New PostgreSQL provider code, new latest-satellite or PIT or bridge strategy work, or benchmark harness changes.
- Threshold retuning without a new PostgreSQL benchmark triplet that compares the current 60-operation boundary against provider-neutral fallback.
- Provider provisioning, connection-string setup, or re-running local Podman benchmarks.
- Ticket-graph cleanup beyond noting that relation reads were unavailable through the blocked local transport.

Open questions
- none

Follow-up questions
- If a later ticket wants to challenge the 60-operation threshold, which exact PostgreSQL before-and-after benchmark triplet and workload shapes should be required so the change is compared against the existing v0.32 evidence rather than an ad hoc local run?
- Should future PostgreSQL evidence collection stay under the shared provider gap-matrix execution backlog or be broken out as a dedicated benchmark ticket once relation reads are available again?

Risks
- Changing the threshold now without a new provider-configured benchmark triplet would replace a repository-backed boundary with guesswork.
- The v0.39 root benchmark triplet still shows PostgreSQL provider rows as skipped placeholders, so a future developer could misread the root summary unless this ticket explicitly points them to the v0.32 completed evidence bundle.
- Relation state could not be re-verified through the blocked local gicket transport during this run; if downstream workflow depends on exact ticket links, a later tool-enabled pass should confirm them.

Split recommendations
- No split recommended. If follow-up work is opened later, make it a dedicated provider-configured PostgreSQL evidence-collection ticket rather than mixing new benchmarks into this evaluation ticket.

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