Goal: Add bounded state handling and diagnostics for streaming hash-key/hash-diff continuity.

Acceptance criteria:
- Defines how per-parent satellite state is scoped, bounded, and released across chunks.
- Adds diagnostics for retained state, chunk counts, fallback causes, and unsupported memory-sensitive shapes.
- Keeps diagnostics deterministic and redacted.