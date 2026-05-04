Goal: implement the SQL Server optimized save strategy behind the shared provider contract.

Acceptance Criteria:
- The implementation avoids per-row existence probes for large batches where set-based SQL is available.
- Insert-only satellite history semantics remain identical to the fallback implementation.
- The code remains isolated in the SQL Server provider package.