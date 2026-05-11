<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Resolved the routed PO clarification: package validation must use the capable mutable dev or release-validation runner when rerun, but the current ticket already has accepted capable-runner pre-tag evidence. The PO-critic tracking-parent blocker is resolved as a false classification; this ticket is a concrete docs/package-validation task and currently has an incoming parentOf relation, not an outgoing parent obligation.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The active package-validation blocker is answered: accepted capable-runner pre-tag validation satisfies this ticket; future reruns must use the capable mutable dev or release-validation runner, not the restricted cache-incomplete runner.
- The PO-critic parentOf blocker is answered: this ticket is not a tracking-only parent and does not require outgoing parentOf child tickets.
- No child tickets, relation updates, planning documents, or attachments were created in this pass because the live relation state and later manual override resolve the blocker without persistent writes.

### Scope In
- Keep README.md and docs/releases/v0.6.0.md as the authoritative v0.6.0 documentation artifacts.
- Keep package verifier source and tests aligned with README.md v0.6.0 install guidance.
- Accept the recorded capable-runner pre-tag package validation at commit 3967d99c57977b65770dff03c79b0f938ade059d as satisfying this ticket's package-validation requirement.
- Preserve final tagged-release validation and publish approval as release-operator work under docs/manual-nuget-publication.md.

### Scope Out
- Publishing NuGet packages.
- Creating or pushing the v0.6.0 release tag.
- Requiring final 0.6.0 package artifact filenames before the v0.6.0 tag exists.
- Editing documentation, product code, package metadata, or release automation to bypass package verification or sandbox limits.
- Creating child tickets only to satisfy the stale tracking-parent classification.

## Acceptance Criteria
- README.md and docs/releases/v0.6.0.md remain the authoritative updated documentation artifacts for the v0.6.0 release.
- Package verifier source and tests accept README v0.6.0 install guidance and do not require stale v0.5.0 README install strings.
- The accepted capable-runner validation evidence at commit 3967d99c57977b65770dff03c79b0f938ade059d remains part of the ticket history.
- Pre-tag validation may pass with MinVer prerelease package artifact version 0.5.1-alpha.0.69 when all six package ids and six symbol packages are freshly produced and verified.
- docs/manual-nuget-publication.md remains the authority for final tagged-release validation and publish approval.

## Definition of Done
- The older literal $sha validation claim and stale 0.5.1-alpha.0.58 artifact observation are treated as superseded historical evidence.
- The accepted validation record includes exact checkout hash, package artifact version, package directory state, and successful verify-packages summary.
- No product decision remains about runner routing: package validation evidence must come from the capable mutable dev or release-validation runner, and the recorded capable-runner pass satisfies this ticket's pre-tag package-validation requirement.
- No tracking-parent closure work remains: this ticket does not require outgoing parentOf children unless future evidence creates a concrete child-worthy defect.

## Implementation Notes
- Before any future package-validation rerun, clear artifacts/packages to avoid stale artifact evidence, then pack all six projects and run tools/verify-packages.sh.
- Provider dependency checks should compare each provider package to the packed DCoding.Data.DVault core package version, not to a hard-coded final release version.
- Do not alter docs, package metadata, verifier rules, or release automation to hide runner capability failures.
- Treat the no-outgoing-parentOf observation as non-blocking for this ticket because the latest relation/comment evidence shows it is not a tracking-only parent.

## Open Questions
- none

## Follow-Up Questions
- After the v0.6.0 tag exists, the release operator must rerun the manual NuGet publication checklist from the tagged checkout and record final audited 0.6.0 artifact evidence before publication.
- If a future capable-runner validation fails for reasons other than expected pre-tag MinVer versioning, create a concrete packaging-verifier follow-up with the failing output and artifact state.

## Risks
- Routing package validation back to a restricted cache-incomplete runner would repeat the known blocker.
- Reviewers may confuse forward-looking README 0.6.0 install guidance with pre-tag MinVer prerelease artifact filenames; the contract separates those concerns.
- Final package publication remains outside this ticket and still requires the release operator's audited approval.

## Split Recommendations
- No split is recommended now because capable-runner validation already exists and satisfies the current pre-tag package-validation contract.
- Do not create child tickets solely to satisfy the stale tracking-parent closure audit; split only a future concrete non-MinVer packaging or verifier defect with capable-runner output.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Update durable documentation for the new v0.6 usability path once the APIs and examples are implemented.

## Scope In

- README install and quickstart sections for v0.6.0.
- Release notes summarizing Code-First, registry, typed helpers, diagnostics, and examples.
- Migration guidance for users coming from v0.5 metadata-first usage.

## Scope Out

- Documenting v0.7 model-first behavior as implemented.
- Publishing packages.

## Acceptance Criteria

- Docs show the recommended happy path first and keep the metadata-first path as an advanced/compatible option.
- Release notes identify remaining future work for model-first specs and PIT/bridge reads.
- Package verification still passes with the updated README.