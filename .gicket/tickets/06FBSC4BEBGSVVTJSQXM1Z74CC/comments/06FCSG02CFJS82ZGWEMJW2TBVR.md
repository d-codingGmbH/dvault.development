[gicket-bot] PO refinement contract

Summary
- Refined the ticket to ratify the existing baseline provider-optimization evidence surface: SQLite completed root-triplet rows, checked-in v0.32 external-provider bundles where present, and explicit skipped or diagnostics-only placeholders for unavailable external providers including DB2.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already fixes the baseline evidence surfaces: docs/plans/provider-optimization-evidence-matrix.md is the canonical lookup surface, benchmark-summary.md/csv/json are the root artifact triplet, and docs/performance-profiles.md cites the checked-in v0.32 PostgreSQL, SQL Server, MySQL, and Oracle evidence bundles.
- For this ticket, unavailable provider coverage means an explicit provider row is preserved with executionStatus=skipped, iterations=0, blank or null metrics, persistedOutcome=not executed, and a normalized skip reason rather than silently omitting the provider.
- The normalized unavailable categories are already repository-visible through BenchmarkSkipReason: not configured, provider dependency unavailable, and connection unreachable.
- SQLite is the completed required-provider baseline; PostgreSQL, SQL Server, MySQL, and Oracle may use existing checked-in completed bundles where present or skipped-placeholder rows when a local run is unavailable; DB2 remains skipped-placeholder and/or diagnostics-only or smoke-only unless a reachable DB2 connection string produces a checked-in benchmark triplet.
- No child tickets, relation changes, description updates, attachments, or planning documents were applied or queued in this run.

Scope In
- Ratify and populate the baseline provider-optimization evidence set for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 using the existing matrix and artifact vocabulary.
- Use the root benchmark triplet for SQLite completed rows and for explicit skipped placeholder rows when optional provider lanes are unavailable.
- Cite the existing checked-in v0.32 external-provider evidence bundles for PostgreSQL, SQL Server, MySQL, and Oracle when those bundles provide the authoritative completed timing claim.
- Keep DB2 in the current bounded baseline as skipped-placeholder, diagnostics-only, or smoke-only unless a checked-in DB2 benchmark triplet is produced from a configured local run.
- Preserve provider, scenario, baseline, strategy family, evidence posture, and stop or fallback facts so downstream gap-matrix work can consume one consistent evidence surface.

Scope Out
- Do not create new provider strategies, new provider implementations, or new DB2 completed timing claims without a configured and checked-in benchmark run.
- Do not invent a new artifact schema or replace benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, or the existing dvault.provider-evidence.v1 mapping contract.
- Do not provision external databases, credentials, containers, or CI infrastructure.
- Do not broaden this ticket into binary-vs-hex provider-expansion work or cross-provider storage-footprint claims.
- Do not publish the prioritized consumer-facing gap matrix here; that remains downstream story 06FBSC4HSXFJ5FM6GWECH2CTGG.

Open questions
- none

Follow-up questions
- Should the stale incoming blocks relations from done tickets be cleaned up on the owner branch so live relation state matches the completed prerequisites?
- After this baseline is ratified, should story 06FBSC4HSXFJ5FM6GWECH2CTGG rely only on existing checked-in bundles or require a fresh multi-provider rerun when additional local connection strings are available?
- If a later ticket wants new measured evidence beyond the current baseline, should the first expansion be a DB2 completed benchmark bundle or a non-SQLite binary-vs-hex comparison bundle?

Risks
- If downstream docs cite skipped-placeholder rows as timing proof, the repository will overstate external-provider evidence that is only preserved as unavailable guidance.
- DB2 remains a non-timing baseline unless a reachable DB2 connection string produces a checked-in benchmark triplet, so its baseline can still be limited to skipped-placeholder, diagnostics-only, or smoke-only evidence.
- The live stale blocks relations from done tickets may confuse relation reports even though the current ticket record is not blocked.

Split recommendations
- No split recommended; the baseline evidence contract is already bounded and downstream publication work is separated into story 06FBSC4HSXFJ5FM6GWECH2CTGG.
- Any future work to generate new provider bundles or broaden into binary-vs-hex cross-provider evidence should be handled as follow-up tickets rather than expanding this refinement.

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