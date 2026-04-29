<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- PO refinement completed for 06EXB6ZC4M7Q55PXTFBVWP34S0 using persisted ticket state, relation context, attachment metadata, and repository evidence from the current branch. No child tickets or planning documents were created because the implementation scope is bounded enough for one design/API ticket.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 public API should target the visible owning library surface in src/DVault/DVault.csproj, which currently targets net10.0 with nullable and XML documentation enabled.
- Use DVault-facing extension method names AddDVault for startup/service registration and UseDataVault for model configuration because the ticket title already fixes those names and they are directly discoverable through IntelliSense.
- Default behavior must be convention-first and optionless: the default overload must require no custom options, use the existing DefaultNamingPolicy baseline, and preserve deterministic defaults from the referenced persistence and hashing planning documents.
- The parent relationship from 06EXB6Z3YMAPSRYRB8NQX3ZST4 to this ticket already exists; there are no outgoing child-ticket relations and no existing attachments on this ticket.

### Scope In
- Define the public extension-method shape for application startup registration via AddDVault, including namespace, owning static class placement, overload intent, and default service behavior.
- Define the public extension-method shape for model configuration via UseDataVault, including namespace, receiver shape, overload intent, and default convention behavior.
- Document how default overloads compose the existing DefaultNamingPolicy, the MVP Data Vault concepts, and the stable hashing contract without requiring caller-provided options.
- Specify XML documentation and IntelliSense expectations for the primary extension methods and overloads.
- Add or update focused tests or design-contract checks that verify the public API shape, optionless default overload availability, namespace discoverability, and default convention wiring where implementation exists.

### Scope Out
- Provider-specific persistence adapters, SQL dialect behavior, migrations, schema generation, filesystem/cloud storage, and physical schema decisions.
- Implementing full hash computation, domain field normalization, or security-specific hashing beyond honoring the existing stable hashing contract registration boundary.
- Expanding Data Vault concepts beyond hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.
- Changing the established default naming policy semantics except where a compile-time integration point is needed.
- Runtime workflow metadata changes, label transitions, or handoff status updates.

## Acceptance Criteria
- A developer can discover the primary startup API as AddDVault from the library's intended public namespace without knowing internal implementation classes.
- A developer can discover the primary model configuration API as UseDataVault from the library's intended public namespace or modeling namespace without provider-specific setup.
- At least one AddDVault overload and one UseDataVault overload require no custom options object, delegate, provider, or caller-supplied naming policy.
- Default overload behavior is deterministic and uses the existing v1 defaults: DefaultNamingPolicy for table/column names, MVP Data Vault concepts for model vocabulary, and stable hashing defaults where hashing services are registered.
- Optional configuration overloads, if introduced, are additive and do not make the optionless path ambiguous or harder to find in IntelliSense.
- The API design avoids provider-specific names, environment-specific defaults, and deployment-specific identifiers in the public extension-method contract.

## Definition of Done
- The design or implementation identifies the namespaces, extension receiver types, method names, overloads, default behavior, and XML documentation expectations for AddDVault and UseDataVault.
- The default no-options path is represented in code or a durable planning/design artifact and is covered by focused tests or API-shape assertions when code is added.
- The public API shape compiles under the visible net10.0 library project and follows nullable-enabled C# conventions.
- Repository formatting expectations from docs/formatting.md are preserved for any changed files.
- The implementation remains aligned with docs/naming/default-naming-policy.md, docs/architecture/mvp-data-vault-concepts.md, docs/plans/stable-hashing-contract.md, and docs/plans/dvault-v1-default-persistence-convention-policy.md.

## Implementation Notes
- Repository evidence shows the current concrete library root is src/DVault with namespace DVault.Modeling already used by DefaultNamingPolicy; prefer adding public API surfaces under DVault and DVault.Modeling unless a later rename ticket moves the package to DCoding.Data.DVault.
- Treat AddDVault as the startup/service-registration extension. In a typical .NET shape this should extend IServiceCollection once Microsoft.Extensions.DependencyInjection is introduced, but the ticket should not force a provider-specific dependency if the project has not added that package yet.
- Treat UseDataVault as the model-configuration extension. It should expose Data Vault modeling vocabulary and wire default conventions without requiring callers to understand DefaultNamingPolicy internals.
- Default options should be deterministic, culture-invariant, and provider-neutral. Do not use timestamps, current directory, machine identifiers, random values, process-local salts, or provider names as implicit defaults.
- Keep configuration overloads small: an optional configure delegate or options object is acceptable, but the no-argument overload is the primary acceptance path.
- XML documentation should make the two public entry points searchable and self-explanatory in IntelliSense without duplicating long architecture documents.

## Open Questions
- none

## Follow-Up Questions
- A later packaging/API identity decision should reconcile the README's reserved DCoding.Data.DVault layout with the currently visible src/DVault project and DVault namespace if the published package name changes.
- Provider-specific AddDVault overloads can be added later when SQLite or other persistence adapters are introduced.
- A future ticket should decide whether examples under examples/ should demonstrate minimal AddDVault and UseDataVault usage once the APIs exist.
- A later implementation ticket can decide exact helper class names and option property names within the architecture described here.

## Risks
- The repository currently shows both reserved DCoding.Data.DVault layout language and an active src/DVault project, so implementation should avoid baking a package identity migration into this ticket.
- Introducing dependency-injection abstractions before the project has the required package references may expand scope; keep any added dependency limited to what AddDVault actually needs.
- Making provider choices part of the default overload would conflict with the provider-neutral planning documents and should be avoided.

## Split Recommendations
- No split is recommended for this ticket. The evidence supports a bounded API design task covering AddDVault and UseDataVault together; provider adapters, examples, and package identity cleanup should remain separate follow-up work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Design the primary extension methods used by applications.

## Scope
- Define naming, namespaces, overloads, and default behavior for startup and model configuration.

## Acceptance Criteria
- API names are easy to find from IntelliSense.
- The default overload requires no custom options.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.