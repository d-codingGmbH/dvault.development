<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Human metadata cleanup aligned the parent epic with its closure-only state on 2026-05-31.
- Stale stop labels `blocked/dev`, `blocked/test`, and `blocked/po` have been removed from this ticket branch.
- The live relation graph is clean: `incomingCount=0`, `relationReady=true`, and the only outgoing relations are the five existing `parentOf` child links.
- All five v0.23.0 child tickets are `done`; the parent epic owns no direct product-code or documentation work.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: PO-critic can review the closure-only epic state after the human metadata cleanup.

### Clarifications
- The previous PO clarification loop was caused by stale blocked labels contradicting the clean relation graph and done-child evidence, not by missing product scope.
- The authoritative closure baseline remains `docs/architecture/dvault-v1-activity-tracing-contract.md`, `src/DCoding.Data.DVault/DataVaultActivityTracing.cs`, `docs/performance-profiles.md`, `benchmark-summary.md`, `benchmark-summary.csv`, `benchmark-summary.json`, `docs/releases/v0.23.0.md`, and `README.md`.
- No child-ticket, relation, attachment, planning-document, product-code, or documentation writes are required in this parent epic.

### Scope In
- Closure-only tracking for epic `06F5Q93R4633D41Z21WQW3SVGR` against the existing five child tickets and the landed v0.23.0 tracing and performance documentation baseline.
- Verification that the live relation graph remains parentOf-only with `incomingCount=0` before final epic closure.

### Scope Out
- Any new product-code, documentation, tracing, benchmark, or implementation work owned directly by the parent epic.
- Any new child-ticket split, relation reshaping, exporter or observability platform work, or package-publication automation work.

## Acceptance Criteria
- Persisted ticket metadata no longer contains stale `blocked/*` labels and routes the ticket to PO-critic review with `critic-needed` plus `automation/bot-ready`.
- All five existing child tickets linked from the epic remain the authoritative delivery surfaces and remain `done`.
- A fresh live relation read continues to show `incomingCount=0` and only the five existing `parentOf` relations.
- The repository baseline remains coherent across the tracing contract, `DataVaultActivityTracing.cs`, performance guide, benchmark artifact triplet, v0.23.0 release notes, and README.

## Definition of Done
- The parent epic can close without creating or assigning any parent-owned implementation or documentation work.
- The ticket metadata, relation graph, and done-child evidence all agree on a tracking-only closure state with no stale blocked implementation signal.
- Closure evidence includes direct proof of `incomingCount=0` together with the unchanged five-child done set.

## Implementation Notes
- Treat the parent epic as tracking-only and closure-only; all landed implementation and documentation changes remain owned by the existing child tickets.
- Next PO-critic run should use `gicket relation list 06F5Q93R4633D41Z21WQW3SVGR --format json` and `gicket ticket eligibility --id 06F5Q93R4633D41Z21WQW3SVGR --format json` as relation evidence.
- If a bot run recreates terminal relation-follow-up queue entries for already-`done` child tickets, treat them as obsolete closure-evidence noise rather than new child work.

## Open Questions
- none

## Follow-Up Questions
- Before runtime closes the epic, does a final eligibility check still show `incomingCount=0` and the same five done child tickets after any integration activity?

## Risks
- Low: if any child ticket is reopened or a new incoming relation appears, the parent epic must stop at closure tracking until the evidence is clean again.
- Low: repeated PO/PO-critic runs may recreate terminal follow-up queue entries for done child tickets; those entries should not reopen child scope.

## Split Recommendations
- No additional split recommended; the existing five-child decomposition remains complete and the parent epic still owns only closure tracking.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Coordinate v0.23.0 tracing and performance guidance as a small release whose child tickets are independently implementable by the bot.

# Release Intent
Give DVault adopters opt-in Activity tracing for explicit save/read/maintenance operations and practical benchmark-backed performance guidance, while preserving the existing DVault boundaries: explicit service calls, consumer-owned observability pipelines, redacted diagnostics, and evidence-bound performance claims.

# Child Tickets
- `06F5Q93YXHSKABD2SABWY85S78` - define the Activity tracing contract and redaction rules. This is the first implementation blocker for the release.
- `06F5Q9463M0RSHAJJX0F3D1DB0` - implement Activity tracing for save/read service operations after the contract is accepted.
- `06F5Q94D0JDMMWDXSRGWX1E4F0` - implement Activity tracing for PIT and bridge maintenance after the contract is accepted.
- `06F5Q94KX65TXQ8EC75FWSD01W` - add benchmark-backed performance profile guidance after the contract is accepted, without waiting for code implementation.
- `06F5Q94SQ086B2DZ1AKFDXGV94` - update coordinated v0.23.0 public docs after the implementation and guidance tickets are complete.

# Scope In
- One Activity tracing contract with exact source/span/tag/event/status/redaction semantics.
- Listener-driven save/read Activity spans that complement existing metrics and telemetry summaries.
- Listener-driven PIT/bridge maintenance Activity spans that preserve caller-owned orchestration.
- Benchmark-backed adopter guidance for four practical performance profiles.
- Coordinated README, production checklist, benchmark-doc, and release-note updates.

# Scope Out
- No dashboard, OpenTelemetry exporter, collector, alerting, hosted monitoring, scheduler, background worker, database/container provisioning, credential management, or package publication automation.
- No raw business data, hash keys, payload values, record sources, SQL text, query plans, connection strings, provider messages, exception messages, or stack traces in tracing.
- No provider strategy redesign, benchmark harness redesign, public persistence API rewrite, or change to default `AddDVault()` behavior.

# Release-Level Acceptance Criteria
- The contract ticket is completed before save/read and maintenance implementation tickets start their implementation work.
- Save/read and maintenance tracing use the same ActivitySource name, span names, tag keys, event names, status behavior, and redaction rules.
- Performance guidance cites repository benchmark evidence and keeps timing claims attached to artifact and run context.
- Final documentation presents one coherent v0.23.0 story and lists explicit non-goals.
- Existing telemetry, metrics, diagnostics, explicit save/read service boundaries, and PIT/bridge maintenance boundaries remain compatible.

# Bot Handoff Notes
- This epic should not make product-code or documentation edits directly. Work is done in the child tickets.
- If a child ticket appears blocked, inspect the dependency graph before selecting another ticket. The intended first child is the tracing contract ticket.
- Do not close this epic until all child tickets are done and the release documentation task has landed.