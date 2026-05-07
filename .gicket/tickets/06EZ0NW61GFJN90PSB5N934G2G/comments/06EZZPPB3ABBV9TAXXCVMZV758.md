[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester verification detected blocking repository findings on branch 'ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NW61GFJN90PSB5N934G2G`.
- Optimistic claim succeeded (`expectedRevision=06EZZFFHP1VK9JXCYJDNHTSXJ4`, `currentRevision=06EZZMCRM6QNA27A4TPZWBANV8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ' (developer-delivery-outcome contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ' from source 'ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ'.
- Interactive tester tool loop requested deterministic fallback to legacy verification: Branch diff inspection against develop shows the multi-active satellite implementation in src/ plus matching tests and public API snapshot updates in tests/, and targeted file inspection conf...
- Executed runtime-orchestration sync-first fetch/pull before tester verification.
- Checked out verification branch 'ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ'.
- Derived 12 committed repository path(s) from branch delta against base branch 'develop'.
- Inspected committed repository state for 12 repository path(s) at commit '19e3d2f8c444'.
- Expanded deterministic verification evidence using 6 developer verification hint(s) across 4 hinted repository path(s) at commit '19e3d2f8c444'.
- Executed tester command `dotnet test DVault.slnx --nologo`.
- 306 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Changed directory 'tests/DCoding.Data.DVault.Tests/Modeling' contains delivery files 'tests/DCoding.Data.DVault.Tests/Modeling/ModelingConventionCoverageTests.cs', 'tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs' but no local build or unit anchor, while sibling ...
- Trust audit: Trust policy audit
- policy-version: 2026-03-25
- active-mode: trust/repo
- [allowed] command: git checkout ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ (allow: git checkout*) (approval-hook)
- [allowed] command: dotnet te...
- AC check failed: Satellites become multi-active only when one or more driving keys are declared through the sibling-approved opt-in contract, while ordinary satellites keep the current builder, metadata, and save behavior unchanged and expose empty driving-key collections. (Th...
- AC check failed: Validation rejects empty or duplicate driving-key names, overlaps with payload names, missing or extra driving-key values, duplicate supplied names, and null driving-key values, while matching supplied names to canonical declaration order regardless of caller ...
- AC check failed: A replay with the same parent hash key, the same canonical driving-key tuple, and the same latest hash diff writes no new row. (The evidence does not directly confirm that latest-state lookup and unchanged replay suppression are partitioned by parent hash key ...
- AC check failed: For the same parent hash key and canonical driving-key tuple, a later changed hash diff inserts a new history row and preserves the earlier row unchanged. (No direct verification evidence demonstrates insert-only history for a changed hash diff within one pare...
- AC check failed: Rows with the same parent hash key and same load timestamp but different canonical driving-key tuples can both persist without colliding, and SQLite tests plus relevant public API or snapshot coverage prove deterministic RowsWritten, saved-record ordering, and...
- Acceptance-criteria comparison is incomplete: 6 item(s) could not be confirmed due to verification failures.
- DoD check failed: The provider-neutral save service and translated satellite schema honor the sibling-approved multi-active uniqueness and ordering rules without regressing hub, link, or ordinary satellite persistence. (Translated schema evidence is present, but the provided e...
- DoD check failed: The contract-defined public opt-in and save surfaces are implemented exactly as specified by the shared artifact and are reflected in approved snapshot tests together with the required validation behavior. (The evidence does not directly show the exact public...
- DoD check failed: Any provider strategy that cannot yet honor the multi-active rules declines those batches so dispatch falls back to the provider-neutral writer. (The optimized provider-strategy evidence does not show either multi-active-aware partitioning or an explicit decl...
- Definition-of-done comparison is incomplete: 4 item(s) could not be confirmed due to verification failures.
- 4 additional item(s) omitted. See the local context artifact for full run details.

Next steps
- Wire directory 'tests/DCoding.Data.DVault.Tests/Modeling' into an existing delivery unit or add a local unit anchor/manifest before rerunning tester verification.
- Regenerate or repair the deterministic tester evidence so the Modeling-directory anchor finding matches the verified branch state at commit `19e3d2f8c444`.
- Provide direct repository or test evidence for the public driving-key contract surfaces, validation cases, latest-state replay suppression by canonical driving-key tuple, changed-row history insertion, and required snapshot coverage.
- Provide direct evidence that optimized providers either implement the required multi-active semantics or explicitly decline those batches so dispatch falls back to the provider-neutral writer, then rerun tester verification.

Prompt cache usage
- prompt-tokens: `99139`
- cached-tokens: `67200`
- effective-cache-ratio: `0.6778`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `f86697a8f14f441cadee322a992f242c`
- completed-at-utc: `<redacted>-07T00:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NW61GFJN90PSB5N934G2G/runs/20260507T003341420Z-f86697a8f14f441cadee322a992f242c.json`