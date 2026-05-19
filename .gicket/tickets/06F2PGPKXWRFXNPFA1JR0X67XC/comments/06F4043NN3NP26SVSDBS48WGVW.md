[gicket-bot] PO refinement contract

Summary
- Refined the story against the repository's existing latest/as-of read baseline, clarified that current means the latest visible row, and aligned historical reads with the maintained-PIT boundary; no child tickets, relation changes, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence fixes the public naming baseline as latest/as-of reads; for this ticket, current means the latest visible row when no AsOf cutoff is supplied, not a breaking rename to a new API family.
- The ergonomic entry point stays IDataVaultReadService with additive typed projection helpers and registry-backed adapters for UseDataVaultMetadata() callers; do not introduce a second read-service abstraction for this story.
- Historical multi-satellite as-of reads remain the separate PIT-backed path using DataVaultPitAsOfReadRequest, ReadPitRowsAsync(...), and ReadPitAsync(...), and this ticket may assume PIT tables are already maintained per docs/plans/pit-maintenance-service-v1-contract.md.
- Existing latest/as-of compatibility for hub-parent satellites, link-parent satellites, and multi-active driving-key series is part of the preserved baseline and must not regress.
- No child tickets, relations, attachments, or planning documents were created or changed in this refinement run.

Scope In
- Additive improvements to typed latest/current and as-of satellite read ergonomics on top of IDataVaultReadService.
- Support for both explicit metadata requests and registry-backed DataVaultRegistryLatestSatelliteReadRequest flows for DbContexts using UseDataVaultMetadata().
- Preservation of current request semantics: UTC-normalized DateTimeOffset cutoffs, StringComparer.Ordinal parent-hash-key deduplication, deterministic ordering, and empty results for missing parents.
- Alignment of historical as-of ergonomics with the existing PIT-backed read path over already-maintained PIT rows when this story touches multi-satellite history.

Scope Out
- PIT row population, PIT rebuild or incremental maintenance semantics, and any read-time PIT refresh behavior.
- Provider-specific PIT read optimization or broader read-strategy work beyond the existing latest/as-of satellite strategy boundary.
- Bridge read or bridge maintenance changes unrelated to current/latest and as-of satellite ergonomics.
- A breaking rename or removal of the existing DataVaultLatestSatelliteReadRequest and ReadLatestSatelliteAsync(...) public surface.
- Unrelated repository refactors or broad release-note ownership that already belongs to sibling follow-up work.

Open questions
- none

Follow-up questions
- Should a later compatibility ticket add optional convenience aliases that say current while keeping the repository's latest/as-of vocabulary as the stable public baseline?
- After this story lands, does the doc/release-note follow-up need extra end-user examples that contrast latest satellite reads with PIT-backed as-of reads over maintained PIT tables?
- Is the separate PIT read-optimization follow-up 06F2PGPRGN0EVGD6RY5KY9M56W still desired once the ergonomic story scope is settled?

Risks
- The ticket title says current while repository evidence consistently says latest/as-of; without the clarification above, implementation could drift into an unnecessary public API rename.
- Historical as-of ergonomics already span two distinct paths, latest-satellite reads and PIT-backed reads; collapsing them would blur established maintenance, diagnostics, and compatibility boundaries.
- Live gicket re-reads for ticket relations, comments, and attachments were trust-blocked in this run, so this refinement relies on the provided ticket snapshot plus repository evidence and does not claim relation cleanup or attachment changes.

Split recommendations
- No additional split is recommended from PO refinement; repository evidence already establishes adjacent ticket boundaries for PIT maintenance behavior, PIT read optimization (06F2PGPRGN0EVGD6RY5KY9M56W), and broader doc/release-note follow-through (06F2PGPXVAYRBC94RQ7X5V4DVG).

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment