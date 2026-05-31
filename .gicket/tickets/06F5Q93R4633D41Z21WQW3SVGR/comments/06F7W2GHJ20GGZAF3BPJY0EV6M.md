[gicket-bot] PO refinement contract

Summary
- Epic remains a bounded five-child v0.23.0 tracking ticket; live relations and landed repository evidence already align, so no bounded planning writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Live parentOf relations already connect this epic to the five planned child tickets, and no child-ticket creation or relation cleanup was materialized in this refinement pass.
- Recent comments contain only bot claim/lease markers; no human comment changed scope or introduced unanswered PO questions.
- Verified child tickets 06F5Q94D0JDMMWDXSRGWX1E4F0, 06F5Q94KX65TXQ8EC75FWSD01W, and 06F5Q94SQ086B2DZ1AKFDXGV94 are done; repository evidence already contains docs/architecture/dvault-v1-activity-tracing-contract.md, src/DCoding.Data.DVault/DataVaultActivityTracing.cs, docs/performance-profiles.md, and docs/releases/v0.23.0.md, so the existing epic split still matches the landed tracing and performance surfaces.
- No ticket description update, attachment, or planning document write was needed because the current epic description and live relation graph already match the repository baseline.

Scope In
- Track the authoritative Activity tracing contract and redaction rules in child ticket 06F5Q93YXHSKABD2SABWY85S78.
- Track listener-driven save/read Activity spans and listener-driven PIT/bridge maintenance Activity spans as separate child implementation stories using one shared tracing vocabulary.
- Track benchmark-backed adopter guidance for four practical performance profiles using the checked-in benchmark artifact triplet and run context.
- Track the coordinated v0.23.0 public-doc rollup across README, production-adoption guidance, performance profiles, and release notes.

Scope Out
- No exporter, collector, dashboard, alerting, scheduler, hosted worker, database or container provisioning, credential management, or package-publication automation work.
- No raw business keys, hash keys, payload values, record sources, SQL text, query plans, connection strings, provider messages, exception messages, or stack traces in tracing outputs.
- No provider-strategy redesign, benchmark-harness redesign, public persistence API rewrite, or change to AddDVault()'s default telemetry-free behavior.
- No direct product-code or arbitrary documentation edits in the epic itself; delivery remains in the child tickets.

Open questions
- none

Follow-up questions
- After the incoming blocks relation from 06F5Q93H60W6X8FJ88PWTR6NG4 clears, confirm whether any remaining child ticket needs reopen work before runtime closes this epic.
- If the contract or save/read child tickets are later reopened despite the landed repository surfaces, confirm whether that work stays within v0.23.0 or is better handled as a separate follow-up release ticket.

Risks
- This epic still has a live incoming blocks relation from 06F5Q93H60W6X8FJ88PWTR6NG4, so closure can lag even though the epic scope itself is already refined.
- Two child-ticket reads hit the structured result bytes cap in this slice, so current refinement relies on repository evidence rather than a fresh persisted description snapshot for 06F5Q93YXHSKABD2SABWY85S78 and 06F5Q9463M0RSHAJJX0F3D1DB0.
- If any child ticket reopens with scope that exceeds the fixed tracing vocabulary or evidence-bound performance posture, the epic could drift beyond its current bounded v0.23.0 release story.

Split recommendations
- No additional split recommended; the epic is already decomposed into bounded child tickets for contract, save/read tracing, maintenance tracing, performance guidance, and coordinated docs.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment