# DVault Quickstart Examples

These examples run the same bounded customer-profile history flow through the public registry-backed metadata path:

- `DCoding.Data.DVault.SqliteQuickstart` uses SQLite and needs no external infrastructure.
- `DCoding.Data.DVault.PostgresQuickstart` uses PostgreSQL through `AddDVaultPostgres()` and a developer-managed connection string.

Both projects register one shared `DataVaultMetadataModel` with `AddDVault(options => options.UseMetadataModel(...))`, opt the DbContext into that registry with `UseDataVaultMetadata()`, write through `IDataVaultSaveService`, and read typed latest/as-of satellite projections through `IDataVaultReadService`.

## Build

From the repository root:

```sh
dotnet build DVault.slnx --nologo
```

## Run SQLite

```sh
dotnet run --project examples/DCoding.Data.DVault.SqliteQuickstart/DCoding.Data.DVault.SqliteQuickstart.csproj
```

The SQLite quickstart creates a temporary SQLite database file, creates the DVault schema, writes one customer profile twice with distinct load timestamps, then prints the latest profile and the as-of profile from the first timestamp.

## Run PostgreSQL

Set `DVAULT_TEST_POSTGRES_CONNECTION_STRING` to a developer-managed PostgreSQL connection string, then run:

```sh
dotnet run --project examples/DCoding.Data.DVault.PostgresQuickstart/DCoding.Data.DVault.PostgresQuickstart.csproj
```

The PostgreSQL quickstart uses `AddDVaultPostgres()` plus the same `UseDataVaultMetadata()` registry-backed DbContext path as SQLite. It creates the DVault schema in the database named by the connection string and runs the same typed save/read flow.

If `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is missing or empty, the PostgreSQL quickstart exits successfully before opening a database connection and prints:

```text
Skipping PostgreSQL quickstart. Set DVAULT_TEST_POSTGRES_CONNECTION_STRING to a developer-managed PostgreSQL connection string and rerun this example.
```
