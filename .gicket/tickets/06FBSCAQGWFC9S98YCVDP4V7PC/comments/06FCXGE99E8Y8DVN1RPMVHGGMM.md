[gicket-bot] PO refinement contract

Summary
- Repository evidence shows the DB2 clean-context bulk/save baseline is already implemented and documented, but the authoritative spike outcome for ticket `06FBSC9WY4T9T6YWDHFCEMZ0VG` could not be verified, so this ticket still needs PO clarification before PO-critic review.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

Clarifications
- No bounded ticket or planning writes were applied; no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.
- The ticket branch is unchanged from its scratch-source commit `5bd9f18972f3e3c9d0f1dcafc249d08051849f7a`, so this ticket currently carries no implementation or planning delta on branch.
- Visible repository evidence already includes `AddDVaultDb2()`, `Db2DataVaultSaveStrategy`, DB2 smoke coverage, benchmark verifier rows, and the v0.34.0 DB2 release baseline.
- Ticket, spike, comment, relation, and attachment reads through the bounded gicket surfaces were trust-blocked, so refinement is based on the provided ticket snapshot plus repository evidence only.

Scope In
- Clarify whether this ticket still owns any remaining DB2 work beyond the already-landed clean-context `Db2DataVaultSaveStrategy` baseline.
- If the ticket remains open, keep scope bounded to the existing DB2 clean-context hub/link/ordinary-satellite save path, its diagnostics/fallback coverage, and opt-in benchmark evidence/documentation alignment.
- Preserve the checked-in benchmark and verifier contract for the DB2 `provider-native-bulk-ingestion` guidance row.

Scope Out
- Any staged DB2 bulk path.
- Provider-native chunk execution or alternate async chunk semantics.
- A DB2 latest-satellite optimized read strategy.
- DB2 live-schema reading or PIT/bridge maintenance implementation.
- New timing claims without a configured DB2 benchmark run.

Open questions
- What is the authoritative outcome of spike `06FBSC9WY4T9T6YWDHFCEMZ0VG`, and does this ticket still require any work now that the repository already contains the DB2 clean-context save baseline, smoke coverage, and benchmark guidance rows?
- If the ticket is still active, is the remaining scope only opt-in DB2 evidence/documentation for the existing clean-context boundary, or is there a different accepted improvement that is not represented in the visible repository baseline?

Follow-up questions
- After the spike outcome is confirmed, should this ticket be closed as no-work-required or superseded, or retitled as a narrower DB2 evidence follow-up so it no longer suggests staged or expanded bulk scope?
- When trust-blocked ticket tooling is available again, should the current ticket be linked explicitly to the originating spike or landed DB2 baseline ticket for closure traceability?

Risks
- Proceeding without the spike decision risks duplicating already-landed DB2 provider work or reopening staged-bulk scope that current docs explicitly exclude.
- The checked-in benchmark triplet keeps DB2 rows as skipped placeholders because `DVAULT_TEST_DB2_CONNECTION_STRING` was unset, so no completed DB2 timing claim can support broader performance wording.
- The ticket, comment, relation, and attachment surfaces were trust-blocked in this run, so hidden ticket history could still supersede the repository-only interpretation.

Split recommendations
- Do not split until the spike outcome is confirmed.
- If work remains, prefer a narrower child task for DB2 benchmark/evidence/documentation alignment over a broad DB2 bulk-improvement implementation ticket, because the visible repository already carries the clean-context save implementation.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment