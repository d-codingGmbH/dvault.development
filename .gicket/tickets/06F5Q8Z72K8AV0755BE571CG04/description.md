Goal: Implement a SQL Server staged bulk path for eligible DVault save batches.

Acceptance criteria:
- Uses SQL Server-appropriate staging and SqlBulkCopy or equivalent native transfer.
- Preserves idempotency, hash-diff latest-state checks, transactions, cancellation, and cleanup.
- Adds SQL Server-gated integration tests and benchmark rows when configured.