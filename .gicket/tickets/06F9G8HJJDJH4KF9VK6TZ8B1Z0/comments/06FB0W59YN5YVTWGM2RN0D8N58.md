[gicket-bot] closure-only-ticket-closure-v1

Summary
- Closed closure-only ticket '06F9G8HJJDJH4KF9VK6TZ8B1Z0' because PO-critic verified that the ticket is already satisfied and no developer or tester execution remains.
- PO-critic closure audit approved that the ticket is satisfied without developer or tester execution.

Evidence
- ticket: `06F9G8HJJDJH4KF9VK6TZ8B1Z0`
- parentOf child evidence was not required for this closure-only ticket.

PO-critic audit evidence
- .gicket/tickets/06F9G8HJJDJH4KF9VK6TZ8B1Z0/description.md marks the ticket closure-only and its `## Open Questions` section is `none`.
- `git diff --name-only develop..ticket/06F9G8HJJDJH4KF9VK6TZ8B1Z0-task-update-package-verification-for-db2-provide -- ':(exclude).gicket/**'` returned no files, and `git diff --name-status develop..ticket/06F9G8HJJDJH4KF9VK6TZ8B1Z0-task-update-package-verification-for-db2-provide` listed only `.gicket/**` changes, so there is no unfinished verifier/source/doc/test delta on the branch.
- `git show --name-only 30315cdc8a64` listed only `.gicket/tickets/06F9G8HJJDJH4KF9VK6TZ8B1Z0/{description.md,ticket.json,comments/*,events/*}` for the PO->PO-critic handoff commit.
- `tools/pack-release-packages.sh` includes `src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj` and packs `8.34.0`/`net8.0` plus `10.34.0`/`net10.0`.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` defines `Db2PackageId = DCoding.Data.DVault.Db2`, `IBM.EntityFrameworkCore`, and expected package lines `8.34.0` / `10.34.0`.
- `tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs` contains DB2 artifact/dependency assertions, and `tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs` asserts `IBM.EntityFrameworkCore` `8.0.0.400` and `10.0.0.100` plus `Db2ProviderProjectPinsIbmProviderPackageLineForEachSupportedTargetFramework()`.
- `README.md` documents `dotnet add package DCoding.Data.DVault.Db2 --version 8.34.0` and `--version 10.34.0`, while `src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj` pins `IBM.EntityFrameworkCore` `8.0.0.400` for `net8.0` and `10.0.0.100` for `net10.0`.
- Additional residual docs drift is visible in `docs/production-adoption-checklist.md` and `docs/releases/v0.33.0.md`, which still cite the seven-package `8.33.0` / `10.33.0` baseline; that is documentation scope, not verifier implementation scope.

PO-critic non-blocking notes
- The stale incoming `blocks` relation from done story `06F9G8HBXS7Y42J7XFSQKZ2AZ8` is already treated as obsolete by the ticket metadata/comment flow and is not a closure blocker.

PO-critic closure watchouts
- Treat this ticket as closure-only. Do not reopen `tools/pack-release-packages.sh`, `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, or the verifier/unit matrix tests unless a future repository diff or failing evidence shows an actual regression.

<!-- gicket-semantic-idempotency-key: bot-closure:06f9g8hjjdjh4kf9vk6tz8b1z0:closure-only-ticket:done:doing-done -->