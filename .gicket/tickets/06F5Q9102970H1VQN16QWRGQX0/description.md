Goal: Add PIT support for explicitly modeled multi-active satellite state.

Acceptance criteria:
- Defines deterministic PIT row semantics for driving-key tuples and parent hash keys.
- Supports rebuild and targeted parent maintenance without collapsing distinct driving-key states.
- Extends diagnostics for unsupported or ambiguous multi-active PIT shapes.