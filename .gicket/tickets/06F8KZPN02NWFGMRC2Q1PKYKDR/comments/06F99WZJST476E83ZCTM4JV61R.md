[gicket-bot] PO refinement contract

Summary
- Refined the story to the support-bundle-only typed read-model generator contract, clarifying the expected diagnostics for source resolution, fingerprint drift, request-bound PIT/bridge ReadShape failures, and model-first boundary handling; no persistent planning writes were applied.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The generator input boundary remains exactly one authoritative `dvault.support-bundle.v1` additional file; this story does not widen the generator to parse raw `dvault.model.v1`, Code-First callbacks, or literal metadata-first declarations directly.
- `DMV1960` is the authoritative source-resolution diagnostic for missing, invalid, ambiguous, or wrong-version support-bundle inputs.
- `DMV1961` covers `DVaultTypedReadModelMetadataSourceFingerprint` drift and suppresses helper generation until the configured fingerprint matches the authoritative bundle fingerprint.
- `DMV1963` and `DMV1964` are request-bound shape diagnostics: PIT and bridge helpers need matching `diagnostics.readShape` facts as well as compatible explain metadata.
- Under the current v1 baseline, raw or changed model-first artifacts outside the projected support-bundle contract are rejected through the same authoritative-source boundary rather than by adding a direct raw-model parsing lane.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized in this pass.

Scope In
- Support-bundle source-resolution diagnostics for missing, invalid, ambiguous, or incompatible-version inputs.
- Fingerprint drift detection against the authoritative support-bundle metadata-source fingerprint.
- Entity-specific PIT and bridge diagnostics when request-bound `ReadShape` facts are missing, mismatched, or outside the bounded helper contract.
- Model-first boundary handling at the generator input edge without widening inputs beyond projected support bundles.
- Regression coverage and package-local documentation alignment for the affected diagnostics.

Scope Out
- New generator parsing of raw `dvault.model.v1` files or source-visible Code-First/metadata-first declarations.
- Changes to runtime support-bundle export or orchestration, or to how representative PIT/bridge requests are captured.
- New typed helper families beyond the existing satellite, PIT, and bounded bridge v1 contract.
- Provider-specific SQL, maintenance, or query-planning behavior changes.

Open questions
- none

Follow-up questions
- After this story lands, do we want to activate or retire the unused `DMV1968` catalog/README slot so the public diagnostic list matches the shipped behavior?
- Is README-level documentation enough for these diagnostics, or should a later release-note pass add concrete support-bundle failure examples?

Risks
- Current repository evidence is internally inconsistent: the diagnostic catalog and README reserve `DMV1968`, but the executable generator tests currently expect raw `dvault.model.v1` additional files to fall into `DMV1960`.
- Because PIT and bridge evidence is request-bound, incomplete fixture data can accidentally exercise the wrong diagnostic lane and hide regressions.
- This ticket is still a child of `06F8KZP0VKMXGE0JXPZRD1RQDG`, is blocked by `06F8KZP9XJ868GY6GT934QVFH4`, and blocks `06F8KZPZZE8VZEBANP5MPN8HH8`, so dependency drift can delay downstream delivery even after refinement.

Split recommendations
- No split recommended; the remaining work stays bounded to generator diagnostics, tests, and package-local documentation alignment.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment