Goal: implement an Oracle provider optimization boundary and the first safe optimized save behavior for Oracle.

Scope:
- Register Oracle provider capabilities explicitly.
- Use Oracle-compatible SQL patterns where optimized behavior is implemented.
- Keep fallback behavior available for unsupported patterns.
- Add opt-in validation guidance because Oracle is not part of the default local developer environment.

Acceptance Criteria:
- Oracle provider behavior does not depend on SQLite or SQL Server syntax.
- Tests or smoke scripts verify the capability registration and fallback behavior.
- Documentation states the supported Oracle validation path and remaining limitations.