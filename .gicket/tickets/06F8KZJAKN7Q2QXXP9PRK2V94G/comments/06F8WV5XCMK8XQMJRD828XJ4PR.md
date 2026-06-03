[gicket-bot] PO refinement contract

Summary
- Refined the story to extend the existing SQLite-only optimized PIT/bridge dispatch baseline with diagnostics-gated PostgreSQL and SQL Server strategy candidates, while preserving safe provider-neutral fallback and making no ticket or planning writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the v1 baseline: PIT and bridge reads stay on the existing provider-neutral IDataVaultReadService boundary, consume already maintained read-model tables, and do not add automatic maintenance or SaveChanges-time refresh.
- Current published boundary says SQLite is the only repository-proven optimized PIT/bridge provider path today; this story adds PostgreSQL and SQL Server candidates inside that bounded model rather than introducing new PIT/bridge APIs.
- Supported shape scope is the existing maintained PIT/bridge boundary: supported PIT reads only for published hub-parent or bounded link-parent PIT shapes, and supported bridge reads only for published many-to-many and hierarchy bridge shapes.
- Repository structure already includes dedicated provider packages for PostgreSQL and SQL Server, so the default implementation location is the existing provider-support layer rather than new packages or generator surfaces.
- Recent ticket comments contain only bot claim/lease metadata, and no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this PO pass.

Scope In
- Diagnostics-gated PostgreSQL PIT optimized read strategy candidates for the already supported maintained PIT shapes.
- Diagnostics-gated SQL Server PIT optimized read strategy candidates for the already supported maintained PIT shapes.
- Diagnostics-gated PostgreSQL bridge optimized read strategy candidates for the already supported maintained bridge shapes.
- Diagnostics-gated SQL Server bridge optimized read strategy candidates for the already supported maintained bridge shapes.
- Safe fallback to the existing provider-neutral PIT and bridge read pipelines whenever provider, shape, maintenance evidence, or freshness evidence does not qualify.

Scope Out
- New PIT or bridge metadata shapes, request semantics, or public IDataVaultReadService API changes.
- PIT or bridge maintenance orchestration, automatic refresh, SaveChanges integration, or background scheduling.
- Typed read-model helper generator changes beyond consuming the existing runtime read boundary.
- Provider support expansion for MySQL, Oracle, or other databases in this ticket.

Open questions
- none

Follow-up questions
- After PostgreSQL and SQL Server land, should the documented optimized-provider matrix be revisited for MySQL or Oracle, or remain SQLite/PostgreSQL/SQL Server only until new benchmark evidence exists?
- Should release-note and adoption-checklist language explicitly call out any provider-specific supported-shape exclusions discovered during implementation?

Risks
- The ticket still has a live incoming blocks relation from 06F8KZHZ27SDTNCFNMFDQRVCKM, so delivery sequencing may still depend on upstream work even though PO refinement is complete.
- Provider-specific SQL paths can drift from provider-neutral semantics unless parity tests cover bounded multi-active PIT behavior and hierarchy bridge traversal depth rules.
- If stale-maintenance or read-shape evidence checks fail open instead of failing closed to fallback, optimized reads could return incorrect PIT or bridge results.

Split recommendations
- No split was materialized; keep the story whole if implementation stays limited to existing PIT and bridge shapes and fallback safety rules.
- If provider-specific SQL, tests, and benchmark evidence expand beyond that boundary, split next by provider rather than by public API surface.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment