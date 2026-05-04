Goal: implement the PostgreSQL optimized write strategy behind the provider capability contract.

Acceptance Criteria:
- Hub, link, and satellite write paths use set-based operations suitable for PostgreSQL.
- The strategy handles unchanged satellites without duplicate inserts and changed satellites with insert-only history semantics.
- Unsupported cases fall back instead of throwing provider-specific surprises.