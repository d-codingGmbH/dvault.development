[gicket-bot] PO-critic review contract

Summary
- Ticket refinement is ready for developer handoff. The persisted contract has no open questions, identifies the intended API names and constraints, and the local repository evidence supports the stated current surface and known risks.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted delivery contract .gicket/tickets/06EXB6ZC4M7Q55PXTFBVWP34S0/description.md lines 31-44 defines acceptance criteria and DoD for AddDVault, UseDataVault, no-options overloads, deterministic defaults, API-shape compilation, and alignment with referenced docs.
- The same contract lines 54-55 records ## Open Questions as '- none', so approve_for_dev is not blocked by unresolved open questions.
- src/DVault/DVault.csproj lines 1-8 targets net10.0 with Nullable enable and GenerateDocumentationFile true, matching the contract's target library surface.
- src/DVault/Modeling/DefaultNamingPolicy.cs lines 4-9 define namespace DVault.Modeling and public sealed class DefaultNamingPolicy; lines 60-63 expose DefaultNamingPolicy.Instance.
- docs/naming/default-naming-policy.md lines 68-84 documents DVault.Modeling.DefaultNamingPolicy and its public naming methods; tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs lines 41-47 and 96-103 cover Data Vault table prefixes and technical column names.
- docs/architecture/mvp-data-vault-concepts.md lines 3-15 and 17-72 limit MVP vocabulary to hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.
- docs/plans/stable-hashing-contract.md lines 13-22 documents IStableHashService and StableHashDigest as the planned public hash boundary, and lines 85-94 documents expected replacement and registration behavior.
- docs/plans/dvault-v1-default-persistence-convention-policy.md lines 17-33 and 35-45 define deterministic provider-neutral logical defaults and required dvault_* logical object names.
- README.md lines 7-14 still reserves DCoding.Data.DVault layout, while repository inspection found active src/DVault and src/DVault/Modeling; the ticket explicitly captures this as a follow-up/risk instead of making it part of this scope.
- rg over src/DVault and tests/DVault.Tests found DefaultNamingPolicy but no AddDVault, UseDataVault, IStableHashService, StableHashDigest, IServiceCollection, Microsoft.Extensions.DependencyInjection, or PackageReference occurrences in source, confirming this is an API design/implementation handoff rather than already-existing API surface.
- .gicket/relations/T4/S0/06EXB6Z3YMAPSRYRB8NQX3ZST4--06EXB6ZC4M7Q55PXTFBVWP34S0--parentOf.json records parentOf from 06EXB6Z3YMAPSRYRB8NQX3ZST4 to this ticket; rg found no other relation file containing this ticket id.
- find .gicket/tickets/06EXB6ZC4M7Q55PXTFBVWP34S0/attachments returned 'No such file or directory', matching the contract statement that this ticket has no existing attachments.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The stable hashing public types are documented in docs/plans/stable-hashing-contract.md but are not present in src/DVault today; implementation should treat them as a design contract unless source types are introduced in this ticket's implementation.
- The AddDVault receiver shape depends on whether DI abstractions are introduced; current src/DVault/DVault.csproj has no PackageReference and rg found no IServiceCollection/Microsoft.Extensions.DependencyInjection usage, so dependency introduction must stay minimal and justified by the API shape.
- Package identity remains split between README reserved DCoding.Data.DVault layout and active src/DVault/DVault.csproj; the ticket correctly defers identity cleanup, so dev should not fold package migration into this work.

AC / test suggestions
- Add focused API-shape checks for namespace discoverability, one optionless AddDVault overload, one optionless UseDataVault overload, and optional overloads not obscuring the no-options path.
- When implementation exists, assert default convention wiring uses DefaultNamingPolicy.Instance, MVP vocabulary, and stable hashing registration defaults without provider-specific setup.
- Keep compile, nullable, XML documentation, and formatting checks aligned with the visible net10.0 project and docs/formatting.md.

Implementation watchouts
- Keep AddDVault and UseDataVault provider-neutral and avoid SQL, SQLite, environment, deployment, machine, timestamp, random, or process-local defaults in the public contract.
- Use DVault and DVault.Modeling as the intended current public namespaces unless a separate package-identity ticket changes the project layout.
- Do not expand scope into provider adapters, migrations, full hash computation, normalization, examples, or runtime workflow metadata.

Non-blocking notes
- No split is needed for PO handoff; provider-specific overloads, examples, and package identity reconciliation are already listed as follow-ups in the persisted contract.
- There are unrelated dirty .gicket files in git status, but the target ticket files are represented by the current branch commits and the review was read-only.

Split recommendations
- No split recommended for this ticket; keep provider adapters, examples, and package identity cleanup as separate follow-up work as the contract already states.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment