[gicket-bot] PO refinement contract

Summary
- Refined the ticket against the existing privacy boundary and provider matrix; no child tickets, relation writes, or description updates were materialized, the existing done blockers remain historical, and the ticket still blocks 06FE4RBK2MJBS5K3C15JTB8Z9W.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket validates the provider-neutral encrypted attribute conversion proof from 06FE4RASEQZN7XEYH1XR4H06PR; it does not invent a provider-native encryption lane.
- The supported provider baseline for this test matrix is the current visible DVault profile set: sqlite-v1, postgres-v1, sqlserver-v1, oracle-v1, db2-v1, and mysql-pomelo-v1.
- At the shared-core mapping layer, encrypted payload storage stays on the existing PayloadText baseline rather than introducing provider-native encrypted column kinds or SQL crypto features.
- The visible provider store types for that baseline are SQLite TEXT, PostgreSQL text, SQL Server nvarchar(max), Oracle CLOB, DB2 CLOB, and MySQL longtext.
- MySQL provider-name aliases remain one capability profile decision surface; provider mapping assertions should cover the shared MySQL profile once unless a test is explicitly about provider-name selection.
- No relation changes were materialized in this pass; incoming blocks from done tickets 06FE4RAGWXQCQFCTX7QW1T9NAC and 06FE4SENE1ZV45P8DKRQTMG0A0 are already satisfied, and this ticket still blocks 06FE4RBK2MJBS5K3C15JTB8Z9W.

Scope In
- Add automated tests for the provider-neutral encrypted payload mapping path introduced by the privacy conversion-proof work, not just generic payload profile coverage in isolation.
- Cover the finite supported-provider matrix across SQLite, PostgreSQL, SQL Server, Oracle, DB2, and MySQL.
- Assert the provider column type, provider profile, logical property kind, and value-format facts exposed by EF metadata translation or the equivalent encrypted payload mapping surface.
- Verify deterministic unsupported-case diagnostics when the encrypted payload mapping path is asked to use a profile that does not declare the required payload capability.
- Make provider caveats explicit in test names and assertions, including the single shared MySQL profile and any existing gated integration-only limits.

Scope Out
- Provider-native encryption features such as SQL Server Always Encrypted, PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, MySQL function-based encryption, SQLite encrypted-file variants, or DB2 native encryption.
- Designing the caller-owned key-provider or crypto-shredding lifecycle; that belongs to 06FE4RA88AV7ZRRPMDS8YADEX4.
- Implementing the provider-neutral encrypted attribute conversion proof itself; that belongs to 06FE4RASEQZN7XEYH1XR4H06PR.
- Documentation examples and adopter guidance; that remains downstream in 06FE4RBK2MJBS5K3C15JTB8Z9W.
- Adding new provider profiles, a separate MariaDB baseline, or any new provider-specific DDL contract.

Open questions
- none

Follow-up questions
- After this mapping matrix lands, should 06FE4RBK2MJBS5K3C15JTB8Z9W publish the exact provider store-type table or summarize the behavior at the provider-neutral contract level only?
- If future privacy work needs a binary ciphertext storage profile instead of ordinary payload text storage, should that be introduced as a separate storage-policy ticket before any provider mapping changes are attempted?
- If delivery sequencing becomes operationally noisy, does the team want an explicit relation linking 06FE4RASEQZN7XEYH1XR4H06PR and this test ticket, or is the current sibling split sufficient?

Risks
- The current repository still shows only the privacy skeleton plus ordinary payload mappings; if the encrypted payload conversion surface from 06FE4RASEQZN7XEYH1XR4H06PR does not land alongside this task, developers may end up writing placeholder tests against the wrong seam.
- Because generic payload store-type coverage already exists in provider profile tests, this ticket can appear done without actually proving the privacy-specific encrypted payload lane unless the tests bind to that explicit path.
- This ticket currently blocks 06FE4RBK2MJBS5K3C15JTB8Z9W, so vague provider caveats or unsupported-case wording here will cascade into documentation churn.

Split recommendations
- No split is needed for the current finite provider-matrix test scope.
- If live provider coverage expands beyond the existing gated fixtures, keep the unit or metadata matrix in this ticket and move heavier environment-specific smoke coverage into a separate follow-up.
- If future work wants provider-native encryption behavior or non-text ciphertext storage, split it per provider or per storage policy instead of widening this test ticket.

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