# Customer Profile Comparison Contract

Status: v1 shared comparison contract
Tickets: 06EXB7RYFJ3YQDB1E4QHPP8034, 06EXB7S6DB97GVVTS2GGZ3CCX8

## Purpose

This artifact fixes one shared customer profile history sequence and the exact persisted-outcome assertions that the plain EF and DVault comparison tickets must use. It removes scenario drift between the two tickets.

## Shared Business Scenario

Use one customer business key `C-100` and profile attributes `customer_name` and `customer_status`.

### Event 1: initial state

- load timestamp: `2026-04-29T10:15:00Z`
- record source: `crm-import`
- customer business key: `C-100`
- customer_name: `Alice Adams`
- customer_status: `prospect`

### Event 2: changed state

- load timestamp: `2026-04-29T11:30:00Z`
- record source: `crm-change`
- customer business key: `C-100`
- customer_name: `Alice Baker`
- customer_status: `active`

The comparison baseline is an exact persisted outcome contract, not only a generic history narrative. Each ticket must assert the complete stored result for its own persistence shape after both events have been applied.

## Ticket 06EXB7RYFJ3YQDB1E4QHPP8034: plain EF baseline contract

The plain EF baseline uses ordinary EF Core entities and SQLite persistence. Table names and CLR type names may follow normal EF conventions, but the asserted stored history for the customer profile scenario must contain exactly these two persisted history rows, ordered by the persisted history timestamp column ascending:

| Row | Customer business key | CustomerName | CustomerStatus | ChangedAtUtc | RecordSource |
| --- | --- | --- | --- | --- | --- |
| 1 | `C-100` | `Alice Adams` | `prospect` | `2026-04-29T10:15:00Z` | `crm-import` |
| 2 | `C-100` | `Alice Baker` | `active` | `2026-04-29T11:30:00Z` | `crm-change` |

Expected plain EF outcome after both events:

- exactly 2 persisted customer profile history rows for business key `C-100`
- row 1 matches the initial state above
- row 2 matches the changed state above
- no extra unchanged replay row is inserted for this v1 baseline scenario

## Ticket 06EXB7S6DB97GVVTS2GGZ3CCX8: DVault comparison contract

The DVault ticket must use the same two business events above. Its persisted outcome contract is implementation-specific but exact:

- exactly 1 customer hub row for business key `C-100`
- exactly 2 customer profile satellite rows for that hub, ordered by load timestamp ascending
- satellite row 1 stores `customer_name = Alice Adams`, `customer_status = prospect`, `load_timestamp = 2026-04-29T10:15:00Z`, `record_source = crm-import`
- satellite row 2 stores `customer_name = Alice Baker`, `customer_status = active`, `load_timestamp = 2026-04-29T11:30:00Z`, `record_source = crm-change`
- the second event must create a new satellite history row rather than overwrite the first state
- no extra customer hub row is inserted for the second event

## Comparison Intent

The two tickets do not need identical physical schemas. They do need the same input history sequence and exact persisted outcome assertions for their own storage shapes so later comparison can evaluate conventional EF history rows against DVault hub-plus-satellite history rows without scenario drift.
