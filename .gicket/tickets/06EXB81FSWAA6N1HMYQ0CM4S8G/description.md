## Summary
Add a review mechanism for public API changes.

## Current Baseline
- Public API now includes the core API, provider capability/strategy contracts, and provider package registration extensions.
- API review should preserve package boundaries so provider package additions do not hide core API changes.

## Scope
- Use an approval, baseline, or compatibility test approach appropriate for the repo.
- Capture API snapshots per packable package or with equivalent package-aware grouping.

## Acceptance Criteria
- Public API changes require deliberate baseline updates.
- The mechanism is documented for contributors.
- API review output distinguishes core, SQLite, PostgreSQL, SQL Server, Oracle, and MySQL package surfaces.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.