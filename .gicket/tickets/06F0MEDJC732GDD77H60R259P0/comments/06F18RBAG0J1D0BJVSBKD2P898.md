[gicket-bot] PO refinement contract

Summary
- Refined the docs/release ticket into a bounded v0.6.0 documentation contract. Existing evidence shows the README is still on 0.5.0 and metadata-first, while the related Code-First, typed read, diagnostics, and examples work is complete; no child tickets, relation changes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The README update owns changing the durable install guidance from 0.5.0 to 0.6.0 for the coordinated DVault package family.
- The recommended README happy path should lead with the implemented Code-First EF model declaration flow, then explicit save and typed latest/as-of reads.
- The metadata-first and registry-backed paths remain supported and should be documented as compatible or advanced options, not removed.
- Do not imply a public Code-First-to-registry bridge; related example evidence shows the runnable SQLite and PostgreSQL quickstarts use a shared DataVaultMetadataModel registered through AddDVault options and consumed with UseDataVaultMetadata().
- The current examples/README.md already documents SQLite and PostgreSQL quickstart commands, AddDVaultPostgres(), DVAULT_TEST_POSTGRES_CONNECTION_STRING, explicit saves, and typed latest/as-of reads; root README should link to it instead of duplicating all example-local detail.
- Release notes should follow the existing docs/releases/v0.5.0.md shape and the manual NuGet publication checklist: package scope, version, date or intended date, highlights, compatibility notes, known limitations, and validation evidence.

Scope In
- Update README installation guidance for DCoding.Data.DVault and the five provider packages to version 0.6.0.
- Rewrite the README quickstart so the recommended flow appears first: <redacted> DVault, declare Code-First hubs/satellites/links through ApplyDataVaultMetadata(vault => ...), save through IDataVaultSaveService, and read common satellite projections through typed latest/as-of helpers.
- Keep metadata-first DataVaultMetadataModel and registry-backed UseDataVaultMetadata() guidance as supported compatible paths for shared metadata, examples, and advanced usage.
- Add or update v0.6.0 release notes covering Code-First declarations, registry-backed metadata usage, typed read helpers, diagnostics/explain output, and runnable SQLite/PostgreSQL examples.
- Add migration guidance for v0.5 users explaining that metadata-first usage remains valid while new users can prefer Code-First declarations for in-model configuration.
- Document known remaining future work for model-first specs, PIT-backed reads, bridge traversal helpers, PIT/bridge row maintenance, and provider-specific read optimizations.
- Keep package verification aligned with the manual NuGet publication checklist after the documentation changes.

Scope Out
- Implementing or changing product APIs, provider behavior, examples, diagnostics, read services, save services, packaging metadata, or release automation.
- Publishing packages or recording final publish approval.
- Documenting v0.7 model-first behavior as delivered in v0.6.0.
- Claiming PIT-backed reads, bridge traversal reads, model-first import/export, or PIT/bridge row maintenance as shipped v0.6.0 capabilities.
- Adding a public Code-First-to-registry conversion API or presenting one as available.
- Replacing the example-local README content beyond linking to and aligning with it from the root README.

Open questions
- none

Follow-up questions
- Before manual publication, the release operator should replace any placeholder release date or approval evidence in the v0.6.0 release notes with the final audited values.
- After v0.6.0 docs land, a later ticket can decide whether to add a public Code-First-to-registry bridge for examples and shared metadata reuse.
- Future PIT, bridge, and model-first tickets should decide whether their eventual docs belong in README quickstart material or separate advanced guides.

Risks
- The biggest documentation risk is overstating shipped behavior by blending completed Code-First, registry, typed read, and diagnostics work with future model-first, PIT, or bridge capabilities.
- README snippets can drift from the implemented API surface if they imply a Code-First-to-registry bridge or reflection-based typed DTO binding.
- Release notes that omit the six-package coordinated scope or final validation evidence will not satisfy the manual NuGet publication checklist.
- If package verification is skipped, a docs-only change could still miss release packaging or version-alignment regressions.

Split recommendations
- No split recommended. The ticket is bounded to README and v0.6.0 release documentation, and the related implementation/example work is already complete enough to document without creating child tickets.

Persisted contract coverage
- acceptance-criteria items: 9
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment