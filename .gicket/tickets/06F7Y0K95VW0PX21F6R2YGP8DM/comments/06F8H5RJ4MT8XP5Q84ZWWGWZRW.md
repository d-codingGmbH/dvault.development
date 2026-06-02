[gicket-bot] PO refinement contract

Summary
- Refined the story around a deterministic repository-side verifier for the checked-in benchmark artifact triplet, the four benchmark-backed performance-profile categories, and the current provider-native evidence rows used by active docs and provider-tuning diagnostics.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This story is about verifying checked-in repository evidence, not rerunning benchmarks or requiring live external providers in CI.
- For v1, 'supported provider profiles' means the checked-in performance-profile guidance surface and the current benchmark provider matrix: required SQLite local temporary files plus optional PostgreSQL, SQL Server, MySQL, and Oracle external-provider rows. It does not reopen the separate EF metadata provider-profile annotation surface.
- The verifier should treat skipped optional-provider rows as valid evidence when the row is present with executionStatus=skipped, iterations=0, a normalized skip reason, and planned execution detail.
- Stale means the active evidence or copied guidance has drifted from the current artifact contract: missing files, missing required rows or dimensions, row-set mismatch across markdown/CSV/JSON, missing required context fields, or copied performance-profile values and run-context facts that no longer match the verified artifact source.
- Regression-budget validation should ratify the shared defaults already documented in the performance-evidence contract: the targeted metric must improve or hold, required SQLite non-target regressions over 5% fail by default, and configured optional-provider regressions over 10% require explicit callout and justification.

Scope In
- Add a deterministic verifier that validates the checked-in root benchmark artifact triplet `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` against the current repository contract.
- Validate the expected root scenario and provider row set from the current harness: customer-profile history, bulk insert-only, bulk history, streaming save variants, order-product fulfillment history, latest-satellite read, PIT as-of read, bridge traversal read, compiled-model startup, compiled-query hub read, DbContext pooling, and provider-native bulk rows for PostgreSQL, SQL Server, MySQL, and Oracle.
- Validate required measured dimensions and schema semantics across the artifact triplet, including scenario/provider/baseline identity, strategy family, dataset size, change ratio, execution status, skip reason, iterations, timing metrics, allocation metrics, execution detail, persisted outcome, and run-context metadata including optionalProviders.
- Validate that current performance guidance and provider-tuning profile mappings remain anchored to verified evidence, at minimum by checking `docs/performance-profiles.md` against the root artifact triplet and the four checked-in profile categories Small app-local vault, Medium chunked ingestion, Staged provider ingestion, and Read-model heavy.
- Validate that active guidance backed by skipped provider-native rows only cites the current checked-in retained-path and staged-path baselines for PostgreSQL and MySQL plus the current SQL Server and Oracle optimized rows, instead of inventing unsupported provider claims.
- Validate repository-owned regression-budget metadata or expectations used by the verifier against the shared benchmark artifact contract so downstream guidance does not drift from the documented 5% and 10% default gates.

Scope Out
- Re-running benchmarks, provisioning external providers, or requiring live PostgreSQL, SQL Server, MySQL, or Oracle timings as part of the verifier.
- Hosted dashboards, external performance services, CI publication pipelines, or benchmark result transport/reporting infrastructure.
- Adding new benchmark scenarios, new provider families, or new performance-profile categories beyond the current checked-in root harness and four profile categories.
- Validating every exploratory artifact directory under `artifacts/benchmarks` or backfilling every historical release-note bundle that is not part of the active performance-profile or provider-tuning guidance surface.
- Automatic adjudication of fresh before/after performance wins from new benchmark runs; v1 is a checked-in evidence and citation verifier, not a live benchmark approval system.

Open questions
- none

Follow-up questions
- After v1 lands, should the same verifier be widened to `README.md` and `docs/production-adoption-checklist.md` current benchmark citations, or should those stay as separate documentation-alignment checks?
- Should a later story add computed before/after budget evaluation for the historical tuning bundles under `artifacts/benchmarks/*/before` and `after`, instead of limiting v1 to checked-in evidence schema and guidance consistency?
- If the team wants historical release-note benchmark links to stay machine-verified, should those older bundles be backfilled to the current artifact schema or tracked through an explicit historical manifest?

Risks
- The shared regression-budget rules currently live in contract documentation, so duplicating them carelessly in verifier code can create silent drift unless one deterministic expectation source is maintained.
- Optional external-provider evidence is environment-dependent; the verifier must accept documented skipped rows and reject silent omission, or it will produce false failures or false confidence.
- The repository contains exploratory and historical benchmark directories with older shapes; widening v1 indiscriminately beyond the active guidance surface will create noise and obscure real drift in current evidence-backed docs and diagnostics.

Split recommendations
- No split is required for the current bounded verifier story.
- If the team later wants full historical artifact archive validation or live before/after regression adjudication, split that into separate follow-up work instead of widening this story beyond the active checked-in guidance surface.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment