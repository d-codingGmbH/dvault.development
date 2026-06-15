<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story into one bounded documentation deliverable: publish a repository-backed provider optimization gap matrix from the existing evidence matrix and benchmark bundle; no split or durable planning write is needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Live gicket reads for the ticket, comments, relations, and attachments were trust-blocked, so this refinement uses the provided ticket snapshot plus repository evidence from the checked-out branch.
- The repository already fixes the current baseline: SQLite is the only repository-proven optimized latest-satellite read path; PostgreSQL, SQL Server, MySQL, Oracle, and DB2 expose diagnostics-gated PIT and bridge candidates; save-strategy packages exist for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- The matrix must separate capability gaps from evidence gaps: non-SQLite latest-satellite rows show no provider-specific optimized strategy registered, while most external-provider save and PIT and bridge rows already have planned or diagnostics-backed strategies but lack completed timing evidence.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized because the current evidence supports one bounded story.

### Scope In
- Publish one provider optimization gap matrix in the repository documentation set, derived from docs/plans/provider-optimization-evidence-matrix.md and the root benchmark triplet.
- Cover save provider-native-bulk-ingestion and read latest-satellite-read, pit-as-of-read, and bridge-traversal-read across SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- Prioritize rows by visible repository posture: completed timing, skipped placeholder, diagnostics-only or smoke-only, and whether a provider-specific strategy already exists.
- Record a finite stop condition or fallback boundary for each gap instead of open-ended performance aspirations.

### Scope Out
- Adding or changing provider strategies, benchmarks, release notes, or runtime code.
- Re-running external-provider benchmarks or provisioning databases, credentials, or connection strings.
- Expanding beyond save and read strategy work into live-schema reading, SQL artifact export, design-time workflow, or package-publication work.
- Treating SQLite completed evidence rows as new backlog gaps rather than reference baselines unless they are explicitly used to explain why future work should stop.

## Acceptance Criteria
- A published gap matrix cites the canonical repository sources instead of restating raw benchmark prose: docs/plans/provider-optimization-evidence-matrix.md, benchmark-summary.md, docs/performance-profiles.md, docs/architecture/dvault-v1-explicit-save-service.md, docs/architecture/dvault-v1-pit-bridge-boundary.md, and the relevant release notes.
- Each row represents one bounded provider and scenario gap and includes provider, scenario, current baseline or planned strategy, measured evidence or explicit missing-evidence note, expected benefit, current evidence posture, and a finite stop condition or fallback boundary.
- The matrix explicitly separates capability gaps from evidence gaps: non-SQLite latest-satellite rows are documented as no provider-specific optimized strategy registered, while PostgreSQL, SQL Server, MySQL, Oracle, and DB2 save and PIT and bridge rows are documented as strategy-present but timing-missing or diagnostics-only where applicable.
- DB2 rows preserve the narrower v0.34.0 posture: clean-context save and PIT and bridge candidate behavior may be documented from diagnostics and smoke evidence, but no completed DB2 timing, latest-satellite optimization, staged bulk, or provider-native chunk claim is added.
- The published priority order is deterministic and repository-backed, with non-SQLite latest-satellite capability gaps ahead of external-provider timing-evidence gaps for save and PIT and bridge paths that already have registered strategies.

## Definition of Done
- The matrix is stored under a planning-oriented docs/plans path consistent with existing provider planning documents.
- Each row uses current repository vocabulary such as completed-timing, skipped-placeholder, diagnostics-only, smoke-only, and the closed save and read fallback boundary language where relevant.
- No row implies measured provider performance from skipped, diagnostics-only, smoke-only, or storage-footprint evidence.
- No row invents scenarios, providers, baselines, or stop conditions that are not already supported by checked-in evidence and documentation.
- The document makes it obvious which future tickets are evidence-collection work versus strategy-expansion work.

## Implementation Notes
- Use docs/plans/provider-optimization-gap-matrix.md as the default output path so the new matrix sits beside the canonical evidence matrix.
- Use SQLite completed timing rows as the reference baseline, not as open work: latest-satellite-read and bridge-traversal-read show measured SQLite optimized paths, while SQLite pit-as-of-read provides a bounded capability reference without forcing new optimization scope.
- Treat PostgreSQL, SQL Server, MySQL, and Oracle provider-native-bulk-ingestion, pit-as-of-read, and bridge-traversal-read rows as evidence gaps first: strategies are visible in registrations and benchmark guidance rows, but the root benchmark bundle records executionStatus skipped because connection strings were unset.
- Treat PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite-read rows as capability gaps first: the benchmark guidance rows explicitly record that no provider-specific latest-satellite strategy is registered outside SQLite.
- Keep DB2 wording narrower than the other providers: docs prove diagnostics-gated clean-context save behavior plus PIT and bridge candidate behavior and smoke coverage, but not completed timing, latest-satellite optimization, staged bulk, or provider-native chunk execution.
- A practical row shape is priority, gap kind, provider, scenario, current posture, current baseline or strategy, missing proof or measured comparator, expected benefit, stop condition, and source links.

## Open Questions
- none

## Follow-Up Questions
- After the matrix is published, should implementation work split first by scenario family or by provider?
- Should future tickets collect completed external-provider timing bundles before any new non-SQLite latest-satellite strategy design begins?
- If later prioritization needs customer adoption weighting, who will provide provider-usage evidence beyond the repository-only baseline used here?

## Risks
- Live ticket, comment, relation, and attachment state could not be re-read through the local gicket transport because the fallback CLI calls were trust-blocked; this refinement assumes the prompt snapshot is authoritative.
- Expected-benefit ranking is repository-evidence-based rather than customer-usage-based, so a later portfolio pass could reprioritize providers if adoption data differs.
- The matrix can drift back into release-posture prose unless it stays anchored to the canonical evidence matrix and root benchmark triplet.

## Split Recommendations
- No split is needed for this story; one bounded documentation artifact is justified by the current branch evidence.
- If downstream implementation tickets are created from the published matrix, split them by gap family rather than by one large provider-optimization umbrella: non-SQLite latest-satellite capability work, external-provider read evidence work, and external-provider save evidence work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Turn collected evidence into a prioritized provider gap matrix for later save/read strategy work. Acceptance: each gap has provider, scenario, measured evidence or missing-evidence note, expected benefit, and stop condition.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Published the provider optimization gap matrix at `docs/plans/provider-optimization-gap-matrix.md`.
- The matrix keeps SQLite completed timing rows as reference baselines, prioritizes non-SQLite latest-satellite capability gaps first, and then lists external-provider save/PIT/bridge evidence gaps.
- DB2 wording stays inside the v0.34.0 boundary: clean-context save and PIT/bridge candidate behavior only, with no completed DB2 timing, latest-satellite optimization, staged DB2 bulk, or provider-native chunk execution claim.

Repository artifacts
- `docs/plans/provider-optimization-gap-matrix.md`

Verification
- `bash tools/check-format.sh` passed.
- `git diff --check -- docs/plans/provider-optimization-gap-matrix.md` passed.

Notes
- Documentation-only change; no provider strategies, benchmarks, runtime code, release notes, or external-provider benchmark runs were added.
<!-- gicket-bot:developer-delivery:v1:end -->