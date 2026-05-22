[gicket-bot] PO-critic review contract

Summary
- Scope and intent are mostly well-bounded, but the ticket still leaves the authoritative snapshot input contract ambiguous and does not anchor that boundary to repo-local EF API/package evidence, so it should return to PO before dev handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F492AE2C8XBDXDH4V2JPTJDR/description.md` contains a full delivery contract with `## Open Questions` set to `none`, 6 acceptance-criteria bullets, 4 Definition-of-Done bullets, and 5 implementation notes.
- `src/DCoding.Data.DVault/DataVaultModelDriftReporter.cs` already exposes compare overloads from `DataVaultMetadataModel` and `DataVaultModelImportResult` to `IReadOnlyModel`, and the `DbContext` overloads route through `currentContext.GetService<IDesignTimeModel>().Model` rather than `DbContext.Model`.
- `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs` currently supports `validate`, `export`, `support-bundle`, `drift`, and `guardrail`; the `drift` path requires `--artifact`/`<path>` and then calls `DataVaultModelDriftReporter.Compare(importResult, dbContext)`, so the present command surface is artifact-based only.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` lists `model snapshot drift comparison` under `## Unsupported In V1`.
- `docs/releases/v0.8.0.md` says the non-live drift lane is metadata-based and currently supports `ModelSnapshot-style evidence where the current EF model is constructed for comparison`, which stops short of a first-class snapshot input API.
- `find /mnt/c/Projects/DVault -type f -name '*ModelSnapshot.cs' -not -path '*/bin/*' -not -path '*/obj/*'` returned no results; broader snapshot-name search only found `src/DCoding.Data.DVault/DataVaultLiveSchemaSnapshot.cs` and `src/DCoding.Data.DVault/DataVaultPitSatelliteSnapshot.cs`.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` references `Microsoft.EntityFrameworkCore` and `Microsoft.EntityFrameworkCore.Relational` 10.0.8 and no `Microsoft.EntityFrameworkCore.Design`; repo search found no direct source use of EF's public `ModelSnapshot` type.
- `git diff --name-only develop...HEAD` shows only `.gicket/tickets/06F492AE2C8XBDXDH4V2JPTJDR/**` changes on this branch, so there is no implementation evidence yet beyond ticket metadata/comments, which is acceptable for a pre-dev review but confirms this decision is ticket-contract-driven.

Blocking findings
- The contract requires a consumer-supplied `ModelSnapshot` input while also keeping `src/DCoding.Data.DVault` design-package-free, but the repo does not provide direct evidence for the exact EF public type/package boundary that the new API may depend on. Because this story's feasibility depends on an existing external API/type, the ticket needs that boundary grounded before dev handoff.
- The delivery contract mixes multiple snapshot-input shapes: acceptance criteria say explicit `ModelSnapshot` input, clarification text refers to `snapshot type or instance`, and implementation notes talk about a `ModelSnapshot`-materialized model. That leaves developers to discover the real public contract during implementation instead of receiving one authoritative boundary from the ticket.

Required PO actions
- Amend the delivery contract to name one authoritative snapshot input boundary for the additive API: actual `ModelSnapshot` instance, generated snapshot-derived type, or consumer-materialized `IReadOnlyModel`; remove the conflicting alternatives.
- Add repo-local evidence or an explicit package-boundary statement that proves the chosen snapshot input can be supported from `src/DCoding.Data.DVault` without adding `Microsoft.EntityFrameworkCore.Design`. If the intended boundary is really a materialized model instead of the EF snapshot type, say that explicitly in Scope In, acceptance criteria, and implementation notes.

Open issues ledger
- critic-item-1 [required-po-action] Amend the delivery contract to name one authoritative snapshot input boundary for the additive API: actual `ModelSnapshot` instance, generated snapshot-derived type, or consumer-materialized `IReadOnlyModel`; remove the conflicting alternatives.
- critic-item-2 [required-po-action] Add repo-local evidence or an explicit package-boundary statement that proves the chosen snapshot input can be supported from `src/DCoding.Data.DVault` without adding `Microsoft.EntityFrameworkCore.Design`. If the intended boundary is really a materialized model instead of the EF snapshot type, say that explicitly in Scope In, acceptance criteria, and implementation notes.
- critic-item-3 [blocking-finding] The contract requires a consumer-supplied `ModelSnapshot` input while also keeping `src/DCoding.Data.DVault` design-package-free, but the repo does not provide direct evidence for the exact EF public type/package boundary that the new API may depend on. Because this story's feasibility depends on an existing external API/type, the ticket needs that boundary grounded before dev handoff.
- critic-item-4 [blocking-finding] The delivery contract mixes multiple snapshot-input shapes: acceptance criteria say explicit `ModelSnapshot` input, clarification text refers to `snapshot type or instance`, and implementation notes talk about a `ModelSnapshot`-materialized model. That leaves developers to discover the real public contract during implementation instead of receiving one authoritative boundary from the ticket.

Missing examples / edge cases
- A concrete single-project consumer example showing how snapshot input is supplied when this repo has no checked-in `*ModelSnapshot.cs` files.
- Expected behavior when the configured `DbContext` provider/profile and the snapshot provider/profile do not match.
- Expected behavior when a consumer does not ship migrations or cannot supply a snapshot at all: unsupported, skipped, or out of scope.
- Whether differences between `DbContext.Model` and the design-time model should be reported as ordinary drift or treated as a setup/configuration error.

Risky assumptions
- Assumes EF's public `ModelSnapshot` type is usable from the packages already referenced by `src/DCoding.Data.DVault`.
- Assumes snapshot materialization can remain consumer-owned without forcing design-time-only dependencies into the core package.
- Assumes provider/profile selection from the configured context is sufficient for all three lanes and will not create false positives when the snapshot was built differently.

AC / test suggestions
- Add an acceptance criterion or explicit test note that names the accepted snapshot input form and proves the core package remains free of `Microsoft.EntityFrameworkCore.Design`.
- Add explicit matching and drifted coverage for both expected-model authorities (`DataVaultMetadataModel` and `DataVaultModelImportResult`) across metadata-vs-runtime, metadata-vs-snapshot, and runtime-vs-snapshot lanes.
- Add backward-compatibility coverage that `DataVaultDesignTimeCommand drift --artifact` and `DataVaultModelDriftReporter.Compare(..., DbContext)` keep today's design-time semantics.
- Add a negative test for provider/profile mismatch between `DbContext` and snapshot materialization.

Implementation watchouts
- Current `DataVaultModelDriftReporter.Compare(..., DbContext)` uses `IDesignTimeModel`; do not silently repurpose that existing API to `DbContext.Model`.
- Current `DataVaultDesignTimeCommand` drift flow is artifact-only, and command aggregation remains split to ticket `06F492BG6BZYYFMBE5WK7CB024`.
- The repo currently has no checked-in `*ModelSnapshot.cs`; implementation should not drift into repo scanning or migration-discovery heuristics.

Non-blocking notes
- The clarification saying the only stored comment was the claim template is stale against the current repo snapshot; `.gicket/tickets/06F492AE2C8XBDXDH4V2JPTJDR/comments/` now contains multiple bot workflow comments plus the refinement contract comment.

Split recommendations
- Keep command aggregation and broad documentation on tickets `06F492BG6BZYYFMBE5WK7CB024` and `06F492BNDPWS9P4EDSV0W7G6VM`; no further split is needed once the snapshot-input contract is clarified.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment