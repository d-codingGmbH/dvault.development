Goal: implement persistence behavior for baseline multi-active satellites.

Acceptance Criteria:
- The save path inserts changed multi-active rows and suppresses unchanged duplicates based on parent key plus driving key plus hash diff.
- Tests cover insert-only history behavior for repeated saves and changed values.
- Provider-neutral behavior works in the local SQLite baseline.