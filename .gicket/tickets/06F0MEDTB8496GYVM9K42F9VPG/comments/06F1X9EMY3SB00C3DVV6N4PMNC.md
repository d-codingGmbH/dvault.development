[gicket-bot] PO refinement contract

Summary
- Refined the epic as a closure-ready umbrella over four already-materialized delivery tracks: model-first import, model export/drift tooling, PIT/bridge read helpers, and provider-aware read optimization follow-up. Current branch docs and source already establish the bounded v0.7 model-first and advanced read-model baseline, so no new child tickets, relation changes, attachments, or planning documents were needed in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The ticket snapshot shows no recent comments, so this pass ratifies branch evidence and existing child-ticket outcomes rather than responding to a new human scope change.
- The model-first baseline is already fixed on branch as governed JSON-first dvault.model.v1 with exact schemaVersion matching, canonical hubs/links/satellites/pits/bridges categories, strict unknown-field rejection, declaration-order preservation, naming.policy default = default, and loadTimestampStorage default = provider-default.
- The advanced read-model baseline is already bounded on branch to implemented latest/as-of satellite reads, provider-neutral PIT-backed as-of reads, provider-neutral bridge reads, and provider-aware optimization hooks rather than PIT refresh, bridge maintenance, unbounded graph traversal, or blanket provider-specific SQL behavior.
- Already-created child tickets cover the epic split: 06F0MEE0NC2009J73PP0ATE6YW for model-first import, 06F0MEF8N9DXDW01FXYZAEB6T8 for model export and drift tooling, 06F0MEGPPETJD4ZDEN5ESGR7JW for PIT and bridge read/query helpers, and 06F0MEHSH6S31ZE4K0Q3EKR784 for provider-aware read optimization follow-up; all four are already done.
- docs/releases/v0.7.0.md is the current branch release baseline for this epic; docs/releases/v0.6.0.md remains historical context and should not reopen already-delivered model-first, PIT, or bridge scope.
- No new child tickets, relation updates, attachments, or planning documents were materialized during this refinement pass.

Scope In
- Epic-level closure and contract consistency across the existing child deliveries for model-first import, export/drift, PIT/bridge reads, and provider-aware optimization.
- Governed dvault.model.v1 import, canonical export, drift comparison, and projection into the existing registry and EF metadata path used by DVault.
- Provider-neutral PIT as-of reads and bridge traversal read helpers backed by implemented metadata, raw-row contracts, typed projector helpers, and bounded diagnostics.
- Benchmark-informed provider-aware read-strategy hooks where optimized paths remain additive to provider-neutral fallback behavior.

Scope Out
- Replacing the existing Code-First or metadata-first declaration paths.
- Direct YAML ingestion, YAML-specific parser semantics, or a core YAML dependency.
- PIT refresh or maintenance orchestration, bridge row maintenance or closure generation, unbounded graph traversal, or broad ORM-style abstraction over Data Vault semantics.
- Provider-specific optimization for every provider and every read shape in this epic.
- Automatic database, container, or benchmark-environment provisioning.

Open questions
- none

Follow-up questions
- Should a later README or quickstart update make the bounded hierarchy maximumDepth requirement more explicit for bridge-read consumers?
- Should later provider-specific work optimize PIT or bridge reads beyond the current benchmark-backed latest/as-of satellite optimization baseline?
- Should later release or CI work surface drift reports and benchmark artifacts more explicitly once the manual governance workflow has settled?

Risks
- Historical docs such as docs/releases/v0.6.0.md can still be misread as current capability posture unless reviewers anchor on docs/releases/v0.7.0.md and the refined child-ticket contracts.
- Consumers may overread the advanced read-model baseline as including PIT refresh, bridge maintenance, or unbounded traversal unless downstream docs continue to keep those boundaries explicit.
- Provider-aware optimization claims should stay tied to benchmarked, branch-visible evidence so the epic is not interpreted as blanket optimization coverage for every provider or read shape.

Split recommendations
- No further split is recommended. The epic is already decomposed into four done child tickets covering model-first import, export/drift tooling, PIT/bridge read helpers, and provider-aware optimization follow-up.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment