[gicket-bot] PO-critic review contract

Summary
- Return to PO: the epic contract is otherwise well evidenced and split, but its required formatting-gate DoD is currently not executable in the repository.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted delivery contract in .gicket/tickets/06EXB74DC57F8HC98X4D6ZBHXW/description.md has PO handoff decision ready_for_po_critic and ## Open Questions with only '- none'.
- Ticket relation events show parentOf links from 06EXB74DC57F8HC98X4D6ZBHXW to 06EXB74NRVRX18GD33CH1C12SW, 06EXB75DX3YAJFMJ6TNHVPAWYG, and 06EXB765S2X2MR2K18ZBV8RC38.
- git rev-parse returned HEAD, target branch, and scratch ref all at 7e4e8392cb69a7e15960280579023fabb0b3237a; git log shows handoff po->po-critic commit 9bee375b51b5274dd0e687f90c2555ae77ca49a2 and lease claim po-critic at HEAD.
- Source inspection found modeling APIs under src/DCoding.Data.DVault/Modeling including DataVaultMetadata, DataVaultModel, DataVaultConventions, DataVaultModelBuilder, DataVaultModelOptions, DefaultDataVaultNamingPolicy, DefaultNamingPolicy, and IDataVaultNamingPolicy.
- Source inspection found stable hashing APIs and implementations under src/DCoding.Data.DVault including IStableHashService, IStableHashNormalizer, StableHashDigest, DefaultStableHashService, and DefaultStableHashNormalizer; DefaultStableHashService.AlgorithmId is sha256-v1.
- Test inspection found tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs, StableHashServiceTests.cs, StableHashNormalizerTests.cs, and tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs covering metadata roles, deterministic naming, stable hash vectors, culture independence, duplicate field paths, invalid paths, and unsupported inputs.
- docs/architecture/mvp-data-vault-concepts.md defines the MVP baseline as hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources; docs/plans/stable-hashing-contract.md defines sha256-v1, UTF-8 without BOM, lowercase hexadecimal digest output, invariant normalization, and ordinal structured-field ordering.
- The contract DoD requires 'bash tools/check-format.sh' to succeed, and shared implementation standards identify tools/check-format.sh as the required formatting gate.
- Running timeout 30s bash tools/check-format.sh exited 1 with 'tools/check-format.sh: line 10: script_repo_root: unbound variable' and 'line 12: script_repo_root: unbound variable'.
- nl -ba tools/check-format.sh shows line 10 uses git -C "$script_repo_root" and line 12 assigns repo_root=$script_repo_root, but rg found no earlier definition of script_repo_root in the script.

Blocking findings
- The persisted DoD depends on an executable non-mutating formatting gate, but the repository's tools/check-format.sh currently fails before checking files because script_repo_root is undefined. This makes the handoff contract non-actionable for dev/test until PO records a dependency or clarifies the validation path.

Required PO actions
- Update the ticket-level contract to address the broken formatting-gate prerequisite: either record an explicit dependency/blocker on restoring tools/check-format.sh or adjust the DoD to a validated executable gate available to this ticket.

Open issues ledger
- critic-item-1 [required-po-action] Update the ticket-level contract to address the broken formatting-gate prerequisite: either record an explicit dependency/blocker on restoring tools/check-format.sh or adjust the DoD to a validated executable gate available to this ticket.
- critic-item-2 [blocking-finding] The persisted DoD depends on an executable non-mutating formatting gate, but the repository's tools/check-format.sh currently fails before checking files because script_repo_root is undefined. This makes the handoff contract non-actionable for dev/test until PO records a dependency or clarifies the validation path.

Missing examples / edge cases
- No additional Data Vault concept example gap was found at the epic level beyond the executable-gate blocker.

Risky assumptions
- Assuming developers can satisfy the current DoD is unsafe while tools/check-format.sh exits with an unbound variable before performing its checks.

AC / test suggestions
- Add a ticket-level validation note that names the exact formatting command or dependency that must be green before dev/test can close this epic.
- If the parent goes to dev, add an AC or handoff note that dev should verify child-ticket completion and aggregation only, not reopen the full modeling-core implementation as one unit.

Implementation watchouts
- Keep provider-specific DDL, EF provider adapters, migrations, PIT/bridge/multi-active features, runtime loading, and security hashing outside this epic unless a child ticket explicitly owns them.
- Keep hash field selection deliberate and domain-mapped; do not serialize arbitrary objects for stable hashing.
- Keep load timestamp and record source as required provider-neutral lineage metadata for hub, link, and satellite records.

Non-blocking notes
- dotnet --info observed SDK 10.0.203 and runtimes 10.0.7, but the read-only review environment reported a read-only file system while dotnet tried to create a temp subdirectory, so dotnet test was not run.
- git status reported unrelated local modifications in .gicket-bot/.gitignore, .gicket/.gitignore, .gicket/types.json, and unrelated ticket files; they were outside the reviewed ticket contract and not used as blockers.

Split recommendations
- No additional product split is recommended before resolving the PO blockers; the epic already has persisted parentOf relations to three direct child tickets.
- If PO keeps the parent active after resolving blockers, keep any remaining work split by metadata shape, deterministic model behavior, and provider-facing integration boundary rather than reopening the parent as one implementation unit.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment