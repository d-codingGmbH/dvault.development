[gicket-bot] PO refinement contract

Summary
- Epic refinement ratified the existing five-story split and repository baseline for deferred Data Vault capabilities; no new child tickets, relation updates, or planning documents were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This epic is already materially decomposed into existing child stories for architecture (06EZ0NSBM3GD7DY11Y4PZMXD28), PIT (06EZ0NSXY2Y1JZ8SSCX177C770), bridge (06EZ0NTV4SVAKV98C418T8A3CC), multi-active satellites (06EZ0NVN71BN0QWJDCWGVZ2PYG), and advanced hooks (06EZ0NWKC9ZME5BSCJFSQEQ02R); no additional split, relation write, attachment, or planning-document write was justified in this pass.
- Repository and plan evidence already fix the v1 default boundary: opt-in PIT, bridge, and multi-active capability families; additive advanced hooks; unchanged zero-configuration AddDVault()/UseDataVault()/ApplyDataVaultMetadata() path; explicit IDataVaultSaveService request boundary; SQLite as the default capability profile; deterministic naming, metadata, record-source, and UTC load-timestamp defaults when hooks are unset.
- docs/plans/deferred-data-vault-capabilities.md is the governing architecture record for this epic, with docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md and docs/plans/optional-advanced-configuration-hooks.md as bounded supporting contracts for bridge and hook details.
- Deferred advanced items remain outside this epic's v1 baseline unless a new follow-up ticket reopens them explicitly: PIT row population/refresh, bridge row maintenance, provider-specific optimization, link-based PIT, multi-active PIT semantics, and unbounded pattern variants.

Scope In
- Ratify the combined deferred-capability contract across PIT metadata projection, bridge metadata projection, multi-active satellite modeling/persistence semantics, and additive advanced configuration hooks.
- Preserve the current convention-first DVault baseline and require each deferred capability to stay opt-in and additive to existing hub, link, and satellite behavior.
- Define epic-level closure guardrails: tests and docs per capability, provider-neutral baselines, explicit limitations, and compatibility review for new public surfaces.
- Use the existing child-story split and current repository behavior as the bounded delivery structure for this epic.

Scope Out
- Replacing the current MVP hub, link, satellite, zero-configuration startup, or explicit save-service baseline for ordinary scenarios.
- Provider-specific DDL, native SQL, migrations, performance tuning, or optimization commitments beyond provider-owned follow-up work.
- PIT refresh/population orchestration, bridge row population/maintenance, link-based PITs, multi-active PIT semantics, or other advanced pattern expansions not already bounded by the current child stories.
- Unbounded automation for every Data Vault variant or reopening previously ratified naming, hashing, record-source, or timestamp defaults without a dedicated compatibility-reviewed change.

Open questions
- none

Follow-up questions
- After the current provider-neutral baselines are accepted, should PIT refresh/population, bridge runtime maintenance, and multi-active PIT semantics each remain separate follow-up tickets rather than reopening this epic?
- Should epic closure include a lightweight discoverability pass, such as README or docs-index links to the governing deferred-capability decision record and child contracts?
- When closure cleanup happens, should any stale dependency relations on already-delivered umbrella or child tickets be removed in the same pass?

Risks
- The epic could be treated as complete from child status alone even though its own acceptance criteria require a combined behavior review.
- Bridge story 06EZ0NTV4SVAKV98C418T8A3CC still documents a remaining hierarchy-validation gap; if that child contract is ignored, epic closure can mask unsupported bridge metadata shapes.
- If later deferred-capability work bypasses compatibility review or lets provider-specific concerns leak into the core package, the current deterministic default path can drift.

Split recommendations
- No additional split is recommended. The existing child-ticket structure already provides the bounded decomposition this epic needs.
- Keep later PIT refresh, bridge maintenance, multi-active PIT, and provider-specific optimization work in new follow-up tickets instead of expanding the current epic scope.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment