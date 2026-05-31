# Goal
Extend live/schema preflight checks to validate provider-specific idempotency-critical constraints and indexes.

# Scope In
- Verify required uniqueness and access-path structures for hub, link, satellite latest-state, PIT, and bridge operations where metadata supports it.
- Explain provider caveats and fallback remediation without raw database data.

# Scope Out
No automatic schema repair, migration application, or destructive database operation.

# Acceptance Criteria
- Tests cover missing, mismatched, and valid idempotency/index structures.
- Output remains redacted and provider-aware.