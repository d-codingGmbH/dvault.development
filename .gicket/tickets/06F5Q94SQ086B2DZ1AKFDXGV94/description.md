<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repo evidence already contains the v0.23.0 performance guide, the activity tracing contract, and the supporting test and benchmark surfaces; this ticket is now a bounded documentation-consolidation pass to align README.md, docs/production-adoption-checklist.md, and a missing docs/releases/v0.23.0.md release record.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already shows the v0.23.0 performance guide, the activity tracing contract, and the tracing and benchmark evidence surfaces are landed; this ticket should be treated as docs consolidation, not as a wait-state for fresh product-code work.
- Treat docs/architecture/dvault-v1-activity-tracing-contract.md as the authoritative source for exact ActivitySource, span-name, event-name, and redaction wording; public docs should summarize and cross-link it, not redefine it.
- Treat docs/performance-profiles.md as the already-landed detailed v0.23.0 performance guide; this ticket should align public docs to that guide and the checked-in benchmark artifacts rather than invent new performance prose or tables.
- Treat v0.23.0 as the new coordinated public baseline and keep v0.22.0, v0.21.0, and v0.20.0 as historical references for earlier feature introductions.
- Benchmark-documentation changes are conditional: benchmark docs already link to docs/performance-profiles.md, so only stale links or wording need updates.
- Use the README local validation command set as the repository baseline; no dedicated markdown or link checker is visible in the repo, so touched link and anchor verification is a manual docs-review step alongside the existing gates.

### Scope In
- Update README.md current-baseline sections and version-aligned public examples so v0.23.0 becomes the current coordinated documentation baseline.
- Update docs/production-adoption-checklist.md so the current release baseline, activity tracing posture, redaction boundary, telemetry relationship, and performance-profile guidance match the v0.23.0 story.
- Create docs/releases/v0.23.0.md as the coordinated seven-package release record with package scope, compatibility posture, validation evidence, benchmark evidence references, and explicit non-goals.
- Cross-link public docs to the activity tracing contract, the performance profile guide, the benchmark artifact contract, and the root benchmark summary triplet.
- Record the repository validation commands and the focused tracing and performance evidence sources already present in the completed child-ticket outputs.

### Scope Out
- No product-code, benchmark harness, or public API changes.
- No new tracing contract, span vocabulary, redaction decision, or telemetry semantics.
- No new benchmark runs, artifact rewrites, or changed timing claims unless a completed child ticket already landed new evidence.
- No claim that packages were published, optional external providers were measured, or non-SQLite provider wins were validated when the checked-in artifacts show skipped rows.
- No exporter, collector, dashboard, alerting, scheduler, container, database, deployment, or release-automation setup guidance.
- No reopening of carried-forward compatibility boundaries for explicit save and read services, PIT and bridge maintenance, provider registration, package-family membership, or manual publication separation.

## Acceptance Criteria
- README.md, docs/production-adoption-checklist.md, docs/performance-profiles.md, and docs/releases/v0.23.0.md tell one consistent v0.23.0 story, with v0.23.0 as the current coordinated baseline and older releases clearly historical.
- Public docs explain that AddDVault() remains telemetry-free by default, that Activity tracing is listener-driven via the DCoding.Data.DVault ActivitySource, and that AddDVaultTelemetry(), Metrics, and IDataVaultTelemetryObserver remain sibling opt-in observability surfaces rather than prerequisites for tracing.
- Public adopter-facing docs state the tracing redaction boundary without ambiguity: no raw business keys or hash keys, payload values, record sources, SQL text, credentials, connection strings, provider messages, exception messages, or stack traces.
- docs/releases/v0.23.0.md lists the coordinated seven-package family, intended release posture, carried-forward compatibility notes, validation evidence, benchmark evidence references, and explicit non-goals without implying package publication.
- Performance guidance summarizes the four existing profiles by linking to docs/performance-profiles.md and ties any timing claims to the checked-in benchmark artifacts and run context instead of inventing new unverified claims.
- Every touched link or anchor resolves, and the documented validation section records the repository baseline commands plus the focused tracing and benchmark evidence surfaces cited by the release note.

## Definition of Done
- README.md and docs/production-adoption-checklist.md no longer point readers at v0.22.0 as the current coordinated release baseline where v0.23.0 should now be authoritative.
- docs/releases/v0.23.0.md exists and can stand alone as the coordinated release record for tracing and performance guidance consolidation.
- The touched public docs consistently preserve the exact tracing contract language where names matter and do not introduce unsupported tracing, telemetry, or performance claims.
- The touched docs preserve historical references to earlier releases only as carried-forward background, not as the current baseline.
- Repository validation commands and evidence references are documented, and manual review confirms the touched anchors and cross-links within the affected docs.

## Implementation Notes
- Use docs/architecture/dvault-v1-activity-tracing-contract.md for the exact ActivitySource name DCoding.Data.DVault, the listener-driven opt-in posture, the closed span vocabulary, and the explicit redaction prohibitions.
- Keep the public tracing explanation high level in README.md and docs/production-adoption-checklist.md: explain what adopters wire up, what stays application-owned, and where to go for the exact contract details.
- Reuse docs/performance-profiles.md as the detailed performance reference; README.md, the checklist, and the release note should summarize the four profiles and link back instead of duplicating its tables.
- Tie performance prose to the checked-in root benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json triplet and its current run context: 3 iterations, 1 warmup iteration, ProviderDefault load timestamps, provider filter all, Debian GNU/Linux 13 x64, .NET 10.0.8, required SQLite local temporary files, and optional PostgreSQL, SQL Server, MySQL, and Oracle rows skipped because their connection-string environment variables were unset.
- Follow the existing v0.22.0 release-note structure and carry forward compatibility statements for explicit save and read services, chunked save positioning, PIT and bridge maintenance ownership, SQLite-only proven optimized PIT and bridge read paths, provider-package boundaries, and manual package publication.
- Use the README local validation baseline commands: dotnet build DVault.slnx --nologo, dotnet test DVault.slnx --nologo, and bash tools/check-format.sh.
- Cite focused evidence surfaces already present in the repository for the coordinated story: DataVaultActivityTracingTests.cs, DataVaultPitMaintenanceServiceSqliteTests.cs, DataVaultBridgeMaintenanceServiceSqliteTests.cs, BenchmarkScenarioExecutionTests.cs, and the shared performance artifact contract.

## Open Questions
- none

## Follow-Up Questions
- Should a later docs-quality ticket add a dedicated markdown or link validation step, since the current repository-visible automation exposes formatting, build, and test gates but no standalone link checker?

## Risks
- README.md and docs/production-adoption-checklist.md still reference v0.22.0 as the current baseline, so partial edits could leave public-versioning guidance inconsistent if the update is not completed across all touched docs.
- Activity tracing docs must reuse the closed contract vocabulary exactly where names matter; paraphrased span names, redaction lists, or telemetry relationships would create public inconsistencies with the landed contract.
- Performance claims are evidence-bound to the root benchmark artifacts and current run context; optional external-provider rows are skipped, so broad provider-specific performance wording would overstate the checked-in evidence.
- Because no dedicated link checker is visible, broken anchors or stale cross-links remain a manual-review risk unless each touched link is checked during the docs pass.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

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