[gicket-bot] PO-critic review contract

Summary
- Delivery contract is clear and closure-only, open questions are resolved, and repository plus branch evidence shows the DB2 baseline already landed; approve for downstream handling without reopening implementation scope.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSCAQGWFC9S98YCVDP4V7PC/description.md` marks the delivery contract authoritative, says the ticket stays on a `no-work-required closure path`, and `## Open Questions` is `- none`.
- `docs/releases/v0.34.0.md` says the landed DB2 boundary is `AddDVaultDb2()` with clean-context save plus diagnostics-gated PIT/bridge reads, while latest-satellite stays provider-neutral and staged bulk/provider-native chunk execution are out.
- `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` registers `Db2DataVaultSaveStrategy` plus `Db2DataVaultReadStrategy` only for PIT/bridge reads.
- `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs` asserts provider-neutral latest-satellite fallback and provider-selected `Db2DataVaultReadStrategy` for PIT/bridge shapes when DB2 is configured.
- `benchmark-summary.md` keeps DB2 rows as skipped placeholders: save row `dvault-adddvaultdb2-optimized` is `not executed`, latest-satellite keeps `selectedStrategy=<none>`, and PIT/bridge rows plan `Db2DataVaultReadStrategy`.
- `git -C /mnt/c/Projects/DVault log --oneline -- docs/releases/v0.34.0.md src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs benchmark-summary.md` includes commit `1b5820269 Complete v0.34.0 DB2 provider support`, and `git -C /mnt/c/Projects/DVault diff --name-only develop...HEAD` shows the current branch delta is ticket metadata under `.gicket` only.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Downstream routing/closure handling will treat the current `ticket.json` workflow markers (`todo`, `critic-needed`, `blocked/dev`, `blocked/test`) as transient workflow state rather than renewed implementation scope, consistent with comment `06FCYBPYRTKH4KR63GT3ZKACVW.md`.
- Skipped-placeholder DB2 benchmark rows and opt-in smoke coverage will not be restated later as completed DB2 timing evidence.

AC / test suggestions
- When closing or handing off, keep the closure record anchored to `docs/releases/v0.34.0.md`, `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs`, `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs`, and `benchmark-summary.md`.
- Preserve an explicit acceptance note that any later DB2 benchmark or documentation expansion must be opened as one separate evidence-only ticket, not folded back into `06FBSCAQGWFC9S98YCVDP4V7PC`.

Implementation watchouts
- Do not turn this handoff into new DB2 implementation work; staged DB2 bulk, provider-native chunk execution, latest-satellite optimization, and provider-specific PIT/bridge maintenance remain out of scope per `docs/releases/v0.34.0.md` and the contract.
- Do not interpret the DB2 benchmark placeholders in `benchmark-summary.md` as completed timing evidence; they are audit anchors only until a provider-configured run exists.

Non-blocking notes
- The legacy draft still says `Implement the accepted DB2 bulk improvement`, but the authoritative contract block in `description.md` supersedes it and is internally consistent.
- Current branch history is ticket-metadata churn only; no additional repository implementation delta was introduced on this ticket branch beyond the already-landed DB2 baseline.

Split recommendations
- Do not split or reopen this implementation ticket for developer work.
- If stakeholders later want provider-configured DB2 benchmark artifacts or extra DB2 documentation, open one new narrow evidence-only follow-up ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment