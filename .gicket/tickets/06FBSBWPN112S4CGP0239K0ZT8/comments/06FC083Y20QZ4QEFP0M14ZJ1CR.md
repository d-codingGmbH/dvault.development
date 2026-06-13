[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the contract is specific, `## Open Questions` is `none`, and direct repo evidence matches the stated package-line, dependency-matrix, and analyzer-host baseline.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted contract for `06FBSBWPN112S4CGP0239K0ZT8` sets PO handoff to `ready_for_po_critic`, scopes only `README.md`, `CHANGELOG.md`, `docs/manual-nuget-publication.md`, and new `docs/releases/v0.37.0.md`, and its `## Open Questions` section is `none`.
- A `git show --no-patch 0d6ca2479` check reports HEAD on `ticket/06FBSBWPN112S4CGP0239K0ZT8-task-document-v0-37-dependency-and-analyzer-comp` is commit `0d6ca2479` with message `[06FBSBWPN112S4CGP0239K0ZT8] lease claim po-critic`; a `git diff --name-only 0d6ca247950092b8b968baedeb42508029b45390..HEAD -- README.md CHANGELOG.md docs/manual-nuget-publication.md docs/releases/v0.37.0.md` check returned no paths, so the branch is still in pre-development review state.
- `README.md` currently labels `Current v0.36.0 Hash-Key Storage Guidance Baseline` and `Current v0.36.0 Limitations` as current, and links the current release notes to `docs/releases/v0.36.0.md`.
- `CHANGELOG.md` still leads with `v0.36.0 - Binary Hash-Key Storage Adoption Guidance`, and `find /mnt/c/Projects/DVault/docs/releases -maxdepth 1 -type f | sort` shows release-note files through `docs/releases/v0.36.0.md` with no `docs/releases/v0.37.0.md` yet.
- `docs/manual-nuget-publication.md` currently documents the v0.36 compatibility release with package lines `8.36.0` / `net8.0` and `10.36.0` / `net10.0`, matching the contract's consumer-line baseline.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`, `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj`, and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` directly match the contract's dependency matrix: core EF/Relational `8.0.28` plus DI `8.0.2` for `net8.0`, core EF/Relational/DI `10.0.9` for `net10.0`, DB2 `8.0.0.400` / `10.0.0.100`, MySQL `8.0.26` / `10.0.7`, PostgreSQL `8.0.11` / `10.0.2`, Oracle `8.<redacted>` / `<redacted>`, and SQL Server/SQLite `8.0.28` / `10.0.9`.
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0`; `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` references it as an analyzer with `PrivateAssets=all` and `SetTargetFramework=TargetFramework=net10.0`; `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/plans/analyzer-package-compatibility-audit.md`, `docs/local-validation.md`, and `.github/workflows/ci.yml` all state the `.NET 10 SDK` host baseline.
- `tools/pack-release-packages.sh` still packs only `8.36.0` for `net8.0` and `10.36.0` for `net10.0`, and a repository-wide `rg` search found no `v0.37.0`, `8.37.0`, `10.37.0`, or `0.37.0` strings.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- An explicit example that historical `v0.36.0` references may remain only in release-history or background sections while all current-baseline headings and links move to `v0.37.0` would further reduce rewrite ambiguity.
- Keeping the `.NET 10 SDK` host caveat immediately adjacent to every `8.36.0` analyzer install example is the main reader edge case to preserve.

Risky assumptions
- The ticket assumes the visible consumer package lines stay `8.36.0` and `10.36.0` until a separate packaging ticket updates `tools/pack-release-packages.sh`, verifier expectations, and install guidance together.
- The ticket assumes the current validation lane in `docs/local-validation.md` and `.github/workflows/ci.yml` remains the authoritative carried-forward evidence story for `v0.37.0` without new release-process changes.

AC / test suggestions
- Add a doc-review check that `README.md`, `CHANGELOG.md`, `docs/manual-nuget-publication.md`, and `docs/releases/v0.37.0.md` all use `v0.37.0` as the current baseline and leave `v0.36.0` only in historical context.
- Add a doc-review check that no consumer example or approval text introduces `8.37.0`, `10.37.0`, or `0.37.0`.
- Add a doc-review check that every analyzer example keeps the `.NET 10 SDK` build-host requirement and local analyzer posture with `PrivateAssets=all`.

Implementation watchouts
- Do not treat the planning label `v0.37.0` as evidence for new NuGet versions; direct repo evidence still points to `8.36.0` and `10.36.0`.
- Do not broaden analyzer compatibility to pure `.NET 8 SDK` consumption; direct source evidence only proves a single `net10.0` analyzer asset consumed on a `.NET 10 SDK` host.
- Update only the current-baseline framing in the four in-scope docs; keep `v0.36.0` material as release history or background where it no longer claims to be current.

Non-blocking notes
- The branch currently contains only the PO-critic lease-claim commit, which is consistent with this being a pre-development ticket-quality gate.
- The contract already keeps downstream work bounded by leaving `06FBSBWW414TE19KZT14CB7Y3R` as the only active follow-up dependency.

Split recommendations
- No new split for this ticket; if release management later wants consumer lines `8.37.0` and `10.37.0`, handle that as a separate packaging or release ticket rather than folding it into this documentation-baseline task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment