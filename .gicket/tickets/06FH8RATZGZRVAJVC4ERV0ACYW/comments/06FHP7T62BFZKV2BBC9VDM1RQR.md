[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the ticket is now a clear ratification/planning task over already-landed evidence, with no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The .gicket/tickets/06FH8RATZGZRVAJVC4ERV0ACYW/description.md contract names docs/plans/provider-optimization-gap-matrix.md and docs/plans/provider-optimization-evidence-matrix.md as authoritative, says the 2026-06-23 closure bundle is the completed-timing source, and keeps Open Questions at none.
- docs/plans/provider-optimization-gap-matrix.md says PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite, save, PIT, and bridge rows are closed by the 2026-06-23 closure bundle and keeps only bounded follow-up lanes in the Provider PIT Maintenance Expansion Decision Matrix.
- docs/plans/provider-optimization-evidence-matrix.md explicitly separates pit-full-rebuild-maintenance from read evidence and marks MySQL PIT maintenance as source/test-backed only, Oracle PIT maintenance deferred, and DB2 PIT maintenance outside completed timing evidence until a separate child lands.
- artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/README.md plus the checked-in DB2 and MySQL benchmark-summary.csv files confirm completed provider rows on 2026-06-23, including DB2 optimized save 101.037 ms / latest 14.615 / PIT 27.207 / bridge 4.831 and MySQL latest 13.878 / PIT 14.461 / bridge 3.083.
- The live relation files .gicket/relations/YW/P8/06FH8RATZGZRVAJVC4ERV0ACYW--06FH8R9DPSKTNYB46HHVJMZ9P8--blocks.json, .gicket/relations/YW/GM/06FH8RATZGZRVAJVC4ERV0ACYW--06FH8RC9F0QEWF356WF7YYNNGM--blocks.json, and .gicket/relations/YW/TG/06FH8RATZGZRVAJVC4ERV0ACYW--06FH8RDS25081N5S181C7TQGTG--blocks.json match the refined contract's parent/save/read split.
- git diff --stat 352549ada..HEAD over this ticket, the two matrix docs, and the closure bundle touched only .gicket/tickets/06FH8RATZGZRVAJVC4ERV0ACYW/*, and a repository-wide ticket search for DB2 PIT maintenance terms surfaced only the current ticket description/comments plus docs/plans/db2-pit-maintenance-full-rebuild-feasibility.md, so no separate DB2 PIT maintenance child ticket is currently visible in .gicket/tickets.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract leaves relation cleanup after the save/read implementation tickets finish as a follow-up question instead of a worked example.

Risky assumptions
- Downstream work will treat the 2026-06-23 save/read rows as closed evidence and will not reopen them just because the legacy draft mentioned rerunning benchmarks.
- The optional DB2 PIT maintenance lane will not be lost before someone creates the separate bounded child ticket described in the matrix and feasibility note.

AC / test suggestions
- If the team opens the DB2 PIT maintenance child, make its acceptance criteria require IBM.EntityFrameworkCore provider-name gating, clean-context gating, ordinary hub-parent RebuildAsync(...) only, and provider-neutral fallback when savepoint rollback is unproven.
- When the save/read child tickets are refined, copy the closed-row boundaries from docs/plans/provider-optimization-gap-matrix.md so those tickets do not reintroduce closed PostgreSQL, SQL Server, MySQL, Oracle, or DB2 rows as open work.

Implementation watchouts
- Treat this ticket as ratification of existing repository evidence; the current branch does not add new matrix or benchmark artifacts.
- Do not treat PIT or bridge read timing as PIT maintenance timing; the evidence matrix keeps those row families separate.
- Keep Oracle PIT maintenance, bridge-maintenance push-down, DB2 staged bulk, provider-native chunk execution, unsupported latest-satellite shapes, stale maintenance, and dirty-context save fallbacks out of scope unless separate tickets reopen them.

Non-blocking notes
- The inspected comment set for 06FH8RATZGZRVAJVC4ERV0ACYW was workflow/refinement history only; no unresolved human discussion was present.

Split recommendations
- Keep save strategy parity in 06FH8RC9F0QEWF356WF7YYNNGM and read parity in 06FH8RDS25081N5S181C7TQGTG.
- Only add one extra child if the team wants the accepted DB2 lane now, and keep it limited to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) full-rebuild push-down through IDataVaultProviderPitMaintenanceStrategy.
- Do not fold Oracle PIT maintenance reopen work, MySQL PIT maintenance timing evidence work, or bridge-maintenance push-down into this ticket or the existing save/read children.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment