[gicket-bot] PO refinement contract

Summary
- Revised the PO contract to address the critic finding by grounding the MVP concept boundary in visible planning documents and naming policy evidence, while removing any claim that DataVaultModelConcept or DataVaultConventions.ModelConcepts already exist in source. No new child tickets, relations, attachments, or planning documents are needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is restated so the authoritative baseline comes from docs/architecture/mvp-data-vault-concepts.md, docs/plans/deferred-data-vault-capabilities.md, docs/naming/default-naming-policy.md, docs/plans/stable-hashing-contract.md, and docs/plans/dvault-v1-default-persistence-convention-policy.md. DataVaultModelConcept and DataVaultConventions.ModelConcepts are not required as pre-existing source evidence; if a later implementation chooses those names, it may create or adjust them within this documented scope.
- critic-item-2: `answered` - The refined contract no longer presents DataVaultConventions.ModelConcepts or DataVaultModelConcept as existing implementation evidence. It treats them only as permissible downstream implementation names if source work needs a finite concept vocabulary, and it keeps the current story's evidence anchored in documentation and visible tests/source layout.
- critic-item-3: `answered` - The blocking finding is resolved by replacing the prior source-API claim with a documentation-grounded scope statement. The story ratifies the finite MVP concept list from the concept document and naming policy, and explicitly avoids claiming DataVaultModelConcept or DataVaultConventions.ModelConcepts are already implemented unless a later source inspection in a downstream ticket proves that separately.

Clarifications
- The v1 MVP scope is limited to hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.
- The authoritative evidence for this story is the visible planning and architecture documentation, not an assumed existing source API for DataVaultModelConcept or DataVaultConventions.ModelConcepts.
- Existing documentation covers the two sides of this story: docs/architecture/mvp-data-vault-concepts.md defines the MVP concepts and docs/plans/deferred-data-vault-capabilities.md names deferred capabilities.
- The existing child tasks Task: Document MVP Data Vault concepts (06EXB6PX7ZGYNR2SXF44C5VPJM) and Task: Document deferred Data Vault capabilities (06EXB6Q57D5CRQVGB0ZS29DCSW) are already linked as parentOf children and provide sufficient planning evidence for this parent story.
- SQLite-oriented examples are the v1 baseline for concept documentation and tests; provider-specific behavior remains deferred.
- No new child tickets, relations, attachments, or planning documents are required for this refinement pass.

Scope In
- Ratify the first useful library scope as Data Vault 2.x hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources.
- Keep MVP scope documentation aligned with docs/architecture/mvp-data-vault-concepts.md and the naming expectations in docs/naming/default-naming-policy.md.
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
- Any claim that DataVaultModelConcept or DataVaultConventions.ModelConcepts already exists in source for this ticket unless a later implementation ticket verifies that source evidence directly.

Open questions
- none

Follow-up questions
- Should PIT, bridge, multi-active satellite, and provider optimization work become separate epics or smaller capability stories when post-MVP planning starts?
- Should a later implementation ticket bind Data Vault hash-key and hash-diff generation to the stable hashing contract with explicit domain field normalization rules?
- Should non-SQLite providers receive separate acceptance criteria after the MVP SQLite validation path is complete?
- Should later API design expose link satellites directly, since the current modeling builder evidence shows hub satellites and links but not a visible link-satellite declaration surface?
- Should a downstream source ticket introduce an explicit finite model concept enum or convention surface, and if so should it use the names DataVaultModelConcept and DataVaultConventions.ModelConcepts?

Risks
- If downstream tickets treat deferred capabilities as implicit MVP requirements, the first package can become too large to implement and test cleanly.
- Hash-key and hash-diff wording can overconstrain future implementation if it drifts from planned persistence conventions into algorithm commitments before the dedicated hashing work owns those decisions.
- The README still references older DCoding.Data.DVault scaffold paths while the current branch also contains src/DVault and tests/DVault.Tests; downstream implementation tickets should follow the active project evidence on their branch rather than relying on stale layout text alone.
- If future source work assumes DataVaultModelConcept or DataVaultConventions.ModelConcepts already exists without verifying source, it may repeat the evidence gap identified by PO-critic.

Split recommendations
- No additional split is recommended for this story. The already completed child tickets for MVP concepts (06EXB6PX7ZGYNR2SXF44C5VPJM) and deferred capabilities (06EXB6Q57D5CRQVGB0ZS29DCSW) cover the bounded planning work needed for PO-critic review.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 6
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment