[gicket-bot] PO refinement contract

Summary
- Refined ticket 06F0MEJPGG7JBFEXD693BHY07W for documentation-only v0.7.0 readiness covering README, v0.7.0 release notes, and evidence-backed benchmark/package-verification summaries for model-first import/export and advanced read flows.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This is a documentation and release-notes task only; no package publishing, runtime behavior changes, or new API implementation are in scope.
- The v0.7.0 documentation baseline should treat docs/model-first-governance.md as authoritative for model-first governance, including the exact dvault.model.v1 schemaVersion, canonical JSON categories, strict unknown-field rejection, ordering preservation, JSON-first ingestion, EF projection, canonical export, and drift-report review evidence.
- README language must present Code-First, metadata-first, and model-first as distinct supported declaration paths rather than implying one replaces all others.
- Advanced read documentation may describe implemented latest/as-of satellite reads and provider-neutral PIT/bridge read behavior where present, but must not imply PIT row maintenance, bridge traversal maintenance, full graph traversal semantics, or unimplemented provider-specific read optimization.
- Bridge read examples should match the implemented provider-neutral bridge surface: endpoint hash-key filtering, many-to-many from/to endpoints, hierarchy ancestor/descendant endpoints, optional maximum depth for hierarchy reads, TraversalDepth for hierarchy rows, and typed projection delegates using exact generated column names.

Scope In
- Update README documentation for v0.7.0 model-first import/export/governance flows while retaining clear Code-First and metadata-first guidance.
- Add or update v0.7.0 release notes for the coordinated DVault package family, highlighting model-first governance artifacts and implemented advanced read-model behavior.
- Document read examples only for implemented behavior, including latest/as-of satellite reads and bounded PIT/bridge read examples supported by the current branch.
- Update benchmark summary text where read optimization evidence already exists in the repository or release artifacts; omit or qualify benchmark claims when evidence is absent.
- Keep package verification documentation accurate for the v0.7.0 branch without performing publishing work.

Scope Out
- Publishing NuGet packages or changing release automation.
- Implementing model-first APIs, read APIs, provider optimizations, tests, or benchmarks.
- Documenting YAML as a first-party ingestion format.
- Presenting PIT refresh, PIT row maintenance, bridge row maintenance, complex bridge traversal, full graph semantics, or provider-specific read strategies as delivered if they are not implemented.
- Creating subtickets or broad release-planning work beyond this documentation ticket.

Open questions
- none

Follow-up questions
- After release packaging, should the final v0.7.0 release notes be amended with exact package hashes, package verification output, or NuGet publication links?
- Should a later documentation ticket add a deeper model-first migration guide or cookbook once downstream users have exercised the governance workflow?
- Should future release notes split read-model performance evidence by provider once provider-specific read optimization exists?

Risks
- The main documentation risk is overstating PIT or bridge capabilities beyond the implemented provider-neutral read behavior; examples should stay narrowly tied to current code.
- Benchmark wording can become misleading if it summarizes planned optimizations instead of measured evidence already present in the branch.
- Release-note compatibility text should avoid making v0.6.0 historical limitations sound like current v0.7.0 behavior.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment