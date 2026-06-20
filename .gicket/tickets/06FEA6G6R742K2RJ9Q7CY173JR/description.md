<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined for PO critic: DB2 is already recognized as `IBM.EntityFrameworkCore`, the branch still has no implementation beyond the explicit unsupported-reader dispatch, opt-in DB2 smoke/config scaffolding already exists, and active docs still describe DB2 live-schema as unsupported.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- `IBM.EntityFrameworkCore` is already the repository's DB2 provider identity; the missing piece is the live-schema reader dispatch, which still points DB2 at `UnsupportedDataVaultLiveSchemaReader` in `DataVaultLiveSchemaReader`.
- The branch head still matches scratch-source ref `d246f7d84511c1f66ea7185f9c30f9896cdc6f71`, so no DB2 live-schema implementation has landed on this ticket branch yet.
- Existing DB2 opt-in test scaffolding is already present through `Db2IntegrationTestConfiguration`, `Db2ProviderReflection`, and `Db2DataVaultSmokeTests`, so this ticket can extend the current external-provider test lane instead of inventing a new environment contract.
- Related ticket `06FE4QR3DD7EFZ4F35SBTFGWSR` remains a `relates` link for DB2 save/read evidence tuning; no relation cleanup or child-ticket split was needed for this ticket.
- No child tickets, relation edits, description updates, attachments, or planning documents were materialized during this refinement run.

### Scope In
- Add a built-in DB2 live-schema reader path for provider name `IBM.EntityFrameworkCore` within the existing live-schema dispatch and catalog-reader architecture.
- Read the bounded DVault-owned schema facts already used by idempotency preflight: ordered columns, primary-key names/columns, and secondary-index metadata for hubs, links, satellites, PITs, and bridges when those tables exist.
- Return classified DB2 outcomes for success and unavailable catalog access using caller-owned connections only, with deterministic and redacted result messages suitable for `DataVaultPreflightRequest.IdempotencyLiveSchemaReadResult`.
- Add unit and opt-in external-provider coverage that proves DB2 snapshot success when configured and explicit non-success outcomes when configuration, connectivity, or catalog access is not safe.
- Update current public and active planning/adoption docs that currently state DB2 live-schema is unsupported so they describe DB2 as external opt-in evidence instead.

### Scope Out
- DB2 save-strategy tuning, latest-satellite/PIT/bridge read-strategy tuning, benchmark timing promotion, staged bulk execution, or provider-native chunk execution.
- Automatic migrations, automatic schema repair, DB2 DDL generation, or any default live-database CI gate.
- Changing the supported live-schema fact surface beyond the existing idempotency-preflight structures DVault already compares.
- Rewriting historical release notes that are documenting earlier shipped baselines rather than current guidance.

## Acceptance Criteria
- A DB2 `DbContext` using provider name `IBM.EntityFrameworkCore` no longer returns the explicit unsupported-provider boundary from `DataVaultLiveSchemaReader.ReadAsync`; when configured, it returns a structured live-schema snapshot.
- The DB2 snapshot is deterministic and limited to DVault table, column, primary-key, and secondary-index facts needed by idempotency preflight, aligned with the repository's existing DB2 physical-name and index-shape rules.
- Unavailable DB2 cases such as missing configuration, unreachable catalog access, or insufficient privileges return explicit classified outcomes and do not leak connection strings, credentials, host names, raw SQL, raw data, or provider exception text.
- Existing SQLite, PostgreSQL, SQL Server, Oracle, and MySQL live-schema behavior and unsupported-provider handling for truly unsupported providers remain unchanged.
- Current active guidance no longer states that DB2 live-schema reading is unsupported; it states that DB2 live-schema checks are external, opt-in, and consumer-owned like the other non-SQLite live-schema lanes.

## Definition of Done
- Unit coverage is updated so the old DB2-specific explicit-unsupported boundary assertion is removed or replaced with the new DB2 dispatch contract.
- Opt-in DB2 integration coverage exercises the live-schema reader against the canonical shared live-schema fixture under `DVAULT_TEST_DB2_CONNECTION_STRING` gating.
- Documentation is consistent across the current README, adoption, model-first, and current-baseline surfaces that presently advertise DB2 live-schema as unsupported.
- No new public API surface or workflow dependency is introduced beyond the bounded live-schema reader implementation and supporting tests/docs.

## Implementation Notes
- Keep the implementation in the provider-neutral live-schema core path beside the existing `CatalogDataVaultLiveSchemaReader` implementations; no new DI registration surface is required.
- Reuse the repository's shared live-schema fixtures and model options for DB2 coverage rather than inventing a separate schema shape, because the DB2 opt-in test environment contract already exists.
- Use `DataVaultProviderCapabilityProfiles.Db2` as the expected physical-shape baseline so DB2 identifier projection and included-index fallback rules stay aligned with current diagnostics and smoke coverage.
- DB2 unavailable results should use deterministic redacted messaging rather than raw provider exception text so the preflight evidence lane stays within the ticket's redaction boundary.
- No durable planning writes were applied in this refinement run; the existing `relates` link to `06FE4QR3DD7EFZ4F35SBTFGWSR` was verified and left unchanged.

## Open Questions
- none

## Follow-Up Questions
- After this lands, should the next release-note baseline explicitly call out DB2 live-schema support as newly available external opt-in evidence so the evidence matrix and adoption docs stay synchronized?
- If DB2 requires stricter message redaction than the other existing readers, do we want a later consistency ticket to normalize unavailable-message redaction across every live-schema reader?

## Risks
- Default repository validation does not provision DB2, so the live-reader success path will remain proven only through the opt-in external-provider lane behind `DVAULT_TEST_DB2_CONNECTION_STRING`.
- DB2 unsupported wording currently appears in several active documentation surfaces; partial doc updates would leave contradictory adoption guidance behind.
- The existing generic live-schema unavailable path accepts provider-specific messages, so the DB2 implementation must deliberately avoid echoing raw provider error text or host details.

## Split Recommendations
- No split recommended from current evidence; the work stays bounded to one DB2 reader implementation, matching test coverage, and current-guidance updates.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Purpose
Add DB2 live-schema reader support for the opt-in DVault preflight/idempotency evidence lane. Today `IBM.EntityFrameworkCore` is recognized as DB2, but live-schema reads return `UnsupportedProvider`, so DB2 adoption cannot validate idempotency-critical structures from a live DB2 catalog the same way PostgreSQL, SQL Server, Oracle, MySQL, and SQLite can.

## Scope In
- Implement a bounded DB2 live-schema reader for IBM.EntityFrameworkCore using caller-owned DB2 connections only.
- Read the catalog facts DVault already compares for idempotency preflight: hub/link primary keys, business-key indexes, satellite latest-state indexes, PIT read indexes, and bridge traversal indexes.
- Keep output deterministic and redacted: table/column/index/key facts are allowed; connection strings, credentials, provider exception text, host names, schema repair SQL, and raw data are not.
- Add tests or smoke coverage that prove DB2 returns structured live-schema facts when configured and still reports explicit unavailable/unsupported outcomes when it is not safe to read.
- Update design-time/adoption documentation so DB2 moves from unsupported live-schema status to external opt-in evidence with consumer-owned database lifecycle.

## Scope Out
- Automatic migrations, automatic schema repair, or DB2 DDL generation.
- Changing DB2 save/read strategy selection, PIT/bridge maintenance, or benchmark timing claims.
- Making live DB2 checks a default CI gate; they remain opt-in and environment-owned.

## Acceptance Criteria
- `DataVaultLiveSchemaReader` dispatches `IBM.EntityFrameworkCore` to a DB2 reader instead of `UnsupportedDataVaultLiveSchemaReader` when DB2 support is available.
- A configured DB2 live-schema read returns deterministic primary-key/index facts needed by `DataVaultPreflightRequest.IdempotencyLiveSchemaReadResult` without leaking credentials, provider exception text, server names, or raw data.
- Missing DB2 configuration, unavailable DB2 connectivity, or insufficient catalog privileges produce explicit bounded outcomes and do not crash the preflight pipeline.
- Existing non-DB2 live-schema reader behavior remains unchanged.
- Documentation and checklist wording no longer claim DB2 live-schema reading is unsupported once the implementation lands; it must still describe DB2 as external opt-in evidence, not default automation.

## Notes
This is a follow-up to the v0.42 DB2 performance evidence work. It is intentionally separate from DB2 benchmark tuning: benchmark evidence can stay completed while live-schema reading becomes a later adoption/preflight capability.