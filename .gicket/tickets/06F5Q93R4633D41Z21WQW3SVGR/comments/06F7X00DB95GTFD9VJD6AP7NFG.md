[gicket-bot] PO refinement contract

Summary
- Fresh live relation evidence shows the stale incoming blocker is gone, the epic now retains only its five existing parentOf child links, and the ticket is ready to return to PO-critic against the cleaned graph.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Satisfied by equivalent landed cleanup. The current live relation read for the epic shows no incoming relations, so the stale blocks edge from 06F5Q93H60W6X8FJ88PWTR6NG4 is no longer present in the live graph.
- critic-item-2: `answered` - The prerequisite cleanup evidence is now present, so this PO pass returns the epic to PO-critic for the required closure-readiness rerun against the cleaned live relation graph rather than the earlier queued-removal intent.
- critic-item-3: `answered` - The earlier blocking finding is superseded by fresher direct evidence. The current live relation graph no longer shows any incoming blocks relation into the epic, so that finding is no longer true.

Clarifications
- The live graph for 06F5Q93R4633D41Z21WQW3SVGR now shows outgoingCount=5 and incomingCount=0; the remaining relations are the five existing parentOf links to 06F5Q93YXHSKABD2SABWY85S78, 06F5Q9463M0RSHAJJX0F3D1DB0, 06F5Q94D0JDMMWDXSRGWX1E4F0, 06F5Q94KX65TXQ8EC75FWSD01W, and 06F5Q94SQ086B2DZ1AKFDXGV94.
- Current ticket context already states all five v0.23.0 child tickets are done; this PO pass does not reopen, rescope, or split the epic.
- No new child tickets, relation writes, description updates, attachments, or planning documents were materialized in this pass because the required stale-blocker cleanup is already directly evidenced in the live relation read and recent ticket comment.
- The authoritative repository surfaces remain docs/architecture/dvault-v1-activity-tracing-contract.md, src/DCoding.Data.DVault/DataVaultActivityTracing.cs, docs/performance-profiles.md, benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, docs/releases/v0.23.0.md, and README.md.

Scope In
- Track the authoritative Activity tracing contract and redaction rules through child ticket 06F5Q93YXHSKABD2SABWY85S78 and docs/architecture/dvault-v1-activity-tracing-contract.md.
- Track save/read tracing and PIT/bridge maintenance tracing against the shared DCoding.Data.DVault activity vocabulary evidenced by src/DCoding.Data.DVault/DataVaultActivityTracing.cs.
- Track benchmark-backed adopter guidance for four practical performance profiles through docs/performance-profiles.md and the checked-in benchmark-summary.md / benchmark-summary.csv / benchmark-summary.json triplet.
- Track coordinated v0.23.0 documentation consistency across README.md, docs/releases/v0.23.0.md, docs/performance-profiles.md, and child ticket 06F5Q94SQ086B2DZ1AKFDXGV94.

Scope Out
- No exporter, collector, dashboard, alerting, scheduler, hosted worker, database or container provisioning, credential management, or package-publication automation work.
- No raw business keys, hash keys, payload values, record sources, SQL text, query plans, connection strings, provider messages, exception messages, or stack traces in tracing outputs.
- No provider-strategy redesign, benchmark-harness redesign, public persistence API rewrite, or change to AddDVault() default telemetry-free behavior.
- No direct product-code or documentation edits in this epic; authoritative implementation and documentation changes stay in the existing child tickets.

Open questions
- none

Follow-up questions
- Before runtime closes the epic, does a final eligibility/relation check still show incomingCount=0 and the same five done child tickets after any integration activity?

Risks
- Low: if a child ticket is reopened after this cleanup, the epic should wait for that child again.
- Low: if another branch reintroduces the removed stale relation during integration, rerun relation eligibility before final closure.

Split recommendations
- No additional split recommended; the existing five-child decomposition remains bounded and matches the visible repository and ticket evidence.

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