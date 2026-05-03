# API Surface Snapshots

DVault protects the public API for each packable package with committed text snapshots generated from built assembly output.

The approval gate lives in `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs`. Normal repository validation runs it through:

```sh
dotnet test DVault.slnx --nologo
```

To run only the API approval checks:

```sh
dotnet test DVault.slnx --nologo --filter FullyQualifiedName~ApiSurfaceSnapshotTests
```

Approved baselines are stored per package in `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/`:

- `DCoding.Data.DVault.approved.txt`
- `DCoding.Data.DVault.Sqlite.approved.txt`
- `DCoding.Data.DVault.Postgres.approved.txt`
- `DCoding.Data.DVault.SqlServer.approved.txt`
- `DCoding.Data.DVault.Oracle.approved.txt`
- `DCoding.Data.DVault.MySql.approved.txt`

When a public type, member, parameter, or constant changes in one package, the matching package snapshot fails while the other package snapshots continue to report their own surfaces separately. This keeps provider package changes from hiding changes in the core package, even though the provider extension classes share the `DCoding.Data.DVault` namespace.

Intentional API changes should update source and the affected approved snapshot in the same change. Regenerate the baselines with:

```sh
DVAULT_UPDATE_API_SNAPSHOTS=1 dotnet test DVault.slnx --nologo --filter FullyQualifiedName~ApiSurfaceSnapshotTests
```

Review the resulting diff before committing. A snapshot-only update is appropriate only when the current built public API is the intended approved API.
