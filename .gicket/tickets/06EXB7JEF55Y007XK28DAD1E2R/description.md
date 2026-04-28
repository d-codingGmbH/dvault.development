## Summary
Allow local Postgres tests when a developer provides connection configuration.

## Scope
- Use environment variables or test settings to opt in.
- Do not implement Docker provisioning as part of this task.

## Acceptance Criteria
- Tests skip clearly when Postgres is not configured.
- Documentation states that local Docker setup is external.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.