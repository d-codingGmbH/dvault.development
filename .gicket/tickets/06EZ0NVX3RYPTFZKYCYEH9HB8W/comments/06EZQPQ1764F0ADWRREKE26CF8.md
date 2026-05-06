[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the refined ticket now matches current DVault satellite and hashing contracts, has no open questions, and correctly avoids inventing unsupported public API names.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs` defines `DataVaultSatelliteMetadata(string name, DataVaultMetadataReference parent, IEnumerable<string> descriptiveAttributeNames)` with `Parent`, `PayloadColumns`, `HashDiffMetadata`, `LoadTimestampMetadata`, and `RecordSourceMetadata`; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs` verifies both hub-parent and link-parent satellites.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` defines `DataVaultSatelliteSaveOperation(... IEnumerable<KeyValuePair<string,string>> payloadValues, string hashDiff)` and documents that payload values are keyed by satellite metadata payload names; `DataVaultHubSaveOperation.RequireValues` uses `StringComparer.Ordinal` and rejects duplicate names.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` maps satellite payload metadata names through `DefaultDataVaultNamingPolicy.GetColumnNames(...)`; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs` and `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs` show payload `Email Address` becomes produced column `EmailAddress` while the ordinary satellite PK/index remain `(ParentHashKey, LoadTimestamp)`.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` `DefaultSaveServicePersistsSatelliteRowsOnlyWhenHashDiffChanges` shows current ordinary behavior: unchanged duplicates are suppressed, changed rows insert new history, and evaluation is per parent hash key.
- `src/DCoding.Data.DVault/DefaultStableHashNormalizer.cs` sorts structured fields by ordinal field path and rejects duplicate field paths; `tests/DCoding.Data.DVault.Tests/Unit/StableHashNormalizerTests.cs` confirms order-independent canonicalization.
- `rg -n "DrivingKey|MultiActive|multi-active" src tests` returned no source hits, and `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` shows no existing public driving-key API, so the ticket's choice to avoid placeholder public names matches current source reality.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete example where a declared payload name differs from the produced physical column name, such as `Email Address` versus `EmailAddress`, would help downstream docs/tests but is not required to hand the ticket to development.
- A link-parent example using the same driving-key rules would make the hub-parent/link-parent parity more explicit for downstream test writers.
- An example showing input driving-key member order differing from payload declaration order would make the canonical-order rule easier to validate.

Risky assumptions
- The implementation will resolve driving-key member names with the same exact provider-neutral string semantics already used by save-operation dictionaries (`StringComparer.Ordinal`), even though the ticket does not state comparer or casing rules explicitly.
- The persistence ticket will translate the logical `(parentHashKey, drivingKey)` partition into a physical schema that still preserves the ordinary non-multi-active `(parentHashKey, loadTimestamp)` baseline without leaking provider-specific promises into this contract ticket.
- The phrase `other metadata-derived or run-variant members` will be interpreted consistently even though the current source only exposes the closed technical roles `HashDiff`, `LoadTimestamp`, and `RecordSource` directly.

AC / test suggestions
- Add a contract/persistence test that a hub-parent satellite accepts driving-key members by payload name and rejects `CustomerHashKey`, `HashDiff`, `LoadTimestamp`, and `RecordSource`.
- Add the same validation test for a link-parent satellite to prove the contract is not hub-only.
- Add a determinism test where payload declaration order is `A,B,C` and driving-key input order is `C,A`, with the canonical representation resolving to declaration order.
- Add a translation test proving a produced physical column name like `EmailAddress` is rejected when the declared payload name is `Email Address`.

Implementation watchouts
- Do not derive driving-key membership from `ProducedName`; use the provider-neutral payload names already carried by `DataVaultSatelliteMetadata.PayloadColumns` and `DataVaultSatelliteSaveOperation.PayloadValues`.
- Do not reduce hash diff to only driving-key members; current source and tests treat hash diff as full satellite payload change detection.
- Keep ordinary satellites unchanged unless multi-active is explicitly enabled.
- Preserve identical behavior for hub-parent and link-parent satellites.

Non-blocking notes
- Comment `06EZQMQGBS0N7AV6S1BDQGACQM.md` documents the PO refinement handoff as `ready_for_po_critic`, and comment `06EZQN0P1E4N0FKTMQV383B84G.md` records that the durable contract block was written back.
- Sibling tickets `06EZ0NW61GFJN90PSB5N934G2G` and `06EZ0NWCA6NEZH8VBJNGW4FVHG` exist for persistence and docs/tests scope, which matches this ticket's stated split.

Split recommendations
- No additional split is needed; the current split between this contract ticket and sibling persistence/docs-test tickets matches the repository and ticket evidence.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment