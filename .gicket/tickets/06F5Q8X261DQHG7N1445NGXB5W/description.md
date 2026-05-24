Goal: Define the API and behavior contract for streaming or chunked explicit saves.

Acceptance criteria:
- Describes input shape, ordering, cancellation, transaction ownership, and compatibility with current save requests.
- Documents hash-key and hash-diff state across chunks without full source materialization.
- Adds focused tests for validation and existing API compatibility.

Non-goals:
- No automatic background ingestion.
- No scheduler or queue framework integration.
- No implicit SaveChanges interception.