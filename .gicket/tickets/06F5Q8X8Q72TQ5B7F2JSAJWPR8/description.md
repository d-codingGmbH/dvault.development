Goal: Implement the provider-neutral execution path for streaming/chunked save requests.

Acceptance criteria:
- Processes bounded chunks without materializing the entire logical load.
- Preserves hub/link idempotency, satellite hash-diff behavior, ordering, cancellation, and transactions.
- Adds unit and integration coverage for chunk boundaries, duplicates, satellite replay, and cancellation.