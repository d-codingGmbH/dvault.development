[gicket-bot] PO-critic review contract

Summary
- Ticket contract assumes existing source APIs/types that are not evidenced on the current branch and must return to Product Owner refinement.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Observed persisted delivery contract infers an existing public API/type without corresponding visible source evidence in the current branch snapshot or referenced repository documents.
- Unsupported inferred API claim in contract: Refined :: - Refined this as a docs-only v0.22.0 baseline update for typed read-model generation and stable-hash governance, anchored on the current support-bundle-driven satellite-helper implementation, existing public API snapshot evidence, published hash vectors, and the repository validation command baseline.
- Unsupported inferred API claim in contract: Generator :: - Public API snapshot evidence already exists under `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/` for `DCoding.Data.DVault` and the provider packages. Generator evidence currently lives in `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs`, not in separate committed generator approval snapshots.

Blocking findings
- Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Unsupported inferred API claim: Refined :: - Refined this as a docs-only v0.22.0 baseline update for typed read-model generation and stable-hash governance, anchored on the current support-bundle-driven satellite-helper implementation, existing public API snapshot evidence, published hash vectors, and the repository validation command baseline.
- Unsupported inferred API claim: Generator :: - Public API snapshot evidence already exists under `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/` for `DCoding.Data.DVault` and the provider packages. Generator evidence currently lives in `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs`, not in separate committed generator approval snapshots.

Required PO actions
- Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.

Open issues ledger
- critic-item-1 [required-po-action] Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- critic-item-2 [blocking-finding] Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- critic-item-3 [blocking-finding] Unsupported inferred API claim: Refined :: - Refined this as a docs-only v0.22.0 baseline update for typed read-model generation and stable-hash governance, anchored on the current support-bundle-driven satellite-helper implementation, existing public API snapshot evidence, published hash vectors, and the repository validation command baseline.
- critic-item-4 [blocking-finding] Unsupported inferred API claim: Generator :: - Public API snapshot evidence already exists under `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/` for `DCoding.Data.DVault` and the provider packages. Generator evidence currently lives in `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs`, not in separate committed generator approval snapshots.

Missing examples / edge cases
- none

Risky assumptions
- Existing API/type assumption lacks source evidence: Refined :: - Refined this as a docs-only v0.22.0 baseline update for typed read-model generation and stable-hash governance, anchored on the current support-bundle-driven satellite-helper implementation, existing public API snapshot evidence, published hash vectors, and the repository validation command baseline.
- Existing API/type assumption lacks source evidence: Generator :: - Public API snapshot evidence already exists under `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/` for `DCoding.Data.DVault` and the provider packages. Generator evidence currently lives in `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs`, not in separate committed generator approval snapshots.

AC / test suggestions
- none

Implementation watchouts
- Do not approve developer handoff while contract compatibility depends on inferred branch APIs/types that are not visible in source evidence.

Non-blocking notes
- none

Split recommendations
- none

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment