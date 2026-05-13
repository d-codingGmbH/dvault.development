## Goal

Reduce boilerplate for load date, record source, batch/correlation, and similar DVault metadata through explicit opt-in EF Core interceptors.

## Scope In

- Design options for load timestamp, record source, batch id, correlation id, and tenant/source metadata.
- Implement an opt-in SaveChanges interceptor or equivalent EF Core integration.
- Keep manual override behavior clear.
- Document safe defaults and when not to use the interceptor.

## Scope Out

- No hidden mutation without explicit opt-in.
- No security-sensitive tenant inference.
- No replacement for application audit policy.

## Acceptance Criteria

- Interceptor populates configured metadata only when values are absent or explicitly allowed.
- Manual values are preserved by default.
- Tests cover sync/async save paths where applicable.

## Implementation Notes

- Complement explicit save services, do not obscure them.

## Open Questions

- none