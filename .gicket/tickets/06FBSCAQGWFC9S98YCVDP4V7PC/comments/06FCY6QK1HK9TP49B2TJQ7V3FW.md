[gicket-bot] PO-critic review contract

Summary
- Repository evidence confirms the DB2 baseline already landed, but the live ticket still routes as open implementation work instead of a no-work-required closure, so it is not ready for developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/description.md routes the ticket to "close this ticket as no-work-required" and its ## Open Questions section is "- none".
- Comment .gicket/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/comments/06FCY4BJKXQMHJG585NG0RJBA8.md answers the prior critic routing question with "close this ticket as no-work-required", and comment .gicket/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/comments/06FCY4E4WYDM8J35TBNBHA6RJ4.md says the PO run updated labels only and kept status unchanged.
- git diff --name-only develop...HEAD returned only .gicket paths, so the current branch has no non-ticket-file delta against develop.
- git log --oneline --follow for docs/releases/v0.34.0.md, src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs, and tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs shows commit 1b5820269 "Complete v0.34.0 DB2 provider support" in file history.
- src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs registers Db2DataVaultSaveStrategy plus Db2DataVaultReadStrategy for PIT and bridge reads only; it does not register a DB2 latest-satellite read strategy.
- tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs asserts provider-neutral latest-satellite fallback and provider-selected Db2DataVaultReadStrategy for PIT/bridge shapes when configured.
- benchmark-summary.md keeps DB2 rows as skipped placeholders: the save row records db2SaveBoundary=clean-context-set-based and stagedBulkBoundary=not-supported, and the read rows keep latest-satellite provider-neutral while PIT/bridge plan Db2DataVaultReadStrategy.

Blocking findings
- none

Required PO actions
- Keep this ticket closure-only; if any additional DB2 evidence or documentation work is still desired, track it on a separate narrow follow-up instead of reopening this implementation ticket.

Open issues ledger
- critic-item-1 [required-po-action] Keep this ticket closure-only; if any additional DB2 evidence or documentation work is still desired, track it on a separate narrow follow-up instead of reopening this implementation ticket.

Missing examples / edge cases
- Specify the exact steady-state ticket field outcome expected for this no-work-required route, so automation and humans do not reinterpret the old implementation title and labels as active work.
- If PO prefers a different closure classification than no-work-required, encode that explicitly on the live ticket before re-handoff.

Risky assumptions
- Downstream automation would correctly infer a closure-only outcome from description text alone even while ticket.json still advertises an open implementation workflow state.
- The existing blocks relation from 06FBSCAQGWFC9S98YCVDP4V7PC to 06FBSCAX98ZFQZWBYEQMB8WF18 will be reconciled after closure so this ticket does not continue to gate follow-up documentation work.

AC / test suggestions
- Keep an explicit acceptance criterion that any future DB2 benchmark or documentation work must live on a separate ticket and must not widen this ticket beyond the existing clean-context save plus PIT/bridge read boundary.

Implementation watchouts
- Do not reopen staged DB2 bulk, provider-native chunk execution, or latest-satellite optimization from smoke coverage or skipped placeholder benchmark rows.
- Do not treat DB2 smoke tests or diagnostics-gated PIT/bridge strategy selection as completed DB2 timing evidence.

Non-blocking notes
- The previous PO-critic blocker in comment 06FCY25MWB1AQJ31VNWRSQDMHW.md was the missing routing choice; comment 06FCY4BJKXQMHJG585NG0RJBA8.md resolves that point, so the remaining blocker is live ticket-state alignment.
- The contract's ## Open Questions section is already `none`; this is a routing/state problem, not unresolved scope ambiguity.

Split recommendations
- Do not split this implementation ticket for new developer work; closure is the correct route for the current ticket.
- If DB2 evidence/documentation follow-up is still needed, use a separate narrowly scoped follow-up ticket rather than reopening 06FBSCAQGWFC9S98YCVDP4V7PC.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment