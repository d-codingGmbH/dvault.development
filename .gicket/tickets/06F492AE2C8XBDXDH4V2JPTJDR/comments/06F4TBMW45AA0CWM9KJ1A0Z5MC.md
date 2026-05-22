[gicket-bot] PO-critic review contract

Summary
- The delivery contract is now internally consistent, grounded in repo-local IReadOnlyModel drift APIs, and ready for developer handoff; remaining gaps are implementation watchouts and follow-on docs/command work, not PO blockers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F492AE2C8XBDXDH4V2JPTJDR/description.md` defines one authoritative snapshot input as a consumer-materialized snapshot-model `IReadOnlyModel` in PO Summary, Scope In, Acceptance Criteria, and Implementation Notes, and `## Open Questions` is `- none`.
- `.gicket/tickets/06F492AE2C8XBDXDH4V2JPTJDR/comments/06F4QKBMV08PJBPWY3XDD3AZSM.md` records the latest PO refinement and marks prior critic items 1-4 as answered, including keeping EF `ModelSnapshot` and `Microsoft.EntityFrameworkCore.Design` outside the `src/DCoding.Data.DVault` public contract.
- `src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:20-101` already exposes `Compare(...)` overloads from `DataVaultMetadataModel` and `DataVaultModelImportResult` to `IReadOnlyModel`, while the existing `DbContext` overloads still compare against `currentContext.GetService<IDesignTimeModel>().Model`.
- `src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs:<redacted>` defines a private internal `ModelSnapshot` record for the comparer, so the repo-local drift engine is already normalized around DVault-owned snapshots rather than a public EF `ModelSnapshot` input type.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj:28-29` references only `Microsoft.EntityFrameworkCore` and `Microsoft.EntityFrameworkCore.Relational`; there is no `Microsoft.EntityFrameworkCore.Design` package reference in the core package.
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs:39-47` reads `compiledContext.Model` and asserts DVault annotations on the runtime model, which is direct repo evidence for the additive runtime-model lane.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:388-398` still lists model snapshot drift comparison as unsupported in v1, which matches this ticket's purpose as new additive work and the contract's explicit follow-on split for docs/command rollout.
- `rg --files /mnt/c/Projects/DVault -g '*ModelSnapshot.cs' -g '!**/bin/**' -g '!**/obj/**'` returned no matches, so the contract is correctly explicit that DVault must not rely on repo-owned snapshot files or discovery heuristics.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A narrow consumer example for materializing a snapshot-model `IReadOnlyModel` before calling the new preflight would reduce developer guesswork, but the contract is already clear enough for implementation.
- Tests should pin expected behavior when the configured `DbContext` provider/profile and the consumer-materialized snapshot-model provider/profile do not match.
- Tests should pin whether runtime-versus-snapshot drift that is caused by consumer setup differences is surfaced as ordinary drift findings or as a distinct setup/configuration failure.

Risky assumptions
- Consumers will materialize the snapshot-model `IReadOnlyModel` under the same provider/profile and equivalent metadata-source conditions as the configured `DbContext`.
- The new composite preflight can reuse existing drift finding vocabulary and severities for all three pairwise sections without creating ambiguous duplicate findings.
- Follow-on tickets `06F492BG6BZYYFMBE5WK7CB024` and `06F492BNDPWS9P4EDSV0W7G6VM` will absorb command-surface and broad documentation changes, so this story stays library-local.

AC / test suggestions
- Cover both expected-model authorities (`DataVaultMetadataModel` and `DataVaultModelImportResult`) across metadata-versus-runtime, metadata-versus-snapshot-model, and runtime-versus-snapshot-model sections.
- Add deterministic no-difference cases where runtime and snapshot-model surfaces match exactly, plus drift cases for entity, property, key, index, provider-profile, and metadata-source mismatches.
- Add backward-compatibility coverage that `DataVaultModelDriftReporter.Compare(..., DbContext)` remains design-time over `IDesignTimeModel` and that `DataVaultDesignTimeCommand drift --artifact` keeps its current artifact-based behavior.
- Add explicit negative coverage for provider/profile mismatch or mismatched model-cache/materialization inputs producing stable blocking or informational findings.

Implementation watchouts
- Use `DbContext.Model` only for the new runtime lane; do not silently redefine existing `Compare(..., DbContext)` overloads away from `IDesignTimeModel` semantics.
- Keep `src/DCoding.Data.DVault` design-package-free and avoid public EF `ModelSnapshot` inputs, repo scanning, fixed snapshot paths, or automatic migration discovery.
- Because the repo has no checked-in `*ModelSnapshot.cs`, tests and examples must materialize snapshot-model input explicitly instead of discovering migration snapshot files.
- Do not fold command-host aggregation or orchestration UX into this story; that remains with `06F492BG6BZYYFMBE5WK7CB024`.

Non-blocking notes
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` still documents snapshot drift as unsupported in v1; that is consistent with the story being new work and with broader docs/release-note updates already split out.

Split recommendations
- No additional split is recommended; keep command aggregation on `06F492BG6BZYYFMBE5WK7CB024` and broader documentation/release-note rollout on `06F492BNDPWS9P4EDSV0W7G6VM`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment