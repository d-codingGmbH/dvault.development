[gicket-bot] PO refinement contract

Summary
- Verified local .gicket ticket/comment/relation state and current repository docs/source; refined this ticket to a bounded v0.15.0 documentation pass that aligns README and release records with the already-shipped bridge maintenance, PIT maintenance, current/as-of convenience reads, and SQLite PIT/bridge read optimization surface.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current ticket comments contain only bot claim/lease entries and no human scope changes, and no ticket attachments are present in the repository-local ticket store.
- The repository already contains docs/releases/v0.15.0.md, so this ticket should revise that existing release record in place rather than create a new release-note file.
- Current source and tests confirm the shipped v0.15 surface spans four delivered deltas: explicit bridge maintenance, explicit PIT maintenance, current/as-of satellite convenience overloads, and SQLite provider-aware PIT/bridge read optimization with provider-neutral fallback.
- Current README and adopter guidance are only partially aligned: current/as-of convenience reads and SQLite read dispatch are already documented, but the README v0.15 summary/limitations and docs/production-adoption-checklist.md still describe PIT rows as caller-populated and frame v0.15.0 as bridge-only.
- No child tickets, relation writes, attachments, or planning documents were materialized in this refinement pass because the repository already contains the needed feature split and planning context.

Scope In
- Update README.md so the read-model guidance clearly states that PIT-backed reads consume explicitly maintained PIT tables through IDataVaultPitMaintenanceService, bridge reads consume explicitly maintained bridge tables through IDataVaultBridgeMaintenanceService, and AddDVaultSqlite() is the only repository-proven optimized PIT/bridge read path with provider-neutral fallback elsewhere.
- Revise the README.md v0.15.0 summary and limitation sections so they reflect the full shipped release surface instead of only bridge maintenance.
- Rewrite docs/releases/v0.15.0.md as the coordinated release record for bridge maintenance, PIT maintenance, current/as-of convenience overloads, and SQLite PIT/bridge read optimization, while preserving explicit-service boundaries and provider-evidence limits.
- Update adopter-facing supporting docs that still carry stale release posture or stale PIT guidance, including docs/production-adoption-checklist.md and any current-baseline user doc that still points at v0.14.0 as the active release baseline.
- Keep release-note validation evidence tied to committed source and tests that already prove the shipped surface.

Scope Out
- Any product-code, API, diagnostics, benchmark, or test behavior changes beyond documentation-only edits.
- New PIT or bridge maintenance features, registry-backed PIT maintenance APIs, provider-specific PIT/bridge optimization beyond the existing SQLite path, or changes to current/as-of query semantics.
- Relation cleanup, child-ticket creation, or planning-document materialization unless a later refinement pass finds a new bounded planning gap; none is justified by the current local evidence.
- Historical release-note rewrites beyond small cross-links needed to make v0.15.0 the clear current baseline.

Open questions
- none

Follow-up questions
- After v0.15.0 adopter docs land, should a later docs-only cleanup sweep update deeper architecture notes that still label v0.14.0 as the current baseline even when user-facing guidance has moved to v0.15.0?
- Should a later release add a runnable PIT maintenance example or quickstart, since the current repository evidence is source/tests plus README guidance rather than a dedicated sample?

Risks
- If the docs pass only tweaks the release notes and misses README/adoption baseline text, consumers will still read conflicting guidance about whether PIT rows are caller-populated or explicitly maintained.
- If the release notes over-claim provider-aware read optimization beyond SQLite, the public record will outrun the repository's benchmark and test evidence.
- If the docs blur PIT maintenance with PIT reads or bridge maintenance with bridge reads, callers may infer implicit refresh behavior that the shipped services intentionally do not provide.

Split recommendations
- No new split is recommended. The repository already has the durable feature split across bridge maintenance, PIT maintenance, current/as-of convenience reads, and provider-aware read optimization; this ticket should stay a documentation-only consolidation pass over those completed slices.
- If the team later wants broader architecture-doc refresh or new runnable examples, track that work in separate follow-up tickets rather than widening this v0.15.0 release-note and adopter-guidance pass.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment