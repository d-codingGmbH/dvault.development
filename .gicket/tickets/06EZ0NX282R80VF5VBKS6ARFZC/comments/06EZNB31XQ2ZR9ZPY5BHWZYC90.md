[gicket-bot] PO-critic review contract

Summary
- Delivery contract is bounded, backed by direct source evidence for the existing optional-configuration and provider-fallback seams, and contains no unresolved `## Open Questions`; ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EZ0NX282R80VF5VBKS6ARFZC/description.md` contains a durable delivery contract with `PO Handoff` = `ready_for_po_critic` and `## Open Questions` = `none`.
- Comment `.gicket/tickets/06EZ0NX282R80VF5VBKS6ARFZC/comments/06EZN9N5XGH8D8MKDBT6ZGN6M0.md` says only bot claim/lease entries remain, and direct inspection of `.gicket/tickets/06EZ0NX282R80VF5VBKS6ARFZC/comments/*.md` found only bot claim/lease/refinement/handover content.
- `.gicket/relations/2R/ZC/06EZ0NWKC9ZME5BSCJFSQEQ02R--06EZ0NX282R80VF5VBKS6ARFZC--parentOf.json` proves the ticket is already a bounded child of story `06EZ0NWKC9ZME5BSCJFSQEQ02R`.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` and `src/DCoding.Data.DVault/DataVaultOptions.cs` already expose the optionless `AddDVault()` path plus optional category-specific overrides, matching the contract's default-inheriting configuration model.
- `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` calls `DataVaultProviderCapabilityProfileSelection.Select(modelBuilder)`, and `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs` falls back to `DataVaultProviderCapabilityProfiles.Sqlite` when no provider registration matches, which is the current model-translation baseline the ticket references.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` orders `IDataVaultProviderSaveStrategy` registrations by descending `Priority`, tries `CanSave`, and falls back to the core writer when none match; `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs` defines the public override contract the ticket cites as an existing registration/fallback pattern.
- `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs` and `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` call `DataVaultProviderCapabilityProfileSelection.Register(...)`, while `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs` only register save strategies, directly supporting the ticket's narrower provider-profile risk note.
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` already provide the fallback/selection and API-snapshot baselines the contract asks future work to extend.
- `git rev-parse --abbrev-ref HEAD && git rev-parse HEAD` returned branch `ticket/06EZ0NX282R80VF5VBKS6ARFZC-task-implement-provider-behavior-hook-surface-wi` at `427b2be0f7f517b3e5325796b73bbe5f628f5ebe`, and `git diff --name-only c78f0f56..427b2be0` showed only `.gicket/tickets/06EZ0NX282R80VF5VBKS6ARFZC/**` changes, so the branch history for this review is ticket-refinement-only.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A provider-override regression case on a package that has a save strategy but no visible provider-name capability-profile auto-registration yet (Postgres, SQL Server, or Oracle) is not spelled out in the contract text; cover that in implementation tests.

Risky assumptions
- Assuming every provider package already auto-registers provider capability profiles would contradict the current source baseline outside SQLite/MySQL.
- Assuming the new hook may also alter naming, hashing, record source, or timestamp behavior would violate both the ticket scope-out and the provider-behavior sections in `docs/plans/optional-advanced-configuration-hooks.md` and `docs/plans/deferred-data-vault-capabilities.md`.

AC / test suggestions
- Add one negative-path test where an override is registered but incompatible, and prove both model-translation profile selection and save-path fallback stay on the current baseline.
- If the hook introduces public surface for cross-package registration, extend `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs` and `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` in the same change.

Implementation watchouts
- Keep the ordinary `AddDVault()` path zero-configuration by following the existing `DataVaultOptions` optional-category pattern instead of requiring provider-specific startup config.
- Do not broaden provider-name capability-profile commitments beyond what the service-extension source currently proves: visible auto-registration exists for SQLite and MySQL, not for Postgres, SQL Server, or Oracle.

Non-blocking notes
- The current parent story `.gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R/description.md` is still broad legacy prose, but this child ticket's persisted contract is materially more concrete and bounded than the parent text.
- The follow-up question about whether the first public hook surface should be documented as stable or experimental remains useful product guidance, but it is not persisted as an open blocking question in the current contract.

Split recommendations
- No split recommended; the existing parent relation and current contract already keep this task bounded to one provider-behavior hook surface plus fallback/regression coverage.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment