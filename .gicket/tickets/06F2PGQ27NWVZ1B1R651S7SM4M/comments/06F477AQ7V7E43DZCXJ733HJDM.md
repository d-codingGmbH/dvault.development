[gicket-bot] PO refinement contract

Summary
- Reframed ticket 06F2PGQ27NWVZ1B1R651S7SM4M as a tracking-only closure/no-work-required epic; verified four done child tickets, persisted parentOf relations, workflow-only post-integration commits, and the v0.16.0 observability baseline; no child tickets, relation edits, attachments, or planning documents were created in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Ticket 06F2PGQ27NWVZ1B1R651S7SM4M is a tracking-only, closure/no-work-required epic. The parent owns no remaining implementation, documentation, or planning slice beyond the four named done children: 06F2PGQ6T5TGNWCBQBX3700D84, 06F2PGQBGNZPEEJE4KBET4JG24, 06F2PGQJ7THHNSYYBFFPBG4174, and 06F2PGQQJB5FJGDB16M2G7CPCM.
- critic-item-2: `answered` - No additional parent-owned work remains. The branch head equals the current scratch source ref, the most recent substantive commits are the four child auto-integrations, and the diff after the final child integration touches only .gicket ticket/workflow files. Because no required scope remains outside the four done children, no new child or follow-up ticket was materialized in this pass.

Clarifications
- This parent epic is tracking-only and closure/no-work-required: it records that the shipped observability scope is exhausted by four existing done children and that the parent itself owns no direct implementation slice.
- The four authoritative child tickets are 06F2PGQ6T5TGNWCBQBX3700D84 (Explain save and read strategy decisions), 06F2PGQBGNZPEEJE4KBET4JG24 (Add save/read telemetry hooks and counters), 06F2PGQJ7THHNSYYBFFPBG4174 (Add diagnostics support bundle export), and 06F2PGQQJB5FJGDB16M2G7CPCM (Update v0.16.0 documentation and release notes); local parentOf relation files bind this epic to each child.
- The current repository baseline already ratifies the shipped v0.16.0 observability contract across README.md, docs/releases/v0.16.0.md, docs/model-first-governance.md, docs/production-adoption-checklist.md, DataVaultTelemetryServiceCollectionExtensions.cs, DataVaultDesignTimeCommand.cs, and DataVaultDiagnostics.cs.
- No child tickets, relation edits, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Epic-level closure audit for the four completed observability child slices: request-bound strategy explainability, opt-in save/read telemetry, consumer-owned support-bundle export, and v0.16.0 documentation/release-note alignment.
- Explicitly recording the parent epic as tracking-only / closure-only / no-work-required with no remaining parent-owned implementation, documentation, or planning slice beyond the four named children.
- Repository-backed confirmation that the shipped observability baseline stays additive and explainability-focused without changing DVault persistence, read, or maintenance semantics.

Scope Out
- Any new observability implementation beyond the four existing done child tickets.
- Provider behavior changes, persistence/read semantic changes, PIT or bridge maintenance changes, or automatic maintenance/orchestration work.
- Operator dashboards, alerting, telemetry backend setup, support-bundle transport/archive workflows, standalone CLI/tooling, or relation-hygiene cleanup as part of this parent epic.

Open questions
- none

Follow-up questions
- Optional post-epic follow-up only: should a separate ticket add operator-facing troubleshooting examples that map common strategy fallback causes to telemetry counters and support-bundle sections?
- Optional post-epic follow-up only: should PIT and bridge maintenance services get their own bounded telemetry ticket instead of extending this closed epic?

Risks
- If future docs or follow-up work overstate the current contract, consumers may assume automatic instrumentation, support-bundle transport, or broader runtime coverage than the repository ships.
- If future observability work stops reusing the existing diagnostics status/fallback vocabulary, telemetry, support-bundle output, and documentation can drift from one another.
- The historical blocks relation .gicket/relations/B4/4M/06F2PGP7HM8F39K3J0H5JHB3B4--06F2PGQ27NWVZ1B1R651S7SM4M--blocks.json can still confuse later readers even though the source epic is done and the relation is non-blocking today.

Split recommendations
- No additional split is recommended or required. Current ticket-store, relation, commit, and diff evidence show no remaining parent-owned implementation slice beyond the four done children, so no new child or follow-up ticket was materialized in this pass.
- If future work is later desired for troubleshooting examples, maintenance-service telemetry, or historical relation cleanup, track it as separate follow-up tickets instead of reopening this epic.

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