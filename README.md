# DVault

DVault is the repository for the `DCoding.Data.DVault` .NET library.

## Layout

- `DVault.slnx`: Canonical root solution file for build and test automation.
- `src/DCoding.Data/`: Non-packable build anchor for the `DCoding.Data` source-root namespace family.
- `src/DCoding.Data.DVault/`: Main library project. The NuGet package id and root namespace are `DCoding.Data.DVault`.
- `tests/DCoding.Data.DVault.Tests/`: Unit, integration, and shared test projects for DVault.
- `examples/`: Future runnable examples for DVault APIs.
- `benchmarks/`: Future performance benchmark projects.
- `docs/`: Documentation and design notes.

All current .NET projects are included in `DVault.slnx`. Empty future-use folders contain `.gitkeep` files so the layout is present in clean checkouts.

## Local Validation

```sh
dotnet build
dotnet test
dotnet pack src/DCoding.Data.DVault/DCoding.Data.DVault.csproj --configuration Release --nologo
bash tools/check-format.sh
```

## Optional Local Postgres Integration Tests

Postgres integration tests are opt-in and are skipped by default. Normal `dotnet test` execution does not require Postgres, Docker, or checked-in machine-specific configuration.

To run the Postgres-backed integration tests, provide a developer-managed PostgreSQL database connection string in `DVAULT_TEST_POSTGRES_CONNECTION_STRING`:

```sh
DVAULT_TEST_POSTGRES_CONNECTION_STRING='Host=localhost;Port=5432;Database=dvault_tests;Username=dvault;Password=local-secret' dotnet test DVault.slnx --nologo
```

DVault does not provision Docker containers or databases for these tests. The configured database must already exist, and the configured user must be allowed to create and drop temporary schemas. Keep credentials in local environment variables or another untracked secret store, not in repository files.

## License

DVault uses the Apache License 2.0. See `LICENSE`.
