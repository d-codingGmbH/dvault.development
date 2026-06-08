[gicket-bot] PO refinement contract

Summary
- Refined the ticket to document the bounded v0.32 artifact-lane story around the existing consumer-owned design-time dry-run contract; no ticket writes, attachments, or planning documents were applied during this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence already fixes the design-time boundary to a single consumer-owned project with the configured DbContext, IDesignTimeDbContextFactory<TContext>, and command host; that baseline is not reopened here.
- Current source already exposes `dvault sql-artifact --output <path> [--workload provider-native-bulk-ingestion]` and exports schema `dvault.sql-artifact.v1` as a review-only dry-run manifest; documentation must describe that bounded surface rather than a deployable runtime lane.
- The current branch does not contain `docs/releases/v0.32.0.md`, so adding the v0.32 release note is part of this ticket's scope rather than an open question.
- Persisted relations already place this ticket under parent `06F8KZTCEMNNFBFTVMFXEN268M`; no relation cleanup or additional child-ticket materialization was needed during refinement.

Scope In
- Document the artifact lane as explicit opt-in, design-time-only, and consumer-owned within the existing EF/DVault single-project workflow.
- Describe consumer-owned responsibilities for artifact review, storage, deployment, invocation, versioning, rollback, cleanup, credentials, environment selection, transaction policy, observability, and migration compatibility.
- Document the evidence gate using request-bound diagnostics, the shared benchmark artifact triplet and contract, and semantic-parity expectations for the exact provider and workload.
- Add or update v0.32 release documentation and non-goal wording so the repository no longer treats the artifact lane only as future scope.

Scope Out
- Implementing runtime dispatch, interceptors, schedulers, background workers, or automatic invocation of provider-specific artifacts.
- Automatic deployment, automatic cleanup, automatic EF migration synchronization, automatic live-schema repair, or DBA workflow automation.
- Expanding the supported-provider baseline beyond SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Changing benchmark artifact schemas, inventing new evidence formats, or claiming completed external-provider measurements that are not already evidenced.

Open questions
- none

Follow-up questions
- After the evidence and prototype child work lands, should a later ticket broaden the documented implementation beyond the current SQL Server dry-run exporter to additional supported providers?
- Should a later ticket standardize a consumer-facing repository path convention for reviewed manifests and any future sidecar SQL payload files?
- After the dry-run lane is proven, should a later release define a separate contract for deployable sidecar SQL payloads, or keep v1 permanently review-only?

Risks
- The current source exporter is narrower than the high-level provider baseline; docs must avoid implying that all five supported providers already have implemented artifact exporters.
- The evidence gate depends on shared benchmark artifacts and request-bound diagnostics; release prose that collapses skipped optional-provider rows into completed claims would overstate the repository evidence.
- If the evidence or prototype child tickets change manifest fields, workload naming, or prototype scope before merge, the v0.32 docs will need a final consistency pass.

Split recommendations
- No additional split is recommended for this ticket; the existing parent artifact-lane contract already separates documentation scope from evidence and dry-run prototype work.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment