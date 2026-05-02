[gicket-bot] dev-delivery-v1

Summary
- Existing branch state already satisfies the parent story repository contract for the order/product comparison scenario.
- No repository file changes were needed in this dev pass.
- Developer handoff decision: `already_satisfied_on_branch`.

Repository evidence
- `tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:11-105` contains the conventional EF Core SQLite scenario for `Order`, `Product`, and `OrderLine`, including persisted line payload assertions.
- `tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:429-474` maps ordinary EF entities, keys, indexes, and foreign keys without DVault metadata APIs.
- `tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:110-238` contains the DVault scenario using `AddDVault`, `IDataVaultSaveService`, hubs, one `OrderProduct` link, and a link-attached `Fulfillment` satellite.
- `tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:206-226` asserts an unchanged latest fulfillment replay writes zero new rows after two distinct satellite versions.
- `tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:240-298` asserts HubOrder/HubProduct row shape, all four generated table names, and explicit LinkOrderProduct/SatOrderProductFulfillment schema metadata.
- `DVault.slnx:8-10` keeps the integration test project on the root solution validation path.

Verification
- Static inspection with `git grep` confirmed the scenario anchors and expected generated structure assertions.
- `dotnet build DVault.slnx --nologo` was attempted but could not complete because the sandbox blocks NuGet restore access to `https://api.nuget.org/v3/index.json` with NU1301 permission denied.
- `dotnet test DVault.slnx --nologo` was attempted and hit the same sandboxed NuGet restore denial.
- `bash tools/check-format.sh` was attempted but `dotnet format` could not connect to its .NET build-host pipe under the sandbox, reporting permission denied for a `/tmp` pipe.

Tester handoff
- Re-run `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, and `bash tools/check-format.sh` in the normal tester environment where package restore/cache access and .NET build-host pipes are permitted.
- No PO clarification is needed.