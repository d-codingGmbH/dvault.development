[gicket-bot] PO-critic review contract

Summary
- Contract is bounded and internally consistent; repo evidence confirms the analyzer doc surface and there are no open PO questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The persisted delivery contract for `06F2PGJ28KVSZAAFRA40D94128` marks PO handoff as `ready_for_po_critic`, scopes the work to analyzer installation/configuration/suppression docs, and states `## Open Questions` = `none`.
- `repository-read-text` on `src/DCoding.Data.DVault.Analyzers/README.md` shows the primary doc already covers optional analyzer installation with `PrivateAssets="all"`, implemented ids `DMV1901`/`DMV1902`, and concrete suppression examples for `#pragma warning`, `.editorconfig`, and MSBuild `NoWarn`.
- `repository-read-text` on `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj` shows `<PackageReadmeFile>README.md</PackageReadmeFile>` and `<None Include="README.md" Pack="true" PackagePath="/" />`, so the packaged README is the shipping guidance surface.
- `repository-read-text` on `src/DCoding.Data.DVault.Analyzers/CodeFirstDiagnosticCatalog.cs` and `src/DCoding.Data.DVault.Analyzers/CodeFirstAnalyzerDiagnosticMetadata.cs` shows only `DMV1901` and `DMV1902`, both emitted as enabled-by-default `Warning` diagnostics.
- `repository-read-text` on `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs` asserts supported diagnostics `DMV1901` and `DMV1902` and covers unsupported selector shapes, duplicate members, valid direct scalar selectors, separate satellite scopes, and selector-variable false-positive guards.
- `repository-list-directory` on `docs/releases` returned release files through `docs/releases/v0.11.0.md` only; no `v0.12.0` release note is present, which matches the contract's scoping note that broader `v0.12.0` release-note closure belongs to `06F2PGJYY6S97B4Z8044D34K5C`.
- `shell-command` `git diff --name-status e5d3a25a83c0a52455cb463aa9978e488ace8296..ticket/06F2PGJ28KVSZAAFRA40D94128-task-document-analyzer-configuration-and-suppres -- src/DCoding.Data.DVault.Analyzers README.md docs tests` returned empty stdout, so no branch changes are currently present in the doc/test areas this task is expected to touch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Direct `gicket-read-ticket` and `gicket-read-ticket-comments` calls were trust-blocked (`BOT-LOCAL-TOOL-TRUST-BLOCKED`), so this review assumes the prompt snapshot reflects the latest persisted ticket state and comment history.
- The current repository examples still show analyzer package version `0.11.0`; the contract assumes the merge-time coordinated release version will be known and substituted without widening this task into repo-wide release-note work.
- If sibling ticket `06F2PGHWEWYJZSRQ9QPT4NJ0QM` lands more diagnostics before this task merges, the developer will need to keep this ticket constrained to the implemented rule slice at merge time.

AC / test suggestions
- Completion evidence should explicitly show that any touched documentation still matches `DMV1901` and `DMV1902` as asserted by `DataVaultCodeFirstAnalyzerTests`.
- If a broader doc is touched for consistency, validation should show it points back to `src/DCoding.Data.DVault.Analyzers/README.md` instead of restating a second suppression contract.

Implementation watchouts
- Keep the ticket package-local: `src/DCoding.Data.DVault.Analyzers/README.md` is the primary artifact and the `.csproj` already packs it as the package README.
- Do not document diagnostics, code fixes, or suppression mechanisms beyond the currently implemented Roslyn surface: `DMV1901`, `DMV1902`, `#pragma warning`, `.editorconfig`, and `NoWarn`.
- Because the branch diff is currently empty in `src/DCoding.Data.DVault.Analyzers`, `README.md`, `docs`, and `tests`, developers should expect to produce the actual documentation updates from scratch on this ticket branch.

Non-blocking notes
- The contract already narrows related-ticket handling: the done epic relation is historical context, and no child-ticket or relation changes were created during refinement.

Split recommendations
- No split recommended; the contract already bounds this as a focused analyzer-documentation task under story `06F2PGHQ2GATEM13M5QK1MSX1G`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment