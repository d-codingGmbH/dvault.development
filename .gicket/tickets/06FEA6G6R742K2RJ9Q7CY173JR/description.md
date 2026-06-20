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