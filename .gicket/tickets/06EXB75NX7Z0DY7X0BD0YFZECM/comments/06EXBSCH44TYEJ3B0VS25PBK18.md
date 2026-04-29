[gicket-bot] PO-critic review contract

Summary
- Persisted ticket contract is detailed, scoped, and ready for developer handoff; open questions are explicitly resolved as none.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted delivery contract PO Handoff states decision: ready_for_po_critic.
- Persisted Open Questions section contains only '- none', so there are no unresolved blocking questions under the contract rule.
- Acceptance Criteria explicitly cover documented examples for HubCustomer, LinkCustomerOrder, SatCustomerContact, {Base}HashKey, HashDiff, LoadTimestamp, RecordSource, singular/plural behavior, reserved words, collisions, duplicate names, and repeat-call determinism.
- Scope Out explicitly excludes override hook implementation and provider-specific identifier styles, with sibling ticket 06EXB75XTWD7FTRAFE5GNDCS5R assigned to public override points.
- Implementation Notes explicitly require coordination with sibling ticket 06EXB755X9TGQW2EG1G30GJG28 for technical metadata column contracts.
- Direct branch evidence: git status --short --branch returned '## HEAD (no branch)' and git rev-parse HEAD returned c2f5ff20b03d3a60ee94a4c8106f208d93f5fa72.
- Prompt seed repository snapshot lists only .gicket project metadata and no src-roots or test-roots, consistent with the contract statement that current refinement should not assume production file paths.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The naming policy depends on the sibling technical metadata contract remaining aligned; the ticket already calls this out as a risk and implementation note rather than leaving it ambiguous.

AC / test suggestions
- Keep the current edge-case test list in the contract: whitespace and punctuation normalization, snake/kebab/Pascal input, Customer versus Customers, reserved property names such as Order, collisions with technical columns, duplicate normalized names, and repeat calls returning identical names.

Implementation watchouts
- Developers should avoid treating this as the public override API task; that scope belongs to 06EXB75XTWD7FTRAFE5GNDCS5R.
- Developers should verify any public or protected API they introduce is documented, because the contract only requires API documentation if implementation work creates such API.
- Coordinate canonical technical column naming with 06EXB755X9TGQW2EG1G30GJG28 before finalizing examples or tests.

Non-blocking notes
- No split is recommended; override points and technical metadata contracts are already separated into sibling tasks.
- The repository currently has no visible source or test roots in the provided branch snapshot, so a new test layout may be needed during development.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment