Goal: Evaluate and implement an Oracle staged bulk path only for shapes where evidence beats the existing gated strategy.

Acceptance criteria:
- Records the decision boundary between array binding, staging, and provider-neutral fallback.
- Implements staging only when cleanup, transaction, and batch-size behavior are reliable under Oracle limits.
- Adds Oracle-gated integration tests and benchmark evidence when configured.