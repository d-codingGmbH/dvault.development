[gicket-bot] PO-critic review contract

Summary
- Return to PO: the epic's satellite-only shipped boundary conflicts with still-current typed-read generator contract surfaces, so the ticket is not yet internally consistent for developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q91V0YGSA6SH9WDS02GH0M/description.md` says the shipped boundary is support-bundle-driven and satellite-only, `## Open Questions` is `none`, and the Definition of Done says repository docs should consistently describe non-emission of PIT/bridge helpers.
- `src/DCoding.Data.DVault.Analyzers/README.md` says the typed read-model generator emits satellite-only latest/current/as-of helpers from one authoritative `dvault.support-bundle.v1` file and documents `DMV1963`, `DMV1964`, `DMV1967`, `DMV1968`, and `DMV1969` as unsupported PIT/bridge/dynamic/out-of-bound outcomes.
- `docs/releases/v0.22.0.md` says the generator does not emit PIT or bridge helpers, does not parse raw `dvault.model.v1` additional files directly, and that typed helper generation is support-bundle-driven and satellite-only.
- `docs/model-first-governance.md` says a raw `dvault.model.v1` artifact is not a direct generator input and that PIT and bridge declarations do not produce typed read-model helpers in the current generator boundary.
- `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs` contains satellite helper emission assertions (`Read...CurrentAsync`, `Read...LatestAsync`, `Read...AsOfAsync`) plus `DMV1963`, `DMV1964`, `DMV1967`, `DMV1968`, and `DMV1969` tests for non-emitted PIT/bridge and other out-of-bound shapes.
- `docs/plans/typed-read-model-generator-contract.md` still says the v1 generator emits `PIT as-of projections` and `bridge traversal projections`, defines PIT/bridge row and extension naming patterns, and `docs/plans/README.md` still lists that file under `Current Contracts`.
- The done baseline child story `.gicket/tickets/06F5Q922T5B21GJN49FYN6DJH0/description.md` still defines supported generated PIT/bridge shapes and says it is the contract parent for `06F5Q92AHG0ZCTVQGC6NAYVP9C` and `06F5Q92R02HB7FCE1AWKXPTMRW`.
- `git -C /mnt/c/Projects/DVault rev-parse HEAD` and the provided scratch ref both resolve to `15b361b981bdb59e0d05778b451cb92f10f2c373`; `git -C /mnt/c/Projects/DVault diff --name-only 845b807c8..15b361b98` listed only `.gicket/tickets/06F5Q91V0YGSA6SH9WDS02GH0M/...` files, so this branch did not fix the contract/doc contradiction.

Blocking findings
- The epic's authoritative story is internally inconsistent. The parent epic, release notes, analyzer README, model-first guidance, and generator tests all say the current shipped boundary is satellite-only, but `docs/plans/typed-read-model-generator-contract.md` still promises generated PIT and bridge helpers.
- The epic explicitly says to treat `06F5Q922T5B21GJN49FYN6DJH0` as the contract baseline, but that done child story's persisted delivery contract still scopes PIT/bridge helper generation. As written, the parent points to a baseline contract that disagrees with the shipped boundary it asks reviewers to approve.

Required PO actions
- Refine the epic so all authoritative contract surfaces tell one story: either explicitly supersede `docs/plans/typed-read-model-generator-contract.md` and the `06F5Q922T5B21GJN49FYN6DJH0` baseline story for v0.22, or reopen/create a follow-up ticket that retires their PIT/bridge helper promises.
- Update the epic's baseline references or closure evidence so reviewers do not need to infer that the old PIT/bridge helper contract text is historical while `docs/plans/README.md` still marks it as a current contract.

Open issues ledger
- critic-item-1 [required-po-action] Refine the epic so all authoritative contract surfaces tell one story: either explicitly supersede `docs/plans/typed-read-model-generator-contract.md` and the `06F5Q922T5B21GJN49FYN6DJH0` baseline story for v0.22, or reopen/create a follow-up ticket that retires their PIT/bridge helper promises.
- critic-item-2 [required-po-action] Update the epic's baseline references or closure evidence so reviewers do not need to infer that the old PIT/bridge helper contract text is historical while `docs/plans/README.md` still marks it as a current contract.
- critic-item-3 [blocking-finding] The epic's authoritative story is internally inconsistent. The parent epic, release notes, analyzer README, model-first guidance, and generator tests all say the current shipped boundary is satellite-only, but `docs/plans/typed-read-model-generator-contract.md` still promises generated PIT and bridge helpers.
- critic-item-4 [blocking-finding] The epic explicitly says to treat `06F5Q922T5B21GJN49FYN6DJH0` as the contract baseline, but that done child story's persisted delivery contract still scopes PIT/bridge helper generation. As written, the parent points to a baseline contract that disagrees with the shipped boundary it asks reviewers to approve.

Missing examples / edge cases
- If the intended boundary is satellite-only, add one explicit authoritative example of PIT and one of bridge metadata being kept in runtime/diagnostic territory rather than generating helpers, so the contract does not rely on readers cross-referencing `DMV1963`/`DMV1964`/`DMV1969` on their own.

Risky assumptions
- Assuming readers will treat `docs/plans/typed-read-model-generator-contract.md` as historical is risky because `docs/plans/README.md` still lists it under `Current Contracts`.
- Assuming the done `06F5Q922T5B21GJN49FYN6DJH0` child can remain the active contract baseline is risky while its delivery contract still promises PIT/bridge helper generation.

AC / test suggestions
- Add a ticket-level AC or closure-evidence check that every authoritative contract surface named by the epic, including the durable planning contract and baseline child story, matches the satellite-only v0.22 boundary.
- If PIT/bridge helpers are future work, point the contract explicitly at the existing non-emission evidence surfaces: `DMV1963`, `DMV1964`, `DMV1967`, `DMV1968`, `DMV1969`, `IDataVaultReadService`, and the current runtime PIT/bridge docs.

Implementation watchouts
- The current branch tip matches the provided scratch ref and differs from `develop` only in `.gicket/tickets/06F5Q91V0YGSA6SH9WDS02GH0M/...`; no repository contract/doc correction was made on this branch.
- Without a PO-level superseding statement, downstream roles can still read the durable planning contract and believe generated PIT/bridge helpers are in scope.

Non-blocking notes
- The parent delivery contract has `## Open Questions` set to `none`, and all seven child tickets are currently `done`; `06F5Q92R02HB7FCE1AWKXPTMRW` also carries `closure/no-work-required`.
- Stable hash governance evidence is consistent: `src/DCoding.Data.DVault/IStableHashService.cs`, `StableHashDigest.cs`, `DefaultStableHashService.cs`, `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs`, and `StableHashNormalizerTests.cs` all align on `sha256-v1`, UTF-8 without BOM, lowercase hex digests, NFC/LF normalization, invariant formatting, and published vectors.

Split recommendations
- Do not reopen implementation scope for PIT/bridge helper generation inside this epic; keep any future shipped PIT/bridge helper work additive.
- If reconciling the durable contract surfaces needs work beyond this epic's ticket text, create a small additive follow-up ticket dedicated to superseding or correcting the PIT/bridge helper promises in the current planning contract/baseline story.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment