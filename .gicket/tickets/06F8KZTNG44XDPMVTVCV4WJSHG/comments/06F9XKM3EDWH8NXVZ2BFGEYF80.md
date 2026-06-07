[gicket-bot] PO-critic review contract

Summary
- The parent contract content is mostly clear, but the promised docs/plans/README.md index entry is not durably reflected in branch history and the ticket narrative is internally inconsistent about what was materialized.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZTNG44XDPMVTVCV4WJSHG/description.md:33-57 contains the Delivery Contract, shows `## Open Questions` as `- none`, and requires docs/plans/provider-specific-sql-artifact-contract.md to be indexed from docs/plans/README.md.
- docs/plans/provider-specific-sql-artifact-contract.md:8-146 defines a design-time-only contract with supported providers SQLite/PostgreSQL/SQL Server/MySQL/Oracle, manifest schemaVersion `dvault.sql-artifact.v1`, review rules, consumer-owned responsibilities, an evidence gate, and explicit non-goals.
- docs/architecture/dvault-dotnet-ef-design-time-workflow.md:8-18 and :103-122 plus docs/performance-profiles.md:291-293 and docs/architecture/dvault-v1-explicit-save-service.md:56-58 match the stated boundary: single-project consumer-owned design-time workflow, no standalone DVault CLI, no default runtime dispatch, and consumer-owned deployment/invocation.
- Committed branch delta vs develop includes docs/plans/provider-specific-sql-artifact-contract.md and ticket metadata, but not docs/plans/README.md (`git diff --name-only develop...HEAD`).
- The current scratch worktree still has an unstaged docs/plans/README.md change adding `provider-specific-sql-artifact-contract.md` (`git status --short -- docs/plans/README.md` => ` M docs/plans/README.md`; `git diff -- docs/plans/README.md` shows the added line), and `git show HEAD:docs/plans/README.md` still lacks that entry.

Blocking findings
- The README index entry required by the Acceptance Criteria and Definition of Done is not yet durable in branch history. It exists only as an unstaged scratch-worktree diff, so a fresh checkout of HEAD still misses the docs/plans/README.md entry for provider-specific-sql-artifact-contract.md.

Required PO actions
- Persist the docs/plans/README.md index addition into the owner-branch committed state, or remove the claim that the contract is already indexed there.
- Reconcile the Delivery Contract and Implementation Notes with the actual write history so the ticket accurately states whether the description and planning index were updated.
- Re-run PO handoff after the repository state and the persisted ticket narrative agree.

Open issues ledger
- critic-item-1 [required-po-action] Persist the docs/plans/README.md index addition into the owner-branch committed state, or remove the claim that the contract is already indexed there.
- critic-item-2 [required-po-action] Reconcile the Delivery Contract and Implementation Notes with the actual write history so the ticket accurately states whether the description and planning index were updated.
- critic-item-3 [required-po-action] Re-run PO handoff after the repository state and the persisted ticket narrative agree.
- critic-item-4 [blocking-finding] The README index entry required by the Acceptance Criteria and Definition of Done is not yet durable in branch history. It exists only as an unstaged scratch-worktree diff, so a fresh checkout of HEAD still misses the docs/plans/README.md entry for provider-specific-sql-artifact-contract.md.

Missing examples / edge cases
- A future child artifact example should show whether a valid dry-run manifest may contain zero sidecar SQL files versus manifest-relative sidecar payloads.
- The deferred consumer repository path convention for manifests and sidecars remains acceptable here, but child ticket 06F8KZV18BQ0GN3CE4G02ATVA0 will need one deterministic example.

Risky assumptions
- Assuming a scratch-worktree-only README edit is enough to satisfy the ticket's durable planning-surface claim.
- Assuming reviewers will ignore the contradictory note that no ticket description updates were materialized.

AC / test suggestions
- Before marking ready_for_po_critic, add a check that every file named in Acceptance Criteria or Definition of Done is present in committed owner-branch state, not only in the scratch worktree.
- Add a ticket-level sanity check that the final Delivery Contract prose matches the actual ticket/comment/relation mutations reported by the run.

Implementation watchouts
- Keep child implementation work inside the existing design-time command boundary and explicit non-goals; do not let the dry-run prototype widen into runtime dispatch or automatic migration synchronization.
- Because .gicket/relations/HG/A0/06F8KZTNG44XDPMVTVCV4WJSHG--06F8KZV18BQ0GN3CE4G02ATVA0--blocks.json still makes this parent block the dry-run prototype ticket, the parent's durability mismatch should be resolved before developer handoff.

Non-blocking notes
- The contract content itself is otherwise well-bounded: supported provider scope, manifest determinism, evidence gate, consumer-owned operations, and explicit non-goals are all directly documented.

Split recommendations
- No new split is needed; the current parent and child tickets still separate architecture contract, evidence rules, prototype, and rollout documentation.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment