[gicket-bot] PO refinement contract

Summary
- Reframed 06EXB7FF1J9NR2849WKDR8DKPG as a tracking umbrella with no remaining developer-owned work; completion derives from done child tickets 06EXB7FPZRCFC33RF2M5SXZTK4 and 06EXB7FYXNBPMH8VGQCGP2R41R.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - 06EXB7FF1J9NR2849WKDR8DKPG should be treated as a story-tracking umbrella, not an executable dev ticket. Its completion is derived from child tickets 06EXB7FPZRCFC33RF2M5SXZTK4 and 06EXB7FYXNBPMH8VGQCGP2R41R, which already cover the concrete implementation slices.
- critic-item-2: `answered` - No developer work remains on the story itself. The conventions entry-point slice and the provider-neutral EF metadata translation slice are already covered by the two done child tickets, and the repository already contains the corresponding EF surface and translator implementation.
- critic-item-3: `answered` - The story guidance is aligned to that decision by removing any implied developer handoff from this story. This ticket now serves only as the umbrella/tracking record for the already-completed child work, so developers should not be routed this ticket for duplicate implementation.
- critic-item-4: `answered` - Confirmed. The story did not have any remaining developer-owned slice beyond the two separate child tickets, so the corrected contract now derives story completion from those completed child tickets instead of reopening implementation on the parent story.
- critic-item-5: `answered` - The workflow ambiguity is resolved at contract level: this story should advance based on completed child work and should not be routed to dev as a fresh implementation ticket. The remaining action is review of the clarified umbrella contract, not new developer execution.

Clarifications
- 06EXB7FF1J9NR2849WKDR8DKPG is the story-level umbrella for EF Core model-building integration and is no longer an executable developer ticket.
- Story completion is derived from existing child tickets 06EXB7FPZRCFC33RF2M5SXZTK4 and 06EXB7FYXNBPMH8VGQCGP2R41R, which are both already done and already linked through parentOf relations.
- Repository evidence already shows the public EF surface in src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs and the provider-neutral translation logic in src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs, so the parent story does not own an additional uncaptured implementation slice.
- No child tickets, relations, attachments, or planning documents were created in this refinement pass because the bounded split already exists and is already materialized.

Scope In
- Maintain this ticket as the umbrella/tracking story for the EF Core model-building integration slice.
- Treat child tickets 06EXB7FPZRCFC33RF2M5SXZTK4 and 06EXB7FYXNBPMH8VGQCGP2R41R as the authoritative implementation owners for the conventions marker and provider-neutral metadata translation work.
- Keep the story-level contract aligned with the already-visible repository surface represented by UseDataVault() and ApplyDataVaultMetadata(DataVaultMetadataModel).

Scope Out
- Any new developer-owned implementation on the parent story.
- Reopening or duplicating the conventions-only EF entry point work already covered by 06EXB7FPZRCFC33RF2M5SXZTK4.
- Reopening or duplicating the provider-neutral EF metadata translation work already covered by 06EXB7FYXNBPMH8VGQCGP2R41R.
- Provider-specific relational mapping, advanced configuration hooks, or additional ticket splits.

Open questions
- none

Follow-up questions
- If tickets 06EXB7G6YE4X0GA0CT7EPEFMPR and 06EXB7HYG17X73GH0K535GYJH8 still need a live blocker after this umbrella advances, should their blocker relation be re-pointed to a concrete remaining implementation ticket instead of this story?
- Do we want a separate documentation or release-note ticket that summarizes the shipped EF Core model-building surface once this umbrella closes?

Risks
- If story-level wording drifts back toward executable developer scope, automation can hand duplicate work to developers even though the child tickets are already done.
- Downstream tickets that currently use this story as a blocker may need relation hygiene after the umbrella advances to avoid stale workflow dependencies.

Split recommendations
- No additional split is recommended; the only concrete implementation slices are already separated as 06EXB7FPZRCFC33RF2M5SXZTK4 and 06EXB7FYXNBPMH8VGQCGP2R41R, and both are done.

Persisted contract coverage
- acceptance-criteria items: 3
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment