[gicket-bot] PO refinement contract

Summary
- Addressed the PO-critic ledger by making the routing explicit: this is not a closure-only outcome because the branch still lacks landed v0.49 documentation updates, so the ticket remains a bounded developer documentation rollover and the existing delivery contract stays intact.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The ticket is handed off as a normal developer documentation task, not as a closure-only outcome. The current delivery contract already frames the work as updating the v0.49 release notes, CHANGELOG, README limitations, analyzer README, package compatibility guidance, and support-bundle guidance to the 8.49.0 / 10.49.0 baseline.
- critic-item-2: `answered` - Product is not taking a closure-only route here because the current branch does not supply landed v0.49 documentation evidence. The active contract keeps this as unfinished developer documentation work until those named surfaces are actually rolled forward.
- critic-item-3: `answered` - The existing delivery contract content is kept. It is already bounded, preserves the current shipped same-hub, support-bundle, analyzer, and deferred-scope boundaries, and leaves no blocking PO open questions.
- critic-item-4: `answered` - Agreed. This cannot be treated as closure-only because the repository evidence still shows the v0.48 documentation baseline and no landed v0.49 release-note file. That finding is resolved by keeping the ticket active as a normal developer documentation task rather than claiming it is already complete.

Clarifications
- This remains a normal developer documentation ticket because the current branch still lacks landed v0.49 doc-path evidence, including a checked-in docs/releases/v0.49.0.md surface.
- The analyzer package compatibility audit remains the authoritative compatibility constraint: DCoding.Data.DVault.Analyzers stays one net10.0 analyzer asset and both visible package lines continue to require a .NET 10 SDK build host.
- Related done ticket 06FF43YPV3WYDQHEGZSW4T296C establishes shipped repeated same-hub generated typed link-mapper parity when produced participant names are explicit, non-blank, and unique by StringComparer.Ordinal; v0.49 docs should describe that supported boundary.
- Related done ticket 06FF441DM4F4ZDTHY9ZZD9RA8R keeps dependent child key modeling deferred, so v0.49 docs must not imply dependent-child metadata, helper, or API support.
- Related done ticket 06FF442BD5V9CTTNXQQAR3EQTW plus the current typed PIT and bridge helper contract keep the generated read-helper baseline support-bundle-driven: one authoritative dvault.support-bundle.v1 input, request-bound ReadShape evidence for PIT or bridge helpers, and no raw dvault.model.v1 direct generation.
- No child tickets, relation writes, description rewrites, planning documents, or attachments were materialized in this response; this pass answers the PO-critic ledger and preserves the existing bounded contract.

Scope In
- Update the v0.49 release notes, CHANGELOG, README limitations, analyzer README, package compatibility guidance, and support-bundle guidance so the visible package lines roll coherently to 8.49.0 and 10.49.0.
- Document shipped repeated same-hub generated typed link-mapper parity with explicit role-bearing produced participant names and the existing explicit IDataVaultSaveService boundary.
- Keep current typed read-model generator limits explicit: authoritative dvault.support-bundle.v1 input only, request-bound ReadShape evidence for PIT and bridge helpers, and no raw dvault.model.v1 direct parsing or source-visible direct helper generation.
- Carry forward effectivity guidance as the already-implemented generic link-parent satellite pattern rather than introducing a new first-class effectivity API claim.
- Preserve the current analyzer package compatibility and build-host guidance while updating the versioned release baseline.

Scope Out
- Any runtime, generator, analyzer, provider, or package-publication implementation work.
- Analyzer retargeting or any claim that pure .NET 8 SDK analyzer consumption is supported.
- New dependent child key modeling, effectivity-specific fluent APIs, or broader modeling-parity claims beyond the repository-backed shipped boundary.
- Relation cleanup, child-ticket creation, or planning-document materialization unless later evidence shows the documentation slice is no longer bounded.

Open questions
- none

Follow-up questions
- If product later wants runnable same-hub or effectivity examples, should that land as a separate docs or examples ticket after the v0.49 baseline is published?
- If product later wants pure .NET 8 SDK analyzer consumption or dependent child modeling, open separate capability tickets rather than widening the v0.49 documentation baseline.

Risks
- The v0.48 baseline is repeated across multiple docs, so a partial rollover could leave contradictory 8.48.0 and 10.48.0 versus 8.49.0 and 10.49.0 guidance.
- Readers may conflate typed read helpers with typed save mappers or source-generator parity unless the v0.49 docs keep the support-bundle-driven read-helper limits and the same-hub mapper support clearly separated.
- If analyzer wording is relaxed beyond the audit evidence, consumers could infer unsupported pure .NET 8 SDK analyzer compatibility.
- If dependent child and effectivity-specific API caveats are dropped during the refresh, the release could appear to claim broader modeling parity than the repository currently proves.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment