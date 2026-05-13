[gicket-bot] PO refinement contract

Summary
- Current evidence supersedes the prior closure blockers: child 06F23Z08K0W49K5JMEHP60WZC0 is done, docs/releases/v0.8.0.md is present and tracked, and the epic can return to PO-critic for closure review without new planning writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The earlier instruction to keep the epic in PO tracking and out of dev while child 06F23Z08K0W49K5JMEHP60WZC0 was open is satisfied: the parent is still todo/needs-po, the child is now done, and this pass routes the epic back to PO-critic rather than dev.
- critic-item-2: `answered` - Child 06F23Z08K0W49K5JMEHP60WZC0 was completed rather than superseded, and the required repository artifact is landed: docs/releases/v0.8.0.md exists, is tracked, and is the closure-review document for the epic.
- critic-item-3: `answered` - The acceptance-criteria and definition-of-done blocker is resolved because the specific remaining child called out by the critic is no longer open; 06F23Z08K0W49K5JMEHP60WZC0 is done.
- critic-item-4: `answered` - The repository artifact blocker is resolved: docs/releases/v0.8.0.md exists on the reviewed branch, is tracked by git, and contains the lifecycle-guardrail release summary.
- critic-item-5: `answered` - The parent contract remains a tracking and closure-review contract, not a dev handoff. Implementation stays with child tickets, and the last missing deliverable is now represented by completed child 06F23Z08K0W49K5JMEHP60WZC0 plus the landed release notes.

Clarifications
- The earlier critic blockers were valid at review time but are now superseded by current evidence: child 06F23Z08K0W49K5JMEHP60WZC0 is done and docs/releases/v0.8.0.md is present.
- Child 06F23Z08K0W49K5JMEHP60WZC0 already exists and remains correctly linked through the existing parentOf relation; no relation cleanup is required.
- This pass did not materialize new child tickets, relation updates, attachments, or planning documents.
- The parent epic is being returned to PO-critic for closure review only, not handed to dev.

Scope In
- Track closure evidence for the EF Core lifecycle guardrails epic across child work and the landed release-summary artifact.
- Require release documentation for the ratified v1 boundary: stable DMV diagnostics, DVM2001-DVM2006 migration guardrails, consumer-owned single-project dotnet ef preflight, non-live metadata or ModelSnapshot drift comparison, and optional SQLite-first live-schema comparison.
- Use docs/releases/v0.8.0.md as the epic's release-documentation closure artifact.

Scope Out
- No new product or runtime code changes under this parent epic.
- No DVault-owned IDesignTimeServices, custom dotnet ef shim, EF CLI interception, automatic migration execution, or provider-specific online migration runner.
- No broader live-schema support claim than the documented SQLite-first lane.
- No new split, relation rewrite, attachment, or planning-document materialization in this refinement pass.

Open questions
- none

Follow-up questions
- Should a later docs or examples ticket add one operator-facing end-to-end example that chains dotnet ef migrations add, consumer preflight, and dotnet ef database update?
- After v0.8.0, which provider should be the next live-schema reader after the SQLite-first baseline?
- Should a later guide consolidate artifact review, metadata or ModelSnapshot comparison, live-schema comparison, migration scaffolding, preflight, and database update into one operator workflow document?

Risks
- If later edits remove or materially rewrite docs/releases/v0.8.0.md before closure review completes, the epic would regress against its release-documentation criterion.
- If later docs or ticket text reintroduce shorthand about DVault-owned design-time services, the closure evidence could drift from the ratified consumer-owned preflight boundary.

Split recommendations
- No further split recommended; the remaining closure evidence is already materialized through completed child 06F23Z08K0W49K5JMEHP60WZC0 and tracked artifact docs/releases/v0.8.0.md.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment