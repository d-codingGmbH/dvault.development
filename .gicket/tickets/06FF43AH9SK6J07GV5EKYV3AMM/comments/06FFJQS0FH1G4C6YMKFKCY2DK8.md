[gicket-bot] PO refinement contract

Summary
- Refined the ticket against current repository contracts: PostgreSQL PIT maintenance already exists as a bounded provider strategy, but the benchmark harness still lacks a `pit-full-rebuild-maintenance` lane, so this ticket is the benchmark and evidence slice for PostgreSQL only. No bounded child tickets, relation writes, description edits, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the maintenance scenario name as `pit-full-rebuild-maintenance`; `pit-as-of-read` and `bridge-traversal-read` must not be reused as PIT maintenance evidence.
- `AddDVaultPostgres()` already registers `PostgresDataVaultPitMaintenanceStrategy`, and the current supported optimized full-rebuild shapes are ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs on clean Npgsql-backed contexts with no ambient caller transaction.
- The current benchmark harness emits provider-native save plus latest-satellite, PIT, and bridge read rows, but no PIT full-rebuild maintenance rows yet, so this ticket is additive benchmark coverage rather than capability design.
- No bounded ticket writes were applied during refinement.

Scope In
- Add PostgreSQL `pit-full-rebuild-maintenance` benchmark coverage under the existing benchmark triplet and run-context contract.
- Compare `dvault-adddvault-fallback` provider-neutral full rebuilds with `dvault-adddvaultpostgres-optimized` on the PostgreSQL external-provider lane.
- Cover the repository-approved PostgreSQL full-rebuild shape boundary: ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs.
- Add verifier and test coverage that preserves both completed-row and skipped-placeholder behavior for the new maintenance lane.

Scope Out
- Any reinterpretation of `pit-as-of-read` or `bridge-traversal-read` as PIT maintenance timing evidence.
- Ambient-caller-transaction optimization, dirty-context optimization, `MaintainParentsAsync(...)`, automatic PIT refresh, or bridge-maintenance push-down.
- SQL Server, MySQL, Oracle, or DB2 PIT maintenance benchmark lanes beyond any reusable helper extraction a developer needs internally.
- New artifact files or benchmark schema changes beyond the existing `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` contract.

Open questions
- none

Follow-up questions
- After the PostgreSQL lane lands, should the sibling SQL Server PIT maintenance benchmark work reuse the same `pit-full-rebuild-maintenance` scenario family and token contract while keeping its own narrower fallback vocabulary?

Risks
- Completed timing evidence still depends on an opt-in PostgreSQL environment; absent or unreachable PostgreSQL must yield skipped-placeholder rows rather than omitted rows.
- If implementation blends caller-transaction fallback cases or unsupported PIT shapes into optimized timing claims, it will violate the current PIT maintenance evidence boundary.
- Because PostgreSQL supports three approved full-rebuild shapes, row metadata must stay deterministic enough for later evidence citations to distinguish the exercised shape from general provider availability.

Split recommendations
- Keep SQL Server PIT maintenance timing as a sibling ticket because it uses a different runtime seam (`SqlServerDataVaultPitMaintenanceService`) and fallback vocabulary from PostgreSQL.
- Any future MySQL, Oracle, or DB2 PIT maintenance benchmarking should remain separate from this ticket until their runtime maintenance lanes are implemented or explicitly accepted beyond the current repository boundary.

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