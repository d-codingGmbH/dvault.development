# Goal
Implement v2 read-plan explanations across DVault read diagnostics and support-bundle output.

# Scope In
- Extend IDataVaultReadDiagnosticsService for latest/current/as-of satellite, PIT, and bridge requests.
- Include registry-backed variants where metadata resolves deterministically.
- Add deterministic support-bundle serialization plus redaction and snapshot tests.

# Acceptance Criteria
- Diagnostics explain selected strategy, fallback, shape, and expected access path without raw SQL or request values.
- Existing read service behavior remains unchanged.