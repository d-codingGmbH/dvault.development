[gicket-bot] PO refinement contract

Summary
- Refined as ready for PO critic: the repository already treats `docs/plans/stable-hashing-contract.md` as the v1 canonicalization manifest and backs it with stable-hash normalizer/service vector tests; no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- For this ticket, the durable manifest is the existing repository document `docs/plans/stable-hashing-contract.md`; a separate JSON/YAML manifest file is not required for v1.
- The v1 binary-handling decision is already bounded by repository evidence: unsupported scalar types such as `byte[]` fail fast before hashing rather than receiving a new binary encoding.
- The compatibility baseline is provider-neutral published vectors plus unit assertions against the shared hashing services, not a provider-specific database matrix.
- No bounded ticket write was applied in this run; live parent/block relations remain unchanged.

Scope In
- Ratify the v1 stable-hashing contract as the authoritative manifest for algorithm id, UTF-8 encoding, lowercase digest shape, null handling, field ordering, delimiter rules, culture invariance, and failure behavior.
- Treat the published normalized-input/digest pairs as the compatibility vectors that future versions and providers must continue to satisfy.
- Guard the shared canonicalization path used by DVault hub and link hash-key computation through normalizer/service tests and negative-case regression coverage.

Scope Out
- Automatic derivation of satellite `hashDiff` values from payload fields; current save APIs still accept caller-supplied hash diff strings.
- Domain-specific field-selection rules for individual hubs, links, or satellites beyond the shared stable-hash contract.
- A new binary canonicalization format, provider-specific hash implementations, or migration tooling for changing `sha256-v1` semantics.

Open questions
- none

Follow-up questions
- If DVault later needs automatic satellite hash-diff generation, should that land as a separate story that defines participating payload fields and publishes its own contract-aligned vectors?
- If binary value hashing becomes a real v1/v2 requirement, should it be introduced as a separately versioned scalar encoding or algorithm identifier instead of altering `sha256-v1` behavior?

Risks
- The current downstream `blocks` relation should remain until this story's documented contract and tests are accepted on the canonical target branch; current branch evidence alone is not closure evidence.
- Changing published scalar encodings, ordering, or failure behavior later without versioning would break the compatibility vectors that downstream hash-key producers depend on.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment