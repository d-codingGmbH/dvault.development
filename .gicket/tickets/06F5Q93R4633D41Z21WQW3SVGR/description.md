# Goal
Coordinate v0.23.0 tracing and performance guidance as a small release whose child tickets are independently implementable by the bot.

# Release Intent
Give DVault adopters opt-in Activity tracing for explicit save/read/maintenance operations and practical benchmark-backed performance guidance, while preserving the existing DVault boundaries: explicit service calls, consumer-owned observability pipelines, redacted diagnostics, and evidence-bound performance claims.

# Child Tickets
- `06F5Q93YXHSKABD2SABWY85S78` - define the Activity tracing contract and redaction rules. This is the first implementation blocker for the release.
- `06F5Q9463M0RSHAJJX0F3D1DB0` - implement Activity tracing for save/read service operations after the contract is accepted.
- `06F5Q94D0JDMMWDXSRGWX1E4F0` - implement Activity tracing for PIT and bridge maintenance after the contract is accepted.
- `06F5Q94KX65TXQ8EC75FWSD01W` - add benchmark-backed performance profile guidance after the contract is accepted, without waiting for code implementation.
- `06F5Q94SQ086B2DZ1AKFDXGV94` - update coordinated v0.23.0 public docs after the implementation and guidance tickets are complete.

# Scope In
- One Activity tracing contract with exact source/span/tag/event/status/redaction semantics.
- Listener-driven save/read Activity spans that complement existing metrics and telemetry summaries.
- Listener-driven PIT/bridge maintenance Activity spans that preserve caller-owned orchestration.
- Benchmark-backed adopter guidance for four practical performance profiles.
- Coordinated README, production checklist, benchmark-doc, and release-note updates.

# Scope Out
- No dashboard, OpenTelemetry exporter, collector, alerting, hosted monitoring, scheduler, background worker, database/container provisioning, credential management, or package publication automation.
- No raw business data, hash keys, payload values, record sources, SQL text, query plans, connection strings, provider messages, exception messages, or stack traces in tracing.
- No provider strategy redesign, benchmark harness redesign, public persistence API rewrite, or change to default `AddDVault()` behavior.

# Release-Level Acceptance Criteria
- The contract ticket is completed before save/read and maintenance implementation tickets start their implementation work.
- Save/read and maintenance tracing use the same ActivitySource name, span names, tag keys, event names, status behavior, and redaction rules.
- Performance guidance cites repository benchmark evidence and keeps timing claims attached to artifact and run context.
- Final documentation presents one coherent v0.23.0 story and lists explicit non-goals.
- Existing telemetry, metrics, diagnostics, explicit save/read service boundaries, and PIT/bridge maintenance boundaries remain compatible.

# Bot Handoff Notes
- This epic should not make product-code or documentation edits directly. Work is done in the child tickets.
- If a child ticket appears blocked, inspect the dependency graph before selecting another ticket. The intended first child is the tracing contract ticket.
- Do not close this epic until all child tickets are done and the release documentation task has landed.