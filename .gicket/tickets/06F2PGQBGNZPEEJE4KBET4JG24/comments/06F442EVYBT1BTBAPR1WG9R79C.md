[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract is bounded, open questions are resolved, and the required save/read surfaces are directly evidenced in the repository.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGQBGNZPEEJE4KBET4JG24/description.md:7-15 and :53-54 records PO handoff `ready_for_po_critic`, reuses the existing diagnostics vocabulary, keeps telemetry additive/opt-in, and sets `## Open Questions` to `- none`.
- .gicket/tickets/06F2PGQ6T5TGNWCBQBX3700D84/ticket.json shows upstream story `Story: Explain save and read strategy decisions` is `done`; .gicket/tickets/06F2PGQBGNZPEEJE4KBET4JG24/events/06F2PH086S1BWYNHDE7TNQ7XSR.json shows this story blocks downstream docs ticket `06F2PGQQJB5FJGDB16M2G7CPCM`, whose ticket.json is still `todo`.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-31 and README.md:37-57 show the default `AddDVault()` path registers save/read/maintenance services without any telemetry registration, matching the additive opt-in requirement.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:12-35 defines explicit single and bulk `IDataVaultSaveService.SaveAsync(...)` entry points. src/DCoding.Data.DVault/IDataVaultReadService.cs:8-31, DataVaultReadServiceCurrentSatelliteExtensions.cs:19-33 and :167-250, DataVaultReadServicePitExtensions.cs:19-40, DataVaultReadServiceTypedProjectionExtensions.cs:48-93, and DataVaultReadServiceBridgeExtensions.cs:17-76 show the latest/current/as-of, PIT, typed projection, and bridge helper surfaces the telemetry contract must cover.
- src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs:85-93 and DataVaultReadServiceBridgeExtensions.cs:26-36,60-68 confirm some public read helpers can bypass a simple `IDataVaultReadService` decorator and fall directly to pipeline/default-service paths.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:34-54 and :109-129 defines the finite `DataVaultSaveStrategyDiagnosticsStatus`, `DataVaultSaveStrategyFallbackCauseKind`, `DataVaultReadStrategyDiagnosticsStatus`, and `DataVaultReadStrategyFallbackCauseKind` enums the contract tells developers to reuse.
- A search for `System.Diagnostics.Metrics`, `Meter`, `ActivitySource`, `DiagnosticSource`, and `Telemetry` under `src/DCoding.Data.DVault` returned no matches, supporting the contract statement that no telemetry API/package surface exists yet.
- `git show --name-only 91a4f5b14`, `a81d1a9a7`, and `65e0110ff` listed only `.gicket/tickets/06F2PGQBGNZPEEJE4KBET4JG24/...` metadata/comment/event files, so this branch is still at pre-development handoff state rather than implementation state.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add one explicit acceptance-test/example path for typed latest/as-of projections when the read service is not `IDataVaultSatelliteProjectionReadService`, because `DataVaultReadServiceTypedProjectionExtensions.cs:85-93` falls straight to `DataVaultSatelliteReadPipeline`.
- Add one explicit acceptance-test/example path for non-`DefaultDataVaultReadService` bridge helpers, because `DataVaultReadServiceBridgeExtensions.cs:26-36` and :60-68 bypass a simple decorator.
- Add one explicit telemetry example for PIT typed projections through `DataVaultReadServicePitExtensions.ReadPitAsync(...)` so success, failure, and row-count expectations are clear for the PIT helper path.

Risky assumptions
- Do not infer a registry-backed PIT helper from the wording alone. `DataVaultReadServiceRegistryExtensions.cs` contains registry-backed latest-satellite and bridge adapters, but repository search found no `DataVaultRegistry*Pit*` read request/helper surface in `src/DCoding.Data.DVault`.
- Do not assume release-note prose is part of this story's done state. `docs/releases/v0.16.0.md` is currently missing, and the contract explicitly leaves the broader v0.16.0 operational write-up to downstream ticket `06F2PGQQJB5FJGDB16M2G7CPCM`.

AC / test suggestions
- Keep the exactly-once telemetry AC tied to representative helper paths: registry-backed current/as-of satellite helpers, typed latest/as-of helpers, PIT typed helpers, and bridge typed helpers.
- Lock low-cardinality expectations in tests by asserting only finite dimensions: success/failure, request family, strategy status, fallback-cause kind, provider name, selected strategy name when finite, and bounded counts/durations.
- Add at least one save bulk case with mixed hub/link/satellite operations so request-count, operation-count, saved-record-count, and `RowsWritten` expectations are independently visible.

Implementation watchouts
- Keep telemetry inside the library read/save execution path rather than only around `IDataVaultSaveService`/`IDataVaultReadService`, or typed latest/as-of and bridge helpers will be under-instrumented.
- Reuse the existing diagnostics enums directly or by exact mirrored names; re-deriving fallback labels separately risks drift from `DataVaultDiagnostics.cs`.
- Preserve the default `AddDVault()` behavior and keep maintenance-service telemetry out of scope for this story.

Non-blocking notes
- The story is already wired as a prerequisite for the downstream docs task: `.gicket/tickets/06F2PGQBGNZPEEJE4KBET4JG24/events/06F2PH086S1BWYNHDE7TNQ7XSR.json` adds the `blocks` relation to `06F2PGQQJB5FJGDB16M2G7CPCM`.

Split recommendations
- No split needed. The current contract is already bounded to explicit save/read telemetry and leaves maintenance-service telemetry, support-bundle export, and coordinated v0.16.0 documentation wrap-up to separate follow-up work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment