[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the persisted contract has no open questions, the governing decision record already exists, and current source/doc evidence matches the claimed opt-in deferred-capability boundary.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NSBM3GD7DY11Y4PZMXD28/description.md:8 sets PO handoff to `ready_for_po_critic`, and :48-49 shows `## Open Questions` -> `- none`.
- docs/plans/deferred-data-vault-capabilities.md:15-26 and :79-111 already ratify the baseline: `AddDVault()` stays optionless, `UseDataVault()` / `ApplyDataVaultMetadata()` stay convention-first, PIT/bridge/multi-active/hooks are opt-in, and provider-specific behavior remains outside the core story.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:29-40 and :43-229 only create hub, link, and satellite EF projections; no PIT, bridge, multi-active, or hook projection surface is present in the current baseline.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:10-18 and :45-72 default `UseDataVault()` to `DataVaultProviderCapabilityProfiles.Sqlite` and route projection through `ApplyDataVaultMetadata(...)`.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs:19-43 keeps provider-profile registration separate and falls back to `DataVaultProviderCapabilityProfiles.Sqlite` when no provider name is registered.
- docs/architecture/dvault-v1-explicit-save-service.md:31-37 and provider startup extensions at src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-29, src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-25, src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15-20, src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:15-20, and src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:15-20 show provider-specific save behavior/profile registration stays in provider packages rather than this core architecture story.
- Child/dependency relations are already materialized in .gicket/relations/28/JM/06EZ0NSBM3GD7DY11Y4PZMXD28--06EZ0NSHJVC9SD2KS6PWWNHPJM--parentOf.json, .gicket/relations/28/A0/06EZ0NSBM3GD7DY11Y4PZMXD28--06EZ0NSQFCD3W4CDCJ44GFSKA0--parentOf.json, and the four `blocks` relations under .gicket/relations/28/{70,CC,YG,2R}/.
- .gicket/tickets/06EZ0NSHJVC9SD2KS6PWWNHPJM/ticket.json:7 and .gicket/tickets/06EZ0NSQFCD3W4CDCJ44GFSKA0/ticket.json:7 are `done`, satisfying the story's cited decision-record and API-guardrail children.
- Branch-history check: `git rev-parse HEAD` returned `6d26230e7def922526c0e10dba3c5fa146dd994e` and `git diff --name-only 6d26230e7def922526c0e10dba3c5fa146dd994e..HEAD -- [governing docs/core files]` returned `DIFF:no-paths`, so the current branch already contains the cited architecture evidence without further repo changes.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract intentionally does not include a worked example of a single hook category overriding its own boundary while the other categories stay on defaults; the hooks owner story should supply that example when it refines implementation scope.
- The contract does not include a concrete provider-package example for a deferred capability declining provider-specific handling and falling back to the provider-neutral path; that example belongs in the owning provider/capability story, not this architecture anchor.

Risky assumptions
- Approval assumes README-level discoverability is optional for v0.5; `rg` against README.md returned `README:no-matches` for the deferred-capability and advanced-hooks documents.
- Approval assumes future internal-only deferred-capability changes will consistently carry the explicit no-public-contract note required by the done API guardrail ticket, because that guardrail is now a per-owner-story review rule rather than standalone dev work.

AC / test suggestions
- Require each implementing deferred-capability story to state either `public API changed` with a same-change snapshot diff or `no public contract` with no changes under `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/`.
- Keep downstream acceptance criteria explicit that unset hooks preserve deterministic naming, hashing, record source, timestamp, and provider defaults exactly as restated in `docs/plans/deferred-data-vault-capabilities.md` and `docs/plans/optional-advanced-configuration-hooks.md`.
- Have downstream owner stories cite the existing `blocks` relation back to this architecture story so reviewers can reject scope creep that tries to reopen the preserved baseline.

Implementation watchouts
- Do not treat this approval as permission to widen the baseline public surface or to add PIT/bridge/multi-active/hook runtime behavior under the current story; the contract is architecture-only.
- Do not let deferred-capability work change the ordinary `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, or `IDataVaultSaveService` path unless a later owning story also clears the explicit API compatibility guardrail.
- Keep provider-specific save behavior and provider-name capability-profile handling in provider packages/save strategies, not in the core architecture story.

Non-blocking notes
- The child API snapshot task has its own resolved contract with `## Open Questions` -> `none` in .gicket/tickets/06EZ0NSQFCD3W4CDCJ44GFSKA0/description.md, so this story does not depend on unresolved compatibility-governance prose.
- Downstream implementation stories remain separate work items; approving this story only confirms the architecture anchor is ready for developer handoff.

Split recommendations
- No additional split. The decomposition is already materialized through the two `parentOf` children and the four downstream `blocks` stories.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment