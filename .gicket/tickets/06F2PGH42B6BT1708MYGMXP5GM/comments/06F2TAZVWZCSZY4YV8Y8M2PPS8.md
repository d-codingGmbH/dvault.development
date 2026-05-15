[gicket-bot] PO refinement contract

Summary
- Refinement narrows this ticket to provider-neutral CreateTableOperation guardrail coverage, reusing the current DVM2001-DVM2006 catalog and explain-baseline comparison so CI can catch malformed newly generated DVault tables without widening into rename-table or provider-specific SQL analysis.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Done story 06F2PGGEY26Y65G97NGFKH381M already delivers the consumer-owned guardrail command surface; this ticket hardens rule coverage inside DataVaultMigrationOperationDiagnostics rather than reopening command verbs, host ownership, or exit-code behavior.
- Current source already handles AddColumn, DropColumn, AlterColumn, RenameColumn, CreateIndex, DropIndex, RenameIndex, AddPrimaryKey, DropPrimaryKey, and DropTable guardrails; the first visible high-confidence gap for CI enforcement is that CreateTableOperation is still ignored.
- Ratify CreateTableOperation shape validation as the first expansion lane for this ticket, using the current diagnostics explain baseline for hub, link, satellite, PIT, and bridge entities.
- Reuse DVM2001 through DVM2004 where the invariant meaning still fits create-table findings, and only broaden existing catalog wording where it is currently too drop-or-alter-specific; do not add a new migration diagnostic code in this first expansion.
- Limit analysis to create-table operations whose table name already matches a current DVault-produced table name; do not infer missing or renamed tables from absence in the operation set.
- Broader README and release-note rollout remains separate in 06F2PGHA0EXJRGDHM4GQM7NPYR; this ticket only needs a doc touch if a narrow guardrail example or catalog description must change.

Scope In
- Add provider-neutral CreateTableOperation guardrail analysis for current DVault-produced hub, link, satellite, PIT, and bridge tables.
- Validate created table column shape against the current explain baseline, including hub/link insert-only boundaries, required technical columns, key or parent or participant or driving columns, PIT snapshot-reference columns, and bridge TraversalDepth where applicable.
- Validate the created table primary-key shape when it is expressed inside CreateTableOperation, while leaving existing CreateIndexOperation and AddPrimaryKeyOperation checks in place for separate EF operations.
- Add deterministic tests for quiet and finding-producing create-table cases, including a non-DVault quiet case and representative DVault cases across the current hub/link and PIT/bridge structural baseline.
- Preserve the existing migration path format and deterministic report ordering when create-table findings are surfaced through the public guardrail report and command path.

Scope Out
- No RenameTableOperation, foreign-key, check-constraint, default-SQL, or provider-specific store-type analysis in this first expansion.
- No absence-based detection for expected DVault tables that were not created by the operation set, because this guardrail pass does not own prior-schema state.
- No changes to DataVaultDesignTimeCommand verbs, exit-code policy, consumer-owned migration resolution, or public diagnostics issue shape.
- No live-schema drift changes, migration SQL parsing, migration execution, or schema repair behavior.
- No broad documentation consolidation beyond any narrowly necessary guardrail wording update.

Open questions
- none

Follow-up questions
- Should a later hardening ticket add RenameTableOperation coverage, potentially with a new stable DVM code if table-name drift cannot fit the current catalog cleanly?
- Should later guardrails compare create-table operations against reviewed artifact or ModelSnapshot context so missing or renamed DVault tables can be detected without inferring from operation absence?
- Should a later provider-focused ticket add optional provider-specific checks for store-type or default-SQL mismatches once the provider-neutral create-table baseline proves stable?

Risks
- CreateTableOperation carries provider-specific facets that vary across databases; comparing those directly would create noisy false positives, so this ticket must stay on provider-neutral DVault structural invariants.
- The guardrail pass has no authoritative prior-schema state, so trying to infer that an expected DVault table should have been created or renamed would produce unstable CI behavior.
- If create-table findings emit overlapping DVM2001 through DVM2004 issues in nondeterministic order, downstream command and CI assertions will churn.
- Manual migration edits that change a DVault table name without reusing a current produced name may still evade this first expansion and should be handled by a later rename-table or drift-aware follow-up.

Split recommendations
- Keep this ticket bounded to provider-neutral CreateTableOperation rule coverage and tests.
- Track RenameTableOperation or missing-table inference as a separate follow-up if later work wants guardrails that reason about name drift or prior schema state.
- Keep broader v0.11 documentation or release-note rollout in 06F2PGHA0EXJRGDHM4GQM7NPYR rather than widening this ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment