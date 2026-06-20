[gicket-bot] PO refinement contract

Summary
- Refined for PO critic: DB2 is already recognized as `IBM.EntityFrameworkCore`, the branch still has no implementation beyond the explicit unsupported-reader dispatch, opt-in DB2 smoke/config scaffolding already exists, and active docs still describe DB2 live-schema as unsupported.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- `IBM.EntityFrameworkCore` is already the repository's DB2 provider identity; the missing piece is the live-schema reader dispatch, which still points DB2 at `UnsupportedDataVaultLiveSchemaReader` in `DataVaultLiveSchemaReader`.
- The branch head still matches scratch-source ref `d246f7d84511c1f66ea7185f9c30f9896cdc6f71`, so no DB2 live-schema implementation has landed on this ticket branch yet.
- Existing DB2 opt-in test scaffolding is already present through `Db2IntegrationTestConfiguration`, `Db2ProviderReflection`, and `Db2DataVaultSmokeTests`, so this ticket can extend the current external-provider test lane instead of inventing a new environment contract.
- Related ticket `06FE4QR3DD7EFZ4F35SBTFGWSR` remains a `relates` link for DB2 save/read evidence tuning; no relation cleanup or child-ticket split was needed for this ticket.
- No child tickets, relation edits, description updates, attachments, or planning documents were materialized during this refinement run.

Scope In
- Add a built-in DB2 live-schema reader path for provider name `IBM.EntityFrameworkCore` within the existing live-schema dispatch and catalog-reader architecture.
- Read the bounded DVault-owned schema facts already used by idempotency preflight: ordered columns, primary-key names/columns, and secondary-index metadata for hubs, links, satellites, PITs, and bridges when those tables exist.
- Return classified DB2 outcomes for success and unavailable catalog access using caller-owned connections only, with deterministic and redacted result messages suitable for `DataVaultPreflightRequest.IdempotencyLiveSchemaReadResult`.
- Add unit and opt-in external-provider coverage that proves DB2 snapshot success when configured and explicit non-success outcomes when configuration, connectivity, or catalog access is not safe.
- Update current public and active planning/adoption docs that currently state DB2 live-schema is unsupported so they describe DB2 as external opt-in evidence instead.

Scope Out
- DB2 save-strategy tuning, latest-satellite/PIT/bridge read-strategy tuning, benchmark timing promotion, staged bulk execution, or provider-native chunk execution.
- Automatic migrations, automatic schema repair, DB2 DDL generation, or any default live-database CI gate.
- Changing the supported live-schema fact surface beyond the existing idempotency-preflight structures DVault already compares.
- Rewriting historical release notes that are documenting earlier shipped baselines rather than current guidance.

Open questions
- none

Follow-up questions
- After this lands, should the next release-note baseline explicitly call out DB2 live-schema support as newly available external opt-in evidence so the evidence matrix and adoption docs stay synchronized?
- If DB2 requires stricter message redaction than the other existing readers, do we want a later consistency ticket to normalize unavailable-message redaction across every live-schema reader?

Risks
- Default repository validation does not provision DB2, so the live-reader success path will remain proven only through the opt-in external-provider lane behind `DVAULT_TEST_DB2_CONNECTION_STRING`.
- DB2 unsupported wording currently appears in several active documentation surfaces; partial doc updates would leave contradictory adoption guidance behind.
- The existing generic live-schema unavailable path accepts provider-specific messages, so the DB2 implementation must deliberately avoid echoing raw provider error text or host details.

Split recommendations
- No split recommended from current evidence; the work stays bounded to one DB2 reader implementation, matching test coverage, and current-guidance updates.

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