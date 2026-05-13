[gicket-bot] PO refinement contract

Summary
- Parent epic stays in PO tracking/blocking state; child 06F23Z08K0W49K5JMEHP60WZC0 still owns the missing docs/releases/v0.8.0.md artifact, so the epic is not ready to return to PO-critic.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The parent epic remains a tracking-only closure ticket and must not be handed to dev while child 06F23Z08K0W49K5JMEHP60WZC0 remains open.
- critic-item-2: `answered` - This pass intentionally preserves child 06F23Z08K0W49K5JMEHP60WZC0 as the live docs ticket instead of superseding it. Because the child is still todo and docs/releases/v0.8.0.md is still missing, the parent stays open and cannot be resubmitted for closure review yet.
- critic-item-3: `answered` - Confirmed. The parent acceptance criteria and definition of done remain unmet while child 06F23Z08K0W49K5JMEHP60WZC0 is still open and the docs/releases/v0.8.0.md artifact has not landed. The contract keeps the epic open against that dependency instead of implying closure readiness.
- critic-item-4: `answered` - Confirmed. Repository evidence still stops at docs/releases/v0.7.0.md; docs/releases/v0.8.0.md does not exist on the reviewed branch, so the release-summary closure condition is still open.
- critic-item-5: `answered` - Resolved by preserving the parent as tracking-only and scoping out direct developer handoff. Any later execution remains on child 06F23Z08K0W49K5JMEHP60WZC0 until that ticket completes its own workflow and lands the release note.

Clarifications
- This epic remains a tracking and closure-governance ticket; it does not own new product code or direct developer handoff.
- The existing parentOf relation from 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 to 06F23Z08K0W49K5JMEHP60WZC0 already captures the remaining docs closure dependency.
- Repository evidence still shows release notes only through docs/releases/v0.7.0.md.
- docs/architecture/dvault-dotnet-ef-design-time-workflow.md keeps design-time support consumer-owned and single-project, with no DVault-owned IDesignTimeServices or CLI shim.
- docs/model-first-governance.md keeps DataVaultModelDriftReporter.Compare as metadata-only evidence and the live-schema lane SQLite-first.
- No child-ticket, relation, attachment, or planning-document writes were materialized in this pass.

Scope In
- Keep the parent epic open as a tracking wrapper until child 06F23Z08K0W49K5JMEHP60WZC0 is done or intentionally superseded.
- Track closure on the missing docs/releases/v0.8.0.md release summary plus the already-ratified completed implementation stories.
- Keep the release-summary boundary aligned with docs/architecture/dvault-dotnet-ef-design-time-workflow.md and docs/model-first-governance.md.
- Preserve the ratified v1 boundaries for stable diagnostics, migration guardrails, consumer-owned design-time preflight, and metadata-only versus optional SQLite-first live-schema drift evidence.

Scope Out
- No direct developer handoff from the parent epic while child 06F23Z08K0W49K5JMEHP60WZC0 remains open.
- No new runtime, diagnostics, migration, design-time, or drift implementation work on this parent epic.
- No claim that docs/releases/v0.8.0.md already exists on the branch.
- No DVault-owned IDesignTimeServices, custom dotnet ef shim, CLI interception, or provider-specific migration runner promises.

Open questions
- Child 06F23Z08K0W49K5JMEHP60WZC0 is still todo and needs-po; the parent cannot return to PO-critic until that child is done or intentionally superseded.
- docs/releases/v0.8.0.md is still missing from the branch; the parent cannot satisfy its release-summary closure condition until the file lands through child 06F23Z08K0W49K5JMEHP60WZC0.

Follow-up questions
- After docs/releases/v0.8.0.md lands, should later work consolidate validation, migration guardrails, ModelSnapshot comparison, and live-schema comparison into one consumer-facing preflight or report?
- Which provider should become the next optional live-schema lane after the current SQLite-first proof path?
- If consumer-owned preflight remains too heavy, should later work add packaged tooling or broader multi-project design-time support?

Risks
- Later ticket edits could reintroduce language that implies direct developer work still belongs to this parent epic.
- The release summary could over-promise support if it blurs the consumer-owned preflight boundary or the SQLite-first live-schema evidence boundary.
- If the child ticket and parent ticket drift, the epic could again overstate repository documentation readiness before docs/releases/v0.8.0.md lands.

Split recommendations
- No additional split is justified beyond existing docs-only child 06F23Z08K0W49K5JMEHP60WZC0.
- Keep any later runtime ergonomics or broader tooling ideas in downstream tickets rather than reopening this tracking epic.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment