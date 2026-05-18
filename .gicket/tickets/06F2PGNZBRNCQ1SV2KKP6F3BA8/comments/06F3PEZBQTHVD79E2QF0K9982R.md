[gicket-bot] PO refinement contract

Summary
- Refined the story to the remaining benchmark-validity gap: keep the existing harness and artifacts, but require provider-native bulk rows to use provider-eligible write shapes and prove selected strategies before docs reuse the timings.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current ticket comments are only the bot claim and lease comments, the ticket has no persisted attachments, and no child ticket, relation change, attachment, or planning document was created in this pass.
- Repository evidence already fixes the v1 benchmark baseline: the existing benchmark project and benchmark README keep classic EF as the required local SQLite comparison lane, while optional external providers are DVault fallback versus provider-specific rows with deterministic skipped-row handling.
- The current external-provider matrix is not yet safe to treat as native-proof everywhere: MySQL, Oracle, and SQL Server optimized write rows can silently fall back on undersized batches, SQL Server also declines batches with more than 500 satellite operations, and only SQLite currently exposes a provider-specific read strategy.
- Existing relations remain coherent without cleanup: parent epic 06F2PGMFWSEC95ATBCGZ6HYT5W still owns this story, done blockers 06F2PGK4QJ0YGXK5479W83Z2J0 and 06F2PGNGVQ3TZZWSABAK5SNFK4 are historical ordering context, and docs task 06F2PGP2B2RZGGK3CVKK5WRRP8 remains the downstream documentation owner.

Scope In
- Tighten the bulk-write benchmark matrix so any row presented as provider-native optimized bulk execution uses a provider-eligible request shape and proves the named strategy was selected.
- Preserve the existing required-local SQLite write comparisons for classic EF, AddDVault fallback, and AddDVaultSqlite optimized execution.
- Align optional external-provider native benchmark shapes with the current live bulk-test and strategy-gate baseline so native-versus-fallback timings stay comparable.
- Keep the benchmark artifact contract in scope: benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json remain the documentation-ready evidence surface.

Scope Out
- New provider save-strategy algorithms, provider gate-threshold changes, or fallback semantic changes already owned by the completed implementation tickets.
- Live external-provider integration test implementation, which stays with done task 06F2PGNT7DF4DVNKYWDFZC8DEM.
- Read-model benchmark expansion or non-SQLite provider-specific read claims; this story stays on bulk-ingestion write evidence.
- Broader README, release-note, or adoption-document packaging beyond any narrow benchmark guidance needed to keep the evidence boundary accurate.

Open questions
- none

Follow-up questions
- Should docs task 06F2PGP2B2RZGGK3CVKK5WRRP8 publish any crossover guidance only from completed benchmark-summary artifacts so the provider and machine context stays attached to copied timings?
- If a future release adds non-SQLite provider read strategies or wants a broader scale-matrix publication, should that be tracked in a separate follow-on benchmark ticket instead of widening this bulk-ingestion story?

Risks
- Without strategy-selection proof, benchmark output can mislabel fallback timings as provider-native results and create false performance claims.
- External-provider timings remain environment-sensitive because they depend on developer-managed databases and conditional provider dependencies, so downstream docs must preserve skip status and run context.
- Cross-provider comparisons will drift if benchmark request shapes stop matching the bounded native-strategy eligibility proven by the live bulk integration tests.

Split recommendations
- No additional split is recommended; the current graph already separates fallback implementation, native strategy implementation, provider integration coverage, benchmarks, and documentation.
- If future work needs read-strategy benchmarking or a materially broader benchmark matrix, open a fresh follow-on ticket instead of widening this story.

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