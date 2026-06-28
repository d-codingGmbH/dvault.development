<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratifies the repository-backed finite provider-native encryption boundary already documented for the optional privacy package; no ticket or planning writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 shared privacy lane is provider-neutral, opt-in, and alias-driven: `DCoding.Data.DVault.Privacy` documents caller-owned encrypted-payload conversion over ordinary EF Core mapped payload properties rather than provider-native runtime encryption behavior.
- The finite provider baseline for this matrix is SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2; the MySQL row covers the repository MySQL profile for `MySql.EntityFrameworkCore` and Pomelo and does not create a separate MariaDB capability profile.
- Provider-native encryption examples such as SQL Server TDE or Always Encrypted, PostgreSQL deployment encryption or `pgcrypto`, Oracle TDE or `DBMS_CRYPTO`, MySQL SQL crypto or file or tablespace encryption, SQLite encrypted-file builds, and DB2 native database encryption are guidance-only and remain outside DVault shared-runtime behavior until a separate provider-specific ticket owns one exact capability.
- Alias-driven EF value conversion and key-provider resolution remain provider-neutral and caller-owned; DVault must not infer keys, key lifecycle, provider capabilities, or runtime dispatch from native provider features.

### Scope In
- Define one authoritative matrix or equivalent finite documentation contract that states, per supported provider, which native encryption capabilities are only environmental or provider guidance and which behaviors DVault explicitly does not own.
- State that the approved v1 implementation boundary is caller-invoked alias-driven encrypted payload conversion through the optional privacy package over ordinary EF Core mapped payload properties.
- Keep documentation wording aligned across the existing privacy architecture contract, README/install guidance, package compatibility guidance, and production adoption guidance where this boundary is already surfaced.
- Preserve the rule that any future native encryption support must be split into separate provider-specific tickets with one exact provider capability each.

### Scope Out
- Implementing provider-native encryption, provider-specific SQL crypto calls, encrypted DDL, capability probing, or runtime dispatch based on provider-native availability.
- Expanding the provider list beyond SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 or introducing a separate MariaDB capability profile in this ticket.
- Owning application or operator responsibilities such as key management, compliance posture, database-at-rest configuration, retention, purge, or crypto-shredding workflows.
- Changing DVault core save, read, or runtime behavior outside the optional opt-in privacy package documentation boundary.

## Acceptance Criteria
- The refined contract explicitly covers SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 and states that native encryption features are guidance-only unless a later provider-specific ticket owns one exact capability.
- The contract clearly distinguishes caller-owned alias-driven encrypted payload conversion in `DCoding.Data.DVault.Privacy` from database-at-rest encryption and provider-native column, cell, or row encryption features.
- The contract states that DVault does not emit provider-native encrypted DDL, call provider SQL crypto functions, probe provider encryption capabilities, or route runtime behavior based on native encryption availability.
- The contract keeps MySQL scoped to the repository MySQL baseline (`MySql.EntityFrameworkCore` and Pomelo) and avoids opening a separate MariaDB capability matrix in v1.
- The contract routes any future native encryption implementation work to separate provider-specific tickets instead of widening the shared provider-neutral privacy package.

## Definition of Done
- Repository-backed documentation contains one consistent v1 boundary statement that matches the privacy architecture contract and consumer-facing guidance.
- No acceptance text or supporting notes imply that DVault itself provides GDPR or DSGVO compliance, automatic encryption, automatic redaction, provider-native encryption support, or hidden runtime negotiation.
- The deliverable remains documentation or planning scope only; no product-code scope is introduced by this ticket refinement.
- Downstream tickets can rely on this refinement without reopening the provider set, ownership boundary, or alias-driven provider-neutral default lane.

## Implementation Notes
- The repository already fixes the core boundary in `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md`, especially the provider-neutral boundary, caller-owned key-provider seam, and provider-native encryption decision sections; refinement should ratify that baseline rather than reopen capability research.
- Consumer-facing docs already repeat the same posture in `README.md`, `docs/package-compatibility.md`, and `docs/production-adoption-checklist.md`; implementation should align wording with those sources instead of inventing a second matrix vocabulary.
- Use the finite provider set already present in the repository and keep examples at the guidance level: SQL Server TDE or Always Encrypted, PostgreSQL deployment encryption or `pgcrypto`, Oracle TDE or `DBMS_CRYPTO`, MySQL SQL crypto or file or tablespace encryption, SQLite encrypted-file builds, and DB2 native database encryption.
- Keep the approved shared lane bounded to alias-driven encrypted payload conversion with caller-owned key resolution; do not broaden this ticket into runtime provider branching, migration work, diagnostics expansion beyond redaction-safe statements, or provider package implementation details.
- Live relation context is unchanged: this ticket remains a child of `06FGX5KZHC9ZAKAT71C89MEYV8` and currently blocks `06FGX5QAZSAB0M0W8FW807GQQR` and `06FGX5R67T2G0FEGMWE0JBEKJ8`, so the boundary wording should be authoritative enough for downstream tickets to consume directly.

## Open Questions
- none

## Follow-Up Questions
- If DVault later wants actual native encryption support, which single provider and exact capability should be prioritized first for a separate bounded ticket.
- After this matrix is accepted, do the dependent blocked tickets need additional child splits by provider capability family, or can they proceed against the shared documentation boundary as written.

## Risks
- Because the same caveat appears in multiple repository documents, partial wording updates could reintroduce contradictory claims about automatic encryption or runtime provider dispatch.
- Readers may still conflate database-at-rest guidance with DVault field-level privacy unless the matrix explicitly separates DVault-owned behavior from application, operator, and database-admin responsibilities.

## Split Recommendations
- Do not split the current refinement further; if future work needs real native encryption behavior, create separate provider-specific tickets for one exact capability at a time, such as SQL Server Always Encrypted, PostgreSQL `pgcrypto`, Oracle `DBMS_CRYPTO`, MySQL SQL crypto or storage encryption, SQLite encrypted-file builds, or DB2 native encryption.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Create the finite provider-native encryption boundary contract for the optional privacy package.

Acceptance:
- SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 have explicit guidance for what DVault does and does not manage.
- The matrix covers column/cell/row or database-at-rest capabilities only as guidance and avoids provider-native runtime dispatch claims.
- The contract states that alias-driven EF value conversion remains provider-neutral and caller-owned.
- Any future native encryption support is routed to separate provider-specific tickets.