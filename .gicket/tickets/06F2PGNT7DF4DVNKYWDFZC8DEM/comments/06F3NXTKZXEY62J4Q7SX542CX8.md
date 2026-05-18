## Dev Rework Report

Resolved the tester documentation findings for provider bulk coverage.

Changes in this pass:
- Updated `README.md` Provider Packages wording so Oracle is described as supporting clean `Oracle.EntityFrameworkCore` contexts that meet the native bulk gate, including ordinary hub, link, and satellite batches.
- Updated `docs/architecture/dvault-v1-explicit-save-service.md` so the Oracle matrix row no longer says hub/link-only or satellite fallback, and so the SQL Server ownership bullet describes the external opt-in live bulk lane instead of a smoke lane.

Verification run in this sandbox:
- `git grep -n "Oracle.*satellite request batches declined\|hub/link batches only\|opt-in smoke lane\|one hub, one link, and one satellite" -- README.md docs/architecture/dvault-v1-explicit-save-service.md` returned no matches.
- `git diff --check -- README.md docs/architecture/dvault-v1-explicit-save-service.md` passed.
- `git diff --check develop...HEAD -- README.md docs/architecture/dvault-v1-explicit-save-service.md src tests` passed.
- `bash tools/check-format.sh` passed with the existing solution-workspace warning and folder whitespace fallback.
- `dotnet test DVault.slnx --nologo --no-restore` did not run tests because the sandbox package cache is missing `Microsoft.EntityFrameworkCore.Analyzers` 10.0.8 for the test projects.

Live provider pass evidence still requires a restored package cache plus the documented `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, and `DVAULT_TEST_MYSQL_CONNECTION_STRING` inputs.