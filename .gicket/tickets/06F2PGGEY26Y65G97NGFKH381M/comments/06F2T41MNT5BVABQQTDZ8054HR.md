[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the persisted contract is closed, the repository already contains the named design-time command surface and docs/tests, and the story split matches related tickets.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs, src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs, and src/DCoding.Data.DVault/DataVaultDesignTimeExportSource.cs exist in the repository; DataVaultDesignTimeCommand.cs dispatches the four verbs validate, export, drift, and guardrail.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs covers deterministic help/usage plus success and failure paths for validate/export/drift/guardrail, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt includes the three public design-time command types.
- docs/architecture/dvault-dotnet-ef-design-time-workflow.md documents a consumer-owned single-project host, dotnet run --project ... -- validate|drift|guardrail, and explicitly excludes DVault-owned IDesignTimeServices or EF CLI interception; docs/production-adoption-checklist.md makes validate and artifact-based drift the default CI guidance.
- src/DCoding.Data.DVault/DCoding.Data.DVault.csproj references Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Relational, and Microsoft.Extensions.DependencyInjection.Abstractions only; rg across src/*.csproj found no Microsoft.EntityFrameworkCore.Design package reference in DVault packages.
- git diff --name-only develop...ticket/06F2PGGEY26Y65G97NGFKH381M-story-add-dvault-design-time-command-surface returned only .gicket/tickets/06F2PGGEY26Y65G97NGFKH381M/** changes, while git log on the command/doc/test paths showed prior develop integrations 342ed946c [06F2PGGR30XXCDKCZ8W2J2WX8C], c1c350b70 [06F2PGGJQMKH2T5948VJH93M5R], and f75897f66 [06F1XPVPKVGYKCV04PY98TSS78].

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No PO-level example gap blocks handoff; if later documentation needs expansion, the main candidate examples are export --output <path> and classified --live-schema unsupported or unavailable outcomes.

Risky assumptions
- The consumer project can reliably resolve a passed migration name to scaffolded Migration.UpOperations; the contract keeps that responsibility consumer-owned instead of promising DVault-owned CLI discovery.
- Teams will follow the documented default of artifact-versus-design-time-model drift and will not treat the opt-in --live-schema lane as the standard blocking CI gate.

AC / test suggestions
- Keep downstream verification explicit for help/usage, export --output, and guardrail missing-migration usage errors so the automation-safe parser and exit-code contract stays visible.
- If machine-readable command output becomes necessary, open the already-recorded follow-up as a separate ticket instead of expanding this story beyond deterministic text plus existing structured APIs.

Implementation watchouts
- Do not broaden the public surface beyond DataVaultDesignTimeCommand, DataVaultDesignTimeCommandHost, and DataVaultDesignTimeExportSource; the approved public API snapshot already ratifies the minimal boundary.
- Keep Microsoft.EntityFrameworkCore.Design, IDesignTimeServices, and any DVault-owned EF CLI shim out of src/DCoding.Data.DVault; the current csproj and docs make that package boundary explicit.
- Keep default drift artifact-based and design-time-model-based; live-schema drift should remain opt-in and classified through the existing live-schema APIs.
- Reuse existing ToDisplayString() diagnostics, drift, and guardrail reporting surfaces rather than inventing a parallel command-specific taxonomy.

Non-blocking notes
- The story branch differs from develop only in .gicket/tickets/06F2PGGEY26Y65G97NGFKH381M/**, so the command-surface baseline is already repository-backed outside the ticket metadata branch.
- Broader documentation and release-note cleanup remains intentionally separate in 06F2PGHA0EXJRGDHM4GQM7NPYR.

Split recommendations
- Keep the current split already modeled in relations: story-level ratification here, command implementation in 06F2PGGJQMKH2T5948VJH93M5R, and CI/example guidance in 06F2PGGR30XXCDKCZ8W2J2WX8C.
- Keep migration-guardrail hardening or rule coverage in 06F2PGGW8ZBW80V6B8RPWNVM70 and 06F2PGH42B6BT1708MYGMXP5GM, and keep broader v0.11 documentation or release-note work in 06F2PGHA0EXJRGDHM4GQM7NPYR.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment