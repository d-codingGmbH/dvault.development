[gicket-bot] PO refinement contract

Summary
- Resolved: keep the existing docs unchanged and resume to dev only if the dev workflow can run on a network/cache-enabled mutable runner; otherwise route to release-validation with a complete NuGet cache before tester signoff.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- Can the workflow be rescheduled onto the PO-approved network/cache-enabled mutable dev or release-validation runner, or should this ticket be routed directly to the release-validation role that has a complete NuGet cache?: `answered` - Use the first available capable validation lane. Prefer rescheduling dev onto a network/cache-enabled mutable runner so the required commands can be executed in the implementation workflow. If that runner cannot be provided, route directly to release-validation with a complete NuGet cache. Do not send this ticket to tester until successful `dotnet pack DVault.slnx --configuration Release --nologo` and `bash tools/verify-packages.sh` evidence exists from one of those capable runners.

Clarifications
- The correct product decision is not to change repository content or weaken validation. The remaining work is to run package validation in a capable environment.
- A network/cache-enabled mutable dev runner and a release-validation runner with a complete NuGet cache are both acceptable validation lanes for this ticket.
- Tester handoff remains blocked until package validation pass evidence is attached or recorded by the capable runner.

Scope In
- Keep README.md and docs/releases/v0.6.0.md as the current v0.6.0 documentation scope.
- Produce successful `dotnet pack DVault.slnx --configuration Release --nologo` evidence from a capable runner.
- Produce successful `bash tools/verify-packages.sh` evidence from a capable runner.

Scope Out
- Editing docs, product code, package metadata, provider behavior, or release automation to work around the sandbox limitation.
- Treating the current no-network/cache-incomplete sandbox failure as package-validation pass evidence.
- Routing to tester before capable-runner package validation passes.

Open questions
- none

Follow-up questions
- Before manual publication, the release operator still needs final audited validation evidence and publish approval values.
- If the capable runner fails package verification for repository-content reasons, create or route a concrete packaging follow-up with the failing command output.

Risks
- Sending this directly to tester would repeat the known package-verification blocker.
- Running dev again in the same network-restricted/cache-incomplete sandbox will not satisfy the ticket contract.
- Bypassing the validation gate through docs, metadata, or automation edits would violate the approved scope.

Split recommendations
- No split recommended. Use a capable validation runner; split only if capable-runner output proves a real packaging defect that needs separate remediation.

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