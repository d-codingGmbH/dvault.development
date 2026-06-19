[gicket-bot] PO refinement contract

Summary
- Refined this ticket into a bounded evidence-closure/docs-consistency task: the repository already contains completed SQL Server PIT and bridge artifact evidence plus existing strategy/fallback coverage, while current planning surfaces still describe those two rows as open gaps.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- `AddDVaultSqlServer()` already registers `SqlServerDataVaultReadStrategy` for provider read, PIT read, and bridge read services; this ticket is about closing evidence/documentation gaps for existing SQL Server PIT/bridge candidates, not inventing a new strategy.
- Approved repository evidence already exists in `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.{md,csv,json}`, where SQL Server `pit-as-of-read` and `bridge-traversal-read` rows are completed and select `SqlServerDataVaultReadStrategy`.
- The root quick benchmark triplet should remain a skipped-placeholder surface when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset; closing this ticket means promoting the existing checked-in artifact-bundle evidence in planning/docs, not requiring the root triplet itself to become provider-configured.
- Live relations remain consistent with the current refinement: incoming `blocks` from `06FBSCGBG8CJ0QNRX4JZJA638G` and outgoing `blocks` to `06FBSCHBJEYYERDPA7JN34Y8PG` stay unchanged.
- No child-ticket split or relation rewrite is justified from the current repository evidence.

Scope In
- Promote the completed SQL Server `pit-as-of-read` and `bridge-traversal-read` artifact rows into the authoritative planning/documentation surfaces that still call them open gaps.
- Align evidence and gap documentation so SQL Server PIT/bridge read claims cite the preserved v0.32 smoke-read artifact bundle and its run context.
- Preserve and restate the existing read boundary: explicit PIT/bridge maintenance is required, incomplete read-shape evidence falls back, stale maintenance falls back, unsupported shapes fall back, and no new public read API is introduced.

Scope Out
- SQL Server `latest-satellite-read` timing closure; that remains the separate `latest-satellite-read` evidence gap.
- New SQL Server PIT or bridge algorithm work, new provider strategy names, or alternative read-shape design.
- Changing skipped root quick-benchmark SQL Server rows into completed rows when provider connection strings are unset.
- PostgreSQL, MySQL, Oracle, or DB2 PIT/bridge closure work.
- New benchmark-runner features, external database provisioning, or credential/setup automation.

Open questions
- none

Follow-up questions
- Should a later ticket also promote SQL Server `latest-satellite-read` from root-placeholder guidance to completed external-provider evidence, or should that P0.02 gap remain separate until a dedicated benchmark lane is approved?
- After SQL Server PIT/bridge closure lands, should the broader documentation/parity ticket `06FBSCHBJEYYERDPA7JN34Y8PG` downgrade or remove any now-historical dependency on this ticket?

Risks
- If documentation updates only add the v0.32 smoke-read link without clearing `P2.02` and `P3.02`, the repository will continue to publish contradictory evidence posture for the same SQL Server rows.
- If reviewers insist that only the root quick triplet can close a read-evidence gap, this ticket will need an explicit policy decision because the existing artifact contract and v0.32 evidence bundle already preserve completed SQL Server PIT/bridge timing evidence.

Split recommendations
- No split recommended. SQL Server PIT and bridge closure share one provider, one existing artifact bundle, one strategy name, and one documentation-consistency problem; keep SQL Server latest-satellite evidence as its separate existing follow-up.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment