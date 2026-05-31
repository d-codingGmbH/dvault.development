[gicket-bot] PO refinement contract

Summary
- Durable ticket revision 06F7X4CJX92B885WN4WZ7AHAEG now explicitly marks epic 06F5Q93R4633D41Z21WQW3SVGR as tracking-only closure/no-work-required, preserves the five done child tickets and clean live relation graph, and makes the post-PO-critic path closure-only rather than dev-facing; no further bounded writes are needed in this turn.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The durable ticket contract now explicitly states that this parent epic is tracking-only closure/no-work-required and owns no direct implementation or documentation slice; authoritative delivery remains in the five existing child tickets.
- critic-item-2: `answered` - The durable handoff now makes the success path explicit: after a successful PO-critic review, this parent epic is treated as no-work-required closure tracking and must not be handed to ordinary developer implementation work.
- critic-item-3: `answered` - The contract preserves the existing clean-relation and child-completion evidence and aligns the durable handoff wording with closure-only intent. The remaining status and label transitions are runtime-managed workflow metadata, so they are not left as a PO blocker once this accepted contract returns the ticket to PO-critic.
- critic-item-4: `answered` - The workflow-contract mismatch identified by PO-critic is resolved in the durable contract text: the parent epic is now explicitly framed as tracking-only closure/no-work-required, with closure-eligibility verification as the only remaining parent action and no dev-facing implementation slice.

Clarifications
- The only persisted planning change needed for this PO pass is the durable ticket description update already visible at revision 06F7X4CJX92B885WN4WZ7AHAEG; no child-ticket, relation, attachment, or planning-document writes were required beyond that contract update.
- Live relation evidence remains clean at outgoingCount=5 and incomingCount=0, with only the five existing parentOf links to 06F5Q93YXHSKABD2SABWY85S78, 06F5Q9463M0RSHAJJX0F3D1DB0, 06F5Q94D0JDMMWDXSRGWX1E4F0, 06F5Q94KX65TXQ8EC75FWSD01W, and 06F5Q94SQ086B2DZ1AKFDXGV94.
- Current ticket context already records that all five v0.23.0 child tickets are done, so this parent epic stays on closure tracking only and does not reopen or rescope delivery.
- The authoritative repository baseline remains docs/architecture/dvault-v1-activity-tracing-contract.md, src/DCoding.Data.DVault/DataVaultActivityTracing.cs, docs/performance-profiles.md, benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, docs/releases/v0.23.0.md, and README.md.

Scope In
- Closure-only tracking of the five done child tickets and the clean live relation graph for epic 06F5Q93R4633D41Z21WQW3SVGR.
- Preservation of the landed repository baseline for activity tracing, performance guidance, benchmark evidence, release notes, and README guidance that the child tickets already delivered.
- Final closure eligibility verification that the five child tickets remain done and the live relation graph still shows incomingCount=0 before runtime closes the parent epic.

Scope Out
- Any direct product-code, documentation, benchmark, tracing, or implementation work in the parent epic.
- Any new tracing or performance slice, new child-ticket split, or reopened delivery unless later evidence explicitly reopens scope.
- Any exporter, collector, dashboard, alerting, scheduler, hosted worker, database or container provisioning, credential management, or package-publication automation work.

Open questions
- none

Follow-up questions
- Before runtime closes the epic, does a final eligibility check still show incomingCount=0 and the same five done child tickets after any integration activity?

Risks
- If any child ticket is reopened, the parent epic should stop at closure tracking until that child is done again.
- If another branch reintroduces a blocks relation or other parent-owned work signal during integration, rerun closure eligibility before final closure.

Split recommendations
- No additional split recommended; the existing five-child decomposition remains complete and the parent epic now carries only closure tracking.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment