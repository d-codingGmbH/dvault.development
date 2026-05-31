[gicket-bot] PO-critic review contract

Summary
- Five child tickets are done and the landed repo surfaces match the tracking epic, but the stale incoming blocker relation still persists locally and replay completion is not yet directly evidenced.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F5Q93R4633D41Z21WQW3SVGR/description.md marks the epic as a tracking-only five-child v0.23.0 coordination ticket, shows Open Questions as none, and states closure requires the stale-blocker replay plus a live relation graph that no longer shows 06F5Q93H60W6X8FJ88PWTR6NG4 blocking the epic.
- All five child tickets are persisted as done in .gicket/tickets/06F5Q93YXHSKABD2SABWY85S78/ticket.json, .gicket/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/ticket.json, .gicket/tickets/06F5Q94D0JDMMWDXSRGWX1E4F0/ticket.json, .gicket/tickets/06F5Q94KX65TXQ8EC75FWSD01W/ticket.json, and .gicket/tickets/06F5Q94SQ086B2DZ1AKFDXGV94/ticket.json.
- The epic still has exactly five persisted parentOf relations at .gicket/relations/GR/78/06F5Q93R4633D41Z21WQW3SVGR--06F5Q93YXHSKABD2SABWY85S78--parentOf.json, .gicket/relations/GR/B0/06F5Q93R4633D41Z21WQW3SVGR--06F5Q9463M0RSHAJJX0F3D1DB0--parentOf.json, .gicket/relations/GR/F0/06F5Q93R4633D41Z21WQW3SVGR--06F5Q94D0JDMMWDXSRGWX1E4F0--parentOf.json, .gicket/relations/GR/1W/06F5Q93R4633D41Z21WQW3SVGR--06F5Q94KX65TXQ8EC75FWSD01W--parentOf.json, and .gicket/relations/GR/94/06F5Q93R4633D41Z21WQW3SVGR--06F5Q94SQ086B2DZ1AKFDXGV94--parentOf.json.
- The stale incoming blocker relation is still present at .gicket/relations/G4/GR/06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.json, while .gicket/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/ticket.json shows the source ticket is already done.
- Epic comment .gicket/tickets/06F5Q93R4633D41Z21WQW3SVGR/comments/06F7W74RTC69JQRWCG3REE2YGG.md records queued removal of relation 06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks via outbox mutation-3848c5922287e32c, and that same ticket contract still says replay must be confirmed before closure.
- Branch-history evidence on develop includes AUTO-INTEGRATION squashes 71ad0c253, fae105f57, ff56f91df, 70795cd1e, and ec7f7bdb9 for the five child tickets; git diff --name-only develop..HEAD shows only .gicket/tickets/06F5Q93R4633D41Z21WQW3SVGR/** metadata on the epic branch.
- The landed repository surfaces referenced by the epic are present at docs/architecture/dvault-v1-activity-tracing-contract.md, src/DCoding.Data.DVault/DataVaultActivityTracing.cs, docs/performance-profiles.md, benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, docs/releases/v0.23.0.md, docs/production-adoption-checklist.md, and README.md.

Blocking findings
- Direct local evidence still shows a live incoming blocks relation into the epic, and there is no direct local evidence that replay or equivalent cleanup has landed. That means the epic does not yet satisfy its own closure condition in .gicket/tickets/06F5Q93R4633D41Z21WQW3SVGR/description.md.

Required PO actions
- Persist evidence that outbox mutation-3848c5922287e32c has replayed, or otherwise land equivalent relation cleanup, so the live graph no longer contains 06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.
- After that evidence is present, rerun PO-critic on the epic so closure readiness is verified against the cleaned relation graph rather than only the queued-removal intent.

Open issues ledger
- critic-item-1 [required-po-action] Persist evidence that outbox mutation-3848c5922287e32c has replayed, or otherwise land equivalent relation cleanup, so the live graph no longer contains 06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.
- critic-item-2 [required-po-action] After that evidence is present, rerun PO-critic on the epic so closure readiness is verified against the cleaned relation graph rather than only the queued-removal intent.
- critic-item-3 [blocking-finding] Direct local evidence still shows a live incoming blocks relation into the epic, and there is no direct local evidence that replay or equivalent cleanup has landed. That means the epic does not yet satisfy its own closure condition in .gicket/tickets/06F5Q93R4633D41Z21WQW3SVGR/description.md.

Missing examples / edge cases
- The contract does not show a concrete example of what persisted evidence is acceptable when replay completes on another owner branch but the current scratch snapshot may still show the stale relation file.
- The contract does not spell out how closure automation should arbitrate between ticket.json showing isBlocked=false and a still-present .gicket/relations/**--blocks.json file.

Risky assumptions
- Assuming a queued outbox mutation is equivalent to replayed relation cleanup would overstate closure readiness.
- Assuming a done source ticket automatically neutralizes a still-persisted incoming blocks relation would contradict the epic's own closure wording.
- Assuming child completion alone is enough for epic closeout would skip the explicit relation-graph requirement.

AC / test suggestions
- Add a closure-only verification note that names the exact relation id 06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks and requires the live graph to be clear before the epic can close.
- Record replay confirmation as its own durable closure evidence item, separate from child completion, so later reviewers do not have to infer it from queueing comments.

Implementation watchouts
- git diff --name-only develop..HEAD shows only .gicket/tickets/06F5Q93R4633D41Z21WQW3SVGR/** changes on the epic branch, so no parent-owned product or documentation work should be handed to a developer from this ticket.
- Downstream automation may keep treating the epic as blocked while .gicket/relations/G4/GR/06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.json remains visible, even though .gicket/tickets/06F5Q93R4633D41Z21WQW3SVGR/ticket.json currently says isBlocked=false.

Non-blocking notes
- The delivery contract is otherwise coherent for a tracking-only epic: Open Questions is none, the five-child decomposition remains intact, and the tracing, performance-guidance, release-note, checklist, and README surfaces are all present.
- Epic comment 06F7W74RTC69JQRWCG3REE2YGG materially answers the earlier PO-critic feedback by recording queued cleanup; the remaining gap is replay evidence, not missing scope definition.

Split recommendations
- No functional split is needed if replay lands promptly.
- If replay cannot be materialized through the existing owner-branch flow, create one tiny closure-only follow-up ticket dedicated to relation-graph cleanup and epic closeout.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment