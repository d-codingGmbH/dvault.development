<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket as active implementation work, not closure-only, and bound any future closure review to landed binary-first quickstart updates plus a visible compatibility caveat on the named surfaces.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket remains normal implementation work. Current branch evidence still shows the quickstart path on the default-only setup, so closure is not in scope for this handoff.
- The existing delivery contract remains the correct scope: update the root README quickstart, docs/getting-started.md, examples/README.md, and the runnable SQLite/PostgreSQL quickstarts to recommend the shipped binary-first APIs for new projects.
- Any future closure-ready review must be based on landed repository evidence on those named surfaces, including a visible existing-project compatibility caveat in the primary quickstart path itself.
- No child tickets, relation changes, description writes, attachments, or planning documents were materialized in this refinement run.

### Scope In
- Update the primary new-project quickstart path in the root README, docs/getting-started.md, examples/README.md, and the runnable SQLite/PostgreSQL quickstarts so new projects are shown the binary-first recommendation.
- Use the shipped named APIs that match each example style: UseBinaryFirstProfile() for registry-backed AddDVault(...) quickstarts and UseDataVaultBinaryFirstProfile() for direct ModelBuilder code-first quickstarts.
- Add an explicit existing-project caveat in the quickstart path stating that existing databases/configurations are not migrated automatically and that HexString-compatible setups remain valid until an intentional reviewed change is performed.
- Keep the public hash-key contract intact by stating that logical/public hash-key values remain lowercase hexadecimal strings even when binary physical storage is recommended for new projects.

### Scope Out
- Changing runtime defaults, provider capability code, hash algorithms, or the already-shipped binary-first API surface.
- Automatic migration, backfill, dual-write, repair, or any promise that an existing HexString-backed database can switch profiles without a separate reviewed plan.
- Broader release-note, changelog, benchmark, or migration-guide expansion beyond the bounded quickstart-path updates and the minimal compatibility caveat needed there.

## Acceptance Criteria
- The root README quickstart, docs/getting-started.md, examples/README.md, and the runnable SQLite/PostgreSQL quickstart setup code show the binary-first profile as the recommended new-project setup using the shipped named API that matches each example style.
- The primary quickstart path explicitly states that existing databases/configurations are not migrated automatically and that HexString-compatible setups remain valid until the adopter intentionally plans and executes a separate compatibility change.
- Quickstart wording keeps the public hash-key contract intact by stating that logical/public hash-key values remain lowercase hexadecimal strings even when binary physical storage is recommended for new projects.
- The runnable SQLite and PostgreSQL quickstarts and their surrounding README snippets no longer model the default-only path as the recommended setup for new projects.
- No quickstart example text implies that switching to binary-first performs provider DDL changes, data backfill, or seamless migration for an existing database.

## Definition of Done
- The named quickstart surfaces are landed with one coherent binary-first recommendation for new projects across the root README, docs/getting-started.md, examples/README.md, and the runnable SQLite/PostgreSQL quickstarts.
- A visible compatibility note is present in the quickstart path itself explaining that existing persisted databases stay on the compatible path unless the adopter intentionally plans and executes a separate migration, reset, or data-move decision.
- Any remaining default AddDVault() or direct-model quickstart snippet in the primary entry path is either converted to the binary-first recommendation or explicitly framed as existing-project compatibility guidance rather than the recommended new-project choice.
- PO-critic closure-ready review is deferred until the landed repository evidence on those named surfaces is visible and consistent with the current storage contract.

## Implementation Notes
- Current visible evidence still shows the default path: docs/getting-started.md Register Services uses services.AddDVault(), examples/README.md describes AddDVault(options => options.UseMetadataModel(...)), and the PostgreSQL quickstart Program.cs still registers metadata without UseBinaryFirstProfile().
- The authoritative ticket contract already identifies the remaining paired surfaces beyond the branch snapshot excerpt: the root README quickstart and the SQLite quickstart Program.cs still need the same binary-first update.
- For metadata-first quickstarts, the direct recommendation should chain UseBinaryFirstProfile() with UseMetadataModel(...) on AddDVault(...); for code-first README guidance, use UseDataVaultBinaryFirstProfile() rather than reopening low-level provider capability composition.
- Compatibility wording should stay anchored to the existing storage baseline already documented in docs/getting-started.md and docs/releases/v0.37.0.md: HexString remains the compatible default physical storage profile, while Binary is an explicit physical opt-in that requires intentional migration/storage planning.

## Open Questions
- none

## Follow-Up Questions
- Coordinate with sibling ticket 06FBSC0TMZBXVVECGQGESWPCY4 on whether the same recommendation and caveat wording should be echoed outside the quickstart path in release-note or changelog surfaces.
- After the quickstart path lands, decide whether a separate existing-project migration guide is needed or whether the explicit non-goal and compatibility caveat remain sufficient for this release.

## Risks
- If the binary-first recommendation is added without an equally visible compatibility caveat in the quickstart path, readers can misread the docs as promising automatic migration for existing persisted databases.
- If README or getting-started text is updated but examples/README.md or the runnable quickstart programs stay on the default-only setup, the quickstart path remains internally inconsistent and weakens the recommendation.
- If future routing treats this as closure-ready before the named surfaces are actually landed, the ticket can regress into the same unsupported closure posture flagged by PO-critic.

## Split Recommendations
- No further split is justified. The remaining work is a bounded quickstart and runnable-example documentation pass, while broader release-note or changelog follow-up already has sibling ownership in ticket 06FBSC0TMZBXVVECGQGESWPCY4.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update the quickstart path for new projects to show the binary-first profile as the recommended option while keeping an explicit existing-project compatibility note. Acceptance: quickstart examples do not imply automatic migration for existing databases.