[gicket-bot] PO refinement contract

Summary
- Verified the repository and persisted ticket state, then aligned the epic contract around the shipped v0.22 satellite-only typed-read boundary and queued planning-document supersession for the stale PIT/bridge helper plan surfaces.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The epic now explicitly supersedes docs/plans/typed-read-model-generator-contract.md and done child 06F5Q922T5B21GJN49FYN6DJH0 for the shipped v0.22 boundary, instead of treating them as the active baseline, and it records queued planning-document rewrites to retire the PIT/bridge helper promises on develop.
- critic-item-2: `answered` - The epic baseline references now require reviewers to treat the old planning surfaces as historical, and the updated contract records queued mutations mutation-baa7edf5136439f2 and mutation-67556c67217d884c to rewrite docs/plans/typed-read-model-generator-contract.md and docs/plans/README.md so the README no longer presents that PIT/bridge helper contract as current.
- critic-item-3: `answered` - The authoritative v0.22 story is now unified around support-bundle-driven satellite-only helpers plus stable hash governance. PIT and bridge stay in runtime-read-service or unsupported-shape diagnostic territory, and the old plan document is explicitly treated as superseded historical design context rather than a competing current contract.
- critic-item-4: `answered` - The epic no longer asks reviewers to approve done child 06F5Q922T5B21GJN49FYN6DJH0 as the operative v0.22 contract baseline. It now treats that ticket as historical design context only and points reviewers to the release-note, analyzer, model-first, generator-test, and stable-hash surfaces plus the queued planning-document supersession.

Clarifications
- Epic ticket 06F5Q91V0YGSA6SH9WDS02GH0M revision 06F6XNSSG7AXMGFMWFBDDH7GF8 now treats docs/releases/v0.22.0.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/model-first-governance.md, tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs, and docs/plans/stable-hashing-contract.md as the authoritative v0.22 contract surface.
- Done child 06F5Q922T5B21GJN49FYN6DJH0 and the older docs/plans/typed-read-model-generator-contract.md text are explicitly historical design context for this epic, not the shipped v0.22 boundary.
- Queued planning-document mutations mutation-baa7edf5136439f2 and mutation-67556c67217d884c are recorded in the epic contract to rewrite docs/plans/typed-read-model-generator-contract.md and docs/plans/README.md on develop so PIT and bridge helper promises stop presenting as current v0.22 contract text.
- No new child tickets, attachments, or relation writes were needed; the existing seven parentOf relations remain the authoritative decomposition.

Scope In
- Opt-in typed read-model generation from exactly one authoritative dvault.support-bundle.v1 artifact after metadata projection into EF and DVault metadata.
- Typed Current, Latest, and AsOf satellite helper generation for supported hub-parent, link-parent, and deterministic multi-active satellite shapes over existing IDataVaultReadService reads.
- Explicit DMV1960 through DMV1969 diagnostics for missing, stale, ambiguous, unsupported, colliding, or skipped generated-helper cases.
- Stable hash canonicalization governance, published compatibility vectors, and regression coverage for sha256-v1.
- Documentation and planning-surface alignment for the shipped v0.22 typed-read and hash-governance boundary, including explicit supersession of earlier PIT and bridge helper planning promises.

Scope Out
- Raw dvault.model.v1 additional-file parsing by the typed read-model generator.
- PIT or bridge typed helper emission in the shipped v0.22 boundary.
- Provider-specific SQL generation, dynamic request compilation, or automatic support-bundle routing or publication.
- Runtime boundary expansion beyond existing IDataVaultReadService surfaces and documented consumer-owned compiled EF query alternatives.
- Automatic satellite hashDiff generation, binary scalar hashing changes, or any unversioned change to sha256-v1 semantics.

Open questions
- none

Follow-up questions
- If the product later wants shipped PIT or bridge typed helpers instead of the current runtime or diagnostic-only handling, should that land as a new additive follow-up rather than reopening this epic?
- If automatic satellite hashDiff generation or binary scalar hashing becomes a requirement later, should that ship under a separately versioned contract instead of changing sha256-v1 behavior?

Risks
- Until the queued develop-branch replay lands, readers of docs/plans/typed-read-model-generator-contract.md or docs/plans/README.md on develop can still encounter the older PIT or bridge helper wording that this epic now explicitly supersedes.
- Future docs or implementation work could overstate the shipped typed-read boundary by implying PIT or bridge helper emission before a separate additive ticket lands.
- Any unversioned change to the sha256-v1 canonicalization rules or published vectors would break the compatibility contract this epic establishes.
- If DMV196x unsupported-shape behavior regresses, consumers may no longer distinguish unsupported metadata from misconfiguration, which would blur the current satellite-only boundary.

Split recommendations
- No additional split is recommended now; the existing seven-child decomposition is already persisted and complete for this epic.
- If future work expands into shipped PIT or bridge helpers, automatic hashDiff generation, or new hash encodings, create additive follow-up tickets instead of reopening this parent epic.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment