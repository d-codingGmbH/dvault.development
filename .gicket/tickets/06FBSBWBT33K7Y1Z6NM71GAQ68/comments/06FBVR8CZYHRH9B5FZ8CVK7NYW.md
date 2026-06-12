[gicket-bot] PO refinement contract

Summary
- Repository and ticket evidence show the explicit `.NET 10 SDK` gate path is already present on `develop`; this ticket has no residual implementation delta and should be treated as a closure-only/no-work-required refinement while docs and verifier ownership stay outside this ticket.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - No residual implementation work remains for this ticket. `develop` already carries the selected single-asset `net10.0` analyzer packaging plus the explicit `.NET 10 SDK` host guidance, so this ticket should move to closure/no-work-required rather than back to developers.
- critic-item-2: `answered` - Ownership is now explicit: this ticket is narrowed to closure-only for the already-landed asset-or-SDK-gate decision, and README/package-verifier/validation ownership is removed from this ticket. If any residual docs or verification delta is later proven, that scope belongs only to `06FBSBWH9F415E12VRHRYQ2JJM`.
- critic-item-3: `answered` - No concrete missing artifact or failing verifier expectation remains. The correct refinement is to remove developer handoff, because the repository already contains the analyzer-host validation lane, the packaged README guidance, and the package-verifier expectations that this ticket previously described.
- critic-item-4: `answered` - Confirmed. Direct repository evidence already matches the single-asset `net10.0` plus explicit `.NET 10 SDK` gate path, and the current ticket branch carries no non-ticket repository delta. The contract is therefore changed from ambiguous developer handoff to explicit closure/no-work-required.
- critic-item-5: `answered` - Scope ownership is clarified by removing documentation and verification work from this ticket. `06FBSBWH9F415E12VRHRYQ2JJM` remains the only open ticket that could own that area if a real residual delta later appears; the live `blocks` relation from this ticket is now only stale metadata, not proof of remaining implementation work here.

Clarifications
- This refinement converts `06FBSBWBT33K7Y1Z6NM71GAQ68` from an implementation handoff into a closure-only/no-work-required ticket because the selected explicit SDK-gate path is already landed on `develop`.
- This ticket no longer owns README, packaged README, package-verifier, or validation-lane work; that scope is outside this ticket and would belong to `06FBSBWH9F415E12VRHRYQ2JJM` only if a real residual delta were later proven.
- No child tickets, attachments, planning documents, or relation writes were materialized in this run; the refinement only narrows the contract based on repository and ticket evidence.
- The live `blocks` relation from `06FBSBWBT33K7Y1Z6NM71GAQ68` to `06FBSBWH9F415E12VRHRYQ2JJM` remains stale metadata in current state and should not be read as evidence of remaining implementation work on this ticket.

Scope In
- Confirm that the repository already implements the single-asset `net10.0` analyzer plus explicit `.NET 10 SDK` host requirement baseline.
- Refine this ticket to closure/no-work-required based on that already-landed baseline.
- Preserve the compatibility decision boundary so any future expansion beyond the current SDK-gate path is tracked separately.

Scope Out
- Any new analyzer asset retargeting, extra package layout work, README edits, packaged README edits, package-verifier edits, or test changes for the current v0.36 baseline.
- Documentation or verification implementation work inside this ticket; that area is not owned here.
- A future pure `.NET 8 SDK` analyzer-consumption compatibility expansion.

Open questions
- none

Follow-up questions
- Should `06FBSBWH9F415E12VRHRYQ2JJM` also be closed or re-scoped as no-work-required, since its stated README/package-verification changes appear already present on `develop`?
- Should the stale live `blocks` relation `06FBSBWBT33K7Y1Z6NM71GAQ68 -> 06FBSBWH9F415E12VRHRYQ2JJM` be removed as relation housekeeping when that related ticket is next touched?

Risks
- If the stale live `blocks` relation stays in place, later readers may wrongly infer that `06FBSBWH9F415E12VRHRYQ2JJM` still waits on implementation work from this ticket even though the repository delta is already landed.
- Reopening this ticket for documentation or verification edits would duplicate scope and recreate ambiguity with `06FBSBWH9F415E12VRHRYQ2JJM`.

Split recommendations
- No new split is needed. Keep `06FBSBWBT33K7Y1Z6NM71GAQ68` as closure/no-work-required and use a separate ticket only if a new compatibility delta appears later; if a real docs/verifier delta reappears, `06FBSBWH9F415E12VRHRYQ2JJM` is the existing holder.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment