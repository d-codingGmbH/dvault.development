[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q93R4633D41Z21WQW3SVGR/description.md` says `No direct product-code or documentation edits in this epic` and `Keep the epic tracking-only; authoritative implementation and documentation changes stay in the existing child tickets`, while the PO handoff still only says `decision: ready_for_po_critic`.
- Direct relation inspection under `.gicket/relations` shows exactly five `parentOf` files for this epic and no current incoming relation file; the stale path `.gicket/relations/G4/GR/06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.json` is absent.
- `git log --summary -- .gicket/relations/G4/GR/06F5Q93H60W6X8FJ88PWTR6NG4--06F5Q93R4633D41Z21WQW3SVGR--blocks.json` shows commit `f0038ba64 Unblock v0.23.0 epic relation` deleting that stale `blocks` relation file.
- Repository surfaces line up with the epic scope: `docs/architecture/dvault-v1-activity-tracing-contract.md` and `src/DCoding.Data.DVault/DataVaultActivityTracing.cs` both contain `DCoding.Data.DVault` plus matching activity names such as `dvault.save.single_request`, `dvault.read.pit`, and `dvault.maintenance.bridge.maintain_incremental`; `docs/performance-profiles.md` links `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` and repeats the checked-in run context (`3 iterations`, `1 warmup iteration`, `ProviderDefault`); `docs/releases/v0.23.0.md` and `README.md` keep package publication out of scope.

Blocking findings
- The durable ticket still does not make the closure/tracking-only lifecycle explicit enough for a success path that hands off to `dev`. The contract text says the parent epic owns no direct implementation or documentation work, but the persisted ticket metadata still presents it as a normal `todo` epic. That workflow-contract mismatch is the blocker.

Required PO actions
- Update the durable ticket contract and/or ticket metadata so this epic is explicitly marked as tracking-only closure/no-work-required, with no parent-owned implementation slice.
- Make the post-PO-critic routing explicit so a successful review does not hand this parent epic to ordinary developer implementation work.
- Preserve the existing child-completion and clean-relation evidence, but align status, labels, and handoff wording with the actual closure-only intent before rerunning PO-critic.

Open issues ledger
- critic-item-1 [required-po-action] Update the durable ticket contract and/or ticket metadata so this epic is explicitly marked as tracking-only closure/no-work-required, with no parent-owned implementation slice.
- critic-item-2 [required-po-action] Make the post-PO-critic routing explicit so a successful review does not hand this parent epic to ordinary developer implementation work.
- critic-item-3 [required-po-action] Preserve the existing child-completion and clean-relation evidence, but align status, labels, and handoff wording with the actual closure-only intent before rerunning PO-critic.
- critic-item-4 [blocking-finding] The durable ticket still does not make the closure/tracking-only lifecycle explicit enough for a success path that hands off to `dev`. The contract text says the parent epic owns no direct implementation or documentation work, but the persisted ticket metadata still presents it as a normal `todo` epic. That workflow-contract mismatch is the blocker.

Missing examples / edge cases
- The contract does not yet spell out the ticket-state behavior if integration later reopens one child or reintroduces an incoming `blocks` relation after the epic has been reclassified for closure.

Risky assumptions
- It assumes the live relation state referenced in comment `06F7X00DB95GTFD9VJD6AP7NFG.md` will remain `incomingCount=0` through final closure.
- It assumes the automation behind `approve_for_dev` can safely handle an epic whose own contract says there is no parent-owned implementation work, which the current durable metadata does not make explicit.

AC / test suggestions
- Convert the final relation or eligibility recheck from a follow-up question into explicit closure criteria or closure-routing language.

Implementation watchouts
- Do not hand this parent epic to a developer role unless the PO adds a genuine parent-owned implementation slice; current scope-out and implementation notes say all authoritative work stays in the child tickets.

Non-blocking notes
- The delivery content itself looks bounded and coherent: `## Open Questions` is `none`, the five-child decomposition still matches the relation graph, and the tracing contract, implementation vocabulary, benchmark guidance, release notes, and README all line up with the parent scope.

Split recommendations
- No additional split recommended; the existing five-child breakdown already covers contract, implementation, maintenance tracing, performance guidance, and coordinated documentation.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment