[gicket-bot] PO refinement contract

Summary
- Refined this as a bounded v0.49 documentation rollover: move package guidance to 8.49.0 and 10.49.0, document shipped same-hub mapper parity, carry forward support-bundle and analyzer limits, and keep deferred modeling gaps explicit.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository docs still present a v0.48.0 / 8.48.0 / 10.48.0 baseline, so this ticket is a documentation alignment pass rather than a new feature-design ticket.
- The analyzer package compatibility audit remains the authoritative compatibility constraint: DCoding.Data.DVault.Analyzers stays one net10.0 analyzer asset and both 8.49.0 and 10.49.0 package lines still require a .NET 10 SDK build host.
- Related done ticket 06FF43YPV3WYDQHEGZSW4T296C establishes shipped repeated same-hub generated typed link-mapper parity when produced participant names are explicit, non-blank, and unique by StringComparer.Ordinal; v0.49 docs should describe that as supported scope.
- Related done ticket 06FF441DM4F4ZDTHY9ZZD9RA8R keeps dependent child key modeling deferred, so v0.49 docs must not imply dependent-child metadata, helper, or API support.
- Related done ticket 06FF442BD5V9CTTNXQQAR3EQTW plus the current typed PIT and bridge helper contract keep the generated read-helper baseline support-bundle-driven: one authoritative dvault.support-bundle.v1 input, request-bound ReadShape evidence for PIT or bridge helpers, and no raw dvault.model.v1 direct generation.
- All current incoming blocks relations for this ticket point to done tickets, so the remaining work is documentation alignment only and no new PO blocker remains.

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