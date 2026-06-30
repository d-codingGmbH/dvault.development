[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded additive privacy-diagnostics/support-bundle slice: expose a finite static provider crypto capability matrix for the supported provider baseline while keeping the existing unmanaged guidance-only boundary unchanged; no ticket or planning mutations were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the shared v1 boundary to DataVaultPrivacyDiagnostics plus DataVaultProviderNativeEncryptionBoundaryFact; this ticket adds additive capability facts and does not reopen the unmanaged guidance-only boundary.
- The finite provider baseline is ratified from checked-in docs/code: SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2, with one reviewed MySQL capability set covering both MySql.EntityFrameworkCore and Pomelo.EntityFrameworkCore.MySql.
- Capability facts are static, redaction-safe, and profile-backed; they do not prove that a database instance is configured, enabled, reachable, or compatible at runtime.
- Architecture predecessor ticket 06FH8RGQZA7D9JZSTSAJEM9B3M is already done, so its shared-boundary blocks relation is historical completion context rather than a fresh PO blocker.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement run.

Scope In
- Add additive privacy diagnostics/support-bundle facts that enumerate reviewed provider-native crypto capability rows for the finite supported-provider baseline.
- Classify each reviewed capability row as supported, conditional, or unsupported with bounded reason/guidance text when the row is not unconditionally supported.
- Cover the provider-native capability families already named in checked-in docs, including SQL Server guidance (TDE and Always Encrypted), PostgreSQL guidance (deployment encryption posture and pgcrypto), Oracle guidance (TDE and DBMS_CRYPTO), MySQL guidance (SQL crypto plus file/tablespace encryption), SQLite encrypted-file guidance, and DB2 native encryption guidance.
- Emit capability facts through the existing DataVaultPrivacyDiagnostics/support-bundle lane with deterministic JSON and no live database probe by default.
- Add tests for deterministic per-provider fact selection, MySQL dual-provider-name mapping, unknown/unregistered-provider behavior, and redaction safety.

Scope Out
- New provider-native execution behavior, SQL generation, encrypted DDL, key-store integration, or automatic runtime dispatch.
- Consumer configuration or selection APIs for choosing provider-native crypto behavior; that remains in ticket 06FH8RKDJTS3BB11J6J6QJVVD4.
- Broad documentation rollout; that remains in ticket 06FH8RMZPSZ7H3AQRP8FX72S08.
- Live capability probing, connectivity checks, or verification that a concrete database instance has encryption enabled.
- Compliance, retention, deletion, backup shredding, or DVault-owned key lifecycle claims.

Open questions
- none

Follow-up questions
- Which provider-native capability should get the first provider-specific execution ticket after this fact-reporting slice lands: SQL Server Always Encrypted, PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, MySQL SQL crypto, SQLite encrypted-file integration, or DB2 native encryption?
- After the static fact lane is in place, do we want a separate opt-in probe ticket that can confirm environment-specific prerequisites, or should runtime behavior stay entirely guidance-only?
- Should the docs task 06FH8RMZPSZ7H3AQRP8FX72S08 publish the exact same reviewed capability matrix verbatim to minimize drift between diagnostics and documentation?

Risks
- Consumers may misread a supported capability row as DVault runtime support or environment activation unless the fact model and docs keep the unmanaged guidance-only boundary explicit.
- Generic diagnostics already default unknown providers to the SQLite storage profile for some explain paths; this ticket must avoid reusing that fallback for provider-native crypto facts.
- Mixing deployment-at-rest features and SQL-function features in one static matrix can create false equivalence unless the reported capability family makes the distinction explicit.
- Provider docs or package baselines can drift over time; without checked-in tests per provider row, the static matrix could become stale or contradictory.

Split recommendations
- Keep provider-native runtime activation or conversion behavior split by one provider and one exact capability per ticket after this discovery/reporting slice.
- Keep consumer-facing configuration and selection behavior in existing ticket 06FH8RKDJTS3BB11J6J6QJVVD4 rather than expanding this ticket.
- Keep docs rollout in existing ticket 06FH8RMZPSZ7H3AQRP8FX72S08 rather than widening this diagnostics ticket.
- If optional live probing is ever desired, split it into a later opt-in diagnostics ticket with its own redaction and secret-handling review.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment