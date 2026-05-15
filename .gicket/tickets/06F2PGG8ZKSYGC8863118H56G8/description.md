<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Implement first-class live schema readers for PostgreSQL, SQL Server, Oracle, and MySQL so `DataVaultLiveSchemaReader.ReadAsync(...)` no longer treats recognized non-SQLite EF Core providers as unsupported.
- This ticket is an implementation task under Story `06F2PGFZWC5PXSDH46RCZPN1CG`; it is ready for developer work. Implementation evidence is expected from the dev/test phases, not from the pre-development PO/PO-critic handoff.
- Keep the public live-schema result contract stable and reuse the existing external opt-in fixture boundary for provider verification.

### PO Handoff
- decision: `ready_for_dev`
- meaning: ticket is sufficiently specified for developer implementation

### Clarifications
- Current baseline: `DataVaultLiveSchemaReader.ReadAsync(...)` supports SQLite and returns `UnsupportedProvider` for recognized non-SQLite providers.
- Provider names are already centralized in `DataVaultProviderCapabilityProfileSelection`; reuse those provider-name baselines, including both MySQL provider identifiers.
- Existing non-SQLite fixture contracts and conditional provider package references are scaffolding. They are useful starting points but do not by themselves satisfy this ticket.
- README and release-note updates for user-facing first-class non-SQLite reader support belong to ticket `06F2PGHA0EXJRGDHM4GQM7NPYR` after behavior lands.

### Scope In
- Add built-in reader dispatch and catalog readers for:
  - `Npgsql.EntityFrameworkCore.PostgreSQL`
  - `Microsoft.EntityFrameworkCore.SqlServer`
  - `Oracle.EntityFrameworkCore`
  - `Pomelo.EntityFrameworkCore.MySql`
  - `MySql.EntityFrameworkCore`
- Return `Succeeded` with DVault-owned tables, ordered columns, provider storage types, named primary keys, and secondary indexes that align with shared fixture expectations.
- Return `Unavailable` for recognized provider catalog/connectivity failures without widening the public result contract.
- Preserve deterministic ordering for tables, columns, primary-key columns, and indexes.
- Add direct external opt-in integration coverage for the non-SQLite providers through the existing `DVAULT_TEST_*_CONNECTION_STRING` boundary.

### Scope Out
- Adding new public live-schema result statuses, result types, or snapshot fields.
- Broad drift coverage for foreign keys, views, sequences, triggers, or arbitrary non-DVault objects.
- Changing package shape, provider package defaults, or CI container orchestration beyond what the tests already support.
- Treating fixture-only scaffolding as delivered behavior without direct `ReadAsync(...)` execution.

## Acceptance Criteria
- `DataVaultLiveSchemaReader.ReadAsync(...)` dispatches built-in readers for PostgreSQL, SQL Server, Oracle, and both supported MySQL provider names instead of returning `UnsupportedProvider` solely because the provider is non-SQLite.
- Each recognized non-SQLite reader returns `Succeeded` for a reachable provider fixture with DVault-owned tables, ordered columns with provider storage types, named primary keys, and secondary indexes matching expected snapshots.
- Recognized provider catalog/connectivity failures return `Unavailable`; unknown providers still return `UnsupportedProvider`.
- Tests under `tests/` directly execute `ReadAsync(...)` for PostgreSQL, SQL Server, Oracle, and MySQL through existing external opt-in fixture lanes and assert zero blocking drift against expected snapshots where the provider is configured.
- Existing SQLite success, unavailable, and unsupported-provider coverage remains intact.

## Definition of Done
- The implementation branch contains non-ticket `src/` changes for provider dispatch and catalog readers.
- The implementation branch contains non-ticket `tests/` changes proving direct provider-specific live-schema success paths while keeping the SQLite baseline coherent.
- New provider tests respect existing provider traits, external opt-in boundaries, and documented connection string variables.
- The result remains compatible with the parent story and does not require reworking already completed provider-scaffolding tickets.

## Implementation Notes
- Keep implementation near `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` and adjacent internal helpers unless a narrow supporting helper is clearly useful.
- Use raw `DbConnection`/`DbCommand` catalog queries or provider-safe EF Core metadata access where appropriate; keep provider-specific SQL internal.
- Normalize provider catalog quirks carefully: schema scoping, identifier casing, physical-name limits, MySQL provider-name aliases, index metadata, and storage-type strings.
- Prefer shared fixture expectations from `ExternalProviderLiveSchemaModelOptions.ExpectedSnapshot` and existing provider traits instead of duplicating divergent expected shapes.

## Open Questions
- none

## Risks
- Provider catalogs differ in schema scoping, identifier casing, index metadata shape, and storage-type text; normalization mistakes can create false drift or hide real mismatches.
- Oracle physical-name limits and dual MySQL provider-name handling make identifier mapping and dispatch logic more brittle than the SQLite path.
- External opt-in coverage depends on developer-managed databases, so unconfigured provider lanes must skip cleanly while configured lanes verify real behavior.

## Split Recommendations
- Keep this as one bounded implementation ticket unless provider catalog quirks make a single review unmanageably large.
- If a split becomes necessary, split by provider family and preserve `blocks` relations so shared dispatch and fixture expectations land before documentation work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Implement provider-specific live schema readers using the agreed contract.

## Scope
- Refine and complete the work for "Implement provider catalog readers" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.
