[gicket-bot] PO-critic review contract

Summary
- The ticket is bounded, repository-backed, and has no unresolved Open Questions; it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F9GF417FDFWPBF1039G45FEW/description.md` is the authoritative delivery contract, includes seven concrete acceptance criteria, and its `## Open Questions` section is explicitly `- none`.
- `src/DCoding.Data.DVault/DataVaultOptions.cs` exposes the existing `Use...` advanced-configuration surface but currently has no hashing-specific option, matching the story's chosen extension point.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` already exposes both `AddDVault()` and `AddDVault(Action<DataVaultOptions>)`; the optionless path registers `IStableHashService` as `DefaultStableHashService.Instance`.
- `src/DCoding.Data.DVault/DefaultStableHashService.cs` hard-codes `AlgorithmId => "sha256-v1"`, and `src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs` hard-codes `StableHashAlgorithmId = "sha256-v1"` plus `PersistenceContentHashAlgorithm = "sha-256"`, so the contract is aligned to current behavior and explicit about what must remain unchanged.
- `docs/plans/stable-hashing-contract.md` section `Non-Default Algorithm Id Candidates` lists `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`, and `src/DCoding.Data.DVault/StableHashDigest.cs` already validates digest shapes for those ids.
- `git diff --name-status develop...ticket/06F9GF417FDFWPBF1039G45FEW-story-add-opt-in-hash-algorithm-registration` shows only `.gicket/tickets/06F9GF417FDFWPBF1039G45FEW/...` metadata files changed; there are no `src/` or `tests/` changes yet, which is consistent with a pre-development PO handoff.
- `git log --oneline --max-count=4` on `ticket/06F9GF417FDFWPBF1039G45FEW-story-add-opt-in-hash-algorithm-registration` shows commit `330abb200` as `handoff po->po-critic` and HEAD `733f1dcc2` as the active `po-critic` claim, confirming the ticket is at the critic gate now.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract implies exact id matching, but it does not give a worked example for `null`, empty, whitespace-only, or case-variant selector input values.
- The truncation rule is stated, but there is no explicit worked example showing one shared normalized input where `sha256-128-v1` and `sha256-160-v1` equal the leading bytes of the corresponding `sha256-v1` digest.
- The precedence rule is described in prose, but there is no compact example matrix covering optionless `AddDVault()`, a caller-pre-registered custom `IStableHashService`, and explicit built-in selection together.

Risky assumptions
- Developers will interpret `accepts exactly` as ordinal lowercase matching with no trimming, aliasing, or case-folding.
- Developers will implement the explicit built-in selector as authoritative for the approved ids while still preserving the current caller-override behavior on the optionless `AddDVault()` path.
- Callers will understand that non-default algorithms remain bounded identity trade-offs and do not imply storage compatibility before ticket `06F9GF5FV54DGWY9GA8ZEZWM5R` lands.

AC / test suggestions
- Keep one explicit vector-style test or AC proving that `sha256-128-v1` and `sha256-160-v1` are prefix truncations of the same `sha256-v1` digest for the same normalized input.
- Keep one explicit invalid-selector test covering `null` or blank input in addition to unsupported nonblank ids.
- Keep one explicit precedence test covering a caller-registered custom `IStableHashService` together with `AddDVault(options => ...)` built-in selection so the intended override rule stays unambiguous.

Implementation watchouts
- Do not let this story alter `DataVaultConventions.PersistenceContentHashAlgorithm`; the contract keeps that value at `sha-256`.
- Do not broaden this ticket into diagnostics/support-bundle surfacing, documentation guidance, or storage/migration compatibility; those concerns are already split to tickets `06F9GF46KZYRKR1EGEPR3TV824`, `06F9GF4CRMXKEY2QT97W0S3GTR`, and `06F9GF5FV54DGWY9GA8ZEZWM5R`.
- Registration precedence is the main regression risk because the current optionless path preserves caller overrides while the new explicit selector is supposed to be authoritative for the approved built-ins.

Non-blocking notes
- The owner branch currently contains ticket metadata only; that is expected for this pre-development gate and is not a PO blocker by itself.
- No assignees are present in `.gicket/tickets/06F9GF417FDFWPBF1039G45FEW/ticket.json`; that is operationally fine for handoff but may be assigned downstream by the workflow.

Split recommendations
- No additional split is needed; the delivery contract is already tightly bounded, and diagnostics, documentation, and storage-profile compatibility are already isolated in follow-up tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment