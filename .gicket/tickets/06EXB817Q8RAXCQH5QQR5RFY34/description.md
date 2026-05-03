<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the live ticket and repository baseline without materializing new child tickets, relations, attachments, or planning documents; the work is already tightly bounded to the six packable DVault packages and is ready for PO-critic.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Verified persisted relation context: parent story `06EXB80ZNQTTGT6VN2DKEDGB0M` (`Story: Enforce public API quality`), outgoing `blocks` relation to `06EXB81FSWAA6N1HMYQ0CM4S8G` (`Task: Add API approval or compatibility snapshot tests`), and incoming `blocks` relations from `06EXB7HPGW3Y9MSP10DEC8RBK4` and `06EXB7J6HCA9QZ3DPP5Z03YGJ0`.
- The two incoming blocker tickets are already `done`, so they do not leave a PO-level dependency question for this ticket.
- Recent persisted comments on this ticket are automation follow-ups only; no human comment or attachment changed scope in this refinement pass.
- No child tickets, relation writes, attachments, or planning documents were created in this refinement pass.

### Scope In
- Documentation warning enforcement for the six packable packages: `src/DCoding.Data.DVault/`, `src/DCoding.Data.DVault.Sqlite/`, `src/DCoding.Data.DVault.Postgres/`, `src/DCoding.Data.DVault.SqlServer/`, `src/DCoding.Data.DVault.Oracle/`, and `src/DCoding.Data.DVault.MySql/`.
- Public and protected consumer-facing APIs in those packages, including the visible registration and modeling entry points such as `AddDVault*`, `UseDataVault`, `ApplyDataVaultMetadata`, `IDataVaultSaveService`, and provider capability contracts.
- Retention or consolidation of XML documentation generation and missing-doc enforcement so the package API surface fails clearly when required XML comments are absent.
- Verification that each packable package ships its generated XML documentation file in pack output.

### Scope Out
- `src/DCoding.Data/DCoding.Data.csproj`, the unit/integration/shared test projects, and `benchmarks/DCoding.Data.DVault.Benchmarks/`, because they are non-packable and not the release package surface for this ticket.
- API approval or compatibility snapshot testing, which is already separated into downstream ticket `06EXB81FSWAA6N1HMYQ0CM4S8G`.
- Provider-specific writer behavior, persistence semantics, or additional provider capability design beyond the existing public API documentation surface.
- Blanket repository-wide suppression of `CS1591` or broad global exceptions for undocumented public APIs.

## Acceptance Criteria
- Each packable DVault package emits XML documentation and enforces missing XML documentation for public/protected APIs as a visible build failure or equivalent enforced warning gate.
- The enforcement covers the consumer-facing APIs already visible in repository source and README, including `AddDVault`, `AddDVaultSqlite`, `AddDVaultPostgres`, `AddDVaultSqlServer`, `AddDVaultOracle`, `AddDVaultMySql`, `UseDataVault`, `ApplyDataVaultMetadata`, `IDataVaultSaveService`, and the public provider capability contracts.
- Any exception for generated or intentionally internal-only code is explicit and local to the affected source or project rather than a global disable of the documentation gate.
- Packing each packable package produces the generated XML documentation file with the package output.

## Definition of Done
- The approved XML-doc policy is applied consistently across the six packable DVault projects, whether kept in the individual project files or centralized through shared MSBuild configuration scoped to those packages.
- Public API source needed to satisfy the gate contains XML documentation comments instead of bypassing the requirement with broad suppressions.
- Verification demonstrates both the build-time enforcement and the presence of XML documentation files in pack output for every packable package.
- Repository standards referenced by `docs/plans/shared-implementation-standards.md` and `docs/formatting.md` remain satisfied.

## Implementation Notes
- Repository evidence already fixes the v1 package baseline: the packable set is `DCoding.Data.DVault` plus provider packages `Sqlite`, `Postgres`, `SqlServer`, `Oracle`, and `MySql`; the `DCoding.Data` source-root anchor, tests, and benchmarks are non-packable.
- The current branch already shows `GenerateDocumentationFile=true` and `WarningsAsErrors=$(WarningsAsErrors);CS1591` in each packable package project file, so the safe default is to keep or centralize that compiler-based enforcement instead of introducing a new analyzer stack unless a concrete gap is found.
- `docs/plans/shared-implementation-standards.md` already sets `GenerateDocumentationFile` as the .NET project baseline and says public API source should carry generated XML documentation coverage where documentation generation is enabled; this ticket is the owning feature ticket for satisfying that baseline on the package API surface.
- README and unit tests already identify the current consumer-facing registration and modeling surface, including provider registration methods and explicit-save/model-builder entry points; implementation should use that visible surface as the documentation coverage baseline rather than reopening API-selection questions.

## Open Questions
- none

## Follow-Up Questions
- If future examples, benchmarks, or other non-packable projects become externally published or packable, should the same XML-doc enforcement be promoted from the current package-scoped baseline to a broader repository convention?

## Risks
- If the enforcement is moved into shared MSBuild files without a packable-project condition, non-packable tests or benchmarks could start failing on unrelated public APIs and create avoidable churn.
- If implementation stops at compile settings and never validates pack output, one package could still miss the shipped XML documentation artifact despite compiling with documentation generation enabled.

## Split Recommendations
- No additional split is recommended; the parent quality story already separates XML-doc enforcement from downstream API snapshot testing through ticket `06EXB81FSWAA6N1HMYQ0CM4S8G`.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Make undocumented public/protected APIs visible during builds.

## Current Baseline
- XML documentation coverage must include the core package and provider extension packages.
- The new provider registration APIs are consumer-facing and should not bypass docs warnings.

## Scope
- Configure documentation warnings or analyzers for public/protected APIs in packable projects.
- Keep generated or internal-only code exceptions explicit.

## Acceptance Criteria
- Build fails or warns clearly for missing docs.
- Generated XML docs are included in pack output for every packable package.
- Provider registration extension methods and public provider contracts are covered.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.