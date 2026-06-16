[gicket-bot] PO refinement contract

Summary
- Repository evidence resolves the ticket to the already-landed DB2 clean-context baseline: no unrepresented accepted DB2 bulk implementation is visible, and no ticket or planning writes were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- What is the authoritative outcome of spike 06FBSC9WY4T9T6YWDHFCEMZ0VG, and does this ticket still require any work now that the repository already contains the DB2 clean-context save baseline, smoke coverage, and benchmark guidance rows?: `answered` - The spike record itself was not readable through the trust-blocked ticket surfaces, but the later landed repository and release baseline is sufficient to refine this ticket now: the accepted DB2 improvement visible on branch is already implemented as the clean-context AddDVaultDb2() / Db2DataVaultSaveStrategy save path with diagnostics-gated PIT/bridge reads, provider-neutral latest-satellite fallback, smoke coverage, and skipped-placeholder benchmark guidance. On current evidence, this ticket does not justify new product-code work and should be treated as no-work-required or superseded unless it is explicitly re-scoped to a separate DB2 evidence-only follow-up.
- If the ticket is still active, is the remaining scope only opt-in DB2 evidence/documentation for the existing clean-context boundary, or is there a different accepted improvement that is not represented in the visible repository baseline?: `answered` - There is no repository evidence of a different accepted DB2 improvement beyond the already-landed clean-context boundary. If any work remains at all, it is limited to opt-in DB2 evidence/documentation for the existing clean-context save and PIT/bridge candidate posture, and even that backlog belongs on a narrowly named evidence ticket rather than this broad implementation ticket. This ticket must not reopen staged DB2 bulk, DB2 latest-satellite optimization, provider-native chunk execution, or other capability expansion.

Clarifications
- No bounded ticket or planning writes were applied in this run; no child tickets, relation changes, description updates, attachments, or planning documents were materialized.
- The branch already carries the DB2 implementation and documentation baseline introduced by the v0.34.0 release posture and carried forward by the v0.39.0 provider-evidence documentation baseline.
- For current refinement purposes, the landed repository/release baseline supersedes the inaccessible spike text as the authoritative scope boundary: clean-context DB2 save support is in, while staged bulk, latest-satellite optimization, provider-native chunk execution, and live-schema reading remain out.
- The current ticket title overstates scope. The safe contract is closure as no-work-required or superseded, or explicit re-scoping to a separate DB2 evidence-only follow-up if provider-configured benchmark evidence is still desired.

Scope In
- Record that the visible DB2 baseline is the already-landed clean-context AddDVaultDb2() save path plus diagnostics-gated PIT/bridge read candidates and provider-neutral latest-satellite fallback.
- Allow only closure/no-work-required handling for this implementation ticket, or a deliberate re-scope to narrow opt-in DB2 evidence/documentation work.
- Preserve the existing DB2 benchmark-guidance identity and documentation boundary without widening capability claims.

Scope Out
- Any staged DB2 bulk path.
- DB2 latest-satellite optimized read strategy.
- Provider-native chunk execution or alternate async chunk semantics for DB2.
- DB2 live-schema reading, PIT/bridge maintenance automation, or other provider capability expansion.
- New DB2 timing claims without a configured DB2 benchmark artifact triplet.

Open questions
- none

Follow-up questions
- When ticket relation/history tooling is available again, should this ticket be closed directly as superseded/no-work-required and linked to the originating spike or landed DB2 baseline for audit traceability?
- If the team still wants provider-configured DB2 timing evidence, should that work live in a separate narrowly named evidence ticket rather than under this implementation ticket?

Risks
- The gicket ticket/comment/relation/attachment surfaces were trust-blocked in this run, so hidden historical metadata could still exist even though the landed repository baseline is sufficient to bound current scope.
- Leaving the current title/status unchanged risks future automation or humans reopening staged-bulk scope that the repository and release notes explicitly exclude.
- The checked-in benchmark triplet still keeps DB2 rows as skipped placeholders because DVAULT_TEST_DB2_CONNECTION_STRING was unset, so no completed DB2 timing claim exists today.

Split recommendations
- Do not split this implementation ticket further.
- If DB2 follow-up work is still wanted, create a separate narrow ticket for provider-configured DB2 evidence/documentation only; do not keep it under the broad Implement accepted DB2 bulk improvement title.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment