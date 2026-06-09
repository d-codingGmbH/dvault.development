[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the PO contract is complete, repository evidence matches the stated 0.x baseline, and the remaining work is already split into follow-on implementation tickets.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git log --oneline --decorate -n 20` in `/mnt/c/Projects/DVault` shows HEAD `ee5963d55` on `ticket/06F9GF2Z4Y7A91ZHG4NW1YTNMC-task-define-8-x-and-10-x-package-version-line-po`; `git diff --name-only develop...HEAD` lists only `.gicket/tickets/06F9GF2Z4Y7A91ZHG4NW1YTNMC/**`, so this branch is still ticket-contract work rather than implementation.
- `.gicket/tickets/06F9GF2Z4Y7A91ZHG4NW1YTNMC/description.md` now contains the delivery contract, and its `## Open Questions` section is `none` while acceptance criteria cover the historical 0.x line, `8.33.0`/`10.33.0`, same package IDs, consumer update behavior, pack flow, verifier behavior, and documentation wording.
- `README.md:10-16` still shows the seven coordinated package IDs at version `0.32.0`, `README.md:1215` keeps `dotnet pack DVault.slnx --configuration Release --nologo` as the pack surface, and `docs/releases/v0.32.0.md:11-22` records the same seven-package `0.32.0` baseline.
- `Directory.Build.props:16-22` sets `MinVerTagPrefix` to `v` for the seven packable DVault packages, matching the contract's assumption that planning-release tags remain the version source.
- `docs/manual-nuget-publication.md:9-18,63-91` defines exactly the seven-package family, keeps the canonical pack step as `dotnet pack DVault.slnx --configuration Release --nologo`, and states that package versions are derived from Git tags with the `v` prefix by MinVer.
- `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs:8,15-66,264-280,445-473` currently hard-codes `TargetFramework = "net10.0"`, expects exactly the seven package IDs, validates README/XML/analyzer assets, and fails provider packages whose `DCoding.Data.DVault` dependency version differs from the packed core version.
- `rg -n "<TargetFramework>net10.0" /mnt/c/Projects/DVault/src -g '*.csproj'` returns all current DVault package projects, and `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj:28-29` references EF Core `10.0.8`, so the repository baseline is still net10-only and the ticket is correctly acting as pre-development policy input.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Optional consumer examples for safe versus unsafe NuGet floating ranges across `0.x`, `8.x`, and `10.x` would make the documentation follow-up easier.
- Optional examples showing how the analyzer package should stay aligned with the selected compatibility line, even with `PrivateAssets="all"`, would reduce downstream doc ambiguity.

Risky assumptions
- The follow-on verifier and multitargeting tickets will successfully translate the current net10-only pack and verification surfaces into separate 8.x and 10.x artifact runs without producing mixed-line outputs.
- Consumers will immediately understand that planning release `v0.33.0` no longer equals a NuGet package version; the documentation ticket will need to make that distinction prominent.

AC / test suggestions
- Add verifier coverage that rejects mixed 8.x/10.x artifact sets, wrong provider-to-core dependency versions, and README/install guidance that still implies a `v0.33.0` NuGet package version.
- Add consumer-facing examples that distinguish an ordinary within-line upgrade from an explicit cross-line major migration.

Implementation watchouts
- `PackageVerifier.cs` currently assumes `net10.0` asset paths, so dual-line support will need explicit verifier updates rather than relying on current package checks.
- All current packable projects under `src/` are single-target `net10.0`, so the implementation work belongs in the already-split compatibility and multitarget tickets, not in this policy ticket.
- `docs/manual-nuget-publication.md` currently assumes one aligned seven-package family per release step; follow-on tickets must clarify how manual approval and pack verification apply per 8.x versus 10.x artifact family without broadening the publication boundary.

Non-blocking notes
- The ticket branch is appropriately limited to `.gicket` metadata updates; no product-code or documentation implementation was attempted here.
- The existing epic decomposition is adequate for this scope; no new child ticket is needed to start development.

Split recommendations
- No additional split recommended. The existing epic already separates this policy ticket from the compatibility-contract, multitargeting, verifier/CI, and documentation follow-on tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment