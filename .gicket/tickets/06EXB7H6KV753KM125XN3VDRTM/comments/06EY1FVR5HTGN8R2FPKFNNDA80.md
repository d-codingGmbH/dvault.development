[gicket-bot] PO-critic review contract

Summary
- Persisted contract is internally consistent with the current repo state and dependency chain; this ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7H6KV753KM125XN3VDRTM/description.md contains `## Open Questions` with `- none` and a persisted `PO Handoff` decision of `ready_for_po_critic`.
- `git log --oneline --decorate -n 10` on `ticket/06EXB7H6KV753KM125XN3VDRTM-task-design-savechanges-integration-or-explicit` shows HEAD `1a68e3cb` at the PO-critic claim and prior ticket handoff commit `c0a21009` for `06EXB7H6KV753KM125XN3VDRTM`.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs exposes `AddDVault()` and currently registers only convention/hash services (`DefaultNamingPolicy`, `DataVaultConventions`, `IStableHashService`, `IStableHashNormalizer`), matching the ticket claim that the save boundary still needs to be introduced through the existing explicit DI pattern.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs exposes `UseDataVault()` and `ApplyDataVaultMetadata()`, confirming the explicit repository surface the ticket references as precedent.
- Repository search `rg -ni "savechanges|interceptor" src tests docs` returned no matches, supporting the clarification that no SaveChanges interceptor surface exists on this branch today.
- tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj references `Microsoft.EntityFrameworkCore.Sqlite`; tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs provides the current SQLite integration harness; DVault.slnx includes the integration, shared, and unit test projects.
- docs/architecture/mvp-data-vault-concepts.md requires load timestamp and record source on hub/link/satellite rows, docs/plans/optional-advanced-configuration-hooks.md treats record-source and timestamp behavior as boundary-level concerns, and src/DCoding.Data.DVault/IStableHashService.cs confirms the existing stable-hash abstraction the ticket must reuse.
- .gicket/tickets/06EXB7GPRGEJHKFMJ8MVAVF8ZG/ticket.json shows prerequisite ticket `06EXB7GPRGEJHKFMJ8MVAVF8ZG` is `done`; .gicket/relations/ZG/TM/06EXB7GPRGEJHKFMJ8MVAVF8ZG--06EXB7H6KV753KM125XN3VDRTM--blocks.json shows it blocks this ticket; .gicket/relations/TM/ZC/06EXB7H6KV753KM125XN3VDRTM--06EXB7HEJY18HEB5A5MVTN5KZC--blocks.json shows this ticket blocks downstream `06EXB7HEJY18HEB5A5MVTN5KZC`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- An explicit example is not yet shown for one service call that supplies both `record source` and `load timestamp` and then asserts those exact persisted hub/link values.
- An explicit example is not yet shown for caller override of the new save service via ordinary DI registration before `AddDVault()`, analogous to the existing stable-hash override tests.
- An explicit negative example is not yet shown for the default write path working without any SaveChanges interceptor registration or interception hook.

Risky assumptions
- The ticket assumes one public save boundary can remain stable when later satellite and idempotent hub/link work is added through follow-up tickets.
- The ticket assumes the current `TryAddSingleton`-style override behavior in `AddDVault()` is the intended precedent for the new save-service registration shape.
- The ticket assumes a SQLite-backed hub/link proof is sufficient to validate a provider-neutral public write contract before any second provider exists.

AC / test suggestions
- Add one unit test that `AddDVault()` resolves the default save service and preserves a caller override registered before `AddDVault()`.
- Add one SQLite integration test that persists at least one hub row and one link row and asserts `LoadTimestamp` and `RecordSource` values exactly.
- Add one negative test that proves the default v1 write path does not require any SaveChanges interceptor registration.

Implementation watchouts
- Keep write intent in the explicit service boundary; do not reintroduce hidden ChangeTracker or SaveChanges orchestration behind the public API.
- Consume hashing only through `IStableHashService`; do not inline or duplicate hash generation inside the persistence path.
- Keep the request/operation shape extensible enough for later satellite work so the package does not need a second public write entry point.

Non-blocking notes
- Current branch history for this ticket is workflow and handoff only; no developer implementation has started on this ticket branch yet.

Split recommendations
- Keep idempotent hub/link reuse behavior in ticket `06EXB7HEJY18HEB5A5MVTN5KZC` instead of widening this ticket.
- If SaveChanges convenience is still desired after the explicit service lands, schedule it as a separate follow-up ticket.
- Keep provider-specific write optimizations or non-SQLite provider implementations in separate follow-up tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment