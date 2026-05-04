Goal: implement and validate a SQL Server-specific optimized save path in the existing SQL Server provider project.

Scope:
- Use SQL Server-appropriate set-based existence checks and insert-only writes.
- Preserve fallback behavior for unsupported model shapes or unavailable capabilities.
- Add opt-in integration or smoke coverage that does not run by default without a configured SQL Server instance.

Acceptance Criteria:
- The SQL Server provider registers an explicit optimized capability profile.
- Provider tests cover strategy selection and at least one write scenario with SQL Server semantics.
- Documentation explains how maintainers can enable live SQL Server validation.