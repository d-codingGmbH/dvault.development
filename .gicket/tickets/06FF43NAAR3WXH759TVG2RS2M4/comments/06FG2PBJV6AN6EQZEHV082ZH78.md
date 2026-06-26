[gicket-bot] dev rework completed

Implemented the tester rework for `06FF43NAAR3WXH759TVG2RS2M4`.

Changes made:
- Added the missing diagnostics regression test for a marked `personalData` field whose `encryptedPayloadAlias` is not registered in the active privacy proof.
- The test verifies fail-closed `personal-data-privacy-coverage-unusable`, includes the expected alias in the diagnostic message, and checks the explicit "has not registered encrypted payload alias" branch.

Verification:
- `dotnet test tests\DCoding.Data.DVault.Tests\Unit\DCoding.Data.DVault.Tests.Unit.csproj --filter "FullyQualifiedName~DataVaultDiagnosticsTests|FullyQualifiedName~DataVaultEncryptedPayloadValueConverterTests|FullyQualifiedName~ApiSurfaceSnapshotTests" --nologo`
- The Microsoft Testing Platform ignored the VSTest filter and ran the unit test assembly for `net8.0` and `net10.0`; both target runs passed.

Routing:
- Removed `needs-dev` and added `needs-test` so the tester can re-run verification.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "operationToken": "runtime-environment-precondition",
  "role": "dev",
  "outcome": "resolved",
  "observedAtUtc": "2026-06-25T22:46:46.0468726Z",
  "resolvedAtUtc": "2026-06-26T00:33:54.3768401Z",
  "reason": "Manual dev rework applied the tester-identified missing unregistered-alias diagnostics test and verified the unit test assembly for net8.0 and net10.0. The ticket is routed back to test.",
  "returnToRole": "test",
  "stopFurtherAutoWrites": false
}
```