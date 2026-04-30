[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the persisted contract has no unresolved Open Questions, current ticket state is routed to po-critic, and local repository evidence confirms the named stable-hash public API, implementation, DI registration, contract document, and unit test surfaces exist.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB765S2X2MR2K18ZBV8RC38/description.md records PO Handoff decision ready_for_po_critic and ## Open Questions contains only '- none'.
- git rev-parse/log observed HEAD 798cb5f3dd10588a1f67b8b648836d04f3e868d9 on ticket/06EXB765S2X2MR2K18ZBV8RC38-story-implement-hash-key-and-hash-diff-services; recent branch history includes cd5ef540 handoff po->po-critic and 798cb5f3 lease claim po-critic.
- docs/plans/stable-hashing-contract.md defines IStableHashService.AlgorithmId, IStableHashService.ComputeHash(string), StableHashDigest.AlgorithmId/Value, sha256-v1, UTF-8 without BOM, lowercase SHA-256 output, normalization rules, replacement rules, and test vectors.
- src/DCoding.Data.DVault/IStableHashService.cs exposes public AlgorithmId and ComputeHash(string normalizedInput); src/DCoding.Data.DVault/StableHashDigest.cs exposes public AlgorithmId and Value and validates 64 lowercase hex digest values.
- src/DCoding.Data.DVault/DefaultStableHashService.cs implements sha256-v1 using UTF8Encoding without BOM and SHA256.HashData; src/DCoding.Data.DVault/DefaultStableHashNormalizer.cs implements scalar and structured normalization with invariant formatting, NFC string normalization, LF line endings, ordinal field sorting, duplicate-path rejection, and unsupported-type diagnostics.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers IStableHashService and IStableHashNormalizer through AddDVault while preserving existing service registrations.
- DVault.slnx includes src/DCoding.Data.DVault/DCoding.Data.DVault.csproj and tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj; git ls-files confirms StableHashServiceTests.cs and StableHashNormalizerTests.cs under tests/DCoding.Data.DVault.Tests/Unit.
- tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs asserts published SHA-256 vectors, null-vs-empty behavior, repeat determinism, UTF-8 without BOM behavior, AddDVault registration, replacement service behavior, and digest shape validation.
- tests/DCoding.Data.DVault.Tests/Unit/StableHashNormalizerTests.cs asserts canonical scalar tags, NFC/LF string handling, explicit null fields, ordinal structured ordering, culture-invariant digest behavior, unsupported binary failure before hashing, and invalid timestamp failure before hashing.
- Related relation files exist for parentOf links to 06EXB76DNVSRBD12T4W03AWQZC and 06EXB76NNRDP7WH1F2R5VYYPMR, and blocks links to 06EXB7GYQKBZ8FMQN6YDYCKATG, 06EXB7HEJY18HEB5A5MVTN5KZC, and 06EXB7HPGW3Y9MSP10DEC8RBK4; related ticket JSON shows the two child tickets are done and downstream blocked tickets remain todo.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- DateTimeOffset normalization is accepted by existing tests through UTC conversion; downstream entity tickets should still be explicit about whether their timestamp values are already UTC at the model boundary.
- The parent story overlaps work already present from done child ticket 06EXB76NNRDP7WH1F2R5VYYPMR; dev should treat the persisted Scope In/Out as the source of truth for any remaining closure work rather than expanding into downstream entity-specific hash selection.

AC / test suggestions
- Keep focused unit coverage for duplicate, null/blank, and unsafe field paths, plus no-trailing-LF structured output, because those are explicit AC details and are compatibility-sensitive.
- Keep binary payload behavior covered as unsupported object normalization unless a later domain ticket introduces an explicit binary canonicalization contract.

Implementation watchouts
- Do not add provider-specific schema, migrations, indexes, security hashing, salts, password hashing, or entity-specific hub/link/satellite field-selection behavior under this story.
- Do not construct concrete hash implementations from model code; use IStableHashService/IStableHashNormalizer through the AddDVault/DI boundary so replacement behavior remains testable.
- The local worktree had unrelated modified .gicket/.gicket-bot files in git status; they were not part of this PO-critic decision and should not be folded into developer implementation work.

Non-blocking notes
- Build, test, and formatting gates were not executed because this PO-critic run is read-only and those commands can write build artifacts; the ticket already lists them as Definition of Done for implementation verification.
- The prompt snapshot said there were no recent comments, but the persisted ticket now contains PO refinement, handoff, relation automation, and po-critic lease comments.

Split recommendations
- No additional split is required for PO readiness; the persisted contract already scopes downstream entity field selection and provider persistence storage into follow-up tickets and existing relations.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment