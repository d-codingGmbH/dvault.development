[gicket-bot] PO-critic review contract

Summary
- Ticket 06FBSBWW414TE19KZT14CB7Y3R is ready for developer handoff: the persisted contract has no open questions, the repository already contains a consistent v0.37.0 checklist and validation baseline, and the only issue I found is a non-blocking stale relation sentence in the contract.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSBWW414TE19KZT14CB7Y3R/description.md` contains PO handoff `ready_for_po_critic` and `## Open Questions` set to `none`.
- Ticket comments `comments/06FC1ED4C1FT9E9MJT54BAX8N4.md`, `comments/06FC1EHS1DT51MV1NF7QSK31QG.md`, and `comments/06FC1ER79R6W0TKFGPCNJWAS50.md` record the PO refinement contract, the `handover-po-critic` runtime orchestration template, and the `po-refinement-ready` run report.
- `docs/releases/v0.37.0.md` states `v0.37.0` is a planning label, lists the eight-package family, forbids consumer-facing `0.37.0`, `8.37.0`, and `10.37.0`, and records the two visible lines `8.36.0/net8.0/EF Core 8` and `10.36.0/net10.0/EF Core 10`.
- `README.md` and `docs/manual-nuget-publication.md` both require separate `8.36.0` and `10.36.0` lines, local analyzer references with `PrivateAssets` set to `all`, and a `.NET 10 SDK` host for analyzer consumers.
- `CHANGELOG.md` and `docs/local-validation.md` align on the same v0.37.0 baseline and the five validation commands: `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, `bash tools/pack-release-packages.sh`, `bash tools/verify-packages.sh`, and `bash tools/check-format.sh`.
- `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` targets only `net10.0`, and `src/DCoding.Data.DVault.Analyzers/README.md` says both coordinated package lines require a `.NET 10 SDK` host and do not validate pure `.NET 8 SDK` analyzer consumption.
- `tools/pack-release-packages.sh` packs `8.36.0` for `net8.0` and `10.36.0` for `net10.0`; `tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs` and `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` enforce the same dependency matrix and README/analyzer guidance.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No explicit example final approval record picks which package line is approved first; later release closure should keep `8.36.0` and `10.36.0` as separate approval records, as already required by `docs/manual-nuget-publication.md`.

Risky assumptions
- The delivery contract sentence that `06FBSBWPN112S4CGP0239K0ZT8` currently blocks this ticket is stale against current local state: that related ticket is now `done` and this ticket is `is-blocked: false`.
- Developer handoff assumes the existing repository documentation baseline is the intended implementation surface for this ticket, because the current ticket branch differs from `develop` only in `.gicket` ticket metadata.

AC / test suggestions
- Keep the handoff acceptance check tied to the five documented commands and the five documented surfaces: `docs/releases/v0.37.0.md`, `README.md`, `docs/manual-nuget-publication.md`, `docs/local-validation.md`, and `CHANGELOG.md`.
- Preserve a negative check that no doc or packaged README introduces consumer-facing `0.37.0`, `8.37.0`, or `10.37.0` versions or mixes `8.36.0` and `10.36.0` in one install or approval example.
- Keep the analyzer boundary explicit in future verification: one `net10.0` analyzer asset, local `PrivateAssets` set to `all`, and `.NET 10 SDK` build-host guidance only.

Implementation watchouts
- Current branch history for this ticket (`e2479604e`, `5b3fd0f14`, `6d155d564`) is ticket metadata only; if the developer expects repository documentation edits on this branch, that expectation is stale.
- Do not reopen the analyzer compatibility boundary during dev handoff: multiple repository surfaces and the analyzer csproj/package verifier all align on `.NET 10 SDK` host support only, not pure `.NET 8 SDK` analyzer consumption.

Non-blocking notes
- none

Split recommendations
- No split recommended; the persisted contract already scopes this to documentation and checklist ratification and the repository evidence is bounded and consistent.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment