## Developer rework verification

Tester rework asked dev to close the remaining deterministic verification gap. I did not change repository source files; the implementation paths already present on the branch continue to match the tester's accepted code-level evidence.

Verification performed:

- `bash tools/check-format.sh` passed. Output included `One-member-per-file check passed for 57 packable source files.` and `Formatting check passed.`
- `git diff --check develop...HEAD -- src tests docs` passed with no whitespace/conflict-marker diagnostics in source, tests, or docs.
- `dotnet build DVault.slnx --nologo` failed before compile during NuGet restore with `NU1301` / `Permission denied (api.nuget.org:443)`.
- `dotnet test DVault.slnx --nologo` failed before test execution for the same sandboxed NuGet restore reason.
- `dotnet test DVault.slnx --nologo --no-restore` still failed from the existing restore state with `NU1301`.
- `dotnet test DVault.slnx --nologo -p:RestoreIgnoreFailedSources=true` confirmed the offline cache is insufficient here, ending with missing-package `NU1101` errors for EF Core packages.

Repository verification targets for tester:

- `src/DCoding.Data.DVault/DataVaultSaveService.cs`: `CreateSatelliteSavePlan` builds driving-key fields in `DataVaultSatelliteMetadata.DrivingKeyNames` order, writes those columns before `HashDiff`, constructs `SatelliteSeriesKey(parentHashKey, drivingKey tuple)`, and returns `DataVaultSavedRecord` with canonical driving-key identity.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`: `DefaultSaveServicePersistsMultiActiveSatelliteRowsByCanonicalDrivingKeysThroughSqlite` covers canonical reordering, `RowsWritten` values `2`, `0`, and `1`, saved-record order, unchanged replay suppression, changed-row insertion, and same-parent same-load-timestamp coexistence for different driving-key tuples.
- `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs`: `SatelliteSaveOperationValidatesMultiActiveDrivingKeyValuesExactly` covers missing, extra, duplicate, and null driving-key value validation.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs` and `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs` cover translated multi-active column, primary-key, and index order.

Remaining risk is environmental: the exact policy build/test commands still need to be rerun in an environment with NuGet restore access or a complete offline package cache.