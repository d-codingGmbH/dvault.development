[gicket-bot] PO refinement contract

Summary
- PO refinement completed for 06EXB6ZC4M7Q55PXTFBVWP34S0 using persisted ticket state, relation context, attachment metadata, and repository evidence from the current branch. No child tickets or planning documents were created because the implementation scope is bounded enough for one design/API ticket.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 public API should target the visible owning library surface in src/DVault/DVault.csproj, which currently targets net10.0 with nullable and XML documentation enabled.
- Use DVault-facing extension method names AddDVault for startup/service registration and UseDataVault for model configuration because the ticket title already fixes those names and they are directly discoverable through IntelliSense.
- Default behavior must be convention-first and optionless: the default overload must require no custom options, use the existing DefaultNamingPolicy baseline, and preserve deterministic defaults from the referenced persistence and hashing planning documents.
- The parent relationship from 06EXB6Z3YMAPSRYRB8NQX3ZST4 to this ticket already exists; there are no outgoing child-ticket relations and no existing attachments on this ticket.

Scope In
- Define the public extension-method shape for application startup registration via AddDVault, including namespace, owning static class placement, overload intent, and default service behavior.
- Define the public extension-method shape for model configuration via UseDataVault, including namespace, receiver shape, overload intent, and default convention behavior.
- Document how default overloads compose the existing DefaultNamingPolicy, the MVP Data Vault concepts, and the stable hashing contract without requiring caller-provided options.
- Specify XML documentation and IntelliSense expectations for the primary extension methods and overloads.
- Add or update focused tests or design-contract checks that verify the public API shape, optionless default overload availability, namespace discoverability, and default convention wiring where implementation exists.

Scope Out
- Provider-specific persistence adapters, SQL dialect behavior, migrations, schema generation, filesystem/cloud storage, and physical schema decisions.
- Implementing full hash computation, domain field normalization, or security-specific hashing beyond honoring the existing stable hashing contract registration boundary.
- Expanding Data Vault concepts beyond hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.
- Changing the established default naming policy semantics except where a compile-time integration point is needed.
- Runtime workflow metadata changes, label transitions, or handoff status updates.

Open questions
- none

Follow-up questions
- A later packaging/API identity decision should reconcile the README's reserved DCoding.Data.DVault layout with the currently visible src/DVault project and DVault namespace if the published package name changes.
- Provider-specific AddDVault overloads can be added later when SQLite or other persistence adapters are introduced.
- A future ticket should decide whether examples under examples/ should demonstrate minimal AddDVault and UseDataVault usage once the APIs exist.
- A later implementation ticket can decide exact helper class names and option property names within the architecture described here.

Risks
- The repository currently shows both reserved DCoding.Data.DVault layout language and an active src/DVault project, so implementation should avoid baking a package identity migration into this ticket.
- Introducing dependency-injection abstractions before the project has the required package references may expand scope; keep any added dependency limited to what AddDVault actually needs.
- Making provider choices part of the default overload would conflict with the provider-neutral planning documents and should be avoided.

Split recommendations
- No split is recommended for this ticket. The evidence supports a bounded API design task covering AddDVault and UseDataVault together; provider adapters, examples, and package identity cleanup should remain separate follow-up work.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment