<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Bounded this ticket to adopter-facing GitHub Actions examples for running `validate`, model drift, and migration guardrail checks through the consumer-owned `DataVaultDesignTimeCommand` surface before integration; no child tickets, relation writes, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already exposes the reusable command surface `DataVaultDesignTimeCommand`, `DataVaultDesignTimeCommandHost`, and `DataVaultDesignTimeExportSource`, so this ticket documents how adopters run those commands in CI rather than extending the core API.
- The supported v1 design-time boundary stays single-project and consumer-owned: the project that owns the configured `DbContext` also owns `IDesignTimeDbContextFactory<TContext>`, the command host entrypoint, migration resolution, and the `dotnet ef` invocation point.
- GitHub Actions YAML is the bounded v1 example format because `.github/workflows/ci.yml` is the only visible repository workflow baseline; alternate CI systems are follow-up work, not an open refinement blocker.
- Artifact-versus-design-time-model drift is the default CI lane; `--live-schema` remains optional and SQLite-first or external-opt-in because broader providers are still documented as unsupported or unavailable first-class live drift readers.
- No child tickets, relation updates, or planning documents were materialized in this pass; live relation state still includes historical incoming `blocks` links from done tickets `06F2PGGJQMKH2T5948VJH93M5R` and `06F2PGFZWC5PXSDH46RCZPN1CG`.

### Scope In
- Add focused adopter-facing CI workflow examples that use the existing consumer-owned design-time command surface for pre-integration checks.
- Show one GitHub Actions baseline with concrete rerunnable commands for `validate`, `drift` when a reviewed artifact exists, and `guardrail` after scaffolding a migration.
- Keep the examples aligned with the current single-project consumer layout documented in `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`.
- Add or update the narrow design-time/example docs needed so adopters can discover the workflow without waiting for the broader v0.11.0 README/release-note rollout.

### Scope Out
- No new DVault command verbs, no changes to `DataVaultDesignTimeCommand` public API, and no new packable or standalone CLI project.
- No repo-owned CI workflow expansion, release automation, secret management, or default-on external provider jobs.
- No broad root README or v0.11.0 release-note rollout; that remains in `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- No default live-schema CI lane for PostgreSQL, SQL Server, Oracle, or MySQL, and no implication that DVault intercepts `dotnet ef` or auto-applies migrations.
- No automatic artifact regeneration as a blocking CI check; reviewed `dvault.model.v1` artifacts remain source-controlled inputs, not CI-authored outputs.

## Acceptance Criteria
- Adopter-facing documentation includes at least one GitHub Actions workflow example that runs the consumer-owned design-time command host with concrete rerunnable commands rather than pseudo-steps.
- The default blocking example runs `validate` against the configured design-time `DbContext` and explains that the same consumer project owns the `DbContext`, design-time factory, command entrypoint, and migrations.
- When a reviewed `dvault.model.v1` artifact exists, the example shows a blocking drift check against that committed artifact and uses artifact-versus-design-time-model comparison as the default lane instead of `--live-schema`.
- The workflow examples show migration guardrail execution after migration scaffolding and before apply or integration, using the consumer-owned migration resolver and `guardrail` command without implying DVault intercepts EF CLI commands.
- Any optional live-schema example is clearly marked as non-default and bounded to the current SQLite-first or explicit external-opt-in posture.
- Documentation makes clear that `export` is for artifact maintenance or refresh workflows, not the default blocking CI gate for pre-integration checks.

## Definition of Done
- Focused docs/examples land in the existing design-time guidance surfaces and stay consistent with the current single-project consumer-owned workflow.
- The documented commands map directly to the implemented `validate`, `drift`, and `guardrail` behavior and do not invent extra automation semantics.
- Docs-only changes remain covered by existing formatting and documentation validation; no new runtime or provider test suite is required unless the implementation adds executable sample code.
- The ticket leaves the broader README and release-note consolidation to `06F2PGHA0EXJRGDHM4GQM7NPYR` instead of duplicating that rollout here.

## Implementation Notes
- Use `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` as the primary architectural anchor for the CI examples because it already owns the supported design-time lifecycle boundary.
- Use the implemented `DataVaultDesignTimeCommand` surface for examples, with consumer-owned invocation patterns such as `dotnet run --project <consumer-project> -- validate`, `drift --artifact <path>`, and `guardrail --migration <name>` or equivalent host wiring.
- Prefer a narrow pointer update in `examples/README.md` or `docs/production-adoption-checklist.md` over broad README edits so adopters can discover the workflow without pulling the larger documentation task into this ticket.
- Keep drift examples centered on committed reviewed artifacts; if `export` is shown at all, frame it as a local artifact refresh step outside the default blocking CI lane.
- Keep any live-schema snippet explicitly optional and note that non-SQLite providers still depend on unsupported or external opt-in evidence.
- No relation cleanup or planning document write was materialized during refinement, so the implementation should ignore the historical done-ticket `blocks` links unless a separate planning pass cleans them up.

## Open Questions
- none

## Follow-Up Questions
- Should a later documentation ticket add equivalent templates for GitLab CI, Azure Pipelines, or Jenkins once the GitHub Actions baseline is published?
- Should a later follow-up add optional secret-backed CI examples for external-provider live-schema drift once provider-specific operational guidance exists?
- After the broader v0.11.0 docs rollout lands, should the focused workflow example be summarized in the root README or kept only in the design-time workflow or adoption docs?

## Risks
- If the examples blur the consumer-owned boundary, adopters may assume DVault intercepts `dotnet ef`, auto-discovers migrations, or ships a standalone CLI.
- If the default example uses `--live-schema` instead of artifact-based drift, non-SQLite adopters could copy an unsupported or secret-dependent gate.
- If CI examples teach `export` as the blocking check, teams may validate against freshly generated artifacts instead of a reviewed committed baseline.
- Historical incoming `blocks` relations from done tickets remain live in the ticket store, so schedule views may appear more constrained than the actual implementation dependency baseline.

## Split Recommendations
- No additional split is recommended; the existing breakdown is already bounded across story `06F2PGGEY26Y65G97NGFKH381M`, command-surface implementation task `06F2PGGJQMKH2T5948VJH93M5R`, this CI/examples task, and broader documentation task `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- If the project later wants provider-specific operational templates or non-GitHub CI systems, capture them as separate follow-up tickets instead of widening this ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Show how adopters run DVault checks before integration.

## Scope
- Refine and complete the work for "Add CI workflow examples for design-time checks" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.