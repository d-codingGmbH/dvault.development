[gicket-bot] PO refinement contract

Summary
- Repository evidence supports a bounded recommendation: keep privacy workflow status/effectivity modeling on existing ordinary or link-parent satellite surfaces, do not add first-class STS/RTS core semantics, and keep this ticket documentation-only. No child tickets, planning documents, attachments, or durable ticket edits were materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Treat the STS/RTS wording in this ticket as status/effectivity-style satellite patterns only; the current repository baseline does not define separate STS or RTS metadata kinds, table kinds, or builders.
- The visible core modeling baseline is finite: `DataVaultTableKind` exposes `Hub`, `Link`, `Satellite`, `PointInTime`/`Pit`, and `Bridge`, and `DataVaultSatelliteMetadata` supports ordinary and multi-active satellites only.
- Current repository guidance already ratifies the default effectivity posture: model relationship status/effectivity as caller-owned link-parent satellite state with optional `DrivingKey(...)`, not as a first-class effectivity-specific entity family.
- The optional privacy boundary is already defined by done ticket `06FE4R9PP99G6Q1PTPK4TKD460`; privacy behavior remains an opt-in add-on layered on existing metadata, save, read, and provider-extension seams rather than a core semantic change.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement pass. The live relation graph still shows an incoming `blocks` edge from done ticket `06FE4R9PP99G6Q1PTPK4TKD460`, so treat that edge as stale historical routing rather than an active blocker.

Scope In
- Evaluate whether privacy workflows need first-class STS/RTS semantics in DVault core modeling.
- Ratify the v1 default that privacy status/effectivity state should reuse existing hub-parent satellites, link-parent satellites, and optional multi-active driving-key semantics.
- Define how any future privacy-specific interpretation or helper layer stays inside the optional privacy add-on boundary without changing core Data Vault semantics.
- Produce a documentation-level recommendation that downstream privacy documentation or example work can reference without reopening the architecture baseline.

Scope Out
- Introducing a new `DataVaultTableKind`, metadata kind, builder, or technical-column family for STS/RTS.
- Changing core save, read, PIT, bridge, hashing, or telemetry runtime behavior for this ticket.
- Implementing provider-native encryption, key lifecycle management, retention engines, deletion orchestration, or compliance guarantees.
- Broad product-code work beyond a focused recommendation or documentation update.

Open questions
- none

Follow-up questions
- If a real privacy use case later cannot be expressed with ordinary or link-parent satellites plus driving keys, what concrete gap justifies reopening core semantics instead of adding add-on metadata or helper behavior?
- Should the eventual privacy documentation include one small worked example of consent or relationship effectivity modeled as link-parent satellite state so the recommendation is concrete for consumers?
- If later reader-facing documentation wants stronger STS/RTS terminology, should that remain terminology-only guidance or be backed by a separate add-on helper contract?

Risks
- Downstream work may over-interpret STS/RTS wording as approval for new core table kinds even though the repository baseline intentionally keeps effectivity generic.
- Privacy documentation can drift into provider-specific or compliance-guarantee language if it stops anchoring on the optional boundary already defined by done ticket `06FE4R9PP99G6Q1PTPK4TKD460`.

Split recommendations
- No split is needed if this ticket remains a documentation/recommendation lane.
- If future work reopens semantics, split it into one architecture ticket for any add-on metadata/helper contract and separate implementation tickets per provider or documentation lane rather than broadening this evaluation ticket.
- Do not create a child ticket for first-class STS/RTS core modeling unless a concrete gap is demonstrated beyond the existing ordinary, link-parent, and multi-active satellite surfaces.

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