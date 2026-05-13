[gicket-bot] PO-critic review contract

Summary
- Child-story implementation and validation evidence is present, but the epic's required lifecycle-guardrails release documentation is not in the repository, so the closure contract is not yet supported by direct repo evidence.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/description.md` requires release documentation that follows the existing `docs/releases/v0.x.0.md` pattern and says the lifecycle-guardrails release note should be added as part of epic closure; `## Open Questions` is `none`.
- `git -C /mnt/c/Projects/DVault ls-files 'docs/releases/*'` returned only `docs/releases/v0.5.0.md`, `docs/releases/v0.6.0.md`, and `docs/releases/v0.7.0.md`.
- `git -C /mnt/c/Projects/DVault diff --name-status develop...HEAD` showed only `.gicket/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/*` ticket metadata/comment changes on the epic branch, with no `docs/releases` or other repository documentation files changed.
- The four child tickets are directly marked `done` in `.gicket/tickets/06F1XPS7KGKBP5SVMQPJC49J2G/ticket.json`, `.gicket/tickets/06F1XPTCGWTJHHQVNPN13KANMG/ticket.json`, `.gicket/tickets/06F1XPVPKVGYKCV04PY98TSS78/ticket.json`, and `.gicket/tickets/06F1XPWB8DZR4J8EZ00V8DT25G/ticket.json`.
- `.gicket/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/attachments/manifest.json` lists attachment `v0.8.0-lifecycle-guardrails-plan.md`; the attached blob content orders the slice as diagnostic catalog, migration guardrails, design-time services, then ModelSnapshot and optional live schema drift.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` documents the consumer-owned single-project `IDesignTimeDbContextFactory<TContext>` workflow, explicit diagnostics and migration guardrail preflight, and explicitly says DVault does not provide `IDesignTimeServices` or a custom `dotnet ef` shim.
- `docs/model-first-governance.md` documents `DataVaultModelDriftReporter.Compare` as metadata-only drift evidence and a separate SQLite-first live-schema path via `DataVaultLiveSchemaReader.ReadAsync` and `DataVaultLiveSchemaDriftReporter.Compare`.
- Local validation surfaces already exist in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDotnetEfDesignTimeWorkflowTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/SqliteLiveSchemaDriftTests.cs`.

Blocking findings
- The epic contract requires release documentation for the lifecycle-guardrails slice, but the repository still has release notes only through `docs/releases/v0.7.0.md`; there is no observed `v0.8.0` lifecycle-guardrails release note or equivalent repo release-summary file.
- This closure-only epic branch does not materialize the promised repository documentation work: the branch diff from `develop` contains only `.gicket` ticket updates, so approval would depend on future documentation work rather than current repository evidence.

Required PO actions
- Return the ticket to PO refinement and explicitly track the missing release-documentation deliverable before rerouting; the current closure contract is not yet backed by repository evidence.
- Either keep the epic open until a repo release-summary document is present for the lifecycle-guardrails slice, or revise the parent contract and create a docs-only follow-up ticket if release-note work is intentionally being deferred elsewhere.
- Update PO handoff text so it does not present epic closure as ready while the release-documentation acceptance criterion is still unmet.

Open issues ledger
- critic-item-1 [required-po-action] Return the ticket to PO refinement and explicitly track the missing release-documentation deliverable before rerouting; the current closure contract is not yet backed by repository evidence.
- critic-item-2 [required-po-action] Either keep the epic open until a repo release-summary document is present for the lifecycle-guardrails slice, or revise the parent contract and create a docs-only follow-up ticket if release-note work is intentionally being deferred elsewhere.
- critic-item-3 [required-po-action] Update PO handoff text so it does not present epic closure as ready while the release-documentation acceptance criterion is still unmet.
- critic-item-4 [blocking-finding] The epic contract requires release documentation for the lifecycle-guardrails slice, but the repository still has release notes only through `docs/releases/v0.7.0.md`; there is no observed `v0.8.0` lifecycle-guardrails release note or equivalent repo release-summary file.
- critic-item-5 [blocking-finding] This closure-only epic branch does not materialize the promised repository documentation work: the branch diff from `develop` contains only `.gicket` ticket updates, so approval would depend on future documentation work rather than current repository evidence.

Missing examples / edge cases
- The missing release summary should show the intended lifecycle order end to end: model validation, migration scaffolding, migration guardrail preflight, then the decision to run `dotnet ef database update`.
- The missing release summary should explicitly distinguish metadata-only `DataVaultModelDriftReporter.Compare` from optional SQLite-first live-schema evidence via `DataVaultLiveSchemaReader.ReadAsync`.

Risky assumptions
- Assuming the existing architecture and governance docs satisfy the separate release-documentation acceptance criterion even though no `v0.8.0` release-summary file exists in `docs/releases`.
- Assuming the epic can close solely because the four child stories are `done`, despite the parent contract still requiring closure-only documentation evidence in the repository.
- Assuming release operators will backfill the lifecycle-guardrails summary later without a tracked ticket or an updated parent contract.

AC / test suggestions
- When the release-summary gap is addressed, cite the existing validation evidence already in repo: `DataVaultMigrationOperationDiagnosticsTests`, `DataVaultDotnetEfDesignTimeWorkflowTests`, `DataVaultModelFirstDesignTimeWorkflowTests`, and `SqliteLiveSchemaDriftTests`.
- Keep acceptance language tied to the directly observed public/documented surfaces: `DMV####`, `DVM2001` through `DVM2006`, consumer-owned single-project design-time preflight, and SQLite-first optional live-schema evidence.

Implementation watchouts
- Do not let the release summary imply DVault-owned `IDesignTimeServices`, a custom `dotnet ef` shim, or provider-specific migration runners; the architecture note explicitly rejects those promises.
- Keep ModelSnapshot drift guidance separate from live-schema checks; the design-time workflow doc treats live database checks as outside the preflight workflow itself.
- Preserve the attached plan sequence and do not reopen diagnostic naming or migration-guardrail API baselines already fixed by the done child stories.

Non-blocking notes
- The parent contract's `## Open Questions` section is resolved as `none`, so the return is driven by unmet closure evidence rather than unresolved scope questions.
- The repository already contains the underlying workflow, drift, diagnostics, and SQLite live-schema validation surfaces; the current gap is release-summary closure evidence, not missing child-story implementation.

Split recommendations
- If Product wants to close the tracking epic without waiting for the release-summary deliverable, split a small docs-only follow-up and revise the parent closure criteria before rerouting.
- If Product keeps the current closure contract, no additional technical split is needed; the remaining work is documentation evidence aligned to release closure.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment