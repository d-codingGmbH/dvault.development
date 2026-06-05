[gicket-bot] PO refinement contract

Summary
- Queued one bounded replacement documentation ticket on develop, verified the repo still lacks the README/workflow/v0.30.0 documentation evidence, and kept the epic in needs_po_clarification because the queued create did not return a ticket ULID for the required active follow-up link.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Created one bounded replacement documentation ticket instead of reopening 06F8KZQAWZ7QRGB68KB21C9B0R. The previous tool result create-replacement-doc-ticket queued Task: Deliver v0.30.0 typed helper freshness documentation on develop as outbox mutation-d16ba25963e2af83, with bounded scope matching the missing README freshness/recovery wording, design-time workflow troubleshooting guidance, and new docs/releases/v0.30.0.md release note.
- critic-item-2: `cannot_answer` - Cannot complete the epic tracking-state write in this run. The queued replacement ticket create did not return a created ticket ULID, so no parentOf or follow-up relation from epic 06F8KZP0VKMXGE0JXPZRD1RQDG could be materialized yet. Current relation and ticket state still show only the four existing children, and all four are status done, so the epic still does not show an active linked documentation follow-up.
- critic-item-3: `answered` - Closure-style resubmission is still premature. Repository evidence still lacks docs/releases/v0.30.0.md, README.md still names v0.29.0 as the current coordinated baseline, the design-time workflow doc describes support-bundle export and representative CreateSupportBundleDiagnostics usage but not an explicit stale-input troubleshooting or recovery checklist, and the incoming blocks relation from 06F8KZQAWZ7QRGB68KB21C9B0R to the epic remains present in relation and event data. Landed documentation evidence plus relation reconciliation or explicit supersession are still required before closure review.
- critic-item-4: `answered` - Verified. The epic documentation Definition of Done is not met on current repository surfaces: docs/releases/v0.30.0.md does not exist, README.md still presents v0.29.0 as the current baseline, and the design-time workflow doc has support-bundle export guidance but not the explicit freshness or troubleshooting example for stale support-bundle or readShape evidence recovery.
- critic-item-5: `answered` - Verified. Tracking coverage is inconsistent on the current branch: the epic still links only to four children and all four are status done, while documentation child 06F8KZQAWZ7QRGB68KB21C9B0R remains marked done with closure/no-work-required even though its own delivery contract still calls for the README, workflow, and v0.30.0 release-note changes that are not landed. The queued replacement ticket addresses the split decision, but tracking is not fully repaired until that new ticket exists and is linked back to the epic.

Clarifications
- A bounded replacement documentation ticket was materialized as a queued create-ticket replay on develop: outbox mutation-d16ba25963e2af83 for Task: Deliver v0.30.0 typed helper freshness documentation.
- Current repository evidence still shows the three documentation gaps called out by PO-critic: missing docs/releases/v0.30.0.md, README.md still anchored to v0.29.0, and no explicit stale-input troubleshooting checklist in docs/architecture/dvault-dotnet-ef-design-time-workflow.md.
- The incoming blocks relation from 06F8KZQAWZ7QRGB68KB21C9B0R to the epic remains present in persisted relation and event data and must be reconciled or explicitly superseded before epic closure review.

Scope In
- Track one bounded documentation follow-up covering README freshness and recovery wording, design-time workflow troubleshooting guidance, and the new docs/releases/v0.30.0.md release note.
- Preserve the already-ratified analyzer, generator, fingerprint, and readShape architecture boundaries without reopening implementation scope.
- Make epic closure prerequisites explicit: landed documentation evidence plus reconciliation or explicit supersession of the stale incoming blocks relation when closure is attempted.

Scope Out
- Reopening completed analyzer, generator, or test implementation tickets unless later repository evidence shows a real behavioral regression.
- Any new runtime behavior, source-generator redesign, or diagnostics expansion beyond the bounded documentation carrier.
- Rewriting historical release notes such as docs/releases/v0.29.0.md as though earlier shipped behavior changed.

Open questions
- The queued replacement documentation ticket does not yet expose a created ticket ULID in current branch context, so the epic still cannot materialize the required active parentOf or follow-up link in this run.

Follow-up questions
- After the documentation carrier lands, should closure remove the stale incoming blocks relation from 06F8KZQAWZ7QRGB68KB21C9B0R or supersede it with explicit historical audit wording if the relation file is retained?

Risks
- Until the queued replacement ticket receives a visible ULID and relation link, epic tracking coverage remains inconsistent and critic-item-2 stays open.
- Returning the epic to closure-style review before the README, workflow, and v0.30.0 evidence lands would fail the same documentation Definition of Done again.
- The stale incoming blocks relation from 06F8KZQAWZ7QRGB68KB21C9B0R can confuse closure automation if it is not reconciled or explicitly superseded after the documentation carrier completes.

Split recommendations
- No further split beyond the single bounded documentation carrier already queued as Task: Deliver v0.30.0 typed helper freshness documentation.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment