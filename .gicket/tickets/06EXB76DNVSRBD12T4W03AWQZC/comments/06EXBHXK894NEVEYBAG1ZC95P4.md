[gicket-bot] PO refinement contract

Summary
- Refined the stable hashing contract ticket into a bounded v1 design scope with no PO-level blockers.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- V1 stable hashing is a deterministic modeling/data identity contract, not password hashing, encryption, message authentication, or a general cryptographic policy.
- The v1 default algorithm is SHA-256 over normalized UTF-8 input, returning a lowercase hexadecimal digest unless a consuming implementation already has a stricter local convention.
- Replacement through options or dependency injection is required at the public contract boundary so callers can provide an alternate hash service without changing model code.

Scope In
- Define the public hash service abstraction, including the input type expectations, output format, algorithm identity, and failure behavior for unsupported or invalid inputs.
- Document the default implementation expectations for SHA-256, UTF-8 encoding, deterministic input normalization, and lowercase hexadecimal output.
- Document normalization rules for stable values, including culture-invariant formatting, deterministic field ordering where structured inputs are hashed, explicit null handling, and avoidance of platform-specific serialization side effects.
- Document extensibility expectations so an implementation can swap the hash service through options or dependency injection while preserving deterministic behavior for existing callers.
- Provide enough test vectors or example cases for implementers to verify deterministic hashes across repeated runs and replacement implementations.

Scope Out
- Choosing or implementing password hashing, encryption, HMAC/signature behavior, key management, or secret rotation.
- Designing a full migration framework for changing hash algorithms after persisted hashes already exist.
- Implementing domain-specific entity hashing rules beyond the shared contract and normalization requirements.
- Creating broad repository project structure or source/test roots unless needed by the implementation ticket that consumes this contract.

Open questions
- none

Follow-up questions
- When persisted hashes are introduced, decide whether hash records must store algorithm/version metadata for migration support.
- When domain entities are modeled, decide which entity fields participate in domain-specific hashes and whether canonical serialization should become a shared utility.
- If non-.NET or cross-language clients are planned, add cross-language conformance fixtures for the same normalization and digest vectors.

Risks
- Changing the default algorithm or normalization after hashes are persisted would create compatibility work, so the v1 contract should be treated as stable once consumed.
- Ambiguous structured input normalization can cause non-reproducible hashes across runtimes if field ordering, null handling, or culture-specific formatting are not made explicit.

Split recommendations
- none

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