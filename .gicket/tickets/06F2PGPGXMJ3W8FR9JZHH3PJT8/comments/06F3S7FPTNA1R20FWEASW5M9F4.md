[gicket-bot] PO-critic review contract

Summary
- The ticket is well-grounded in current repository evidence, but hierarchy bridge depth semantics are still underspecified for a schema that permits only one row per ancestor/descendant pair.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGPGXMJ3W8FR9JZHH3PJT8/description.md sets PO handoff to ready_for_po_critic and lists ## Open Questions as '- none'.
- git log --oneline develop..HEAD on branch ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service shows only orchestration commits 334e93b5f, ded9c98de, 23a0423ae, and 1f5936b7b.
- git diff --name-only develop...HEAD lists only .gicket/tickets/06F2PGPGXMJ3W8FR9JZHH3PJT8/** files; no src/, tests/, or docs/ product files are changed yet.
- README.md, docs/production-adoption-checklist.md, and docs/releases/v0.7.0.md currently state that PIT/bridge reads operate over already materialized tables and do not maintain bridge rows.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers IDataVaultSaveService and IDataVaultReadService, while src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs and src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs show the existing bridge surface is read-only plus registry-backed bridge-name resolution.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs seeds BridgeCustomerOrder and BridgeSalesRegionHierarchy with raw INSERT statements, confirming bridge population is currently manual.
- tests/DCoding.Data.DVault.Tests/Integration/Snapshots/SqliteDataVaultSchemaSnapshot.txt and docs/plans/bridge-metadata-v1-contract.md define BridgeSalesRegionHierarchy with columns AncestorSalesRegionHashKey, DescendantSalesRegionHashKey, TraversalDepth and a primary key only on AncestorSalesRegionHashKey + DescendantSalesRegionHashKey.
- A repository search for shortest/minimal/minimum depth wording found no rule that resolves conflicting TraversalDepth values when multiple hierarchy paths reach the same ancestor/descendant pair.
- .gicket/relations/T8/XC/06F2PGPGXMJ3W8FR9JZHH3PJT8--06F2PGPKXWRFXNPFA1JR0X67XC--blocks.json and .gicket/relations/T8/VG/06F2PGPGXMJ3W8FR9JZHH3PJT8--06F2PGPXVAYRBC94RQ7X5V4DVG--blocks.json show this story blocks the downstream query-API and documentation tickets.

Blocking findings
- The contract does not define the authoritative TraversalDepth rule when multiple hierarchy paths produce the same ancestor/descendant pair. That is a real gap because the shipped hierarchy bridge shape stores only one row per ancestor/descendant pair, so developers cannot derive whether maintenance must keep the shortest depth, first-seen depth, fail as unsupported, or use another rule.
- The contract also does not state what incremental maintenance must do when newly ingested recursive-link data creates a shorter path for an ancestor/descendant pair that already exists in the bridge table. Without that rule, the idempotence and maximumDepth semantics consumed by downstream tickets are not stable enough for handoff.

Required PO actions
- Add an explicit hierarchy-depth rule for duplicate ancestor/descendant pairs created by multiple paths, including which TraversalDepth value is persisted.
- Clarify incremental hierarchy behavior when later source-link ingestion creates a shorter alternate path for an already materialized ancestor/descendant pair: update existing depth, reject as unsupported, or preserve the original depth by contract.
- Promote that rule into acceptance criteria and test expectations so downstream query-API and documentation tickets inherit one deterministic hierarchy semantics baseline.

Open issues ledger
- critic-item-1 [required-po-action] Add an explicit hierarchy-depth rule for duplicate ancestor/descendant pairs created by multiple paths, including which TraversalDepth value is persisted.
- critic-item-2 [required-po-action] Clarify incremental hierarchy behavior when later source-link ingestion creates a shorter alternate path for an already materialized ancestor/descendant pair: update existing depth, reject as unsupported, or preserve the original depth by contract.
- critic-item-3 [required-po-action] Promote that rule into acceptance criteria and test expectations so downstream query-API and documentation tickets inherit one deterministic hierarchy semantics baseline.
- critic-item-4 [blocking-finding] The contract does not define the authoritative TraversalDepth rule when multiple hierarchy paths produce the same ancestor/descendant pair. That is a real gap because the shipped hierarchy bridge shape stores only one row per ancestor/descendant pair, so developers cannot derive whether maintenance must keep the shortest depth, first-seen depth, fail as unsupported, or use another rule.
- critic-item-5 [blocking-finding] The contract also does not state what incremental maintenance must do when newly ingested recursive-link data creates a shorter path for an ancestor/descendant pair that already exists in the bridge table. Without that rule, the idempotence and maximumDepth semantics consumed by downstream tickets are not stable enough for handoff.

Missing examples / edge cases
- A hierarchy example where the same ancestor/descendant pair is reachable by two different path lengths.
- An incremental-maintenance example where a newly added recursive link shortens an existing path.
- A cycle or self-loop example that proves termination behavior and confirms whether no implicit self-row still holds under cyclic source data.

Risky assumptions
- Assuming developers will infer a shortest-path rule from the current maximumDepth read behavior, even though no contract text states that rule.
- Assuming recursive source-link data is acyclic or otherwise harmless without an explicit contract for cycles.
- Assuming the v0.15.0 release-note delta can simply be created during implementation; docs/releases currently contains release-note files through v0.14.0 only.

AC / test suggestions
- Add an acceptance criterion that hierarchy maintenance persists one deterministic positive TraversalDepth per unique ancestor/descendant pair and name the rule explicitly.
- Add a SQLite integration test where two paths reach the same descendant at different depths and verify the persisted depth rule.
- Add an incremental hierarchy test where a new edge creates a shorter path after initial maintenance and verify the contract outcome.

Implementation watchouts
- Keep the new surface additive beside IDataVaultSaveService and IDataVaultReadService; the current AddDVault registration pattern is explicit-service based and should not be collapsed into implicit SaveChanges or read-time maintenance.
- Do not weaken existing bridge-read contract tests; the current read tests manually seed bridge rows and should remain read-surface validation while new maintenance-specific tests prove population behavior.
- Documentation updates must replace the current read-only bridge limitation without implying automatic maintenance; current README and checklist wording is explicitly caller-invoked and service-based.

Non-blocking notes
- The persisted contract is otherwise strong: scope boundaries, registry-backed maintenance intent, idempotence expectations, and explicit non-goals are all concrete.
- The current branch state is consistent with a pre-development quality gate: ticket metadata changed, but no product code has been started yet.

Split recommendations
- No split recommended once the hierarchy TraversalDepth rule is clarified; the existing sibling tickets already isolate PIT maintenance, query API follow-up, provider optimization, and broader documentation work.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment