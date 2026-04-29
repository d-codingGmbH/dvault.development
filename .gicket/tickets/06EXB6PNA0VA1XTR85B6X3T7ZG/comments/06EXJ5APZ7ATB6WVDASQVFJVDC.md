[gicket-bot] PO refinement contract

Summary
- Refined the scope-boundary story using the persisted ticket context, existing child documentation tasks, and repository evidence. The MVP concept and deferred-capability documents already establish a bounded v1 Data Vault baseline, so no additional child tickets, relations, attachments, or planning documents are required in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 MVP scope is limited to hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.
- Existing documentation already covers the two sides of this story: docs/architecture/mvp-data-vault-concepts.md defines the MVP concepts and docs/plans/deferred-data-vault-capabilities.md names the deferred capabilities.
- The existing child tasks Task: Document MVP Data Vault concepts (06EXB6PX7ZGYNR2SXF44C5VPJM) and Task: Document deferred Data Vault capabilities (06EXB6Q57D5CRQVGB0ZS29DCSW) are both done and provide sufficient planning evidence for this parent story.
- SQLite-oriented examples are the v1 baseline for concept documentation and tests; provider-specific behavior remains deferred.
- Repository evidence shows source modeling conventions already expose the finite MVP concept vocabulary through DataVaultModelConcept/DataVaultConventions, so the story should ratify that baseline rather than reopen the concept list.
- No new child tickets, relations, attachments, or planning documents were created during this refinement pass.

Scope In
- Ratify the first useful library scope as Data Vault 2.x hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.
- Keep MVP scope documentation aligned with docs/architecture/mvp-data-vault-concepts.md and the existing modeling vocabulary in src/DVault/Modeling.
- Confirm that hubs store stable business identities with business key values, one stable hash key, load timestamp, and record source metadata.
- Confirm that links store relationships between hubs with one relationship hash key, participating hub hash keys, load timestamp, and record source metadata.
- Confirm that satellites store descriptive or contextual history for a hub or link with parent hash key, payload columns, hash diff, load timestamp, and record source metadata.
- Name deferred capabilities explicitly so they do not become hidden MVP requirements.

Scope Out
- PIT table generation, bridge table generation, multi-active satellites, and provider-specific optimizations.
- Schema generation, loading automation, migrations, validation tooling, and provider adapters unless a later implementation ticket explicitly scopes them.
- Final hash-key or hash-diff algorithm selection, normalization rules, delimiter/casing/null-handling rules, and domain field participation beyond the already documented stable hashing contract boundaries.
- Public API naming, internal helper naming, and code-level method shape decisions that developers can choose within the documented architecture.
- Non-SQLite dialect commitments and database-specific physical tuning.

Open questions
- none

Follow-up questions
- Should PIT, bridge, multi-active satellite, and provider optimization work become separate epics or smaller capability stories when post-MVP planning starts?
- Should a later implementation ticket bind Data Vault hash-key and hash-diff generation to the stable hashing contract with explicit domain field normalization rules?
- Should non-SQLite providers receive separate acceptance criteria after the MVP SQLite validation path is complete?
- Should later API design expose link satellites directly, since the current modeling builder evidence shows hub satellites and links but not a visible link-satellite declaration surface?

Risks
- If downstream tickets treat deferred capabilities as implicit MVP requirements, the first package can become too large to implement and test cleanly.
- Hash-key and hash-diff wording can overconstrain future implementation if it drifts from planned persistence conventions into algorithm commitments before the dedicated hashing work owns those decisions.
- The README still references older DCoding.Data.DVault scaffold paths while the current branch also contains src/DVault and tests/DVault.Tests; downstream implementation tickets should follow the active project evidence on their branch rather than relying on stale layout text alone.

Split recommendations
- No additional split is recommended for this story. The already completed child tickets for MVP concepts (06EXB6PX7ZGYNR2SXF44C5VPJM) and deferred capabilities (06EXB6Q57D5CRQVGB0ZS29DCSW) cover the bounded planning work needed for PO-critic review.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment