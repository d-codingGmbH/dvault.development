Goal: add Oracle provider capability registration and the optimized writer boundary needed for provider-specific SQL.

Acceptance Criteria:
- Oracle capabilities are registered through the shared provider contract.
- The provider package contains no accidental dependency on another database provider package.
- Unsupported write shapes route to fallback behavior.