<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository and ticket evidence already ratify the deferred-capability architecture boundary, so this story can advance to PO critic without new splits or relation changes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- `docs/plans/deferred-data-vault-capabilities.md` is already the governing decision record for this story's architecture stance, and child task `06EZ0NSHJVC9SD2KS6PWWNHPJM` is already done.
- Current source and docs fix the preserved v0.5 default baseline: hubs, links, satellites, deterministic naming and hashing, required record source lineage, UTC load-timestamp defaults, optionless `AddDVault()`, convention-first `UseDataVault()` and `ApplyDataVaultMetadata()`, explicit `IDataVaultSaveService`, and SQLite as the default capability profile.
- Deferred PIT, bridge, multi-active, and advanced-hook work is already decomposed into existing downstream tickets; no additional child tickets, relation writes, attachments, or planning documents were created in this refinement pass.
- Provider-specific optimization and storage behavior stay in provider packages and provider strategy or capability-profile contracts rather than in this core deferred-capability architecture story.

### Scope In
- Ratify the architecture boundary that deferred capabilities extend around the current hub, link, and satellite baseline instead of changing default DVault setup.
- Define the release-level posture for PIT tables, bridge tables, multi-active satellites, and advanced hooks as opt-in capability families with deterministic inherited defaults when hooks are unset.
- Set public-surface guardrails: preserve the current visible public baseline and require any new deferred-capability or hook surface to pass API snapshot review or carry an explicit compatibility note before it is treated as stable.
- Identify which provider-specific behavior belongs in provider packages or provider save strategies rather than the core package.

### Scope Out
- Implementing PIT, bridge, multi-active, or hook runtime behavior in product code.
- Finalizing concrete class names, method names, option object shapes, or complete public APIs for deferred capabilities in this story.
- Changing ordinary `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, or `IDataVaultSaveService` baseline behavior to accommodate deferred features.
- Provider-specific DDL, native SQL, migrations, performance tuning, or option matrices beyond the existing provider-boundary documentation.

## Acceptance Criteria
- The story ratifies `docs/plans/deferred-data-vault-capabilities.md` as the governing architecture record for PIT, bridge, multi-active, and advanced-hook extension boundaries.
- The refined contract states that deferred capabilities are opt-in and must not change default hub, link, and satellite modeling, deterministic naming and hashing, required record source lineage, UTC load-timestamp semantics, or ordinary zero-configuration setup unless an explicitly configured hook category overrides its own boundary.
- The contract identifies the stable baseline public surface for this release as the current convention-first registration, modeling, and save-service path, while any new deferred-capability or hook API remains compatibility-reviewed through child task `06EZ0NSQFCD3W4CDCJ44GFSKA0` before being treated as stable public API.
- The contract states that provider-specific behavior for deferred capabilities belongs in provider packages or provider save strategies and capability profiles, not in the core architecture story.
- The contract gives enough architectural guidance for PIT `06EZ0NSXY2Y1JZ8SSCX177C770`, bridge `06EZ0NTV4SVAKV98C418T8A3CC`, multi-active `06EZ0NVN71BN0QWJDCWGVZ2PYG`, and hooks `06EZ0NWKC9ZME5BSCJFSQEQ02R` work to proceed without conflicting designs.

## Definition of Done
- The ticket contract points to the published decision record and does not reopen the already-ratified opt-in architecture stance.
- Downstream teams can tell from the contract which defaults are preserved, which extension categories are deferred, and where provider-specific behavior must live.
- Any future public deferred-capability or hook surface is explicitly routed through API snapshot review or an explicit compatibility note instead of being treated as implicitly stable.
- No new PO-level blockers remain for the existing deferred-capability child tickets.

## Implementation Notes
- Use repository evidence as the baseline contract: `DataVaultEfMetadataTranslator` currently projects only hubs, links, and satellites; `DataVaultModelBuilderExtensions` defaults to `DataVaultProviderCapabilityProfiles.Sqlite`; and `DataVaultProviderCapabilityProfileSelection` keeps provider selection separate from the core modeling path.
- Treat `docs/plans/optional-advanced-configuration-hooks.md` as the detailed hook-boundary reference and keep hooks additive, category-scoped, and zero-configuration by default when unset.
- Keep provider-specific save and capability-profile behavior aligned with `docs/architecture/dvault-v1-explicit-save-service.md`; deferred capability tickets may reference that boundary but should not pull provider optimization commitments into the core architecture story.
- No persistent planning writes were needed in this refinement pass because the decision record, child tickets, and live relations already materialize the required split.

## Open Questions
- none

## Follow-Up Questions
- When deferred hooks start implementation, which first-pass hook surfaces should stay internal or experimental until the API snapshot task confirms a stable public shape?
- Should repository discoverability later add a README link to the deferred-capability decision record, or is the current docs/plan placement sufficient for v0.5?
- After downstream stories land, does the older advanced-hooks planning note need to remain as a separate detailed reference or get narrowed to point primarily at the governing decision record?

## Risks
- If downstream work treats the architecture story as approval for concrete PIT, bridge, multi-active, or hook APIs, the API snapshot boundary could be bypassed and compatibility expectations could drift.
- If provider-specific concerns leak back into the core architecture contract, provider packages may inherit commitments that the current repository evidence does not justify.
- If a future hook overrides more than its own category, deterministic defaults for naming, hashing, record source, or timestamp behavior could be destabilized across the baseline path.

## Split Recommendations
- No additional split is recommended. The existing decision-record child task, API snapshot child task, and PIT, bridge, multi-active, and hooks downstream stories already cover the bounded decomposition.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: define how deferred Data Vault patterns plug into DVault without destabilizing existing model generation and persistence behavior.

Scope:
- Document capability contracts for PIT tables, bridge tables, multi-active satellites, and advanced hooks.
- Decide which APIs are public, which are internal, and which remain experimental for this release.
- Add guardrails for API snapshots, documentation, and examples.
- Preserve deterministic defaults for timestamp, record source, hash keys, and hash diffs unless a hook explicitly overrides them.

Acceptance Criteria:
- A decision record explains the capability architecture and extension boundaries.
- New public contracts are covered by API snapshot review or an explicit compatibility note.
- The story identifies which provider-specific behavior belongs in provider packages instead of the core package.
- PIT, bridge, multi-active, and hook stories have enough architectural guidance to proceed without conflicting designs.