Goal: generate provider-neutral EF Core mapping for baseline bridge tables.

Acceptance Criteria:
- Generated mapping covers table name, key columns, traversal references, and effective/load timestamp fields where applicable.
- Tests verify the mapping through the local SQLite baseline.
- Unsupported bridge shapes fail with clear validation messages.