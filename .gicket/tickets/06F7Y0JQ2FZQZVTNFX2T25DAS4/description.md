# Goal
Define the bounded contract for provider tuning diagnostics before implementation.

# Scope In
- Define eligibility, threshold, selected strategy, fallback, benchmark profile, and recommendation vocabulary for save/read provider paths.
- Reuse existing telemetry and performance-profile concepts where possible.
- Define redaction and omission rules.

# Acceptance Criteria
- Contract avoids raw SQL, query plans, credentials, provider exception messages, and workload data values.
- Contract distinguishes diagnostics from automatic optimization or deployment behavior.