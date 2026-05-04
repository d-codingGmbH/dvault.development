Goal: emit benchmark artifacts that compare provider, strategy, dataset size, and change ratio.

Acceptance Criteria:
- Artifacts include the provider-neutral fallback, optimized provider path where available, and the classic EF baseline where feasible.
- Large insert-only and large change-heavy scenarios are represented.
- The output location stays under artifacts and does not introduce tracked bin or obj output.