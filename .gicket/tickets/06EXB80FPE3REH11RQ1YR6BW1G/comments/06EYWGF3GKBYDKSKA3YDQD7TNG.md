[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester verification detected blocking repository findings on branch 'ticket/06EXB80FPE3REH11RQ1YR6BW1G-task-add-unit-test-categories-for-metadata-hashi'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB80FPE3REH11RQ1YR6BW1G`.
- Optimistic claim succeeded (`expectedRevision=06EYWCJ0PRARP2D3SSRTM2YAE4`, `currentRevision=06EYWEG6S42MPCCD6DSH8NZ7G8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06EXB80FPE3REH11RQ1YR6BW1G-task-add-unit-test-categories-for-metadata-hashi' and commit '923e20563bd9' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06EXB80FPE3REH11RQ1YR6BW1G-task-add-unit-test-categories-for-metadata-hashi' from source '923e20563bd9'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Static review of commit 923e20563bd9 found that tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj now links ../TechnicalMetadataColumnContractTests.cs, tests/DCoding....
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06EXB80FPE3REH11RQ1YR6BW1G-task-add-unit-test-categories-for-metadata-hashi'.
- Checked out verification commit '923e20563bd9'.
- Derived 3 committed repository path(s) from branch delta against base branch 'develop'.
- Expanded committed repository inspection with 1 branch-delta path(s) beyond the 5 ticket-declared path(s).
- Inspected committed repository state for 6 repository path(s) at commit '923e20563bd9'.
- 76 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Expected repository path 'Modeling/DefaultNamingPolicyTests.cs' is absent from the verified committed repository state.
- Expected repository path 'Modeling/NamingPolicyTests.cs' is absent from the verified committed repository state.
- Trust audit: Trust policy audit
- policy-version: 2026-03-25
- active-mode: trust/repo
- [allowed] command: git checkout ticket/06EXB80FPE3REH11RQ1YR6BW1G-task-add-unit-test-categories-for-metadata-hashi (allow: git checkout*) (approval-hook)
- [allowed] command: git check...
- AC check failed: Within that Unit project, metadata/model-building, naming/options, hashing, and provider registration/capability/strategy coverage remain discoverable as deterministic repo-local groups through named xUnit test classes or accepted xUnit bridge entrypoints, not...
- AC check failed: A unit-only run targeted at tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj executes those fast groups without loading tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj. (Verification ran `dotne...
- AC check failed: The metadata group includes provider-neutral model and contract coverage for UseDataVault, ApplyDataVaultMetadata, metadata object validation, produced names and ordinals, and the reusable technical metadata column contracts. (The technical metadata contract h...
- AC check failed: The naming/options group includes the linked Modeling/DefaultNamingPolicyTests.cs and Modeling/NamingPolicyTests.cs harnesses through an xUnit bridge consistent with ConventionFirstEntryPointCoverageTests. (`ConventionFirstEntryPointCoverageTests.cs` exists, b...
- AC check failed: The hashing group includes stable hash normalizer and hash service determinism, published digest vectors, and the null, culture, order, unsupported-type, and invalid-value edge cases visible in the current repository baseline. (No verification evidence ties th...
- AC check failed: The provider group verifies the finite current package baseline: AddDVault resolves the core fallback services, PostgreSQL, SQL Server, Oracle, and MySql provider packages do not register an optimized provider strategy, AddDVaultSqlite does, and DataVaultProvi...
- AC check failed: For standalone harnesses such as Modeling/*.cs and TechnicalMetadataColumnContractTests.cs, one xUnit bridge Fact per harness or harness family is sufficient if it drives the underlying Run or equivalent flow and preserves named internal subcase failure output...
- Acceptance-criteria comparison is incomplete: 8 item(s) could not be confirmed due to verification failures.
- DoD check failed: The agreed grouping is implemented inside tests/DCoding.Data.DVault.Tests/Unit so the Unit project path remains the fast local selection surface for this ticket. (The Unit project remains present and was modified for bridge wiring, but the verified state does...
- 6 additional item(s) omitted. See the local context artifact for full run details.

Next steps
- Inspect bot logs and retry tester verification.
- Restore or otherwise make inspectable the required Modeling harness outputs so the verified commit clearly contains the expected naming/options bridge inputs.
- Add deterministic tester evidence for a direct `dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj` run that demonstrates the Unit surface without loading the Integration project.
- Provide repository-level verification evidence that the metadata, hashing, and provider groups remain wired into named Unit classes or accepted bridge entrypoints.

Prompt cache usage
- prompt-tokens: `36852`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0660`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `0acb88266e214726ae9449072edbe283`
- completed-at-utc: `<redacted>-03T14:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB80FPE3REH11RQ1YR6BW1G/runs/20260503T143308081Z-0acb88266e214726ae9449072edbe283.json`