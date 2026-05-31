# Goal
Expose bounded diagnostics that explain provider strategy eligibility and tuning recommendations for supported save/read paths.

# Scope In
- Add provider-specific eligibility reasons and threshold guidance where benchmark evidence exists.
- Explain why optimized strategies are selected, declined, or falling back.

# Scope Out
No automatic batch-size tuning, stored-procedure switch, query-plan parsing, or provider deployment action.

# Acceptance Criteria
- Tests cover selected, declined, fallback, and unsupported provider cases.
- Output links naturally to performance profiles and remains redacted.