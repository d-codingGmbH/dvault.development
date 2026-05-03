[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7QPAXMRV0AVQGSXQT13MC/description.md` has `## Open Questions` with `- none`.
- `gicket-read-ticket-comments` returned 10 comments for `06EXB7QPAXMRV0AVQGSXQT13MC`, and they are automation claim/lease/handover records rather than human scope changes.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` defines public `AddDVault`, `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` defines public `ApplyDataVaultMetadata`, and `src/DCoding.Data.DVault/DataVaultSaveService.cs` defines public `IDataVaultSaveService`.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` hard-codes `DataVaultProviderCapabilityProfiles.Sqlite`, matching the SQLite-first scope in the delivery contract.
- `benchmarks/DCoding.Data.DVault.Benchmarks/ScenarioContracts.cs` centralizes the shared customer and order/product scenario inputs, and `CustomerProfileBenchmarks.cs` plus `OrderProductBenchmarks.cs` both read from that contract for the conventional EF and DVault baselines.
- `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs` runs four baselines, and `BenchmarkArtifacts.cs` writes `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` as documented in `benchmarks/DCoding.Data.DVault.Benchmarks/README.md`.
- `git log --oneline --decorate -n 5` shows HEAD `3d61d597` is the po-critic claim commit on `ticket/06EXB7QPAXMRV0AVQGSXQT13MC-epic-examples-documentation-and-benchmarks`, and `git diff --name-only develop..HEAD` lists only `.gicket/tickets/...` files with no `src/`, `docs/`, `benchmarks/`, or `examples/` differences from `develop`.
- `README.md` Layout still describes `examples/` as future runnable examples for DVault APIs, and `find /mnt/c/Projects/DVault/examples -maxdepth 2 -type f` returned only `examples/.gitkeep`.

Blocking findings
- The delivery contract does not resolve whether the required runnable examples are satisfied by the existing `README.md` quickstart and benchmark scenarios or whether standalone assets under `examples/` are still required.

Required PO actions
- Clarify whether this epic is now coordination-only and ready for closure once its child stories are done, or add explicit remaining epic-level work that a developer should perform.
- Make the example completion target explicit: either state that `README.md` plus the benchmark scenarios satisfy the runnable-example requirement, or require concrete standalone example assets under `examples/` and update the acceptance criteria and definition of done accordingly.

Open issues ledger
- critic-item-1 [required-po-action] Clarify whether this epic is now coordination-only and ready for closure once its child stories are done, or add explicit remaining epic-level work that a developer should perform.
- critic-item-2 [required-po-action] Make the example completion target explicit: either state that `README.md` plus the benchmark scenarios satisfy the runnable-example requirement, or require concrete standalone example assets under `examples/` and update the acceptance criteria and definition of done accordingly.
- critic-item-3 [blocking-finding] The delivery contract does not resolve whether the required runnable examples are satisfied by the existing `README.md` quickstart and benchmark scenarios or whether standalone assets under `examples/` are still required.

Missing examples / edge cases
- The ticket does not define how a reviewer should validate runnable-example completion if no standalone example project is expected.
- The ticket does not state whether benchmark scenario code counts as example code or only as benchmark baseline code.

Risky assumptions
- Assuming completed child ticket statuses alone mean the epic should move to dev instead of closure or coordination.
- Assuming the existing README quickstart and benchmark scenarios are an acceptable substitute for repository-local runnable examples even though `README.md` still marks `examples/` as future work.

AC / test suggestions
- Add one explicit acceptance bullet that names the expected runnable-example artifact and its validation command.
- Add one epic-level closure check that states whether child completion alone is sufficient when repository source already matches `develop` and only `.gicket` metadata changes remain.

Implementation watchouts
- If the epic stays implementation-facing, keep SQLite-first wording consistent with `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` and `docs/architecture/mvp-data-vault-concepts.md` so the work does not imply broader provider support.
- If benchmark outputs are cited as evidence, preserve the environment context already emitted by `BenchmarkArtifacts.cs` and described in `benchmarks/DCoding.Data.DVault.Benchmarks/README.md`.

Non-blocking notes
- The public API surface named in the contract is directly present in source, so the ticket is anchored to real interfaces and types rather than inferred from prose.
- The current child decomposition already covers documentation, example scenarios, and benchmarks; the blocker is epic-level completion and ownership clarity, not missing split work.

Split recommendations
- No additional split is needed; keep the existing four child stories and resolve the epic-level completion and ownership ambiguity instead.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment