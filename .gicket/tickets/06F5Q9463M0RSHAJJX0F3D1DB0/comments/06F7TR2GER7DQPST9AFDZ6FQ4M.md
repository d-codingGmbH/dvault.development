[gicket-bot] PO-critic review contract

Summary
- Persisted contract is now source-backed, resolves the earlier typed latest-satellite tracing ownership ambiguity, and is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/description.md:7-9 sets PO handoff to `ready_for_po_critic`, and :57-58 says `## Open Questions` = `none`.
- .gicket/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/comments/06F7TNQ6AF6FH5MJTBD4SQ1014.md:10-14 marks all four earlier PO-critic checklist items `answered`, including explicit typed latest-satellite coverage and terminal root-span ownership for row vs projection paths.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:21-48 exposes exactly three public `IDataVaultSaveService.SaveAsync(...)` overloads, and :73-120 shows registry save adapters delegate into the explicit service.
- src/DCoding.Data.DVault/IDataVaultReadService.cs:16-31 exposes only `ReadLatestSatelliteRowsAsync(...)` and `ReadPitRowsAsync(...)`, matching the contract's no-new-interface-member posture for tracing.
- src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs:48-93 defines public `ReadLatestSatelliteAsync<TProjection>(...)` and dispatches projection execution through `IDataVaultSatelliteProjectionReadService.ReadLatestSatelliteProjectionRowsAsync(...)` or `DataVaultSatelliteReadPipeline.ReadLatestProjectionRowsAsync(...)`.
- src/DCoding.Data.DVault/DataVaultReadServiceCurrentSatelliteExtensions.cs:75-125 and :198-251 plus src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs:115-137 show typed current/as-of and registry helpers delegate into the typed latest helper path instead of creating separate execution paths.
- src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:52-101 and :256-305 contains separate latest-satellite row and projection core paths; src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs:17-36 and :49-76 plus DefaultDataVaultReadService.cs:154-253 show bridge coverage must span both `DefaultDataVaultReadService` and direct `DataVaultBridgeReadPipeline` branches; src/DCoding.Data.DVault/DataVaultReadServicePitExtensions.cs:19-40 shows PIT typed projection delegates to `ReadPitRowsAsync(...)`.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:953 and :956 record the registry and explicit typed latest overloads as public API, and a repository grep for `ActivitySource`, `ActivityListener`, and `StartActivity(` over `src/DCoding.Data.DVault` plus that snapshot returned `NO_MATCHES`, consistent with the contract's internal-by-default tracing-holder stance.
- docs/architecture/dvault-v1-activity-tracing-contract.md:10-24 sets ActivitySource name `DCoding.Data.DVault` and listener/sampling behavior, while docs/releases/v0.16.0.md:22-53 is the telemetry compatibility baseline the ticket explicitly preserves.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Implementation still has to prove that the existing bounded telemetry and diagnostics vocabulary is sufficient for Activity tags and events across save, latest-satellite, PIT, and bridge paths without inventing new public API or unbounded data.
- The single-root-span rule for bridge reads depends on avoiding double emission across the `DefaultDataVaultReadService` branch and the direct `DataVaultBridgeReadPipeline` fallback branch.

AC / test suggestions
- Add listener-disabled tests for single, bulk, and chunked save, latest row read, latest typed projection read, PIT typed read, and both bridge branches to prove zero Activity creation when no listener is interested.
- Add listener-enabled tests asserting exactly one `dvault.read.latest_satellite` root span for `ReadCurrentSatelliteAsync(...)`, `ReadAsOfSatelliteAsync(...)`, and the registry typed latest, current, and as-of helpers.
- Add fault and cancellation coverage for provider-pipeline faults, projector-delegate faults, and cancellation before completion, asserting `ActivityStatusCode.Error` plus only bounded failure tags and events.

Implementation watchouts
- Keep latest-satellite root-span ownership only at the terminal row or projection execution boundary; do not add wrapper root spans in `DataVaultReadServiceCurrentSatelliteExtensions.cs` or `DataVaultReadServiceRegistryExtensions.cs`.
- Bridge tracing must cover both branches in `DataVaultReadServiceBridgeExtensions.cs` without double-emitting for raw-row and typed-projection helpers.
- Any new ActivitySource holder or helper should default internal; if a public accessor is added, `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` must change intentionally in the same work.

Non-blocking notes
- Current ticket workflow state is still pre-critic: .gicket/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/ticket.json:7-21 still shows `todo`, `critic-needed`, `blocked/dev`, `blocked/test`, and no assignees.
- The blocker relation to 06F5Q93YXHSKABD2SABWY85S78 is historical context now that .gicket/tickets/06F5Q93YXHSKABD2SABWY85S78/ticket.json:7 is `done`.
- Earlier PO-critic `return_to_po` comments are superseded by the later PO refinement comment and refreshed delivery contract committed at `b94f1146b2b4`.

Split recommendations
- No split recommended; the refreshed contract already keeps PIT and bridge maintenance tracing in 06F5Q94D0JDMMWDXSRGWX1E4F0, while this ticket remains a coherent save/read tracing story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment