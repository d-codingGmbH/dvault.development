[gicket-bot] PO refinement contract

Summary
- The queued replacement documentation carrier `mutation-d16ba25963e2af83` is the authoritative follow-up for the missing README, workflow, and v0.30.0 release-note work; child `06F8KZQAWZ7QRGB68KB21C9B0R` is historical only, and epic closure stays blocked until the docs land and the stale incoming `blocks` relation is reconciled.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - Do not reopen done child `06F8KZQAWZ7QRGB68KB21C9B0R`. The missing documentation work is already covered by the queued replacement ticket create replay `mutation-d16ba25963e2af83` on `develop`; that queued replay is the authoritative carrier unless it later fails, so a second duplicate documentation ticket is out of scope.
- critic-item-2: `answered` - For epic tracking, treat the queued replacement carrier as the active follow-up now. The epic must not use the current all-done child set as closure evidence, and once the replayed ticket exposes its ULID it must receive the active `parentOf` link from epic `06F8KZP0VKMXGE0JXPZRD1RQDG`.
- critic-item-3: `answered` - Do not resubmit this epic for closure-style review yet. First land the README freshness and recovery wording, the design-time workflow troubleshooting guidance, and `docs/releases/v0.30.0.md`; after that, remove or explicitly supersede the live incoming `blocks` relation from done child `06F8KZQAWZ7QRGB68KB21C9B0R` before closure.
- critic-item-4: `answered` - Confirmed. The epic Definition of Done is still open on documentation surfaces because `docs/releases/v0.30.0.md` is missing, `README.md` still names the v0.29.0 baseline, and `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` still lacks the explicit stale-input troubleshooting checklist or example. These are implementation gaps, not open architecture questions.
- critic-item-5: `answered` - Treat child `06F8KZQAWZ7QRGB68KB21C9B0R` as historical mis-tracking only. Remaining work is carried by the queued replacement documentation ticket, so the epic may continue without reopening completed analyzer or generator tickets, but the done child must not be used as closure evidence for the missing documentation.

Clarifications
- The queued create-ticket replay `mutation-d16ba25963e2af83` is the authoritative bounded replacement documentation carrier; do not create a second replacement ticket unless that replay is later confirmed failed.
- Child `06F8KZQAWZ7QRGB68KB21C9B0R` remains `done` as historical delivery history only and must not be treated as the live implementation carrier for the missing docs.
- The missing repository evidence remains bounded to three documentation surfaces: `README.md` freshness and recovery wording, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` stale-input troubleshooting guidance, and a new `docs/releases/v0.30.0.md`.
- The live incoming `blocks` relation from `06F8KZQAWZ7QRGB68KB21C9B0R` to the epic is closure-stage housekeeping: keep the contract consistent with that live state now, then remove or explicitly supersede it after the replacement documentation carrier lands.

Scope In
- Treat the queued replacement ticket as the one active carrier for the remaining documentation work and link it back to the epic when its ULID is visible.
- Land the README refresh and recovery wording for authoritative support-bundle regeneration and stale `DVaultTypedReadModelMetadataSourceFingerprint` recovery.
- Add explicit stale-input troubleshooting guidance to `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` for re-exporting support bundles and re-supplying representative `CreateSupportBundleDiagnostics` requests.
- Add `docs/releases/v0.30.0.md` as the current typed-helper freshness and stale-input recovery documentation baseline.
- Require closure-stage reconciliation or explicit supersession of the stale incoming `blocks` relation from `06F8KZQAWZ7QRGB68KB21C9B0R`.

Scope Out
- Creating a second duplicate documentation carrier while queued replay `mutation-d16ba25963e2af83` remains authoritative pending replay.
- Reopening completed analyzer, generator, or test implementation tickets without new behavioral regression evidence.
- Any new runtime behavior, source-generator redesign, or diagnostics expansion beyond the bounded documentation pass.
- Rewriting historical release notes such as `docs/releases/v0.29.0.md` as if earlier shipped behavior changed.
- Treating closure housekeeping as product-code work.

Open questions
- none

Follow-up questions
- After replay exposes the replacement ticket ULID, has the epic been back-linked with the active `parentOf` relation to that carrier?
- When the documentation carrier lands, should closure remove the stale incoming `blocks` relation outright or supersede it with explicit historical audit wording?

Risks
- If the queued replay is mistaken for a failed create and another documentation carrier is created, the epic may fork the same bounded documentation scope into duplicate tickets.
- Any closure-style review before the README, workflow, and v0.30.0 documentation updates land will fail the same documentation Definition of Done again.
- Leaving the stale incoming `blocks` relation unresolved after the replacement carrier lands can confuse closure automation or audit trails.

Split recommendations
- No further split beyond the single queued bounded replacement documentation carrier recorded as outbox `mutation-d16ba25963e2af83`.

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