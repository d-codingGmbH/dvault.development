[gicket-bot] PO refinement contract

Summary
- Narrowed the parent to the landed 8.50.0/10.50.0 analyzer-host baseline, queued the 8.51.0/10.51.0 follow-up rewrite on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC, and materialized relation cleanup for stale child blockers.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This parent is now narrowed to the already-landed 8.50.0/10.50.0 analyzer-host baseline. The applied parent description rewrite supersedes the earlier mixed wording, and the parent-owned blocks relation to 06FH8RP1SBVZ7K3K48ERGZSMQC was removed so 8.51.0/10.51.0 is no longer a landing condition on this ticket.
- critic-item-2: `answered` - Ticket 06FH8RP1SBVZ7K3K48ERGZSMQC remains the intended carrier for the future 8.51.0/10.51.0 release-note, changelog, install-guidance, and package-validation updates, and a delivery-contract-quality description rewrite was queued on its owner branch.
- critic-item-3: `answered` - Relation noise has been actively cleaned rather than merely noted: the parent-side blocks edge to the future roll-forward ticket was removed immediately, and each stale done-child blocks edge back to the parent now has a durable queued removal on its canonical owner branch.
- critic-item-4: `answered` - The mixed-scope issue is resolved. This parent now tracks only the implemented analyzer-host baseline already evidenced in repository files and validation scripts, while the remaining future release-surface work has one explicit carrier in ticket 06FH8RP1SBVZ7K3K48ERGZSMQC.

Clarifications
- Applied a parent description rewrite on ticket 06FH8QAVJFXANVQFXGPYVAFXSR at revision 06FHCB97DQBFJGPE5SZKK8MZX4 to align the story with the landed 8.50.0/10.50.0 analyzer-host baseline.
- Removed the parent-owned blocks relation to 06FH8RP1SBVZ7K3K48ERGZSMQC at revision 06FHCBCKYJ9QCSZR4JT9SN52JW so the future roll-forward no longer gates this parent.
- Queued replay on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC owner branch for the follow-up delivery contract update as outbox mutation-b90172254935d5d4.

Scope In
- Track the already-landed analyzer-host compatibility baseline for package lines 8.50.0 and 10.50.0: one netstandard2.0 DCoding.Data.DVault.Analyzers asset under analyzers/dotnet/cs/.
- Track repository-backed validation and guidance for packaged analyzer consumption on pure .NET 8 SDK and .NET 10 SDK build hosts, including PrivateAssets=all consumer guidance.
- Track closure alignment between the implemented repo baseline, this parent story contract, and the completed strategy, implementation, proof, and documentation child tickets.

Scope Out
- Any 8.51.0 / 10.51.0 release-note, changelog, install-guidance, package-validation, or publish-baseline roll-forward; that work belongs to ticket 06FH8RP1SBVZ7K3K48ERGZSMQC.
- New analyzer package ids, split code-fix packages, target-specific analyzer asset trees, or runtime lib/<tfm> assets.
- Analyzer-host compatibility claims beyond the repository-backed .NET 8 SDK and .NET 10 SDK CLI build-host boundary.

Open questions
- none

Follow-up questions
- Confirm replay completion for the queued follow-up description update on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC so its branch carries the authoritative 8.51.0 / 10.51.0 delivery contract.
- If future host claims need IDE or editor validation beyond CLI SDK-host proof, schedule that as a separate follow-up rather than broadening this parent story.

Risks
- Until queued replay finishes on ticket 06FH8RP1SBVZ7K3K48ERGZSMQC, the follow-up's persisted description may temporarily lag the intended 8.51.0 / 10.51.0 delivery contract.
- A later package-line roll-forward can drift if changelog, release notes, install guidance, pack script, and package verification are not updated together on the follow-up ticket.

Split recommendations
- No additional split is needed; this parent is now bounded to the landed 8.50.0 / 10.50.0 baseline, and ticket 06FH8RP1SBVZ7K3K48ERGZSMQC is the single remaining carrier for the future 8.51.0 / 10.51.0 release-surface work.

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