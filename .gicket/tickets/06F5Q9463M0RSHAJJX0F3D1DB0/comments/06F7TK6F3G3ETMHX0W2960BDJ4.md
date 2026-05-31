[gicket-bot] PO-critic review contract

Summary
- Return to PO: the contract is close, but direct source evidence shows an existing public latest-satellite typed projection helper path is not explicitly anchored in scope or acceptance criteria, so latest/current/as-of tracing ownership is still ambiguous.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `git rev-parse --abbrev-ref HEAD` returned `ticket/06F5Q9463M0RSHAJJX0F3D1DB0-story-add-activity-tracing-for-save-and-read-ope`; `git show --name-only --format=oneline HEAD` and `git show --name-only --format=oneline dbd6d120b758` show the latest commits only touch `.gicket/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/...`, so the branch is still at metadata/handoff stage.
- `git show HEAD:.gicket/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/description.md` shows `## Open Questions` = `none` and PO handoff decision `ready_for_po_critic`.
- `git grep -n Task<DataVaultSaveResult> SaveAsync src/DCoding.Data.DVault/DataVaultSaveService.cs` returned the three public save overloads at lines 21, 33, and 45.
- `git grep` on `src/DCoding.Data.DVault/IDataVaultReadService.cs` returned only `ReadLatestSatelliteRowsAsync` at line 16 and `ReadPitRowsAsync` at line 28 as public read-interface members.
- `git show HEAD:src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs` shows a separate public `ReadLatestSatelliteAsync<TProjection>` helper that reads through `IDataVaultSatelliteProjectionReadService` or `DataVaultSatelliteReadPipeline`, not through `IDataVaultReadService.ReadLatestSatelliteRowsAsync(...)`; `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:953` and `:956` also record the registry and explicit public overloads.
- `git grep` on `src/DCoding.Data.DVault/DataVaultReadServiceCurrentSatelliteExtensions.cs` and `src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs` shows current/as-of and registry helpers delegate into both row and typed latest-satellite helper paths.
- `git grep` for `ActivitySource`, `ActivityListener`, and `System.Diagnostics.Activity` found matches only in `docs/architecture/dvault-v1-activity-tracing-contract.md`; it found no current tracing accessor in `src/DCoding.Data.DVault` or `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt`.
- `gicket-read-ticket-comments` returned the PO refinement contract comment and runtime handover comment; no newer returned comment reopens scope.

Blocking findings
- The delivery contract does not explicitly anchor the existing public latest-satellite typed projection helper surface in `src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs` (`ReadLatestSatelliteAsync<TProjection>`), even though it is part of the public API snapshot and is the path used by typed current/as-of and registry helpers.
- Because typed latest-satellite reads do not route through `IDataVaultReadService.ReadLatestSatelliteRowsAsync(...)`, the ticket still leaves ambiguous where the single allowed `dvault.read.latest_satellite` root span must be owned across row and projection paths, creating a real duplicate-span or missed-span risk.

Required PO actions
- Amend Scope In, Acceptance Criteria, and Implementation Notes to explicitly include `DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync<TProjection>` and the registry typed latest overload already recorded in `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:953` and `:956`.
- State which latest-satellite execution point owns the single `dvault.read.latest_satellite` root span for row and projection paths so `ReadCurrentSatelliteAsync(...)`, `ReadAsOfSatelliteAsync(...)`, and registry typed helpers cannot miss coverage or emit duplicate root spans.

Open issues ledger
- critic-item-1 [required-po-action] Amend Scope In, Acceptance Criteria, and Implementation Notes to explicitly include `DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync<TProjection>` and the registry typed latest overload already recorded in `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:953` and `:956`.
- critic-item-2 [required-po-action] State which latest-satellite execution point owns the single `dvault.read.latest_satellite` root span for row and projection paths so `ReadCurrentSatelliteAsync(...)`, `ReadAsOfSatelliteAsync(...)`, and registry typed helpers cannot miss coverage or emit duplicate root spans.
- critic-item-3 [blocking-finding] The delivery contract does not explicitly anchor the existing public latest-satellite typed projection helper surface in `src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs` (`ReadLatestSatelliteAsync<TProjection>`), even though it is part of the public API snapshot and is the path used by typed current/as-of and registry helpers.
- critic-item-4 [blocking-finding] Because typed latest-satellite reads do not route through `IDataVaultReadService.ReadLatestSatelliteRowsAsync(...)`, the ticket still leaves ambiguous where the single allowed `dvault.read.latest_satellite` root span must be owned across row and projection paths, creating a real duplicate-span or missed-span risk.

Missing examples / edge cases
- Listener-enabled typed current/as-of satellite executions that flow through `ReadCurrentSatelliteAsync(...)` / `ReadAsOfSatelliteAsync(...)` into `ReadLatestSatelliteAsync<TProjection>`.
- Listener-enabled registry typed latest/current/as-of executions that use the public registry latest helper surface recorded in the public API snapshot.
- Both internal latest-projection branches used by `ReadLatestSatelliteAsync<TProjection>`: `IDataVaultSatelliteProjectionReadService.ReadLatestSatelliteProjectionRowsAsync(...)` and `DataVaultSatelliteReadPipeline.ReadLatestProjectionRowsAsync(...)`.

Risky assumptions
- Assuming instrumentation on `IDataVaultReadService.ReadLatestSatelliteRowsAsync(...)` automatically covers all typed latest/current/as-of helper executions.
- Assuming developers can infer the projection-path span-ownership rule from current prose even though the explicit public helper surface is not named in the ticket.

AC / test suggestions
- Add an explicit AC or verification note for direct `DataVaultLatestSatelliteReadRequest` typed projection calls through `DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync<TProjection>`.
- Add duplicate-span tests proving `ReadCurrentSatelliteAsync(...)`, `ReadAsOfSatelliteAsync(...)`, and registry typed helpers still emit exactly one `dvault.read.latest_satellite` span.
- Keep bridge tests covering both `DefaultDataVaultReadService` and `DataVaultBridgeReadPipeline` branches, as already called out elsewhere in the contract.

Implementation watchouts
- Do not create a latest-satellite root span in both row and projection helper layers.
- Bridge tracing still needs both branches surfaced in `src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs`.
- Keep tag and event materialization behind listener and sampling checks so the no-listener baseline from `docs/architecture/dvault-v1-activity-tracing-contract.md` remains true.

Non-blocking notes
- The current branch history is metadata-only so far, which is expected at this pre-development gate.

Split recommendations
- No split recommended; keep this as one story after the latest-satellite typed projection path is explicitly folded into the contract.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment