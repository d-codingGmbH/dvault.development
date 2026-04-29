[gicket-bot] PO-critic review contract

Summary
- The delivery contract is bounded, documentation-only, and consistent with the visible branch state; it can proceed to developer handoff with minor non-blocking path-label watchouts.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The delivery contract Scope In is limited to creating or updating a concise MVP architecture/concept document for Data Vault 2.x-oriented persistence.
- The contract explicitly covers the named concepts hub, link, satellite, hash key, hash diff, load timestamp, and record source.
- The contract Scope Out excludes persistence code, schema generators, migrations, runtime automation, SQLite tests, enterprise Data Vault standards, and final public API naming.
- The Open Questions section states: none.
- Repository branch state snapshot lists src-roots: [] and test-roots: [], matching the contract statement that no repository source or test roots exist yet.
- Referenced project metadata includes .gicket/project.json with project name DVault and .gicket/milestones/06EXB6F6Z8HMH2BQKDY1ZKQCPC.json for Foundation and architecture.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Non-blocking: the contract asks for SQLite-compatible table-shape or row examples, but leaves exact example count and format to the developer.

Risky assumptions
- The phrase repository documentation area is not backed by a visible docs path in the branch snapshot, so the developer will need to choose an appropriate documentation/planning path without further PO direction.
- The ticket currently has automation/bot-ready but not needs-dev, while the provided dev role policy lists needs-dev as required for dev readiness.

AC / test suggestions
- Verify the delivered document covers each named concept at least once and explicitly relates hubs, links, and satellites.
- Check examples use SQLite-portable table or row shapes and avoid unqualified vendor-specific SQL features.
- Check that hash keys and hash diffs are described as planned persistence conventions without locking in algorithm or normalization details.

Implementation watchouts
- Keep the work documentation-only and avoid implying schema generation, loading automation, hash computation, migrations, or validation tooling already exist.
- Do not reference concrete public APIs, types, method names, or source/test paths because the branch snapshot shows no source or test roots.
- Mark future automation and broader dialect guidance as future work rather than MVP deliverables.

Non-blocking notes
- The contract is appropriate for a low-assurance documentation task in the Foundation and architecture milestone context.
- No ticket comments are present that alter or contradict the persisted delivery contract.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment