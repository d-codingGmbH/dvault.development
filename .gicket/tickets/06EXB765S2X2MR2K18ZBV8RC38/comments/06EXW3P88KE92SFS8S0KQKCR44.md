[gicket-bot] PO refinement contract

Summary
- Refined ticket 06EXB765S2X2MR2K18ZBV8RC38 using the persisted ticket snapshot, comments, relations, attached-context documents, and current branch source/test layout. Existing child tickets and blockers already cover the underlying stable hashing contract and downstream consumers, so no additional split or planning artifact was materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 hashing baseline is ratified from docs/plans/stable-hashing-contract.md: default AlgorithmId sha256-v1, SHA-256 over UTF-8 without BOM, lowercase hexadecimal digest output, and deterministic behavior with no salts, timestamps, machine state, current culture, serializer defaults, or dictionary iteration ordering.
- The public service boundary should remain the small stable-hash abstraction described by the contract: IStableHashService exposes AlgorithmId and ComputeHash over already-normalized text, and StableHashDigest carries the algorithm id plus digest value.
- Normalization is a required part of this story for supported scalar and structured inputs: null, string, boolean, integer, decimal, UTC timestamp, and Guid values; string normalization uses Unicode Form C, CR/CRLF to LF conversion, UTF-8 byte counts, invariant formatting, ASCII type tags, ordinal field ordering, explicit null fields, and no trailing LF.
- Repository evidence shows the owning implementation path is src/DCoding.Data.DVault with executable tests under tests/DCoding.Data.DVault.Tests, including Unit stable hashing coverage; these are the v1 default locations for this ticket.
- Persisted relations already link this story as parent of 06EXB76DNVSRBD12T4W03AWQZC and 06EXB76NNRDP7WH1F2R5VYYPMR, and as blocking downstream tickets 06EXB7GYQKBZ8FMQN6YDYCKATG, 06EXB7HEJY18HEB5A5MVTN5KZC, and 06EXB7HPGW3Y9MSP10DEC8RBK4.

Scope In
- Implement provider-neutral stable hashing services in the main DVault library project for hash key and hash diff use cases.
- Expose a default SHA-256 v1 service through the public abstraction and register it through the convention-first AddDVault service registration path without requiring user options.
- Implement canonical normalization for supported scalar values and structured field sets so business-key and satellite-payload callers can deliberately map fields before hashing.
- Preserve explicit behavior for null, empty string, string Unicode/line-ending normalization, boolean, integer, decimal, UTC timestamp, Guid, duplicate field path, invalid field path, unsupported type, and invalid value cases.
- Add or maintain focused unit tests using the stable hashing contract vectors, including deterministic repeat hashing, empty input, null distinct from empty string, structured-field ordering independence, non-invariant current culture behavior, and failure-before-hashing behavior for invalid inputs.

Scope Out
- Provider-specific persistence schema, migrations, SQL column types, physical indexes, or adapter-specific storage behavior.
- Domain-specific selection of hub, link, or satellite fields for a particular model; callers must explicitly map participating fields in later entity tickets.
- Security-specific hashing, password hashing, encryption, MACs, signatures, key management, salts, or secret rotation.
- Runtime configuration option objects or broad advanced hook implementation beyond preserving replaceability through dependency injection/service abstractions already needed for the default registration path.
- Changing Data Vault naming conventions, persistence logical object names, load timestamp policy, or record-source policy outside the documented shared standards.

Open questions
- none

Follow-up questions
- Future entity-specific tickets should decide exactly which hub business-key, link participant-key, and satellite payload fields participate in each hash key or hash diff and add entity-specific vectors.
- A later advanced-configuration ticket can decide the full options API for alternate hash service factories; this story only needs the abstraction and replaceable registration behavior required by the v1 default path.
- Future persistence tickets should define how algorithm id and digest values are stored in provider-specific schemas while preserving the provider-neutral metadata contract.

Risks
- Hash normalization is compatibility-sensitive: any post-release change to algorithm id, scalar encodings, field ordering, culture formatting, or timestamp handling will require persisted-hash compatibility work.
- Decimal and binary inputs can be misused if callers assume the shared service performs domain-specific scale or byte-payload decisions; the ticket should keep those boundaries explicit in documentation and tests.
- Using serializer output, dictionary iteration order, or current culture anywhere in model-specific callers would break the deterministic contract even if the shared hash service itself is correct.

Split recommendations
- No new child ticket is needed for this refinement pass. Existing relations already split the stable hashing contract and downstream blocked work; this story can proceed as the implementation story for the documented v1 stable hashing services.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 6
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment