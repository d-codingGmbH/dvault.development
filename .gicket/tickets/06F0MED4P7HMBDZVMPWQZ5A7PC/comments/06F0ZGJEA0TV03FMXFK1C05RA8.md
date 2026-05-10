[gicket-bot] PO-critic review contract

Summary
- Contract is now specific enough for developer handoff: the prior PO-critic gaps were answered with request-bound strategy evaluation, explicit not-evaluated behavior, and source-backed fallback thresholds.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MED4P7HMBDZVMPWQZ5A7PC/description.md records PO handoff ready_for_po_critic and Open Questions: none.
- Comment .gicket/tickets/06F0MED4P7HMBDZVMPWQZ5A7PC/comments/06F0Z93EXPHHN647Z01XZ82W3G.md returned the ticket to PO; comment .gicket/tickets/06F0MED4P7HMBDZVMPWQZ5A7PC/comments/06F0ZC1Q0NTHD92GADSY0R5VPM.md marks critic-item-1 through critic-item-5 answered.
- git diff 51554747f..87c955b39 -- .gicket/tickets/06F0MED4P7HMBDZVMPWQZ5A7PC/description.md adds request-bound evaluation against DbContext plus DataVaultSaveRequest/DataVaultBulkSaveRequest, the not evaluated path for request-free calls, and explicit dirty-context, multi-active, unknown-provider, SQL Server 50/500, and MySQL/Oracle 50 fallback categories.
- src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs defines CanSave(DbContext, IReadOnlyList<DataVaultSaveRequest>), and src/DCoding.Data.DVault/DataVaultSaveService.cs orders strategies by descending Priority before provider-neutral fallback.
- Provider gates and defaults are directly evidenced in src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs, src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs, src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs, src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs (unknown provider falls back to Sqlite), and src/DCoding.Data.DVault/DefaultDataVaultProviderBehaviorSelector.cs plus src/DCoding.Data.DVault/DataVaultProviderBehaviorProfiles.cs (fallback is provider-neutral-v1).
- .gicket/tickets/06F0MEAXT99V0P115P0WEJD4P0/ticket.json shows the upstream registry ticket is done, matching the contract's treatment of it as upstream context only.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Non-blocking: include at least one example or test showing the same provider on a clean DbContext and a dirty DbContext so strategy-result variance is explicit.
- Non-blocking: include one validation-only or explain-only call that returns strategy status not evaluated.
- Non-blocking: include one registry-backed convenience-overload case, if exposed, that resolves to the same explicit batch before evaluation.

Risky assumptions
- Assuming fallback-cause reporting can stay accurate if it duplicates provider CanSave logic instead of sharing extracted gates.
- Assuming provider optimization thresholds will remain stable without synchronized diagnostics-test updates.

AC / test suggestions
- Assert capability-profile selection, provider-behavior selection, and save-strategy evaluation as separate structured fields.
- Add paired tests for identical provider metadata with compatible versus incompatible request/context inputs.
- Assert unknown or unregistered provider handling reports an explicit risky fallback state, not just sqlite-v1.
- Cover built-in profiles sqlite-v1, postgres-v1, sqlserver-v1, oracle-v1, mysql-pomelo-v1, plus the WithLoadTimestampStorage variants named in the contract.

Implementation watchouts
- Keep strategy evaluation request-bound and read-only; do not infer actual dispatch from provider name alone.
- If a registry-backed convenience overload is added, resolve it to explicit DataVaultSaveRequest or DataVaultBulkSaveRequest before evaluation.
- Reuse translator, provider-capability selection, provider-behavior selection, and strategy-dispatch logic instead of parallel rule tables.

Non-blocking notes
- Relations remain bounded and consistent with the contract in .gicket/relations/T0/PC/06F0MECWYMPQ4R0KWV1R637RT0--06F0MED4P7HMBDZVMPWQZ5A7PC--parentOf.json and .gicket/relations/PC/P0/06F0MED4P7HMBDZVMPWQZ5A7PC--06F0MEDJC732GDD77H60R259P0--blocks.json.

Split recommendations
- No split recommended; the refined contract is now sufficiently bounded for implementation inside 06F0MED4P7HMBDZVMPWQZ5A7PC.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment