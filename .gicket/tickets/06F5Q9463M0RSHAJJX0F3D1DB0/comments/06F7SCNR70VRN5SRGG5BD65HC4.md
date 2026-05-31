[gicket-bot] PO-critic review contract

Summary
- Ticket contract assumes existing source APIs/types that are not evidenced on the current branch and must return to Product Owner refinement.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Observed persisted delivery contract infers an existing public API/type without corresponding visible source evidence in the current branch snapshot or referenced repository documents.
- Unsupported inferred API claim in contract: Instrument :: - Instrument latest-satellite and PIT explicit read boundaries plus the existing public helper surfaces for current/as-of, typed projection, registry latest-satellite, PIT projection, and bridge reads so each execution emits one root save/read span without duplicate wrapper spans.
- Unsupported inferred API claim in contract: Do :: - Do not assume or require any pre-existing public tracing API; if a holder/helper is added by this story it stays internal unless an intentional public API change is snapshot-reviewed.

Blocking findings
- Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Unsupported inferred API claim: Instrument :: - Instrument latest-satellite and PIT explicit read boundaries plus the existing public helper surfaces for current/as-of, typed projection, registry latest-satellite, PIT projection, and bridge reads so each execution emits one root save/read span without duplicate wrapper spans.
- Unsupported inferred API claim: Do :: - Do not assume or require any pre-existing public tracing API; if a holder/helper is added by this story it stays internal unless an intentional public API change is snapshot-reviewed.

Required PO actions
- Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.

Open issues ledger
- critic-item-1 [required-po-action] Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- critic-item-2 [blocking-finding] Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- critic-item-3 [blocking-finding] Unsupported inferred API claim: Instrument :: - Instrument latest-satellite and PIT explicit read boundaries plus the existing public helper surfaces for current/as-of, typed projection, registry latest-satellite, PIT projection, and bridge reads so each execution emits one root save/read span without duplicate wrapper spans.
- critic-item-4 [blocking-finding] Unsupported inferred API claim: Do :: - Do not assume or require any pre-existing public tracing API; if a holder/helper is added by this story it stays internal unless an intentional public API change is snapshot-reviewed.

Missing examples / edge cases
- none

Risky assumptions
- Existing API/type assumption lacks source evidence: Instrument :: - Instrument latest-satellite and PIT explicit read boundaries plus the existing public helper surfaces for current/as-of, typed projection, registry latest-satellite, PIT projection, and bridge reads so each execution emits one root save/read span without duplicate wrapper spans.
- Existing API/type assumption lacks source evidence: Do :: - Do not assume or require any pre-existing public tracing API; if a holder/helper is added by this story it stays internal unless an intentional public API change is snapshot-reviewed.

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