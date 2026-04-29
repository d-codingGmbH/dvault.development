[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff. The persisted contract has no unresolved Open Questions, gives bounded convention-first scope and acceptance criteria, and the referenced AddDVault/UseDataVault/default-convention public surfaces are directly visible in source.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git rev-parse observed repository root /mnt/c/Projects/DVault, HEAD d39877fb228f724105b5f5aedbcec28d37d3ca2a, branch ticket/06EXB6Z3YMAPSRYRB8NQX3ZST4-story-provide-convention-first-public-entry-poin.
- .gicket/tickets/06EXB6Z3YMAPSRYRB8NQX3ZST4/description.md:7-9 records PO Handoff decision ready_for_po_critic; lines 18-23 scope in startup/model entry points, defaults, optional configuration discoverability, and minimal sample/test fixture; lines 32-38 contain six acceptance criteria; lines 40-46 contain six DoD items; lines 56-57 record Open Questions as '- none'.
- .gicket/relations/T4/S0/06EXB6Z3YMAPSRYRB8NQX3ZST4--06EXB6ZC4M7Q55PXTFBVWP34S0--parentOf.json:3-5 and .gicket/relations/T4/PR/06EXB6Z3YMAPSRYRB8NQX3ZST4--06EXB6ZMBB97J1Z5TBS29QMGPR--parentOf.json:3-5 confirm the two parentOf child relations; .gicket/relations/V8/T4/06EXB6QD5Y9XVVZDVZEN4M6EV8--06EXB6Z3YMAPSRYRB8NQX3ZST4--blocks.json:3-5 confirms the incoming blocks relation.
- Child ticket .gicket/tickets/06EXB6ZC4M7Q55PXTFBVWP34S0/ticket.json:3-8 is done and titled 'Task: Design AddDVault and UseDataVault extension method shape'; its description.md:31-37 defines API-shape AC for AddDVault and UseDataVault.
- Child ticket .gicket/tickets/06EXB6ZMBB97J1Z5TBS29QMGPR/ticket.json:3-8 is done and titled 'Task: Add smoke tests for minimal startup'; its description.md:30-35 defines AddDVault smoke-test AC.
- src/DVault/DVaultServiceCollectionExtensions.cs:16-23 directly defines public AddDVault(this IServiceCollection), null-checks services, registers DefaultNamingPolicy.Instance and DataVaultConventions.Default, and returns the same IServiceCollection.
- src/DVault/Modeling/DataVaultModelBuilderExtensions.cs:13-19 directly defines public UseDataVault(this DataVaultModelBuilder), null-checks the builder, applies DataVaultConventions.Default, and returns the same builder.
- src/DVault/Modeling/DataVaultConventions.cs:14-30 defines the finite default concept set and logical object names; lines 48-57 expose DataVaultConventions.Default using DefaultNamingPolicy.Instance, sha256-v1, sha-256, dvault.persistence-conventions.v1, and the three dvault_* logical objects.
- src/DVault/Modeling/DataVaultModel.cs:21-33 exposes optionless DataVaultModel.Create with optional options; lines 62-103 expose Hub and Link declaration methods; lines 131-287 build hub, satellite, and link tables with hash key, hash diff, load timestamp, and record source technical columns.
- src/DVault/Modeling/DataVaultModelOptions.cs:6-24 exposes optional NamingPolicy configuration and falls back to DefaultDataVaultNamingPolicy.Instance when unset.
- tests/DVault.Tests/Modeling/DefaultNamingPolicyTests.cs:150-193 covers AddDVault API shape, optionless startup provider build, and UseDataVault default conventions; tests/DVault.Tests/Modeling/NamingPolicyTests.cs:37-73 covers deterministic hub/satellite/link model output.
- docs/naming/default-naming-policy.md:21-29 defines hash key, hash diff, load timestamp, and record source column defaults; docs/plans/stable-hashing-contract.md:33-43 defines sha256-v1 default stable hashing; docs/plans/dvault-v1-default-persistence-convention-policy.md:35-43 defines dvault_records, dvault_record_payloads, and dvault_record_metadata logical objects.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocking PO gap. During implementation/test, make sure the public-entry-point coverage visibly includes both the one-call startup path and at least one basic hub/link/satellite model declaration path without custom options.

Risky assumptions
- Optional advanced configuration remains a follow-up boundary. The visible current option surface is naming-only, so dev should not infer that the full advanced hook matrix is in this story.

AC / test suggestions
- Confirm AddDVault null-safety, same-service-collection return, and idempotent behavior when defaults are already registered.
- Confirm UseDataVault null-safety, same-builder return, and application of DataVaultConventions.Default.
- Confirm a basic model-building path produces hub, link, satellite, hash key, hash diff, load timestamp, and record source defaults without provider setup.

Implementation watchouts
- Keep provider-specific persistence, SQL dialects, migrations, schema generation, and adapter options out of this story.
- Keep XML documentation on the public convention-first APIs because src/DVault/DVault.csproj enables GenerateDocumentationFile.
- Use docs/naming/default-naming-policy.md, docs/plans/stable-hashing-contract.md, and docs/plans/dvault-v1-default-persistence-convention-policy.md as source-of-truth defaults rather than reopening those decisions.
- Run the root build/test commands and bash tools/check-format.sh, or document concrete inability, during dev/test roles.

Non-blocking notes
- README.md:7-9 still references reserved src/DCoding.Data.DVault and tests/DCoding.Data.DVault paths while current source is src/DVault; the parent contract already records this as follow-up risk, not a PO handoff blocker.
- I did not run build, test, or formatting gates in this PO-critic pass because the role boundary is read-only and those commands can write build artifacts.

Split recommendations
- No additional PO split is needed. Existing parentOf child tickets 06EXB6ZC4M7Q55PXTFBVWP34S0 and 06EXB6ZMBB97J1Z5TBS29QMGPR already cover API shape and startup smoke-test slices.
- Create future tickets only for advanced configuration hooks, provider-specific adapters, runnable examples, or repository-layout cleanup if those are intentionally pulled forward.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment