[gicket-bot] PO refinement contract

Summary
- Re-routed ticket 06FE4R2EGQ444EGPKZBRZCDEV8 from an invalid closure-only path back to a normal pre-development docs task for dev; repository evidence still shows missing v0.43 deliverables, and no child tickets, relation changes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Ticket routing is corrected to a normal pre-development documentation task for dev. Closure-only is not valid because the contract still requires new repository docs work and the repository does not yet contain those deliverables.
- critic-item-2: `answered` - The refined contract stays as remaining docs implementation work, but the closure-only assumption is removed from workflow context. The next owner should land the release note, changelog, and current-baseline docs updates before closure evidence is claimed.
- critic-item-3: `answered` - No closure-only rewrite is used. Closure-only would require auditable landed paths for the v0.43 release note and changelog updates, and those landed paths are absent in the current repository evidence.
- critic-item-4: `answered` - The contract is now explicitly treated as remaining implementation work, not closure evidence. Its scope, acceptance criteria, and definition of done stay valid for a dev docs task because they describe work to add, not already-landed evidence.
- critic-item-5: `answered` - Confirmed. Branch implementation for the required docs work is not present yet; the only reported changes versus `develop` are `.gicket` ticket metadata, so this ticket must go back through normal dev execution.
- critic-item-6: `answered` - Confirmed. The required deliverables are still absent: `docs/releases/v0.43.0.md` does not exist, and `CHANGELOG.md` still has no `v0.43.0` entry.

Clarifications
- This is a normal pre-development documentation task for `dev`, not a closure-only or no-work-required ticket.
- Current repository evidence still exposes the public docs baseline as v0.42.0 and lacks `docs/releases/v0.43.0.md`, so the v0.43 docs story must be implemented rather than merely audited.
- Public DVault hash-key values remain lowercase hexadecimal strings even when physical storage is `Binary`; post-persistence storage or algorithm changes remain caller-owned migration work routed through `docs/hash-key-storage-migration.md` and the dry-run manifest workflow.
- Analyzer guidance remains bounded to local `PrivateAssets='all'` usage, one `net10.0` analyzer asset, and a `.NET 10 SDK` build-host baseline; do not broaden the claim to pure `.NET 8 SDK` analyzer-host compatibility.
- This refinement keeps the scoped docs contract intact and only corrects workflow interpretation; no child tickets, relation changes, attachments, or planning documents were materialized.

Scope In
- Add `docs/releases/v0.43.0.md` summarizing binary adoption guidance, analyzer DX, provider binary-vs-hex evidence, and allocation evidence with explicit caveats and non-goal boundaries.
- Add a matching `CHANGELOG.md` v0.43.0 entry aligned with the v0.43 release note.
- Update current-baseline docs that still point at v0.42.0 so they present the v0.43 binary-first, analyzer, performance, and release-versus-package-line story consistently on touched surfaces.
- Update performance and adoption guidance to cite the checked-in hash-key matrix bundle, footprint sidecars, hotspot artifacts, refreshed allocation evidence, and migration guide by their actual labels and measured boundaries.
- Keep analyzer docs aligned with the existing project-local diagnostics scope, supported diagnostic ranges, `PrivateAssets='all'`, and `.NET 10 SDK` build-host guidance without widening analyzer behavior or compatibility claims.

Scope Out
- Runtime, analyzer, benchmark-harness, or provider implementation changes.
- New benchmark reruns, provider setup work, or artifact-schema redesign.
- Automatic migration, rehash, backfill, dual-write, repair, or public `byte[]` hash-key behavior.
- New provider-wide timing claims derived from skipped, failed, diagnostics-only, smoke-only, or storage-footprint rows.
- Package publication approval, signed NuGet push, or release automation outcomes.

Open questions
- none

Follow-up questions
- After v0.43 docs land, do any provider-specific binary-storage caveats warrant separate post-v0.43 adopter guidance instead of one shared baseline note?
- Should a later release promote `--allocation-hotspots` from an opt-in benchmark lane to a standard release-validation companion artifact, or should it remain a focused diagnostics tool?
- If future evidence proves pure `.NET 8 SDK` analyzer consumption, should that be handled as a separate compatibility ticket instead of broadening the current analyzer claim retrospectively?

Risks
- Docs can overstate binary-storage wins or allocation reductions if they summarize skipped, failed, diagnostics-only, smoke-only, or storage-footprint rows as general results.
- Docs can regress product clarity if they present binary-first as an automatic migration path or imply a public `byte[]` hash-key model.
- Release-facing guidance can drift if README, release notes, package compatibility, analyzer install guidance, validation guidance, and adoption docs are not updated coherently on the same current-baseline story.
- Because the branch currently lacks documentation implementation beyond ticket metadata, closure evidence still depends on dev landing the repository docs changes.

Split recommendations
- No split is needed; the remaining work is already a bounded v0.43 docs-consolidation lane for release notes, baseline docs, analyzer guidance, and performance evidence citations.
- If later evidence supports materially different provider-specific binary-storage guidance, capture that in a separate post-v0.43 documentation ticket instead of widening this shared baseline update.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment