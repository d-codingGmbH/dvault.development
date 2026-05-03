<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the parent quality story against the live repository and existing child tickets; the work is already split into three done child tasks covering XML-doc enforcement, package-specific API snapshots, and one-member-per-file enforcement, so the parent story is ready for PO-critic without new planning artifacts.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The story is already decomposed into done child tickets 06EXB817Q8RAXCQH5QQR5RFY34 (XML-doc enforcement), 06EXB81FSWAA6N1HMYQ0CM4S8G (package-specific API snapshot review), and 06EXB81QXE7XJPNM6NTPYCTP1M (one-member-per-file enforcement).
- Repository evidence fixes the v1 package boundary to six packable projects: src/DCoding.Data.DVault, src/DCoding.Data.DVault.MySql, src/DCoding.Data.DVault.Oracle, src/DCoding.Data.DVault.Postgres, src/DCoding.Data.DVault.Sqlite, and src/DCoding.Data.DVault.SqlServer; src/DCoding.Data is explicitly non-packable and out of this story's release-surface scope.
- No new child tickets, relations, attachments, or planning documents were created in this refinement pass because the existing split and repository documents already bound the story.

### Scope In
- Enforcing XML documentation generation and missing-doc detection for public and protected APIs in each of the six packable DVault packages.
- Package-aware API surface approval or compatibility checks that keep core and provider package changes separately reviewable.
- One-public-or-protected-top-level-declaration-per-file enforcement for the same six packable source projects, with explicit documented exceptions where retained.

### Scope Out
- src/DCoding.Data, test projects, and benchmark projects as direct enforcement targets because they are non-packable or test-only surfaces.
- Provider runtime behavior changes, save semantics, or new public API design beyond documenting and reviewing the existing visible surface.
- Post-v1 release governance such as published-NuGet backward-compatibility policy, broader repository-wide analyzer expansion, or future provider optimization work.

## Acceptance Criteria
- Each packable DVault package emits XML documentation and fails visibly when required public or protected XML documentation is missing.
- API review fails when the built public surface of any one of the six packable packages changes without a deliberate update to that package's approved baseline, and the review output distinguishes core, SQLite, PostgreSQL, SQL Server, Oracle, and MySQL package surfaces from test-only or non-packable surfaces.
- The one-member-per-file check fails when a C# file in an in-scope packable project contains more than one public or protected top-level declaration unless that file is in the explicit documented exception list.
- Contributor-facing documentation identifies the commands, baseline locations, and exception handling needed to run and intentionally update all three public-API quality gates.

## Definition of Done
- The XML-doc gate, package-aware API snapshot gate, and one-member-per-file gate are wired into the normal DVault validation flow and pass against the approved baseline.
- Each packable package ships its generated XML documentation file, and intentional API changes require both source updates and the matching baseline or exception updates.
- Retained one-member-per-file exceptions are documented in repository-controlled policy files, and no broad suppression or silent bypass weakens the public API quality checks.
- Implementation and supporting documentation continue to follow the shared repository standards referenced by the charter attachment and the existing formatting and quality policy documents.

## Implementation Notes
- Use the already-established repository surfaces as the v1 default: docs/quality/api-surface-snapshots.md documents the package-specific public API snapshot flow, docs/quality/one-member-per-file.md documents the source-layout rule and exceptions, and docs/formatting.md shows that tools/check-format.sh is the shared local and CI entry point.
- The current consumer-visible public API baseline already includes AddDVault, UseDataVault, ApplyDataVaultMetadata, IDataVaultSaveService, provider capability and save-strategy contracts, and each provider package's AddDVault* registration entry point; this story should ratify and guard that visible surface rather than reopen API-selection questions.
- Because provider packages share the DCoding.Data.DVault namespace, API review must stay package- or assembly-scoped rather than namespace-scoped.
- The existing split is the implementation plan for the story: 06EXB817Q8RAXCQH5QQR5RFY34 owns XML-doc enforcement, 06EXB81FSWAA6N1HMYQ0CM4S8G owns API snapshots, and 06EXB81QXE7XJPNM6NTPYCTP1M owns one-member-per-file enforcement.

## Open Questions
- none

## Follow-Up Questions
- After the first public package release, should DVault add a second compatibility check against the last published NuGet versions in addition to the repository-managed package baselines?
- If new packable provider packages or externally published examples are added later, should these gates auto-discover eligible projects or continue to rely on an explicit allowlist update?
- Once the public and protected baseline is stable, should the one-member-per-file policy remain limited to release-surface declarations or expand to internal top-level declarations as well?

## Risks
- If shared MSBuild or shell-gate scope is broadened without packable-project conditions, non-packable tests, benchmarks, or build output could start failing on unrelated surfaces.
- A namespace-based or aggregated API snapshot would be misleading because the provider packages share the DCoding.Data.DVault namespace and could hide package-boundary regressions.
- Over-broad one-member-per-file exceptions or stale exception-list entries would weaken the source-layout gate enough for future public API drift to slip through review.

## Split Recommendations
- No additional split is recommended; the parent story is already bounded by the three existing child tickets for XML-doc enforcement, API snapshot review, and one-member-per-file enforcement.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Keep the public API documented, stable, and reviewable.

## Current Baseline
- Public API now spans the core package plus provider extension packages for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- The SQLite provider package exposes the optimized save strategy through `AddDVaultSqlite`; the other provider packages expose fallback registration helpers.

## Scope
- Enforce XML docs for public and protected APIs in every packable package.
- Track API surface changes per package so core API changes and provider package changes are reviewed deliberately.
- Respect one-member-per-file expectations across core and provider source projects.

## Acceptance Criteria
- Missing public/protected docs are detected for each packable package.
- API changes are visible in review and can distinguish core, provider, and test-only surfaces.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.