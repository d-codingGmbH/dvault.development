Goal: Implement a PostgreSQL staged bulk path for eligible DVault save batches.

Acceptance criteria:
- Uses PostgreSQL-appropriate staging and COPY or equivalent native transfer.
- Preserves idempotency, hash-diff latest-state checks, transactions, cancellation, and cleanup.
- Adds PostgreSQL-gated integration tests and benchmark rows when configured.