[gicket-bot] PO refinement contract

Summary
- Refined the story around the existing repo-local benchmark harness in `benchmarks/DCoding.Data.DVault.Benchmarks`, ratified the SQLite-only comparable-harness baseline and deterministic artifact contract, and noted the existing child split via `parentOf` relations to 06EXB7TE0806E7EY5ZBATHQNK8 and 06EXB7TP9PF2XFRQ9MG7CJQR10. No new child tickets or planning documents were created in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- V1 does not need BenchmarkDotNet specifically; the accepted baseline is the existing repeatable repo-local executable harness under `benchmarks/DCoding.Data.DVault.Benchmarks`.
- The benchmark harness scope is SQLite local temporary files only and explicitly excludes Postgres, Docker, external services, and machine-specific secrets.
- Documentation-ready output is concretized as deterministic `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` artifacts from a single benchmark run.
- `Results distinguish normal EF and DVault scenarios` means the output must report separate baseline rows for `conventional-ef` and `dvault-explicit-save` for each required scenario and include persisted-outcome summaries that make the storage-shape difference visible.
- The story already has two bounded child relations (`parentOf` to 06EXB7TE0806E7EY5ZBATHQNK8 and 06EXB7TP9PF2XFRQ9MG7CJQR10); this refinement keeps the parent focused on shared harness and artifact expectations rather than creating more split tickets.

Scope In
- A repo-local benchmark executable project under `benchmarks/DCoding.Data.DVault.Benchmarks` that is wired into `DVault.slnx` and runnable from the repository root.
- SQLite comparison execution for both `conventional-ef` and `dvault-explicit-save` baselines.
- Customer profile history comparison using the fixed `C-100` two-event contract and DVault persisted-outcome shape.
- Order-product fulfillment comparison using the reduced `O-1000` and `SKU-COFFEE` contract already fixed in the benchmark scenario code.
- Deterministic markdown, CSV, and JSON summary artifact generation for documentation use.
- Automated coverage that proves benchmark execution and artifact emission for the required scenarios.

Scope Out
- Adding Postgres benchmarks or non-SQLite provider comparisons in this ticket.
- Changing DVault to a `SaveChanges` interception write model or otherwise replacing the explicit save-service benchmark path.
- Setting absolute performance thresholds, CI perf gates, or pass-fail timing budgets.
- Publishing or archiving benchmark artifacts outside the produced files themselves.
- Reworking the v1 harness around BenchmarkDotNet unless a later ticket explicitly requires that change.

Open questions
- none

Follow-up questions
- After the SQLite v1 baseline is stable, do we want later tickets for additional providers or environments?
- Do we want a later documentation or operations ticket to persist selected benchmark summary artifacts under `docs/` or as ticket attachments for long-lived historical comparison?
- After the blocking dependency from 06EXB7GYQKBZ8FMQN6YDYCKATG is fully integrated, do we want one reference-machine rerun to capture the first publishable benchmark artifact set?

Risks
- Timing numbers are machine-specific; if benchmark tables are copied without the captured provider, runtime, and hardware context, the documentation can misrepresent the results.
- The v1 harness intentionally measures SQLite local temporary-file behavior only, so readers may overgeneralize the results to providers or concurrency shapes the current DVault profile does not support.

Split recommendations
- No additional split is needed during PO refinement; keep the existing child split to 06EXB7TE0806E7EY5ZBATHQNK8 and 06EXB7TP9PF2XFRQ9MG7CJQR10 and keep this parent story focused on the shared harness and artifact contract.
- If later expansion is needed, split by new provider or environment coverage and by artifact-publication work rather than reopening the fixed SQLite v1 comparison scenarios.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment