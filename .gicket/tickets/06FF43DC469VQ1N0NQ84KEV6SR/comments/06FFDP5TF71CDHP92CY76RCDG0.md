[gicket-bot] PO refinement contract

Summary
- Refined this into a bounded evidence-first Oracle PIT maintenance investigation anchored to the existing v1 asymmetry: PostgreSQL and SQL Server have provider-native PIT full-rebuild paths, while Oracle currently exposes save/read optimizations but no Oracle PIT maintenance implementation.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence shows `AddDVaultOracle()` registers Oracle provider capability, save, and read surfaces, but `src/DCoding.Data.DVault.Oracle` contains no Oracle PIT maintenance strategy and no Oracle-specific `IDataVaultPitMaintenanceService`.
- The investigation stays inside the explicit PIT maintenance boundary from `docs/architecture/dvault-v1-pit-bridge-boundary.md`: caller-invoked rebuild only, with no read-time refresh, `SaveChanges` interception, startup automation, or background scheduling.
- Oracle feasibility should be judged against the existing provider-native rebuild guardrails already used in code: clean-context gating, provider-name and shape-evidence checks, provider-neutral fallback on guard failure, and rollback-clean failure behavior for full rebuilds.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement.

Scope In
- Inspect current Oracle startup and provider code to confirm what Oracle-specific PIT maintenance surface exists today and what is missing.
- Compare any Oracle full-rebuild candidate against the current PostgreSQL and SQL Server PIT maintenance baselines for supported shapes, fallback rules, and transaction safety.
- Record supported and unsupported PIT shapes, SQL shape constraints, transaction caveats, and an explicit implement-or-defer recommendation for Oracle full-rebuild push-down.

Scope Out
- Implementing an Oracle PIT maintenance strategy or service in this ticket.
- Oracle `MaintainParentsAsync(...)`, bridge maintenance push-down, or automatic PIT maintenance orchestration.
- Changing Oracle latest-satellite, PIT-read, or bridge-read strategies except where read-path evidence is referenced as comparison context.

Open questions
- none

Follow-up questions
- If the investigation recommends defer, should the downstream blocked work stay blocked on new provider evidence or be rescheduled behind a separate future-facing Oracle optimization ticket?
- If the investigation recommends implementation, does Oracle fit the existing strategy-selection seam cleanly or does it require SQL Server-style service ownership because of transaction semantics?

Risks
- Oracle may not offer rollback-clean full-rebuild behavior through the same EF Core transaction/savepoint surfaces relied on by the current SQL Server safeguard, which raises partial PIT refresh risk.
- The PostgreSQL rebuild path depends on SQL patterns such as `WITH`, `UNION`, and lateral snapshot selection; Oracle may require materially different SQL that expands the proof surface.
- Existing Oracle PIT read evidence can be misread as maintenance evidence, creating scope pressure to ship a provider push-down path without equivalent rebuild-specific proof.

Split recommendations
- No split is needed during refinement; only create a follow-up implementation ticket if the evaluation produces a clearly bounded Oracle full-rebuild candidate.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment