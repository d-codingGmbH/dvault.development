[gicket-bot] PO refinement contract

Summary
- Resolved the package-verification clarification: keep the current README.md and docs/releases/v0.6.0.md documentation changes unchanged, but route remaining executable package validation to a network/cache-enabled mutable dev or release-validation runner before tester signoff; do not downgrade the delivery contract to publish-time-only deferral.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- Should this ticket be rerun in a network/cache-enabled mutable dev runner with the current repository docs unchanged, or should the delivery contract be changed to defer that executable package verification to the release operator?: `answered` - Rerun the validation portion in a network/cache-enabled mutable dev or release-validation runner with the current repository docs unchanged. The docs artifacts satisfy the documentation scope, but package packing and tools/verify-packages.sh remain part of this ticket's completion evidence and should not be deferred solely to the release operator. If the rerun proves the package gate cannot be completed for reasons unrelated to the docs change, return with the concrete failing command/output rather than editing README.md or docs/releases/v0.6.0.md.

Clarifications
- The remaining blocker is environmental validation capability, not a documentation-content gap.
- README.md and docs/releases/v0.6.0.md should remain unchanged unless the network/cache-enabled runner finds an actual docs, package metadata, or verification failure.
- Package verification is still required before this ticket can pass tester review; it is not reduced to a release-operator-only follow-up.
- A valid completion path is to rerun dotnet pack and bash tools/verify-packages.sh in a runner that can restore required NuGet dependencies or already has the EF Core package cache available.

Scope In
- Keep the existing README.md and docs/releases/v0.6.0.md v0.6.0 documentation updates as the ticket's product scope.
- Produce executable evidence for dotnet pack DVault.slnx --configuration Release --nologo and bash tools/verify-packages.sh from a network/cache-enabled mutable runner.
- Preserve the manual NuGet publication checklist alignment for final release evidence.

Scope Out
- Changing product code, package metadata, provider behavior, or release automation to work around the current sandbox limitation.
- Publishing packages or recording final publish approval as part of this docs ticket.
- Weakening the package verification gate to documentation-only review or release-operator-only deferral.

Open questions
- none

Follow-up questions
- Before manual publication, the release operator still needs to replace pending validation/approval placeholders with final audited release evidence and approval values.
- If the capable runner fails package verification for a non-environmental reason, create or route a concrete packaging follow-up using the failing command output.

Risks
- Returning directly to tester without successful pack and tools/verify-packages.sh evidence would likely repeat the same blocker.
- Deferring all package verification to the release operator would weaken the existing ticket contract and manual publication gate.
- The earlier failed restore attempts may have created ignored restore/build artifacts, but git status evidence shows no tracked file changes from those attempts.

Split recommendations
- No split recommended for the documentation ticket. Use a capable runner for validation rather than creating a child ticket unless package verification fails for a real repository issue.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 3
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment