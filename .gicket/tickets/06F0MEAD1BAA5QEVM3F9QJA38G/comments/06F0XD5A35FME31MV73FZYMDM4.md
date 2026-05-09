### Dev rework update

Resolved the tester path findings by adding the previously missing expected repository paths as concrete artifacts:

- `06F0MEAD1BAA5QEVM3F9QJA38G-fluent-parity-child-boundary.md` now exists at repository root and points back to the canonical `docs/plans/...` addendum.
- `Unit/DataVaultCodeFirstMetadataTranslationTests.cs` now exists and contains a real xUnit parity test for hub, hub-parent satellite, and ordered `DrivingKey(...)` shape.
- `Unit/DataVaultCodeFirstLinkTests.cs` now exists and contains a real xUnit parity test for link participant relationship shape and declaration order.
- `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj` now links `../../../Unit/*.cs` under `ExpectedPaths/`, so the top-level `Unit/*.cs` files compile into the existing unit test project instead of acting as inert placeholders.

Verification run:

- `bash tools/check-format.sh` passed.
- `dotnet build DVault.slnx --nologo --no-restore` and `dotnet test DVault.slnx --nologo --no-restore` are still blocked before compilation by the local NuGet cache issue: `Microsoft.EntityFrameworkCore.Analyzers` version `10.0.0` is missing. Re-run the policy build/test commands after restoring package cache availability.