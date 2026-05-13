[gicket-bot] PO refinement contract

Summary
- Reframed the parent epic as a tracking-only closure ticket, confirmed 06F23Z08K0W49K5JMEHP60WZC0 owns the missing v0.8.0 release-summary artifact, and kept epic closure tied to that child rather than any direct developer work on this branch.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The parent epic remains a tracking ticket only. It should not go to dev while the remaining repository artifact is owned by 06F23Z08K0W49K5JMEHP60WZC0.
- critic-item-2: `answered` - Ticket 06F23Z08K0W49K5JMEHP60WZC0 remains the actionable docs task. It is todo with labels automation/bot-ready and needs-po, so any later developer handoff must originate from that child after its own PO and PO-critic passes, not from the parent epic.
- critic-item-3: `answered` - The parent epic stays open until 06F23Z08K0W49K5JMEHP60WZC0 is done or intentionally superseded. The contract ties closure to that child deliverable and removes any implication that direct implementation remains on the parent branch.
- critic-item-4: `answered` - Confirmed. The four implementation child stories are already ratified as done, and the only unmet repository artifact is the release-summary document tracked by 06F23Z08K0W49K5JMEHP60WZC0. The parent epic therefore no longer owns direct developer work.

Clarifications
- This epic is a tracking and closure-governance ticket; it does not own new product code or direct developer handoff.
- The four ratified implementation stories remain done: 06F1XPS7KGKBP5SVMQPJC49J2G, 06F1XPTCGWTJHHQVNPN13KANMG, 06F1XPVPKVGYKCV04PY98TSS78, and 06F1XPWB8DZR4J8EZ00V8DT25G.
- Ticket 06F23Z08K0W49K5JMEHP60WZC0 is the only remaining actionable deliverable and already owns the missing docs/releases/v0.8.0.md work.
- gicket-read-ticket-relations shows an existing parentOf relation from 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 to 06F23Z08K0W49K5JMEHP60WZC0.
- Repository evidence still stops at docs/releases/v0.7.0.md; docs/releases/v0.8.0.md is not present.
- No new child tickets, relation writes, attachment writes, or planning documents were materialized in this pass.

Scope In
- Keep the epic open as a tracking wrapper until the docs follow-up ticket 06F23Z08K0W49K5JMEHP60WZC0 is done or intentionally superseded.
- Track closure dependency on the existing done implementation stories plus the remaining release-summary artifact in docs/releases/v0.8.0.md.
- Require epic-level documentation to stay aligned with docs/architecture/dvault-dotnet-ef-design-time-workflow.md and docs/model-first-governance.md.
- Preserve the ratified v1 boundaries for stable diagnostics, migration guardrails, consumer-owned design-time preflight, and ModelSnapshot versus optional live-schema evidence.

Scope Out
- No new runtime, diagnostics, migration, design-time, or drift implementation work on this epic branch.
- No direct developer handoff from the parent epic while docs/releases/v0.8.0.md is owned by 06F23Z08K0W49K5JMEHP60WZC0.
- No claim that docs/releases/v0.8.0.md already exists on the repository branch.
- No DVault-owned IDesignTimeServices, custom dotnet ef shim, CLI interception, or provider-specific migration runner promises.

Open questions
- none

Follow-up questions
- After docs/releases/v0.8.0.md lands, should later work consolidate validation, migration guardrails, ModelSnapshot comparison, and live-schema comparison into one consumer-facing preflight or report?
- Which provider should become the next optional live-schema lane after the current SQLite-first proof path?
- If consumer-owned preflight remains too heavy, should later work add packaged tooling or broader multi-project design-time support?

Risks
- Later ticket edits could reintroduce language that implies direct developer work still belongs to this parent epic.
- The release summary could over-promise support if it blurs the consumer-owned preflight boundary or the SQLite-first live-schema evidence boundary.
- If the child ticket and parent ticket drift, the epic could again overstate repository documentation readiness before docs/releases/v0.8.0.md lands.

Split recommendations
- No additional split is justified beyond docs-only follow-up ticket 06F23Z08K0W49K5JMEHP60WZC0.
- Keep later runtime ergonomics or broader tooling ideas in downstream tickets rather than reopening this tracking epic or the completed implementation stories.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment