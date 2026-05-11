[gicket-bot] PO refinement contract

Summary
- Manual capable-runner package validation evidence has now been recorded, so the prior runner-routing blocker is resolved without repository workaround edits. The ticket can continue because dotnet pack and tools/verify-packages.sh are recorded as successful on a capable runner.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - Satisfied by the new ticket-history evidence: a manual capable-runner validation comment records successful dotnet pack and tools/verify-packages.sh results, and the runtime escalation was manually cleared because that evidence was recorded on commit 688f0c7e. The ticket no longer needs a future capable-runner assignment guarantee before PO-critic because the required package-validation pass evidence already exists.
- critic-item-2: `answered` - No repository edits are requested or needed to work around sandbox network/cache restrictions. The contract remains that README.md and docs/releases/v0.6.0.md stay as-is, and validation must use the existing package commands.
- critic-item-3: `answered` - The earlier conditional runner-routing issue is now superseded by recorded capable-runner validation evidence. Because the package-validation gate is already satisfied, PO-critic success no longer depends on guaranteeing that the next dev runner has network/cache capability for these two commands.
- critic-item-4: `answered` - Approving through the configured path no longer violates the routing constraint because the normal-dev capability condition was only needed to obtain package-validation evidence. That evidence has now been recorded by a capable manual validation lane, so the remaining ticket work is evidence recognition and downstream review, not another package-validation attempt in restricted dev.
- critic-item-5: `answered` - The required package-validation pass evidence is no longer absent. Ticket comments now include explicit successful results for both required commands on a capable runner.

Clarifications
- The routed clarification is resolved by newly recorded manual package-validation evidence, not by changing repository content or bypassing validation.
- The capable-runner routing blocker is now historical for this ticket because the required package-validation evidence has already been recorded.
- No child tickets, relation changes, planning documents, or attachments were created in this refinement pass.

Scope In
- Keep README.md and docs/releases/v0.6.0.md as the current v0.6.0 documentation scope.
- Use the recorded manual capable-runner evidence for dotnet pack DVault.slnx --configuration Release --nologo and bash tools/verify-packages.sh as satisfying the package-validation gate.
- Continue downstream review or validation using the existing repository commands and documented release process.

Scope Out
- Repository edits to work around network/cache restrictions.
- Repeating package validation in the known restricted/cache-incomplete runner as a required next step.
- Routing to tester without recognizing the recorded successful package-validation evidence.
- Publishing NuGet packages or replacing the manual release checklist as part of this ticket.

Open questions
- none

Follow-up questions
- Before manual NuGet publication, the release operator still needs final audited release approval values under docs/manual-nuget-publication.md.
- If a later capable validation lane fails for repository-content reasons, route a concrete packaging follow-up with the failing command output.

Risks
- A future automated run could still misinterpret the old runner-routing blocker if it ignores the newer manual validation evidence comment.
- Manual NuGet publication remains outside this ticket and still requires the release operator's final audited approval.

Split recommendations
- No split recommended now. The package-validation blocker is resolved by recorded capable-runner evidence; split only if later validation exposes a real packaging defect.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment