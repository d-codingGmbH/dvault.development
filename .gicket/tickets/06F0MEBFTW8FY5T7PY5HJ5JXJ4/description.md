## Goal

Let existing save/read services use registry metadata for ordinary workflows while preserving explicit request-based APIs for advanced callers.

## Scope In

- Registry-backed overloads or adapters for save and read services.
- Deterministic behavior when explicit metadata is still supplied.
- Tests that prove no behavior change for low-level APIs.

## Scope Out

- Typed object mappers.
- Provider-specific optimization changes.

## Acceptance Criteria

- Existing request-based APIs remain source-compatible.
- Registry-backed flows remove duplicate metadata creation from ordinary save/read code.
- Missing registry entries fail before partial writes occur.