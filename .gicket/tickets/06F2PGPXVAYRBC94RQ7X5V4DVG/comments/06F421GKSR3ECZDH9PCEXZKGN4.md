## Developer Delivery

Summary
- Updated README, v0.15.0 release notes, production adoption checklist, and current-baseline guidance so the public docs align with the shipped explicit PIT maintenance, explicit bridge maintenance, current/as-of satellite convenience reads, and SQLite PIT/bridge read optimization surface.
- Kept this as a documentation-only pass; no product code, tests, package metadata, ticket relations, or planning artifacts were changed.

Validation
- `bash tools/check-format.sh` passed.
- `git diff --check -- README.md docs/releases/v0.15.0.md docs/production-adoption-checklist.md docs/model-first-governance.md docs/plans/fluent-code-first-api-contract.md docs/architecture/dvault-v1-explicit-save-service.md` passed.
- Stale-phrase grep found no remaining matches for the prior caller-populated PIT wording, PIT-maintenance-outside-release wording, or v0.14.0-as-current-baseline wording outside historical v0.14.0 release context.

Notes
- `dotnet build DVault.slnx --nologo` and `dotnet test DVault.slnx --nologo` were not run locally because this ticket changed documentation only; they remain the standard follow-up validation commands for the tester role.