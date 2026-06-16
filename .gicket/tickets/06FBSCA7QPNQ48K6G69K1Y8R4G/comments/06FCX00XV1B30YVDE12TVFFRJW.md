[gicket-bot] PO refinement contract

Summary
- Refined the ticket to the repository-backed PostgreSQL save-strategy baseline: `AddDVaultPostgres()` / `PostgresDataVaultSaveStrategy`, retained direct-or-UNNEST below 60 operations, staged `COPY` at 60-plus, diagnostics/fallback coverage, and benchmark evidence ratified through the checked-in v0.32 PostgreSQL bundle. No child tickets, relation changes, description updates, attachments, or planning documents were materialized; prior canonical gicket read attempts for live ticket/comments/relations were trust-blocked.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The visible repository baseline indicates the spike outcome is already resolved in favor of the existing PostgreSQL provider-save implementation; this refinement ratifies that bounded baseline instead of reopening the spike decision.
- The accepted PostgreSQL bulk scope is save-path work inside `IDataVaultSaveService`, not a new PostgreSQL latest-satellite read optimization, PIT/bridge expansion, or provider-specific runtime artifact lane.
- Benchmark evidence for this ticket is two-tiered: the root `benchmark-summary.*` files preserve skipped-placeholder PostgreSQL row identity when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset, while completed PostgreSQL timing claims come from `artifacts/benchmarks/v0.32.0-06F9XD33MNNVHHW232TC7T1CN8-scale-evidence-<redacted>/after/postgres` and its README.
- No bounded planning writes were applied in this run because repository evidence already bounded the ticket and live gicket ticket/comment/relation reads were unavailable under the current trust policy.

Scope In
- PostgreSQL optimized save behavior behind `AddDVaultPostgres()` and `PostgresDataVaultSaveStrategy` for clean `Npgsql.EntityFrameworkCore.PostgreSQL` contexts.
- Retained direct or UNNEST execution below the 60-operation staged boundary and transient staging plus PostgreSQL `COPY` at 60-plus eligible operations.
- Provider-neutral fallback when provider identity, dirty-context, or unsupported multi-active-satellite gates decline the PostgreSQL path.
- Provider strategy tests, staged-provider diagnostics/fallback coverage, and benchmark evidence/documentation surfaces that prove or describe the PostgreSQL save path.

Scope Out
- New PostgreSQL latest-satellite read optimization.
- PostgreSQL PIT or bridge read expansion beyond the already documented diagnostics-gated candidate posture.
- Work on SQL Server, MySQL, Oracle, DB2, or provider-specific SQL artifact export/runtime dispatch.
- Automatic database provisioning, secret management, or making live PostgreSQL validation mandatory for default local validation.
- Treating the root skipped PostgreSQL rows as completed timing evidence or requiring the root quick baseline to be rewritten as a live-provider artifact.

Open questions
- none

Follow-up questions
- After PO-critic review, confirm whether live ticket lineage should be reconciled with the earlier checked-in PostgreSQL evidence ticket trail, since the repository already carries the implementation and benchmark bundle this task describes.
- If a new benchmark rerun is desired on this branch, confirm whether it should supplement rather than replace the existing v0.32 PostgreSQL provider bundle so completed-timing claims remain tied to preserved run context.

Risks
- Canonical gicket ticket/comment/relation reads were trust-blocked earlier in the session, so duplicate/absorbed-ticket housekeeping could not be verified against live ticket metadata.
- The root quick baseline still emits PostgreSQL rows as skipped when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset; consumers who need completed PostgreSQL timing evidence must cite the checked-in provider-configured bundle instead.
- Configured PostgreSQL integration and benchmark evidence remain opt-in and environment-dependent.

Split recommendations
- No split recommended; repository evidence already bounds the work to one PostgreSQL save-strategy delivery/evidence surface.

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