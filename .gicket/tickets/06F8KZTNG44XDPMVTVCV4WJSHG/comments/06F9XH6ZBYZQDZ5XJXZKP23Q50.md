[gicket-bot] PO refinement contract

Summary
- Refined the provider-specific SQL artifact story into a bounded v1 contract, verified the existing ticket, relation, and repository evidence, and materialized docs/plans/provider-specific-sql-artifact-contract.md plus the docs/plans index entry.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the design-time boundary: provider-specific SQL artifacts must stay inside the consumer-owned single-project design-time workflow documented by docs/architecture/dvault-dotnet-ef-design-time-workflow.md and must not introduce a standalone DVault CLI or runtime dispatcher.
- The finite supported-provider baseline is already visible in the repository and is ratified for this contract: SQLite, PostgreSQL, SQL Server, MySQL, and Oracle only.
- The existing escape-hatch boundary is already documented in docs/performance-profiles.md, docs/architecture/dvault-v1-explicit-save-service.md, and the historical v0.20.0, v0.26.0, and v0.31.0 release notes; this ticket now turns that boundary into an explicit artifact contract rather than reopening whether DVault should auto-run provider SQL.
- A durable ticket-bound planning document was materialized at docs/plans/provider-specific-sql-artifact-contract.md and indexed in docs/plans/README.md.
- The existing epic split is sufficient: 06F8KZVCVRPS3NAGQA7J55EAA4 covers benchmark and semantic parity evidence, 06F8KZV18BQ0GN3CE4G02ATVA0 covers the dry-run manifest prototype, and 06F8KZVRARQPG482YKCQ686PNM covers v0.32 documentation alignment.
- The incoming blocks relation from done ticket 06F8KZSYCVZ21MS983501BZG18 is historical dependency evidence only and does not reopen this story.

Scope In
- Define the v1 architecture contract for reviewed design-time provider-specific SQL or stored-procedure artifacts as an explicit opt-in consumer workflow.
- Fix the source-of-truth boundary to the existing consumer-owned design-time host, validation, drift, guardrail, request-bound diagnostics, and benchmark evidence posture already documented in the repository.
- Ratify an authoritative deterministic manifest-first artifact format with schemaVersion dvault.sql-artifact.v1, provider and workload binding, metadata-source fingerprinting, semantic parity references, and dry-run support.
- Define review rules, redaction rules, and consumer-owned responsibilities for storage, deployment, invocation, versioning, rollback, cleanup, credentials, transaction policy, and observability.
- Keep the first implementation lane dry-run only and aligned with the existing child split for evidence, prototype, and documentation work.

Scope Out
- Runtime dispatch, automatic stored-procedure invocation, interceptors, background workers, schedulers, or any second default DVault runtime path.
- Automatic deployment, automatic cleanup, automatic EF migration synchronization, automatic support-bundle refresh synchronization, or automatic live-schema mutation.
- A standalone DVault CLI, multi-project design-time orchestration, or any provider baseline beyond SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Benchmark-threshold tuning work such as 06F9XD1T3TJK7NEBYNVT2JEPZW, which remains separate from the artifact contract itself.
- Production-ready provider-specific execution before the evidence and dry-run prototype tickets land.

Open questions
- none

Follow-up questions
- Which of the already-supported providers should be used for the first dry-run manifest prototype in 06F8KZV18BQ0GN3CE4G02ATVA0?
- What consumer repository path convention should the later implementation adopt for reviewed manifests and any sidecar SQL payload files?
- After the dry-run prototype and evidence tickets land, should a later ticket permit deployable SQL payload emission, or should the lane remain review-only until more provider evidence exists?
- How should the v0.32 documentation rollout summarize this lane once the prototype and evidence tickets complete?

Risks
- Future implementation work could accidentally widen this lane into runtime dispatch or automatic migration behavior unless it stays anchored to the explicit non-goals now documented.
- A prototype that skips the benchmark or semantic parity evidence gate could create unmeasured provider-specific claims that conflict with the repository's existing performance-evidence posture.
- If later work stores executable SQL payloads without a deterministic manifest-relative convention, review and drift management could become inconsistent across consumer projects.

Split recommendations
- No new split is needed now; the current parent and child tickets already separate architecture contract, evidence rules, dry-run prototype, and v0.32 documentation work.
- If a later phase wants deployable SQL payload emission, runtime invocation helpers, or provider-specific validators, create separate follow-up tickets instead of widening this contract or the dry-run prototype ticket.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment