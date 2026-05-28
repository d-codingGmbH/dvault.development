[gicket-bot] PO-critic review contract

Summary
- Contract is now source-backed and ready for developer handoff; prior PO-critic blockers were addressed and `## Open Questions` is `none`.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/description.md` has `PO Handoff` = `ready_for_po_critic` and `## Open Questions` = `- none`.
- `git diff --unified=80 1ce02b440..43e5b7168 -- .gicket/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/description.md` shows the contract was narrowed from inferred API/generator-snapshot claims to explicit sources: `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs`, `src/DCoding.Data.DVault.Analyzers/README.md`, `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs`, `docs/quality/api-surface-snapshots.md`, `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs`, six `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/*.approved.txt`, and `docs/plans/stable-hashing-contract.md`.
- Previous critic comment `.gicket/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/comments/06F6WF6YPQ6ZQ2NCXD15WQZN10.md` returned the ticket for inferred API/snapshot claims; PO reply `.gicket/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/comments/06F6WHGBH0P8Y8493B9Z4BH8N4.md` records critic-item-1/2/3 as answered.
- `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs` defines `build_property.DVaultGenerateTypedReadModels`, `build_property.DVaultTypedReadModelMetadataSourceFingerprint`, `dvault.support-bundle.v1`, and the diagnostic text requiring exactly one authoritative support-bundle input.
- `src/DCoding.Data.DVault.Analyzers/README.md:54-58` states satellite-only `Read...CurrentAsync`, `Read...LatestAsync`, and `Read...AsOfAsync` helpers from one authoritative `dvault.support-bundle.v1` file and explicitly says raw `dvault.model.v1` files are not parsed directly.
- `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:70-72,277,306,374,397,424,452,489` asserts the helper methods and diagnostics `DMV1960`, `DMV1961`, `DMV1963`, `DMV1964`, `DMV1966`, `DMV1967`, `DMV1968`, and `DMV1969`.
- `docs/quality/api-surface-snapshots.md` plus `ls tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi` shows six committed public API snapshot baselines only: core, Sqlite, Postgres, SqlServer, Oracle, and MySql.
- `find . -maxdepth 1 -name 'dvault.model.v1' -o -name 'dvault.support-bundle.v1' -o -name '*.support-bundle.json'` returned no repo-root artifact files, matching the contract's consumer-owned artifact wording.
- `git ls-files docs/releases/v0.22.0.md` returned no file, so the release note is correctly described as a file to create in development rather than existing baseline evidence.
- `README.md:556,559,826-833`, `docs/model-first-governance.md:240`, and `docs/production-adoption-checklist.md:36,70,74,112` already provide source-backed support-bundle workflow, manual ownership, and local validation command references.
- `git diff --name-only 236b73f1c..84bae6b2c -- . ':(exclude).gicket'` returned no non-ticket files, confirming this is still a pre-development ticket-contract branch rather than implemented doc changes.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- If `docs/releases/v0.22.0.md` describes a new 'current baseline', make sure it distinguishes documentation baseline from package-publication state so README install snippets do not accidentally imply unpublished `0.22.0` packages.
- If multi-active examples are added, keep them within the tested deterministic string driving-key/payload boundary rather than implying broader CLR/member support.

Risky assumptions
- Developers will treat the authoritative contract block as controlling and ignore conflicting legacy-draft wording such as the stale 'generator snapshots' phrase at the bottom of `.gicket/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/description.md`.
- The v0.22.0 documentation roll-forward will not introduce analyzer-package public API snapshot or dedicated generator approval-snapshot claims unless new evidence is added in a separate ticket.

AC / test suggestions
- Use `dotnet test DVault.slnx --nologo --filter FullyQualifiedName~ApiSurfaceSnapshotTests` to validate the cited public API evidence links.
- Use `dotnet test DVault.slnx --nologo --filter FullyQualifiedName~DataVaultTypedReadModelSourceGeneratorTests` to validate the cited generator-boundary evidence.
- Use `dotnet test DVault.slnx --nologo --filter FullyQualifiedName~StableHashServiceTests` plus README local validation commands to verify the hash-governance and validation references.

Implementation watchouts
- Keep `docs/releases/v0.22.0.md` limited to repo-visible evidence surfaces named in the contract; there is no current `docs/releases/v0.22.0.md`, analyzer-package API snapshot, or dedicated generator approval snapshot on this branch.
- Do not describe repo-root `dvault.model.v1` or `dvault.support-bundle.v1` files as checked-in baselines; the current branch shows those artifacts as consumer-owned workflow outputs.
- Keep typed-read docs satellite-only and avoid implying PIT/bridge helpers, provider-specific SQL generation, or dynamic-request compilation.
- Treat the current branch as pre-development: all non-ticket repository paths are unchanged since `236b73f1c`, so the developer still needs to create the docs changes and new release note.

Non-blocking notes
- The handoff commit sequence `43e5b7168 -> f8dcc3821 -> 84bae6b2c` is ticket-metadata only.

Split recommendations
- If the team later wants analyzer-package public API snapshot coverage or dedicated generator approval snapshots, keep that as a separate quality/evidence ticket.
- If PIT or bridge typed helpers become shipped behavior later, widen docs/release-note scope in a follow-up tied to that implementation ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment