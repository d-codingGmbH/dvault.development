## Developer rework verification

Tester rework identified that `tests/DCoding.Data.DVault.Tests/Modeling` had delivery files without a local/unit anchor. The active branch already contains the fix, so I did not make another repository edit in this run.

Evidence now present on branch:

- `tests/DCoding.Data.DVault.Tests/Modeling/ModelingConventionCoverageTests.cs` defines xUnit facts that invoke `DefaultNamingPolicyTests.Run()` and `NamingPolicyTests.Run()`.
- `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj` compiles `../Modeling/*.cs` into the unit test assembly.
- `tests/DCoding.Data.DVault.Tests/Unit/TestDiscoverySmokeTests.cs` includes `typeof(ModelingConventionCoverageTests)` in `UnitProjectOwnsExpectedFastCoverageGroups`.

Verification performed:

- `bash tools/check-format.sh` passed. Output included `One-member-per-file check passed for 57 packable source files.` and `Formatting check passed.` The command also emitted the existing warning that solution workspace format verification failed while folder whitespace verification passed.
- `git diff --check develop...HEAD -- src tests docs` passed with no diagnostics.
- `git status --short -- src tests docs` produced no output.
- `dotnet build DVault.slnx --nologo` failed during restore with `NU1301` and `Permission denied (api.nuget.org:443)`.
- `dotnet test DVault.slnx --nologo` failed during restore with the same `NU1301` network restriction.
- Focused `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj --nologo --no-restore --filter FullyQualifiedName~ModelingConventionCoverageTests` could not execute tests because the existing restore state is missing `Microsoft.EntityFrameworkCore.Analyzers` 10.0.0.

Remaining risk is environmental: the exact policy build/test commands and the focused unit anchor test still need to be rerun where NuGet restore can access the required packages or the cache is complete.