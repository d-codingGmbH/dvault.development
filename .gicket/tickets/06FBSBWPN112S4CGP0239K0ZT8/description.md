<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the v0.37 documentation ticket around the already-settled target-matched dependency policy and analyzer compatibility outcome. Current repo evidence supports keeping `8.36.0` / `10.36.0` as the visible consumer package lines in v0.37 guidance unless a separate packaging change lands, and existing downstream checklist ticket `06FBSBWW414TE19KZT14CB7Y3R` remains the only active dependent.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already settles the dependency-line policy: `net8.0` stays on the EF Core 8 line and `net10.0` stays on the EF Core 10 line, with the exact visible baseline `8.0.28` / `8.0.28` / `8.0.2` plus DB2 `8.0.0.400`, SQLite `8.0.28`, MySQL `8.0.26`, PostgreSQL `8.0.11`, Oracle `8.23.26200`, SQL Server `8.0.28` for `net8.0`, and `10.0.9` / `10.0.9` / `10.0.9` plus DB2 `10.0.0.100`, SQLite `10.0.9`, MySQL `10.0.7`, PostgreSQL `10.0.2`, Oracle `10.23.26200`, SQL Server `10.0.9` for `net10.0`.
- Repository evidence already settles the analyzer outcome: `DCoding.Data.DVault.Analyzers` remains one `net10.0` analyzer asset with a `.NET 10 SDK` build-host requirement for both coordinated consumer lines; current evidence does not prove pure `.NET 8 SDK` analyzer consumption.
- The current repo-visible consumer package lines are still `8.36.0` and `10.36.0`; no visible pack-script, verifier, README, or release-input evidence introduces `8.37.0` / `10.37.0`, so this ticket should document the visible baseline rather than invent a new consumer package version.
- No new child tickets, attachments, planning documents, or relation writes are justified in this refinement. Existing done tickets `06FBSBN23A20NX2K0YAXZ40ZGR`, `06FBSBW6HDT15D1KGVD7XBQXM8`, and `06FBSBWH9F415E12VRHRYQ2JJM` are prerequisite evidence, and live downstream ticket `06FBSBWW414TE19KZT14CB7Y3R` stays blocked on this baseline work.

### Scope In
- Update `README.md` so the current-baseline navigation and install/publication guidance point to `docs/releases/v0.37.0.md` for the dependency-line and analyzer-compatibility record, without leaving `v0.36.0` labeled as the current baseline where v0.37 guidance is expected.
- Update `CHANGELOG.md` so `v0.37.0 - Dependency Line and Analyzer Compatibility` becomes the current top-level release summary and `v0.36.0` becomes historical trail context.
- Update `docs/manual-nuget-publication.md` so the current manual publication baseline matches the settled `8.36.0` / `10.36.0` package lines, the exact target-specific dependency matrix, the analyzer `.NET 10 SDK` build-host boundary, and the current validation evidence story.
- Create `docs/releases/v0.37.0.md` as the authoritative current release record for the settled dependency-line policy, exact package matrix, analyzer compatibility outcome, carried-forward validation commands/evidence, and explicit non-goals.
- Keep the four in-scope current-baseline surfaces consistent with the already-landed project, test, verifier, and analyzer-audit evidence so no stale v0.36 dependency matrix remains where v0.37 guidance is expected.

### Scope Out
- Changing project `PackageReference` values, pack-script version lines, `PackageVerifier` logic, unit/integration tests, or analyzer asset targeting unless a direct contradiction is found in the named documentation surfaces.
- Reopening the dependency-line policy or analyzer compatibility decision already settled by done tickets `06FBSBN23A20NX2K0YAXZ40ZGR`, `06FBSBW6HDT15D1KGVD7XBQXM8`, and `06FBSBWH9F415E12VRHRYQ2JJM`.
- Updating `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/plans/shared-implementation-standards.md`, or downstream release-checklist ticket `06FBSBWW414TE19KZT14CB7Y3R` as primary delivery surfaces for this ticket.
- Inventing consumer package versions `8.37.0`, `10.37.0`, or `0.37.0` without separate repo-visible packaging evidence.
- Changing hash-key storage behavior, release automation, publication approval mechanics, or other product/runtime scope unrelated to the bounded current-baseline documentation pass.

## Acceptance Criteria
- `README.md`, `CHANGELOG.md`, `docs/manual-nuget-publication.md`, and new `docs/releases/v0.37.0.md` all present one consistent current-baseline story: planning label `v0.37.0`, consumer package lines `8.36.0` for `net8.0` / EF Core 8 and `10.36.0` for `net10.0` / EF Core 10, and no mixed-line consumer install or approval guidance.
- The v0.37 guidance records the exact current accepted dependency matrix from repo-visible evidence: `net8.0` uses EF/Relational `8.0.28`, DI.Abstractions `8.0.2`, DB2 `8.0.0.400`, SQLite `8.0.28`, MySQL `8.0.26`, PostgreSQL `8.0.11`, Oracle `8.23.26200`, SQL Server `8.0.28`; `net10.0` uses EF/Relational/DI.Abstractions `10.0.9`, DB2 `10.0.0.100`, SQLite `10.0.9`, MySQL `10.0.7`, PostgreSQL `10.0.2`, Oracle `10.23.26200`, SQL Server `10.0.9`.
- The v0.37 guidance explicitly carries forward the accepted analyzer compatibility boundary: `DCoding.Data.DVault.Analyzers` ships one `net10.0` analyzer asset, analyzer references stay local with `PrivateAssets=all`, and supported analyzer consumption for both coordinated consumer lines uses a `.NET 10 SDK` build host without claiming validated pure `.NET 8 SDK` analyzer consumption.
- The v0.37 release record and manual publication guidance point to the current validation evidence surfaces and commands already used in-repo: `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, `bash tools/pack-release-packages.sh`, `bash tools/verify-packages.sh`, and `bash tools/check-format.sh`, plus the analyzer audit and matrix/verifier evidence paths.
- No in-scope current-baseline surface leaves a stale `v0.36.0` dependency/analyzer baseline where the new `v0.37.0` record should be authoritative, while historical `v0.36.0` material may remain only as carried-forward background or release history.

## Definition of Done
- `docs/releases/v0.37.0.md` exists and is the current release record linked by the updated README and changelog surfaces for dependency-line/analyzer baseline guidance.
- All four in-scope docs agree on the same package lines, target-specific dependency matrix, analyzer build-host boundary, manual-publication posture, and validation evidence with no contradictory wording.
- The refined ticket leaves no PO-level ambiguity about consumer package versions: the visible baseline stays `8.36.0` / `10.36.0` unless a separate packaging change lands, and the ticket does not imply an unproved `8.37.0` / `10.37.0` line.
- Downstream checklist work in `06FBSBWW414TE19KZT14CB7Y3R` can consume the v0.37 baseline without reopening dependency policy or analyzer compatibility decisions.

## Implementation Notes
- Use `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`, `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj`, `tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj`, `tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs`, and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` as the authoritative dependency-matrix sources.
- Use `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/plans/analyzer-package-compatibility-audit.md`, `docs/local-validation.md`, and `.github/workflows/ci.yml` as the authoritative analyzer-host and validation-baseline sources.
- Treat done tickets `06FBSBN23A20NX2K0YAXZ40ZGR`, `06FBSBW6HDT15D1KGVD7XBQXM8`, and `06FBSBWH9F415E12VRHRYQ2JJM` as settled prerequisite evidence. This ticket should document their accepted outcomes, not reopen or duplicate their implementation scope.
- Inference from current repo state: because `README.md`, `docs/manual-nuget-publication.md`, `tools/pack-release-packages.sh`, and verifier expectations still expose only `8.36.0` / `10.36.0`, the safe bounded default is to document those visible consumer lines inside the `v0.37.0` planning baseline rather than inventing `8.37.0` / `10.37.0`.
- Keep relation context consistent with live state: outgoing `blocks` from this ticket to `06FBSBWW414TE19KZT14CB7Y3R` is the active downstream dependency, while incoming `blocks` from done ticket `06FBSBWH9F415E12VRHRYQ2JJM` is historical landed evidence and not a current blocker.

## Open Questions
- none

## Follow-Up Questions
- If release management later wants consumer package lines `8.37.0` / `10.37.0`, should that be a separate packaging/release ticket that updates pack-script, verifier, and install guidance together instead of being inferred here from the planning label alone?

## Risks
- `README.md` currently labels `v0.36.0` as the current baseline and uses v0.36-specific section wording, so a partial update could leave competing current-baseline signals between README and the new v0.37 release record.
- Because the planning label is `v0.37.0` but current repo-visible consumer lines are still `8.36.0` / `10.36.0`, careless documentation could wrongly invent `8.37.0` / `10.37.0` or a consumer-facing `0.37.0` package version.
- If the v0.37 docs omit the explicit `.NET 10 SDK` analyzer build-host boundary, they will overstate compatibility beyond what the repository actually proves for net8-target consumers.
- The downstream release-checklist ticket `06FBSBWW414TE19KZT14CB7Y3R` remains blocked until this current-baseline documentation work lands.

## Split Recommendations
- No new split. Keep existing done tickets as prerequisite evidence and keep `06FBSBWW414TE19KZT14CB7Y3R` as the downstream checklist follow-up that consumes this ticket's finalized baseline.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update README, CHANGELOG, manual NuGet publication guidance, and docs/releases/v0.37.0.md for the accepted dependency-line policy, latest package baseline, analyzer compatibility outcome, and validation evidence. Acceptance: no stale v0.36 dependency matrix remains where v0.37 guidance is expected.