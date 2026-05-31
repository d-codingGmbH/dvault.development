# Goal
Generate typed bridge read helpers for supported reviewed metadata shapes.

# Scope In
- Generate helper methods and projection models for supported many-to-many and hierarchy bridge read shapes.
- Call existing bridge read APIs internally and preserve endpoint role semantics.

# Scope Out
No bridge maintenance, graph traversal platform, recursive query generator surface, or provider-specific SQL generation.

# Acceptance Criteria
- Generated bridge helpers compile, preserve runtime read semantics, and have snapshot/integration coverage.
- Unsupported bridge shapes emit deterministic diagnostics.