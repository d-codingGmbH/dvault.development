[gicket-bot] PO refinement contract

Summary
- Refined the ticket as active implementation work, not closure-only, and bound any future closure review to landed binary-first quickstart updates plus a visible compatibility caveat on the named surfaces.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now treats this as remaining implementation work, not a closure-only ticket. Current repository evidence still shows the quickstart path on the default-only setup, so the ticket stays open for delivery.
- critic-item-2: `answered` - The current delivery contract stays in force, but the handoff is normal development work after PO-critic rather than closure routing. Closure remains blocked until the documentation and runnable quickstart updates named by the contract are landed.
- critic-item-3: `answered` - Closure-ready review now explicitly depends on landed repository evidence in the root README quickstart, docs/getting-started.md, examples/README.md, and both runnable quickstart programs, including the existing-project compatibility caveat in the primary quickstart path itself.
- critic-item-4: `answered` - The owner branch still lacks the required landed quickstart updates. The visible branch snapshot and referenced quickstart code still show default registration paths with no binary-first profile call.
- critic-item-5: `answered` - The repository still contradicts the intended binary-first recommendation because the visible quickstart docs and runnable example code continue to model the default-only setup instead of the binary-first new-project path. The root README and SQLite quickstart remain part of that same outstanding surface set in the authoritative ticket contract.
- critic-item-6: `answered` - The compatibility caveat must be added directly where the quickstart introduces the binary-first recommendation. Existing storage-contract language elsewhere remains baseline evidence, but it is not a substitute for quickstart-path visibility.

Clarifications
- This ticket remains normal implementation work. Current branch evidence still shows the quickstart path on the default-only setup, so closure is not in scope for this handoff.
- The existing delivery contract remains the correct scope: update the root README quickstart, docs/getting-started.md, examples/README.md, and the runnable SQLite/PostgreSQL quickstarts to recommend the shipped binary-first APIs for new projects.
- Any future closure-ready review must be based on landed repository evidence on those named surfaces, including a visible existing-project compatibility caveat in the primary quickstart path itself.
- No child tickets, relation changes, description writes, attachments, or planning documents were materialized in this refinement run.

Scope In
- Update the primary new-project quickstart path in the root README, docs/getting-started.md, examples/README.md, and the runnable SQLite/PostgreSQL quickstarts so new projects are shown the binary-first recommendation.
- Use the shipped named APIs that match each example style: UseBinaryFirstProfile() for registry-backed AddDVault(...) quickstarts and UseDataVaultBinaryFirstProfile() for direct ModelBuilder code-first quickstarts.
- Add an explicit existing-project caveat in the quickstart path stating that existing databases/configurations are not migrated automatically and that HexString-compatible setups remain valid until an intentional reviewed change is performed.
- Keep the public hash-key contract intact by stating that logical/public hash-key values remain lowercase hexadecimal strings even when binary physical storage is recommended for new projects.

Scope Out
- Changing runtime defaults, provider capability code, hash algorithms, or the already-shipped binary-first API surface.
- Automatic migration, backfill, dual-write, repair, or any promise that an existing HexString-backed database can switch profiles without a separate reviewed plan.
- Broader release-note, changelog, benchmark, or migration-guide expansion beyond the bounded quickstart-path updates and the minimal compatibility caveat needed there.

Open questions
- none

Follow-up questions
- Coordinate with sibling ticket 06FBSC0TMZBXVVECGQGESWPCY4 on whether the same recommendation and caveat wording should be echoed outside the quickstart path in release-note or changelog surfaces.
- After the quickstart path lands, decide whether a separate existing-project migration guide is needed or whether the explicit non-goal and compatibility caveat remain sufficient for this release.

Risks
- If the binary-first recommendation is added without an equally visible compatibility caveat in the quickstart path, readers can misread the docs as promising automatic migration for existing persisted databases.
- If README or getting-started text is updated but examples/README.md or the runnable quickstart programs stay on the default-only setup, the quickstart path remains internally inconsistent and weakens the recommendation.
- If future routing treats this as closure-ready before the named surfaces are actually landed, the ticket can regress into the same unsupported closure posture flagged by PO-critic.

Split recommendations
- No further split is justified. The remaining work is a bounded quickstart and runnable-example documentation pass, while broader release-note or changelog follow-up already has sibling ownership in ticket 06FBSC0TMZBXVVECGQGESWPCY4.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment