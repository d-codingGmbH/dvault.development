<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket into a bounded same-as link role-support story on top of the existing role-bearing metadata and diagnostic baseline, scoped dependent child keys into follow-up work, and made no child-ticket, relation, or planning-document writes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Live .gicket state shows the story remains under epic 06F2PGK4QJ0YGXK5479W83Z2J0, still blocks documentation task 06F2PGM9038RXVJH0RJFYEJEV0, and still has incoming blocks relations from done tickets 06F2PGHJAFMH80TZAMANQWH9PW and 06F2PGKV9AFAMKGJEKKZ3AXHGC; no relation cleanup was materialized in this run.
- Current branch source already has the role concept for recursive links in dvault.model.v1 and the internal metadata pipeline: DataVaultModelArtifactParser preserves participant role names, DMV1601 and DMV1602 already govern repeated-hub role binding, and DataVaultEfMetadataTranslator names link participant columns from SourceEndpointName.
- Current code-first source does not yet expose that role-bearing path: DataVaultCodeFirstLinkBuilder only offers Participant<TEntity>() for participants, and DataVaultCodeFirstModelBuilder still rejects repeated same-hub participants.
- Current explicit save and typed link-mapper surfaces remain unique-hub oriented: DataVaultSaveService.CreateLinkSavePlan, IDataVaultLinkMapper, DataVaultLinkParticipantBindingAttribute, and DMV1955 still assume participant keys are unique by participant name and do not yet provide a role-based same-hub path.
- Repository search found no existing dependent child key concept in the visible public API, metadata model, diagnostics, or tests, so dependent child keys should not be folded into the same bounded delivery as same-as role support.

### Scope In
- Add an additive Participant<TEntity>(string role) overload on DataVaultCodeFirstLinkBuilder and support repeated same-hub participants only when every repeated occurrence supplies a distinct non-blank role.
- Require explicit relationship names for role-bearing repeated-hub links; keep the existing derived-name overload behavior unchanged for distinct-hub links.
- Project role-bearing code-first participants through DataVaultLinkMetadata so the role becomes the authoritative produced participant name for EF link-column naming, validation, and downstream metadata that already uses SourceEndpointName.
- Update the explicit link save boundary so same-hub links can be persisted with role-keyed participant values while existing distinct-hub links remain valid without rework.
- Add regression coverage for valid same-as link declarations, clear rejection of missing or duplicate roles, projected EF schema naming, and the supported explicit save path.

### Scope Out
- Dependent child key modeling, including any new link-key column family, relationship-hash contract change, or new public metadata concept for non-participant key components.
- Typed link-mapper or source-generator parity for same-hub role-bearing links, including IDataVaultLinkMapper guidance, DataVaultLinkParticipantBindingAttribute shape, and DMV1955 behavior.
- Bridge, PIT, effectivity, or other advanced link-shape expansion beyond the repeated same-hub same-as baseline.
- README, planning-doc, and release-note authoring already tracked on 06F2PGM9038RXVJH0RJFYEJEV0.

## Acceptance Criteria
- DataVaultCodeFirstLinkBuilder exposes Participant<TEntity>(string role), and code-first repeated same-hub links succeed only when every repeated participant has a distinct non-blank role; existing distinct-hub Participant<TEntity>() behavior remains unchanged.
- Role-bearing repeated-hub links are supported only through Link(string relationshipName, ...), and the supplied role names become the produced participant names carried through projected link metadata and generated EF column/index naming.
- The explicit save path can persist a same-hub link by supplying participant hash keys keyed by the produced participant names, while existing distinct-hub link saves remain compatible.
- Regression tests cover at least one same-as or self-link happy path plus clear failures for missing repeated-hub roles and duplicate repeated-hub roles.
- Documentation and release-note follow-through remains on 06F2PGM9038RXVJH0RJFYEJEV0 and is not reopened inside this story.

## Definition of Done
- A developer can declare a same-as or other repeated same-hub link in code-first metadata by using an explicit link name and distinct participant roles without hitting the current repeated-hub rejection path.
- Projected metadata and translated EF schema preserve the repeated-hub participant roles as authoritative participant names and do not regress existing distinct-hub link projections.
- The supported explicit save boundary accepts the role-bearing participant names required to persist the new same-hub link shape, and automated tests cover that supported path.
- No child tickets, relation changes, attachments, or planning documents were materialized in this refinement run.

## Implementation Notes
- Reuse the repository-visible term role rather than inventing alias: dvault.model.v1, DataVaultModelArtifactParser, and DMV1601/DMV1602 already use role as the disambiguation token for repeated-hub links and recursive bridge binding.
- DataVaultEfMetadataTranslator already names link participant columns from participant.SourceEndpointName, so the main same-as gaps are the code-first builder or projection entry point and the explicit save resolution that still keys participants by hub name.
- Keep the bounded implementation on the existing explicit save boundary. If same-hub role support later needs typed link-mapper or generated-mapper parity, that should land as separate follow-up work rather than expanding this story again.
- Dependent child keys have no visible repository baseline today. Treat them as a separate future architecture surface instead of overloading repeated-hub participant roles.

## Open Questions
- none

## Follow-Up Questions
- Should dependent child key modeling be created as a separate child story for this milestone, or deferred until a dedicated metadata, naming, hash, and save contract is approved?
- After core same-as role support lands, should IDataVaultLinkMapper and DataVaultLinkParticipantBindingAttribute gain role-based participant-name support, or stay unique-hub-only for v0.13?
- Should 06F2PGM9038RXVJH0RJFYEJEV0 add one canonical same-as example that shows an explicit link name plus repeated same-hub participant roles?

## Risks
- If dependent child keys are pulled back into this story, the work will expand beyond repeated-hub role modeling into new link-key, hashing, save-service, and documentation contracts with no visible repository baseline.
- Changing the explicit link save boundary from hub-name keys to produced participant-name keys needs careful backward-compatibility handling so distinct-hub callers do not regress.
- Repository docs still lag the live code-first surface; until 06F2PGM9038RXVJH0RJFYEJEV0 lands, reviewers may misread current support boundaries.

## Split Recommendations
- Create a separate child ticket for dependent child key modeling if the release still requires it; that capability is a different architectural expansion than same-as repeated-hub role support.
- If product wants same-hub typed mapper or source-generator parity after the core same-as path lands, track that as a separate follow-up ticket rather than folding it into this story now.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add fluent support for same-as links and dependent child keys.

## Scope
- Refine and complete the work for "Add same-as link and dependent child key modeling" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.