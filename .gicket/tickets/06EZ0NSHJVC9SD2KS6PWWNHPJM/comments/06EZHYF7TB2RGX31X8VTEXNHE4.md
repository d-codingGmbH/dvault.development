[gicket-bot] PO-critic review contract

Summary
- Ticket 06EZ0NSHJVC9SD2KS6PWWNHPJM is sufficiently refined for developer handoff: scope is bounded to publishing one architecture record, preserved baseline behavior is directly evidenced in local source and docs, downstream ownership is already materialized, and the persisted contract has no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `docs/plans/deferred-data-vault-capabilities.md:5-13,17-29` explicitly keeps PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations outside the MVP baseline and says they must not block the first package.
- `docs/plans/optional-advanced-configuration-hooks.md:9-39,157-165` defines naming, hashing, record source, timestamp, and provider behavior as optional additive hooks with zero-configuration defaults and no concrete API commitments.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:11-25` makes `AddDVault()` optionless, and `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:10-18,45-71` defaults `UseDataVault()` to `DataVaultProviderCapabilityProfiles.Sqlite` and routes `ApplyDataVaultMetadata()` through the same convention-first path.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:29-40,43-229` only creates hub, link, and satellite projections; no PIT, bridge, or multi-active projection path is present in the current translator.
- `docs/architecture/mvp-data-vault-concepts.md:3-15,23-50` and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md:9-17,47-58` preserve the SQLite-oriented MVP and example baseline that the ticket says must remain valid.
- `docs/architecture/dvault-v1-explicit-save-service.md:8-27,37-42` preserves the explicit `IDataVaultSaveService` boundary and separates provider-specific save-strategy concerns from the core compatibility baseline the new record must reference rather than redefine.
- Persisted relations already match the contract: `.gicket/relations/28/JM/06EZ0NSBM3GD7DY11Y4PZMXD28--06EZ0NSHJVC9SD2KS6PWWNHPJM--parentOf.json` places this task under the parent story, `.gicket/relations/JM/A0/06EZ0NSHJVC9SD2KS6PWWNHPJM--06EZ0NSQFCD3W4CDCJ44GFSKA0--blocks.json` blocks the API snapshot task, and epic child relations exist under `.gicket/relations/F0/{28,70,CC,YG,2R}/...` for the wider decomposition.
- `git diff --stat f00382072223be774784f1c811c5a6874e34e4fb..e758efbb8fd4 -- .gicket/tickets/06EZ0NSHJVC9SD2KS6PWWNHPJM docs README.md src` changed only `.gicket/tickets/06EZ0NSHJVC9SD2KS6PWWNHPJM/*`, and `git log` shows HEAD `e7bd441f` is only the current PO-critic lease-claim commit, so the branch remains a ticket-refinement handoff rather than partially implemented doc work.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not give a sample sentence for how the published record should describe unsupported advanced shapes versus deferred future API depth; this is survivable because the acceptance criteria already require that distinction.
- The follow-up questions leave README linking and post-publication handling of `docs/plans/deferred-data-vault-capabilities.md` open as optional cleanup choices rather than part of the core deliverable.

Risky assumptions
- The developer will treat the publication surface choice between `docs/plans/` and `docs/architecture/` as implementation judgment, because the contract allows either and does not nominate one path.
- The developer will treat README linking and narrowing the older deferred-capabilities note as non-blocking follow-up work, because those topics appear under `## Follow-Up Questions` rather than `## Open Questions` or Acceptance Criteria.
- The developer will keep provider-specific optimization discussion referential only; `docs/architecture/dvault-v1-explicit-save-service.md` already owns that boundary and the new record should not silently broaden it.

AC / test suggestions
- At review time, verify that the published record explicitly cites `docs/plans/deferred-data-vault-capabilities.md`, `docs/plans/optional-advanced-configuration-hooks.md`, `docs/architecture/mvp-data-vault-concepts.md`, and `docs/architecture/dvault-v1-explicit-save-service.md` rather than paraphrasing them loosely.
- Check that the finished record names the downstream owners by ticket id or unmistakable capability area: PIT `06EZ0NSXY2Y1JZ8SSCX177C770`, bridge `06EZ0NTV4SVAKV98C418T8A3CC`, multi-active `06EZ0NVN71BN0QWJDCWGVZ2PYG`, hooks `06EZ0NWKC9ZME5BSCJFSQEQ02R`, and blocked API snapshot task `06EZ0NSQFCD3W4CDCJ44GFSKA0`.
- Confirm the published text states that ordinary `AddDVault()` / `UseDataVault()` / `ApplyDataVaultMetadata()` usage and SQLite-oriented examples remain valid without new configuration.

Implementation watchouts
- Do not let the decision record promise concrete hook method names, stable public API shape, or automation depth beyond the currently visible baseline; `docs/plans/optional-advanced-configuration-hooks.md:11,157-165` explicitly defers that.
- Do not blur deferred capability architecture with provider-optimization release posture; `docs/plans/deferred-data-vault-capabilities.md:26-29` and `docs/architecture/dvault-v1-explicit-save-service.md:37-42` keep those concerns separate.
- Keep the preserved baseline explicit: the current model-projection evidence is hub-link-satellite only, the default capability profile is SQLite, and the caller boundary remains convention-first plus explicit save service.

Non-blocking notes
- A bounded repo search currently finds only the two planning inputs in `docs/` for this topic, so the developer is authoring a net-new governing record rather than updating an existing published decision record.

Split recommendations
- No additional split is warranted; the parent, blocked, and epic-child relations are already materialized locally and the contract correctly uses this ticket as the publication anchor.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment