<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Delivery contract refined and ready for PO-critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Ticket 06F9G8GS08VNH0DT09Q4PC2HRC is done and is now the authoritative DB2 contract baseline for this story, so dependency, provider-name, and package-line decisions are already settled for PO purposes.
- The live child split under epic 06F9G8GH969DQXD7WZ8JHD1GRR already covers package, schema and guardrails, integration, package verification, and documentation, so no additional child tickets were created in this run.
- Repository evidence shows a current seven-package solution and five-provider capability baseline; this story is the bounded lane that introduces the eighth provider package and its registration surface, not a reopen of the DB2 architecture contract.

### Scope In
- Create the packable multi-target provider project DCoding.Data.DVault.Db2 and include it in DVault.slnx using the same package metadata, readme, license, and symbol conventions as the existing provider packages.
- Add an AddDVaultDb2() startup extension that registers IBM.EntityFrameworkCore against the DB2 capability profile, calls AddDVault(), and adds the DB2 provider behavior or registration services needed for provider selection and diagnostics.
- Wire the current codebase's explicit-provider surfaces for DB2 where this package story owns them: provider capability profile exposure, provider-name selection and registration, known-provider diagnostics, and model-artifact or provider-profile availability required by the DB2 package.
- Pin the provider dependency per target framework with conditional IBM.EntityFrameworkCore references: 8.0.0.400 for net8.0 and 10.0.0.100 for net10.0, aligned with the planned 8.34.0 and 10.34.0 DVault DB2 package lines.
- Make the DB2 package and dependency shape explicit enough that the downstream package-verification task can validate the new artifact without reopening package identity or version decisions.

### Scope Out
- DB2 identifier rules, DDL guardrails, migration-operation diagnostics, and live-schema reader behavior; those stay with story 06F9G8H5HE1CJHQXGC2C2YK7P8.
- DB2 save and read execution proof, opt-in external database coverage, and strategy behavior evidence; those stay with story 06F9G8HBXS7Y42J7XFSQKZ2AZ8.
- Comprehensive package verifier updates such as package counts, README or XML documentation checks, symbol checks, and dependency assertions; those stay with task 06F9G8HJJDJH4KF9VK6TZ8B1Z0.
- README, release-note, adoption-guide, and external DB2 setup documentation changes; those stay with task 06F9G8HRZ72XP5Z7FNWM6MBMQC.
- Any DB2-specific benchmark, provider-specific SQL artifact, provisioning, container lifecycle, or CI-infrastructure commitments.

## Acceptance Criteria
- The repository contains a new multi-target provider package at src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj, that package is included in DVault.slnx, and its package metadata follows the established provider-package pattern while aligning to the planned 8.34.0 and 10.34.0 package lines.
- The package exposes AddDVaultDb2() and registers IBM.EntityFrameworkCore to the DB2 capability profile before DB2-specific behavior or strategy services are added, without relying on the unknown-provider SQLite fallback to claim DB2 support.
- The DB2 package pins IBM.EntityFrameworkCore 8.0.0.400 under net8.0 and 10.0.0.100 under net10.0, with no mixed EF Core line references across target frameworks.
- The DB2 provider name and capability-profile wiring are reachable from the package-owned runtime surfaces that currently enumerate supported providers, so diagnostics and model-artifact or provider-profile selection can identify DB2 as explicit support rather than unknown fallback.
- The story leaves a clear package artifact contract for downstream verification: the new DB2 package id, dependency matrix, and expected package lines are explicit enough that task 06F9G8HJJDJH4KF9VK6TZ8B1Z0 can verify them without reopening PO scope.

## Definition of Done
- A consumer can reference DCoding.Data.DVault.Db2, call AddDVaultDb2(), and get explicit DB2 provider registration on both net8.0 and net10.0 without changing the default provider-neutral AddDVault() path.
- The solution, project graph, and package metadata surfaces recognize the new DB2 provider package and preserve the existing multi-target package-family boundary.
- DB2 is represented as an explicit provider in the package-owned registration and selection surfaces, while unsupported schema, live-schema, and read-strategy details remain delegated to the sibling tickets instead of being implied by fallback behavior.
- The new package's dependency and artifact expectations are clear enough that downstream package-verification and documentation tickets can complete without reopening DB2 package identity, version, or provider-name decisions.

## Implementation Notes
- Current provider packages live under src/DCoding.Data.DVault.{Provider}, multi-target net8.0 and net10.0, pack the root README.md, emit symbols, and reference the core DVault project; the DB2 package should follow that bounded shape.
- Only SQLite, Postgres, SQL Server, Oracle, and MySQL appear today in DataVaultProviderCapabilityProfiles, DataVaultProviderCapabilityProfileSelection, DataVaultModelArtifactImporter.CreateProviderCapabilityProfiles(...), DataVaultModelArtifactExporter, and KnownProviderNames, so DB2 support must be wired explicitly across those finite lists.
- Existing provider startup extensions follow the pattern AddDVault{Provider}() -> register provider name -> call AddDVault() -> register provider behavior and optional strategy services; MySQL is the current two-provider-name example.
- The integration project and version-matrix tests currently gate non-SQLite live providers behind DVAULT_TEST_*_CONNECTION_STRING conditions; DB2 should preserve the same external opt-in posture instead of expanding default local validation.
- Current package verification assumes seven packable packages and 8.33.0 / 10.33.0 package lines, so the DB2 package lane must align with the planned 8.34.0 / 10.34.0 family update and leave deterministic inputs for the dedicated verifier task.

## Open Questions
- none

## Follow-Up Questions
- none

## Risks
- If the package adds partial DB2 wiring but misses one of the finite provider-name or profile lists, the repository can fall back to SQLite-oriented defaults or incomplete diagnostics for DB2 contexts.
- Current packaging and version-matrix surfaces still encode a seven-package, 8.33.0 / 10.33.0 baseline, so DB2 package work and the dedicated verification task must land coherently to avoid broken package validation.
- The existing outgoing blocks relation from this ticket to 06F9G8H5HE1CJHQXGC2C2YK7P8 means downstream schema and live-schema guardrail work remains sequenced after this package lane even though PO refinement is complete.
- DB2 live execution remains external opt-in and environment-sensitive, so package-level success alone will not prove live schema or provider-read behavior until the sibling integration and schema tickets land.

## Split Recommendations
- No additional split is recommended; epic 06F9G8GH969DQXD7WZ8JHD1GRR already separates DB2 work into contract, package, schema and guardrails, integration, package verification, and documentation tickets.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Create the DB2 provider package and startup registration, including AddDVaultDb2-style service registration, provider profile/capability wiring, conditional IBM.EntityFrameworkCore package references for net8.0 and net10.0, package metadata for the 8.34.0 and 10.34.0 package lines, solution inclusion, and package verifier expectations.