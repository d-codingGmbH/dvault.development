[gicket-bot] PO-critic review contract

Summary
- Approved for developer handoff: the persisted contract is closed, upstream registry prerequisites are already landed, and the repository already exposes the explicit save/read seams and registry lookup surface this refactor must preserve.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted contract in `.gicket/tickets/06F0MEBFTW8FY5T7PY5HJ5JXJ4/description.md` has `## Open Questions` = `none`, so there is no unresolved PO question gating handoff.
- `git rev-list --left-right --count HEAD...develop` returned `4 0`, and `git branch --contains c5c2e3155` plus `git branch --contains b2c3bf6c2` both include `ticket/06F0MEBFTW8FY5T7PY5HJ5JXJ4-task-refactor-save-and-read-services-to-consume`, so the current review branch already contains the blocker integrations from commits `c5c2e3155` and `b2c3bf6c2`.
- The authoritative metadata-source wiring already exists in source: `src/DCoding.Data.DVault/DataVaultOptions.cs` exposes `UseMetadataModel(...)` and `UseMetadataRegistry(...)`, and `src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs` exposes `UseDataVaultMetadata()` overloads for app-default and context-scoped registry selection.
- `src/DCoding.Data.DVault/DataVaultDbContextOptionsExtension.cs` already defines the missing-app-default diagnostic (`no app-level DataVaultMetadataRegistry is registered`) and resolves the app-default versus context-scoped authoritative registry.
- The explicit public APIs that this ticket must preserve are directly present in source: `src/DCoding.Data.DVault/DataVaultSaveService.cs` defines `IDataVaultSaveService.SaveAsync(DbContext, DataVaultSaveRequest/DataVaultBulkSaveRequest)` and operation types that currently require `DataVaultHubMetadata`, `DataVaultLinkMetadata`, and `DataVaultSatelliteMetadata`; `src/DCoding.Data.DVault/IDataVaultReadService.cs` plus `src/DCoding.Data.DVault/DataVaultLatestSatelliteReadRequest.cs` define the current explicit read path that takes `DataVaultSatelliteMetadata`.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs` already exposes exact-name lookup seams the refactor can consume: `TryGetHub`, `TryGetLink`, and parent-scoped `TryGetSatellite`.
- Existing tests already pin the two baselines this ticket must bridge: `tests/DCoding.Data.DVault.Tests/Integration/DataVaultMetadataRegistrationIntegrationTests.cs` covers app-default registry projection, context override, and source-conflict behavior, while `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` covers explicit save plus `IDataVaultReadService.ReadLatestSatelliteRowsAsync(...)` latest/as-of behavior.
- `README.md` already contains both the one-time registry registration section (`Register metadata once and opt in a DbContext`) and the current explicit save/read examples, so the ticket’s follow-up question about quickstart example ordering is documentation polish rather than a scope blocker.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No example currently spells out a registry-backed read against a satellite name that is only unique within a parent scope; that edge case matters because `DataVaultMetadataRegistry.TryGetSatellite(...)` is parent-scoped rather than globally keyed by satellite name.
- No example currently shows a registry-backed save flow that mixes hub, link, and satellite resolution in one ordinary caller path under a DbContext-level registry override.
- No example currently shows a registry-backed read for a link-parent satellite, even though the existing low-level read model supports parent hash keys for either hub or link parents.

Risky assumptions
- The contract leaves the additive public shape open (`overloads or companion adapters`); approval assumes Product is intentionally delegating that API-shape choice to implementation as long as explicit request APIs stay source-compatible and deterministic.

AC / test suggestions
- Add a regression test that a registry-backed save path resolves all required metadata before write orchestration starts and leaves `RowsWritten` at `0` when registry lookup fails.
- Add coverage for both app-default and DbContext-override authoritative sources on the save side and the read side, not just model projection.
- Add a test for parent-scoped satellite lookup where the same satellite logical name exists under different parents, so the registry-backed read path cannot accidentally assume global satellite-name uniqueness.
- Add explicit parity tests showing registry-backed wrappers/adapters produce the same saved-record ordering, validation messages, and read results as the existing explicit metadata path for equivalent inputs.

Implementation watchouts
- Resolve registry metadata once from the same authoritative source already chosen by `AddDVault(...)/UseDataVaultMetadata(...)`; do not add a second ambient lookup path.
- Keep registry-backed entry points thin and hand off to the existing explicit save/read pipeline after metadata resolution, or diagnostics and ordering can drift from the established low-level behavior.
- Do not flatten satellite lookup to global name matching; the existing registry surface is parent-scoped for satellites.
- Preserve the current low-level read result model (`DataVaultSatelliteReadRecord`) and the current explicit save contracts unchanged for advanced callers.

Non-blocking notes
- The stale `blocks` relation edges are worth leaving visible in the ticket history, but they are not a current refinement blocker because the related tickets are already persisted as `done`.
- `README.md` already shows the registry-registration baseline and still uses explicit metadata in the save/read examples, which matches the ticket’s documented follow-up question without requiring a PO decision now.
- `git log --oneline --decorate --graph --grep='06F0MEBFTW8FY5T7PY5HJ5JXJ4|06F0MEAXT99V0P115P0WEJD4P0|06F0MEB634X6CTBZ00W108G3FG' --all` shows only ticket handoff/lease commits on this branch after the dependency work, so there is no competing design artifact to reconcile before dev starts.

Split recommendations
- Keep the split as-is: this ticket stays focused on registry-backed consumption of existing save/read services, while typed helpers remain on `06F0MECFNF42NK9PND9DWVW9VW` and `06F0MECPFAVBFBNC5XMVDZRQ6M`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment