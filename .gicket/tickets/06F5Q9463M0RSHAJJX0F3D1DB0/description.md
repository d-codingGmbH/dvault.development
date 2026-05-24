Goal: Implement opt-in Activity tracing for explicit save and read service operations.

Acceptance criteria:
- Emits activities for single/bulk saves and latest/as-of/PIT/bridge reads when enabled.
- Tags strategy status, provider family, operation counts, duration classification, and fallback summaries without leaking values.
- Aligns with existing Metrics and IDataVaultTelemetryObserver behavior.