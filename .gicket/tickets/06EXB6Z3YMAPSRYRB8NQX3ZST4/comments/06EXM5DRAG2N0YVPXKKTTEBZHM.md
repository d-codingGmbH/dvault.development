[gicket-bot] PO refinement contract

Summary
- PO refinement verified the current ticket, comments, relations, attachments, referenced planning docs, and visible src/DVault evidence. The story is ready for PO-critic; no new planning writes were needed, and existing child relations remain the split structure.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The convention-first v1 baseline is ratified from current repository evidence: public service registration is AddDVault on IServiceCollection, model conventions live under DVault.Modeling, and src/DVault/DVault.csproj is the owning project for this work.
- The minimal path should require no DVault options object, custom naming policy, custom hashing policy, provider selection, migrations, schema generation, or configuration file.
- Existing child tickets 06EXB6ZC4M7Q55PXTFBVWP34S0 and 06EXB6ZMBB97J1Z5TBS29QMGPR are already linked from this story with parentOf relations; no additional split was materialized in this run.
- The incoming blocks relation from 06EXB6QD5Y9XVVZDVZEN4M6EV8 is treated as dependency context, not a PO clarification blocker, because recent relation comments show that upstream PO, PO-critic, dev, and test workflows completed.
- The ticket has no persisted attachments in the current read; the referenced repository planning documents are already accepted as ticket context for this refinement.

Scope In
- Provide and preserve a convention-first service registration entry point for application startup that registers DVault defaults without requiring caller configuration.
- Provide a convention-first model-building entry point that uses the v1 Data Vault defaults for hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.
- Use DefaultNamingPolicy and DataVaultConventions.Default as the v1 default source for naming, model concepts, stable hash identifiers, persistence content hash algorithm, convention version, and logical object names.
- Make optional configuration discoverable through public API documentation or additive overload shape, while keeping every option unset by default for the minimal path.
- Include a minimal example, documentation sample, or test fixture showing a small number of DVault-specific calls for startup plus basic model declaration.

Scope Out
- Provider-specific persistence behavior, SQL dialect mapping, migrations, physical schema generation, and adapter-specific options.
- Full implementation of the optional advanced configuration hook matrix for naming, hashing, record source, timestamp, or provider behavior.
- Runtime data loading, ingestion pipelines, content payload serialization, and persistence execution.
- Deferred Data Vault capabilities such as PIT table generation, bridge table generation, multi-active satellites, and provider-specific optimizations.
- Creating a runnable examples project under examples unless a child ticket explicitly scopes that work.

Open questions
- none

Follow-up questions
- Decide in a later ticket when to implement the full optional advanced configuration hooks for naming, hashing, record source, timestamp, and provider behavior.
- Decide in a later documentation or examples ticket whether to add runnable examples under examples once the public API shape stabilizes.
- Consider a separate repository-layout documentation cleanup if README placeholder paths need reconciliation with the current src/DVault and tests/DVault.Tests branch layout.
- Review the two already-linked child tickets for slice-specific acceptance criteria before development if their descriptions do not already distinguish service registration from model-building work.

Risks
- The story can expand accidentally into provider-specific EF or persistence work because adjacent planning documents mention provider behavior; keep this ticket to public entry points and defaults.
- README layout text still references older reserved project paths while current source evidence uses src/DVault; implementation should follow the current branch baseline unless a separate layout ticket changes it.
- Public entry point names become durable API surface, so tests and XML documentation should cover behavior without adding broad configuration commitments prematurely.

Split recommendations
- No additional child tickets are recommended from this PO refinement because two parentOf child tickets already exist for this story.
- Use the existing child split to keep service-registration work and model-building entry-point work independently reviewable if their current child descriptions support that division.
- Create future follow-up tickets only if advanced configuration hooks, provider-specific adapters, or runnable example projects are intentionally pulled forward.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 6
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment