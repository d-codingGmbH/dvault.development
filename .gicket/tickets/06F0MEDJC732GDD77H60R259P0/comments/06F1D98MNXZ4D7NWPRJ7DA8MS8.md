[gicket-bot] PO refinement contract

Summary
- Resolved the routed PO clarification: package validation must use the capable mutable dev or release-validation runner when rerun, but the current ticket already has accepted capable-runner pre-tag evidence. The PO-critic tracking-parent blocker is resolved as a false classification; this ticket is a concrete docs/package-validation task and currently has an incoming parentOf relation, not an outgoing parent obligation.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- package-validation-routing: `answered` - Do not route package validation back to the restricted cache-incomplete runner. The accepted capable-runner validation at commit 3967d99c57977b65770dff03c79b0f938ade059d satisfies this ticket's pre-tag package-validation requirement; any future rerun belongs on the PO-approved network/cache-enabled mutable dev or release-validation runner with a complete NuGet cache.
- critic-item-1: `answered` - The tracking-parent closure audit finding is resolved by classifying this ticket as a concrete documentation/package-validation task rather than a tracking-only parent. No child-ticket materialization is required for closure of this ticket; release-operator tagged validation remains a documented follow-up outside this ticket.
- critic-item-2: `answered` - No outgoing parentOf child tickets are required. The absence of outgoing parentOf relations is expected for this non-tracking docs task, so no gicket-create-ticket or gicket-add-relation action was performed.

Clarifications
- The active package-validation blocker is answered: accepted capable-runner pre-tag validation satisfies this ticket; future reruns must use the capable mutable dev or release-validation runner, not the restricted cache-incomplete runner.
- The PO-critic parentOf blocker is answered: this ticket is not a tracking-only parent and does not require outgoing parentOf child tickets.
- No child tickets, relation updates, planning documents, or attachments were created in this pass because the live relation state and later manual override resolve the blocker without persistent writes.

Scope In
- Keep README.md and docs/releases/v0.6.0.md as the authoritative v0.6.0 documentation artifacts.
- Keep package verifier source and tests aligned with README.md v0.6.0 install guidance.
- Accept the recorded capable-runner pre-tag package validation at commit 3967d99c57977b65770dff03c79b0f938ade059d as satisfying this ticket's package-validation requirement.
- Preserve final tagged-release validation and publish approval as release-operator work under docs/manual-nuget-publication.md.

Scope Out
- Publishing NuGet packages.
- Creating or pushing the v0.6.0 release tag.
- Requiring final 0.6.0 package artifact filenames before the v0.6.0 tag exists.
- Editing documentation, product code, package metadata, or release automation to bypass package verification or sandbox limits.
- Creating child tickets only to satisfy the stale tracking-parent classification.

Open questions
- none

Follow-up questions
- After the v0.6.0 tag exists, the release operator must rerun the manual NuGet publication checklist from the tagged checkout and record final audited 0.6.0 artifact evidence before publication.
- If a future capable-runner validation fails for reasons other than expected pre-tag MinVer versioning, create a concrete packaging-verifier follow-up with the failing output and artifact state.

Risks
- Routing package validation back to a restricted cache-incomplete runner would repeat the known blocker.
- Reviewers may confuse forward-looking README 0.6.0 install guidance with pre-tag MinVer prerelease artifact filenames; the contract separates those concerns.
- Final package publication remains outside this ticket and still requires the release operator's audited approval.

Split recommendations
- No split is recommended now because capable-runner validation already exists and satisfies the current pre-tag package-validation contract.
- Do not create child tickets solely to satisfy the stale tracking-parent closure audit; split only a future concrete non-MinVer packaging or verifier defect with capable-runner output.

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