# Goal
Define the v2 read-plan explanation contract before implementation.

# Scope In
- Specify bounded output for latest/current/as-of satellite, PIT, and bridge reads.
- Include provider strategy, fallback cause, translated shape facts, expected key/index access paths, and omission rules.
- Preserve redaction for raw hash keys, request keys, timestamps, SQL text, query plans, credentials, and provider errors.

# Acceptance Criteria
- Contract reuses existing diagnostics vocabularies where possible.
- It states that read-plan explainability is diagnostic output, not a new query planner or LINQ provider.