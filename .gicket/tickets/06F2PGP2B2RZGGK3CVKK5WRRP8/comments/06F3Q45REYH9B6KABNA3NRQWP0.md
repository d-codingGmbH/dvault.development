[gicket-bot] PO refinement contract

Summary
- Verified repository and local `.gicket` evidence: this v0.14.0 docs task is unblocked, no planning writes were needed, and the remaining work is bounded to current-guidance alignment plus a new `docs/releases/v0.14.0.md` release record for provider bulk ingestion and benchmark evidence.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository-local `.gicket` artifacts were sufficient to verify ticket, comment, and relation state: this ticket currently has only bot claim/lease comments, no persisted attachments, and no child-ticket, relation, attachment, or planning-document write was materialized in this pass.
- Live relation evidence shows parent epic `06F2PGMFWSEC95ATBCGZ6HYT5W` plus incoming `blocks` from `06F2PGMSQ4D4FV8W5ZERD4GS8C`, `06F2PGNGVQ3TZZWSABAK5SNFK4`, `06F2PGNZBRNCQ1SV2KKP6F3BA8`, and `06F2PGK4QJ0YGXK5479W83Z2J0`; the blocking stories relevant to v0.14 bulk ingestion are already `done`, so this ticket is now downstream documentation closure rather than blocked implementation work.
- Release `06F2PH9EF1YYJ8F6F6KWG4DBY8` is `v0.14.0 - Provider Bulk Ingestion`, and `docs/releases/` currently stops at `v0.13.0`, so the v0.14.0 release-note artifact is still missing.
- Current repository evidence already fixes the behavior baseline that docs must describe: explicit ordered bulk saves through `IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest)`, registry-backed `DataVaultRegistryBulkSaveRequest`, provider-neutral fallback, and diagnostics-gated provider-native strategies.
- Current README guidance already documents opt-in bulk-provider commands and bulk-lane wording for Postgres, SQL Server, and Oracle, while the MySQL section still needs parity and `docs/architecture/dvault-v1-explicit-save-service.md` still understates the current benchmark scope.
- Historical release notes such as `docs/releases/v0.5.0.md` should stay historical; the current baseline should be corrected in v0.14.0 release notes and current guidance docs instead of rewriting past release history.

Scope In
- Add `docs/releases/v0.14.0.md` with the coordinated seven-package scope, bulk-ingestion highlights, documentation updates, compatibility notes, known limitations, benchmark evidence boundary, and validation evidence for `v0.14.0 - Provider Bulk Ingestion`.
- Update current-release pointers and aligned package versions in `README.md`, `examples/README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, and any doc that still explicitly treats `v0.13.0` as the current public baseline.
- Document the shipped bulk-ingestion surface already in the repository: explicit ordered bulk saves, provider-neutral fallback, provider-native opt-in strategies, and the current eligibility gates exposed by `DataVaultDiagnostics`.
- Align current opt-in provider setup text with the shipped external bulk-provider lanes and restore-marker requirements, especially the MySQL section.
- Align benchmark and architecture guidance with the shipped harness and artifact contract: optional provider-native bulk rows, deterministic skipped rows, and preserved provider and hardware context.

Scope Out
- New persistence code, provider save-strategy algorithms, gate-threshold changes, or external integration and benchmark implementation work already delivered by done sibling tickets.
- New runnable bulk quickstart projects, checked-in benchmark result snapshots, or repository-managed Docker, Podman, or database provisioning.
- Retroactive edits that rewrite historical release notes as if they were current guidance.
- Non-SQLite provider-specific read-optimization claims or broader read-benchmark expansion.
- Workflow or status bookkeeping and relation cleanup beyond keeping the contract consistent with the current live graph.

Open questions
- none

Follow-up questions
- If a later release wants a runnable bulk-ingestion quickstart or a checked-in sample benchmark artifact, should that be tracked as a separate docs or example ticket instead of widening this release-note closure task?
- If later consumer guidance needs a richer example of `DataVaultRegistryBulkSaveRequest` or typed bulk helper usage, should that be handled as a focused example-doc follow-up rather than a prerequisite for v0.14.0 release notes?
- If future releases add non-SQLite provider read strategies or materially different native-bulk gates, should benchmark and public guidance expand in a new follow-on ticket rather than retrofitting the v0.14.0 contract?

Risks
- Current docs still mix v0.13 latest-release wording with v0.14 bulk-ingestion behavior, so partial updates can leave consumers with conflicting version and feature baselines.
- Benchmark timings are machine- and provider-dependent, and optional-provider rows can be skipped; summarizing numbers outside the shipped artifact context can create misleading performance claims.
- If `README.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and the benchmark README are not updated together, current guidance can continue to drift behind shipped implementation and test evidence.
- Overstating provider internals beyond the current gate evaluator or benchmark harness would turn bounded current-release docs into unsupported future promises.

Split recommendations
- No additional split is recommended; the live graph already separates fallback implementation, provider-native strategies, external bulk-provider coverage, benchmarking, and this downstream documentation closure task.
- If later work needs a runnable bulk example, checked-in benchmark artifacts, or broader read or performance publication, open a fresh follow-on docs or example ticket instead of widening this v0.14.0 closure task.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment