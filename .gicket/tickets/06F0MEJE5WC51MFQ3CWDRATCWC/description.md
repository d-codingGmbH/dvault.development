## Goal

Implement the first provider-specific read optimization selected from measured benchmark evidence.

## Scope In

- Choose the provider/read shape with the largest measured improvement potential.
- Implement optimized SQL/query path through the read strategy hook.
- Add correctness tests and before/after benchmark evidence.

## Scope Out

- Optimizing every provider.
- Changing write strategy behavior.

## Acceptance Criteria

- Optimization choice is justified by benchmark data in comments or docs.
- Fallback remains correct for unsupported shapes.
- The implementation does not regress write benchmarks or public API compatibility.