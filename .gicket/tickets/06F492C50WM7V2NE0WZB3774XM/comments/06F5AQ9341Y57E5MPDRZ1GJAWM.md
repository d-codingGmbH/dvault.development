[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff. The delivery contract is now source-backed, `## Open Questions` is `none`, and current repository evidence confirms the existing read-diagnostics/read-shape/public-API baseline the story extends.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket revision `06F5ANWVF3Q61NAFQQ5WZ3MYJ4` shows the persisted delivery contract for ticket `06F492C50WM7V2NE0WZB3774XM` has `## Open Questions` = `none` and scopes the work as additive extensions to existing read-shape diagnostics.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:452,482,505,527,541,551,570,791` already defines `DataVaultReadShapeColumnSet`, the satellite/PIT/bridge read-shape records, `DataVaultReadShapeDiagnostics`, `DataVaultDiagnosticsResult.ReadShape`, and `IDataVaultReadDiagnosticsService`; existing baseline fields remain `ExpectedIndexBaseline` at lines 490 and 514 and `ExpectedTraversalIndexBaseline` at line 536.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:71,80,120,139,150,161,170` already resolves `IDataVaultReadDiagnosticsService`, asserts `ReadShape` population for explicit and registry-backed latest/PIT/bridge requests, asserts `ExpectedIndexBaseline` and `ExpectedTraversalIndexBaseline`, and verifies support-bundle serialization omits request values.
- `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs` plus `docs/quality/api-surface-snapshots.md` confirm committed public-API snapshot coverage already exists for the core package, matching the contract's additive-compatibility requirement.
- `git diff --name-only f9577aafd250060cf9072cccbcc31ba428ead6d9..HEAD` returned no files, and `git show --name-status --stat --oneline HEAD -- .gicket/tickets/06F492C50WM7V2NE0WZB3774XM` shows HEAD `f9577aafd` is only a po-critic lease-claim commit touching `.gicket` metadata, so missing implementation on the branch is current-state evidence, not a PO blocker for this pre-development gate.
- `.gicket/tickets/06F492C50WM7V2NE0WZB3774XM/comments/06F5AN9VYPCVY7M79A2TSTB388.md` reports the latest PO refinement outcome as `po-refinement-ready`, and `06F5AN0VF564Z1FYW4D0H23PMW.md` records the updated source-backed contract that resolved the earlier inferred-API blocker.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No concrete acceptance example currently names a multi-active or driving-key satellite case proving `drivingKeyProjection` appears only when driving keys exist.
- No concrete acceptance example currently names a depth-bounded hierarchy bridge case proving `depthProjection` appears only when `MaximumDepth` is supplied.

Risky assumptions
- Approval assumes developers will follow the delivery contract rather than the stale `index hints` wording in the stored ticket title.

AC / test suggestions
- Add one acceptance/example tied to `DataVaultDiagnosticsTests` for a driving-key satellite so `drivingKeyProjection` is observable and deterministic.
- Add one acceptance/example for a depth-bounded hierarchy bridge so `depthProjection` behavior is pinned as clearly as `endpointProjection`.
- When the public members land, expect `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs` and the corresponding `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` baseline to prove additive API compatibility.

Implementation watchouts
- The contract is only safe if the new members stay additive on the existing `DataVaultDiagnosticsResult.ReadShape` and satellite/PIT/bridge read-shape records; do not reinterpret this story as a new diagnostics service or a new index-hint subsystem.
- Current branch history shows no implementation delta beyond ticket metadata, so dev handoff should treat this as a pre-development story rather than as closure evidence.

Non-blocking notes
- The prior po-critic blocker about inferred existing APIs is now resolved by direct source evidence in `DataVaultDiagnostics.cs` and `DataVaultDiagnosticsTests.cs`.
- The stored title still says `Add query-shape performance diagnostics and index hints`, but the persisted contract explicitly narrows scope to projected-column facts plus PIT lookup counts while preserving existing baseline index fields.

Split recommendations
- No delivery split is required for the implementation scope; if Product wants the stored title cleaned up, keep that as a separate ticket-admin follow-up rather than expanding this story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment