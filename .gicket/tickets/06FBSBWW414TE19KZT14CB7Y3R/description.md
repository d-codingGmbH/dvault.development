<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence already satisfies refinement for the v0.37 release checklist and validation note: the current docs ratify the dual package lines, exact dependency matrix, net10 analyzer host boundary, and required validation commands, so the ticket is ready for PO critic without additional planning writes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already ratifies `docs/releases/v0.37.0.md` as the authoritative v0.37 release-note and validation-note surface for this ticket.
- `docs/manual-nuget-publication.md`, `docs/local-validation.md`, `README.md`, and `CHANGELOG.md` align with that release record on package-line separation, analyzer guidance, and validation evidence.
- No bounded planning writes were applied in this run: no child tickets, relation changes, description updates, attachments, or planning documents were materialized.

### Scope In
- Document the coordinated eight-package v0.37.0 documentation baseline for `DCoding.Data.DVault`, `DCoding.Data.DVault.Analyzers`, `DCoding.Data.DVault.Db2`, `DCoding.Data.DVault.MySql`, `DCoding.Data.DVault.Oracle`, `DCoding.Data.DVault.Postgres`, `DCoding.Data.DVault.Sqlite`, and `DCoding.Data.DVault.SqlServer`.
- Ratify the two visible consumer package-version lines: `8.36.0` for `net8.0` / EF Core 8 and `10.36.0` for `net10.0` / EF Core 10.
- Record the accepted analyzer compatibility boundary, required validation commands, publication checklist expectations, and known limitations that future release closure must reuse.

### Scope Out
- Publishing packages, recording final publish approval, package hashes, signing evidence, or release automation changes.
- Changing runtime behavior, project `PackageReference` values, provider pins, package verifier logic, tests, or pack-script package versions.
- Claiming pure `.NET 8 SDK` analyzer consumption or retargeting the analyzer package away from its current `net10.0` build-host baseline.

## Acceptance Criteria
- The release-note material states that `v0.37.0` is a planning/release-note label, not a consumer NuGet version, and explicitly forbids consumer-facing `0.37.0`, `8.37.0`, or `10.37.0` versions for this baseline.
- The release-note material records the exact dual package lines and target-specific dependency matrix already enforced by repository tests and package verification.
- The release-note material states that `DCoding.Data.DVault.Analyzers` remains one `net10.0` analyzer asset for both package lines, requires `PrivateAssets="all"`, and is supported on a `.NET 10 SDK` build host for both lines.
- The release closure guidance includes the five required validation commands: `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, `bash tools/pack-release-packages.sh`, `bash tools/verify-packages.sh`, and `bash tools/check-format.sh`.
- The release closure guidance states the known limits: no mixed package lines in one consumer example or approval, no pure `.NET 8 SDK` analyzer compatibility claim, and no package publication evidence inside the release-note artifact itself.

## Definition of Done
- The current repository release surfaces (`docs/releases/v0.37.0.md`, `docs/manual-nuget-publication.md`, `docs/local-validation.md`, `README.md`, and `CHANGELOG.md`) tell one consistent v0.37 story for package scope, version lines, analyzer boundary, validation, and non-goals.
- The exact dependency matrix is ratified against existing repository enforcement, including `tools/pack-release-packages.sh`, `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj`, and `tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs`.
- A release operator can use the documented checklist and validation commands for later closure without reopening package-line, analyzer-host, or limitation decisions in this ticket.

## Implementation Notes
- Visible dependency baseline for `8.36.0` / `net8.0`: `Microsoft.EntityFrameworkCore` `8.0.28`, `Microsoft.EntityFrameworkCore.Relational` `8.0.28`, `Microsoft.Extensions.DependencyInjection.Abstractions` `8.0.2`, `IBM.EntityFrameworkCore` `8.0.0.400`, `Microsoft.EntityFrameworkCore.Sqlite` `8.0.28`, `MySql.EntityFrameworkCore` `8.0.26`, `Npgsql.EntityFrameworkCore.PostgreSQL` `8.0.11`, `Oracle.EntityFrameworkCore` `8.23.26200`, `Microsoft.EntityFrameworkCore.SqlServer` `8.0.28`.
- Visible dependency baseline for `10.36.0` / `net10.0`: `Microsoft.EntityFrameworkCore` `10.0.9`, `Microsoft.EntityFrameworkCore.Relational` `10.0.9`, `Microsoft.Extensions.DependencyInjection.Abstractions` `10.0.9`, `IBM.EntityFrameworkCore` `10.0.0.100`, `Microsoft.EntityFrameworkCore.Sqlite` `10.0.9`, `MySql.EntityFrameworkCore` `10.0.7`, `Npgsql.EntityFrameworkCore.PostgreSQL` `10.0.2`, `Oracle.EntityFrameworkCore` `10.23.26200`, `Microsoft.EntityFrameworkCore.SqlServer` `10.0.9`.
- The analyzer package is packed once per version line but still targets only `net10.0` and ships under `analyzers/dotnet/cs/`; the accepted claim is `net8.0` or `net10.0` consumer projects compiled on a `.NET 10 SDK` host, not a pure `.NET 8 SDK` analyzer baseline.
- Live relation evidence was reviewed without mutation: ticket `06FBSBWPN112S4CGP0239K0ZT8` currently blocks this ticket, and this ticket currently blocks `06FBSBZRR9DP7YTR1ZZA3N6ANG`.

## Open Questions
- none

## Follow-Up Questions
- When release closure happens, which package-version line is approved first: `8.36.0` / `net8.0` / EF Core 8 or `10.36.0` / `net10.0` / EF Core 10? The checklist already requires separate approvals.
- If product requirements later expand to `net8.0` consumer projects compiling analyzers on a pure `.NET 8 SDK` host, should that ship as a separate compatibility ticket with analyzer retargeting and a new verification lane?
- At delivery closure, confirm whether the current `blocks` chain with `06FBSBWPN112S4CGP0239K0ZT8` and `06FBSBZRR9DP7YTR1ZZA3N6ANG` still reflects execution order or needs cleanup.

## Risks
- Manual release closure still depends on rerunning the five required validation commands against the selected package line and recording the final approval record before any package push.
- Because `DCoding.Data.DVault.Analyzers` stays on a single `net10.0` asset, any downstream assumption of pure `.NET 8 SDK` analyzer support would overstate what the repository currently validates.

## Split Recommendations
- No split recommended; current scope is already bounded and evidenced by the existing repository release documentation.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Prepare the small v0.37 release checklist content that future release closure can use: selected package lines, dependency versions, analyzer compatibility decision, verification commands, and known limitations. Acceptance: release-note material is ready before v0.38 feature work starts.