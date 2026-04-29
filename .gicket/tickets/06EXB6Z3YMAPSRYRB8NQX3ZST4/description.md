<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- PO refinement verified the current ticket, comments, relations, attachments, referenced planning docs, and visible src/DVault evidence. The story is ready for PO-critic; no new planning writes were needed, and existing child relations remain the split structure.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The convention-first v1 baseline is ratified from current repository evidence: public service registration is AddDVault on IServiceCollection, model conventions live under DVault.Modeling, and src/DVault/DVault.csproj is the owning project for this work.
- The minimal path should require no DVault options object, custom naming policy, custom hashing policy, provider selection, migrations, schema generation, or configuration file.
- Existing child tickets 06EXB6ZC4M7Q55PXTFBVWP34S0 and 06EXB6ZMBB97J1Z5TBS29QMGPR are already linked from this story with parentOf relations; no additional split was materialized in this run.
- The incoming blocks relation from 06EXB6QD5Y9XVVZDVZEN4M6EV8 is treated as dependency context, not a PO clarification blocker, because recent relation comments show that upstream PO, PO-critic, dev, and test workflows completed.
- The ticket has no persisted attachments in the current read; the referenced repository planning documents are already accepted as ticket context for this refinement.

### Scope In
- Provide and preserve a convention-first service registration entry point for application startup that registers DVault defaults without requiring caller configuration.
- Provide a convention-first model-building entry point that uses the v1 Data Vault defaults for hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.
- Use DefaultNamingPolicy and DataVaultConventions.Default as the v1 default source for naming, model concepts, stable hash identifiers, persistence content hash algorithm, convention version, and logical object names.
- Make optional configuration discoverable through public API documentation or additive overload shape, while keeping every option unset by default for the minimal path.
- Include a minimal example, documentation sample, or test fixture showing a small number of DVault-specific calls for startup plus basic model declaration.

### Scope Out
- Provider-specific persistence behavior, SQL dialect mapping, migrations, physical schema generation, and adapter-specific options.
- Full implementation of the optional advanced configuration hook matrix for naming, hashing, record source, timestamp, or provider behavior.
- Runtime data loading, ingestion pipelines, content payload serialization, and persistence execution.
- Deferred Data Vault capabilities such as PIT table generation, bridge table generation, multi-active satellites, and provider-specific optimizations.
- Creating a runnable examples project under examples unless a child ticket explicitly scopes that work.

## Acceptance Criteria
- A minimal .NET consumer can register DVault defaults with one startup-level DVault call and can declare a basic Data Vault model without providing custom options.
- The optionless path uses deterministic v1 defaults from the documented naming policy, persistence convention policy, stable hashing contract, and DataVaultConventions.Default.
- Service registration is null-safe, fluent, and idempotent with respect to already-registered DVault services so host startup composition remains predictable.
- The model-building entry point applies defaults for hub, link, satellite, hash key, hash diff, load timestamp, and record source concepts without provider-specific setup.
- Optional configuration is visible to implementers and consumers but unset options inherit defaults and do not force ordinary users to restate default conventions.
- Tests or executable examples cover the zero-configuration startup path and at least one basic model-building path using the public entry points.

## Definition of Done
- Implementation and tests are limited to the current src/DVault and corresponding visible test layout unless an already-linked child ticket narrows the slice further.
- Public XML documentation explains the convention-first path and states that advanced options are optional.
- Root build and test commands appropriate for the branch succeed, or any unavailable command is documented with the concrete reason.
- The repository formatting gate from shared implementation standards is run or the inability to run it is documented.
- No product code introduces provider-specific persistence promises, migrations, schema generation, or advanced hook behavior beyond this story scope.
- The minimal example remains aligned with the package identity DCoding.Data.DVault and the net10.0 baseline in src/DVault/DVault.csproj.

## Implementation Notes
- Follow the existing public namespace pattern: startup extensions in DVault and modeling concepts in DVault.Modeling.
- Ratify AddDVault(IServiceCollection) as the service registration entry point already visible in the branch; keep it optionless and returning the same IServiceCollection for fluent startup composition.
- Align the model-building entry point with the existing DataVaultConventions comment that names AddDVault and UseDataVault as the shared default consumers.
- Use the existing DataVaultMetadata validation posture for null, empty, and structurally invalid hub, link, and satellite declarations; exact helper names are implementation details.
- Keep the finite v1 concept set from DataVaultConventions as the bounded default: Hub, Link, Satellite, HashKey, HashDiff, LoadTimestamp, and RecordSource.
- Do not reopen naming or persistence logical-object defaults; use docs/naming/default-naming-policy.md and docs/plans/dvault-v1-default-persistence-convention-policy.md as source of truth.

## Open Questions
- none

## Follow-Up Questions
- Decide in a later ticket when to implement the full optional advanced configuration hooks for naming, hashing, record source, timestamp, and provider behavior.
- Decide in a later documentation or examples ticket whether to add runnable examples under examples once the public API shape stabilizes.
- Consider a separate repository-layout documentation cleanup if README placeholder paths need reconciliation with the current src/DVault and tests/DVault.Tests branch layout.
- Review the two already-linked child tickets for slice-specific acceptance criteria before development if their descriptions do not already distinguish service registration from model-building work.

## Risks
- The story can expand accidentally into provider-specific EF or persistence work because adjacent planning documents mention provider behavior; keep this ticket to public entry points and defaults.
- README layout text still references older reserved project paths while current source evidence uses src/DVault; implementation should follow the current branch baseline unless a separate layout ticket changes it.
- Public entry point names become durable API surface, so tests and XML documentation should cover behavior without adding broad configuration commitments prematurely.

## Split Recommendations
- No additional child tickets are recommended from this PO refinement because two parentOf child tickets already exist for this story.
- Use the existing child split to keep service-registration work and model-building entry-point work independently reviewable if their current child descriptions support that division.
- Create future follow-up tickets only if advanced configuration hooks, provider-specific adapters, or runnable example projects are intentionally pulled forward.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Design first-use APIs that do not require extensive configuration.

## Scope
- Provide service registration and model-building entry points.
- Keep defaults usable for simple examples.

## Acceptance Criteria
- A minimal example has only a small number of DVault-specific calls.
- Options are discoverable but optional.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.