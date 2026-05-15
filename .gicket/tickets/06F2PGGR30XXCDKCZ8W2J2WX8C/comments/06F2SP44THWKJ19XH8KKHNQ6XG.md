[gicket-bot] PO refinement contract

Summary
- Bounded this ticket to adopter-facing GitHub Actions examples for running `validate`, model drift, and migration guardrail checks through the consumer-owned `DataVaultDesignTimeCommand` surface before integration; no child tickets, relation writes, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already exposes the reusable command surface `DataVaultDesignTimeCommand`, `DataVaultDesignTimeCommandHost`, and `DataVaultDesignTimeExportSource`, so this ticket documents how adopters run those commands in CI rather than extending the core API.
- The supported v1 design-time boundary stays single-project and consumer-owned: the project that owns the configured `DbContext` also owns `IDesignTimeDbContextFactory<TContext>`, the command host entrypoint, migration resolution, and the `dotnet ef` invocation point.
- GitHub Actions YAML is the bounded v1 example format because `.github/workflows/ci.yml` is the only visible repository workflow baseline; alternate CI systems are follow-up work, not an open refinement blocker.
- Artifact-versus-design-time-model drift is the default CI lane; `--live-schema` remains optional and SQLite-first or external-opt-in because broader providers are still documented as unsupported or unavailable first-class live drift readers.
- No child tickets, relation updates, or planning documents were materialized in this pass; live relation state still includes historical incoming `blocks` links from done tickets `06F2PGGJQMKH2T5948VJH93M5R` and `06F2PGFZWC5PXSDH46RCZPN1CG`.

Scope In
- Add focused adopter-facing CI workflow examples that use the existing consumer-owned design-time command surface for pre-integration checks.
- Show one GitHub Actions baseline with concrete rerunnable commands for `validate`, `drift` when a reviewed artifact exists, and `guardrail` after scaffolding a migration.
- Keep the examples aligned with the current single-project consumer layout documented in `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`.
- Add or update the narrow design-time/example docs needed so adopters can discover the workflow without waiting for the broader v0.11.0 README/release-note rollout.

Scope Out
- No new DVault command verbs, no changes to `DataVaultDesignTimeCommand` public API, and no new packable or standalone CLI project.
- No repo-owned CI workflow expansion, release automation, secret management, or default-on external provider jobs.
- No broad root README or v0.11.0 release-note rollout; that remains in `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- No default live-schema CI lane for PostgreSQL, SQL Server, Oracle, or MySQL, and no implication that DVault intercepts `dotnet ef` or auto-applies migrations.
- No automatic artifact regeneration as a blocking CI check; reviewed `dvault.model.v1` artifacts remain source-controlled inputs, not CI-authored outputs.

Open questions
- none

Follow-up questions
- Should a later documentation ticket add equivalent templates for GitLab CI, Azure Pipelines, or Jenkins once the GitHub Actions baseline is published?
- Should a later follow-up add optional secret-backed CI examples for external-provider live-schema drift once provider-specific operational guidance exists?
- After the broader v0.11.0 docs rollout lands, should the focused workflow example be summarized in the root README or kept only in the design-time workflow or adoption docs?

Risks
- If the examples blur the consumer-owned boundary, adopters may assume DVault intercepts `dotnet ef`, auto-discovers migrations, or ships a standalone CLI.
- If the default example uses `--live-schema` instead of artifact-based drift, non-SQLite adopters could copy an unsupported or secret-dependent gate.
- If CI examples teach `export` as the blocking check, teams may validate against freshly generated artifacts instead of a reviewed committed baseline.
- Historical incoming `blocks` relations from done tickets remain live in the ticket store, so schedule views may appear more constrained than the actual implementation dependency baseline.

Split recommendations
- No additional split is recommended; the existing breakdown is already bounded across story `06F2PGGEY26Y65G97NGFKH381M`, command-surface implementation task `06F2PGGJQMKH2T5948VJH93M5R`, this CI/examples task, and broader documentation task `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- If the project later wants provider-specific operational templates or non-GitHub CI systems, capture them as separate follow-up tickets instead of widening this ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment