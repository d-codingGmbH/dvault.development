[gicket-bot] PO refinement contract

Summary
- Refined the read-parity task around the current repository baseline: the branch already shows bounded provider read strategy registrations, parity/fallback tests, skipped-placeholder root rows, and the 2026-06-23 closure bundle, so no further read-ticket split is needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Treat `docs/plans/provider-optimization-gap-matrix.md` and `docs/plans/provider-optimization-evidence-matrix.md` as the authoritative decision and row-lookup surfaces for this ticket.
- The current completed-timing source for external-provider read closure is `artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/`; do not reopen those rows as unmeasured gaps.
- The repository-backed provider set for this task is PostgreSQL, SQL Server, MySQL, Oracle, and DB2, with SQLite as the local reference baseline.
- Latest-satellite scope stays limited to hub-parent, non-multi-active satellites; PIT and bridge scope stays limited to already-maintained read models with complete read-shape evidence and fresh maintenance.
- Provider-neutral fallback remains required for provider mismatch, unsupported shapes, incomplete read-shape evidence, or stale read-model maintenance signals.
- The incoming `blocks` relation from done ticket `06FH8RATZGZRVAJVC4ERV0ACYW` is historical routing context, not an active blocker, because the source ticket is `done` and this ticket is not marked blocked.

Scope In
- Selected latest-satellite and maintained PIT/bridge read-path parity work for the repository-backed external-provider set already evidenced in the branch.
- Shared relational read-pipeline changes and provider-specific read tuning that preserve row/projection parity with the provider-neutral path.
- Unit and benchmark-verifier coverage that proves strategy selection, fallback boundaries, and correct evidence posture for completed versus skipped provider rows.

Scope Out
- Save-path threshold or staged-bulk work owned by sibling ticket `06FH8RC9F0QEWF356WF7YYNNGM`.
- Documentation, performance-profile, and release-note work owned by sibling ticket `06FH8REKX113JRZQ42HEB1NVZ8`.
- Provider-specific PIT maintenance expansion, bridge-maintenance push-down, staged DB2 bulk, and provider-native chunk execution.
- Fresh provider benchmarking or infrastructure provisioning beyond the checked-in root triplet and 2026-06-23 closure bundle.

Open questions
- none

Follow-up questions
- Should a later maintenance-focused child be created for the accepted DB2 ordinary hub-parent PIT full-rebuild lane rather than reopening closed read rows?
- Once the read and save implementation children finish, should the remaining parent-story blocking chain be simplified before closure?

Risks
- If downstream work treats remaining fallback boundaries as open implementation gaps, this ticket will sprawl into PIT maintenance, bridge maintenance, or save-path work that already belongs elsewhere.
- The root `benchmark-summary.*` files still show skipped external-provider read rows; without explicit closure-bundle citation, reviewers can misread intentional placeholders as missing evidence.
- The stale inbound `blocks` relation from the done matrix-refresh ticket can confuse workflow history until it is cleaned up.

Split recommendations
- Do not split this ticket further; save-path work and documentation work already have separate bounded children.
- If the team wants additional implementation after this ticket, create one separate DB2 PIT maintenance child limited to `IBM.EntityFrameworkCore` ordinary hub-parent `RebuildAsync(...)` push-down through `IDataVaultProviderPitMaintenanceStrategy`.
- Any Oracle PIT maintenance reopen, MySQL PIT maintenance timing evidence, bridge-maintenance push-down, or DB2 staged-bulk follow-up should stay in separate later tickets.

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