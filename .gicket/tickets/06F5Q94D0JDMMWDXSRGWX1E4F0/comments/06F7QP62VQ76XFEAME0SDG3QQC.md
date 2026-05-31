[gicket-bot] PO-critic review contract

Summary
- Delivery contract is concrete, repository-grounded, and ready for developer handoff; I found no PO-level blockers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q94D0JDMMWDXSRGWX1E4F0/description.md` contains the authoritative Delivery Contract with `## Open Questions` = `- none`, explicit scope in/out, 6 acceptance criteria, 4 definition-of-done items, and 4 implementation notes.
- Relation events are present in-repo: `.gicket/tickets/06F5Q93YXHSKABD2SABWY85S78/events/06F5Q99WPD7BKN6HH65DVTBR08.json` adds `06F5Q93YXHSKABD2SABWY85S78 --blocks--> 06F5Q94D0JDMMWDXSRGWX1E4F0`; `.gicket/tickets/06F5Q94D0JDMMWDXSRGWX1E4F0/events/06F5Q9A4Z0Y2JX6Q9NE4473DX4.json` adds `06F5Q94D0JDMMWDXSRGWX1E4F0 --blocks--> 06F5Q94SQ086B2DZ1AKFDXGV94`; `.gicket/tickets/06F5Q93R4633D41Z21WQW3SVGR/events/06F5Q974D24AWNCYEVXZ1SDPB0.json` adds the epic `parentOf` relation.
- `src/DCoding.Data.DVault/DataVaultPitMaintenanceService.cs` and `src/DCoding.Data.DVault/IDataVaultBridgeMaintenanceService.cs` expose the four exact maintenance entry points named by the story: `RebuildAsync`, `MaintainParentsAsync`, `RebuildBridgeAsync`, and `MaintainBridgeAsync`.
- `src/DCoding.Data.DVault/DataVaultPitMaintenanceResult.cs` and `src/DCoding.Data.DVault/DataVaultBridgeMaintenanceResult.cs` expose the bounded count/no-op data the ticket relies on: `ParentHashKeyCount`, `RowsDeleted`, `RowsWritten`, `IsNoOp`, `RowsInserted`, `RowsUpdated`, `RowsDeleted`, and `RowsUnchanged`.
- Repository tests already anchor the explicit no-op/count semantics the contract references: `tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceServiceTests.cs` has `EmptyParentMaintenanceRequestIsNoOpWithoutModelValidation`, and `tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs` asserts an unchanged incremental bridge maintenance result with `RowsInserted=0`, `RowsUpdated=0`, and `RowsUnchanged=3`.
- `git -C /mnt/c/Projects/DVault diff --name-only 90a40e93b74be3c3a554b99ec0b97ebeb67d595c..HEAD` returned no paths, and `git -C /mnt/c/Projects/DVault show --stat --summary --format=fuller HEAD` shows HEAD `90a40e93b74be3c3a554b99ec0b97ebeb67d595c` is only the PO-critic lease-claim metadata commit.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract requires fault and cancellation tracing coverage, but unlike the no-op cases it does not point to a repo-proven PIT/bridge-specific failure seed; that is a developer test-design gap to handle, not a PO refinement blocker.
- The contract mentions the `Activity.IsAllDataRequested=false` minimal-data path in implementation notes, but it does not name a concrete existing test harness for that sampling mode.

Risky assumptions
- The authoritative Delivery Contract is assumed to override the legacy draft's conflicting lowercase examples for `dvault.read_model.kind`; the contract doc requires exact bounded values `Pit` and `Bridge`.
- The Risks section still says the story is dependency-bound by ticket `06F5Q93YXHSKABD2SABWY85S78`, but current persisted state shows `.gicket/tickets/06F5Q93YXHSKABD2SABWY85S78/ticket.json` is `done` and this ticket's `.gicket/tickets/06F5Q94D0JDMMWDXSRGWX1E4F0/ticket.json` has `isBlocked:false`.

AC / test suggestions
- Add explicit listener tests for the two repository-grounded no-op paths: PIT `MaintainParentsAsync([])` and bridge `MaintainBridgeAsync(...)` with zero change counts, and assert `dvault.maintenance.noop` is only emitted when an Activity exists.
- For each of the four span names, assert exact name, `ActivityKind.Internal`, `dvault.operation` == span name, exact maintenance/read-model vocabulary, and affected-row math derived from existing result fields.
- Include redaction assertions that prove tags/events/status omit metadata names, table names, hash keys, payload values, SQL/provider text, and exception messages.

Implementation watchouts
- Use the exact contract vocabulary/casing from `docs/architecture/dvault-v1-activity-tracing-contract.md`: `DCoding.Data.DVault`, `ActivityKind.Internal`, `Pit|Bridge`, `PitRebuild|PitMaintainParents|BridgeRebuild|BridgeMaintainIncremental`, `Full|Parents|Incremental`, and `success|fault|canceled`.
- Keep the listener-driven fast path intact so no-listener execution does not allocate meaningful tag/event payloads or alter observable maintenance behavior.
- Do not invent maintenance semantics beyond existing request/result data; bridge no-op detection must stay grounded in current row-count results, and PIT no-op detection must stay grounded in the existing empty-parent request behavior.

Non-blocking notes
- This branch is still a pre-development review surface; the current diff from scratch source is empty and HEAD only contains PO-critic lease metadata.
- The downstream documentation task `06F5Q94SQ086B2DZ1AKFDXGV94` already exists as a separate blocked ticket, so no further split is needed to preserve release sequencing.

Split recommendations
- No split recommended; the repository already separates the upstream tracing contract ticket `06F5Q93YXHSKABD2SABWY85S78`, this maintenance implementation story `06F5Q94D0JDMMWDXSRGWX1E4F0`, and the downstream docs task `06F5Q94SQ086B2DZ1AKFDXGV94`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment