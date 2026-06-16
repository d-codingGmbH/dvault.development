[gicket-bot] PO-critic review contract

Summary
- Repository evidence supports a closure-only DB2 baseline, but the persisted ticket metadata still routes this item as an open implementation ticket, so it should return to PO for ticket-state cleanup instead of dev handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/description.md` lines 5-45 says the ticket is `closure-only`, on a `no-work-required closure path`, and `## Open Questions` is `- none`.
- `git show --stat --name-only HEAD` shows HEAD `09ef7af6fdaec87f150b8f4537aad7df86bb22c3` is only the PO-critic lease-claim commit, and `git diff --name-only ce08bcae99d093c9168022ab3878669e7ae15abf..09ef7af6fdaec87f150b8f4537aad7df86bb22c3` lists only `.gicket/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/...` files.
- `docs/releases/v0.34.0.md` lines 41-43 and 82 say the landed DB2 boundary is `AddDVaultDb2()` plus clean-context save and diagnostics-gated PIT/bridge reads; latest-satellite stays provider-neutral, and staged bulk plus provider-native chunk execution remain out of scope.
- `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` lines 15-25 registers `Db2DataVaultSaveStrategy` plus `Db2DataVaultReadStrategy` for PIT/bridge reads, and `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs` lines 307-348 assert provider-neutral latest-satellite fallback plus `Db2DataVaultReadStrategy` for PIT/bridge reads.
- `benchmark-summary.md` lines 73-74 and 87-89 record DB2 rows as `skipped` and `not executed`; the save row names `Db2DataVaultSaveStrategy`, the latest-satellite row keeps `selectedStrategy=<none>`, and PIT/bridge rows name `Db2DataVaultReadStrategy`. `git log --oneline --decorate --max-count=12 -- docs/releases/v0.34.0.md src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs benchmark-summary.md` includes `1b5820269 (tag: v0.34.0) Complete v0.34.0 DB2 provider support`.

Blocking findings
- `.gicket/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/comments/06FCY6SZ8XFX59KFA143ZG0EM8.md` already recorded the same workflow-state risk, and the current `ticket.json` still shows that mismatch.

Required PO actions
- If additional DB2 benchmark or documentation evidence is still desired, track it on a separate narrow follow-up ticket rather than reopening `06FBSCAQGWFC9S98YCVDP4V7PC`.

Open issues ledger
- critic-item-1 [required-po-action] If additional DB2 benchmark or documentation evidence is still desired, track it on a separate narrow follow-up ticket rather than reopening `06FBSCAQGWFC9S98YCVDP4V7PC`.
- critic-item-2 [blocking-finding] `.gicket/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/comments/06FCY6SZ8XFX59KFA143ZG0EM8.md` already recorded the same workflow-state risk, and the current `ticket.json` still shows that mismatch.

Missing examples / edge cases
- No extra product examples are missing from the closure contract; the remaining gap is ticket-state routing, not scope definition.
- The main ticket-level edge case still needing explicit handling is the metadata transition from an implementation workflow state to a no-work-required closure state.

Risky assumptions
- Assuming downstream automation or reviewers will infer `no-work-required closure` from `description.md` alone while `.gicket/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/ticket.json` still says `todo` and carries implementation-routing labels.
- Assuming skipped-placeholder DB2 benchmark rows or opt-in smoke coverage will not be overstated as completed DB2 timing evidence despite the contract and release note warning against that.

AC / test suggestions
- Keep the closure comment anchored to the four repository artifacts already named in the contract: `docs/releases/v0.34.0.md`, `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs`, `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs`, and `benchmark-summary.md`.

Implementation watchouts
- If this ticket is mistakenly handed to development, the likely regression is scope creep into staged DB2 bulk, provider-native chunk execution, or DB2 latest-satellite optimization, all of which the current contract and `docs/releases/v0.34.0.md` keep out of scope.
- Any later DB2 timing claims still require provider-configured benchmark evidence; the current `benchmark-summary.md` DB2 rows are preserved placeholders, not completed timing results.

Non-blocking notes
- The current contract is internally consistent: `description.md` explicitly marks the contract block as authoritative and records `## Open Questions` as `- none`.
- Branch history supports closure rather than development: the latest branch delta is ticket metadata and comment churn under `.gicket`, while the actual DB2 baseline traces back to the earlier `v0.34.0` landing.

Split recommendations
- Do not split this implementation ticket into new developer work.
- If the team still wants DB2 evidence or documentation expansion, use one separate narrow evidence or documentation follow-up ticket.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment