Goal: add MySQL provider capability registration and optimized writer boundary implementation.

Acceptance Criteria:
- MySQL capabilities are registered through the shared provider contract.
- MySQL-specific SQL stays inside the MySQL provider project.
- Unsupported write cases route to fallback behavior.