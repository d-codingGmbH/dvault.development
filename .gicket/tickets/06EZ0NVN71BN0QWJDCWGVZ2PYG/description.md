Goal: add baseline support for multi-active satellites where multiple active records can exist for the same business key at the same time.

Scope:
- Define a driving-key contract for multi-active satellites.
- Persist multi-active satellite records with deterministic uniqueness and insert-only history semantics.
- Document supported patterns and limitations.

Acceptance Criteria:
- Multi-active satellites are opt-in and do not alter normal satellite behavior by default.
- Tests cover duplicate prevention, changed record insertion, unchanged record suppression, and driving-key validation.
- Documentation includes a minimal multi-active satellite example.