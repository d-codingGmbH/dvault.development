[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Provider PIT maintenance evidence is a distinct contract from PIT and bridge read evidence: pit-full-rebuild-maintenance is the only maintenance timing row family, while pit-as-of-read and bridge-traversal-read prove reads over already-maintained rows only.
- The accepted provider-maintenance baseline is intentionally asymmetric. PostgreSQL supports clean full rebuilds for ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs when no caller transaction is active. SQL Server supports clean ordinary hub-parent full rebuilds only. MySQL supports clean ordinary hub-parent full rebuilds only on MySql.EntityFrameworkCore. Oracle remains deferred. DB2 remains provider-neutral until a future ordinary hub-parent lane is implemented.
- Benchmark-backed maintenance claims must carry the preserved benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json triplet from the same run plus run context, maintenanceScope=FullRebuild, selected or planned strategy, fallback posture, and bounded fallback causes. Source and test backed availability alone is not a timing claim.

Scope In
- Define the evidence contract for provider-specific PIT full-rebuild maintenance rows, artifact requirements, and claim boundaries.
- Ratify the current provider-specific maintenance shape support and provider-neutral fallback boundaries for PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- Require the bounded diagnostics and fallback vocabularies used to explain selected strategy, declined strategy, or provider-neutral fallback for PIT maintenance claims.
- Keep the canonical documentation surfaces aligned: Provider Optimization Evidence Matrix, Performance Evidence And Benchmark Artifact Contract, Performance Profiles, DVault V1 PIT And Bridge Boundary, and the v0.47 release notes.

Scope Out
- Bridge maintenance push-down, bridge maintenance timing, or using bridge-traversal-read rows as maintenance evidence.
- Automatic PIT or bridge refresh, background scheduling, EF SaveChanges orchestration, or read-time maintenance.
- Oracle PIT maintenance implementation and DB2 PIT maintenance implementation beyond the current evidence contract.
- New benchmark runs, provider code changes, or widening provider maintenance shape support beyond the repository-proven v0.47 baseline.

Open questions
- none

Follow-up questions
- When provider-configured pit-full-rebuild-maintenance artifact triplets are later produced for SQL Server, MySQL, or a future DB2 lane, should the evidence matrix rows be promoted from source/test-backed or skipped-placeholder posture to completed-timing without reopening the v0.47 shape boundary?
- If the accepted future DB2 ordinary hub-parent full-rebuild implementation is opened, keep it as a separate ticket limited to IBM.EntityFrameworkCore, rollback-clean transaction behavior, and provider-strategy seam parity rather than widening this story.

Risks
- Downstream documentation or release summaries could still overstate PIT read rows or source/test-backed provider lanes as maintenance timing evidence if they stop citing the evidence matrix and artifact contract.
- The provider-maintenance baseline is intentionally asymmetric, so careless summaries can imply Oracle or DB2 parity that the repository does not currently implement or benchmark.

Split recommendations
- none

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