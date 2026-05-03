[gicket-bot] PO-critic review contract

Summary
- Refined contract is now ready for developer handoff: it names the Unit csproj as the only selectable proof, resolves the earlier runner-filter ambiguity, and is grounded in existing repo paths, APIs, comments, and ticket relations.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB80FPE3REH11RQ1YR6BW1G/description.md contains `## Open Questions` with `- none` and its Acceptance Criteria now name `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj` as the only required selectable fast-test proof.
- Previous blocker comment `.gicket/tickets/06EXB80FPE3REH11RQ1YR6BW1G/comments/06EYW2B349YYQYJ7HCESJ0ZYJM.md` returned the ticket to PO for missing runner-selectable proof; latest PO refinement comment `.gicket/tickets/06EXB80FPE3REH11RQ1YR6BW1G/comments/06EYW75CTFHC4S7QGQQPTZCEE8.md` marks `critic-item-1` through `critic-item-4` as `answered` and explicitly removes the dependency on undocumented Trait/Category filtering under `xunit.v3.mtp-v1` / Microsoft Testing Platform.
- `git -C /mnt/c/Projects/DVault show --stat --oneline 3dcc200d82dd --` shows the handoff commit changed only `.gicket/tickets/06EXB80FPE3REH11RQ1YR6BW1G/*` files, so the current branch state is ticket-refinement-only rather than code churn.
- `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj` sets `UseMicrosoftTestingPlatformRunner=true`, references `xunit.v3.mtp-v1`, links `../Modeling/*.cs`, and does not reference `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj`; the integration surface remains isolated in its own csproj.
- `tests/DCoding.Data.DVault.Tests/Unit/ConventionFirstEntryPointCoverageTests.cs` already bridges `DefaultNamingPolicyTests.Run()` and `NamingPolicyTests.Run()`, while `tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs` still exposes a standalone `Main()` harness, matching the contract's explicit bridge task.
- `src/DCoding.Data.DVault/Modeling/DataVaultModelBuilderExtensions.cs` exposes `UseDataVault(this DataVaultModelBuilder)`; `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` exposes `UseDataVault(this ModelBuilder)` and `ApplyDataVaultMetadata(this ModelBuilder, DataVaultMetadataModel)`; `src/DCoding.Data.DVault/TechnicalMetadataColumnContract.cs` and `TechnicalMetadataColumnRole.cs` expose the metadata-contract types named in scope.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` registers `IStableHashService`, `IStableHashNormalizer`, and `IDataVaultSaveService`; `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs` adds `IDataVaultProviderSaveStrategy`; the Postgres/SqlServer/Oracle/MySql service-collection extensions only call `AddDVault()`, which matches the contract's finite provider baseline.
- Relation files `.gicket/relations/MG/1G/06EXB807MN08HABHTHVPKKNFMG--06EXB80FPE3REH11RQ1YR6BW1G--parentOf.json`, `.gicket/relations/1G/S0/06EXB80FPE3REH11RQ1YR6BW1G--06EXB80QQHAYH61RY4X3T1E8S0--blocks.json`, and `.gicket/relations/MR/1G/06EXB76NNRDP7WH1F2R5VYYPMR--06EXB80FPE3REH11RQ1YR6BW1G--blocks.json` match the written parent/downstream/upstream split; `.gicket/tickets/06EXB76NNRDP7WH1F2R5VYYPMR/ticket.json` is `done` and `.gicket/tickets/06EXB80QQHAYH61RY4X3T1E8S0/ticket.json` remains `todo`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No additional blocking examples are missing after the latest refinement; the remaining per-group CLI example idea is already parked under `## Follow-Up Questions` in the persisted contract.

Risky assumptions
- Future contributors will keep new fast tests under the named unit-group ownership pattern so discoverability inside the Unit project does not drift.
- Bridging `tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs` through a single xUnit bridge Fact will preserve the existing named subcase failure output.

AC / test suggestions
- Keep the current contract boundary: developer proof should stay at `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj`, not expand back into undocumented intra-project filters.
- If repo-local Microsoft Testing Platform filtering proof is later documented, capture exact per-group CLI examples in a follow-up ticket instead of expanding this ticket.

Implementation watchouts
- Do not satisfy this ticket by moving SQLite or provider integration coverage out of `tests/DCoding.Data.DVault.Tests/Integration`; the current split with `06EXB80QQHAYH61RY4X3T1E8S0` is explicit.
- Keep the existing bridge style from `tests/DCoding.Data.DVault.Tests/Unit/ConventionFirstEntryPointCoverageTests.cs`; the new technical-metadata bridge should surface failures without orphaning the underlying harness.
- Keep provider-group expectations aligned to the currently observed public surfaces only: `AddDVault()`, `AddDVaultSqlite()`, `IDataVaultSaveService`, `IDataVaultProviderSaveStrategy`, and `DataVaultProviderCapabilityProfiles.Sqlite`.

Non-blocking notes
- The ticket currently still carries `blocked/dev`, `blocked/test`, and `critic-needed` in `.gicket/tickets/06EXB80FPE3REH11RQ1YR6BW1G/ticket.json`; that reflects pre-approval state and is not a contract-quality blocker.
- Earlier PO-critic blockers are now directly answered in comment `06EYW75CTFHC4S7QGQQPTZCEE8.md`, so the prior `return_to_po` rationale no longer applies.

Split recommendations
- No additional split recommended; the unit-surface ticket and downstream integration-category ticket already form the intended boundary.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment