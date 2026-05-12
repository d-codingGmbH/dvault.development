[gicket-bot] PO refinement contract

Summary
- Refined ticket 06F0MEGPPETJD4ZDEN5ESGR7JW using the live ticket snapshot, comments, relation state, referenced planning contracts, and current source/test layout. No bounded planning writes were needed; the ticket is ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Recent comments contain only bot claim and lease metadata; there are no human scope changes to incorporate.
- Ticket attachments are empty; the referenced repository documents are sufficient planning context for this refinement.
- Existing parentOf relations already split the story to child tickets 06F0MEGYHADPVN575H64D56W2G, 06F0MEH660Y5QTNR5P8JPS2QXC, 06F0MEHDFYCVK42FFY77FXHXBR, and 06F0MEHKYTBJEJH2DVZ2CFH9Z0; incoming parentOf relation from 06F0MEDTB8496GYVM9K42F9VPG is retained.
- The PIT v1 baseline is the DataVaultPitMetadata contract in docs/plans/06F0MEGYHADPVN575H64D56W2G-pit-backed-as-of-read-api-contract.md, not the historical DataVaultPointInTimeMetadata surface.
- The bridge baseline is the provider-neutral many-to-many and hierarchy metadata contract in docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md.
- Repository evidence shows PIT and bridge read-helper source files plus unit and SQLite integration test files already present on the branch, so naming and layout are ratified rather than reopened.

Scope In
- Provider-neutral PIT as-of read request, raw row API, and typed projection helper on the existing IDataVaultReadService boundary.
- PIT reads for one DataVaultPitMetadata declaration with one hub parent and ordered ordinary hub-attached satellite references.
- Bridge traversal read/query helper contract and baseline implementation for many-to-many From/To traversal and hierarchy Ancestor/Descendant traversal.
- Bridge raw read records and typed projection rows that expose exact generated endpoint hash-key column names and TraversalDepth for hierarchy bridges.
- Deterministic diagnostics for unsupported PIT or bridge metadata shapes and inconsistent generated EF shared-type metadata.
- Focused correctness coverage over generated PIT and bridge shared-type tables, public API shape, request validation, missing data behavior, and SQLite integration.

Scope Out
- Provider-specific read query tuning, SQL hints, physical optimization, or provider-specific DDL behavior.
- PIT row refresh, PIT maintenance orchestration, late-arriving reconciliation, and persisted snapshot population jobs.
- Bridge row maintenance, hierarchy closure computation, path payload columns, effectivity windows, EF relationships, navigations, or foreign keys.
- Link-based PIT parents, link-attached PIT satellites, multi-active PIT satellites, bridge-driven PIT reads, and legacy PointInTime modeling APIs.
- A full graph query engine, arbitrary path traversal, or unbounded recursive hierarchy semantics.
- Reflection-based DTO binding or ad hoc table-name read APIs outside declared metadata requests.

Open questions
- none

Follow-up questions
- Decide in a later ticket whether PIT refresh or bridge row/closure maintenance should be owned by DVault or left entirely to caller-managed jobs.
- Decide after the provider-neutral baseline lands whether any provider-specific read optimizations are worth adding for SQLite, PostgreSQL, SQL Server, MySQL, or Oracle.
- Decide whether user-facing README or quickstart examples should be expanded once the read helpers are fully verified.
- Review v0.6 release-note known limitations after implementation ships so shipped read helpers are not still described as unavailable.

Risks
- Consumers may expect PIT or bridge helpers to populate maintenance tables; diagnostics and documentation must keep the read-only, source-backed boundary explicit.
- Generated shared-type metadata drift could produce confusing runtime failures unless validation remains deterministic and names the metadata/table/property involved.
- Hierarchy bridge reads depend on precomputed rows and bounded maximum-depth filtering; they do not prove recursive traversal correctness beyond the generated table contents.
- PIT timestamp conversion must stay provider-neutral and caller-facing; storage-mode regressions can create subtle as-of boundary errors.

Split recommendations
- No additional split is recommended now; the story already has four parentOf child tickets materialized and the current repository layout gives bounded PIT, bridge, and test ownership surfaces.

Persisted contract coverage
- acceptance-criteria items: 9
- definition-of-done items: 6
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment