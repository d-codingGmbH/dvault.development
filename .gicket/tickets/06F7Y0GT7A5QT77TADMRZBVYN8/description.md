# Goal
Define a narrow generator contract for typed PIT and bridge helpers based on reviewed support-bundle metadata.

# Scope In
- Decide generated type names, method names, projection shapes, nullability, fingerprint behavior, diagnostics, and unsupported-shape reporting.
- Reuse existing runtime read services; helpers must not generate provider-specific SQL or perform maintenance.

# Scope Out
No raw model parsing beyond reviewed support-bundle explain metadata, no automatic dynamic request compilation, and no custom LINQ provider.

# Acceptance Criteria
- Supported and unsupported PIT/bridge shapes are explicit.
- Public API and generated output compatibility expectations are clear before implementation.