[gicket-bot] PO-critic review contract

Summary
- Delivery contract is ready for developer handoff: the story is bounded to an opt-in EF SaveChanges runtime guard, Open Questions are resolved, and repository evidence supports the explicit-save and metadata-interceptor boundaries.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F492AYE4A3PKA2D20DDPQ37C/description.md contains a Delivery Contract with PO Handoff decision ready_for_po_critic, a bounded scope/DoD, and a ## Open Questions section set to '- none'.
- git diff --name-only develop..ticket/06F492AYE4A3PKA2D20DDPQ37C-story-add-optional-ef-runtime-guard-interceptor lists only .gicket/tickets/06F492AYE4A3PKA2D20DDPQ37C/... files, so the branch is still a ticket-only pre-development handoff rather than an implementation branch.
- git log --oneline --max-count=8 on ticket/06F492AYE4A3PKA2D20DDPQ37C-story-add-optional-ef-runtime-guard-interceptor shows PO handoff commit 64e0c2713 followed by PO-critic lease claim f35b2a438; git show --stat 64e0c2713ff3 shows the handoff updated only ticket comment/description/event metadata.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-33 registers IDataVaultSaveService, IDataVaultReadService, PIT maintenance, and bridge maintenance in AddDVault() with no SaveChanges interceptor registration, while src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:83-109 exposes opt-in UseDataVaultSaveChangesMetadataInterceptor(...) overloads.
- src/DCoding.Data.DVault/DataVaultSaveChangesMetadataInterceptor.cs:43-97 targets Added hub/link/satellite entities and fills only LoadTimestamp and RecordSource by reading DataVaultAnnotationNames.EntityKind, PropertyRole, and TechnicalColumnRole; src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:18-50 and TechnicalMetadataColumnRole.cs:6-25 confirm those annotations/roles exist in source.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:66-75,129-145,194-215 shows generated hub/link/satellite projections already annotate hash keys, participant references, hash diff, load timestamp, and record source, which grounds the ticket's annotation-driven detection requirement.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveChangesMetadataInterceptorSqliteTests.cs:15-97 already proves metadata-only interceptor behavior and annotation-based detection, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:125-126,<redacted> confirms the current public opt-in DbContext API plus explicit IDataVaultSaveService boundary.
- .gicket/relations/QM/7C/06F492A3MPSGP3KXDNZECN01QM--06F492AYE4A3PKA2D20DDPQ37C--parentOf.json and .gicket/relations/7C/VM/06F492AYE4A3PKA2D20DDPQ37C--06F492BNDPWS9P4EDSV0W7G6VM--blocks.json match the delivery contract's stated epic parent and downstream docs-task relation.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- An explicit acceptance example for mixed SaveChanges batches containing ordinary non-DVault entities plus generated DVault rows would make the warning/block surface easier to validate without expanding scope.
- An explicit example for effective-name overrides or shared-type tables in warning mode would further de-risk explanation formatting, although the annotation-driven requirement is already stated.
- A concrete example for satellite rows with driving keys and caller-supplied structural values would help confirm the safe caller-owned lane for non-trivial satellite shapes.

Risky assumptions
- The ticket assumes the existing annotations are sufficient to determine every required non-fillable structural value; developers will need to distinguish hash keys, participant references, parent hash keys, hash diff, and fillable metadata carefully.
- The ticket assumes guard evaluation can observe post-fill state whenever the metadata interceptor is also registered; EF interceptor ordering must be made deterministic in implementation.
- The ticket intentionally leaves the concrete warning-mode report surface open, so implementation must avoid collapsing it into exception text only or a logging-only dependency.

AC / test suggestions
- Keep one test that proves AddDVault() plus ordinary DbContext configuration still has no runtime guard unless the new opt-in API is called.
- Add paired sync and async SaveChanges coverage for warning and block modes so both paths return the same offending-entry explanations.
- Add SQLite coverage with effective-name overrides or annotated shared-type entities to prove the guard stays annotation-driven end-to-end.

Implementation watchouts
- Do not hard-code table or property names; reuse DataVaultAnnotationNames and TechnicalMetadataColumnRole metadata exactly as the current metadata interceptor and translator do.
- Treat IDataVaultSaveService as unchanged default behavior; the guard is a separate opt-in EF lane, not a replacement write path.
- Warning mode needs a deterministic caller-facing explanation surface without mutating tracked rows beyond any separately configured metadata-fill behavior.

Non-blocking notes
- The current branch contains no src/, tests/, or docs/ implementation changes; this is consistent with a pre-development quality gate and is not a PO blocker by itself.
- The downstream docs task 06F492BNDPWS9P4EDSV0W7G6VM is blocked by this story, not the other way around, so it does not prevent developer handoff.

Split recommendations
- No split is needed for the current story; the contract stays bounded to opt-in hub/link/satellite SaveChanges misuse detection with deterministic warning/block explanations.
- If future work expands into PIT or bridge guard coverage, richer observability sinks, or analyzer/runtime wording unification, keep that as separate follow-up tickets rather than widening this story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment