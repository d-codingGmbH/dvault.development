[gicket-bot] Run report (outcome: test-workflow-returned)

Summary
- Tester workflow returned ticket '06F5Q92AHG0ZCTVQGC6NAYVP9C' for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92AHG0ZCTVQGC6NAYVP9C`.
- Optimistic claim succeeded (`expectedRevision=06F6TK112EBSNVXJ0PQFC2E9PC`, `currentRevision=06F6TKAH2PERY5B5CWH6BNPAG8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' and commit '8e0ea8742ab6' (ticket-comment branch+commit reference).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' from source '8e0ea8742ab6'.
- Interactive tester tool loop completed review for branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite'.
- Evidence: `git -C /mnt/c/Projects/DVault diff --name-only develop...8e0ea8742ab6` shows the implementation is concentrated in `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs`, `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCata...
- Evidence: `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:323-339` builds row properties from support-bundle produced names and assigns `HashDiff`, `LoadTimestamp`, and `RecordSource` property names from `hashDiff.ProducedName`, `loadTimestamp.Prod...
- Evidence: `docs/plans/typed-read-model-generator-contract.md:155-162` requires generated satellite rows to expose `HashDiff`, `LoadTimestamp`, and `RecordSource` as public members while preserving exact produced column bindings separately in constants or binding tables.
- Evidence: `tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs:288-301,448-449` shows the repository supports custom naming policies that rename technical produced columns to values beginning with `custom_col_`.
- Evidence: `src/DCoding.Data.DVault/DataVaultSatelliteProjectionRow.cs:7-15` fixes the runtime projection mapped-name space to `ParentHashKey`, `HashDiff`, `LoadTimestamp`, and `RecordSource`.
- Evidence: `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:564-609` wires generated `Current`, `Latest`, and `AsOf` helpers through the existing `IDataVaultReadService` latest/current/as-of APIs.
- 40 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- AC check failed: Generated satellite row types preserve exact produced table/column bindings and expose the parent hash key, driving keys in metadata order, `HashDiff`, `LoadTimestamp`, `RecordSource`, and payload properties with nullability derived from authoritative CLR/EF m...
- DoD check failed: Regression coverage protects deterministic naming, metadata-source and fingerprint handling, payload nullability, multi-active driving-key ordering, and unsupported-shape diagnostics. (Regression coverage does not protect the custom naming path that renames s...
- `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs:337-339` violates the satellite contract by deriving public `HashDiff`, `LoadTimestamp`, and `RecordSource` row member names from produced technical column names. With a supported custom naming policy...
- `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs:12-274` do not cover the custom technical-column naming path, so the regression in the public row shape is currently unprotected.

Next steps
- Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.
- Re-run tester verification after updating tests or implementation.
- Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.
- Re-run tester verification after completing the missing implementation, test, or documentation work.
- Update the generator so satellite row public members stay `HashDiff`, `LoadTimestamp`, and `RecordSource` regardless of produced technical column names, while still preserving exact produced bindings in constants or binding metadata.
- Add analyzer coverage with a support-bundle fixture that uses renamed technical produced columns and asserts the generated row still exposes the contract-fixed technical members plus the correct produced-column constants.
- After the fix lands, verify the branch with `dotnet test DVault.slnx --nologo` and `bash tools/check-format.sh` in the supported test environment.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9502`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `374ce0373b944a21951bf19535e1a02f`
- completed-at-utc: `<redacted>-28T06:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/runs/20260528T064634828Z-374ce0373b944a21951bf19535e1a02f.json`