[gicket-bot] PO refinement contract

Summary
- Refined the story around SQLite-backed benchmark evidence and bounded consumer guardrails for the repository-owned compiled-model, compiled-query, and pooled-DbContext paths, reusing the existing benchmark artifact contract instead of redefining it.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ticket 06F492BZPP5YT9SJSPDHQBGF3R is already done, so this story should consume the existing benchmark artifact, allocation, SQL-capture, and regression-budget contract instead of reopening those decisions.
- The current repository already proves compiled-model and compiled-query compatibility on SQLite in docs/architecture/dvault-ef-compiled-compatibility.md and tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs, but that note explicitly says no performance claim exists yet.
- The repository-owned compiled-model boundary for this story is the documented UseModel(runtimeModel) path built from a DVault-projected design model; DVault does not need to generate, own, or benchmark EF consumer-owned compiled-model code artifacts separately in v1.
- Compiled-query evidence should stay on stable direct EF query expressions over generated shared-type DVault tables with deterministic projections; dynamic IDataVaultReadService request composition remains the default path for flexible reads and is out of scope for compilation claims.
- SQLite is the required local evidence baseline for this story; provider-specific compiled-model, compiled-query, or pooling guarantees for PostgreSQL, SQL Server, MySQL, and Oracle are follow-up expansion work rather than a blocker for v0.18.
- The pool-friendly v1 baseline should use a standard DbContextOptions<TContext>-only context plus one fixed metadata source per context model, because the README already documents that caller-owned model-shape discriminators outside DVault registry selection require caller-owned model-cache-key handling.

Scope In
- Add benchmark evidence for the bounded compiled-model path, compiled-query path, and DbContext pooling path within the existing benchmarks/DCoding.Data.DVault.Benchmarks harness and artifact contract.
- Define or update repository documentation so the compiled-model/query note moves from compatibility-only wording to compatibility plus bounded performance-evidence wording where evidence now exists.
- Document only the DVault-specific consumer guardrails that matter for these features, such as stable generated-table query shapes and fixed metadata/model-shape assumptions for pooled contexts.
- Add automated verification that the new benchmark rows, artifact fields, and scenario naming stay stable enough for downstream regression-baseline and documentation tickets to consume.
- Produce evidence that downstream tickets 06F492CTREZEDXVKJ839YGCPWW and 06F492D05THPGQVT3B3K7853A0 can reuse without inventing separate benchmark formats or baseline assumptions.

Scope Out
- Adding a DVault-owned compiled-model generator, design-time command wrapper, or provider-specific compiled-query optimizer.
- Reworking dynamic IDataVaultReadService APIs into compiled delegates or promising compiled support for dynamic request-built read shapes.
- General provider-neutral read-allocation tuning, save change-tracker tuning, or provider-optimization work already covered by sibling tickets 06F492CAB2293R7BGJWMWMRKT4, 06F492CFSJHN0RGXXRG3KT63FM, and 06F492CTREZEDXVKJ839YGCPWW.
- A broad external-provider performance matrix for compiled or pooled scenarios beyond the required SQLite baseline.
- Final v0.18.0 release-note packaging and broad end-user documentation rollup, which remain with ticket 06F492D05THPGQVT3B3K7853A0.

Open questions
- none

Follow-up questions
- After the SQLite baseline lands, should configured PostgreSQL, SQL Server, MySQL, or Oracle providers gain matching compiled-model, compiled-query, or pooling scenario rows, or should those remain SQLite-only until a provider-specific need appears?
- After AddDbContextPool<TContext> is covered, should a separate follow-up benchmark AddPooledDbContextFactory<TContext> for background-worker or factory-driven consumer patterns?
- When ticket 06F492D05THPGQVT3B3K7853A0 packages the v0.18 evidence, should it publish per-scenario artifact links and SQL captures or summarize the recommendations once and link one artifact bundle per release?

Risks
- Compiled-model wins are easy to misattribute if the timed window mixes database setup, seeding, runtime-model creation, and steady-state work instead of isolating the actual UseModel(...) effect.
- DbContext-pooling numbers will be misleading if each iteration rebuilds the service provider, metadata registry, or SQLite database instead of reusing a fixed pooled configuration and measuring only the intended context-acquisition or operation path.
- If the documentation generalizes SQLite measurements into provider-neutral promises, consumers may infer compiled or pooling guarantees that the repository has not actually measured.
- If the story stops at prose and compatibility tests without new benchmark rows and artifact assertions, downstream tickets will still lack reusable performance evidence despite the documentation update.

Split recommendations
- No split recommended; keep compiled model, compiled query, and DbContext pooling evidence together because they share the same benchmark harness, SQLite baseline, and consumer-guardrail documentation boundary.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment