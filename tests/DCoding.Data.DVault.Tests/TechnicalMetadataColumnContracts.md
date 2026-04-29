# Technical Metadata Column Contracts

Ticket: 06EXB755X9TGQW2EG1G30GJG28

This artifact records the reusable Data Vault technical metadata column contract implemented by this ticket. The contract is implemented in `src/DCoding.Data.DVault` and covered by executable acceptance tests in `tests/DCoding.Data.DVault.Tests`.

This branch intentionally leaves generated `bin` and `obj` output out of the deliverable. The source and test project files are included only to host the concrete contract implementation and its acceptance tests now that the foundation scaffold is available.

The repository root contains `DVault.slnx` as the automation entrypoint for the policy `dotnet build` and `dotnet test` commands.

## Contract Shape

A reusable technical metadata column contract must expose:

- Metadata role identity.
- Semantic purpose.
- Requiredness expectation.
- Default effective column name.
- Current effective column name.

The v1 role set is closed to exactly:

- `HashKey`
- `HashDiff`
- `LoadTimestamp`
- `RecordSource`

Names are overrideable per contract instance. An override changes only the current effective column name used by consumers. It must not change the metadata role identity, the semantic purpose, the requiredness expectation, or the default effective column name.

The same representation is intended for downstream hub, link, and satellite modeling work. Downstream vault structures should reuse this shared role model instead of defining incompatible structure-specific metadata roles.

## v1 Default Contracts

| Role identity | Semantic purpose | Requiredness expectation | Default effective column name |
| --- | --- | --- | --- |
| `HashKey` | Stable hashed identifier derived from business key values for Data Vault keying and joins. | Required when a consuming model declares the hash key metadata role. | `HashKey` |
| `HashDiff` | Hash of descriptive or change-detection attributes for satellite change detection. | Required when a consuming model declares the hash diff metadata role. | `HashDiff` |
| `LoadTimestamp` | Timestamp recording when the row was loaded into the vault. | Required when a consuming model declares the load timestamp metadata role. | `LoadTimestamp` |
| `RecordSource` | Lineage value identifying the originating source system, feed, or batch. | Required when a consuming model declares the record source metadata role. | `RecordSource` |

## Acceptance Cases For Automated Tests

The executable tests in `tests/DCoding.Data.DVault.Tests` cover these cases:

- The default contract set contains exactly four contracts.
- The default contract set contains one `HashKey` contract, one `HashDiff` contract, one `LoadTimestamp` contract, and one `RecordSource` contract.
- The default `HashKey` contract has default effective column name `HashKey` and current effective column name `HashKey`.
- The default `HashDiff` contract has default effective column name `HashDiff` and current effective column name `HashDiff`.
- The default `LoadTimestamp` contract has default effective column name `LoadTimestamp` and current effective column name `LoadTimestamp`.
- The default `RecordSource` contract has default effective column name `RecordSource` and current effective column name `RecordSource`.
- Overriding the `HashKey` contract name to `CustomerHashKey` preserves role identity `HashKey` and default effective column name `HashKey`, while changing only the current effective column name to `CustomerHashKey`.
- Overriding the `HashDiff` contract name to `CustomerHashDiff` preserves role identity `HashDiff` and default effective column name `HashDiff`, while changing only the current effective column name to `CustomerHashDiff`.
- Overriding the `LoadTimestamp` contract name to `LoadedAtUtc` preserves role identity `LoadTimestamp` and default effective column name `LoadTimestamp`, while changing only the current effective column name to `LoadedAtUtc`.
- Overriding the `RecordSource` contract name to `SourceSystem` preserves role identity `RecordSource` and default effective column name `RecordSource`, while changing only the current effective column name to `SourceSystem`.
- All four roles use the same reusable contract shape rather than separate hub, link, or satellite-specific role definitions.

## Foundation Dependency

This artifact does not create a solution file, database DDL, migration script, provider-specific casing policy, or full hub/link/satellite modeling behavior.

The shared contract is implemented in namespace `DCoding.Data.DVault` under `src/DCoding.Data.DVault`, with focused automated tests under `tests/DCoding.Data.DVault.Tests`.
