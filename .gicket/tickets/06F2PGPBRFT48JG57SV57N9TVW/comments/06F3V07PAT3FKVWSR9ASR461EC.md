## Dev rework handoff

Summary
- Repaired the row-generation unit test added for tester rework so it matches DVault's current generated object naming policy.
- The Status satellite shared-type entity is now configured and seeded as `SatCustomerStatu`, while the metadata annotation remains `Status`; this matches the PIT maintenance service lookup that failed in the isolated test run.

Verification focus
- Inspect `tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceRowGenerationTests.cs`, especially `RebuildCreatesDeterministicPitRowsFromDistinctSatelliteLoadTimestamps`.
- Run `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --filter FullyQualifiedName~DataVaultPitMaintenanceRowGenerationTests`.
- Re-run the normal gates: `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, and `bash tools/check-format.sh`.

Deferred scope preserved
- README and v0.15.0 release-note follow-through remain deferred to `06F2PGPXVAYRBC94RQ7X5V4DVG` per the delivery contract.