[gicket-bot] PO-critic review contract

Summary
- Ticket contract assumes existing source APIs/types that are not evidenced on the current branch and must return to Product Owner refinement.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Observed persisted delivery contract infers an existing public API/type without corresponding visible source evidence in the current branch snapshot or referenced repository documents.
- Unsupported inferred API claim in contract: Verified, DataVaultDiagnosticsResult, ReadShape, IDataVaultReadDiagnosticsService :: - Verified current branch source: `DataVaultDiagnosticsResult.ReadShape`, the public read-shape model types, request-bound `IDataVaultReadDiagnosticsService` overloads, and support-bundle export already exist, so this story remains an additive extension of that surface with performance-stage signals and index/provider guidance. No child tickets, relation writes, attachments, or planning documents were materialized.
- Unsupported inferred API claim in contract: Extend, DataVaultReadShapeDiagnostics :: - Extend the existing public `DataVaultReadShapeDiagnostics` family and nested public read-shape model types with additive performance-stage facts for projected columns, join count, predicate shape, ordering or row-selection posture, likely index needs, and bounded provider caveats.
- Unsupported inferred API claim in contract: Extend, DataVaultReadShapeDiagnostics, DataVaultDiagnosticsResult, ReadShape :: - Extend the existing public types in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` rather than adding a second diagnostics member or service; the current branch already exposes `DataVaultReadShapeDiagnostics`, nested public read-shape records, and `DataVaultDiagnosticsResult.ReadShape`.

Blocking findings
- Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Unsupported inferred API claim: Verified, DataVaultDiagnosticsResult, ReadShape, IDataVaultReadDiagnosticsService :: - Verified current branch source: `DataVaultDiagnosticsResult.ReadShape`, the public read-shape model types, request-bound `IDataVaultReadDiagnosticsService` overloads, and support-bundle export already exist, so this story remains an additive extension of that surface with performance-stage signals and index/provider guidance. No child tickets, relation writes, attachments, or planning documents were materialized.
- Unsupported inferred API claim: Extend, DataVaultReadShapeDiagnostics :: - Extend the existing public `DataVaultReadShapeDiagnostics` family and nested public read-shape model types with additive performance-stage facts for projected columns, join count, predicate shape, ordering or row-selection posture, likely index needs, and bounded provider caveats.
- Unsupported inferred API claim: Extend, DataVaultReadShapeDiagnostics, DataVaultDiagnosticsResult, ReadShape :: - Extend the existing public types in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` rather than adding a second diagnostics member or service; the current branch already exposes `DataVaultReadShapeDiagnostics`, nested public read-shape records, and `DataVaultDiagnosticsResult.ReadShape`.

Required PO actions
- Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.

Open issues ledger
- critic-item-1 [required-po-action] Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- critic-item-2 [blocking-finding] Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- critic-item-3 [blocking-finding] Unsupported inferred API claim: Verified, DataVaultDiagnosticsResult, ReadShape, IDataVaultReadDiagnosticsService :: - Verified current branch source: `DataVaultDiagnosticsResult.ReadShape`, the public read-shape model types, request-bound `IDataVaultReadDiagnosticsService` overloads, and support-bundle export already exist, so this story remains an additive extension of that surface with performance-stage signals and index/provider guidance. No child tickets, relation writes, attachments, or planning documents were materialized.
- critic-item-4 [blocking-finding] Unsupported inferred API claim: Extend, DataVaultReadShapeDiagnostics :: - Extend the existing public `DataVaultReadShapeDiagnostics` family and nested public read-shape model types with additive performance-stage facts for projected columns, join count, predicate shape, ordering or row-selection posture, likely index needs, and bounded provider caveats.
- critic-item-5 [blocking-finding] Unsupported inferred API claim: Extend, DataVaultReadShapeDiagnostics, DataVaultDiagnosticsResult, ReadShape :: - Extend the existing public types in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` rather than adding a second diagnostics member or service; the current branch already exposes `DataVaultReadShapeDiagnostics`, nested public read-shape records, and `DataVaultDiagnosticsResult.ReadShape`.

Missing examples / edge cases
- none

Risky assumptions
- Existing API/type assumption lacks source evidence: Verified, DataVaultDiagnosticsResult, ReadShape, IDataVaultReadDiagnosticsService :: - Verified current branch source: `DataVaultDiagnosticsResult.ReadShape`, the public read-shape model types, request-bound `IDataVaultReadDiagnosticsService` overloads, and support-bundle export already exist, so this story remains an additive extension of that surface with performance-stage signals and index/provider guidance. No child tickets, relation writes, attachments, or planning documents were materialized.
- Existing API/type assumption lacks source evidence: Extend, DataVaultReadShapeDiagnostics :: - Extend the existing public `DataVaultReadShapeDiagnostics` family and nested public read-shape model types with additive performance-stage facts for projected columns, join count, predicate shape, ordering or row-selection posture, likely index needs, and bounded provider caveats.
- Existing API/type assumption lacks source evidence: Extend, DataVaultReadShapeDiagnostics, DataVaultDiagnosticsResult, ReadShape :: - Extend the existing public types in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` rather than adding a second diagnostics member or service; the current branch already exposes `DataVaultReadShapeDiagnostics`, nested public read-shape records, and `DataVaultDiagnosticsResult.ReadShape`.

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