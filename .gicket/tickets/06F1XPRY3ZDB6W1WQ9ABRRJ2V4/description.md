<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the current ticket, comments, relations, and repository documents; the epic is a tracking-only closure ticket with no parent-owned implementation slice, and current evidence supports returning it to PO-critic without new planning writes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This epic is closure/tracking only and does not own a new parent-level implementation slice.
- Current closure evidence supersedes the earlier blocker: child `06F23Z08K0W49K5JMEHP60WZC0` is done in current ticket-comment evidence and `docs/releases/v0.8.0.md` is present as the release-documentation artifact.
- The existing `parentOf` relation to child `06F23Z08K0W49K5JMEHP60WZC0` remains the relevant closure link and was not changed in this pass.
- This pass did not materialize new child tickets, relation writes, attachments, or planning documents.

### Scope In
- Track closure evidence for the EF Core lifecycle guardrails epic through completed child work and `docs/releases/v0.8.0.md`.
- Confirm that the v0.8.0 closure artifact documents stable `DMV####` diagnostics, `DVM2001-DVM2006` migration guardrails, the consumer-owned single-project `dotnet ef` preflight boundary, non-live metadata or `ModelSnapshot` drift comparison, and the optional SQLite-first live-schema lane.
- Keep the parent contract aligned with the consumer-owned design-time workflow documented in `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`.

### Scope Out
- No parent-owned product or runtime implementation work under this epic.
- No DVault-owned `IDesignTimeServices`, custom `dotnet ef` shim, EF CLI interception, automatic migration execution, or provider-specific online migration runner.
- No broader live-schema support claim than the documented SQLite-first lane.
- No new split, relation rewrite, attachment, or planning-document materialization in this pass.

## Acceptance Criteria
- The epic is explicitly treated as a tracking-only closure ticket whose implementation is represented by child tickets rather than new parent-level dev work.
- Current child-ticket coverage remains the parent implementation slice, including done child `06F23Z08K0W49K5JMEHP60WZC0` for the release-summary deliverable.
- The repository contains tracked release notes at `docs/releases/v0.8.0.md` for `v0.8.0 - EF Core Lifecycle Guardrails`.
- The release notes describe stable `DMV####` diagnostics, `DVM2001-DVM2006` migration guardrails, the consumer-owned single-project `dotnet ef` preflight workflow, non-live drift comparison, and the optional SQLite-first live-schema lane.
- The closure evidence can be verified from repository-local ticket and document evidence without package publication credentials.

## Definition of Done
- The parent epic remains a tracking/closure ticket and does not reopen a parent-owned implementation slice.
- Existing child-ticket evidence and relations remain intact, including the `parentOf` link from `06F1XPRY3ZDB6W1WQ9ABRRJ2V4` to `06F23Z08K0W49K5JMEHP60WZC0`.
- `docs/releases/v0.8.0.md` remains present and tracked as the release-documentation artifact for this epic.
- PO-critic can review the epic for closure without requiring new planning writes from this parent ticket.

## Implementation Notes
- Use `docs/releases/v0.8.0.md` as the repository artifact that satisfies the epic's release-documentation criterion.
- Keep the parent contract aligned with `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`: consumer-owned `IDesignTimeDbContextFactory<TContext>` plus a consumer-owned preflight entrypoint, not DVault-owned design-time services or CLI interception.
- Treat child `06F23Z08K0W49K5JMEHP60WZC0` as the completed release-summary owner; this parent ticket only tracks closure evidence.
- Do not reopen direct dev work from the parent epic unless later evidence shows a gap outside the completed child-ticket scope.

## Open Questions
- none

## Follow-Up Questions
- Should a later docs or examples ticket add one operator-facing end-to-end example that chains `dotnet ef migrations add`, consumer preflight, and `dotnet ef database update`?
- After v0.8.0, which provider should be the next live-schema reader after the SQLite-first baseline?
- Should a later guide consolidate artifact review, metadata or `ModelSnapshot` comparison, live-schema comparison, migration scaffolding, preflight, and database update into one operator workflow document?

## Risks
- If later edits remove or materially rewrite `docs/releases/v0.8.0.md` before closure review completes, the epic would regress against its release-documentation criterion.
- If later docs or ticket text reintroduce shorthand about DVault-owned design-time services, the closure evidence could drift from the ratified consumer-owned preflight boundary.

## Split Recommendations
- No further split recommended; this parent epic is closure/tracking only and the remaining work is PO-critic closure review against existing child and release-note evidence.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Make DVault safer inside the normal EF Core development lifecycle before adding heavier runtime features.

## Scope In

- Introduce stable DVault diagnostic codes.
- Validate generated EF migrations for Data Vault invariants.
- Expose design-time services for dotnet-ef workflows.
- Compare governed model artifacts against EF ModelSnapshot and optional live schema metadata.

## Scope Out

- No automatic migration execution.
- No provider-specific online migration engine.
- No breaking changes to existing v0.7.0 runtime APIs unless explicitly documented and guarded.

## Acceptance Criteria

- Child stories are done or intentionally superseded.
- Release documentation explains the lifecycle guardrail workflow.
- The release can be validated without package publishing credentials.

## Implementation Notes

- Tracking epic; implementation belongs in child stories and tasks.

## Open Questions

- none