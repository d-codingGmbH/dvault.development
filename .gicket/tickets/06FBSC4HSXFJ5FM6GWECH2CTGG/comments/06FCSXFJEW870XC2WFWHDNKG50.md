[gicket-bot] PO refinement contract

Summary
- Refined the story into one bounded documentation deliverable: publish a repository-backed provider optimization gap matrix from the existing evidence matrix and benchmark bundle; no split or durable planning write is needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Live gicket reads for the ticket, comments, relations, and attachments were trust-blocked, so this refinement uses the provided ticket snapshot plus repository evidence from the checked-out branch.
- The repository already fixes the current baseline: SQLite is the only repository-proven optimized latest-satellite read path; PostgreSQL, SQL Server, MySQL, Oracle, and DB2 expose diagnostics-gated PIT and bridge candidates; save-strategy packages exist for SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- The matrix must separate capability gaps from evidence gaps: non-SQLite latest-satellite rows show no provider-specific optimized strategy registered, while most external-provider save and PIT and bridge rows already have planned or diagnostics-backed strategies but lack completed timing evidence.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized because the current evidence supports one bounded story.

Scope In
- Publish one provider optimization gap matrix in the repository documentation set, derived from docs/plans/provider-optimization-evidence-matrix.md and the root benchmark triplet.
- Cover save provider-native-bulk-ingestion and read latest-satellite-read, pit-as-of-read, and bridge-traversal-read across SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- Prioritize rows by visible repository posture: completed timing, skipped placeholder, diagnostics-only or smoke-only, and whether a provider-specific strategy already exists.
- Record a finite stop condition or fallback boundary for each gap instead of open-ended performance aspirations.

Scope Out
- Adding or changing provider strategies, benchmarks, release notes, or runtime code.
- Re-running external-provider benchmarks or provisioning databases, credentials, or connection strings.
- Expanding beyond save and read strategy work into live-schema reading, SQL artifact export, design-time workflow, or package-publication work.
- Treating SQLite completed evidence rows as new backlog gaps rather than reference baselines unless they are explicitly used to explain why future work should stop.

Open questions
- none

Follow-up questions
- After the matrix is published, should implementation work split first by scenario family or by provider?
- Should future tickets collect completed external-provider timing bundles before any new non-SQLite latest-satellite strategy design begins?
- If later prioritization needs customer adoption weighting, who will provide provider-usage evidence beyond the repository-only baseline used here?

Risks
- Live ticket, comment, relation, and attachment state could not be re-read through the local gicket transport because the fallback CLI calls were trust-blocked; this refinement assumes the prompt snapshot is authoritative.
- Expected-benefit ranking is repository-evidence-based rather than customer-usage-based, so a later portfolio pass could reprioritize providers if adoption data differs.
- The matrix can drift back into release-posture prose unless it stays anchored to the canonical evidence matrix and root benchmark triplet.

Split recommendations
- No split is needed for this story; one bounded documentation artifact is justified by the current branch evidence.
- If downstream implementation tickets are created from the published matrix, split them by gap family rather than by one large provider-optimization umbrella: non-SQLite latest-satellite capability work, external-provider read evidence work, and external-provider save evidence work.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment