<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratifies this story as provider-neutral migration-guardrail hardening for the existing consumer-owned `guardrail` preflight, centered on already-materialized child task `06F2PGH42B6BT1708MYGMXP5GM`; broader v0.11 documentation and release-note rollout stays in blocked task `06F2PGHA0EXJRGDHM4GQM7NPYR`.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence shows the upstream command surface is already delivered by done story `06F2PGGEY26Y65G97NGFKH381M`; this story does not reopen `DataVaultDesignTimeCommand`, `DataVaultDesignTimeCommandHost`, the `guardrail` verb, or consumer-owned command hosting.
- This story already has one materialized child ticket, `06F2PGH42B6BT1708MYGMXP5GM` (`Task: Add migration guardrail rule coverage`), and that child is `done`.
- Live relations also show epic parent `06F2PGFT8Z406HFBJGQSY7YRJ0` and a current `blocks` relation from this story to `06F2PGHA0EXJRGDHM4GQM7NPYR` (`Task: Update v0.11.0 documentation and release notes`); broader README and release-note rollout stays there.
- Local ticket evidence contains no human scope comments or ticket attachments; only automation claim comments are present, so repository source and persisted relations are the authoritative refinement inputs.
- Repository code and tests already ratify the bounded v1 guardrail taxonomy: `DVM2001` through `DVM2006`, deterministic `migration/{Operation}/{Target}/{Member?}` paths, and provider-neutral analysis in `DataVaultMigrationOperationDiagnostics`.

### Scope In
- Provider-neutral hardening of migration-operation diagnostics so the existing `guardrail` preflight is safe to use as a blocking CI step.
- Coverage for current DVault-produced hub, link, satellite, PIT, and bridge table shapes, including `CreateTableOperation` analysis alongside the existing add/drop/alter/rename-column, primary-key, index, and drop-table checks.
- Deterministic quiet behavior for non-DVault tables and structurally matching DVault tables.
- Narrow catalog or wording adjustments needed to keep `DVM2001`-`DVM2006` accurate for the expanded rule coverage.
- Automated tests that prove exact code, severity, path, and report ordering for the migration guardrail matrix.

### Scope Out
- No new command verbs, host architecture changes, EF CLI interception, or `dotnet ef` shim work; that command surface is already handled by done story `06F2PGGEY26Y65G97NGFKH381M`.
- No provider-specific store-type, default-SQL, collation, annotation, or SQL-text parsing checks.
- No live-schema drift changes, migration execution, schema repair, or prior-schema inference.
- No RenameTable or missing-table detection that depends on model snapshot or reviewed-artifact state.
- No broad v0.11 README or release-note consolidation; that remains in `06F2PGHA0EXJRGDHM4GQM7NPYR`.

## Acceptance Criteria
- The existing consumer-owned `guardrail` preflight can be used as a blocking CI step because migration diagnostics cover the current DVault structural invariants for `CreateTableOperation`, add/drop/alter/rename-column, default-index, primary-key, and drop-table operations.
- Non-DVault tables are ignored, and a DVault migration operation set that matches the current explain baseline for hub, link, satellite, PIT, and bridge tables produces no guardrail findings.
- Finding-producing operations reuse the current stable `DVM2001` through `DVM2006` catalog instead of introducing a new public migration-diagnostic taxonomy.
- Guardrail findings keep deterministic `migration/{Operation}/{Target}/{Member?}` paths and stable report ordering so CI and tests can assert exact output.
- Automated coverage proves quiet and finding cases for the create-table lane and the existing migration-operation matrix without changing the public command surface or diagnostics API shape.

## Definition of Done
- The story stays bounded to provider-neutral guardrail hardening; consumer-owned command hosting, exit-code behavior, and public command verbs remain unchanged.
- The repository keeps one authoritative migration-guardrail taxonomy through `DVM2001`-`DVM2006`, with any wording updates kept consistent across code, tests, and focused docs.
- Tests cover representative hub, link, satellite, PIT, and bridge cases and assert deterministic code, severity, path, and ordering.
- Any documentation touch is limited to guardrail-specific wording or focused workflow guidance and does not duplicate the broader v0.11 documentation task.
- No additional child split is required for this story beyond the already-materialized child `06F2PGH42B6BT1708MYGMXP5GM` and the existing blocked docs follow-up `06F2PGHA0EXJRGDHM4GQM7NPYR`.

## Implementation Notes
- Use `DataVaultMigrationOperationDiagnostics` as the single implementation lane; repository source already shows provider-neutral analysis over the diagnostics explain baseline rather than provider SQL.
- Keep the current bounded default visible in source and tests: `CreateTableOperation` plus add/drop/alter/rename-column, create/drop/rename-index, add/drop-primary-key, and drop-table coverage.
- Preserve quiet behavior for application-owned tables and deterministic report ordering so the existing `guardrail` verb remains automation-safe.
- Focused adopter CI guidance already exists in `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` and `docs/production-adoption-checklist.md`; broader v0.11 release-note work still needs `06F2PGHA0EXJRGDHM4GQM7NPYR`, and `docs/releases/v0.11.0.md` is not yet present in the repository.
- No new planning documents, child tickets, or relation changes were materialized in this run because the current split and live relations are already present in the ticket store.

## Open Questions
- none

## Follow-Up Questions
- Should a later hardening ticket add `RenameTableOperation` coverage if table-name drift proves common in reviewed migrations?
- Should a later drift-aware ticket compare migration operations against model snapshot or reviewed-artifact state so missing or renamed DVault tables can be detected safely?
- Should a later provider-focused ticket add optional provider-specific checks for store type, default SQL, or other engine facets after the provider-neutral CI baseline is stable?

## Risks
- Broadening beyond provider-neutral structural invariants would create noisy false positives across providers and weaken CI trust.
- The current guardrail lane has no authoritative prior-schema state, so absence-based or rename-based inference would be unstable if added here.
- If create-table and existing operation findings stop sorting deterministically, downstream `guardrail` output and test baselines will churn.
- Broader v0.11 discoverability still depends on separate documentation task `06F2PGHA0EXJRGDHM4GQM7NPYR`.

## Split Recommendations
- No new split is required now; keep implementation-level rule coverage in already-done child `06F2PGH42B6BT1708MYGMXP5GM`.
- Keep broader README and release-note rollout in existing blocked task `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- Treat rename-table, missing-table inference, and provider-specific facet checks as later follow-up tickets rather than widening this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Make migration diagnostics actionable enough to gate CI safely.

## Scope
- Refine and complete the work for "Harden migration guardrails for CI enforcement" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.