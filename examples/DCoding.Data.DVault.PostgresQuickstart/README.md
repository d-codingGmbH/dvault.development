# PostgreSQL Container Fixture Quickstart

This sample starts a developer-managed PostgreSQL container and passes the resulting connection string to the existing PostgreSQL quickstart and opt-in integration tests. It is local-only setup guidance; DVault does not start containers, provision databases, or require Docker or Podman for default test runs.

The fixture uses the checked-in provider baseline image `docker.io/postgres:18` and the same environment variable as the tests and quickstart:

```sh
DVAULT_TEST_POSTGRES_CONNECTION_STRING='Host=localhost;Port=5432;Database=dvault_tests;Username=dvault;Password=<local-password>'
```

Keep the password in a local environment variable, shell prompt history-safe secret store, or another untracked source. Do not commit machine-specific connection strings or real credentials.

## Fixture Defaults

| Setting | Value |
| --- | --- |
| Image | `docker.io/postgres:18` |
| Container name | `dvault-postgres-fixture` |
| Host port | `5432` |
| Database | `dvault_tests` |
| User | `dvault` |
| Password | supplied locally as `DVAULT_POSTGRES_PASSWORD` |

The configured PostgreSQL user must be able to connect to `dvault_tests`, create and drop temporary `dvault_test_*` schemas, and create/drop tables inside those schemas. The official Postgres image creates the configured database with the configured user as owner, which satisfies that local fixture boundary.

If port `5432` is already in use, map a different host port and use the same port in `DVAULT_TEST_POSTGRES_CONNECTION_STRING`. For example, `--publish 55432:5432` pairs with `Port=55432`.

## Start With Podman

```sh
export DVAULT_POSTGRES_PASSWORD='<local-password>'
podman run --name dvault-postgres-fixture --detach --replace --publish 5432:5432 --env POSTGRES_DB=dvault_tests --env POSTGRES_USER=dvault --env POSTGRES_PASSWORD="$DVAULT_POSTGRES_PASSWORD" docker.io/postgres:18
```

## Start With Docker

```sh
export DVAULT_POSTGRES_PASSWORD='<local-password>'
docker rm -f dvault-postgres-fixture
docker run --name dvault-postgres-fixture --detach --publish 5432:5432 --env POSTGRES_DB=dvault_tests --env POSTGRES_USER=dvault --env POSTGRES_PASSWORD="$DVAULT_POSTGRES_PASSWORD" docker.io/postgres:18
```

If Docker reports that the container does not exist during cleanup, continue with the `docker run` command.

## Configure The Connection String

```sh
export DVAULT_TEST_POSTGRES_CONNECTION_STRING="Host=localhost;Port=5432;Database=dvault_tests;Username=dvault;Password=$DVAULT_POSTGRES_PASSWORD"
```

Podman and Docker networking can differ by host. If the container is reachable through a different hostname or port, update `Host=` and `Port=` in the connection string rather than changing the DVault environment variable name.

## Run The PostgreSQL Quickstart

From the repository root:

```sh
dotnet run --project examples/DCoding.Data.DVault.PostgresQuickstart/DCoding.Data.DVault.PostgresQuickstart.csproj
```

The quickstart uses `AddDVaultPostgres()`, `UseNpgsql(...)`, and `UseDataVaultMetadata()` to create the DVault schema in the configured database, save the sample customer profile history, and read latest/as-of projections.

## Run The Opt-In Postgres Tests

From the repository root:

```sh
dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=Postgres" -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured
```

The `-p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured` marker is intentionally non-secret. It makes the integration test project restore the conditional `Npgsql.EntityFrameworkCore.PostgreSQL` package for this run while the real connection string stays in the environment variable.

## Expected Missing-Setup Outcomes

- Missing Docker or Podman: the container start command fails before any DVault command runs. Install or start the selected runtime, or provide another developer-managed PostgreSQL database.
- Missing image or blocked image pull: the runtime fails while resolving `docker.io/postgres:18`. Pull the image locally or use an approved local mirror while keeping the effective PostgreSQL version explicit in local notes.
- Missing `DVAULT_TEST_POSTGRES_CONNECTION_STRING`: the quickstart exits successfully with its skip message, and Postgres integration tests report their configured skip instead of requiring an external database.
- Unreachable database, wrong port, or wrong credentials: the quickstart or opt-in tests fail with the underlying Npgsql connection/authentication error. This is an opt-in local configuration failure, not a default test-suite failure.
- Insufficient database privileges: the opt-in tests fail when they create or clean up temporary schemas/tables. Grant the configured user database-level `CREATE` permission for local test schemas, or recreate the fixture with the defaults above.

## Clean Up

```sh
podman rm -f dvault-postgres-fixture
```

or:

```sh
docker rm -f dvault-postgres-fixture
```

## Reusable Provider Fixture Pattern

Future provider fixture samples should keep the same lifecycle visible:

1. Start a developer-managed provider container with an explicit image tag.
2. Configure the existing provider-specific connection-string environment variable.
3. Run the targeted quickstart or external opt-in tests from the repository root.
4. Inspect skip or failure output for missing runtime, missing configuration, unreachable database, or insufficient privileges.
5. Clean up the local container without adding a default test dependency on external services.
