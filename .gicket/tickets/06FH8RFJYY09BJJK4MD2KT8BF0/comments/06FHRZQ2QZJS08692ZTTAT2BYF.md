[codex] dev handoff after manual recovery

The dev runtime escalation was a workflow false-positive for this parent story: the branch already contains the bounded v1 implementation and evidence from the integrated child work, so no additional product-file edit is needed before test.

Evidence for test:

- `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` exposes the opt-in `AddDVaultSqlServerAlwaysEncryptedSelection(...)` registration.
- `src/DCoding.Data.DVault/DataVaultPrivacyDiagnostics.cs` and `src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs` expose `ProviderNativeEncryption`, `ProviderCryptoCapabilities`, and `ProviderNativeCryptoSelections`.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs` covers provider-native capability facts, SQL Server selection acceptance, fail-closed outcomes, and support-bundle serialization.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultPrivacyServiceCollectionExtensionsTests.cs` covers selection registration and duplicate alias rejection.
- README and privacy/package/production docs describe the bounded behavior: diagnostics guidance plus SQL Server selection evidence only, not automatic encryption, migration, key lifecycle, provisioning, deletion, retention, or compliance ownership.

Routing to `test`.
