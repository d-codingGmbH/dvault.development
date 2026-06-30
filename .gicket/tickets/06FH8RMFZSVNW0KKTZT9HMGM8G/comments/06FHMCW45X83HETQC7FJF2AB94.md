[gicket-bot] PO-critic review contract

Summary
- Approved for developer handoff: the persisted contract is specific, has no open questions, and is anchored to an existing public SQL Server provider-owned selection seam plus existing fail-closed and redaction-safe diagnostics surfaces.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FH8RMFZSVNW0KKTZT9HMGM8G/description.md contains the current delivery contract, marks PO handoff `ready_for_po_critic`, and shows `## Open Questions` = `none` while spelling out scope, acceptance criteria, DoD, and `DVAULT_TEST_*` gating.
- .gicket/tickets/06FH8RMFZSVNW0KKTZT9HMGM8G/comments/06FHMA9V8FEQ2KPZJBNW0YKCQ8.md records the manual PO refinement and handoff decision `ready_for_po_critic` with the bounded SQL Server Always Encrypted proof path.
- src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs exposes the provider-owned public seam `AddDVaultSqlServerAlwaysEncryptedSelection(IServiceCollection, string, string[])`, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.SqlServer.approved.txt snapshots that API.
- src/DCoding.Data.DVault.SqlServer/SqlServerAlwaysEncryptedDataVaultProviderNativeCryptoSelectionProvider.cs and src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs already encode fail-closed selection statuses and emit `provider-native-crypto-selection-unavailable` at `privacy/provider-native-crypto/<alias>` for rejected requests.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs already anchors the reviewed-capability path, redaction checks (`Assert.DoesNotContain("Password=", ...)` and support-bundle `Assert.DoesNotContain("Data Source", ...)`), and missing-prerequisite fail-closed behavior.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultEncryptedPayloadValueConverterTests.cs already covers the caller-owned custom encrypted-payload converter failing closed when key-provider prerequisites are missing or decline conversion, matching the ticket's custom-path-preservation requirement.
- tests/DCoding.Data.DVault.Tests/Integration/SqlServerIntegrationTestConfiguration.cs gates live SQL Server integration behind `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, matching the contract's optional live-provider requirement.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The existing SQL Server provider-owned seam is sufficient for proof/fallback coverage; if real runtime execution needs wider provider-specific design, the contract already assumes a follow-up ticket instead of widening this one.
- Optional live SQL Server coverage stays additive; if developers cannot prove a runtime path without broadening ownership boundaries, unit/diagnostics proof is still the minimum valid outcome for this ticket.

AC / test suggestions
- Keep one explicit test/evidence lane for each fail-closed reason already visible in source: missing prerequisite proof, incompatible provider profile, unsupported capability, and unavailable/defaulted capability fact.
- Use the existing redaction anchors in `DataVaultDiagnosticsTests` and the support-bundle export assertions so ticket handoff evidence can name concrete issue codes and privacy paths without exposing secrets.
- If live SQL Server proof is added, tie its skip behavior to `SqlServerIntegrationTestConfiguration.MissingConfigurationSkipMessage` and `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` so no-environment runs stay deterministic.

Implementation watchouts
- Do not let dev widen this ticket into shared provider-name dispatch or shared native runtime behavior; `SqlServerAlwaysEncryptedDataVaultProviderNativeCryptoSelectionProvider` explicitly says the selection remains owned by the SQL Server provider package.
- Do not accept any silent downgrade from requested native selection to plaintext or implicit custom conversion; current diagnostics code turns rejected selections into hard errors and the ticket should preserve that posture.
- Keep the shared privacy package provider-neutral; the ticket contract and existing diagnostics boundary both assume no database capability probing by default.

Non-blocking notes
- The ticket already has a direct manual PO refinement comment and a resolved PO runtime escalation in `.gicket/tickets/06FH8RMFZSVNW0KKTZT9HMGM8G/comments`, so this review is against the corrected persisted contract, not the earlier failed PO run.
- No repository evidence suggests the acceptance criteria depend on a missing public API; the required SQL Server seam and custom converter path are already present in source.

Split recommendations
- No immediate split is required for developer handoff. If implementation uncovers a need for real SQL Server Always Encrypted runtime execution beyond proof/fallback diagnostics, use the ticket's existing follow-up candidate instead of widening this task.
- Keep non-SQL Server provider-native proof work on separate follow-up tickets, as already listed in the delivery contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment