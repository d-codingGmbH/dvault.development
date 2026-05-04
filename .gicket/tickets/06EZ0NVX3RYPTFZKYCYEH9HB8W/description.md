Goal: define the driving-key contract for multi-active satellites.

Acceptance Criteria:
- The contract identifies which fields distinguish concurrently active satellite rows for the same parent key.
- Validation rejects missing or unstable driving-key definitions.
- Hash key and hash diff behavior remains deterministic and documented.