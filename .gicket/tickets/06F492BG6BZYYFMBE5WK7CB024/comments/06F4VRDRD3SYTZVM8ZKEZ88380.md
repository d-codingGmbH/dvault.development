[gicket-bot] PO refinement contract

Summary
- Verified the todo ticket, bot-only comments, no attachments, current epic/prerequisite/doc relations, and the repo-local preflight/report APIs; refined the story to a consumer-called composite preflight facade that reuses existing diagnostics, drift, guardrail, and request-bound diagnostics surfaces, and no child tickets, relation writes, description updates, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current ticket has only bot lease/claim comments and no ticket-local attachments, so the authoritative refinement inputs were the ticket body, checked-in repo docs/APIs, and persisted relation state.
- Current relation files keep the ticket under epic 06F492A3MPSGP3KXDNZECN01QM, record prerequisite blocks links from completed stories 06F492A8WV0EP2V03CWXXWH71G, 06F492AE2C8XBDXDH4V2JPTJDR, 06F492AKGMKPCRJYF4Z1EC9WY4, and 06F492B40K7B0WWPKH8N3PPG3G, and keep the outgoing blocks link to documentation task 06F492BNDPWS9P4EDSV0W7G6VM; no relation cleanup was justified.
- Repository evidence already provides the reusable building blocks this story should compose: DataVaultDesignTimeCommand and DataVaultDesignTimeCommandHost, IDataVaultDiagnosticsService.Analyze(DbContext), DataVaultModelDriftPreflightReporter.Compare(...), DataVaultMigrationOperationDiagnostics.AnalyzeReport(...), and DataVaultDiagnosticsResult explain/read-strategy surfaces.
- Completed story 06F492AE2C8XBDXDH4V2JPTJDR already ratifies the explicit snapshot-model IReadOnlyModel boundary, so this story should consume that input rather than add ModelSnapshot coupling or repository snapshot discovery.
- Completed story 06F492AKGMKPCRJYF4Z1EC9WY4 already ratifies the built-in registry-backed model-cache guarantee and the consumer-owned IModelCacheKeyFactory escape hatch, so v1 here should surface that boundary through existing annotations and drift evidence rather than inventing cache-stress probes.
- Todo story 06F492B9PR036PDNN52S06S9BC remains the place for richer query-shape and index-hint diagnostics; this story should own the aggregate preflight envelope and accept explicit request-bound diagnostics inputs without inventing representative queries.

Scope In
- Add one additive library-owned in-process preflight facade and composite report in src/DCoding.Data.DVault that consumers can call from tests, CI, startup, or a thin consumer-owned CLI wrapper.
- Aggregate the existing validation/provider-explain baseline, artifact-versus-design-time drift, snapshot-model preflight drift, migration guardrail output, and explicit request-bound diagnostics into one deterministic result with per-lane status and overall blocking state.
- Support the current authoritative expected-model inputs already used elsewhere in the repo: DataVaultMetadataModel and successful DataVaultModelImportResult.
- Accept optional explicit inputs for snapshot-model IReadOnlyModel, migration operations, reviewed artifact/import result, and representative diagnostics requests or results instead of assuming repo paths, migration discovery, or query capture.
- Surface model-cache-relevant evidence through metadata-source kind/fingerprint and runtime-versus-expected drift lanes so adopters can see when the realized model no longer matches the authoritative metadata or snapshot.
- Provide deterministic human-readable rendering that summarizes passed, blocked, and skipped lanes without reclassifying the underlying DMV, DVM, or drift diagnostics.

Scope Out
- No standalone dvault CLI, hosted service, platform, dashboard, or background orchestration.
- No dotnet ef interception or shim, automatic migration scaffolding/application, migration-name discovery, snapshot-file discovery, or repository scanning.
- No default live database connection or mandatory live-schema drift lane; any live-schema use remains explicit consumer-managed follow-up work.
- No new diagnostics code family or parallel taxonomy for guardrails, drift, provider behavior, or query shape; reuse the current report and diagnostics surfaces.
- No heuristic detection of arbitrary caller-owned model-shaping state or automatic validation that a custom IModelCacheKeyFactory carries every tenant, schema, or profile discriminator.
- No repo-wide LINQ inspection, query interception, or invented representative read requests; richer read-shape and index-hint analysis stays with 06F492B9PR036PDNN52S06S9BC and should flow into this envelope additively.

Open questions
- none

Follow-up questions
- When 06F492B9PR036PDNN52S06S9BC lands, should its richer query-shape and index-hint diagnostics flow through the same aggregate request-diagnostics section with no envelope change?
- Should 06F492BNDPWS9P4EDSV0W7G6VM publish one canonical consumer-owned aggregated preflight wrapper example for CI and one for startup once this contract is implemented?
- Should a later opt-in story add live-schema drift aggregation to the same envelope for managed environments, or keep live-schema as a separate operational lane?

Risks
- If the composite report reclassifies or copies underlying diagnostics instead of carrying the existing report objects, it can drift from the provider, drift, and guardrail semantics already ratified in completed prerequisite stories.
- If implementation starts auto-discovering migrations, snapshots, queries, or consumer cache-key state, it will violate the repository's consumer-owned design-time boundary and create brittle automation behavior.
- If pass/block/skip rules are not deterministic across omitted lanes and optional request inputs, CI and startup consumers will get unstable results from the same preflight contract.
- If the aggregate request-diagnostics section is shaped too narrowly around today's read-strategy output, the separate query-shape diagnostics story will force a breaking redesign instead of additive expansion.

Split recommendations
- No new child-ticket split is recommended; the main prerequisite library surfaces are already covered by completed stories 06F492A8WV0EP2V03CWXXWH71G, 06F492AE2C8XBDXDH4V2JPTJDR, 06F492AKGMKPCRJYF4Z1EC9WY4, and 06F492B40K7B0WWPKH8N3PPG3G.
- Keep richer read-query-shape and index-hint logic on existing story 06F492B9PR036PDNN52S06S9BC, and keep broad adoption/release-note rollout on 06F492BNDPWS9P4EDSV0W7G6VM.
- If a future iteration wants automatic live-schema aggregation, repo discovery, or query interception, raise that as a separate follow-up story instead of widening this v1 composite facade.

Persisted contract coverage
- acceptance-criteria items: 9
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment