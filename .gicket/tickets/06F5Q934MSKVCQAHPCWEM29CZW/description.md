Goal: Add a durable hash canonicalization manifest and test vectors.

Acceptance criteria:
- Records algorithm, encoding, null handling, ordering, culture, binary, and delimiter behavior.
- Adds compatibility vectors that can be verified across providers and future versions.
- Detects accidental changes to hash-key or hash-diff canonicalization.