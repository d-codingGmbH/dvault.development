[gicket-bot] PO refinement contract

Summary
- Refined the ticket into a v0.44 scope decision: DVault may pursue explicit provider-neutral encrypted payload mapping, while SQL Server, PostgreSQL, Oracle, MySQL, SQLite, and DB2 native encryption features remain caller/database-admin guidance and not shared-surface runtime scope. No new child tickets, relation changes, attachments, or planning documents were materialized; the existing downstream split already covers key-provider, conversion-proof, mapping-test, and documentation follow-up.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The done parent story 06FE4R9PP99G6Q1PTPK4TKD460 and docs/architecture/dvault-v1-optional-privacy-extension-boundary.md already fix the boundary: the privacy add-on is explicit, opt-in, provider-neutral in shared core, and not a compliance guarantee, KMS surface, or automatic workflow engine.
- The visible v0.44 release and milestone baseline already narrows implementation scope to caller-owned crypto-shredding patterns, personal-data metadata, key-provider boundaries, and provider-neutral encrypted payload mapping; this ticket should not reopen a broader provider-native encryption platform.
- DVault v0.44 should expose only explicit caller-driven encrypted attribute or payload mapping behind provider-neutral EF Core and provider-package seams; database-native encryption features stay guidance unless a later provider-specific ticket proves one exact lane.
- SQL Server TDE, PostgreSQL TDE posture, Oracle TDE, MySQL or MariaDB at-rest encryption, SQLite encrypted-file variants, and DB2 at-rest or native encryption remain database-admin or deployment owned concerns rather than DVault runtime behavior.
- SQL Server Always Encrypted, PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, and similar provider-specific function, driver, or key-store integrations do not belong in the shared v0.44 contract without separate provider evidence, diagnostics, and tests.
- No additional child tickets, relation changes, attachments, or planning documents were materialized in this refinement pass; the current outgoing blocks split to 06FE4RA88AV7ZRRPMDS8YADEX4, 06FE4RASEQZN7XEYH1XR4H06PR, 06FE4RB219AXVF2535MFF36PN4, and 06FE4RBK2MJBS5K3C15JTB8Z9W already covers the downstream work.

Scope In
- Decide the v0.44 product posture for provider-native encryption capabilities across the current supported-provider baseline: SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- Ratify provider-neutral EF Core value conversion or equivalent explicit encrypted payload mapping as the only approved shared-surface encryption lane for v0.44.
- Document the ownership boundary between DVault-exposed explicit library behavior and caller or database-admin owned encryption configuration, keys, deployment, and operations.
- Record per-provider guidance for SQL Server, PostgreSQL, Oracle, MySQL, SQLite, and DB2 without turning database-native features into promised DVault runtime behavior.

Scope Out
- Shipping provider-native cell, column, or row encryption as a DVault-owned v0.44 runtime feature.
- Automatic key storage, key rotation, KMS or HSM ownership, secret management, or crypto-shredding execution inside DVault.
- Provider-specific DDL, SQL function generation, migration automation, or deployment automation for Always Encrypted, pgcrypto, DBMS_CRYPTO, TDE, or similar features.
- Treating database-level at-rest encryption as equivalent to DVault field-level privacy semantics.
- Adding a separate MariaDB capability baseline or any provider outside the repository's visible provider set without separate evidence.

Open questions
- none

Follow-up questions
- If DVault ever wants a native provider lane, which single provider and single feature should be first: <redacted> Server Always Encrypted, PostgreSQL pgcrypto, Oracle DBMS_CRYPTO, a MySQL-specific function-based lane, or a different provider-backed proof?
- Does the product want a separate future evaluation ticket for MariaDB-specific behavior, given the current repository baseline does not ratify MariaDB as its own provider-capability profile?
- After the provider-neutral proof lands, should later documentation include a non-normative comparison matrix of caller-owned versus database-admin owned encryption options for adopters?

Risks
- Without explicit non-goal wording, downstream work could overread the privacy extension as approval for provider-specific encryption platform work, KMS ownership, or database-feature automation.
- Cross-provider native encryption semantics differ enough that a shared abstraction could become misleading or untestable if DVault promises one before provider-specific evidence exists.
- MariaDB, SQLite encrypted builds, and other variant provider environments can create false support expectations if the ticket does not keep the visible provider baseline finite.
- Database-level at-rest features such as TDE can be mistaken for field-level privacy behavior unless the contract keeps those responsibilities application- and admin-owned.

Split recommendations
- No additional split is needed now; the existing downstream tickets already cover key-provider design, provider-neutral conversion proof, mapping tests, and documentation after this decision ticket.
- If a future native provider lane is approved, split it into one ticket per provider and per exact capability rather than a broad multi-provider encryption story.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment