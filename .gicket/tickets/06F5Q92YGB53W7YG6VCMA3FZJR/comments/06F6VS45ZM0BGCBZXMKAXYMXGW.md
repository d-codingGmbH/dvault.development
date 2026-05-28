[gicket-bot] PO-critic review contract

Summary
- Return to PO: the contract text is detailed, but this ticket's remaining owned deliverable is unclear after the main implementation slices were delegated to already-done child tickets and the documentation scope overlaps a separate blocked docs task.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q92YGB53W7YG6VCMA3FZJR/description.md` has `## Open Questions` -> `none`, but its acceptance criteria still include package/analyzer documentation and deterministic-only code-fix scope.
- `docs/plans/typed-read-model-generator-contract.md` states ticket `06F5Q92AHG0ZCTVQGC6NAYVP9C` implements the satellite slice and ticket `06F5Q92R02HB7FCE1AWKXPTMRW` implements the PIT/bridge slice.
- `git diff --name-status develop...HEAD` shows only `.gicket/tickets/06F5Q92YGB53W7YG6VCMA3FZJR/{ticket.json,description.md,comments/*,events/*}` changes; no repository source or docs files changed on this ticket branch.
- Existing repo docs already cover much of this story surface: `README.md` and `src/DCoding.Data.DVault.Analyzers/README.md` mention optional analyzer install, `PrivateAssets="all"`, `DVaultGenerateTypedReadModels=true`, `DMV1910`/`DMV1911`, and `DMV1960`-`DMV1969`.
- Documentation ownership overlaps another ticket: `.gicket/relations/JR/G4/06F5Q92YGB53W7YG6VCMA3FZJR--06F5Q93H60W6X8FJ88PWTR6NG4--blocks.json` shows this ticket blocks docs task `06F5Q93H60W6X8FJ88PWTR6NG4`, and that task's `.gicket/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/description.md` says `Goal: Update docs for typed read model generation and hash governance.`
- Related child descriptions still defer residual decisions back here: `.gicket/tickets/06F5Q92R02HB7FCE1AWKXPTMRW/description.md` asks whether this follow-up still needs deterministic local code fixes, and `.gicket/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/description.md` asks whether README should consolidate the full `DMV1960`-`DMV1969` section after both child tickets land.

Blocking findings
- The ticket no longer states a distinct remaining developer-owned deliverable after the implementation slices were assigned to `06F5Q92AHG0ZCTVQGC6NAYVP9C` and `06F5Q92R02HB7FCE1AWKXPTMRW`, and both related tickets now read `done`.
- Documentation ownership is ambiguous: this ticket's acceptance criteria include documentation work, but separate blocked task `06F5Q93H60W6X8FJ88PWTR6NG4` also owns the typed-read documentation rollup.
- The residual decision on whether this ticket still ships any deterministic typed-read code fix is not explicit; related ticket text still treats that as a follow-up question.

Required PO actions
- Re-baseline this ticket to one explicit residual deliverable that is not already owned by `06F5Q92AHG0ZCTVQGC6NAYVP9C`, `06F5Q92R02HB7FCE1AWKXPTMRW`, or docs task `06F5Q93H60W6X8FJ88PWTR6NG4`.
- Decide documentation ownership explicitly: either keep docs in this ticket and narrow/remove the overlap on `06F5Q93H60W6X8FJ88PWTR6NG4`, or strip docs AC from this story and let the docs task own the rollup.
- State explicitly whether zero new typed-read code fixes is acceptable for this ticket; if not, name the exact diagnostic/edit pair that this ticket must ship.

Open issues ledger
- critic-item-1 [required-po-action] Re-baseline this ticket to one explicit residual deliverable that is not already owned by `06F5Q92AHG0ZCTVQGC6NAYVP9C`, `06F5Q92R02HB7FCE1AWKXPTMRW`, or docs task `06F5Q93H60W6X8FJ88PWTR6NG4`.
- critic-item-2 [required-po-action] Decide documentation ownership explicitly: either keep docs in this ticket and narrow/remove the overlap on `06F5Q93H60W6X8FJ88PWTR6NG4`, or strip docs AC from this story and let the docs task own the rollup.
- critic-item-3 [required-po-action] State explicitly whether zero new typed-read code fixes is acceptable for this ticket; if not, name the exact diagnostic/edit pair that this ticket must ship.
- critic-item-4 [blocking-finding] The ticket no longer states a distinct remaining developer-owned deliverable after the implementation slices were assigned to `06F5Q92AHG0ZCTVQGC6NAYVP9C` and `06F5Q92R02HB7FCE1AWKXPTMRW`, and both related tickets now read `done`.
- critic-item-5 [blocking-finding] Documentation ownership is ambiguous: this ticket's acceptance criteria include documentation work, but separate blocked task `06F5Q93H60W6X8FJ88PWTR6NG4` also owns the typed-read documentation rollup.
- critic-item-6 [blocking-finding] The residual decision on whether this ticket still ships any deterministic typed-read code fix is not explicit; related ticket text still treats that as a follow-up question.

Missing examples / edge cases
- none

Risky assumptions
- Assuming a meaningful `code fixes` deliverable still exists here even though the contract mostly defines negative fixer guardrails and no positive typed-read fixer target.
- Assuming the existing README and analyzer README coverage is not already the documentation work, even though a separate blocked docs task carries that ownership.

AC / test suggestions
- If the ticket stays open, rewrite acceptance criteria so every remaining item is residual-only and directly attributable to this ticket instead of restating child-ticket scope.
- If a typed-read code fix is still expected, add one concrete acceptance criterion naming the diagnostic and the deterministic local edit; if none is expected, say that explicitly.

Implementation watchouts
- Do not reopen satellite-generator work already owned by `06F5Q92AHG0ZCTVQGC6NAYVP9C` or PIT/bridge-generator work already owned by `06F5Q92R02HB7FCE1AWKXPTMRW`.
- Do not duplicate the typed-read documentation rollup if `06F5Q93H60W6X8FJ88PWTR6NG4` remains the docs owner.
- Keep any residual scope bounded to the existing analyzer package and current `IDataVaultReadService` boundaries.

Non-blocking notes
- The delivery contract itself is otherwise detailed and internally consistent, and `Open Questions` is explicitly `none`.
- Repository sources already align with many boundary claims: `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs`, `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs`, and `src/DCoding.Data.DVault/IDataVaultReadService.cs` are present on the branch.

Split recommendations
- Keep satellite generator work on `06F5Q92AHG0ZCTVQGC6NAYVP9C` and PIT/bridge generator work on `06F5Q92R02HB7FCE1AWKXPTMRW`.
- Treat this ticket as residual integration/closure-only work only after PO explicitly states what remains and how it differs from docs task `06F5Q93H60W6X8FJ88PWTR6NG4`.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment