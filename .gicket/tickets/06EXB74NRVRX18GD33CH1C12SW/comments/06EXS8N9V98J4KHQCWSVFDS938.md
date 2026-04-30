[gicket-bot] PO refinement contract

Summary
- Repository and ticket evidence are now available through the declared tool loop, so the missing-repo-state clarification is resolved for planning. The downstream dev role should resume implementation using the verified branch state and existing delivery contract; no product-code contract changes or new planning artifacts are required.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- Expose the declared repository tools through the adapter tool surface, or rerun with direct repository mutation enabled for this role so the implementation artifacts can be repaired and verified.: `answered` - The declared repository and ticket tools are available and were used in the interactive tool loop. The ticket, comments, relations, attachments, src/DCoding.Data.DVault/Modeling layout, tests/DCoding.Data.DVault.Tests/Modeling layout, TechnicalMetadataColumnRole role set, and src/DCoding.Data.DVault/DCoding.Data.DVault.csproj baseline were re-read. This resolves the missing_repo_state blocker for PO planning; the implementation can resume in dev with repository mutation enabled through the declared tool surface, using the current branch state rather than the prior unverified format-gate plan.
- Risk: Returning source artifacts without re-reading and verifying the current repository state would risk repeating the prior failing format-gate plan unchanged.: `answered` - The current repository state was re-read through bounded repository tools before this PO response. The verified state supports the existing delivery contract and narrows the downstream instruction: dev should repair and verify against the current branch, including bash tools/check-format.sh and dotnet test/build diagnostics, rather than returning or reusing stale source artifacts from earlier failed attempts.

Clarifications
- The missing_repo_state blocker is resolved because the declared repository and ticket read tools were exposed and used successfully in this PO pass.
- No new child tickets, relations, attachments, or planning documents were created in this pass; the two existing parentOf child relations remain the persisted split context.
- The authoritative ticket contract remains unchanged: implementation belongs in src/DCoding.Data.DVault, modeling APIs should use DCoding.Data.DVault.Modeling where appropriate, and tests belong under tests/DCoding.Data.DVault.Tests.
- The closed v1 technical metadata role baseline remains HashKey, HashDiff, LoadTimestamp, and RecordSource.
- Prior sandbox-specific dotnet/MSBuild IPC limitations should be recorded by dev if they recur, but they do not change the PO scope or acceptance contract.

Scope In
- Define provider-neutral metadata abstractions for hub, link, and satellite concepts.
- Represent business key metadata for hubs, link participant metadata for links, and satellite payload metadata for satellites.
- Represent technical metadata roles for hash keys, hash diffs, load timestamps, and record source using the existing closed v1 role set.
- Document public or protected APIs with XML documentation consistent with the net10.0 project baseline and CS1591 enforcement.
- Add focused unit coverage for concept shape, role coverage, naming/default behavior, and provider-neutral behavior.

Scope Out
- Schema generation, migrations, loading automation, validation tooling, and provider-specific Sqlite/Postgres behavior.
- Hash algorithm implementation or model-specific hash input normalization beyond referencing the stable hashing contract.
- PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations.
- Runtime configuration APIs, custom option matrices, and advanced override hooks unless a minimal internal shape is necessary to keep the abstractions provider-neutral.
- Changing default naming semantics, persistence convention policy, repository layout, target framework, or package identity.

Open questions
- none

Follow-up questions
- Decide in later implementation or governance tickets whether the existing child tickets 06EXB74XQJFKGSKVJ6THQWJY8W and 06EXB755X9TGQW2EG1G30GJG28 should remain separate delivery slices or be completed before this parent story closes.
- Plan separate stories for schema generation, loading automation, provider adapters, PIT tables, bridge tables, and multi-active satellites when those capabilities are scheduled.
- Plan a separate API design ticket if advanced configuration hooks need a public options surface beyond the current convention-first defaults.
- If sandbox-specific dotnet/MSBuild IPC failures recur, dev should report them as execution-environment limitations with the exact command and diagnostic, separate from source correctness.

Risks
- The parent story spans several related modeling concepts, so implementation should keep the first pass narrow and avoid drifting into provider persistence or automation work.
- Existing source already includes technical metadata contract types; developers should preserve that baseline and extend around it rather than creating a competing concept model.
- Hash key and hash diff metadata may be confused with hash computation. This ticket should keep computation and normalization out of scope.
- Format-gate and dotnet verification should be rerun from the current repository state after any implementation repair so earlier failed automation loops are not repeated blindly.

Split recommendations
- No new child ticket was created in this pass because existing parentOf relations already show two child tickets under this story.
- If implementation proves too large, split by concept family: hub/business-key metadata, link/participant metadata, and satellite/payload metadata, while keeping the shared technical metadata role set common.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment