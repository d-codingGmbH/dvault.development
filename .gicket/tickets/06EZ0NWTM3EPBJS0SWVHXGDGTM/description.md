Goal: implement opt-in timestamp and record-source hooks for advanced capability scenarios.

Acceptance Criteria:
- Defaults match existing behavior exactly.
- Hooks can be configured per model or save operation where appropriate.
- Tests cover custom values, null/invalid values, and deterministic fallback.