[gicket-bot] PO refinement contract

Summary
- Ratified this epic as a completed v0.11.0 design-time/drift roll-up: the four already-created child tickets are done, the repository evidence matches the intended boundary, and no further split or relation cleanup is needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Local ticket-store relation state shows the epic already has four `parentOf` children and all are `done`: `06F2PGFZWC5PXSDH46RCZPN1CG` (provider live-schema readers), `06F2PGGEY26Y65G97NGFKH381M` (design-time command surface), `06F2PGGW8ZBW80V6B8RPWNVM70` (migration guardrails), and `06F2PGHA0EXJRGDHM4GQM7NPYR` (v0.11.0 documentation and release notes).
- Repository evidence ratifies the intended v0.11.0 baseline: `DataVaultDesignTimeCommand` and `DataVaultDesignTimeCommandHost` exist for `validate`, `export`, `drift`, and `guardrail`; `DataVaultLiveSchemaReader` has built-in SQLite/PostgreSQL/SQL Server/Oracle/MySQL dispatch; and `DataVaultMigrationOperationDiagnostics` plus tests anchor the CI-safe guardrail lane.
- The supported design-time boundary remains consumer-owned and single-project: the application owns the configured `DbContext`, `IDesignTimeDbContextFactory<TContext>`, command host entrypoint, reviewed-artifact path, and `dotnet ef` invocation point; DVault does not ship a standalone CLI, intercept EF commands, auto-apply migrations, or repair schema drift.
- Current public docs on the branch already align to the v0.11.0 baseline in `README.md`, `examples/README.md`, `docs/production-adoption-checklist.md`, `docs/model-first-governance.md`, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`, and `docs/releases/v0.11.0.md`.
- Existing downstream `blocks` relations were verified and left unchanged: `06F2PGHJAFMH80TZAMANQWH9PW`, `06F2PGHQ2GATEM13M5QK1MSX1G`, `06F2PGHWEWYJZSRQ9QPT4NJ0QM`, `06F2PGJ28KVSZAAFRA40D94128`, `06F2PGJBRXFCP038CN6XVAYSZM`, `06F2PGJGDGMXHPT1VP0ASQ5HJ4`, `06F2PGJN1XCV8F7NWH567SQSKM`, `06F2PGJSXP18VKKV52QZA4NP30`, and `06F2PGJYY6S97B4Z8044D34K5C`; no relation adds/removals, new child tickets, attachments, or planning documents were materialized in this pass.
- Ticket comments in the local store are automation claim/lease comments only; there is no human clarification to fold into the epic scope.

Scope In
- Epic-level roll-up of the already-materialized v0.11.0 design-time/drift work delivered by child tickets `06F2PGFZWC5PXSDH46RCZPN1CG`, `06F2PGGEY26Y65G97NGFKH381M`, `06F2PGGW8ZBW80V6B8RPWNVM70`, and `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- Consumer-owned design-time workflow around a configured `DbContext`, `IDesignTimeDbContextFactory<TContext>`, reusable `validate`/`export`/`drift`/`guardrail` verbs, reviewed-artifact drift checks, and explicit migration preflight before `dotnet ef database update`.
- Built-in live-schema reader support for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL, with external-provider execution remaining opt-in operational evidence rather than default local validation.
- Provider-neutral migration guardrail diagnostics that are deterministic enough for CI preflight without parsing provider SQL or inferring automatic repair actions.
- Public documentation and release-note alignment for the v0.11.0 baseline, including current package versions and the explicit non-goals around CLI interception and automatic migration behavior.

Scope Out
- Future analyzer, code-fix, and source-generator ergonomics already tracked in blocked v0.12.0 tickets beginning with epic `06F2PGHJAFMH80TZAMANQWH9PW`.
- A DVault-owned standalone executable, `dotnet ef` shim/interception, automatic migration execution, or automatic schema repair.
- Startup-project and target-project split support, multi-project design-time discovery, or host auto-discovery beyond the documented single-project v1 boundary.
- Provider-specific database provisioning, secret-management recipes, container lifecycle guides, or runnable non-SQLite operational tutorials.
- Model-snapshot drift, rename/missing-table inference from prior schema state, or broader drift surfaces beyond reviewed-artifact comparison and the bounded live-schema reader.

Open questions
- none

Follow-up questions
- Should a later tooling ticket broaden the current single-project design-time boundary to support startup-project/target-project or other multi-project discovery patterns?
- Should a later drift ticket add model-snapshot or prior-schema-aware comparison lanes in addition to reviewed-artifact drift and the current bounded live-schema reader?
- Should later docs/tooling add structured JSON command output or provider-specific operational walkthroughs for PostgreSQL, SQL Server, Oracle, and MySQL live-schema validation?

Risks
- Non-SQLite live-schema validation remains environment-dependent and can regress unnoticed unless consuming applications explicitly enable the opt-in provider lanes.
- If later docs or downstream tickets blur the boundary, users may misread built-in provider readers as DVault-managed database provisioning, CI infrastructure, or automatic migration behavior.
- Future analyzer/generator work could accidentally reopen the v0.11.0 scope unless the explicit consumer-owned command-host and preflight model stays fixed.

Split recommendations
- No additional split is recommended; the epic already has the correct bounded child set in `06F2PGFZWC5PXSDH46RCZPN1CG`, `06F2PGGEY26Y65G97NGFKH381M`, `06F2PGGW8ZBW80V6B8RPWNVM70`, and `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- Keep the existing downstream analyzer/code-fix/source-generator tickets blocked and separate instead of widening this v0.11.0 design-time/drift epic.
- Do not materialize new planning docs or relation rewrites unless a later ticket intentionally re-scopes the downstream dependency graph.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment