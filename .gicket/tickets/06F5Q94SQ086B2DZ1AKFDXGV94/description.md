# Goal
Update the coordinated v0.23.0 documentation after the tracing and performance-profile work has landed.

# Background
This is the release documentation consolidation task. It should not invent tracing semantics or performance claims. It should pull the final contract and implementation evidence from the completed child tickets, then make the public docs consistent.

# Dependencies
This task should be handled after these tickets are complete:
- `06F5Q93YXHSKABD2SABWY85S78` - tracing contract and redaction rules.
- `06F5Q9463M0RSHAJJX0F3D1DB0` - save/read Activity tracing implementation.
- `06F5Q94D0JDMMWDXSRGWX1E4F0` - PIT/bridge maintenance Activity tracing implementation.
- `06F5Q94KX65TXQ8EC75FWSD01W` - benchmark-backed performance profile guidance.

# Scope In
- Update `README.md` current-baseline sections for v0.23.0.
- Update `docs/production-adoption-checklist.md` with Activity tracing opt-in, redaction, diagnostics, and performance-profile guidance.
- Create or update `docs/releases/v0.23.0.md` as the coordinated release record if it does not already exist.
- Update benchmark documentation links if performance-profile guidance added or moved docs.
- Cross-link the Activity tracing contract document and performance evidence contract from public docs.
- Record validation commands and evidence actually run by the completed child tickets.

# Scope Out
- No product-code changes.
- No new tracing contract decisions.
- No benchmark result changes unless a child ticket explicitly regenerated artifacts.
- No dashboards, hosted monitoring, collector setup, alert rules, container/database provisioning, scheduler templates, deployment automation, or package publication steps.
- No claim that a package was published or that optional providers were measured if the evidence does not show it.

# Documentation Requirements
- Explain that `AddDVault()` remains telemetry-free by default.
- Explain how Activity tracing relates to `AddDVaultTelemetry()`, Metrics, and `IDataVaultTelemetryObserver`.
- Document the redaction boundary in adopter-facing language: no raw keys, payloads, record sources, SQL, credentials, connection strings, provider messages, exception messages, or stack traces.
- State that exporter, collector, dashboard, and alerting ownership stays with the consuming application.
- Summarize the four performance profiles and link to the detailed guidance.
- Keep benchmark timing claims attached to artifact files and run context.
- Preserve existing compatibility notes for explicit save/read services, PIT/bridge maintenance, provider registration, and package family boundaries.

# Acceptance Criteria
- README, production checklist, release notes, and benchmark/performance docs tell one consistent v0.23.0 story.
- Public docs link to the Activity tracing contract and benchmark-backed performance guidance.
- Release notes list the coordinated package family, intended release posture, compatibility notes, validation evidence, and explicit non-goals.
- Documentation uses the exact ActivitySource/span/redaction language from the contract ticket where names matter.
- Documentation does not add unsupported platform responsibilities or unverified performance claims.
- Links and anchors touched by the change are valid.

# Verification
- Run available docs/markdown/link validation if present.
- Run any lightweight repository validation normally used for documentation-only release tasks.
- Inspect `README.md`, `docs/production-adoption-checklist.md`, `docs/releases/v0.23.0.md`, and benchmark docs for consistent wording before closing.