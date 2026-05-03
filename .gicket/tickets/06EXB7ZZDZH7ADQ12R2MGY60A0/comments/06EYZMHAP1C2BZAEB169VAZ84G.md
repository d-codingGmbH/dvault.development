[gicket-bot] PO-critic review contract

Summary
- Tracking-only epic closure audit found blocking readiness gaps.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7ZZDZH7ADQ12R2MGY60A0/description.md` shows `## Open Questions` followed by `- none`, so the persisted delivery contract has no unresolved open questions.
- `.gicket/relations/A0/MG/06EXB7ZZDZH7ADQ12R2MGY60A0--06EXB807MN08HABHTHVPKKNFMG--parentOf.json`, `.gicket/relations/A0/0M/06EXB7ZZDZH7ADQ12R2MGY60A0--06EXB80ZNQTTGT6VN2DKEDGB0M--parentOf.json`, `.gicket/relations/A0/YM/06EXB7ZZDZH7ADQ12R2MGY60A0--06EXB8202A88KJJP7WEGBESBYM--parentOf.json`, and `.gicket/relations/WR/A0/06EXB4MDREV2T51VJNJEP6R0WR--06EXB7ZZDZH7ADQ12R2MGY60A0--relates.json` confirm the expected relation topology.
- `README.md` lists the root validation baseline as `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, `dotnet pack DVault.slnx --configuration Release --nologo`, `bash tools/verify-packages.sh`, and `bash tools/check-format.sh`; `docs/manual-nuget-publication.md` repeats the same commands under Required Pre-Publish Evidence.
- `README.md` defines `Category=ProviderIntegration.RequiredLocal`, `Category=ProviderSmoke.Default`, and `Category=ProviderIntegration.ExternalOptIn`; `tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs` asserts SQLite tests are required-local and Postgres live integration is external-opt-in.
- `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` only restores `Npgsql.EntityFrameworkCore.PostgreSQL` when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is set, and `tests/DCoding.Data.DVault.Tests/Integration/PostgresIntegrationTestConfiguration.cs` states that Postgres coverage is opt-in.
- `docs/quality/api-surface-snapshots.md`, `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs`, and the six files under `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/` provide per-package API snapshot coverage for all six packable packages.
- `docs/quality/one-member-per-file.md` scopes the one-member-per-file rule to the six packable projects, `tools/check-one-member-per-file.sh` hard-codes those six roots, and `src/DCoding.Data/DCoding.Data.csproj` sets `<IsPackable>false</IsPackable>` for the excluded source-root anchor.
- Repository search across `src/DCoding.Data.DVault*.csproj` shows `GenerateDocumentationFile>true` and `WarningsAsErrors` including `CS1591` in all six packable package projects, which is direct source evidence for XML documentation enforcement.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` hard-codes the six expected package ids and validates packaged README presence, XML docs, symbol archives, and provider dependency alignment to the packed `DCoding.Data.DVault` version.
- `git diff --name-only develop..ticket/06EXB7ZZDZH7ADQ12R2MGY60A0-epic-quality-gates-and-nuget-readiness` returns only `.gicket/...` paths, so the branch adds ticket-state metadata rather than new product-code scope.
- parentOf child 06EXB807MN08HABHTHVPKKNFMG status done: Story: Establish automated test strategy
- parentOf child 06EXB80ZNQTTGT6VN2DKEDGB0M status done: Story: Enforce public API quality
- parentOf child 06EXB8202A88KJJP7WEGBESBYM status done: Story: Prepare NuGet release gate

Blocking findings
- The persisted delivery contract does not explicitly mark this tracking-only epic as closure/tracking with no parent-owned implementation slice.

Required PO actions
- Resolve the tracking-epic closure audit findings before this parent ticket can be closed.

Open issues ledger
- critic-item-1 [required-po-action] Resolve the tracking-epic closure audit findings before this parent ticket can be closed.
- critic-item-2 [blocking-finding] The persisted delivery contract does not explicitly mark this tracking-only epic as closure/tracking with no parent-owned implementation slice.

Missing examples / edge cases
- none

Risky assumptions
- This approval assumes future live integration harnesses for SQL Server, Oracle, and MySQL remain out of scope until separate follow-up tickets are created; current opt-in external-provider evidence is Postgres-specific.
- This approval assumes the epic remains a coordination wrapper over the already-done child stories rather than needing its own repository implementation branch, because the target branch differs from `develop` only in `.gicket` metadata.

AC / test suggestions
- During dev/test handoff, execute the documented root validation baseline exactly as written in `README.md` and `docs/manual-nuget-publication.md` so the epic evidence stays aligned with the contract.
- Keep focused default provider runs excluding `Category=ProviderIntegration.ExternalOptIn` unless a follow-up ticket explicitly opts into live external-provider coverage.

Implementation watchouts
- `docs/manual-nuget-publication.md` makes publication all-or-nothing for the six-package family and defines stop conditions after any failed validation or push step; do not treat provider release work as independently shippable under this epic.
- Do not let consumer guidance drift to NuGet-first wording before public packages exist; `README.md` and `docs/manual-nuget-publication.md` both still require source/project-reference guidance.
- Any future package-family change must update `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs`, the API snapshot set, and the one-member-per-file scope together or the documented gates will diverge.

Non-blocking notes
- `find .gicket/tickets/06EXB7ZZDZH7ADQ12R2MGY60A0 -maxdepth 1 -type d` returns only the ticket root, `comments`, and `events`; no attachment directory is present.

Split recommendations
- No additional split is recommended; the existing parentOf children 06EXB807MN08HABHTHVPKKNFMG, 06EXB80ZNQTTGT6VN2DKEDGB0M, and 06EXB8202A88KJJP7WEGBESBYM already match the bounded three-track delivery plan.
- If CI-driven publication, credential handling, or post-publication NuGet-first guidance becomes necessary later, keep them as follow-on tickets rather than widening this epic.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment