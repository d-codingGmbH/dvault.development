[gicket-bot] PO refinement contract

Summary
- Refinement ratifies this story as provider-neutral migration-guardrail hardening for the existing consumer-owned `guardrail` preflight, centered on already-materialized child task `06F2PGH42B6BT1708MYGMXP5GM`; broader v0.11 documentation and release-note rollout stays in blocked task `06F2PGHA0EXJRGDHM4GQM7NPYR`.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows the upstream command surface is already delivered by done story `06F2PGGEY26Y65G97NGFKH381M`; this story does not reopen `DataVaultDesignTimeCommand`, `DataVaultDesignTimeCommandHost`, the `guardrail` verb, or consumer-owned command hosting.
- This story already has one materialized child ticket, `06F2PGH42B6BT1708MYGMXP5GM` (`Task: Add migration guardrail rule coverage`), and that child is `done`.
- Live relations also show epic parent `06F2PGFT8Z406HFBJGQSY7YRJ0` and a current `blocks` relation from this story to `06F2PGHA0EXJRGDHM4GQM7NPYR` (`Task: Update v0.11.0 documentation and release notes`); broader README and release-note rollout stays there.
- Local ticket evidence contains no human scope comments or ticket attachments; only automation claim comments are present, so repository source and persisted relations are the authoritative refinement inputs.
- Repository code and tests already ratify the bounded v1 guardrail taxonomy: `DVM2001` through `DVM2006`, deterministic `migration/{Operation}/{Target}/{Member?}` paths, and provider-neutral analysis in `DataVaultMigrationOperationDiagnostics`.

Scope In
- Provider-neutral hardening of migration-operation diagnostics so the existing `guardrail` preflight is safe to use as a blocking CI step.
- Coverage for current DVault-produced hub, link, satellite, PIT, and bridge table shapes, including `CreateTableOperation` analysis alongside the existing add/drop/alter/rename-column, primary-key, index, and drop-table checks.
- Deterministic quiet behavior for non-DVault tables and structurally matching DVault tables.
- Narrow catalog or wording adjustments needed to keep `DVM2001`-`DVM2006` accurate for the expanded rule coverage.
- Automated tests that prove exact code, severity, path, and report ordering for the migration guardrail matrix.

Scope Out
- No new command verbs, host architecture changes, EF CLI interception, or `dotnet ef` shim work; that command surface is already handled by done story `06F2PGGEY26Y65G97NGFKH381M`.
- No provider-specific store-type, default-SQL, collation, annotation, or SQL-text parsing checks.
- No live-schema drift changes, migration execution, schema repair, or prior-schema inference.
- No RenameTable or missing-table detection that depends on model snapshot or reviewed-artifact state.
- No broad v0.11 README or release-note consolidation; that remains in `06F2PGHA0EXJRGDHM4GQM7NPYR`.

Open questions
- none

Follow-up questions
- Should a later hardening ticket add `RenameTableOperation` coverage if table-name drift proves common in reviewed migrations?
- Should a later drift-aware ticket compare migration operations against model snapshot or reviewed-artifact state so missing or renamed DVault tables can be detected safely?
- Should a later provider-focused ticket add optional provider-specific checks for store type, default SQL, or other engine facets after the provider-neutral CI baseline is stable?

Risks
- Broadening beyond provider-neutral structural invariants would create noisy false positives across providers and weaken CI trust.
- The current guardrail lane has no authoritative prior-schema state, so absence-based or rename-based inference would be unstable if added here.
- If create-table and existing operation findings stop sorting deterministically, downstream `guardrail` output and test baselines will churn.
- Broader v0.11 discoverability still depends on separate documentation task `06F2PGHA0EXJRGDHM4GQM7NPYR`.

Split recommendations
- No new split is required now; keep implementation-level rule coverage in already-done child `06F2PGH42B6BT1708MYGMXP5GM`.
- Keep broader README and release-note rollout in existing blocked task `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- Treat rename-table, missing-table inference, and provider-specific facet checks as later follow-up tickets rather than widening this story.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment