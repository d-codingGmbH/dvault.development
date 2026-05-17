[gicket-bot] PO refinement contract

Summary
- Refined the ticket into a bounded same-as link role-support story on top of the existing role-bearing metadata and diagnostic baseline, scoped dependent child keys into follow-up work, and made no child-ticket, relation, or planning-document writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Live .gicket state shows the story remains under epic 06F2PGK4QJ0YGXK5479W83Z2J0, still blocks documentation task 06F2PGM9038RXVJH0RJFYEJEV0, and still has incoming blocks relations from done tickets 06F2PGHJAFMH80TZAMANQWH9PW and 06F2PGKV9AFAMKGJEKKZ3AXHGC; no relation cleanup was materialized in this run.
- Current branch source already has the role concept for recursive links in dvault.model.v1 and the internal metadata pipeline: DataVaultModelArtifactParser preserves participant role names, DMV1601 and DMV1602 already govern repeated-hub role binding, and DataVaultEfMetadataTranslator names link participant columns from SourceEndpointName.
- Current code-first source does not yet expose that role-bearing path: DataVaultCodeFirstLinkBuilder only offers Participant<TEntity>() for participants, and DataVaultCodeFirstModelBuilder still rejects repeated same-hub participants.
- Current explicit save and typed link-mapper surfaces remain unique-hub oriented: DataVaultSaveService.CreateLinkSavePlan, IDataVaultLinkMapper, DataVaultLinkParticipantBindingAttribute, and DMV1955 still assume participant keys are unique by participant name and do not yet provide a role-based same-hub path.
- Repository search found no existing dependent child key concept in the visible public API, metadata model, diagnostics, or tests, so dependent child keys should not be folded into the same bounded delivery as same-as role support.

Scope In
- Add an additive Participant<TEntity>(string role) overload on DataVaultCodeFirstLinkBuilder and support repeated same-hub participants only when every repeated occurrence supplies a distinct non-blank role.
- Require explicit relationship names for role-bearing repeated-hub links; keep the existing derived-name overload behavior unchanged for distinct-hub links.
- Project role-bearing code-first participants through DataVaultLinkMetadata so the role becomes the authoritative produced participant name for EF link-column naming, validation, and downstream metadata that already uses SourceEndpointName.
- Update the explicit link save boundary so same-hub links can be persisted with role-keyed participant values while existing distinct-hub links remain valid without rework.
- Add regression coverage for valid same-as link declarations, clear rejection of missing or duplicate roles, projected EF schema naming, and the supported explicit save path.

Scope Out
- Dependent child key modeling, including any new link-key column family, relationship-hash contract change, or new public metadata concept for non-participant key components.
- Typed link-mapper or source-generator parity for same-hub role-bearing links, including IDataVaultLinkMapper guidance, DataVaultLinkParticipantBindingAttribute shape, and DMV1955 behavior.
- Bridge, PIT, effectivity, or other advanced link-shape expansion beyond the repeated same-hub same-as baseline.
- README, planning-doc, and release-note authoring already tracked on 06F2PGM9038RXVJH0RJFYEJEV0.

Open questions
- none

Follow-up questions
- Should dependent child key modeling be created as a separate child story for this milestone, or deferred until a dedicated metadata, naming, hash, and save contract is approved?
- After core same-as role support lands, should IDataVaultLinkMapper and DataVaultLinkParticipantBindingAttribute gain role-based participant-name support, or stay unique-hub-only for v0.13?
- Should 06F2PGM9038RXVJH0RJFYEJEV0 add one canonical same-as example that shows an explicit link name plus repeated same-hub participant roles?

Risks
- If dependent child keys are pulled back into this story, the work will expand beyond repeated-hub role modeling into new link-key, hashing, save-service, and documentation contracts with no visible repository baseline.
- Changing the explicit link save boundary from hub-name keys to produced participant-name keys needs careful backward-compatibility handling so distinct-hub callers do not regress.
- Repository docs still lag the live code-first surface; until 06F2PGM9038RXVJH0RJFYEJEV0 lands, reviewers may misread current support boundaries.

Split recommendations
- Create a separate child ticket for dependent child key modeling if the release still requires it; that capability is a different architectural expansion than same-as repeated-hub role support.
- If product wants same-hub typed mapper or source-generator parity after the core same-as path lands, track that as a separate follow-up ticket rather than folding it into this story now.

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