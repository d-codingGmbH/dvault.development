[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the repository-backed MySQL dual-lane save baseline: retained multi-row below the staged boundary, staged temporary-table bulk at 60-plus operations, and no current LOAD DATA lane; no child tickets, relation updates, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already implements two MySQL save lanes: MySqlDataVaultSaveStrategy for retained multi-row inserts and MySqlStagedDataVaultSaveStrategy for staged bulk using temporary tables.
- The visible MySQL gates are bounded: provider-native candidacy starts at 50 total operations, staged bulk starts at 60 total operations, and tiny satellite-only history batches deliberately stay provider-neutral at 10 or fewer operations in one request or 100 or fewer across multiple requests.
- The current staged implementation uses temporary tables plus parameterized inserts and INSERT IGNORE or INSERT ... SELECT flow; no LOAD DATA or LOAD DATA INFILE path is present in the visible MySQL provider code, docs, or artifacts.
- The root v0.39 evidence rows for MySQL provider-native-bulk-ingestion are skipped placeholders when DVAULT_TEST_MYSQL_CONNECTION_STRING is unset, but the repository also preserves completed v0.32 local MySQL evidence for the retained multi-row representative row at 57 operations and the staged representative row at 63 operations.
- No bounded child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Evaluate whether the existing MySQL retained multi-row lane, staged temporary-table lane, and documented thresholds already close the current save-strategy gap.
- Produce a repository-backed recommendation for this ticket: document no-op, defer with reason, or open a future implementation follow-up only if a distinct unsupported gap remains.
- Tie the recommendation to the v0.39 evidence matrix and gap-matrix posture plus the completed v0.32 MySQL evidence bundles.

Scope Out
- Changing MySQL provider code, thresholds, or save strategy selection logic in this ticket.
- Adding a new LOAD DATA or LOAD DATA INFILE ingestion lane.
- Claiming new MySQL timing evidence from the root v0.39 quick baseline when those rows are skipped.
- Changing latest-satellite, PIT, or bridge read behavior.

Open questions
- none

Follow-up questions
- If maintainers still want a LOAD DATA experiment after this evaluation, should it be opened as a separate implementation ticket with explicit operational constraints, artifact and evidence requirements, and benchmark reruns?
- If maintainers want to revisit the 50 and 60 operation MySQL thresholds, which representative mixed hub, link, and satellite workloads should be rerun to prove a changed boundary against both provider-neutral fallback and the retained multi-row lane?

Risks
- Because the root v0.39 quick baseline skips MySQL provider-native rows when the connection string is unset, future readers may misread the posture unless this ticket explicitly cites the completed v0.32 evidence bundles.
- The repository proves the current retained and staged MySQL boundaries, but any threshold retune or LOAD DATA proposal would need new provider-configured evidence rather than reinterpretation of the existing bundles.
- A future LOAD DATA lane would expand operational scope beyond the current temporary-table and save-service baseline, including permissions, file movement, cleanup, and deployment ownership concerns.

Split recommendations
- Do not split while the ticket outcome is evaluation only; close it as a documentation or no-op plus LOAD DATA deferral if the recommendation matches the current evidence.
- If the evaluation still calls for action beyond documentation, create one separate follow-up ticket for a MySQL LOAD DATA experiment or threshold-retune benchmark rerun rather than widening this ticket.

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