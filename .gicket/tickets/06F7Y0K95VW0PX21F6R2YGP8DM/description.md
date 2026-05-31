# Goal
Add a deterministic verifier for benchmark artifacts so performance guidance stays evidence-backed.

# Scope In
- Validate expected scenario names, provider profiles, artifact schema, measured dimensions, and regression budget metadata.
- Fail fast on missing or stale artifacts used by docs or diagnostics.

# Scope Out
No hosted dashboard or external performance service.

# Acceptance Criteria
- Verifier can run in the existing quality/test workflow or as a focused test.
- Performance profile docs cite artifacts that pass verification.