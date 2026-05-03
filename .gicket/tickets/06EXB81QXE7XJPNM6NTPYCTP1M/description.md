<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified ticket, relation, and repository evidence for one-member-per-file enforcement; scope is fixed to the six packable packages, existing core multi-declaration files are explicitly in scope, and no planning artifacts were created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Automation comments added on 2026-05-03 are lease/follow-up notes only; there is no human scope change to absorb.
- Relation context is already coherent: parent story `06EXB80ZNQTTGT6VN2DKEDGB0M` tracks public API quality, upstream API snapshot ticket `06EXB81FSWAA6N1HMYQ0CM4S8G` is `done`, and this ticket still blocks packaging task `06EXB828EAG5QE3WDR503GTBY8`.
- No child tickets, relation writes, attachments, or planning documents were created in this refinement pass.
- Repository evidence fixes the project boundary: `src/DCoding.Data.DVault` and provider packages `MySql`, `Oracle`, `Postgres`, `Sqlite`, and `SqlServer` are in scope; non-packable `src/DCoding.Data` is out of scope.

### Scope In
- A repository-enforced one-public/protected-top-level-declaration-per-file rule for the six packable source projects.
- Remediation or explicitly documented exceptions for the current core-package multi-declaration files before the rule is treated as passing.
- Actionable diagnostics that report violating source file paths and do not scan `obj`, `bin`, or other generated/build artifacts.
- Provider package source inclusion under the same rule, even though those packages currently mostly expose one registration extension file each.

### Scope Out
- Non-packable `src/DCoding.Data`, test projects, benchmarks, and build/generated output as enforcement targets.
- XML-doc enforcement and package-aware API snapshot design, which are already handled by done sibling tickets `06EXB817Q8RAXCQH5QQR5RFY34` and `06EXB81FSWAA6N1HMYQ0CM4S8G`.
- NuGet publication policy, package content verification behavior, or broader release governance beyond enabling this source-level rule.
- A broader repository-wide rule for internal/private-only declarations.

## Acceptance Criteria
- The agreed validation path fails when a C# source file in any of the six packable projects contains more than one public/protected top-level declaration unless that file is in an explicitly documented exception list.
- Failure output identifies the violating file path or paths so the developer can remediate without manual hunting.
- Current known core baseline violations are either refactored into compliant files or captured in repository documentation as practical exceptions, with no silent pass-through.
- The check ignores generated/build output and does not flag non-packable `src/DCoding.Data`, test projects, or benchmark projects.
- The same enforcement path covers `DCoding.Data.DVault` and each provider extension package.

## Definition of Done
- The rule runs through normal local repository validation for the in-scope projects and passes without relying on manual review.
- Repository documentation records any retained practical exceptions and explains how contributors interpret or satisfy one-member-per-file failures.
- The current packable source baseline is compliant or explicitly documented before the gate is left enabled.
- Implementation continues to follow shared repository standards from the charter attachment and existing local validation conventions.

## Implementation Notes
- There is no existing one-member-per-file analyzer or rule configuration in the repository today; implementation may use an existing analyzer or a lightweight custom or project-based check, but it must work inside the existing local validation flow and produce file-level diagnostics.
- Tests currently set `RunAnalyzers=false`, so the v1 enforcement target should be the packable source projects themselves rather than the test projects.
- Current core-package files with multiple public/protected declarations include `DataVaultAnnotationNames.cs` (2), `DataVaultProviderCapabilities.cs` (7), `DataVaultProviderSaveStrategy.cs` (2), `DataVaultSaveService.cs` (8), `Modeling/DataVaultMetadata.cs` (8), `Modeling/DataVaultModel.cs` (10), and `Modeling/IDataVaultNamingPolicy.cs` (10).
- The visible partial-type case is `DataVaultModelBuilder`, split between `Modeling/DataVaultModel.cs` and `Modeling/DataVaultModelBuilder.cs`; if that layout remains, it must be treated as an explicit documented exception rather than an implicit loophole.
- The current branch shows no ticket-specific implementation delta from `276d56aa07bd06f8b5841b817a8a133b66b129bd`, so the refinement contract is based on repository baseline evidence rather than in-flight code changes.

## Open Questions
- none

## Follow-Up Questions
- If future packable provider packages are added, should the enforcement mechanism auto-discover packable `src/DCoding.Data.DVault.*` projects or require an explicit allowlist update?
- After the public/protected baseline is stable, does the team want the same rule extended to internal top-level declarations or to remain limited to release-surface code?

## Risks
- Enabling the rule without first addressing the existing core multi-declaration files will create an immediate failing baseline.
- A path-only scan that is not project-aware could accidentally include `obj` output or the non-packable `src/DCoding.Data` anchor and create noisy failures.
- Over-broad exception handling for partial types or provider registration files could weaken the rule enough that future regressions slip through.

## Split Recommendations
- No additional planning split is recommended; this ticket is already the dedicated downstream work item for one-member-per-file enforcement under story `06EXB80ZNQTTGT6VN2DKEDGB0M`, while XML-doc and API-snapshot quality work is already separated into done sibling tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Plan and enforce the one public/protected member per file rule where practical.

## Current Baseline
- The rule must be checked across the core source project and the provider extension projects.
- Provider packages may contain small registration classes, but any exception must be documented rather than silently expanding file scope.

## Scope
- Use existing analyzers or a lightweight custom check.
- Apply the check to packable source projects without flagging generated or non-packable build artifacts unnecessarily.

## Acceptance Criteria
- Violations are reported with actionable paths.
- Generated or unavoidable exceptions are documented.
- Provider package source files are covered by the same policy as the core package.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.