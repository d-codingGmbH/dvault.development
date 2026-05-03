[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff; the delivery contract is concrete, repository-backed, scoped to documentation only, and the persisted Open Questions section is none.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `README.md` explicitly states DVault is currently consumed from source and that live NuGet install commands wait until publication; its Local Validation section lists `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, `dotnet pack DVault.slnx --configuration Release --nologo`, `bash tools/verify-packages.sh`, and `bash tools/check-format.sh`.
- `rg -n` under `/mnt/c/Projects/DVault/src` found `<PackageId>` entries for `DCoding.Data.DVault`, `DCoding.Data.DVault.MySql`, `DCoding.Data.DVault.Oracle`, `DCoding.Data.DVault.Postgres`, `DCoding.Data.DVault.Sqlite`, and `DCoding.Data.DVault.SqlServer`; `src/DCoding.Data/DCoding.Data.csproj:6` has `<IsPackable>false</IsPackable>`.
- The same `rg -n` found each provider project referencing `../DCoding.Data.DVault/DCoding.Data.DVault.csproj`; `tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs` defines the expected six-package set and emits dependency-alignment failures when a provider package does not depend on `DCoding.Data.DVault` or uses a mismatched packed core version.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract says the release stops immediately on any push failure, but the resulting checklist should still spell out the operator-facing escalation note for the case where the core package or an earlier provider package has already been published before a later push fails.
- The contract requires release-note or changelog preparation and review, but it does not give an example of the minimum auditable content; the implementation will need to choose a lightweight format.

Risky assumptions
- Repo search in `docs/`, `README.md`, and `tools/` did not surface an existing changelog or release-governance document, so the developer will need to choose where release-note and approval evidence is recorded.
- The required provider publish order is a product-policy constraint from the contract, not something derived from source dependencies; the implementation should preserve that exact order rather than infer a different one from project layout.

AC / test suggestions
- When reviewing the resulting documentation, explicitly verify that it names all six packable package IDs and excludes `src/DCoding.Data` from publication scope.
- Verify that dependency alignment is tied to packed-artifact verification via `bash tools/verify-packages.sh` and the existing package verifier, not only to source-level project references.
- Verify that pre-publication guidance keeps current source/project-reference installation instructions intact and does not introduce live `dotnet add package` examples or version strings.

Implementation watchouts
- This is one coordinated manual release checklist, not a generic release-process rewrite and not per-package publication documentation.
- The documentation should reuse the existing validation commands and package-verification gate named in `README.md` instead of inventing new checks or alternate command sequences.
- `PackageVerifier.cs` already encodes six-package artifact count, symbol-package presence, README/XML metadata, and provider dependency alignment; the doc should point maintainers at that existing gate rather than restating ad hoc rules.

Non-blocking notes
- Ticket comments `06EYZ2SF7QRK2D6GZ5MY57DEN4.md` and `06EYZ2TMJMJGE16S053X2268GW.md` match the persisted contract and do not introduce unresolved PO questions.
- Repository documentation standards are already anchored by `docs/formatting.md` and `docs/plans/shared-implementation-standards.md`, so the deliverable has a clear repo-local documentation context without needing extra planning artifacts.

Split recommendations
- No split recommended; the persisted contract already constrains this to one bounded documentation deliverable for the coordinated six-package release flow.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment