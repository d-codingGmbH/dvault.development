[gicket-bot] PO-critic review contract

Summary
- Delivery contract is bounded and locally evidenced; the ticket is ready for developer handoff on same-hub role support, with dependent child keys explicitly deferred.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGM1HQ5W1M2H8T50MZ3EEC/description.md:18-36 scopes the work to same-hub role support, explicit-save compatibility, and regression coverage, while lines 25-29 explicitly scope dependent child keys and mapper/generator parity out; lines 50-51 show `## Open Questions` = `none`.
- .gicket/relations/J0/EC/06F2PGK4QJ0YGXK5479W83Z2J0--06F2PGM1HQ5W1M2H8T50MZ3EEC--parentOf.json shows the story remains under epic `06F2PGK4QJ0YGXK5479W83Z2J0`, and .gicket/relations/EC/V0/06F2PGM1HQ5W1M2H8T50MZ3EEC--06F2PGM9038RXVJH0RJFYEJEV0--blocks.json shows it still blocks doc task `06F2PGM9038RXVJH0RJFYEJEV0`.
- Current code evidence matches the refinement gap: src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs:18-23 exposes only `Participant<TEntity>()`, and src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:137-152 still throws on repeated same-hub participants and builds link metadata from plain hub references.
- The downstream naming path already exists: src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:117-130 carries `SourceEndpointName` on `DataVaultLinkParticipantMetadata`, src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs:<redacted> builds link participants with `GetParticipantProducedBaseName`, and src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:111-125 names link columns from `participant.SourceEndpointName`.
- Model-first diagnostics already encode the intended role semantics: src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs:547-577 rejects duplicate roles and repeated hubs without roles, and line 917 makes the produced participant name `role ?? hub`.
- The save and mapper boundaries are still unique-hub keyed today: src/DCoding.Data.DVault/DataVaultSaveService.cs:290-313 keys registry link saves by participant hub metadata name, src/DCoding.Data.DVault/DataVaultSaveService.cs:<redacted> creates save plans from `participant.HubReference.Name`, src/DCoding.Data.DVault/IDataVaultLinkMapper.cs:8-13 documents repeated same-hub typed mappings as unsupported, src/DCoding.Data.DVault/DataVaultLinkParticipantBindingAttribute.cs:4-17 binds `participantHubName`, and src/DCoding.Data.DVault.Analyzers/DataVaultMappingDiagnosticCatalog.cs:53-60 defines DMV1955 for repeated link participants.
- Repository search for dependent-child-key baseline came back empty outside the ticket contract: `rg -n "dependent child key|dependent child|child key" src tests docs README.md` returned no matches outside .gicket/tickets/06F2PGM1HQ5W1M2H8T50MZ3EEC/description.md.
- Branch history is still ticket-only, which is expected at this gate: `git log --oneline -n 4` shows PO handoff commits `baa2d8b88` and `ca685e512` plus PO-critic claim `6914a7379`, and `git diff --name-only develop..HEAD` lists only .gicket ticket/comment/event files for `06F2PGM1HQ5W1M2H8T50MZ3EEC`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Whitespace-only repeated-hub roles should fail the same as missing roles; the contract says roles must be non-blank but does not give an explicit example.
- A repeated same-hub link using the derived-name overload `Link(link => ...)` should fail clearly, since the contract allows same-hub role support only through `Link(string relationshipName, ...)`.
- A mixed multi-participant link with one repeated hub and one distinct hub would help prove that only the repeated occurrences need roles and that distinct-hub naming still behaves unchanged.

Risky assumptions
- The release can defer dependent child key modeling without leaving a milestone gap; no separate child ticket for that work exists in this branch snapshot yet.
- Developers will treat the delivery contract as authoritative even though the ticket title and legacy draft still mention dependent child keys.

AC / test suggestions
- Keep one happy-path same-as/self-link test that asserts produced EF table, column, and index names use the supplied role names rather than duplicate hub names.
- Add an explicit-save regression that persists a same-hub link with role-keyed participant values and a compatibility regression that preserves the current distinct-hub save path unchanged.
- Keep at least one regression for the existing distinct-hub derived-name overload so the additive role work does not alter current `Participant<TEntity>()` behavior.

Implementation watchouts
- Role names need to flow through `DataVaultLinkParticipantMetadata.SourceEndpointName`; otherwise `DataVaultEfMetadataTranslator` will keep emitting hub-name-based column names.
- Same-hub save support must be implemented without silently widening typed mapper or source-generator scope, because `IDataVaultLinkMapper`, `DataVaultLinkParticipantBindingAttribute`, and DMV1955 are explicitly scoped out of this story.
- The current save-plan hashing and row construction path uses `participant.HubReference.Name`; that path must gain a backward-compatible role-keyed lane for repeated same-hub links while leaving distinct-hub callers untouched.

Non-blocking notes
- The authoritative delivery contract is clear enough for dev handoff, but the ticket title and legacy draft still mention dependent child keys, so casual readers may need to rely on the contract block rather than the title.
- The current review branch contains only .gicket ticket metadata changes and no implementation changes yet; this is consistent with a pre-development PO-critic gate, not a quality issue.
- Documentation and release-note follow-through is intentionally downstream on ticket `06F2PGM9038RXVJH0RJFYEJEV0`, which is why this story can move to dev even though the docs ticket is not ready yet.

Split recommendations
- Create a separate child story if v0.13 still requires dependent child key modeling; the repository has no visible baseline for that capability today.
- Track any same-hub typed mapper or source-generator parity as a separate follow-up instead of widening this story beyond the explicit save boundary.
- Use ticket `06F2PGM9038RXVJH0RJFYEJEV0` for the canonical same-as documentation example once the implementation lands.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment