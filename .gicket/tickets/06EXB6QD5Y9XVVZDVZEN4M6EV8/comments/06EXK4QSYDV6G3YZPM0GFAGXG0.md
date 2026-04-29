[gicket-bot] PO-critic review contract

Summary
- The persisted contract is ready for handoff: it has no open PO questions, scopes this as a product-principle story with no product-code changes, and is supported by ticket relations, completed child work, direct source evidence for the current public defaults, and planning docs.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB6QD5Y9XVVZDVZEN4M6EV8/description.md:7-9 records PO Handoff decision ready_for_po_critic; lines 54-55 record Open Questions as '- none'.
- .gicket/tickets/06EXB6QD5Y9XVVZDVZEN4M6EV8/description.md:17-29 scopes in convention-first zero configuration and deterministic defaults while scoping out new runtime APIs, providers, schema generation, migrations, hashing code, provider-specific behavior, and advanced Data Vault capabilities.
- .gicket/tickets/06EXB6QD5Y9XVVZDVZEN4M6EV8/description.md:31-44 contains six acceptance criteria and five DoD items, including no custom configuration before first use, optional additive advanced configuration, referenced defaults, and no product code changes required.
- .gicket/relations/V8/T4/06EXB6QD5Y9XVVZDVZEN4M6EV8--06EXB6Z3YMAPSRYRB8NQX3ZST4--blocks.json:3-5 links this story as blocking downstream public-entry-point story 06EXB6Z3YMAPSRYRB8NQX3ZST4; that ticket exists at .gicket/tickets/06EXB6Z3YMAPSRYRB8NQX3ZST4/ticket.json:3-8.
- .gicket/tickets/06EXB6QD5Y9XVVZDVZEN4M6EV8/comments/06EXK3552JPYNGTC35ETYSBPFW.md:6-14 records the PO refinement handoff, repository-evidence summary, completed child outcomes, and downstream public-entry-point boundary; lines 30-36 record open questions none and follow-up questions.
- git rev-parse --abbrev-ref HEAD returned ticket/06EXB6QD5Y9XVVZDVZEN4M6EV8-story-define-the-minimal-configuration-product-p and git rev-parse HEAD returned 0864b2252ac643bd08643cfee6dbe551d0b63d79, matching the provided scratch-source-ref.
- git diff --name-status develop...HEAD listed only .gicket ticket/comment/event/json changes for this ticket and related relations; no src/ or docs/ product implementation files are changed on the review branch.
- src/DVault/DVaultServiceCollectionExtensions.cs:16-23 defines optionless AddDVault(IServiceCollection), registers DefaultNamingPolicy.Instance and DataVaultConventions.Default, and returns the same IServiceCollection.
- src/DVault/Modeling/DataVaultModel.cs:21-33 defines DataVaultModel.Create(Action<DataVaultModelBuilder>, Action<DataVaultModelOptions>? configureOptions = null), creates default options, invokes optional configuration, and builds the model.
- src/DVault/Modeling/DataVaultConventions.cs:10-29 and 51-57 define DataVaultConventions.Default with sha256-v1, sha-256, dvault.persistence-conventions.v1, MVP concepts Hub/Link/Satellite/HashKey/HashDiff/LoadTimestamp/RecordSource, and logical objects dvault_records, dvault_record_payloads, dvault_record_metadata.
- src/DVault/Modeling/DataVaultModelBuilderExtensions.cs:13-19 defines optionless UseDataVault that applies DataVaultConventions.Default.
- docs/plans/optional-advanced-configuration-hooks.md:9-11 says the hook plan keeps the normal path convention-first and zero-configuration without requiring runtime implementation; lines 23 and 33-39 say every hook is optional and unset hooks inherit deterministic defaults.
- docs/naming/default-naming-policy.md:3 says default naming applies when no model naming configuration is supplied and is provider-neutral/deterministic; lines 68-80 document the public DefaultNamingPolicy API.
- docs/plans/dvault-v1-default-persistence-convention-policy.md:5-7 defines logical defaults and required preservation by adapters; lines 35-45 define default logical objects dvault_records, dvault_record_payloads, and dvault_record_metadata.
- docs/plans/stable-hashing-contract.md:33-43 defines default v1 hashing with AlgorithmId sha256-v1, UTF-8 input without BOM, deterministic output, and no salts/timestamps/culture-dependent side effects.
- docs/architecture/mvp-data-vault-concepts.md:3-5 limits MVP concepts to hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources; docs/plans/deferred-data-vault-capabilities.md:5-7 and 19-26 keep PIT, bridge, multi-active, and provider-specific optimizations outside MVP.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The exact canonical quickstart snippet is not specified in this ticket; the contract records it as a follow-up for downstream story 06EXB6Z3YMAPSRYRB8NQX3ZST4, so it is not a current PO blocker.

Risky assumptions
- Downstream public-entry-point work must preserve the contract's assumption that first use does not require provider selection, configuration files, custom naming, custom hashing, or timestamp setup.
- Source evidence shows DataVaultConventions.Default uses DefaultNamingPolicy.Instance while DataVaultModelOptions.ResolveNamingPolicy() currently falls back to DefaultDataVaultNamingPolicy.Instance; downstream entry-point refinement should verify the intended default-policy binding before relying on model output shape.

AC / test suggestions
- For downstream entry-point work, add acceptance coverage that a first-use example can call AddDVault() and create/build a simple model without provider tuning, configuration files, custom naming, custom hashing, or timestamp setup.
- For downstream documentation or tests, keep advanced options after the default path and assert unset hooks inherit documented v1 defaults.

Implementation watchouts
- Do not reopen the completed default convention policy, optional hook plan, default naming policy, stable hashing contract, or MVP/deferred capability boundaries in this story.
- Use src/DVault and tests/DVault.Tests as the visible current evidence surfaces, while avoiding unrelated DCoding.Data.DVault layout churn noted in the contract.
- Keep advanced configuration grouped by responsibility and independently overridable so configuring one hook category does not force users to restate defaults for unrelated categories.

Non-blocking notes
- git status showed unrelated modified .gicket/.gicket-bot files outside this ticket's branch diff; the review used read-only evidence from HEAD 0864b2252ac643bd08643cfee6dbe551d0b63d79 and targeted ticket/source/doc files.
- No new split is needed at this story level because both planning children are done and the remaining public-entry-point work already exists as downstream story 06EXB6Z3YMAPSRYRB8NQX3ZST4.

Split recommendations
- No additional split recommended for this ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment