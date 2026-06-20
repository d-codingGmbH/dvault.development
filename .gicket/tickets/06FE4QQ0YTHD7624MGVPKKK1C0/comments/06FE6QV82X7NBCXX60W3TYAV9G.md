[gicket-bot] PO refinement contract

Summary
- Refined the SQL Server latest-satellite tuning ticket around the existing SqlServerDataVaultReadStrategy evidence gap. Repository and related-ticket evidence already fix the supported shape, placeholder row identity, and downstream documentation split, so no new child ticket, relation change, description update, attachment, or planning document was materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The root benchmark triplet already contains the SQL Server latest-satellite guidance row `dvault-adddvaultsqlserver-optimized`, but it is still a skipped placeholder with `selectedStrategy=SqlServerDataVaultReadStrategy`, `plannedReadStrategy=SqlServerDataVaultReadStrategy`, `readShape=LatestSatellite`, and `persistedOutcome=not executed` because `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset in the checked-in baseline.
- SQL Server latest-satellite optimization is already bounded in repository code and tests to hub-parent satellites with no driving-key-based multi-active shape; unsupported parent kinds, multi-active satellites, provider mismatch, or declined diagnostics must stay on the provider-neutral fallback path.
- SQL Server PIT and bridge timing are already closed elsewhere through the provider-configured v0.32.0 smoke-read bundle, so this ticket is only about latest-satellite tuning evidence and must not reopen PIT/bridge scope.
- The shared lane-normalization prerequisite is already done in ticket 06FE4QP6FB892E7TJMB47A3MSR, and the broader documentation/release update after tuning already exists as ticket 06FE4QRMXVGJVA65ZR5MZ817K8.

Scope In
- Tune or explicitly retain the SQL Server current/as-of latest-satellite read path behind AddDVaultSqlServer for supported hub-parent, non-multi-active satellite requests.
- Capture provider-configured SQL Server latest-satellite benchmark evidence or equivalent measured validation against the existing provider-neutral fallback, using the current `dvault-adddvaultsqlserver-optimized` row identity.
- Keep diagnostics and benchmark artifacts clear about when `SqlServerDataVaultReadStrategy` is selected, what read shape it covers, and when fallback is used instead.
- Preserve correctness parity between optimized SQL Server latest/as-of reads and the provider-neutral read pipeline.

Scope Out
- Changing PIT or bridge read boundaries, evidence posture, or completed timing claims for SQL Server.
- Widening latest-satellite support to link-parent or multi-active satellite shapes.
- Promoting measured SQL Server latest-satellite timing from the skipped root placeholder alone without a provider-configured run context.
- The coordinated post-tuning documentation and release sweep already owned by ticket 06FE4QRMXVGJVA65ZR5MZ817K8.

Open questions
- none

Follow-up questions
- After provider-configured SQL Server latest-satellite evidence is collected, should ticket 06FE4QRMXVGJVA65ZR5MZ817K8 promote the result in the coordinated v0.42 documentation sweep even if the decision is to retain the current SQL shape?

Risks
- If `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` stays unset in local or CI evidence lanes, the repository will still only have the skipped placeholder row and developers may overstate strategy-registration evidence as measured timing.
- SQL Server latest-satellite tuning can regress correctness or performance differently for current versus as-of reads, or for large parent-hash batches near the parameter ceiling, unless evidence covers both shapes.
- Because SQL Server PIT and bridge already have completed timing evidence, later documentation or benchmark summaries could accidentally blend that proof into this ticket's latest-satellite claim boundary.
- If the benchmark row tokens, diagnostics tokens, or fallback causes drift from tests and matrices, the downstream documentation ticket will inherit inconsistent evidence.

Split recommendations
- No additional split is recommended. Shared lane normalization is already done in 06FE4QP6FB892E7TJMB47A3MSR, this ticket carries the SQL Server latest-satellite evidence/tuning work, and 06FE4QRMXVGJVA65ZR5MZ817K8 remains the coordinated documentation follow-up.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment