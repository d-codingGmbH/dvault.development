[gicket-bot] PO refinement contract

Summary
- Refined this diagnostics task against the completed stable-hash contract, the done registration story, and the live ticket relations; the ticket is ready for PO-critic with no split or persisted planning changes needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The approved v1 algorithm set is already fixed by repository contract and completed registration work: default sha256-v1 plus opt-in sha1-v1, sha256-128-v1, and sha256-160-v1.
- This ticket should expose only stable-hash compatibility metadata in diagnostics and support bundles: selected algorithm id, digest byte length, and canonical lowercase hexadecimal encoding without a prefix.
- The ticket remains a child of epic 06F9GF3E7224Q4HSZ0E71ZXDB4, blocks documentation task 06F9GF4CRMXKEY2QT97W0S3GTR, and has an incoming blocks relation from done story 06F9GF417FDFWPBF1039G45FEW that should be treated as historical routing context rather than a live blocker.
- Repository HEAD still matches scratch source 1d9bd42b837c3a450297e45979a0d74d48ca1b3d, so there is no in-branch implementation to ratify yet.
- No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Add additive stable-hash metadata under DataVaultDiagnosticsResult.Explain and into the exported dvault.support-bundle.v1 payload for the active selected hash service.
- Report the selected hash algorithmId, digestByteLength, and canonical digest encoding needed for hash-key compatibility decisions.
- Update human-readable diagnostics or explain output so it summarizes the selected algorithm and digest shape without printing digest values or key material.
- Add automated coverage for default and approved opt-in built-in algorithm selections across diagnostics results and support-bundle serialization, including no-leak assertions.

Scope Out
- README, release-note, adoption-guidance, and broader hashing documentation work tracked by 06F9GF4CRMXKEY2QT97W0S3GTR.
- Hash-key storage-profile, schema-width, migration, backfill, or compatibility-gating work tracked by 06F9GF5FV54DGWY9GA8ZEZWM5R.
- Reopening the already completed built-in registration and digest-contract work from 06F9GF417FDFWPBF1039G45FEW, 06F9GF3MZHKQQ6D4SAQ0AMTKJR, and 06F9GF3TRG65G8MTMG7DH4PREC.
- Serializing digest values, business key values, raw hash-key values, or example computed hashes into diagnostics or support bundles.
- Automatic rehashing, dual-write compatibility, provider-side hashing changes, or support-bundle transport and publication behavior.

Open questions
- none

Follow-up questions
- Once ticket 06F9GF4CRMXKEY2QT97W0S3GTR resumes, should product examples show only algorithm metadata in support-bundle snippets and avoid publishing any digest samples?
- After storage-profile contract ticket 06F9GF5FV54DGWY9GA8ZEZWM5R lands, should diagnostics add non-blocking compatibility warnings when a selected digest length no longer matches the persisted schema profile?

Risks
- If the implementation reads conventions instead of the active hash service, diagnostics can drift from real runtime behavior for caller-supplied IStableHashService overrides.
- Adding new public diagnostics fields changes both public API snapshot and support-bundle JSON expectations, so additive compatibility needs explicit approval-test coverage.
- The human-readable diagnostics summary is a redaction-sensitive surface; weak negative tests could let digest text or hash-key-related values leak into display output.
- Documentation task 06F9GF4CRMXKEY2QT97W0S3GTR stays blocked until this metadata surface lands.

Split recommendations
- No further split is recommended. Documentation and storage-profile follow-up are already decomposed into existing sibling tickets, and the current task is a bounded diagnostics and support-bundle slice.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment