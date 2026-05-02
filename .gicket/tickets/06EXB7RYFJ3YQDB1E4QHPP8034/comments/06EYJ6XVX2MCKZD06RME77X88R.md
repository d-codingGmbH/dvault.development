[gicket-bot] PO-critic review contract

Summary
- Ticket is improved and grounded in repo docs, but it still leaves the comparison history sequence implicit, so it is not yet safe for developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- The delivery contract in `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/description.md:12-21` anchors the work to the MVP customer-profile scenario, requires multiple profile states, and says the persisted outcomes must serve as the later DVault comparison baseline.
- The same contract in `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/description.md:44-57` requires deterministic timestamps and cross-ticket alignment, and explicitly lists divergence on the exact customer-profile change sequence as a risk.
- `docs/architecture/mvp-data-vault-concepts.md:114-121` contains only one concrete `sat_customer_profile` example row: `customer_hk='hk_customer_001'`, `customer_name='Ada Lake'`, `customer_status='active'`, `load_ts='<redacted>-29T10:18:00Z'`; it does not define the subsequent changed state required by this ticket.
- `rg -n "customer_name|customer_status|sat_customer_profile|customer profile" src tests docs` only returns `docs/architecture/mvp-data-vault-concepts.md`, so there is no existing source/test example in the repository that fills in the missing second state.

Blocking findings
- The authoritative baseline source the ticket points to documents only the initial customer profile row, not the required subsequent changed state. Developers would have to invent the exact second state, timestamps, and expected stored rows.
- Cross-ticket comparison is a stated goal, but the paired DVault ticket is not yet refined to the same scenario detail. Without a shared sequence/assertion contract, this ticket can still drift in the exact history it establishes.

Required PO actions
- Add one concrete history sequence to this ticket or a shared linked artifact: business key, initial values, changed values, deterministic timestamps, and the expected persisted rows the plain EF baseline must assert.
- Either refine ticket `06EXB7S6DB97GVVTS2GGZ3CCX8` to reference the same exact sequence or attach one shared comparison contract that both tickets must follow.
- Clarify whether the comparison baseline must assert only understandable history or an exact row-by-row persisted outcome contract.

Open issues ledger
- critic-item-1 [required-po-action] Add one concrete history sequence to this ticket or a shared linked artifact: business key, initial values, changed values, deterministic timestamps, and the expected persisted rows the plain EF baseline must assert.
- critic-item-2 [required-po-action] Either refine ticket `06EXB7S6DB97GVVTS2GGZ3CCX8` to reference the same exact sequence or attach one shared comparison contract that both tickets must follow.
- critic-item-3 [required-po-action] Clarify whether the comparison baseline must assert only understandable history or an exact row-by-row persisted outcome contract.
- critic-item-4 [blocking-finding] The authoritative baseline source the ticket points to documents only the initial customer profile row, not the required subsequent changed state. Developers would have to invent the exact second state, timestamps, and expected stored rows.
- critic-item-5 [blocking-finding] Cross-ticket comparison is a stated goal, but the paired DVault ticket is not yet refined to the same scenario detail. Without a shared sequence/assertion contract, this ticket can still drift in the exact history it establishes.

Missing examples / edge cases
- No concrete example of the second customer-profile state exists.
- The ticket does not say whether the change is `customer_status`, `customer_name`, or both.
- The expected row count or row ordering for the persisted history is not defined.

Risky assumptions
- Assuming the developer will choose the same changed-state sequence that the later DVault ticket should follow.
- Assuming the single MVP example row is enough to infer a deterministic two-state comparison contract.

AC / test suggestions
- Make one acceptance criterion name the exact baseline sequence, including fixed values and timestamps for the initial and changed profile states.
- Make one acceptance criterion state what persisted outcome must be asserted, such as exact history rows or an exact documented row set kept beside the tests.

Implementation watchouts
- The ticket correctly scopes this work away from DVault APIs; direct source evidence exists for `IDataVaultSaveService` in `src/DCoding.Data.DVault/DataVaultSaveService.cs:10-21`, `ApplyDataVaultMetadata` in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:29-38`, and `AddDVault` in `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-24`.
- The existing execution surface is real: `README.md:7-23` and `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:1-28` confirm the repository test harness and SQLite test dependency already exist.

Non-blocking notes
- The ticket has no unresolved `## Open Questions` in `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/description.md:48-49`.
- Comment history under `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/comments/` is all bot orchestration and refinement material; there is no later human clarification that fills the scenario gap.

Split recommendations
- Keep the runnable-example question separate, as already suggested in `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/description.md:59-60`; first lock the exact comparison scenario contract.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment