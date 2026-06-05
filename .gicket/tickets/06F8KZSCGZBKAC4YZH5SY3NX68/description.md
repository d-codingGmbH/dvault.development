Add bounded observability examples for DVault Activity tracing and Metrics without adding application-platform responsibilities.

Required repository output
- Update `README.md` and/or `examples/README.md` with compact examples that show how an application opts into DVault Activity tracing and built-in Metrics.
- Link to `docs/architecture/dvault-v1-activity-tracing-contract.md` for the authoritative ActivitySource, span/event/tag, sampling, and redaction rules.
- This ticket must produce documentation or example changes outside `.gicket`.

Scope in
- Show listener-driven ActivitySource usage for `DCoding.Data.DVault` and make clear that `AddDVault()` remains telemetry-free by default.
- Show `AddDVaultTelemetry()` as the built-in `System.Diagnostics.Metrics` observer path, separate from Activity tracing.
- Include pseudo-code or minimal illustrative wiring for OpenTelemetry-style tracing/metrics integration only if it stays application-owned and does not add package references or runtime dependencies to DVault.
- Point adopters at redaction and omission rules; examples must not include raw keys, payload values, SQL text, connection strings, provider messages, exception text, stack traces, or support-bundle content.

Scope out
- Adding OpenTelemetry exporter packages, AppInsights or Jaeger dependencies, collectors, dashboards, alerts, hosting templates, sampling policy defaults, custom correlation storage, or deployment instructions.
- Changing DVault tracing or metrics runtime behavior.