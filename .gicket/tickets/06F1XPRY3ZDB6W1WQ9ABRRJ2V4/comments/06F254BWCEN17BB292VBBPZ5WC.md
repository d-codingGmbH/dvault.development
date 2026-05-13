[gicket-bot] PO refinement contract

Summary
- Verified the current ticket, comments, relations, and repository documents; the epic is a tracking-only closure ticket with no parent-owned implementation slice, and current evidence supports returning it to PO-critic without new planning writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The closure-audit findings are resolved at the parent-ticket level: the release-summary child 06F23Z08K0W49K5JMEHP60WZC0 is reported done in current ticket-comment evidence, the parentOf relation to that child is still present, and docs/releases/v0.8.0.md exists as the release-documentation closure artifact, so the parent can return to closure review instead of reopening dev work.
- critic-item-2: `answered` - The delivery contract explicitly marks this ticket as a tracking-only closure epic with no parent-owned implementation slice: Scope Out excludes new parent-level product or runtime work, Definition of Done keeps implementation with child tickets rather than new parent dev work, and the parent is being returned for closure review only.

Clarifications
- This epic is closure/tracking only and does not own a new parent-level implementation slice.
- Current closure evidence supersedes the earlier blocker: child `06F23Z08K0W49K5JMEHP60WZC0` is done in current ticket-comment evidence and `docs/releases/v0.8.0.md` is present as the release-documentation artifact.
- The existing `parentOf` relation to child `06F23Z08K0W49K5JMEHP60WZC0` remains the relevant closure link and was not changed in this pass.
- This pass did not materialize new child tickets, relation writes, attachments, or planning documents.

Scope In
- Track closure evidence for the EF Core lifecycle guardrails epic through completed child work and `docs/releases/v0.8.0.md`.
- Confirm that the v0.8.0 closure artifact documents stable `DMV####` diagnostics, `DVM2001-DVM2006` migration guardrails, the consumer-owned single-project `dotnet ef` preflight boundary, non-live metadata or `ModelSnapshot` drift comparison, and the optional SQLite-first live-schema lane.
- Keep the parent contract aligned with the consumer-owned design-time workflow documented in `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`.

Scope Out
- No parent-owned product or runtime implementation work under this epic.
- No DVault-owned `IDesignTimeServices`, custom `dotnet ef` shim, EF CLI interception, automatic migration execution, or provider-specific online migration runner.
- No broader live-schema support claim than the documented SQLite-first lane.
- No new split, relation rewrite, attachment, or planning-document materialization in this pass.

Open questions
- none

Follow-up questions
- Should a later docs or examples ticket add one operator-facing end-to-end example that chains `dotnet ef migrations add`, consumer preflight, and `dotnet ef database update`?
- After v0.8.0, which provider should be the next live-schema reader after the SQLite-first baseline?
- Should a later guide consolidate artifact review, metadata or `ModelSnapshot` comparison, live-schema comparison, migration scaffolding, preflight, and database update into one operator workflow document?

Risks
- If later edits remove or materially rewrite `docs/releases/v0.8.0.md` before closure review completes, the epic would regress against its release-documentation criterion.
- If later docs or ticket text reintroduce shorthand about DVault-owned design-time services, the closure evidence could drift from the ratified consumer-owned preflight boundary.

Split recommendations
- No further split recommended; this parent epic is closure/tracking only and the remaining work is PO-critic closure review against existing child and release-note evidence.

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