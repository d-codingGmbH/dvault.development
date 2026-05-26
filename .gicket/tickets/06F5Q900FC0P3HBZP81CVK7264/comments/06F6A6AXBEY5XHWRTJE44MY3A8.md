[gicket-bot] PO refinement contract

Summary
- Refined the story around the existing provider-native bulk benchmark surface, the staged/direct provider thresholds already visible in the repo, and the shared artifact-contract budgets; no PO-blocking questions remain.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Done provider stories already landed the current staged or provider-specific save behavior for PostgreSQL, SQL Server, MySQL, and the retained direct Oracle path, so this ticket is benchmark-and-evidence work only.
- The visible benchmark baseline is the existing `provider-native-bulk-ingestion` scenario with external PostgreSQL, SQL Server, MySQL, and Oracle rows that remain present as completed rows when configured or skipped rows with normalized reasons when not configured.
- The shared artifact contract already fixes the row schema and regression-budget defaults: the targeted metric must improve or hold, required SQLite non-target regressions above 5% fail by default, and configured optional-provider regressions above 10% must be called out and justified.
- Repository evidence already fixes the current provider-path boundaries: PostgreSQL stays on direct or UNNEST below its 60-operation staged threshold, MySQL keeps a 50-operation native gate with staged bulk at 60+, SQL Server uses its current native bulk gate, and Oracle keeps the retained direct batching path with staged Oracle still `not-selected-no-measured-win`.
- No human comments or attachments added extra scope beyond the ticket description and referenced repository documents.

Scope In
- Extend benchmark coverage for staged-bulk comparison rows on the existing provider-native bulk-ingestion surface, reusing the current artifact schema and benchmark triplet.
- Make the matrix distinguish provider-neutral fallback, retained provider-native direct or multi-row paths where they exist, and staged-provider paths where they exist.
- Preserve normalized skipped optional-provider rows and planned execution detail for all staged/direct comparison rows when external providers are not configured.
- Capture before/after evidence for this ticket under the shared artifact contract with explicit staged-bulk targeted rows and the existing regression budgets.
- Add or update benchmark contract tests and benchmark-facing docs only as needed to describe the new matrix and budget application.

Scope Out
- Changing provider save semantics, thresholds, or strategy-selection behavior already owned by the landed provider stories.
- New public save APIs, chunked provider-specific execution work, or staged SPI and transaction-contract redesign.
- Benchmark artifact schema redesign or new artifact file types beyond the existing markdown, CSV, and JSON triplets plus optional SQL capture already defined in the shared contract.
- Broad release-note, README, production-checklist, or stored-procedure positioning work already owned by `06F5Q90718D21DN1N1Q2AP7YEM`.
- Rewriting historical release bundles as the public claim record for past releases; new evidence should stand on its own ticket or release label.

Open questions
- none

Follow-up questions
- After this matrix lands, should `06F5Q90718D21DN1N1Q2AP7YEM` publish the staged/direct provider comparison as the v0.20.0 documentation baseline rather than relying on prose-only staged-bulk guidance?
- If Oracle later gains a runnable staged path with a measured win over the retained direct path, should a separate follow-up add Oracle direct-versus-staged timing rows instead of widening this ticket beyond the current benchmark baseline?

Risks
- Because PostgreSQL, SQL Server, MySQL, and Oracle rows remain external opt-in, unattended runs may still archive skipped rows only; the contract must stay informative enough that missing live providers does not look like missing matrix coverage.
- If the new matrix does not separate direct, staged, and fallback row identities cleanly, regression budgets and downstream docs will compare the wrong execution paths.
- Updating or superseding historical provider-optimization bundles without a clearly labeled new evidence set could blur release provenance and make regressions harder to interpret.

Split recommendations
- No additional split is needed for PO refinement if the work stays on benchmark harness, artifact evidence, and benchmark-contract documentation for staged bulk comparisons.
- If future work wants cross-scenario budget policy changes beyond provider-native bulk ingestion, split that governance work into a separate artifact-contract ticket rather than widening this story.

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