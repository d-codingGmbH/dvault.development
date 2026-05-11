[gicket-bot] PO refinement contract

Summary
- Resolved: do not send this ticket to tester and do not change repository content. The only acceptable next lane is a capable validation lane that can record successful dotnet pack and package verification evidence; if the normal dev path cannot guarantee that capability, route to release-validation rather than looping through the same restricted runner.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - Tester handoff remains blocked until successful `dotnet pack DVault.slnx --configuration Release --nologo` and `bash tools/verify-packages.sh` output is recorded from a network/cache-enabled mutable dev runner or a release-validation runner with a complete NuGet cache. The current network-restricted/cache-incomplete run is not acceptable pass evidence.
- critic-item-2: `answered` - No docs, product-code, package metadata, provider behavior, or release automation edits are requested or allowed to work around the sandbox limitation. The repository evidence shows the required docs and validation script already exist; the blocker is runner capability, not repository content.
- critic-item-3: `answered` - The ticket must not be approved for unconditional developer handoff through a runner with unknown capability. It may resume to dev only when the next dev assignment is explicitly network/cache-enabled and mutable. If that guarantee is unavailable, route to release-validation with a complete NuGet cache so the same validation commands can be recorded without repository edits.
- critic-item-4: `answered` - Because the current PO-critic success path cannot itself encode release-validation routing, PO approval is limited to a conditional routing contract: do not use the normal dev success path unless the orchestrator assigns a capable dev runner. Otherwise the ticket-level next lane is release-validation with a complete NuGet cache. This resolves the loop risk without changing repository scope.

Clarifications
- The product decision is runner routing, not repository editing: use a capable validation lane and record the required package evidence.
- Preferred next lane is dev only if the next dev run is explicitly network/cache-enabled and mutable.
- Fallback next lane is release-validation with a complete NuGet cache when capable dev cannot be guaranteed by orchestration.
- Tester remains blocked until both package validation commands pass in one of those capable lanes.
- No child tickets, relation updates, planning documents, or attachments were created because the existing ticket contract and repository evidence are sufficient.

Scope In
- Keep README.md and docs/releases/v0.6.0.md as the current v0.6.0 documentation scope.
- Record successful `dotnet pack DVault.slnx --configuration Release --nologo` output from a capable runner.
- Record successful `bash tools/verify-packages.sh` output from the same capable validation lane or another explicitly capable validation lane.
- If capable-runner validation fails for repository-content reasons, capture the failing output and route a concrete packaging follow-up.

Scope Out
- Editing docs, product code, package metadata, provider behavior, or release automation to work around network/cache restrictions.
- Treating the current network-restricted/cache-incomplete sandbox failure as validation pass evidence.
- Routing to tester before capable-runner package validation pass evidence is recorded.
- Publishing packages or replacing the manual release checklist as part of this ticket.

Open questions
- none

Follow-up questions
- Before manual NuGet publication, the release operator still needs final audited validation evidence and publish approval values.
- If the capable validation lane fails for repository-content reasons, create or route a concrete packaging follow-up with the failing command output.

Risks
- Returning directly to tester would repeat the known package-verification blocker.
- Running dev again in the same restricted runner will not satisfy the validation gate.
- Approving a generic dev handoff without a capable-runner guarantee could loop back to PO-critic.
- Bypassing the validation gate through repository edits would violate the approved scope.

Split recommendations
- No split recommended now. Split only if capable-runner output proves a real packaging defect that needs separate remediation.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment