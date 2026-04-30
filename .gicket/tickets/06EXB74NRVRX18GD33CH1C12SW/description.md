<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository and ticket evidence are now available through the declared tool loop, so the missing-repo-state clarification is resolved for planning. The downstream dev role should resume implementation using the verified branch state and existing delivery contract; no product-code contract changes or new planning artifacts are required.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The missing_repo_state blocker is resolved because the declared repository and ticket read tools were exposed and used successfully in this PO pass.
- No new child tickets, relations, attachments, or planning documents were created in this pass; the two existing parentOf child relations remain the persisted split context.
- The authoritative ticket contract remains unchanged: implementation belongs in src/DCoding.Data.DVault, modeling APIs should use DCoding.Data.DVault.Modeling where appropriate, and tests belong under tests/DCoding.Data.DVault.Tests.
- The closed v1 technical metadata role baseline remains HashKey, HashDiff, LoadTimestamp, and RecordSource.
- Prior sandbox-specific dotnet/MSBuild IPC limitations should be recorded by dev if they recur, but they do not change the PO scope or acceptance contract.

### Scope In
- Define provider-neutral metadata abstractions for hub, link, and satellite concepts.
- Represent business key metadata for hubs, link participant metadata for links, and satellite payload metadata for satellites.
- Represent technical metadata roles for hash keys, hash diffs, load timestamps, and record source using the existing closed v1 role set.
- Document public or protected APIs with XML documentation consistent with the net10.0 project baseline and CS1591 enforcement.
- Add focused unit coverage for concept shape, role coverage, naming/default behavior, and provider-neutral behavior.

### Scope Out
- Schema generation, migrations, loading automation, validation tooling, and provider-specific Sqlite/Postgres behavior.
- Hash algorithm implementation or model-specific hash input normalization beyond referencing the stable hashing contract.
- PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations.
- Runtime configuration APIs, custom option matrices, and advanced override hooks unless a minimal internal shape is necessary to keep the abstractions provider-neutral.
- Changing default naming semantics, persistence convention policy, repository layout, target framework, or package identity.

## Acceptance Criteria
- Hub, link, and satellite abstractions are available in the library and expose documented public or protected API surface for their metadata responsibilities.
- Hub metadata can represent one or more business key columns plus required hash key, load timestamp, and record source technical metadata.
- Link metadata can represent two or more participating hub/key references plus required relationship hash key, load timestamp, and record source technical metadata.
- Satellite metadata can represent a hub or link parent reference, payload columns, hash diff, load timestamp, and record source technical metadata.
- Technical metadata roles cover the closed v1 role set: hash key, hash diff, load timestamp, and record source.
- The abstractions remain provider-neutral and do not depend on Sqlite, Postgres, EF provider-specific APIs, SQL dialect names, migrations, generated columns, sequences, or triggers.
- Tests demonstrate the concept model and technical metadata defaults without requiring a database provider.

## Definition of Done
- All new public/protected APIs introduced for this story have XML documentation and compile under the existing net10.0 project settings.
- Unit tests cover hub, link, satellite, business key, participant, payload, and technical metadata behavior introduced by the story.
- The implementation follows docs/plans/shared-implementation-standards.md, docs/architecture/mvp-data-vault-concepts.md, docs/naming/default-naming-policy.md, docs/plans/stable-hashing-contract.md, and docs/plans/dvault-v1-default-persistence-convention-policy.md where relevant.
- Formatting validation is run with bash tools/check-format.sh, and dotnet test is run through the repository solution or documented test entry point.
- No provider-specific persistence behavior or deferred Data Vault capability is introduced as part of this story.

## Implementation Notes
- Use the current branch state verified through repository tools; do not reuse stale source artifacts from earlier failed format-gate attempts without re-reading the files.
- Use the existing src/DCoding.Data.DVault/Modeling folder and DCoding.Data.DVault.Modeling namespace for modeling abstractions unless a nearby existing type establishes a narrower placement.
- Use the existing TechnicalMetadataColumnRole values as the v1 default role baseline; do not reopen the role set unless a separate governance ticket expands MVP concepts.
- Default table and technical column naming should rely on DefaultNamingPolicy and docs/naming/default-naming-policy.md instead of restating naming rules in each abstraction.
- The abstractions should model metadata and concept relationships only; they should not compute hashes, normalize hash inputs, generate schemas, or call provider APIs.
- Prefer the current Unit/Modeling test layout for new modeling coverage, with additional verification through the repository formatting gate and dotnet test/build entry points where the sandbox permits.

## Open Questions
- none

## Follow-Up Questions
- Decide in later implementation or governance tickets whether the existing child tickets 06EXB74XQJFKGSKVJ6THQWJY8W and 06EXB755X9TGQW2EG1G30GJG28 should remain separate delivery slices or be completed before this parent story closes.
- Plan separate stories for schema generation, loading automation, provider adapters, PIT tables, bridge tables, and multi-active satellites when those capabilities are scheduled.
- Plan a separate API design ticket if advanced configuration hooks need a public options surface beyond the current convention-first defaults.
- If sandbox-specific dotnet/MSBuild IPC failures recur, dev should report them as execution-environment limitations with the exact command and diagnostic, separate from source correctness.

## Risks
- The parent story spans several related modeling concepts, so implementation should keep the first pass narrow and avoid drifting into provider persistence or automation work.
- Existing source already includes technical metadata contract types; developers should preserve that baseline and extend around it rather than creating a competing concept model.
- Hash key and hash diff metadata may be confused with hash computation. This ticket should keep computation and normalization out of scope.
- Format-gate and dotnet verification should be rerun from the current repository state after any implementation repair so earlier failed automation loops are not repeated blindly.

## Split Recommendations
- No new child ticket was created in this pass because existing parentOf relations already show two child tickets under this story.
- If implementation proves too large, split by concept family: hub/business-key metadata, link/participant metadata, and satellite/payload metadata, while keeping the shared technical metadata role set common.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Define the metadata abstractions for hubs, links, satellites, and technical columns.

## Scope
- Represent business keys, link participants, satellite payloads, hash keys, hash diffs, load timestamps, and record source.

## Acceptance Criteria
- Each concept has a documented public/protected API.
- The abstractions are independent from Sqlite or Postgres specifics.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.