[gicket-bot] PO refinement contract

Summary
- Refined against live ticket/relation state plus the completed 2026-06-07 v0.32.0 Podman scale bundle; safe scope is MySQL tiny-workload eligibility and benchmark/diagnostic clarity, while PostgreSQL seed regressions from 2026-06-06 are treated as historical unless reproduced in fresh before/after evidence.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Treat `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-scale-5-all-providers-<redacted>` as the current authoritative baseline and `artifacts/benchmarks/v0.31.0-scale-5-all-providers-<redacted>` as historical seed evidence when they conflict.
- Historical v0.31.0 seed evidence from 2026-06-06 showed PostgreSQL optimized slower than fallback at `customer-profile-scale-10x1` (34.508 ms vs 25.631 ms) and `customer-profile-scale-10x10` (31.335 ms vs 30.635 ms), but the completed v0.32.0 baseline from 2026-06-07 reverses both rows to 14.595 ms vs 28.393 ms and 22.236 ms vs 26.005 ms respectively.
- MySQL tiny rows remain the consistent live small-batch problem across the visible bundles: in v0.32.0, `customer-profile-scale-10x1` is 28.798 ms optimized-registration vs 22.111 ms fallback with `MySqlMinimumOperationThreshold` causes, and `customer-profile-scale-10x10` is 43.905 ms optimized vs 37.033 ms fallback even when `MySqlStagedDataVaultSaveStrategy` is selected.
- Benchmark execution detail currently hardcodes PostgreSQL/MySQL optimized-path wording in `BenchmarkRunner` even when diagnostics report `ProviderNeutralFallback` or staged-provider decline; treat that as an actionable diagnostics/benchmark-artifact clarity gap, not just a labeling quirk.

Scope In
- Use the same Podman-backed provider setup and v0.32.0 artifact conventions to capture before/after evidence for any code or threshold change.
- For MySQL, evaluate deliberate provider-neutral fallback or higher eligibility thresholds only for tiny workloads with consistently worse live evidence, starting with `customer-profile-scale-10x1` and `customer-profile-scale-10x10`.
- Preserve and, if needed, clarify the existing MySQL two-lane distinction: below 60 operations can retain multi-row or provider-neutral behavior, while staged bulk remains the larger-batch lane.
- For PostgreSQL, keep the current optimized eligibility unless a fresh before snapshot on this ticket reproduces the small-batch regression; still fix diagnostic and artifact wording so retained direct or UNNEST behavior is distinguishable from staged COPY and from provider-neutral fallback.
- Update tests and benchmark-contract assertions that describe selected strategy, fallback causes, staged-provider phase, and execution-detail text for the adjusted providers.

Scope Out
- SQL Server and Oracle threshold work already owned by tickets `06F9XD2M71D1XFT7FJX62KD8HM` and `06F9XD2TGEYEG6S0AK86YF295M`.
- Any provider-wide retuning justified only by the historical 2026-06-06 seed bundle when the 2026-06-07 v0.32.0 baseline disagrees.
- New provider lanes, DB orchestration, stored-procedure deployment, or changes to the public `IDataVaultSaveService` contract.
- Promoting external-provider rows into the root checked-in `benchmark-summary.*` rollup as part of this ticket.

Open questions
- none

Follow-up questions
- After this ticket lands, should the parent story `06F9XD1T3TJK7NEBYNVT2JEPZW` add a separate benchmark-stability follow-up if PostgreSQL or MySQL medium rows continue to flip between v0.31 and v0.32 style outcomes?
- If MySQL mid-sized rows remain inconsistent across reruns, should a later task introduce a separate evidence-only calibration band instead of widening this ticket beyond tiny workloads?

Risks
- The visible benchmark history already flips PostgreSQL tiny-row results between 2026-06-06 and 2026-06-07, so any one-off rerun can mislead unless before and after inputs stay identical and the comparison path is explicitly recorded.
- MySQL medium rows (`100x1`, `100x10`, `1000x1`) are not stable across the two visible bundles, so tuning above tiny workloads can easily trade one regression for another.
- Current execution-detail wording can overstate provider-specific execution even when diagnostics show fallback or staged decline, which risks incorrect release-note or documentation claims if not corrected alongside any threshold change.
- Because this ticket blocks documentation task `06F8KZVRARQPG482YKCQ686PNM`, leaving benchmark wording ambiguous can propagate stale provider claims downstream even if runtime behavior is correct.

Split recommendations
- No additional split is required if implementation keeps the ticket bounded to MySQL tiny-workload eligibility plus PostgreSQL diagnostics or no-change unless reproduced.
- If a fresh PostgreSQL before snapshot reproduces a separate small-batch regression that needs its own eligibility rule, create a dedicated follow-up instead of widening the MySQL tuning work.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment