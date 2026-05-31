# Goal
Reduce boilerplate for caller-owned async domain sources while keeping DVault saves explicit.

# Scope In
- Add helper/adaptor APIs for IAsyncEnumerable<TSource> where callers supply explicit mapping to DataVaultSaveRequest values or existing typed mapper contracts.
- Keep chunk sizing, load timestamp, record source, and business-key mapping caller-visible.

# Scope Out
No CSV/JSON ingestion, schema inference, entity tracking magic, or generic ETL subsystem.

# Acceptance Criteria
- Helpers compose with the async chunked save entry point.
- Tests cover ordering, chunk sizing, mapper failures, cancellation, and generated typed mapper compatibility where applicable.