## Summary
Persist hubs, links, and satellites according to Data Vault-oriented insert/idempotency rules.

## Scope
- Persist hubs and links idempotently.
- Insert satellite rows only when hash diff changes.

## Acceptance Criteria
- Repeated saves do not create duplicate hubs or links.
- Unchanged satellite payloads do not create new rows.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.