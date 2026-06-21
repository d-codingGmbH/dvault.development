[gicket-bot] PO refinement contract

Summary
- Refined the v0.43 docs-consolidation task around already-landed binary-first, analyzer, provider-matrix, and allocation evidence so the ticket can advance without reopening product-boundary questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The active release context already fixes this lane as `v0.43.0 - Binary Adoption, Analyzer DX, and Runtime Efficiency`; touched release-facing docs must keep the release-label versus package-line distinction explicit and must never document a consumer-facing `0.43.0` package version.
- Public DVault hash-key values remain canonical lowercase hexadecimal strings even when physical storage is `Binary`; storage-profile, algorithm-id, digest-length, or truncation changes after persistence remain caller-owned compatibility work with no automatic DVault rehash, backfill, dual-write, repair, or migration behavior.
- The analyzer boundary is already fixed by repository evidence: `DCoding.Data.DVault.Analyzers` remains optional local tooling with `PrivateAssets="all"`, one `net10.0` analyzer asset, and a `.NET 10 SDK` build-host baseline for both coordinated package lines; the repository still does not claim pure `.NET 8 SDK` analyzer-host compatibility.
- Measured evidence sources already exist and should be cited rather than reinterpreted: the provider binary-vs-hex bundle under ticket `06FE4R1N2ADN77NDFDP4GR7020`, the hotspot baseline under done ticket `06FE4R1XJVQZTQ8S9WN2YE3ZKW`, the refreshed allocation evidence under done ticket `06FE4R261S2FSQ786S4F4JE90R`, and the caller-owned migration guide plus dry-run manifest lane under done ticket `06FE4R0TBG8JP5WA2SHXKH438M`.
- Current repository docs still advertise the v0.42.0 baseline and `docs/releases/v0.43.0.md` does not yet exist, so this ticket is the bounded documentation lane that moves the public guidance forward to the v0.43 evidence baseline.

Scope In
- Add `docs/releases/v0.43.0.md` and a matching `CHANGELOG.md` v0.43.0 entry that summarize binary adoption guidance, analyzer DX, provider binary-vs-hex evidence, and targeted allocation reductions with explicit evidence and non-goal boundaries.
- Advance the current-baseline docs that still point at v0.42.0 so they consistently present the v0.43 binary-first guidance, analyzer local-tooling posture, code-first convenience path, and release/package-line distinction on the touched surfaces.
- Update performance and evidence-facing docs to cite the checked-in hash-key matrix bundle, footprint sidecars, hotspot sidecars, and refreshed benchmark triplets by their actual artifact labels and run contexts.
- Update migration/adoption guidance so existing persisted `HexString` users are routed to `docs/hash-key-storage-migration.md` and the dry-run manifest workflow instead of any implied automatic migration path.
- Keep analyzer-facing docs aligned with the existing project-local diagnostics scope, supported diagnostic ranges, and `.NET 10 SDK` build-host claim without widening analyzer behavior or compatibility claims.

Scope Out
- Runtime, analyzer, or benchmark-harness implementation changes.
- New benchmark reruns, provider setup work, or artifact-schema redesign.
- Automatic migration, rehash, backfill, dual-write, repair, or public `byte[]` hash-key behavior.
- New provider-wide timing claims from skipped, failed, diagnostics-only, smoke-only, or storage-footprint rows.
- Package publication approval, signed NuGet push, or release automation outcomes.

Open questions
- none

Follow-up questions
- After v0.43 docs land, do any provider-specific binary-storage caveats warrant separate post-v0.43 adopter guidance instead of one shared baseline note?
- Should a later release promote `--allocation-hotspots` from an opt-in benchmark lane to a standard release-validation companion artifact, or should it remain a focused diagnostics tool?
- If future evidence proves pure `.NET 8 SDK` analyzer consumption, should that be handled as a separate compatibility ticket instead of broadening the current analyzer claim retrospectively?

Risks
- Docs can overstate binary-storage wins or allocation reductions if they summarize failed, skipped, diagnostics-only, smoke-only, or storage-footprint rows as general performance results.
- Docs can accidentally regress product clarity if they present binary-first as an automatic migration path or imply a public byte-array hash-key model.
- Release-facing guidance can drift if versioned install pages, analyzer install guidance, release notes, and adopter checklists are not updated coherently on the same current-baseline story.

Split recommendations
- No new split is needed; this ticket is already the bounded release-note and docs-consolidation lane downstream of the done migration, analyzer, benchmark, and allocation tickets.
- If later evidence supports materially different provider-specific adoption guidance, capture that in a separate post-v0.43 documentation ticket instead of widening this shared baseline update.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment