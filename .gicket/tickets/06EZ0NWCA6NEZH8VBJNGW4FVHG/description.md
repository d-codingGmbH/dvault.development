<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repaired this ticket as a leaf implementation task. It is not a tracking-only parent and does not require child tickets before Dev can work.
- Repository evidence shows the multi-active driving-key contract and core validation/persistence seams already exist; the remaining work is a bounded durable-docs and coverage pass.
- The previous PO-critic complaint about missing children is resolved by this explicit leaf-task contract, not by adding artificial children.

### PO Handoff
- decision: `ready_for_dev`
- meaning: ticket can be claimed by Dev directly

### Clarifications
- This ticket is intentionally a leaf task under story 06EZ0NVN71BN0QWJDCWGVZ2PYG; it should not be treated as a tracking-only parent.
- The driving-key contract is already fixed by `docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md`; this ticket should document and verify that contract rather than invent new API, ordering, or identity rules.
- Existing tests already cover several technical seams: metadata validation in `DataVaultMetadataTests`, EF projection order in `DataVaultEfMetadataTranslationTests`, save-operation validation in `ExplicitDataVaultSaveServiceTests`, and SQLite persistence in `ExplicitDataVaultSaveServiceSqliteTests`.
- `README.md` currently mentions multi-active satellites only as a deferred opt-in capability; this ticket should add durable discoverable documentation of the driving-key concept with one minimal example and explicit future-work boundaries.
- Existing incoming parent/block relations remain valid; no outgoing parentOf child tickets are expected for this leaf task.

### Scope In
- Add or update durable repository docs outside planning-only notes to explain multi-active satellite opt-in, driving keys, canonical ordering, and one minimal example.
- Ratify or extend the maintained unit/integration suites so they assert multi-active validation, EF projection order, and SQLite persistence behavior against the already-established contract.
- Document unsupported or deferred cases as future work, including PIT over multi-active satellites and provider-specific optimized multi-active save behavior.

### Scope Out
- New public API design for multi-active satellites beyond the already-documented contract.
- PIT semantics for multi-active satellites, bridge interactions, or link-based PIT support.
- Provider-specific concurrency, upsert, retry, or optimization guarantees beyond the provider-neutral and SQLite baseline.
- Product-code changes unrelated to documentation or test coverage for the established multi-active behavior.
- Creating child tickets just to satisfy a tracking-parent pattern; this ticket is deliberately small enough for direct Dev implementation.

## Acceptance Criteria
- At least one durable repository doc page explains that multi-active satellites are opt-in via driving keys, defines the driving-key purpose, and includes one minimal end-to-end example using declared driving keys and save values.
- Documentation states that driving-key names are canonical in declaration order, driving-key values are matched by logical name, and `hashDiff` continues to represent payload state rather than driving-key identity.
- Tests in existing suites cover invalid multi-active declarations and save inputs, including empty or duplicate driving-key names, overlap with payload names, missing required keys, extra keys, duplicate keys, and null values.
- Tests verify provider-neutral EF projection for multi-active satellites, including driving-key column placement/order and the expanded `(parent hash key, driving keys..., load timestamp)` primary-key and index shape.
- SQLite persistence tests verify that different driving-key tuples for the same parent can coexist, caller enumeration order does not change canonical matching, and repeated or changed saves follow the documented reuse and history behavior.
- Durable docs explicitly list unsupported or deferred scenarios as future work instead of implying current support.

## Definition of Done
- Durable docs and the cited tests both align with `docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md` and the current provider-neutral implementation.
- Coverage lives in the existing `tests/DCoding.Data.DVault.Tests` unit and integration projects rather than parallel ad hoc test assets.
- Repository documentation does not contradict the README deferred-capability framing and clearly keeps multi-active support opt-in.
- The ticket can move from Dev to Test without another PO/PO-critic loop caused by missing child-ticket expectations.

## Implementation Notes
- Use `docs/plans/multi-active-satellite-driving-key-contract-06EZ0NVX3R-06EZ0NW61G.md` as the normative source for terminology, validation rules, canonical order, and the minimal customer contact example.
- Expected test seams: `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`.
- Source behavior to document and verify is visible in `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs`, `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`, and `src/DCoding.Data.DVault/DataVaultSaveService.cs`.

## Open Questions
- none

## Follow-Up Questions
- Should a later docs ticket add a broader capability matrix or tutorial-style example once PIT, bridge, and multi-active capabilities all have stable public documentation?
- Should provider-specific packages later document when optimized save strategies accept or decline multi-active batches beyond the provider-neutral fallback?
- When PIT work resumes, should the docs cross-link this future-work note to the PIT story so unsupported multi-active PIT semantics stay consistent?

## Risks
- If docs land only in planning notes, package consumers still will not see the driving-key contract in durable discoverable documentation.
- Because `README.md` still frames multi-active as deferred, careless wording could overstate current support or imply PIT or provider-optimized behaviors that the code explicitly treats as unsupported or deferred.
- Rewriting tests instead of extending the current suites could create redundant coverage and drift from the existing provider-neutral baseline.

## Split Recommendations
- No split recommended; this is a bounded leaf documentation and coverage task anchored to an existing contract and existing test seams.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: complete documentation and test coverage for multi-active satellite support.

Acceptance Criteria:
- Documentation explains the driving-key concept with one minimal example.
- Tests cover configuration validation and persistence behavior.
- Unsupported scenarios are listed as future work instead of hidden assumptions.