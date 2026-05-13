## Goal

Track and complete the missing repository release-summary document required before the EF Core lifecycle-guardrails epic can close.

## Scope In

- Add `docs/releases/v0.8.0.md` following the existing `docs/releases/v0.x.0.md` release-note pattern.
- Summarize the lifecycle guardrail workflow: model validation with stable DMV diagnostics, migration preflight with stable DVM guardrails, the consumer-owned design-time factory/preflight boundary, and ModelSnapshot versus optional live-schema drift evidence.
- Keep the scope boundary explicit: no DVault-owned `IDesignTimeServices`, no custom `dotnet ef` shim, no CLI interception, and no provider-specific migration runner claims.
- Capture the current evidence boundary using the existing repository tests and docs, including migration-guardrail coverage, `DataVaultModelFirstDesignTimeWorkflowTests`, and the SQLite-first optional live-schema proof lane.

## Scope Out

- No product code changes.
- No new runtime APIs or CLI features.
- No broader provider-support promise beyond current repository evidence.

## Acceptance Criteria

- Repository contains `docs/releases/v0.8.0.md`.
- The release summary aligns with `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` and `docs/model-first-governance.md`.
- The release summary states that v1 design-time support is consumer-owned and single-project, without DVault-owned `IDesignTimeServices` or a first-party CLI shim.
- The release summary distinguishes metadata-only ModelSnapshot comparison from optional live-schema evidence and keeps the live-schema lane SQLite-first unless later repository evidence expands it.
- The parent epic can cite this ticket as the tracked release-documentation deliverable required for closure.

## Implementation Notes

- Mirror the structure and evidence style used by `docs/releases/v0.7.0.md`.
- Treat the release note as the repository artifact that closes the parent epic's current documentation gap.

## Open Questions

- none