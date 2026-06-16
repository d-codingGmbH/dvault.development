[gicket-bot] PO refinement contract

Summary
- Repository evidence already bounds Oracle bulk work to the existing direct AddDVaultOracle batching path with staged Oracle bulk still unselected, so no split is justified and this ticket should be treated as closure-focused rather than new staged-bulk implementation.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The accepted Oracle bulk baseline is the current direct Oracle batching path behind AddDVaultOracle and OracleDataVaultSaveStrategy, not a staged Oracle bulk lane.
- Oracle bulk eligibility is already bounded to clean Oracle.EntityFrameworkCore contexts with at least 50 total operations, no multi-active satellites, and no more than 10000 satellite operations; batches outside that gate fall back to the provider-neutral writer.
- The root benchmark triplet may remain a skipped-placeholder for Oracle when DVAULT_TEST_ORACLE_CONNECTION_STRING is unset; the checked-in v0.32.0 Oracle threshold artifact is the current completed evidence for retaining the direct path and 10000-satellite cap.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement run.

Scope In
- Ratify the existing direct Oracle optimized bulk save boundary as the only implementation and closure path owned by this ticket.
- Keep Oracle strategy-selection diagnostics, fallback causes, and benchmark/verifier coverage aligned with that direct batching path.
- If any narrow code or artifact refresh is still performed, keep Oracle unit, integration, and smoke coverage proving strategy selection, rollback and fallback behavior, and the 10000-satellite threshold boundary.

Scope Out
- Selecting or implementing staged Oracle bulk without new benchmark evidence showing a measured win and deterministic cleanup under the caller-owned transaction boundary.
- Changing Oracle latest-satellite, PIT, or bridge read scope beyond the current registration and evidence baseline.
- Provider-native chunk execution, SQL artifact exporter expansion, or new provider-support work.
- Changing the 50-operation minimum or 10000-satellite safety cap without new artifact-backed evidence.

Open questions
- none

Follow-up questions
- When an Oracle environment is available, should a separate follow-up collect fresh provider-configured root-triplet timing artifacts for the retained direct Oracle path so the current skipped-placeholder row can be supplemented with completed timing evidence?
- If future experiments revisit staged Oracle bulk, should that happen only in a separate evidence-first ticket with explicit before-and-after artifacts and cleanup-parity proof?

Risks
- The root quick benchmark baseline still carries Oracle as a skipped-placeholder row when DVAULT_TEST_ORACLE_CONNECTION_STRING is unset, so downstream documentation can overstate Oracle timing evidence if it ignores evidence posture.
- Reopening staged Oracle bulk inside this ticket would conflict with the current source, docs, and artifact contract that keep Oracle on the retained direct batching path.
- Any stale live relation cleanup could not be re-verified in-session because gicket relation reads were trust-blocked.

Split recommendations
- No split is justified from current repository evidence; the remaining Oracle work is already bounded as an evidence-gap follow-up in the provider optimization gap matrix rather than a child implementation ticket from this task.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment