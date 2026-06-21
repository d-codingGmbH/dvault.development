[gicket-bot] PO refinement contract

Summary
- Refined this task to a focused code-first API ergonomics slice: give new projects one obvious binary-first opt-in at the fluent projection call site, preserve legacy-compatible defaults, and leave analyzer/docs work in sibling tickets.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the policy baseline: AddDVault(options => options.UseBinaryFirstProfile()) and modelBuilder.UseDataVaultBinaryFirstProfile() are the recommended new-project path, while AddDVault() and UseDataVault() remain the compatible HexString defaults for existing persisted models.
- The current direct code-first path is discoverable but split across two model-builder calls: the docs require UseDataVaultBinaryFirstProfile() before ApplyDataVaultMetadata(vault => ...), which is the bounded ergonomics gap for this ticket.
- The related story 06FE4R089MT3BYRCVH7Q4EX6CG is done and already treats this ticket as the dedicated code-first ergonomics slice; no additional child split is justified by current evidence.
- The incoming blocks relation from done ticket 06FE4R13DS6S2ZTGYTHA458HGM is historical analyzer sequencing context, not an active blocker; the current outgoing blocks relation to 06FE4R2EGQ444EGPKZBRZCDEV8 remains valid because broader docs consolidation still sits downstream.
- Public DVault hash-key values stay lowercase hexadecimal strings even when physical storage is Binary; this ticket does not reopen migration, rehash, backfill, dual-write, or public byte[] hash-key scope.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement.

Scope In
- Add one focused direct code-first model-builder convenience so a new project can opt into the recommended binary-first projection at the same call site as fluent ApplyDataVaultMetadata(...) usage instead of relying on an easy-to-miss separate prelude step.
- Preserve the current provider-aware code-first translation behavior, binary hash-key and participant-reference projection, and binary-first conventions annotation once the focused convenience is used.
- Keep the existing UseDataVaultBinaryFirstProfile() plus ApplyDataVaultMetadata(...) path supported for compatibility, and keep plain ApplyDataVaultMetadata(...) without explicit binary-first opt-in on the compatible default path.
- Add the bounded API-surface, projection, and regression coverage needed to make the new code-first entry point discoverable and safe.

Scope Out
- Changing DVault's default storage profile, auto-enabling binary-first for existing code-first models, or implicitly migrating persisted HexString databases.
- Broad metadata-first, model-first, or registry-backed setup changes; those paths already have their own AddDVault and UseDataVaultMetadata guidance.
- A general-purpose model-builder conventions DSL or broader stable-hash configuration redesign beyond the binary-first new-project ergonomics slice.
- Analyzer guidance work already completed in 06FE4R13DS6S2ZTGYTHA458HGM.
- Broad docs, release-note, and performance-profile consolidation work already bounded by 06FE4R2EGQ444EGPKZBRZCDEV8.

Open questions
- none

Follow-up questions
- After the binary-first call-site convenience lands, should a later ticket offer an equally focused direct code-first path for non-default stable hash algorithm and digest selections, or keep that strictly on explicit provider-capability profiles?
- When 06FE4R2EGQ444EGPKZBRZCDEV8 consolidates docs, should older two-step code-first examples be trimmed aggressively to reduce future drift between the legacy-compatible and recommended new-project paths?

Risks
- If the ergonomics change silently alters default ApplyDataVaultMetadata(...) behavior instead of staying explicit, legacy HexString-compatible code-first models could drift unexpectedly.
- If the convenience path does not preserve existing conventions annotations and translation semantics, diagnostics, migration guardrails, and docs may disagree about the realized model shape.
- If the ticket expands into general stable-hash or provider-configuration design, the public API surface will sprawl and overlap with already-bounded adjacent tickets.

Split recommendations
- No new split is needed; the done analyzer task 06FE4R13DS6S2ZTGYTHA458HGM already covers guidance, and this ticket remains the dedicated code-first ergonomics slice.
- No new split is needed for documentation alignment; the existing downstream task 06FE4R2EGQ444EGPKZBRZCDEV8 already owns docs, release-note, and profile-consolidation work.
- No new split is needed for broader binary-adoption planning because the done story 06FE4R089MT3BYRCVH7Q4EX6CG already materialized the bounded downstream graph.

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