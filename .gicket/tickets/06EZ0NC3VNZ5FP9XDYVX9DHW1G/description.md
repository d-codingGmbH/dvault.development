Goal: provide repeatable opt-in MySQL smoke coverage for provider-specific behavior.

Acceptance Criteria:
- The test suite is skipped by default unless MySQL configuration is present.
- The smoke path verifies provider registration and at least one insert-only save scenario.
- Documentation identifies how to enable the test locally or in CI.