[gicket-bot] PO refinement contract

Summary
- Refined the ticket around one authoritative minimal SQLite onboarding path. Repository evidence already supports binary-first AddDVault/AddDVaultSqlite registration in README/getting-started, while examples/README still carries stale 8.45.0/10.45.0 package lines and the current SQLite quickstart hides the save/read flow behind shared helper orchestration.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current package-line baseline in this repository is 8.47.0 for net8.0 / EF Core 8 and 10.47.0 for net10.0 / EF Core 10; the minimal getting-started surfaces should align to those versions and should not show a consumer-facing 0.47.0 package version.
- The minimal mainline is SQLite-first and binary-first for new projects: use AddDVault(options => options.UseBinaryFirstProfile()), AddDVaultSqlite(), and ordinary UseSqlite(...), while stating that existing HexString storage is not auto-migrated.
- No hidden orchestration means the surfaced mainline must let a reader see registration, model or metadata declaration, schema creation/provisioning, one explicit save call, and one explicit read call without needing to inspect QuickstartHistoryFlow or similar shared helper layers.
- SaveChanges interception, background jobs, PIT/bridge flows, privacy, and observability remain secondary or advanced surfaces and should not appear as prerequisites in the minimal path.

Scope In
- Refresh the primary README/getting-started/SQLite-example onboarding path for a brand-new SQLite DVault project.
- Align minimal-path package installation guidance to the current 8.47.0 and 10.47.0 coordinated package lines, including the SQLite provider package.
- Show the binary-first SQLite service-registration path and one visible model or metadata declaration path that fits the minimal tutorial.
- Show explicit IDataVaultSaveService and IDataVaultReadService usage in the surfaced path, with caller-owned load timestamp and record source still visible.
- Keep the SQLite path runnable or directly traceable end-to-end without hidden helper orchestration.

Scope Out
- Do not broaden the mainline to PostgreSQL, SQL Server, MySQL, Oracle, DB2, privacy, PIT, bridge, telemetry, tracing, drift tooling, or production rollout checklists.
- Do not make SaveChanges interceptors, background maintenance, typed generator output, or diagnostics deep dives prerequisites for first-use success.
- Do not redesign the richer metadata-first/PostgreSQL quickstart evidence except where a small relink or wording change is needed to keep the minimal SQLite path consistent.

Open questions
- none

Follow-up questions
- After the minimal SQLite mainline lands, should the richer metadata-first/PostgreSQL quickstarts be explicitly labeled as advanced or extended examples to reduce onboarding ambiguity?
- If the team wants both Code-First and metadata-first first-run tutorials, should that be a separate follow-up instead of expanding this minimal SQLite ticket?

Risks
- If the shared quickstart helper or metadata-first example structure is heavily rewritten instead of merely demoted or relinked, older release-note and documentation references to QuickstartHistoryFlow may need follow-up cleanup.
- If multiple onboarding surfaces remain equally prominent after the refresh, users may still be unsure which path is the intended first-run default.
- A very compact tutorial can drift from the runnable example and package-version baseline unless README, getting-started, and examples guidance are updated together on future release-line bumps.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment