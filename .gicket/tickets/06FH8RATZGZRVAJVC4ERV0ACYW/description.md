<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence already contains the refreshed provider optimization gap matrix and closure bundle; this ticket should ratify that baseline, keep the existing save/read child split, and treat only the remaining bounded maintenance and fallback items as follow-up work.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Use docs/plans/provider-optimization-gap-matrix.md and docs/plans/provider-optimization-evidence-matrix.md as the authoritative outcome of this ticket; the branch already includes the refreshed matrix and the 2026-06-23 provider-configured closure bundle.
- Do not plan fresh benchmark reruns in this ticket: the current repository baseline already carries completed PostgreSQL, SQL Server, MySQL, Oracle, and DB2 save, latest-satellite, PIT, and bridge timing rows in artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-20260623/.
- Treat the current relation set as intentional planning structure: the ticket is a child of story 06FH8R9DPSKTNYB46HHVJMZ9P8 and currently blocks that story plus implementation tasks 06FH8RC9F0QEWF356WF7YYNNGM and 06FH8RDS25081N5S181C7TQGTG.
- PIT and bridge maintenance scope is already narrowed by repository evidence: MySQL ordinary hub-parent PIT full rebuild is landed, Oracle PIT maintenance stays deferred, bridge-maintenance push-down remains outside the accepted lanes, and DB2 has one accepted future ordinary hub-parent PIT full-rebuild lane but no visible implementation child ticket yet.

### Scope In
- Ratify the existing provider optimization gap matrix as the authoritative parity-gap planning surface for save, latest-satellite read, PIT read, bridge read, and PIT full-rebuild maintenance decisions.
- Use the matrix to choose which remaining rows are implement-now, evidence-only, or defer-lane follow-up work instead of reopening already closed evidence rows.
- Keep the already-created save and read implementation children aligned to the matrix boundaries.
- Document the remaining maintenance decisions and fallback-only boundaries without turning them into implicit new implementation scope inside this ticket.

### Scope Out
- Running new benchmarks or provisioning external provider infrastructure in this ticket.
- Changing provider runtime code, diagnostics, or benchmark schemas in this planning ticket.
- Reopening PostgreSQL, SQL Server, MySQL, Oracle, or DB2 save, latest-satellite, PIT, or bridge rows that the closure bundle already marks as completed timing.
- Treating unsupported-shape, dirty-context, stale-maintenance, bridge-maintenance push-down, staged DB2 bulk, or provider-native chunk execution boundaries as in-scope implementation work without separate tickets.

## Acceptance Criteria
- The ticket contract explicitly points to docs/plans/provider-optimization-gap-matrix.md and docs/plans/provider-optimization-evidence-matrix.md as the canonical row and decision surfaces instead of asking for a new matrix format.
- The contract states that the 2026-06-23 closure bundle is the completed-timing source for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 save, latest-satellite, PIT, and bridge rows, so those rows are treated as closed evidence rather than open parity gaps.
- The contract identifies the only remaining bounded parity follow-up classes: implementation work already split into save ticket 06FH8RC9F0QEWF356WF7YYNNGM, read ticket 06FH8RDS25081N5S181C7TQGTG, one recommended DB2 PIT maintenance implementation child, and deferred or evidence-only lanes that stay outside the current implementation tickets.
- PIT maintenance language stays separate from PIT read timing: MySQL maintenance is source and test backed but still unmeasured, Oracle remains deferred, and DB2 is limited to one future ordinary hub-parent full-rebuild candidate with provider-neutral fallback preserved until a child lands.
- No acceptance text in this ticket requires new benchmark execution, release-note work, or provider code changes.

## Definition of Done
- The refined ticket makes the existing matrix and closure artifacts the authoritative planning baseline for downstream work.
- Closed provider save and read rows are not restated as open work, and remaining boundaries are classified as implement-now, evidence-only, or defer with a finite reason.
- The existing save and read child split remains aligned to the matrix, and any additional maintenance follow-up is described as a separate bounded child rather than folded into the current tasks.
- No blocking PO questions remain once the ticket is interpreted as ratification and prioritization of existing repository evidence.

## Implementation Notes
- Repository evidence already satisfies the refresh part of the draft through docs/plans/provider-optimization-gap-matrix.md, which now cites the closure bundle and a PIT maintenance decision matrix; refinement should ratify that document instead of creating a competing planning surface.
- Current actionable implementation work is already partly materialized: save parity belongs in 06FH8RC9F0QEWF356WF7YYNNGM, read parity belongs in 06FH8RDS25081N5S181C7TQGTG, and the remaining unmaterialized implement-now maintenance lane is the accepted DB2 ordinary hub-parent PIT full rebuild through IDataVaultProviderPitMaintenanceStrategy.
- Treat MySQL PIT maintenance timing as evidence-only follow-up, not as an open implementation gap, because the implementation lane already landed but lacks a dedicated pit-full-rebuild-maintenance artifact triplet.
- Keep Oracle PIT maintenance, bridge-maintenance push-down, DB2 staged bulk, provider-native chunk execution, unsupported latest-satellite shapes, stale PIT or bridge maintenance, and dirty-context save fallbacks as deferred boundary work unless a separate ticket explicitly reopens them.
- No bounded planning writes were materialized in this run; the refinement relies on existing repository documents and live .gicket relation state.

## Open Questions
- none

## Follow-Up Questions
- Should the owner branch materialize the missing DB2 PIT maintenance implementation child now so the accepted maintenance lane is tracked beside the existing save and read children?
- After the current save and read children land, should any future parity pass reopen only maintenance-specific evidence lanes such as MySQL PIT maintenance timing rather than closed save and read rows?
- When the blocking implementation tickets complete, should the remaining blocks chain be simplified so this planning ticket no longer blocks the parent story?

## Risks
- The ticket draft still mentions rerunning targeted benchmarks, so without this refinement downstream work could duplicate the already checked-in 2026-06-23 closure evidence.
- Because no DB2 PIT maintenance implementation child is currently visible, the accepted DB2 maintenance lane could be lost between planning and delivery.
- The v2 gap matrix is closure-oriented; if downstream tickets treat every remaining fallback boundary as implementation scope, they will overrun the bounded parity plan.

## Split Recommendations
- Do not split the current ticket further for save or read work; those paths already have bounded implementation children 06FH8RC9F0QEWF356WF7YYNNGM and 06FH8RDS25081N5S181C7TQGTG.
- Create one additional child only if the team wants to pursue the accepted DB2 PIT maintenance lane now: limit it to IBM.EntityFrameworkCore ordinary hub-parent RebuildAsync(...) full-rebuild push-down through IDataVaultProviderPitMaintenanceStrategy.
- Keep Oracle PIT maintenance reopen work, MySQL PIT maintenance timing evidence, and any bridge-maintenance push-down or staged DB2 bulk work as separate later tickets rather than enlarging the current parity implementation children.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Review current benchmark artifacts and rerun targeted benchmark lanes where available. Produce or update a provider optimization gap matrix that identifies concrete gaps by provider and path: save, latest-satellite read, PIT read, bridge read, and PIT/bridge maintenance. Mark each gap as implement-now, evidence-only, or defer with reason.