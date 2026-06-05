[gicket-bot] PO refinement contract

Summary
- Refined the epic as a parent tracker over the existing four-child split, ratified the support-bundle and read-shape generator boundary from current repository evidence, and left no blocking PO questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already fixes the v1 typed-helper contract around exactly one authoritative `dvault.support-bundle.v1` input and the v2 read-plan explain contract around bounded request-bound `readShape` evidence, so this epic should ratify those baselines rather than reopen them.
- No persistent ticket, relation, attachment, or planning-document write was materialized in this pass because the live child split and referenced architecture documents already provide the needed planning boundary.

Scope In
- Analyzer and source-generator diagnostics for stale, missing, malformed, ambiguous, or non-authoritative support-bundle inputs used by typed read-model generation.
- Fingerprint drift diagnostics between the authoritative support bundle and `DVaultTypedReadModelMetadataSourceFingerprint`.
- Diagnostics and skip behavior for missing, incomplete, or unsupported request-bound `readShape` evidence needed for PIT and bridge helper emission.
- Epic-level coordination of tests and documentation that keep the support-bundle-driven typed helper contract aligned with the current README and architecture docs.

Scope Out
- New runtime reflection paths, dynamic query expansion, or generator fallback to raw `dvault.model.v1` inputs.
- New query execution APIs, provider-specific SQL promises, PIT or bridge maintenance scheduling, or runtime maintenance semantics beyond current provider-neutral contracts.
- Broad expansion of read-shape vocabularies or alternate generator input formats beyond the finite baselines already documented.

Open questions
- none

Follow-up questions
- When the remaining child work lands, should the epic also queue relation cleanup if `06F8KZQAWZ7QRGB68KB21C9B0R -> 06F8KZP0VKMXGE0JXPZRD1RQDG` stops being a real blocker and becomes housekeeping only?
- Do adopter-facing docs need an explicit example of supplying representative `CreateSupportBundleDiagnostics` requests so PIT and bridge helper evidence is easier to produce outside the test suite?

Risks
- The live relation set still includes an incoming `blocks` edge from child `06F8KZQAWZ7QRGB68KB21C9B0R`, so epic closure remains operationally dependent on that child until the relation is cleared or satisfied.
- Because PIT and bridge helper emission depends on representative request-bound `readShape` evidence, incomplete support-bundle diagnostics capture can still prevent intended helper generation even when metadata itself is valid.

Split recommendations
- No additional split is recommended in this refinement pass; the existing four-child epic structure should remain the delivery vehicle unless one child grows beyond a single bounded diagnostics theme.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment