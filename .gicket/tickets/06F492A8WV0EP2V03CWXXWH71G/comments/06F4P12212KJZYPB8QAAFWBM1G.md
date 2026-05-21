[gicket-bot] PO refinement contract

Summary
- Refined the story against the checked-in ticket and relation state: the existing DVM2001-DVM2006 guardrail catalog remains authoritative, the current blocks relations to 06F492BG6BZYYFMBE5WK7CB024 and 06F492BNDPWS9P4EDSV0W7G6VM were verified and retained, and no child tickets, relation writes, description updates, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Checked-in ticket state shows no human scope comments or attachments for 06F492A8WV0EP2V03CWXXWH71G, so the authoritative refinement inputs were the current repository source plus the persisted ticket and relation files under .gicket.
- Done story 06F2PGGW8ZBW80V6B8RPWNVM70 and done child task 06F2PGH42B6BT1708MYGMXP5GM already ratify the current provider-neutral migration-guardrail matrix and CreateTable coverage; this story is about strengthening report structure and wording on top of that baseline, not reopening rule coverage or command ownership.
- The existing DVM2001-DVM2006 catalog and severity mapping stay authoritative for findings: DVM2001, DVM2002, DVM2003, and DVM2006 are incompatible or error findings, while DVM2004 and DVM2005 are risky or warning findings.
- For this story's v1 report semantics, safe means an inspected migration operation produced no DVM finding, risky means it produced warning-severity findings, and incompatible means it produced error-severity findings; safe operations should be reported explicitly as operation outcomes rather than synthesized as new DMV or DVM info issues.
- Provider-aware wording must come from the existing diagnostics baseline already exposed through AnalyzeReport and DataVaultDiagnosticsResult, including the resolved provider name, capability profile, provider-behavior profile, and current provider or value-format metadata where relevant, so wording reflects the actual EF context without implying unsupported provider-specific SQL analysis.
- The authoritative public boundary remains DataVaultMigrationOperationDiagnostics.AnalyzeReport(...), DataVaultMigrationGuardrailReport, DataVaultMigrationGuardrailIssue, and DataVaultMigrationGuardrailReport.ToDisplayString(); strengthening can add ordered operation-summary data, but downstream callers must not be forced to recover safe, risky, or incompatible status by parsing ad hoc CLI text.
- The verified live relations remain parent epic 06F492A3MPSGP3KXDNZECN01QM plus blocks links to 06F492BG6BZYYFMBE5WK7CB024 and 06F492BNDPWS9P4EDSV0W7G6VM; no relation cleanup was justified by the current evidence.

Scope In
- Strengthen migration guardrail reporting so every inspected MigrationOperation can be surfaced with a deterministic safe, risky, or incompatible outcome instead of limiting the report to finding-only rows.
- Keep the current DVM2001-DVM2006 codes, severities, remediation text, and migration/{Operation}/{Target}/{Member?} paths as the single finding taxonomy behind those outcomes.
- Preserve the underlying Diagnostics, IsValid, HasFindings, and existing finding list behavior while adding a machine-readable ordered operation-summary surface and matching human-readable rendering.
- Add provider-aware wording derived from the active diagnostics baseline so report output identifies the configured provider or context and any defaulted provider-profile state without implying new provider-specific validation rules.
- Cover representative destructive or ambiguous migration shapes already represented in the existing guardrail matrix, including dropped DVault tables, dropped or altered required technical or structural columns, wrong key or index coverage, renamed DVault-owned columns, and malformed created DVault tables.
- Add tests for safe, risky, and incompatible report outcomes, deterministic ordering, and at least one SQLite-backed provider-context example.

Scope Out
- No new migration diagnostic code family or parallel taxonomy outside the existing DVM2001-DVM2006 catalog.
- No provider-specific SQL parsing, store-type validation, default-SQL comparison, collation analysis, or engine-specific migration rewriting.
- No changes to EF command ownership, DataVaultDesignTimeCommand verb shape, migration execution, or automatic dotnet ef interception.
- No redesign of sibling preflight aggregator story 06F492BG6BZYYFMBE5WK7CB024 or blocked documentation task 06F492BNDPWS9P4EDSV0W7G6VM beyond keeping this report surface reusable by those tickets.
- No live-schema drift, ModelSnapshot comparison, or prior-schema inference beyond the current migration-operation plus diagnostics baseline.

Open questions
- none

Follow-up questions
- Once this report contract lands, should story 06F492BG6BZYYFMBE5WK7CB024 expose the same structured safe, risky, and incompatible matrix through its aggregated preflight surface without reshaping it?
- Once implementation is done, should task 06F492BNDPWS9P4EDSV0W7G6VM publish one canonical example each for safe, risky, and incompatible guardrail output in release notes and adoption guidance?
- Should a later provider-focused story add optional provider-specific hints for store types or engine limitations after this provider-aware but provider-neutral report baseline proves stable?

Risks
- If implementation only changes ToDisplayString() and does not add a machine-readable ordered operation surface, downstream automation and the preflight aggregator will still need to parse text or reimplement classification.
- Provider-aware wording can become misleading if it hard-codes engine claims instead of reflecting the actual diagnostics baseline, especially when provider or profile selection defaulted.
- Building safe, risky, or incompatible summaries from unordered dictionaries or merged finding sets instead of the input operation order will destabilize CI baselines and human review output.
- Because the current guardrail engine remains provider-neutral structural analysis, consumers may overread provider-aware wording as provider-specific validation unless non-goals stay explicit in code, tests, and downstream docs.

Split recommendations
- No new split is recommended; repository evidence already shows the guardrail rule catalog and operation matrix are in place, so this story can stay focused on report-surface strengthening.
- Keep story 06F492BG6BZYYFMBE5WK7CB024 and task 06F492BNDPWS9P4EDSV0W7G6VM as downstream consumers of the finalized report contract rather than pulling their scope into this ticket.
- If later work needs provider-specific SQL or store-type hints, migration-history reasoning, or ModelSnapshot-aware inference, raise that as a separate follow-up story instead of widening this ticket.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment