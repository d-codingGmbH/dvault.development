Goal: implement and validate a MySQL-specific optimized save path in the existing MySQL provider project.

Scope:
- Use MySQL-compatible SQL for optimized existence checks and insert-only writes.
- Preserve provider-neutral fallback behavior.
- Add opt-in validation and benchmark guidance for MySQL environments.

Acceptance Criteria:
- The MySQL provider registers explicit optimized capabilities.
- The optimized strategy keeps Data Vault insert-only semantics intact.
- Tests or smoke coverage demonstrate behavior without requiring MySQL for default local validation.