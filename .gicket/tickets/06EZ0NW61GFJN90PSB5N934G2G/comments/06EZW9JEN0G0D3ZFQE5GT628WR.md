[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the persisted ticket contract is anchored to a done sibling contract ticket, `Open Questions` is `none`, prior critic questions are answered, and the repository evidence matches the ticket's stated implementation gap.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EZ0NVX3RYPTFZKYCYEH9HB8W/attachments/manifest.json` lists attachment `06EZSBRWD150ATNH9T6FCXYQ2R` as `multi-active-satellite-driving-key-contract.md`, and `docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md` mirrors that contract in-repo.
- `.gicket/tickets/06EZ0NW61GFJN90PSB5N934G2G/comments/06EZW7N6GJ5XFE2FNF7V11JCHC.md` records critic items 1 through 4 as `answered` and explicitly anchors this ticket to the sibling contract artifact.
- `.gicket/tickets/06EZ0NW61GFJN90PSB5N934G2G/comments/06EZW2MQAJKG7W4P20YSDNCQ1M.md` states that `develop` was merged and ticket `06EZ0NVX3RYPTFZKYCYEH9HB8W` is now visible here as `done` for re-evaluation against the finalized contract.
- `src/DCoding.Data.DVault/Modeling/DataVaultModel.cs` currently shows `DataVaultSatelliteBuilder` exposing `Payload(string propertyName)` only; there is no current `DrivingKey(...)` member in the repository baseline.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs` currently shows only `DataVaultSatelliteMetadata(string name, DataVaultMetadataReference parent, IEnumerable<string> descriptiveAttributeNames)` and no `DrivingKeyNames`; `src/DCoding.Data.DVault/DataVaultSaveService.cs` currently shows only `DataVaultSatelliteSaveOperation(metadata, parentHashKey, payloadValues, hashDiff)` and no `DrivingKeyValues`.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` currently translates satellites with technical order `[ParentHashKey, HashDiff, LoadTimestamp, RecordSource]` before payload columns and uses primary key/index `[ParentHashKey, LoadTimestamp]`, matching the ticket's implementation note about the ordinary-satellite baseline.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` currently loads and compares latest satellite hash diffs by `ParentHashKey` only (`LoadLatestSatelliteHashDiffsAsync`, `ShouldWriteSatelliteRow`, `TrackLatestSatelliteHashDiff`), and the SQLite/Postgres/SqlServer/MySql/Oracle provider save strategies currently gate `CanSave` by provider, clean-context, and sometimes batch shape rather than multi-active request shape.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The shared contract allows empty-string driving-key values, but the ticket examples do not include a proving example for that allowed case.
- Same-series same-load-timestamp changed-row conflicts remain intentionally unspecified by the shared contract and should stay follow-up work rather than being inferred during implementation.

Risky assumptions
- Assuming provider-specific optimized save strategies will automatically handle multi-active batches would be unsafe; current `CanSave` gates in the SQLite, Postgres, SQL Server, MySQL, and Oracle strategies do not inspect multi-active request shape.
- Assuming parent hash key alone is enough for unchanged replay suppression would be unsafe; the current provider-neutral save service tracks latest satellite hash diffs by `ParentHashKey` only.

AC / test suggestions
- Add one explicit baseline test that empty-string driving-key values are accepted while `null` is rejected, because the shared artifact allows empty strings.
- Add dispatch coverage proving each optimized provider path either declines multi-active batches or honors the canonical parent-plus-driving-key partition before claiming compatibility.
- Lock the new public members and canonical schema/key order together in public API snapshot coverage and schema-projection assertions.

Implementation watchouts
- `src/DCoding.Data.DVault/Modeling/DataVaultModel.cs` currently exposes only `DataVaultSatelliteBuilder.Payload(...)`; ordinary satellite behavior must stay unchanged while `DrivingKey(...)` is added.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs` and `src/DCoding.Data.DVault/DataVaultSaveService.cs` currently expose only the ordinary-satellite constructor shapes; the new driving-key constructor/property pair must stay exactly aligned with the shared contract member names and ordering.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` currently emits satellite columns `[ParentHashKey, HashDiff, LoadTimestamp, RecordSource, payload...]` with primary key/index `[ParentHashKey, LoadTimestamp]`; the multi-active expansion must preserve canonical driving-key order and same-parent same-load-timestamp coexistence for different tuples.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` currently partitions latest-state lookup and replay suppression by `ParentHashKey` only (`LoadLatestSatelliteHashDiffsAsync`, `ShouldWriteSatelliteRow`, `TrackLatestSatelliteHashDiff`).

Non-blocking notes
- The persisted contract already carries its own split guidance: keep `06EZ0NVX3RYPTFZKYCYEH9HB8W` as the finalized contract-definition slice and keep documentation/examples in `06EZ0NWCA6NEZH8VBJNGW4FVHG`.

Split recommendations
- No split needed. Keep `06EZ0NVX3RYPTFZKYCYEH9HB8W` as the completed contract-definition slice, this ticket focused on implementation/proof coverage, and `06EZ0NWCA6NEZH8VBJNGW4FVHG` as the docs/examples follow-up.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment