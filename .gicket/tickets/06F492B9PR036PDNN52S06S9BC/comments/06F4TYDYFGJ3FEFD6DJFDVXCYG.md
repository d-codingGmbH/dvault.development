[gicket-bot] PO-critic review contract

Summary
- Ticket contract assumes existing source APIs/types that are not evidenced on the current branch and must return to Product Owner refinement.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Observed persisted delivery contract infers an existing public API/type without corresponding visible source evidence in the current branch snapshot or referenced repository documents.
- Unsupported inferred API claim in contract: DataVaultDiagnosticsResult, ReadStrategy :: - DataVaultDiagnosticsResult already exists but currently only exposes ReadStrategy for read-bound data; the query-shape payload in this ticket is a new additive member/model that must be introduced explicitly.
- Unsupported inferred API claim in contract: Implement, DataVaultDiagnosticsResult, DataVaultReadStrategyDiagnostics :: - Implement the read-shape payload as a fresh additive model/property on DataVaultDiagnosticsResult; do not overload DataVaultReadStrategyDiagnostics or imply that a read-shape member already exists.

Blocking findings
- Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Unsupported inferred API claim: DataVaultDiagnosticsResult, ReadStrategy :: - DataVaultDiagnosticsResult already exists but currently only exposes ReadStrategy for read-bound data; the query-shape payload in this ticket is a new additive member/model that must be introduced explicitly.
- Unsupported inferred API claim: Implement, DataVaultDiagnosticsResult, DataVaultReadStrategyDiagnostics :: - Implement the read-shape payload as a fresh additive model/property on DataVaultDiagnosticsResult; do not overload DataVaultReadStrategyDiagnostics or imply that a read-shape member already exists.

Required PO actions
- Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.

Open issues ledger
- critic-item-1 [required-po-action] Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- critic-item-2 [blocking-finding] Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- critic-item-3 [blocking-finding] Unsupported inferred API claim: DataVaultDiagnosticsResult, ReadStrategy :: - DataVaultDiagnosticsResult already exists but currently only exposes ReadStrategy for read-bound data; the query-shape payload in this ticket is a new additive member/model that must be introduced explicitly.
- critic-item-4 [blocking-finding] Unsupported inferred API claim: Implement, DataVaultDiagnosticsResult, DataVaultReadStrategyDiagnostics :: - Implement the read-shape payload as a fresh additive model/property on DataVaultDiagnosticsResult; do not overload DataVaultReadStrategyDiagnostics or imply that a read-shape member already exists.

Missing examples / edge cases
- none

Risky assumptions
- Existing API/type assumption lacks source evidence: DataVaultDiagnosticsResult, ReadStrategy :: - DataVaultDiagnosticsResult already exists but currently only exposes ReadStrategy for read-bound data; the query-shape payload in this ticket is a new additive member/model that must be introduced explicitly.
- Existing API/type assumption lacks source evidence: Implement, DataVaultDiagnosticsResult, DataVaultReadStrategyDiagnostics :: - Implement the read-shape payload as a fresh additive model/property on DataVaultDiagnosticsResult; do not overload DataVaultReadStrategyDiagnostics or imply that a read-shape member already exists.

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
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment