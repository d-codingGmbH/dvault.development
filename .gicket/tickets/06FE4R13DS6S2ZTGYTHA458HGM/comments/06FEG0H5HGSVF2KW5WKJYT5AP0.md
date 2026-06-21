[gicket-bot] PO refinement contract

Summary
- Refined this task to a bounded analyzer-guidance slice: add a high-confidence source-visible advisory for new-model HexString defaults where the repository recommends binary-first, while keeping legacy-compatible HexString paths non-diagnostic. No child tickets, relation changes, description updates, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the storage baseline: `services.AddDVault(options => options.UseBinaryFirstProfile())` and `modelBuilder.UseDataVaultBinaryFirstProfile()` are the recommended new-project setup paths, while `AddDVault()` and `UseDataVault()` remain compatible HexString defaults for existing persisted models.
- Public DVault hash-key values remain lowercase hexadecimal strings even when physical storage is `Binary`; this ticket does not reopen migration, rehash, backfill, dual-write, or public `byte[]` hash-key scope.
- The analyzer package remains optional local tooling with `PrivateAssets="all"` and the existing `.NET 10 SDK` build-host baseline for both coordinated package lines.
- Existing analyzer behavior already uses direct source-visible evidence and skips ambiguous cases instead of guessing; this ticket should extend that same high-confidence boundary rather than introduce whole-application or runtime inference.
- No bounded planning writes were applied during this refinement: no child tickets, relation changes, description updates, attachments, or planning documents were created or queued.

Scope In
- Add a high-confidence analyzer advisory for source-visible DVault setup that leaves new-model hash-key storage on the compatible HexString path where the repository now recommends binary-first for new generated schemas.
- Keep remediation guidance aligned with the documented new-project opt-in APIs on both service-registration and Code-First model-builder surfaces.
- Document and test that legacy-compatible or ambiguous HexString setups remain supported and non-diagnostic.

Scope Out
- Changing the runtime default storage profile, auto-enabling binary-first, or broadening DVault into automatic migration tooling.
- Automatic rehash, backfill, dual-write, repair, or public `byte[]` hash-key behavior.
- Whole-application, cross-assembly, or historical-database inference to guess whether a project is new or existing.
- Code-first API ergonomics work already bounded by ticket `06FE4R1C96NBSNMM7AFDTHJ7A4`.

Open questions
- none

Follow-up questions
- After the first source-visible advisory lands, should a later ticket extend comparable high-confidence guidance to metadata-first or model-first setup lanes only if a similarly direct evidence surface becomes available?
- Once developer feedback exists, should the advisory severity remain informational or suggestion-level, or be revisited in a later release?

Risks
- Any attempt to infer persisted database history or project age from ambiguous source will create false positives and break the supported legacy-compatible HexString posture.
- If service-registration guidance and Code-First model-builder guidance drift apart, adopters may receive inconsistent binary-first recommendations for the same product policy.
- If the analyzer message overstates the recommendation as a mandatory error, it will conflict with the repository's documented compatibility baseline for existing persisted models.

Split recommendations
- No new split is needed; this ticket is already the bounded analyzer-guidance slice for the parent story.
- No new split is needed for API ergonomics or broad docs work because those lanes are already separated into `06FE4R1C96NBSNMM7AFDTHJ7A4` and `06FE4R2EGQ444EGPKZBRZCDEV8`.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment