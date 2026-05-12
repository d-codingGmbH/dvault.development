[gicket-bot] PO refinement contract

Summary
- Verified the ticket, comment/relation state, child-ticket delivery, current source/tests, and release-note context; rewrote the parent story as a completion/consistency umbrella, made hierarchy bridge depth explicitly required and bounded, and clarified that consumer-facing release-note follow-up is already owned by downstream docs ticket 06F0MEJPGG7JBFEXD693BHY07W.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The hierarchy bridge criterion is now explicit: many-to-many bridge requests may filter from From or To and must not supply maximumDepth, while hierarchy bridge requests may filter from Ancestor or Descendant and must supply a non-negative bounded maximumDepth. Missing or negative maximumDepth is a validation failure, not optional behavior.
- critic-item-2: `answered` - Repository and ticket state show this parent story is an umbrella/completion story, not a fresh dev implementation ticket. All four existing parentOf children are already done, and the parent branch HEAD still equals scratch-source-ref 4338cbbe21387b187adada102418f5039d09c72d, so this PO pass is reconciling delivered scope and routing expectations rather than opening new parent-branch implementation work.
- critic-item-3: `answered` - Release-note and changelog consistency is already downstream work, not new implementation scope on this parent story. The current v0.6.0 notes still say PIT-backed reads and bridge helpers are not delivered, but the existing docs/release task 06F0MEJPGG7JBFEXD693BHY07W owns the next consumer-facing wording update and is already blocked by both implementation children 06F0MEH660Y5QTNR5P8JPS2QXC and 06F0MEHKYTBJEJH2DVZ2CFH9Z0.
- critic-item-4: `answered` - The delivery contract no longer describes hierarchy depth as optional. It now matches the implemented public surface and tests: hierarchy bridge reads require a bounded non-negative maximumDepth and fail validation before query orchestration when that requirement is not met.

Clarifications
- The ticket comment history currently contains bot-authored workflow, prior PO refinement, and PO-critic artifacts only; no human scope-change comment was found during this pass.
- No ticket-local attachment files were found; the referenced repository documents and current source/test layout were sufficient planning context for this refinement.
- Parent/child structure remains unchanged: epic 06F0MEDTB8496GYVM9K42F9VPG is parentOf this story, and this story is parentOf 06F0MEGYHADPVN575H64D56W2G, 06F0MEH660Y5QTNR5P8JPS2QXC, 06F0MEHDFYCVK42FFY77FXHXBR, and 06F0MEHKYTBJEJH2DVZ2CFH9Z0.
- This parent story should now be treated as umbrella/completion scope over already-delivered PIT and bridge read-helper work, not as a request for new parent-branch implementation beyond the existing done child tickets.
- No child tickets, relation updates, planning documents, or attachments were created during this PO pass.

Scope In
- Story-level completion and contract consistency for the already-delivered provider-neutral PIT and bridge read-helper baseline across source, tests, public API snapshots, and child-ticket outcomes.
- Provider-neutral PIT as-of reads for one DataVaultPitMetadata declaration with one hub parent, ordered ordinary hub-attached satellites, a raw row API, and caller-owned typed projection helper behavior.
- Provider-neutral bridge raw-row and typed-projection helpers for many-to-many From/To traversal and hierarchy Ancestor/Descendant traversal over generated bridge shared-type tables.
- Deterministic validation and diagnostics for unsupported PIT or bridge metadata shapes, malformed generated EF shared-type metadata, and bounded hierarchy traversal request validation.
- Repository-level confirmation that remaining consumer-facing docs/release and benchmark follow-up already exists as downstream work instead of unresolved parent-story implementation scope.

Scope Out
- New parent-story implementation work beyond the four existing done child tickets.
- Provider-specific read optimization, SQL tuning, or provider-specific read-strategy expansion beyond already-tracked downstream work.
- PIT refresh, PIT maintenance orchestration, bridge row maintenance, closure computation, or any automatic population of maintenance tables.
- Unbounded hierarchy traversal, arbitrary graph/path querying, path payload columns, effectivity windows, EF relationships, or foreign-key/navigation behavior.
- Consumer-facing release-note or README edits themselves; those remain downstream documentation work rather than parent-story implementation scope.

Open questions
- none

Follow-up questions
- When docs/release ticket 06F0MEJPGG7JBFEXD693BHY07W resumes, should the stale v0.6.0 limitation text be amended directly, or should the next release notes supersede it as the authoritative consumer-facing update?
- When benchmark ticket 06F0MEJ0NE80R7CNS982S3PKVR resumes, which provider-specific read workloads still need measurement now that PIT and bridge helper surfaces are already present on branch?
- After documentation catches up, should README or quickstart examples explicitly show the bounded hierarchy maximumDepth requirement so consumers do not assume unbounded traversal?

Risks
- Consumers may still read docs/releases/v0.6.0.md and conclude PIT-backed reads and bridge helpers are absent until downstream docs/release work updates the consumer-facing notes.
- Hierarchy bridge reads depend on precomputed rows and a required bounded maximumDepth; they do not imply arbitrary recursive traversal or automatic closure maintenance.
- Consumers may expect PIT or bridge helpers to populate PIT/bridge maintenance tables; the read-only boundary must stay explicit in diagnostics and follow-up documentation.

Split recommendations
- No further split is recommended. The parent story is already decomposed into four done child tickets, and the remaining docs/release and benchmark work already exists as downstream tickets rather than missing child scope.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment