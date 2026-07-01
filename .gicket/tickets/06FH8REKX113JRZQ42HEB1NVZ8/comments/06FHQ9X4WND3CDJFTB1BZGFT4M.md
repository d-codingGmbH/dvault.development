[gicket-bot] PO refinement contract

Summary
- Refined this as the documentation and evidence-publication child for the provider parity closure baseline: reuse the checked-in 2026-06-23 closure bundle, evidence and gap matrices, and aligned performance and release docs instead of rerunning benchmarks or reopening closed provider rows.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use docs/plans/provider-optimization-evidence-matrix.md and docs/plans/provider-optimization-gap-matrix.md as the canonical row-lookup and decision surfaces for this ticket.
- Treat benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json at the repository root as the quick SQLite plus skipped-placeholder optional-provider baseline, not as the authoritative completed external-provider timing source.
- Treat artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/ as the completed-timing source for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 provider-native save, latest-satellite read, PIT read, and bridge read rows.
- Ratify the already aligned documentation baseline instead of creating a new planning surface: docs/performance-profiles.md, docs/architecture/dvault-v1-pit-bridge-boundary.md, docs/releases/v0.46.0.md, and CHANGELOG.md already carry this closure evidence, with later docs carrying it forward.
- Keep PIT and bridge read timing separate from PIT maintenance timing: MySQL PIT maintenance is source and test backed but still unmeasured, Oracle PIT maintenance stays deferred, and DB2 only has one accepted future ordinary hub-parent full-rebuild lane.
- This ticket remains the documentation and evidence child under story 06FH8R9DPSKTNYB46HHVJMZ9P8; sibling save ticket 06FH8RC9F0QEWF356WF7YYNNGM and read ticket 06FH8RDS25081N5S181C7TQGTG are already done.

Scope In
- Ratify the existing matrices and documentation surfaces as the authoritative provider-parity evidence baseline.
- Publish guidance that closed provider rows are cited by scenario, provider, baseline, and posture, with the matching closure-bundle artifact triplets as evidence.
- Preserve explicit caveats for provider-neutral fallback, supported latest-satellite shape, maintained PIT and bridge prerequisites, and non-goal boundaries.
- Keep the current save, read, and documentation split intact and limit this ticket to documentation and evidence publication scope.

Scope Out
- Fresh benchmark reruns or external-provider provisioning.
- Provider runtime code, benchmark schema, or diagnostics-contract changes.
- Reopening closed PostgreSQL, SQL Server, MySQL, Oracle, or DB2 save, latest-satellite, PIT, or bridge timing rows as unmeasured gaps.
- PIT maintenance implementation, bridge-maintenance push-down, staged DB2 bulk, provider-native chunk execution, or other future capability expansions.

Open questions
- none

Follow-up questions
- Should the owner branch create one separate DB2 PIT maintenance implementation child for the accepted IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) lane?
- If a later parity pass reopens benchmark work, should it be limited to maintenance-specific evidence lanes such as MySQL PIT full-rebuild timing rather than the already closed save and read rows?

Risks
- The current ticket description still reads like a request to run or collect benchmarks, which could duplicate the already checked-in closure bundle if the scope is not ratified.
- Because the repository-root benchmark-summary files still show skipped optional-provider rows, reviewers can misread placeholders as missing evidence unless the closure bundle and matrices stay explicit.
- The accepted DB2 PIT maintenance lane is not yet materialized as a child ticket, so that future work can get lost between documentation closure and later delivery.
- Historical block relations from done tickets can confuse workflow history until relation cleanup happens.

Split recommendations
- Do not split the current ticket further for save, latest-satellite, PIT, or bridge work; those implementation lanes are already handled by sibling tickets.
- Create at most one additional child only if the team wants to pursue DB2 PIT maintenance now, and limit it to IDataVaultProviderPitMaintenanceStrategy push-down for IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...).
- Keep Oracle PIT maintenance, MySQL PIT maintenance timing evidence, bridge-maintenance push-down, staged DB2 bulk, and provider-native chunk execution as separate later tickets rather than enlarging this documentation ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment