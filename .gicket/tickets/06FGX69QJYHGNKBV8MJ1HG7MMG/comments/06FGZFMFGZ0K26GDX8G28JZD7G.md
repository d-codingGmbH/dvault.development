[gicket-bot] PO refinement contract

Summary
- Refined the manifest-validator ticket against the checked-in storage-profile contract and migration guide; scope stays limited to deterministic parse/validate behavior for dvault.hash-key-storage-migration.v1.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository contract evidence fixes the v1 manifest schemaVersion to dvault.hash-key-storage-migration.v1 with expectedStorageProfiles source=HexString and target=Binary only.
- The authoritative baseline for validation is a reviewed redacted dvault.support-bundle.v1 or equivalent translated EF metadata; live-schema evidence is supplemental and may only downgrade to warning when the authoritative baseline is otherwise complete.
- Relation context was verified: this ticket is a child of 06FGX5VQ9Y665A727EFJ677SBC, is currently blocked by 06FGX67TZV1F6S949F96ZE201W, and currently blocks 06FGX6B9KQME0NJ8B810239DG0.
- No attachment, relation, or description write was materialized during refinement because docs/plans/hash-key-storage-profile-contract.md and docs/hash-key-storage-migration.md already provide the authoritative contract baseline.

Scope In
- Parse and validate one dvault.hash-key-storage-migration.v1 manifest for a single selected existing model boundary.
- Require top-level facts for schemaVersion, selectedModelBoundary, reviewedSourceEvidence, providerProfileId, modelHashFacts, expectedStorageProfiles, coverage, and validation findings.
- Validate complete-boundary coverage for every DVault-owned HashKey and ParticipantReference column across generated hubs, links, satellites, PITs, and bridges exactly once.
- Validate the bounded v1 provider/profile/hash baselines, including the visible built-in providerProfileId values and stable-hash algorithm and digest-size set.
- Produce deterministic error, warning, and info findings suitable for diagnostics and preflight consumption.

Scope Out
- Executing migrations, generating SQL, opening or inspecting live databases by default, or altering EF models.
- Backfill, repair, reconcile, dual-write, rehash, rollback orchestration, or provider-specific data-move tooling.
- Support-bundle capture, manifest generation, or broader CLI workflow work beyond consuming and validating a manifest.
- Any manifest direction other than HexString to Binary, same-profile audits, or custom provider/profile vocabularies.

Open questions
- none

Follow-up questions
- Should a later ticket expose this validator through a dedicated consumer-facing preflight command or manifest-emission workflow if that surface is not already covered elsewhere in the hash-key migration epic?
- After the validator lands, do dependent tickets need a shared finding-code catalog or rendering guidance for support-bundle and CLI consumers?

Risks
- This ticket is already in a dependency chain: it is blocked by 06FGX67TZV1F6S949F96ZE201W and currently blocks 06FGX6B9KQME0NJ8B810239DG0, so contract drift in the finding shape or coverage rules can ripple into adjacent work.
- Fail-closed complete-boundary coverage will surface producer or baseline gaps immediately; any upstream manifest producer that omits PIT or bridge references will prevent successful validation until the evidence source is corrected.
- Provider-specific normalization or storage-fact assumptions must stay within the finite built-in profile baseline; expanding beyond that set inside this ticket would increase scope and validation ambiguity.

Split recommendations
- No split recommended for this ticket as refined; keep provider-specific live-schema enhancements, manifest generation, or broader migration orchestration in separate follow-up tickets if they arise.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment