Goal: Make streaming/chunked save strategy decisions actionable.

Acceptance criteria:
- Extends diagnostics with reasons a streaming save used fallback or rejected a provider-native path.
- Provides bounded remediation hints for chunk sizing, dirty context, provider mismatch, unsupported shapes, or transaction constraints.
- Updates tests and public API snapshots where applicable.