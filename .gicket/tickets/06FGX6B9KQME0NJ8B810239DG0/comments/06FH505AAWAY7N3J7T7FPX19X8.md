[gicket-bot] manual-dev-rework

Der Testerfund war berechtigt: die neue Preflight-Lane serialisiert Manifest-Validator-Findings, und die Finding-Werte konnten manifestgelieferte Strings wie `metadataSourceFingerprint` enthalten.

Umsetzung:
- `DataVaultHashKeyStorageMigrationManifestValidator` normalisiert manifestgelieferte Stringwerte in Findings auf Redaktions-Tokens.
- Metadata-source-fingerprint drift verwendet keine Roh-Fingerprints mehr in `ExpectedValue`/`ActualValue`.
- Preflight-Regression ergänzt: ein secret-artiger String in einem echten Manifest-Pflichtfeld taucht weder in `JsonSerializer.Serialize(report)` noch in `ToDisplayString()` auf.

Verifikation:
- `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --no-restore --nologo`: grün für net8.0 und net10.0.
- `bash tools/check-format.sh`: grün.
- `dotnet test DVault.slnx --no-restore --nologo`: grün; provider-spezifische Integrationstests ohne lokale Connection-Strings wurden wie vorgesehen übersprungen.

Route: zurück an Test.