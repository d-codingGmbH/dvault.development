[gicket-bot] PO refinement contract

Summary
- Re-routed the ticket from a closure-only posture to a normal pre-development implementation handoff for PIT maintenance comparator-row normalization and regression coverage. Local repository inspection confirmed existing PIT maintenance provider code/tests but no landed `pit-full-rebuild-maintenance` rows in `benchmark-summary.*` and no benchmark-scenario coverage for that row family. No child tickets, relation changes, attachments, description writes, or planning documents were materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This ticket is re-routed as normal pre-development implementation work, not a closure-only evidence ticket. The contract now stays focused on benchmark generation/normalization and regression coverage for provider-neutral PIT full-rebuild maintenance comparator rows.
- critic-item-2: `answered` - The ticket remains open because the repository does not yet show landed PIT maintenance comparator-row benchmark outputs or benchmark-output regression coverage. Existing PostgreSQL and SQL Server PIT maintenance implementations/tests are baseline inputs, but this ticket still needs the benchmark row normalization and verification work its own contract describes.

Clarifications
- This is a developer handoff ticket for benchmark-output implementation and verification, not a closure-only ratification of already-landed evidence.
- The bounded repository baseline is already fixed: PIT maintenance evidence uses scenario `pit-full-rebuild-maintenance`, not `pit-as-of-read` or `bridge-traversal-read`.
- The comparator row must preserve provider-neutral fallback posture with `selectedStrategy=<none>`, deterministic execution detail, and bounded fallback-cause tokens when a provider-specific maintenance path is not selected.
- Existing PostgreSQL and SQL Server PIT maintenance provider implementations are baseline inputs; this ticket adds benchmark comparator-row normalization and artifact/test coverage on top of that baseline.
- No child tickets, relation changes, description writes, attachments, or planning documents were materialized in this pass.

Scope In
- Normalize provider-neutral comparator rows for `pit-full-rebuild-maintenance` in PostgreSQL and SQL Server benchmark outputs.
- Keep markdown, CSV, and JSON PIT maintenance rows structurally aligned with the shared benchmark artifact contract.
- Preserve provider-neutral fallback posture, bounded fallback-cause vocabularies, run-context fields, and persisted-outcome semantics needed for later evidence citations.
- Add regression or contract coverage that locks PIT maintenance comparator-row identity and detail tokens across artifact formats.

Scope Out
- New PostgreSQL or SQL Server PIT maintenance strategies or wider maintenance-shape support.
- MySQL, Oracle, or DB2 PIT maintenance comparator lanes.
- Promoting `pit-as-of-read` or `bridge-traversal-read` rows into PIT maintenance evidence.
- Bridge maintenance, maintain-parents expansion, or unrelated read-optimization work.
- Introducing a new benchmark schema or ad hoc provider-specific comparator prose outside the shared contract.

Open questions
- none

Follow-up questions
- After PostgreSQL and SQL Server comparator rows are normalized, should later MySQL, Oracle, or DB2 PIT maintenance tickets adopt the same comparator-row contract from the start?
- Once provider-configured PIT maintenance artifact triplets land, should downstream docs add one representative comparator-row example to the evidence matrix or release notes?

Risks
- If comparator rows drift into provider-specific prose or inconsistent tokens, downstream evidence consumers will need brittle special-case parsing.
- If PIT read or bridge read rows are cited as maintenance evidence, the repository will violate its documented maintenance-evidence boundary.
- PostgreSQL and SQL Server use different maintenance seams, so normalization must preserve a shared artifact contract without hiding bounded fallback-cause meaning.

Split recommendations
- No split recommended; the repository baseline supports one bounded implementation ticket for PostgreSQL and SQL Server PIT maintenance comparator-row normalization and coverage.

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