Goal: generate provider-neutral EF Core mapping for baseline PIT tables.

Acceptance Criteria:
- Generated mapping covers table name, key columns, hub reference, satellite snapshot references, and load timestamp fields.
- Tests verify the mapping against SQLite as the local baseline without embedding SQLite-specific SQL in core logic.
- Unsupported PIT shapes fail with clear validation errors.