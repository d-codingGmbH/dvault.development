[gicket-bot] PO refinement contract

Summary
- Verified the live epic, bot-only comments, no attachments, inbound/outbound relations, and current repository baseline; this epic is already fully decomposed into four done child tickets for strategy explainability, telemetry, support-bundle export, and v0.16.0 documentation, so no new split or planning write was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Live parentOf relations already bind this epic to done child tickets 06F2PGQ6T5TGNWCBQBX3700D84 (Explain save and read strategy decisions), 06F2PGQBGNZPEEJE4KBET4JG24 (Add save/read telemetry hooks and counters), 06F2PGQJ7THHNSYYBFFPBG4174 (Add diagnostics support bundle export), and 06F2PGQQJB5FJGDB16M2G7CPCM (Update v0.16.0 documentation and release notes).
- The only epic comments present were bot claim/lease comments, and no ticket attachments were present.
- Repository evidence already ratifies the shipped observability baseline through docs/releases/v0.16.0.md, README.md, docs/model-first-governance.md, docs/production-adoption-checklist.md, and the public source/test surfaces for request-bound diagnostics explainability, AddDVaultTelemetry/IDataVaultTelemetryObserver, and consumer-owned support-bundle export under dvault.support-bundle.v1.
- No child tickets, relation edits, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Epic coordination for the four existing observability child slices: request-bound strategy explanation, save/read telemetry, diagnostics support-bundle export, and v0.16.0 documentation/release notes.
- Epic-level confirmation that observability stays additive and does not change DVault persistence or read semantics.
- Reuse of the existing diagnostics status/fallback vocabulary and explicit service boundaries as the shared contract across the completed child tickets.
- Repository-backed release-note and current-baseline documentation alignment for the v0.16.0 observability posture.

Scope Out
- New observability feature areas beyond the four existing child tickets.
- Changes to provider dispatch behavior, persistence semantics, read semantics, PIT/bridge maintenance semantics, or automatic maintenance/orchestration.
- Backend-specific telemetry setup, dashboards, alerting, support-bundle upload/archive workflows, or a standalone DVault CLI.
- Additional child-ticket or planning-document materialization without new repository evidence.

Open questions
- none

Follow-up questions
- Should a separate post-v0.16 observability ticket add operator-facing troubleshooting examples that map common strategy fallback causes to telemetry counters and support-bundle sections?
- Should PIT and bridge maintenance services get their own bounded telemetry story, or should observability remain limited to explicit save/read operations?
- After the epic is operationally closed, should the historical inbound blocks relation from done epic 06F2PGP7HM8F39K3J0H5JHB3B4 be cleaned up as ticket-hygiene follow-up?

Risks
- If later docs or follow-up work overstate the current contract, consumers may assume automatic instrumentation, support-bundle transport, or broader runtime coverage than the repository actually ships.
- If future observability work stops reusing the existing diagnostics status/fallback vocabulary, telemetry, support-bundle output, and documentation can drift from one another.
- The lingering historical blocks relation from done epic 06F2PGP7HM8F39K3J0H5JHB3B4 could confuse later readers even though it is non-blocking today.

Split recommendations
- No additional split is recommended. The epic already has a finite four-ticket decomposition, and all four child tickets are present and done.
- If future work is needed for maintenance-service telemetry, backend-specific observability guidance, support-bundle transport, or relation-hygiene cleanup, track it as separate follow-up tickets instead of reopening this epic.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment