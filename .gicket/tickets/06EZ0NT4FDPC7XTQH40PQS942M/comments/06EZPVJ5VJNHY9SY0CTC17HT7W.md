[gicket-bot] PO-critic review contract

Summary
- Ready for dev: the contract is bounded, opt-in PIT scope matches the current repository architecture, and I did not find a blocking refinement gap that should send this back to PO.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The delivery contract has `## Open Questions` = `none`, so the approval gate for unresolved open questions is satisfied.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs` currently aggregates only `Hubs`, `Links`, and `Satellites`, so PIT is a genuinely new provider-neutral metadata surface rather than a retrofit of an existing PIT API.
- `src/DCoding.Data.DVault/Modeling/DataVaultModel.cs`, `src/DCoding.Data.DVault/Modeling/DataVaultModelBuilderExtensions.cs`, and `src/DCoding.Data.DVault/Modeling/DataVaultModelOptions.cs` show an existing convention-first modeling path (`DataVaultModel.Create`, `DataVaultModelBuilder`, `UseDataVault()`, `UseNamingPolicy(...)`) that the ticket can extend for PIT.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` currently projects only hubs, links, and satellites by iterating `metadataModel.Hubs`, `metadataModel.Links`, and `metadataModel.Satellites`, which supports the ticket split that keeps PIT metadata/model-builder work separate from sibling EF mapping work.
- `src/DCoding.Data.DVault/TechnicalMetadataColumnRole.cs` exposes a closed shared technical-role set with only `HashKey`, `HashDiff`, `LoadTimestamp`, and `RecordSource`, matching the ticket's warning not to overload that set for PIT-owned snapshot fields without a clear contract reason.
- `docs/plans/deferred-data-vault-capabilities.md` explicitly marks PIT as an opt-in deferred capability and says it must not become a prerequisite for ordinary hub/link/satellite setup, which matches the ticket's scope and non-regression requirements.
- Observed tests already prove the cross-surface drift called out by the ticket: `tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs` expects pure-model satellite index `IxSatCustomerContactSatelliteParentCustomerHashKey`, while `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs` and `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs` assert `IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp`.
- `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` already snapshots the current public modeling API (`DataVaultModelBuilder`, `DataVaultModelOptions`, `DataVaultMetadataModel`, related metadata types), so there is an established baseline for PIT public API additions.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Optional only: one concrete default-naming example for a PIT table plus its per-satellite snapshot timestamp fields would make later API review faster.
- Optional only: one explicit collision example where two satellite descriptors normalize toward the same PIT snapshot-field base name would strengthen naming-policy review.

Risky assumptions
- This assumes PIT-specific snapshot reference fields will get their own provider-neutral representation instead of being forced into the closed `TechnicalMetadataColumnRole` set in `src/DCoding.Data.DVault/TechnicalMetadataColumnRole.cs`.
- This assumes the new PIT pure-model shape will stay aligned with later EF translation even though the current branch already has satellite index-shape drift between pure modeling and EF/schema tests.

AC / test suggestions
- Add paired determinism tests that build the same PIT declaration twice and compare full produced table, column, key, and index names.
- Add a no-PIT regression that proves existing hub/link/satellite model output and EF translation output stay unchanged when PIT is absent.
- Add collision tests for per-satellite snapshot timestamp fields under custom `IDataVaultNamingPolicy` implementations.
- Snapshot any public-enum or public-metadata-shape additions touched by PIT so later mapping work cannot silently redefine them.

Implementation watchouts
- Current source separates provider-neutral modeling under `src/DCoding.Data.DVault/Modeling/*` from EF translation in `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`; PIT should preserve that boundary and leave EF projection to ticket `06EZ0NTB26CCYQ7FCN2REEGDGW`.
- The current pure-model satellite index expectation in `tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs` omits `LoadTimestamp`, while EF/schema tests include it; PIT should define one baseline and assert it across both surfaces.
- Current public enums and roles are hub/link/satellite-centric (`DataVaultTableKind` in `src/DCoding.Data.DVault/Modeling/DataVaultModel.cs`, `DataVaultPropertyRole` in `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs`, and `DataVaultLogicalPropertyKind` in `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs`), so PIT additions need deliberate public-surface choices rather than ad hoc reuse.
- PIT must remain opt-in per `docs/plans/deferred-data-vault-capabilities.md`; do not make it a prerequisite for `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, or ordinary hub/link/satellite declarations.

Non-blocking notes
- The current three-way split across this ticket, `06EZ0NTB26CCYQ7FCN2REEGDGW` for EF mapping, and `06EZ0NTJZEMVA5RPR01V0KNVMR` for docs/examples matches the observed repository seams.

Split recommendations
- No further split recommended; the existing metadata/builder, EF mapping, and docs/example breakdown already matches the repository structure and the current contract boundaries.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment