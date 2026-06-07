[gicket-bot] PO refinement contract

Summary
- Refined the v0.32.0 all-provider baseline task against the shared benchmark artifact contract, the completed v0.31.0 scale seed bundle, and the existing cleanup-fix smoke evidence; clarified that the scale run supplies threshold-tuning evidence while a separate smoke/read verification covers bridge-traversal cleanup, and no persistent ticket or planning write was materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The authoritative artifact format remains the benchmark-summary.md / benchmark-summary.csv / benchmark-summary.json triplet from one execution, stored under a new v0.32.0 ticket-labeled artifacts/benchmarks path while keeping artifacts/benchmarks/v0.31.0-scale-5-all-providers-<redacted> available for comparison.
- Repository evidence shows that --scale is limited to customer-profile scale scenarios, so the required --provider all --scale --iterations 5 --warmup 1 run is the threshold-baseline bundle and does not by itself satisfy bridge/PIT read verification.
- Repository evidence also shows that SatCustomerStatu is an intentional current fixture/model baseline and that artifacts/benchmarks/v0.31.0-all-providers-smoke-after-cleanup-fix-<redacted> already proved completed bridge-traversal rows across PostgreSQL, SQL Server, MySQL, and Oracle after the cleanup fix; this ticket should repeat that bounded verification or record equivalent evidence alongside the scale bundle.
- The downstream tuning ticket set already exists and is the consumer of this evidence: story 06F9XD1T3TJK7NEBYNVT2JEPZW plus tasks 06F9XD2M71D1XFT7FJX62KD8HM, 06F9XD2TGEYEG6S0AK86YF295M, and 06F9XD33MNNVHHW232TC7T1CN8.
- No child-ticket creation, relation mutation, description update, attachment, or planning-document write was materialized in this refinement pass.

Scope In
- Run the benchmark harness with --provider all --scale --iterations 5 --warmup 1 against SQLite, PostgreSQL, SQL Server, MySQL, and Oracle using the documented Podman-backed provider endpoints.
- Persist the resulting v0.32.0 artifact triplet under a ticket-labeled artifacts/benchmarks path and preserve the existing v0.31.0 scale bundle for side-by-side comparison.
- Perform a bounded all-provider smoke/read verification that proves bridge-traversal rows stay green after the SatCustomerStatu cleanup fix across the shared external databases.
- Record the concrete threshold-driving rows and fallback causes that the existing downstream tuning tickets must cite.

Scope Out
- Changing provider thresholds, save/read strategy code, or other product behavior while capturing the baseline.
- Replacing the shared root benchmark-summary.md / .csv / .json rollup as part of this ticket unless a separate follow-up explicitly decides to do so.
- Adding DB2 or any new provider lane beyond SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Automatic database orchestration, deployment tooling, or non-benchmark operational automation around the Podman containers.

Open questions
- none

Follow-up questions
- After the v0.32.0 evidence bundle lands, should a separate docs/release task promote any completed all-provider rows into the root benchmark-summary rollup or keep the root triplet as the lightweight shared baseline?
- Should follow-up tuning tickets preserve raw command stdout/stderr logs beside their before/after artifact triplets for rerun auditing even though the artifact contract itself only requires the summary triplet?
- When the downstream tuning tasks capture before/after evidence, should they reuse one shared v0.32.0 baseline label or keep provider-specific before/after bundles per task?

Risks
- The checked-in root benchmark-summary triplet still reflects a skipped external-provider posture, so downstream work may cite the wrong baseline if the new v0.32.0 bundle is not explicitly referenced.
- Because --scale does not execute bridge or PIT read rows, relying on the scale artifact alone would miss the SatCustomerStatu cleanup verification for bridge-traversal-read.
- External-provider results depend on live Podman endpoints and conditional provider packages; misconfigured connection strings or running PostgreSQL outside the Podman network can produce false skips or failures unrelated to product behavior.
- If the implementer tweaks thresholds or provider code while capturing this baseline, the evidence becomes unusable as a pre-tuning snapshot and undermines the downstream comparison tasks.

Split recommendations
- No new split is justified; keep this ticket as the evidence-capture prerequisite for the existing tuning story 06F9XD1T3TJK7NEBYNVT2JEPZW and tasks 06F9XD2M71D1XFT7FJX62KD8HM, 06F9XD2TGEYEG6S0AK86YF295M, and 06F9XD33MNNVHHW232TC7T1CN8.
- If bridge-traversal cleanup verification expands beyond a quick bounded smoke/read rerun, create a separate validation-only follow-up instead of widening this baseline ticket into product-code or diagnostics work.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment