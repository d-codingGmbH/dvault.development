Goal: Define the tracing contract for DVault save/read/maintenance operations.

Acceptance criteria:
- Specifies ActivitySource names, operation names, tags, status behavior, correlation, and sampling expectations.
- Defines redaction rules for keys, payload values, record sources, SQL text, credentials, and connection strings.
- Keeps tracing opt-in and independent from dashboards or OpenTelemetry exporter setup.