## Developer Delivery: Technical Metadata Column Contracts

Code implementation is deferred because the current branch does not contain the foundation solution/project scaffold required by the delivery contract. The repository has `src/DVault` and `tests/DVault.Tests` directories, but no `.sln`, `.slnx`, `.csproj`, or non-generated source/test files are present. Per the approved fallback path, this comment preserves the bounded v1 contract until the foundation scaffold is available.

### Contract Shape
A reusable technical metadata column contract must expose these members conceptually:

- Metadata role identity.
- Semantic purpose.
- Requiredness expectation.
- Default effective column name.
- Current effective column name.

An override changes only the current effective column name used by consumers. It must not change the metadata role identity or the default effective column name. The same representation is intended for downstream hub, link, and satellite modeling work; downstream structure applicability should reuse this role model instead of creating parallel definitions.

### v1 Role Set

| Role | Semantic purpose | Requiredness expectation | Default effective column name |
| --- | --- | --- | --- |
| Hash key | Stable hashed identifier derived from business key values for Data Vault keying and joins. | Required when a consuming model declares the hash key metadata role. | `HashKey` |
| Hash diff | Hash of descriptive/change-detection attributes for satellite change detection. | Required when a consuming model declares the hash diff metadata role. | `HashDiff` |
| Load timestamp | Timestamp recording when the row was loaded into the vault. | Required when a consuming model declares the load timestamp metadata role. | `LoadTimestamp` |
| Record source | Lineage value identifying the originating source system, feed, or batch. | Required when a consuming model declares the record source metadata role. | `RecordSource` |

The v1 role set is closed to exactly these four roles: hash key, hash diff, load timestamp, and record source.

### Acceptance Cases To Convert Into Automated Tests

- The default contract set contains exactly four contracts with role identities for hash key, hash diff, load timestamp, and record source.
- The default hash key contract has default effective column name `HashKey` and current effective column name `HashKey`.
- The default hash diff contract has default effective column name `HashDiff` and current effective column name `HashDiff`.
- The default load timestamp contract has default effective column name `LoadTimestamp` and current effective column name `LoadTimestamp`.
- The default record source contract has default effective column name `RecordSource` and current effective column name `RecordSource`.
- Overriding the hash key name, for example to `CustomerHashKey`, preserves the hash key role and default name `HashKey` while setting the current effective name to `CustomerHashKey`.
- Overriding the hash diff name, for example to `CustomerHashDiff`, preserves the hash diff role and default name `HashDiff` while setting the current effective name to `CustomerHashDiff`.
- Overriding the load timestamp name, for example to `LoadedAtUtc`, preserves the load timestamp role and default name `LoadTimestamp` while setting the current effective name to `LoadedAtUtc`.
- Overriding the record source name, for example to `SourceSystemCode`, preserves the record source role and default name `RecordSource` while setting the current effective name to `SourceSystemCode`.

### Foundation Dependency
After the foundation setup work provides the solution, `src/DVault` library project, and `tests/DVault.Tests` test project, implement this contract under namespace `DCoding.Data.DVault` and add focused automated tests for the cases above. This pass intentionally does not create solution, project, source, or test scaffold files.