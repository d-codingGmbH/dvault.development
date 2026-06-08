[gicket-bot] PO refinement contract

Summary
- Refined the SQL Server threshold task against the completed v0.32.0 baseline, ratified the current 50-minimum/500-satellite gate baseline, and narrowed the work to measured SQL Server tuning plus removal of misleading planned-path diagnostics; no persistent planning writes were applied.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Prerequisite evidence ticket 06F9XD26D2MHVAKZ2GCZ67BEFC is done; its v0.32.0 artifact bundle is the evidence source for this task, so the live incoming blocks relation is historical rather than a blocker.
- Repository baseline already fixes the SQL Server gates at 50 minimum total operations and 500 maximum satellite operations through DataVaultDiagnostics, telemetry explanations, performance guidance, and the activity-tracing fallback contract.
- The scale benchmarks analyze only the satellite bulk request after hub rows are written, so 10x1 = 10 satellite operations, 100x1 and 10x10 = 100, and 1000x1 and 100x10 = 1000; that already explains why only the 100-operation rows currently select SQL Server.
- The concrete ambiguity to resolve is in benchmark detail generation: scale rows still prepend SQL Server staged native bulk execution wording even when diagnostics report saveStrategyStatus=ProviderNeutralFallback and selectedStrategy=<none>.
- No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Capture comparable SQL Server before/after evidence under the shared benchmark artifact contract and reuse the completed v0.32.0 baseline bundle as the pre-tuning reference.
- Re-evaluate the SQL Server 50-minimum and 500-maximum-satellite save gates for the customer-profile scale rows and any directly comparable SQL Server provider-native bulk rows where measured evidence justifies a change.
- Fix benchmark and diagnostics wording so completed rows that actually fell back do not claim that the SQL Server staged/native path executed.
- Preserve or extend tests around SQL Server gate evaluation, telemetry and diagnostics explanations, benchmark execution-detail reporting, and save-path semantics.

Scope Out
- Changing PostgreSQL, MySQL, or Oracle thresholds owned by sibling tickets under story 06F9XD1T3TJK7NEBYNVT2JEPZW.
- Inventing a new benchmark artifact format or replacing the shared before/after benchmark-summary triplet contract.
- New provider packages, DB2 work, or Podman orchestration changes.
- Forcing SQL Server provider-native dispatch for batches above 500 satellite operations without measured evidence and preserved semantics.

Open questions
- none

Follow-up questions
- After SQL Server tuning lands, should a later docs or release ticket promote one SQL Server before/after bundle into the root checked-in benchmark-summary rollup, or should the root rollup stay a lightweight shared baseline?
- If the 500-satellite ceiling proves intentionally protective, should a later UX or docs pass add friendlier benchmark-facing wording beyond the bounded diagnostics strings required here?

Risks
- The current scale benchmark detail generator can keep misleading SQL Server staged native bulk wording even when diagnostics prove provider-neutral fallback, which can hide whether a threshold change actually altered execution.
- The 1000-plus-satellite rows currently look better than provider-neutral fallback even while remaining fallback executions, so changing the 500 ceiling without verifying actual provider-native semantics could create false performance conclusions.
- Because the benchmark writes hubs separately from the analyzed satellite bulk request, anyone reasoning from total end-to-end row counts instead of satellite-operation counts can misread why specific scale rows cross or miss the SQL Server gate.
- Live SQL Server evidence still depends on the shared Podman sqlserver endpoint, so environment drift can blur threshold conclusions with infrastructure noise.

Split recommendations
- No new split is justified. SQL Server threshold tuning and SQL Server fallback-versus-executed diagnostics wording are one bounded refinement surface under story 06F9XD1T3TJK7NEBYNVT2JEPZW.
- If a later release or documentation pass needs broader artifact-lane wording changes after all provider-tuning tickets finish, keep that as follow-up work on 06F8KZVRARQPG482YKCQ686PNM instead of widening this task now.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment