[gicket-bot] dev rework completed

Implemented the tester rework for `06FF43MQ3AXXK2S5TK65X4Y9S8`.

Changes made:
- The privacy converter now exposes its `EncryptedPayloadAlias`, allowing diagnostics to verify the field-level converter alias without adding a core-package dependency on the privacy package.
- Personal-data diagnostics no longer accept alias registration plus encrypted-payload key provider as sufficient coverage by itself.
- Metadata-only analysis with opt-in privacy now fails closed when no field-level `DataVaultEncryptedPayloadValueConverter` wiring can be observed.
- DbContext diagnostics inspect the EF model and accept a marked personal-data payload field only when the matching payload property is wired to `DCoding.Data.DVault.Privacy.DataVaultEncryptedPayloadValueConverter` for the expected alias.
- Unit tests cover the tester's regression case and the positive DbContext converter-wired case; the privacy public API snapshot was updated for the new alias property.

Verification:
- `dotnet test tests\DCoding.Data.DVault.Tests\Unit\DCoding.Data.DVault.Tests.Unit.csproj --filter "FullyQualifiedName~DataVaultDiagnosticsTests|FullyQualifiedName~ApiSurfaceSnapshotTests" --nologo`
- The Microsoft Testing Platform ignored the VSTest filter and ran the unit test assembly for `net8.0` and `net10.0`; both target runs passed.

Routing:
- Removed `needs-dev` and added `needs-test` so the tester can re-run verification.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "operationToken": "model-execution",
  "role": "dev",
  "outcome": "resolved",
  "observedAtUtc": "2026-06-25T13:27:23.3430544Z",
  "resolvedAtUtc": "2026-06-25T20:40:53.6801184Z",
  "reason": "Manual dev rework completed the tester-identified converter-coverage gap after the prior model/tool-loop failure. The ticket is routed back to test.",
  "returnToRole": "test",
  "stopFurtherAutoWrites": false
}
```