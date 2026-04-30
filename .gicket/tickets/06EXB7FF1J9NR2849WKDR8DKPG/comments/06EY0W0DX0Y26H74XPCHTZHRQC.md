[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Repository source already contains the advertised EF surface: `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` exposes `ModelBuilder.UseDataVault()` and `ApplyDataVaultMetadata(DataVaultMetadataModel)`.
- `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs` defines the DVault-owned EF annotation keys, and `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` projects hubs, links, and satellites into provider-neutral shared-type EF entities with produced-name, entity-kind, metadata-name, ordinal, property-role, technical-column-role, primary-key, and secondary-index metadata.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` targets `net10.0` and references `Microsoft.EntityFrameworkCore` version `10.0.0`.
- `git show --stat --oneline c92bb1cd1750` shows the latest `[06EXB7FF1J9NR2849WKDR8DKPG] handoff po->po-critic` commit changed only `.gicket` description/comment/event metadata for this ticket, not repository source files.

Blocking findings
- The story explicitly frames itself as an umbrella over existing downstream work, but the two named implementation slices are already separate tickets and both are `done` (`06EXB7FPZRCFC33RF2M5SXZTK4`, `06EXB7FYXNBPMH8VGQCGP2R41R`). The story does not identify any remaining developer-owned work beyond those completed tickets.
- The latest PO->PO-critic handoff only refreshed ticket metadata and did not clarify whether this story should still go to `dev`, be advanced based on completed downstream work, or be treated as a tracking umbrella. That workflow ambiguity is blocking at ticket level because it can hand a developer duplicate or already-satisfied scope.

Required PO actions
- Clarify whether `06EXB7FF1J9NR2849WKDR8DKPG` is still intended to be an executable dev ticket or an umbrella/story-tracking item whose completion is derived from `06EXB7FPZRCFC33RF2M5SXZTK4` and `06EXB7FYXNBPMH8VGQCGP2R41R`.
- If developer work still remains on the story, state that remaining slice explicitly and distinguish it from the already-done conventions and EF metadata translation tickets.
- Align the story status/comment guidance with that decision so a developer is not handed duplicate or already-satisfied scope.

Open issues ledger
- critic-item-1 [required-po-action] Clarify whether `06EXB7FF1J9NR2849WKDR8DKPG` is still intended to be an executable dev ticket or an umbrella/story-tracking item whose completion is derived from `06EXB7FPZRCFC33RF2M5SXZTK4` and `06EXB7FYXNBPMH8VGQCGP2R41R`.
- critic-item-2 [required-po-action] If developer work still remains on the story, state that remaining slice explicitly and distinguish it from the already-done conventions and EF metadata translation tickets.
- critic-item-3 [required-po-action] Align the story status/comment guidance with that decision so a developer is not handed duplicate or already-satisfied scope.
- critic-item-4 [blocking-finding] The story explicitly frames itself as an umbrella over existing downstream work, but the two named implementation slices are already separate tickets and both are `done` (`06EXB7FPZRCFC33RF2M5SXZTK4`, `06EXB7FYXNBPMH8VGQCGP2R41R`). The story does not identify any remaining developer-owned work beyond those completed tickets.
- critic-item-5 [blocking-finding] The latest PO->PO-critic handoff only refreshed ticket metadata and did not clarify whether this story should still go to `dev`, be advanced based on completed downstream work, or be treated as a tracking umbrella. That workflow ambiguity is blocking at ticket level because it can hand a developer duplicate or already-satisfied scope.

Missing examples / edge cases
- Missing a concrete example of what a developer should still do on this story after the completed conventions and translation tickets.

Risky assumptions
- Assumes there is no untracked residual integration work outside `06EXB7FPZRCFC33RF2M5SXZTK4` and `06EXB7FYXNBPMH8VGQCGP2R41R`.

AC / test suggestions
- If the story remains executable, add an acceptance criterion that names the remaining developer-owned outcome not already covered by `06EXB7FPZRCFC33RF2M5SXZTK4` and `06EXB7FYXNBPMH8VGQCGP2R41R`.
- If the story is only an umbrella, replace execution-oriented closure language with relation/status criteria that describe when the story advances.

Implementation watchouts
- Do not reopen provider-specific or advanced-configuration scope that the story and downstream task contracts explicitly exclude.
- Do not duplicate work already covered by the conventions-only ticket and the provider-neutral EF metadata translation ticket.

Non-blocking notes
- The persisted delivery contract shows `## Open Questions` = `none`, so there is no open-question gate once the remaining-work/workflow ambiguity is resolved.
- Direct source evidence already backs the technical contract in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs`, `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs`, `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`, and `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`.

Split recommendations
- No additional split is needed until PO first resolves whether the existing done tickets already exhaust the story scope.
- If residual work exists after that clarification, capture it as a distinct task instead of keeping it implicit in this umbrella story.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment