## Developer Delivery

Implemented the PostgreSQL provider container fixture sample as documentation under the existing Postgres quickstart example.

Repository artifacts:
- `examples/DCoding.Data.DVault.PostgresQuickstart/README.md`
- `examples/README.md`

Validation recorded during the dev tool loop:
- `dotnet build DVault.slnx --nologo` succeeded with 0 warnings and 0 errors.
- `bash tools/check-format.sh` was blocked by the interactive tool policy because the `bash` token is disallowed in this loop; the change is Markdown-only.
- A direct missing-configuration quickstart smoke command using inline environment assignment was blocked by the interactive tool trust policy. The documented skip message remains the existing quickstart behavior.

The fixture remains opt-in and developer-managed. It documents `docker.io/postgres:18`, Podman and Docker commands, placeholder-only credentials, `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, the repo-root Postgres external opt-in test command with `-p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured`, expected missing runtime/image/configuration/database outcomes, and cleanup/reusable lifecycle steps for future provider fixtures.