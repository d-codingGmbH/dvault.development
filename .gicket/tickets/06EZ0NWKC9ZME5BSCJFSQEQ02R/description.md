Goal: expose advanced hooks needed by deferred capabilities while preserving deterministic defaults.

Scope:
- Add or refine hooks for timestamp, record source, provider behavior, and validation policies used by PIT, bridge, and multi-active scenarios.
- Keep default behavior unchanged for existing v0.4 usage.
- Document hook failure modes and validation behavior.

Acceptance Criteria:
- Hooks are opt-in and covered by tests.
- Default behavior remains deterministic and compatible with existing examples.
- Hook contracts do not couple core code to one database provider.