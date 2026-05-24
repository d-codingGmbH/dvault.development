Goal: Implement opt-in Activity tracing for explicit PIT and bridge maintenance services.

Acceptance criteria:
- Emits activities for rebuild and targeted maintenance operations when enabled.
- Tags maintenance kind, provider family, row-count summaries, duration classification, and fallback causes without leaking values.
- Keeps scheduling and orchestration outside DVault.