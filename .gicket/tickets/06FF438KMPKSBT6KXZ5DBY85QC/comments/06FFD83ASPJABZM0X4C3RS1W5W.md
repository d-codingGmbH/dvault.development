[gicket-bot] PO-critic review contract

Summary
- Closure-only audit failed: the refined ticket contract is specific, but branch `ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi` at `668d9d85c756c122f39ab499ead920d22acd0e55` only carries ticket metadata changes and does not land the required provider-evidence contract updates.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FF438KMPKSBT6KXZ5DBY85QC/description.md` keeps `## Open Questions` as `none` and its `## Definition of Done` requires the authoritative evidence-contract document set to be updated with PIT maintenance timing rows.
- `git diff --name-only main...HEAD` for branch `ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi` lists only `.gicket/tickets/06FF438KMPKSBT6KXZ5DBY85QC/**` files; no changes under `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/performance-evidence-benchmark-artifact-contract.md`, or `docs/architecture/dvault-v1-pit-bridge-boundary.md` are present on this branch.
- `docs/plans/provider-optimization-evidence-matrix.md` currently contains `## Save Matrix`, `## Read Matrix`, `## Deferred Bridge Maintenance Push-Down`, and `## Hash-Key Storage Matrix`; its concrete rows still use read scenarios such as `pit-as-of-read` and `bridge-traversal-read`, and no PIT full-rebuild maintenance row family is present.
- `docs/plans/performance-evidence-benchmark-artifact-contract.md` still defines the minimum scenario baseline as `latest satellite read`, `PIT as-of read`, and `bridge traversal read`, and its provider-fact mapping only names `executionPath`, `selectedStrategy`, `plannedReadStrategy`, `readShape`, and fallback-cause tokens; no PIT maintenance row mapping is documented there.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md` already fixes the provider boundary the ticket depends on: PostgreSQL full rebuilds may use `PostgresDataVaultPitMaintenanceStrategy`, SQL Server full rebuilds stay limited to `SqlServerDataVaultPitMaintenanceService` for clean ordinary hub-parent PITs, and bridge maintenance remains provider-neutral/out of scope.

Blocking findings
- This closure-only audit cannot pass because the repository branch does not contain the documentation changes that the ticket's Definition of Done requires; the observed branch diff is ticket metadata only.
- The runtime is treating this as a closure-only ticket, but the persisted contract still describes unapplied documentation work on `docs/plans/provider-optimization-evidence-matrix.md` and possibly the shared benchmark contract. That routing mismatch must be corrected before the ticket can move forward.

Required PO actions
- Return the ticket to PO refinement and correct the workflow classification: either remove the closure-only expectation and hand this ticket to development as an implementation task, or attach landed repository evidence proving the required docs changes already exist.
- If closure-only handling is still intended, update the ticket contract to cite the exact landed commit/path evidence that satisfies the PIT maintenance timing row requirements; the current branch evidence does not support closure.

Open issues ledger
- critic-item-1 [required-po-action] Return the ticket to PO refinement and correct the workflow classification: either remove the closure-only expectation and hand this ticket to development as an implementation task, or attach landed repository evidence proving the required docs changes already exist.
- critic-item-2 [required-po-action] If closure-only handling is still intended, update the ticket contract to cite the exact landed commit/path evidence that satisfies the PIT maintenance timing row requirements; the current branch evidence does not support closure.
- critic-item-3 [blocking-finding] This closure-only audit cannot pass because the repository branch does not contain the documentation changes that the ticket's Definition of Done requires; the observed branch diff is ticket metadata only.
- critic-item-4 [blocking-finding] The runtime is treating this as a closure-only ticket, but the persisted contract still describes unapplied documentation work on `docs/plans/provider-optimization-evidence-matrix.md` and possibly the shared benchmark contract. That routing mismatch must be corrected before the ticket can move forward.

Missing examples / edge cases
- none

Risky assumptions
- The contract assumes a future PIT maintenance row can reuse existing benchmark row fields and `executionDetail` vocabulary without an explicit maintenance-row mapping in the shared artifact contract.
- The contract assumes sibling tickets `06FF43AH9SK6J07GV5EKYV3AMM`, `06FF43AYQYZKFF400CK5Q84WYR`, and `06FF43BPP5NRJR3JTY48ZNEKHM` will land compatible scenario naming and comparator wording without reopening this ticket.

AC / test suggestions
- When rerouted, require the eventual docs to use a maintenance-specific scenario label that cannot be confused with `pit-as-of-read` or `bridge-traversal-read`.
- If the shared artifact contract must participate, require one explicit example of how a PIT full-rebuild maintenance row maps from benchmark row identity plus deterministic `executionDetail` tokens.

Implementation watchouts
- Do not let read-side evidence rows or maintained-bridge read timings stand in for PIT maintenance claims; the current matrix already warns against analogous bridge-maintenance overreach.
- Keep SQL Server maintenance claims limited to clean ordinary hub-parent full rebuilds; `MaintainParentsAsync(...)`, link-parent, multi-active, dirty-context, and no-savepoint cases stay provider-neutral per `docs/architecture/dvault-v1-pit-bridge-boundary.md`.

Non-blocking notes
- The refined delivery contract itself is otherwise specific: provider boundary, artifact-triplet gate, scope split, and `Open Questions = none` are all directly stated in `description.md`. If the workflow is corrected away from closure-only, the ticket looks close to normal dev handoff quality.

Split recommendations
- No additional split recommended; the existing sibling breakdown still looks sufficient once the ticket is routed out of closure-only handling.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment