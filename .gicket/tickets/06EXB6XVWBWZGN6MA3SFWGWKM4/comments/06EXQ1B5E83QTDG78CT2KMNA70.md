Post-cleanup status note after commit `ae81a4e`:

The main library project has been normalized from the transitional `src/DVault/DVault.csproj` location to `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`. It still targets `net10.0`, keeps `RootNamespace` and `PackageId` as `DCoding.Data.DVault`, enables nullable reference types and XML documentation, and builds through the root `DVault.slnx`.