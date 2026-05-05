[gicket-bot] PO-critic review contract

Summary
- Repository evidence confirms the snapshot guardrail already exists, but the ticket still persists live `blocks` relations to the PIT, bridge, and multi-active stories, so the closure-only contract is not yet self-consistent for handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Direct repo read: `tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs` defines six approval tests, `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/` contains six approved snapshot files, and `docs/quality/api-surface-snapshots.md` documents the `dotnet test DVault.slnx --nologo` / `DVAULT_UPDATE_API_SNAPSHOTS=1` workflow.
- Branch-history check: `git show --stat --oneline ee83fd3de -- tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs docs/quality/api-surface-snapshots.md tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi` lists the approval test, policy doc, and six approved snapshot files, so the shared guardrail predates this closure-only ticket state.
- Direct repo read: `DVault.slnx`, `docs/manual-nuget-publication.md`, and `src/DCoding.Data/DCoding.Data.csproj` show the six coordinated packable packages and that `src/DCoding.Data` is non-packable.
- Repository search: `rg -n 'PIT|Bridge|Multi|Hook' tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi` returned no matches, so no deferred-capability public surface is currently approved in the snapshot baselines.
- Direct repo read: `docs/plans/deferred-data-vault-capabilities.md` maps PIT to `06EZ0NSXY2Y1JZ8SSCX177C770`, bridge to `06EZ0NTV4SVAKV98C418T8A3CC`, multi-active to `06EZ0NVN71BN0QWJDCWGVZ2PYG`, hooks to `06EZ0NWKC9ZME5BSCJFSQEQ02R`, and says ticket `06EZ0NSQFCD3W4CDCJ44GFSKA0` must not infer concrete API names.
- Persisted ticket state: `.gicket/relations/A0/70/06EZ0NSQFCD3W4CDCJ44GFSKA0--06EZ0NSXY2Y1JZ8SSCX177C770--blocks.json`, `.gicket/relations/A0/CC/06EZ0NSQFCD3W4CDCJ44GFSKA0--06EZ0NTV4SVAKV98C418T8A3CC--blocks.json`, and `.gicket/relations/A0/YG/06EZ0NSQFCD3W4CDCJ44GFSKA0--06EZ0NVN71BN0QWJDCWGVZ2PYG--blocks.json` still exist as live `blocks` relations.
- Ticket/comment evidence: `.gicket/tickets/06EZ0NSQFCD3W4CDCJ44GFSKA0/description.md` says the legacy `blocks` links still remain because relation cleanup could not be materialized, and comment `06EZMC29HCFWNQ5E9EBDA1NFJW.md` repeats the risk that humans or automation may still read this ticket as a prerequisite.
- Current branch diff check: `git diff --name-only develop..HEAD | rg -v '^\.gicket/'` returned no output, so this branch is carrying ticket-state changes only.

Blocking findings
- The persisted ticket state still actively blocks PIT, bridge, and multi-active owner stories, which conflicts with the contract claim that this ticket no longer serves as the reason to block those stories.

Required PO actions
- Remove or downgrade the three live `blocks` relations from this ticket to the PIT, bridge, and multi-active stories, or reopen the contract language so it does not claim the ticket no longer blocks them before that cleanup exists.
- If the per-owning-story snapshot rule is intended to cover hooks too, name hook story `06EZ0NWKC9ZME5BSCJFSQEQ02R` explicitly or state why it is intentionally excluded.

Open issues ledger
- critic-item-1 [required-po-action] Remove or downgrade the three live `blocks` relations from this ticket to the PIT, bridge, and multi-active stories, or reopen the contract language so it does not claim the ticket no longer blocks them before that cleanup exists.
- critic-item-2 [required-po-action] If the per-owning-story snapshot rule is intended to cover hooks too, name hook story `06EZ0NWKC9ZME5BSCJFSQEQ02R` explicitly or state why it is intentionally excluded.
- critic-item-3 [blocking-finding] The persisted ticket state still actively blocks PIT, bridge, and multi-active owner stories, which conflicts with the contract claim that this ticket no longer serves as the reason to block those stories.

Missing examples / edge cases
- No ticket-level example shows how the same ownership rule applies when the deferred capability is the existing hooks story rather than PIT, bridge, or multi-active.
- No example shows how reviewers should interpret backlog state if the closure text says a ticket is not a blocker while live `blocks` relations still remain.

Risky assumptions
- Assuming readers and automation will ignore live `blocks` relations because the description says they are stale.
- Assuming advanced-hook work will follow the same snapshot ownership rule without naming the existing hook owner story in the contract.

AC / test suggestions
- Make the non-blocking acceptance criterion conditional on actual relation cleanup, or add a separate acceptance criterion that tracks the required relation downgrade/removal as ticket hygiene.
- Add one explicit sentence that any future hook public API follows the same owning-story snapshot rule as PIT, bridge, and multi-active.

Implementation watchouts
- Any future public deferred-capability change should update only the affected file under `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/` in the same change; do not create placeholder public types to force snapshot activity.
- Because provider packages share the `DCoding.Data.DVault` namespace, ownership and review should anchor to the package-specific approved snapshot filename, not namespace text.
- Until relation cleanup lands, humans or automation can still route dependency decisions from the stale `blocks` relations instead of the refined contract.

Non-blocking notes
- `## Open Questions` is `none`, so the decision is not blocked by unresolved open questions.
- The contract now names the internal-only audit evidence location: the implementing story's final delivery summary or change description plus no diff under `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/`.
- Current repo evidence supports the main closure claim that the shared snapshot guardrail already exists and is already documented.

Split recommendations
- Keep public API snapshot diffs on the concrete owning capability story that first exports the public contract.
- Use a small backlog/admin follow-up for relation cleanup if the normal PO write surface still cannot remove or downgrade the stale `blocks` links.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment