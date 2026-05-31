# Goal
Record the DVault boundary for optional provider-specific stored-procedure or SQL artifact ideas without making them a default feature.

# Scope In
- Document that such artifacts require explicit opt-in, design-time generation only, no deployment ownership, no default runtime path, no automatic migration synchronization, and benchmark evidence first.
- Compare this boundary with existing staged provider bulk strategies.

# Scope Out
No stored-procedure implementation, artifact generator, runtime dispatcher, or deployment automation.

# Acceptance Criteria
- Documentation clearly says stored procedures are not DVault's default path.
- Future tickets can use this as a decision gate.