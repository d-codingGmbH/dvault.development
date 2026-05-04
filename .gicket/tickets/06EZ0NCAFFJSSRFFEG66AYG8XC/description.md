Goal: make provider optimization results comparable across fallback, SQLite, and external providers.

Scope:
- Extend benchmark output so provider, strategy, dataset size, change ratio, and fallback/classic comparison are visible.
- Keep local benchmark execution useful even when external providers are unavailable.
- Record skipped provider benchmarks explicitly instead of silently omitting them.

Acceptance Criteria:
- Benchmark artifacts can compare fallback and optimized strategies for at least SQLite and any configured external providers.
- The report format is stable enough to support release-quality evidence.
- Documentation explains how to run provider-specific benchmarks and interpret skipped providers.