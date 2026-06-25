<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around one authoritative minimal SQLite onboarding path. Repository evidence already supports binary-first AddDVault/AddDVaultSqlite registration in README/getting-started, while examples/README still carries stale 8.45.0/10.45.0 package lines and the current SQLite quickstart hides the save/read flow behind shared helper orchestration.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The current package-line baseline in this repository is 8.47.0 for net8.0 / EF Core 8 and 10.47.0 for net10.0 / EF Core 10; the minimal getting-started surfaces should align to those versions and should not show a consumer-facing 0.47.0 package version.
- The minimal mainline is SQLite-first and binary-first for new projects: use AddDVault(options => options.UseBinaryFirstProfile()), AddDVaultSqlite(), and ordinary UseSqlite(...), while stating that existing HexString storage is not auto-migrated.
- No hidden orchestration means the surfaced mainline must let a reader see registration, model or metadata declaration, schema creation/provisioning, one explicit save call, and one explicit read call without needing to inspect QuickstartHistoryFlow or similar shared helper layers.
- SaveChanges interception, background jobs, PIT/bridge flows, privacy, and observability remain secondary or advanced surfaces and should not appear as prerequisites in the minimal path.

### Scope In
- Refresh the primary README/getting-started/SQLite-example onboarding path for a brand-new SQLite DVault project.
- Align minimal-path package installation guidance to the current 8.47.0 and 10.47.0 coordinated package lines, including the SQLite provider package.
- Show the binary-first SQLite service-registration path and one visible model or metadata declaration path that fits the minimal tutorial.
- Show explicit IDataVaultSaveService and IDataVaultReadService usage in the surfaced path, with caller-owned load timestamp and record source still visible.
- Keep the SQLite path runnable or directly traceable end-to-end without hidden helper orchestration.

### Scope Out
- Do not broaden the mainline to PostgreSQL, SQL Server, MySQL, Oracle, DB2, privacy, PIT, bridge, telemetry, tracing, drift tooling, or production rollout checklists.
- Do not make SaveChanges interceptors, background maintenance, typed generator output, or diagnostics deep dives prerequisites for first-use success.
- Do not redesign the richer metadata-first/PostgreSQL quickstart evidence except where a small relink or wording change is needed to keep the minimal SQLite path consistent.

## Acceptance Criteria
- The primary minimal path documents the current coordinated consumer package lines: 8.47.0 for net8.0 / EF Core 8 and 10.47.0 for net10.0 / EF Core 10, and the minimal-path surfaces no longer show stale 8.45.0 / 10.45.0 or a consumer-facing 0.47.0 package version.
- The setup path visibly registers AddDVault(...) with UseBinaryFirstProfile(), AddDVaultSqlite(), and the application's normal UseSqlite(...) DbContext configuration.
- The mainline visibly shows a bounded schema or metadata declaration plus schema creation/provisioning appropriate for the quickstart, without forcing the reader through shared helper indirection.
- The mainline visibly shows at least one explicit IDataVaultSaveService call and at least one explicit IDataVaultReadService latest/current read call over the example data; the flow does not rely on implicit SaveChanges DVault writes.
- The mainline states that binary-first is the recommended new-project storage profile but does not auto-migrate existing HexString-compatible databases.
- A reader can follow the SQLite mainline from package install to first save and first read without needing PostgreSQL, external infrastructure, PIT/bridge setup, privacy setup, or observability setup.

## Definition of Done
- README, docs/getting-started.md, and any surfaced SQLite quickstart/example text referenced by that mainline are internally consistent about package versions, provider registration, and binary-first guidance.
- Any SQLite example or sample code promoted as the minimal path shows the registration, save, and read flow directly enough that the reader does not need to inspect QuickstartHistoryFlow or another shared helper to understand it.
- Stale 8.45.0 / 10.45.0 package guidance is removed from the minimal-path example/docs surfaces touched by this ticket.
- The minimal SQLite path remains runnable or demonstrably valid within the repository's existing example/build conventions.
- Non-minimal advanced surfaces remain secondary and do not contradict the refreshed mainline.

## Implementation Notes
- Repository evidence already establishes the intended startup baseline in README.md and docs/getting-started.md: AddDVault(...) plus UseBinaryFirstProfile() plus AddDVaultSqlite().
- The main inconsistency is examples/README.md, which still documents 8.45.0 / 10.45.0 package lines while the repo-wide baseline is 8.47.0 / 10.47.0.
- The current SQLite example program delegates the substantive flow to examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs. That shared helper is useful richer example evidence, but it is not suitable as the only visible minimal getting-started path because the save/read orchestration is hidden from first-time readers.
- Keep one authoritative minimal SQLite mainline. If fuller metadata-first or PostgreSQL quickstarts remain, label or position them as secondary/runnable examples rather than the shortest onboarding path.
- Reuse existing explicit-service architecture boundaries: IDataVaultSaveService as the default write boundary and IDataVaultReadService latest/current read helpers for the first read proof; do not introduce SaveChanges-time automation as part of the tutorial.

## Open Questions
- none

## Follow-Up Questions
- After the minimal SQLite mainline lands, should the richer metadata-first/PostgreSQL quickstarts be explicitly labeled as advanced or extended examples to reduce onboarding ambiguity?
- If the team wants both Code-First and metadata-first first-run tutorials, should that be a separate follow-up instead of expanding this minimal SQLite ticket?

## Risks
- If the shared quickstart helper or metadata-first example structure is heavily rewritten instead of merely demoted or relinked, older release-note and documentation references to QuickstartHistoryFlow may need follow-up cleanup.
- If multiple onboarding surfaces remain equally prominent after the refresh, users may still be unsure which path is the intended first-run default.
- A very compact tutorial can drift from the runnable example and package-version baseline unless README, getting-started, and examples guidance are updated together on future release-line bumps.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Refresh the shortest documented path for a new binary-first SQLite DVault project. Acceptance: setup uses aligned package lines, AddDVault provider registration, binary-first profile, explicit save/read service calls, and no hidden orchestration.