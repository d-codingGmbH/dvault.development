# Goal
Improve EF migration guardrails for generated DVault structures.

# Scope In
- Detect destructive or suspicious changes to generated hub, link, satellite, PIT, bridge, index, and constraint structures.
- Emit actionable diagnostics that distinguish intentional evolution from accidental metadata/naming drift.

# Scope Out
No automatic migration rewrite, schema repair, or deployment.

# Acceptance Criteria
- Tests cover generated table/column/index/constraint drop/rename cases and safe additive cases.
- Diagnostics reference DVault metadata concepts and remediation steps.