[gicket-bot] PO refinement contract

Summary
- Ratified this ticket as a bounded decision-record publication task that synthesizes the existing deferred-capabilities and advanced-hooks plans, preserves the current hub/link/satellite plus SQLite baseline, and does not require further ticket splitting.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use docs/plans/deferred-data-vault-capabilities.md and docs/plans/optional-advanced-configuration-hooks.md as the authoritative planning inputs for the published record; the task is to consolidate and publish their architecture stance, not to reopen that scope.
- Repository evidence already fixes the preserved baseline: docs/architecture/mvp-data-vault-concepts.md and the current source only establish hub, link, and satellite concepts and EF projection, while DataVaultModelBuilderExtensions defaults to the SQLite capability profile and the explicit save-service path stays convention-first.
- Persisted relations already place this task under story 06EZ0NSBM3GD7DY11Y4PZMXD28 and make task 06EZ0NSQFCD3W4CDCJ44GFSKA0 depend on this decision record; the wider deferred-capability decomposition is already materialized under epic 06EZ0NS59T2SW9976HHSGP2GF0 through stories 06EZ0NSXY2Y1JZ8SSCX177C770, 06EZ0NTV4SVAKV98C418T8A3CC, 06EZ0NVN71BN0QWJDCWGVZ2PYG, and 06EZ0NWKC9ZME5BSCJFSQEQ02R.
- No new child tickets, relation writes, attachments, or planning documents were created in this refinement pass.

Scope In
- Publish one decision record for the v0.5 deferred-capability architecture covering PIT tables, bridge tables, multi-active satellites, and the advanced hooks needed to support them.
- Explain why those capabilities are opt-in expansion work for v0.5 instead of part of the preserved MVP hub/link/satellite baseline.
- Ratify the default-first hook stance: naming, hashing, record source, timestamp, and provider behavior remain optional override categories and must not make ordinary DVault setup require configuration.
- State the downstream ownership boundary so the existing PIT, bridge, multi-active, hook, and API snapshot tickets can proceed without reopening architecture.

Scope Out
- Implementing PIT, bridge, multi-active, or hook behavior in product code.
- Reworking the current AddDVault, UseDataVault, ApplyDataVaultMetadata, or explicit save-service caller contract beyond documenting how deferred capabilities extend around it.
- Pulling provider-specific optimization scope back into this ticket; that work remains separate from this deferred-capability decision record.
- Creating additional child tickets unless a new scope gap appears after publication; the current decomposition is already materialized.

Open questions
- none

Follow-up questions
- When the record is published, should README.md also link to it, or is architecture/planning-surface discoverability sufficient for v0.5?
- When hook implementation starts, which parts of the hook surface should ship as stable public API in the first pass versus remain internal or compatibility-noted scaffolding?
- After the decision record lands, should the older deferred-capabilities note be retained as supporting context or narrowed to point at the new governing record?

Risks
- If the record blurs the line between advanced hooks and provider-specific optimization work, downstream tickets may over-claim provider commitments that belong to separate release planning.
- If the record fails to restate the preserved hub/link/satellite plus SQLite baseline, later capability work could unintentionally treat deferred features as a replacement for current MVP behavior.
- If the record promises too much API shape now, the existing implementation tickets may inherit artificial constraints that the current repository evidence does not justify.

Split recommendations
- No additional split is recommended. The capability tree is already materialized through epic 06EZ0NS59T2SW9976HHSGP2GF0, parent story 06EZ0NSBM3GD7DY11Y4PZMXD28, the PIT/bridge/multi-active/hooks stories, and blocked API snapshot task 06EZ0NSQFCD3W4CDCJ44GFSKA0.
- Use this ticket as the architectural publication anchor for that existing decomposition rather than creating another planning child ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment