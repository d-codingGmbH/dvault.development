[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0ME84YSZ62WRX1SJQE7BMTC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0ME84YSZ62WRX1SJQE7BMTC`.
- Optimistic claim succeeded (`expectedRevision=06F0QH2QCGVZ2PAGMM0DMB4CWM`, `currentRevision=06F1DNE5JPQWT94FK92AP4J0EC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0ME84YSZ62WRX1SJQE7BMTC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0ME84YSZ62WRX1SJQE7BMTC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0ME84YSZ62WRX1SJQE7BMTC-epic-code-first-and-typed-workflow-usability' from source 'd4d42638fe47afa998a8400f0ab2e4988a22916e'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Because this is an epic spanning API, persistence, reads, diagnostics, and examples, child stories must stay aligned to the same bounded v0.6.0 contract to avoid documentation/API drift.
- Users may infer Code-First declarations are an authoritative registry source unless documentation continues to distinguish Code-First projection from registry-backed metadata.
- Typed read helper ergonomics must remain narrow enough to preserve explicit projection control and avoid implying a broader model-first read contract.
- Split recommendation: Keep this ticket as the umbrella epic and route implementation through bounded child stories rather than expanding the epic into direct implementation scope.
- Split recommendation: If additional work is discovered, split by product surface: fluent API projection, registry integration, explicit save/read helpers, diagnostics/explain output, and examples/docs.
- Split recommendation: Do not add new subtickets for v0.6.0 limitations already documented as future work unless a separate release planning decision promotes one of them into current scope.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `23945`
- cached-tokens: `2432`
- effective-cache-ratio: `0.1016`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `89559d8387dc41e38467b3f7246ae519`
- completed-at-utc: `<redacted>-11T11:43:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0ME84YSZ62WRX1SJQE7BMTC/runs/20260511T114318287Z-89559d8387dc41e38467b3f7246ae519.json`