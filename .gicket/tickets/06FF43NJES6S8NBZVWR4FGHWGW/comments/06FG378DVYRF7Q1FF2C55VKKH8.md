[gicket-bot] PO refinement contract

Summary
- Refined this into extending the existing metadata-first SQLite quickstart and Getting Started privacy proof into one local binary-first privacy example; prerequisite privacy report, diagnostics, and test work is already done, so no blocking PO questions remain.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already provides the two pieces this ticket needs: the runnable binary-first SQLite quickstart in examples/DCoding.Data.DVault.SqliteQuickstart and the opt-in privacy proof in docs/getting-started.md; this ticket combines them instead of inventing new privacy APIs or a separate platform surface.
- Use the existing metadata-first quickstart baseline (UseBinaryFirstProfile().UseMetadataModel(...), UseDataVaultMetadata(), and AddDVaultSqlite()) rather than adding a second code-first or provider-matrix variant.
- Use the current privacy seam only: AddDVaultPrivacy(...), RegisterEncryptedPayloadAlias(...), UseCallerOwnedKeyProvider(...), IDataVaultEncryptedPayloadKeyProvider, and DataVaultEncryptedPayloadValueConverter.
- Keep the flow local and library-focused: no external key store, no compliance workflow, no provider-native encryption feature claims, and no automatic SaveChanges encryption or redaction.
- Done prerequisite tickets 06FF43M7AE9DN3K1YXBPB1R574, 06FF43MQ3AXXK2S5TK65X4Y9S8, and 06FF43NAAR3WXH759TVG2RS2M4 are historical prerequisite context, not unresolved PO blockers for this refinement pass; no relation writes, description updates, attachments, or planning documents were materialized here.

Scope In
- Extend the existing SQLite quickstart path so one local sample shows binary-first DVault registration plus explicit privacy registration and one encrypted payload round-trip in the same SQLite-backed flow.
- Update docs/getting-started.md with the combined SQLite privacy quickstart path and its current caveats, using repository-backed runtime names and behaviors.
- Add or adjust small example support code under the existing SQLite quickstart or shared example surfaces when needed to keep the privacy proof runnable and inspectable.
- Make the example show how the runtime privacy alias aligns with the documented personalData[].encryptedPayloadAlias vocabulary without turning the sample into a broader modeling tutorial.

Scope Out
- No new standalone SqlitePrivacyQuickstart project, no separate PostgreSQL or other provider privacy variant, and no broader provider matrix expansion.
- No key management service, rotation, escrow, deletion workflow, retention workflow, or GDPR/DSGVO compliance ownership.
- No provider-native encrypted DDL, encrypted-file build guidance, runtime encryption capability probing, or provider-specific crypto dispatch.
- No new privacy diagnostics, coverage-report behavior, analyzer work, or release-doc/package-version alignment; downstream ticket 06FF43WMMC8R3T4ZKVR4312NJC keeps the broader v0.48 docs sweep.

Open questions
- none

Follow-up questions
- After this ticket lands, decide whether the combined SQLite quickstart is sufficient as the canonical privacy proof or whether a later dedicated privacy example project is worth the extra maintenance.
- When ticket 06FF43WMMC8R3T4ZKVR4312NJC resumes, decide whether the root README and examples README should link directly to the combined privacy quickstart section or simply point to docs/getting-started.md.
- Consider a later documentation slice that shows how the privacy coverage report relates to the same alias used in the quickstart, but that is not required for this ticket.

Risks
- If implementation expands from a small local proof into key-management or compliance guidance, it will violate the established optional privacy boundary.
- If the example blurs ordinary EF Core converter mapping with DVault satellite modeling, readers may incorrectly infer automatic encryption from DVault metadata or save services.
- If version-line or broader release-note churn is pulled into this ticket, it will overlap with downstream v0.48 documentation alignment work.
- The live relation graph still carries incoming blocks edges from done prerequisite tickets, which is graph-hygiene noise but not a PO blocker for this refinement.

Split recommendations
- No split recommended; repository evidence already provides one existing SQLite quickstart and one existing privacy proof, and combining them is a single bounded slice.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment