## Dev repair handoff

Summary
- Repaired the test failure in `ProviderIntegrationCategoryDiscoveryTests` by adding `DataVaultPitMaintenanceServiceSqliteTests` to the required local SQLite integration coverage list.
- The PIT maintenance implementation from the prior dev snapshot remains the intended repository baseline: additive service, request/result types, DI registration, validation, rebuild and bounded parent maintenance, unit coverage, SQLite integration coverage, and public API snapshot coverage.

Verification focus
- Re-run `dotnet test DVault.slnx --nologo`; the previously reported collection mismatch should be resolved because the discovered public test class is now represented in the expected integration-test type list.
- Re-run `dotnet build DVault.slnx --nologo` and `bash tools/check-format.sh` as normal policy gates.

Deferred scope preserved
- README and v0.15.0 release-note follow-through remain deferred to `06F2PGPXVAYRBC94RQ7X5V4DVG` per the delivery contract.
- PIT read API shape, provider-specific PIT optimization, legacy `DataVaultPointInTimeMetadata`, link-parent PITs, multi-active PITs, bridge maintenance, and background orchestration remain out of scope.