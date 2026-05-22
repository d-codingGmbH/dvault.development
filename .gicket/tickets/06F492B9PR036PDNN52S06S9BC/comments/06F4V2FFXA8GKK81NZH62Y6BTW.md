[gicket-bot] PO-critic review contract

Summary
- Ticket contract assumes existing source APIs/types that are not evidenced on the current branch and must return to Product Owner refinement.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Observed persisted delivery contract infers an existing public API/type without corresponding visible source evidence in the current branch snapshot or referenced repository documents.
- Unsupported inferred API claim in contract: Revalidated, IDataVaultReadDiagnosticsService, Analyze, DataVaultDiagnosticsResult, Explain, SaveStrategy, ReadStrategy, No :: - Revalidated the ticket against current branch source. IDataVaultReadDiagnosticsService already exposes request-bound Analyze overloads for latest-satellite, registry latest-satellite, PIT, bridge, and registry bridge requests, while DataVaultDiagnosticsResult currently exposes constructor fields Validation/Explain/SaveStrategy/Issues plus init-only ReadStrategy. The contract is narrowed so query-shape diagnostics are introduced as a new additive member and supporting model(s), not inferred as preexisting. No child tickets, relation writes, attachments, or planning documents were materialized.

Blocking findings
- Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Unsupported inferred API claim: Revalidated, IDataVaultReadDiagnosticsService, Analyze, DataVaultDiagnosticsResult, Explain, SaveStrategy, ReadStrategy, No :: - Revalidated the ticket against current branch source. IDataVaultReadDiagnosticsService already exposes request-bound Analyze overloads for latest-satellite, registry latest-satellite, PIT, bridge, and registry bridge requests, while DataVaultDiagnosticsResult currently exposes constructor fields Validation/Explain/SaveStrategy/Issues plus init-only ReadStrategy. The contract is narrowed so query-shape diagnostics are introduced as a new additive member and supporting model(s), not inferred as preexisting. No child tickets, relation writes, attachments, or planning documents were materialized.

Required PO actions
- Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.

Open issues ledger
- critic-item-1 [required-po-action] Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- critic-item-2 [blocking-finding] Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- critic-item-3 [blocking-finding] Unsupported inferred API claim: Revalidated, IDataVaultReadDiagnosticsService, Analyze, DataVaultDiagnosticsResult, Explain, SaveStrategy, ReadStrategy, No :: - Revalidated the ticket against current branch source. IDataVaultReadDiagnosticsService already exposes request-bound Analyze overloads for latest-satellite, registry latest-satellite, PIT, bridge, and registry bridge requests, while DataVaultDiagnosticsResult currently exposes constructor fields Validation/Explain/SaveStrategy/Issues plus init-only ReadStrategy. The contract is narrowed so query-shape diagnostics are introduced as a new additive member and supporting model(s), not inferred as preexisting. No child tickets, relation writes, attachments, or planning documents were materialized.

Missing examples / edge cases
- none

Risky assumptions
- Existing API/type assumption lacks source evidence: Revalidated, IDataVaultReadDiagnosticsService, Analyze, DataVaultDiagnosticsResult, Explain, SaveStrategy, ReadStrategy, No :: - Revalidated the ticket against current branch source. IDataVaultReadDiagnosticsService already exposes request-bound Analyze overloads for latest-satellite, registry latest-satellite, PIT, bridge, and registry bridge requests, while DataVaultDiagnosticsResult currently exposes constructor fields Validation/Explain/SaveStrategy/Issues plus init-only ReadStrategy. The contract is narrowed so query-shape diagnostics are introduced as a new additive member and supporting model(s), not inferred as preexisting. No child tickets, relation writes, attachments, or planning documents were materialized.

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