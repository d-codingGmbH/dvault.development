[gicket-bot] PO-critic review contract

Summary
- Tracking-only epic closure audit found blocking readiness gaps.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7QPAXMRV0AVQGSXQT13MC/description.md defines the epic as coordination-only, says `README.md` plus `benchmarks/DCoding.Data.DVault.Benchmarks` satisfy the runnable-example requirement, and has `## Open Questions` -> `- none`.
- Previous blocker and resolution are both persisted: comment `06EYPPMDC3KV4HQK1YH99X0TT0.md` returned the ticket to PO over example-surface ambiguity, and comment `06EYPQMQ9348STY0PW6V4FB41W.md` answers `critic-item-2` and `critic-item-3` by ratifying `README.md` plus the benchmark scenarios as the completion target.
- Source evidence for the required public path exists in `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16` (`AddDVault`), `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:29` (`ApplyDataVaultMetadata`), `src/DCoding.Data.DVault/DataVaultSaveService.cs:10` (`IDataVaultSaveService`), and `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:9` (`DataVaultProviderCapabilityProfiles.Sqlite`).
- `README.md:21,30,50,77,129,154,160` documents the quickstart and benchmark entrypoints, and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md:3,15,20` documents `--output` plus the three artifact files.
- Benchmark comparability is directly grounded in `benchmarks/DCoding.Data.DVault.Benchmarks/ScenarioContracts.cs:5-79`, which is consumed by both `conventional-ef` and `dvault-explicit-save` baselines in `CustomerProfileBenchmarks.cs` and `OrderProductBenchmarks.cs`.
- `git diff --name-only develop..HEAD` shows only `.gicket/tickets/...` changes, and `git log --oneline -n 8` shows the recent workflow sequence `1ec828df` (po-critic -> po), `847dade7` (po -> po-critic), and `0abcfead` (current po-critic claim), consistent with a ticket-only refinement branch.
- parentOf child 06EXB7QYF1BB1REM7HQZ4WWVMM status done: Story: Write getting started documentation
- parentOf child 06EXB7RPKGTEW4RZKYQ2DXS554 status done: Story: Build example scenario for customer profile history
- parentOf child 06EXB7SEAWB2KSBQSHQB2MVV38 status done: Story: Build example scenario for orders and product relationships
- parentOf child 06EXB7T62EMCD7CSHS9PE501SC status done: Story: Build benchmark harness for normal EF versus DVault

Blocking findings
- The persisted delivery contract does not explicitly mark this tracking-only epic as closure/tracking with no parent-owned implementation slice.

Required PO actions
- Resolve the tracking-epic closure audit findings before this parent ticket can be closed.

Open issues ledger
- critic-item-1 [required-po-action] Resolve the tracking-epic closure audit findings before this parent ticket can be closed.
- critic-item-2 [blocking-finding] The persisted delivery contract does not explicitly mark this tracking-only epic as closure/tracking with no parent-owned implementation slice.

Missing examples / edge cases
- Satellite history coverage lives in the benchmark scenarios rather than the README quickstart snippet, so epic validation must treat both surfaces as mandatory evidence.
- The contract intentionally excludes provider-specific and NuGet-consumption examples, so reviewers should not treat those as hidden MVP requirements.

Risky assumptions
- Assuming the contract-level resolution is enough even though `README.md:137` still advertises `examples/` as future runnable examples and `examples/.gitkeep` is the only file under that directory.
- Assuming downstream workflow consumers respect the coordination-only boundary and do not reopen new epic-level implementation work after approval.

AC / test suggestions
- Validate the epic against the exact two repository surfaces named in the contract: `README.md` for the canonical quickstart and `benchmarks/DCoding.Data.DVault.Benchmarks` for runnable comparison/satellite evidence.
- At closure, verify benchmark artifacts retain provider and environment context exactly as described in `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and emitted by `BenchmarkArtifacts.cs`.

Implementation watchouts
- Keep SQLite-first wording aligned across README, benchmark guidance, and architecture notes because the implementation baseline is hard-wired to `DataVaultProviderCapabilityProfiles.Sqlite`.
- Do not create new epic-level coding tasks from this ticket; the contract says remaining repository work belongs to the existing child stories.
- Because `git diff develop..HEAD` contains only ticket metadata, any future repo-content changes tied to this epic should be traceable to child-ticket work, not ad hoc epic edits.

Non-blocking notes
- The earlier po-critic blocker was directly answered in persisted PO comments and copied into the current description contract.
- All four child stories are already `done`, so the epic now primarily serves as a coordination and consistency umbrella.
- Current `ticket.json` still carries `critic-needed`, `blocked/dev`, and `blocked/test`; that is a workflow state observation, not a PO-refinement blocker.

Split recommendations
- No additional split is recommended at the epic level; the existing four child stories already cover docs, example scenarios, and benchmarks.
- Keep any future standalone `examples/` tree, provider-specific documentation, or broader benchmark publication as follow-up tickets/epics rather than enlarging this MVP epic.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment