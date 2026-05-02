[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the root README quickstart scope is bounded, the required public API surface is directly present in source, the minimal example path is already test-anchored, and the persisted contract has no unresolved open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7R6MTJW1PYRN172MW34DM/description.md` contains `## Open Questions` followed by `- none`, so the persisted delivery contract has no unresolved open questions.
- `git log --oneline --decorate -n 6 ticket/06EXB7R6MTJW1PYRN172MW34DM-task-create-readme-quickstart-with-minimal-confi` shows HEAD `acbc4332` on the po-critic claim and earlier PO handoff commit `16f965d3`, which is consistent with this being a pre-dev review surface.
- `git diff --stat a0f03517..acbc4332` touches `.gicket/tickets/06EXB7R6MTJW1PYRN172MW34DM/...` and related ticket metadata only; it does not touch `README.md`, `src/`, or `tests/`.
- `README.md` currently contains only `Layout`, `Local Validation`, `Optional Local Postgres Integration Tests`, and `License`; `find docs -maxdepth 2 -type f` shows no separate quickstart document under `docs/`.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` targets `net10.0`, sets `PackageId` and `RootNamespace` to `DCoding.Data.DVault`, and packs `../../README.md` as the package readme, which supports using the root README as the canonical quickstart document.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs`, `src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs`, `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs`, and `src/DCoding.Data.DVault/DataVaultSaveService.cs` directly expose the public surfaces named in the contract: `AddDVault()`, `ApplyDataVaultMetadata(...)`, `DataVaultMetadataModel`, `DataVaultHubMetadata`, `DataVaultLinkMetadata`, `DataVaultSatelliteMetadata`, `IDataVaultSaveService`, and `DataVaultSaveRequest`.
- `tests/DCoding.Data.DVault.Tests/Modeling/DefaultNamingPolicyTests.cs` verifies the optionless `AddDVault` overload; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs` verifies `ApplyDataVaultMetadata`; `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` exercises the Customer/Order/CustomerOrder save flow and shared-type queries against `HubCustomer`, `HubOrder`, and `LinkCustomerOrder`.
- Relation files `.gicket/relations/MM/DM/06EXB7QYF1BB1REM7HQZ4WWVMM--06EXB7R6MTJW1PYRN172MW34DM--parentOf.json` and `.gicket/relations/DM/YC/06EXB7R6MTJW1PYRN172MW34DM--06EXB7REMY41DF7RE8J3N1RZYC--blocks.json` confirm the ticket is already bounded between a parent story and a sibling installation follow-up ticket.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- There is no current combined end-to-end quickstart sample in the repository; the existing proof is distributed across `tests/DCoding.Data.DVault.Tests/Modeling/DefaultNamingPolicyTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`.

Risky assumptions
- The ticket assumes prerequisite wording in `README.md` can stay to a brief already-referenced-library handoff and not drift into the installation/publication slice already split to ticket `06EXB7REMY41DF7RE8J3N1RZYC`.
- The ticket assumes the README author will include the correct namespace imports for modeling types from `src/DCoding.Data.DVault/Modeling/*.cs`; those metadata types are not all declared in the root `DCoding.Data.DVault` namespace.

AC / test suggestions
- Keep the existing acceptance criterion that each README snippet must either compile directly or be mirrored by tests; the current repository already provides direct anchors for service registration, metadata translation, and explicit save/query behavior.
- If the README merges those separately proven fragments into one contiguous snippet, require explicit proof for that merged snippet during dev/test, because the current evidence is distributed across multiple test files.

Implementation watchouts
- Do not imply a published NuGet package or detailed install path; the contract and `.gicket/relations/DM/YC/06EXB7R6MTJW1PYRN172MW34DM--06EXB7REMY41DF7RE8J3N1RZYC--blocks.json` already reserve that slice for ticket `06EXB7REMY41DF7RE8J3N1RZYC`.
- Stay on the optionless `AddDVault()` path from `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs`; `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs` also shows the default path is explicit-save-service based and does not use a SaveChanges interceptor.
- Use the proven Customer/Order/CustomerOrder shape and shared-type query pattern from `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` so the README does not imply a typed query API that does not exist.

Non-blocking notes
- The branch currently contains only PO and po-critic ticket metadata commits, which is consistent with handing the work to the dev role next.
- Because `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` packs the root README as the package readme, this ticket is correctly targeted at a user-visible surface rather than an internal doc only.

Split recommendations
- No split recommended; `.gicket/relations/MM/DM/06EXB7QYF1BB1REM7HQZ4WWVMM--06EXB7R6MTJW1PYRN172MW34DM--parentOf.json` and `.gicket/relations/DM/YC/06EXB7R6MTJW1PYRN172MW34DM--06EXB7REMY41DF7RE8J3N1RZYC--blocks.json` already bound the work as a README quickstart slice between a parent story and a sibling installation follow-up.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment