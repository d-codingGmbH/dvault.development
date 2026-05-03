# One Member Per File

DVault enforces one public or protected top-level C# declaration per source file for the six packable packages:

- `src/DCoding.Data.DVault`
- `src/DCoding.Data.DVault.MySql`
- `src/DCoding.Data.DVault.Oracle`
- `src/DCoding.Data.DVault.Postgres`
- `src/DCoding.Data.DVault.Sqlite`
- `src/DCoding.Data.DVault.SqlServer`

The non-packable `src/DCoding.Data` anchor, test projects, benchmark projects, generated source, and `bin` or `obj` output are outside this policy.

## Automated Check

Normal local validation runs the policy through:

```sh
bash tools/check-format.sh
```

To run only this source-layout check:

```sh
bash tools/check-one-member-per-file.sh
```

The check scans tracked and untracked C# source files under the six packable project roots and fails when a file contains more than one public or protected top-level `class`, `struct`, `interface`, `record`, `enum`, or `delegate` declaration. Failure output includes the repository-relative source path so the declaration can be moved or reviewed directly.

## Exceptions

The authoritative exception list is `docs/quality/one-member-per-file-exceptions.txt`. Each entry must be a repository-relative C# source path under one of the six packable project roots and must continue to contain more than one public or protected top-level declaration. Stale or out-of-scope exceptions fail the same check.

Current retained practical exceptions are limited to existing core-package API cluster files:

- `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs`: annotation constants and the EF property-role enum share one small metadata naming surface.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs`: provider capability enums, mapping records, and profile accessors describe one provider-profile contract.
- `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs`: the strategy interface and its execution context are a single extension contract.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs`: explicit save request, operation, result, and service contracts are retained together for the v1 write boundary.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs`: metadata DTOs and references are retained together for the v1 modeling metadata surface.
- `src/DCoding.Data.DVault/Modeling/DataVaultModel.cs`: the model entry point and builder implementation are retained together until the builder partial layout is decomposed.
- `src/DCoding.Data.DVault/Modeling/IDataVaultNamingPolicy.cs`: naming policy interface and naming context records are retained together for the naming contract.

The visible `DataVaultModelBuilder` partial split between `src/DCoding.Data.DVault/Modeling/DataVaultModel.cs` and `src/DCoding.Data.DVault/Modeling/DataVaultModelBuilder.cs` is intentional for the current baseline. `DataVaultModel.cs` remains a documented exception because it also declares `DataVaultModel`; `DataVaultModelBuilder.cs` is not an exception because it contains only one public top-level declaration.

New exceptions should be rare and should document why splitting the file would make the public API harder to maintain. Prefer moving new public or protected top-level declarations into their own files.
