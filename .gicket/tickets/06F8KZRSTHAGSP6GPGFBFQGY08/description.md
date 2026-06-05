Add adopter-facing performance decision-tree documentation based on the v0.31.0 contract.

Required repository output
- Update `docs/performance-profiles.md` with practical examples, fallback examples, and rerun/stop-condition guidance that build on the contract story.
- Add short checklist wording to `docs/production-adoption-checklist.md` only if it helps adopters find the final decision tree without duplicating the full contract.
- This ticket must produce documentation changes outside `.gicket`.

Scope in
- Show concrete save-path examples for small materialized batches, bounded chunked ingestion, already-asynchronous chunk sources, and diagnostics-gated staged provider ingestion.
- Show concrete read-path examples for latest satellite, PIT as-of, and bridge traversal reads, including maintenance freshness and incomplete read-shape evidence fallback.
- Include explicit "when not to optimize" guidance and when to re-run local benchmarks or inspect diagnostics again.
- Preserve SQLite as the only repository-proven optimized latest-satellite provider path, and keep non-SQLite PIT/bridge claims behind diagnostics and available evidence.

Scope out
- New APIs, new benchmarks, generated SQL artifacts, automatic PIT/bridge maintenance, dashboards, exporters, or implementation changes.
- Repeating the full v0.31 release summary; release documentation is handled by the final release-docs task.