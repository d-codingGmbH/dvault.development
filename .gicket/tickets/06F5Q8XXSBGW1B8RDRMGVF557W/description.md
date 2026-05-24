Goal: Add benchmark evidence for streaming/chunked saves against materialized request baselines.

Acceptance criteria:
- Records timing and allocation behavior for bounded chunk sizes.
- Keeps optional provider rows visible as completed or skipped.
- Stores before/after evidence where release claims depend on measured behavior.