[gicket-bot] PO refinement contract

Summary
- Refined the ticket to one bounded DB2 external-opt-in integration story: add DB2 save and read smoke coverage around the existing provider-neutral baseline, keep optimized-strategy and live-schema-reader work out of scope, and materialize no child tickets, relation updates, description updates, attachments, or planning documents.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- No bounded ticket writes were materialized in this refinement run: no child tickets, relation updates, description updates, attachments, or planning documents.
- The target branch head matches the supplied scratch source ref `a2317f2f84b07998327e06ba0b0846b8c334dabf`, so there is no partial implementation on the branch to ratify.
- `src/DCoding.Data.DVault.Db2` currently exposes `AddDVaultDb2()` plus DB2 provider-behavior/capability-profile wiring only; unlike PostgreSQL, SQL Server, MySQL, Oracle, and SQLite provider packages, it does not register a DB2-specific save strategy or PIT/bridge/latest read strategy.
- The current read-optimization baseline keeps optimized latest-satellite reads SQLite-only and optimized PIT/bridge reads limited to PostgreSQL, SQL Server, MySQL, and Oracle, so DB2 read coverage for this story should prove provider-neutral execution and diagnostics fallback rather than invent a DB2 optimized path.
- Unit coverage already marks `IBM.EntityFrameworkCore` live-schema reading as explicitly unsupported until a reader exists, so DB2 live-schema drift and reader parity are not part of this ticket.

Scope In
- DB2 external opt-in integration test scaffolding in the integration test project, including conditional DB2 provider package wiring and connection-string-gated execution consistent with existing external providers.
- Smoke coverage that `AddDVaultDb2()` plus the real IBM EF Core provider can persist explicit hub, link, and satellite saves against a live DB2 database.
- DB2 current/latest and as-of latest-satellite reads plus PIT as-of and bridge traversal integration coverage on maintained test data, using the existing provider-neutral read boundary where no DB2 optimized strategy is registered.
- Diagnostic assertions or equivalent observable evidence that DB2 save and read execution remains on the documented provider-neutral fallback path when no DB2-specific strategy is available.

Scope Out
- New DB2 provider-specific optimized save, latest-satellite, PIT, or bridge read strategies.
- DB2 live-schema reader or drift-reporting support.
- Making DB2 part of the default local validation lane or provisioning DB2 or Podman infrastructure inside the repository.
- DB2 benchmark or performance-claim rows, or release-posture expansion beyond the tested smoke and integration boundary.

Open questions
- none

Follow-up questions
- After DB2 integration coverage lands, should README and release-validation docs be aligned with the DB2 package line and test story, since README installation guidance includes `DCoding.Data.DVault.Db2` but the current `v0.33.0` release note omits DB2 from the documented package family and external-provider matrix?
- Should DB2 later get a dedicated live-schema reader or optimized save and read strategies once there is stable evidence that provider-neutral coverage is insufficient?

Risks
- `IBM.EntityFrameworkCore` DDL, type-mapping, or transaction behavior may require DB2-specific fixture handling even though the runtime save and read path stays provider-neutral.
- The integration project must maintain conditional IBM provider package wiring for both `net8.0` and `net10.0`; missing one target would create a parity gap.
- Because DB2 coverage stays opt-in and externally provisioned, unattended default-local validation will only prove discovery and skip behavior unless a DB2 instance is explicitly supplied.

Split recommendations
- No split recommended; the visible branch state supports one bounded story covering DB2 opt-in test scaffolding plus representative save and read integration coverage.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment