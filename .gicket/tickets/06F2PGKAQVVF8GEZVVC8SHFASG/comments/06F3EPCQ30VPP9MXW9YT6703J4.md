[gicket-bot] PO-critic review contract

Summary
- Ticket contract assumes existing source APIs/types that are not evidenced on the current branch and must return to Product Owner refinement.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Observed persisted delivery contract infers an existing public API/type without corresponding visible source evidence in the current branch snapshot or referenced repository documents.
- Unsupported inferred API claim in contract: DataVaultCodeFirstSatelliteBuilder, T, DataVaultCodeFirstHubBuilder :: - The existing public reuse point for satellite member selection is `DataVaultCodeFirstSatelliteBuilder<T>`, already used by `DataVaultCodeFirstHubBuilder<TEntity>.Satellite(...)` for `Payload(...)` and optional `DrivingKey(...)` behavior.
- Unsupported inferred API claim in contract: TSatellite, DataVaultCodeFirstSatelliteBuilder :: - Add `Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null)` on the existing public `DataVaultCodeFirstLinkBuilder` while leaving current `Link(...)` overloads and `Participant<TEntity>()` semantics additive and unchanged.
- Unsupported inferred API claim in contract: State :: - Any assumption that a product CLR type named `State` already exists; tests may introduce a local sample type if useful.
- Unsupported inferred API claim in contract: Reuse, DataVaultCodeFirstSatelliteBuilder, T :: - Reuse the existing public `DataVaultCodeFirstSatelliteBuilder<T>` rather than introducing a parallel link-satellite builder type; the current hub builder already proves that builder's intended `Payload(...)` and `DrivingKey(...)` semantics.

Blocking findings
- Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Unsupported inferred API claim: DataVaultCodeFirstSatelliteBuilder, T, DataVaultCodeFirstHubBuilder :: - The existing public reuse point for satellite member selection is `DataVaultCodeFirstSatelliteBuilder<T>`, already used by `DataVaultCodeFirstHubBuilder<TEntity>.Satellite(...)` for `Payload(...)` and optional `DrivingKey(...)` behavior.
- Unsupported inferred API claim: TSatellite, DataVaultCodeFirstSatelliteBuilder :: - Add `Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null)` on the existing public `DataVaultCodeFirstLinkBuilder` while leaving current `Link(...)` overloads and `Participant<TEntity>()` semantics additive and unchanged.
- Unsupported inferred API claim: State :: - Any assumption that a product CLR type named `State` already exists; tests may introduce a local sample type if useful.
- Unsupported inferred API claim: Reuse, DataVaultCodeFirstSatelliteBuilder, T :: - Reuse the existing public `DataVaultCodeFirstSatelliteBuilder<T>` rather than introducing a parallel link-satellite builder type; the current hub builder already proves that builder's intended `Payload(...)` and `DrivingKey(...)` semantics.

Required PO actions
- Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.

Open issues ledger
- critic-item-1 [required-po-action] Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- critic-item-2 [blocking-finding] Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- critic-item-3 [blocking-finding] Unsupported inferred API claim: DataVaultCodeFirstSatelliteBuilder, T, DataVaultCodeFirstHubBuilder :: - The existing public reuse point for satellite member selection is `DataVaultCodeFirstSatelliteBuilder<T>`, already used by `DataVaultCodeFirstHubBuilder<TEntity>.Satellite(...)` for `Payload(...)` and optional `DrivingKey(...)` behavior.
- critic-item-4 [blocking-finding] Unsupported inferred API claim: TSatellite, DataVaultCodeFirstSatelliteBuilder :: - Add `Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null)` on the existing public `DataVaultCodeFirstLinkBuilder` while leaving current `Link(...)` overloads and `Participant<TEntity>()` semantics additive and unchanged.
- critic-item-5 [blocking-finding] Unsupported inferred API claim: State :: - Any assumption that a product CLR type named `State` already exists; tests may introduce a local sample type if useful.
- critic-item-6 [blocking-finding] Unsupported inferred API claim: Reuse, DataVaultCodeFirstSatelliteBuilder, T :: - Reuse the existing public `DataVaultCodeFirstSatelliteBuilder<T>` rather than introducing a parallel link-satellite builder type; the current hub builder already proves that builder's intended `Payload(...)` and `DrivingKey(...)` semantics.

Missing examples / edge cases
- none

Risky assumptions
- Existing API/type assumption lacks source evidence: DataVaultCodeFirstSatelliteBuilder, T, DataVaultCodeFirstHubBuilder :: - The existing public reuse point for satellite member selection is `DataVaultCodeFirstSatelliteBuilder<T>`, already used by `DataVaultCodeFirstHubBuilder<TEntity>.Satellite(...)` for `Payload(...)` and optional `DrivingKey(...)` behavior.
- Existing API/type assumption lacks source evidence: TSatellite, DataVaultCodeFirstSatelliteBuilder :: - Add `Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null)` on the existing public `DataVaultCodeFirstLinkBuilder` while leaving current `Link(...)` overloads and `Participant<TEntity>()` semantics additive and unchanged.
- Existing API/type assumption lacks source evidence: State :: - Any assumption that a product CLR type named `State` already exists; tests may introduce a local sample type if useful.
- Existing API/type assumption lacks source evidence: Reuse, DataVaultCodeFirstSatelliteBuilder, T :: - Reuse the existing public `DataVaultCodeFirstSatelliteBuilder<T>` rather than introducing a parallel link-satellite builder type; the current hub builder already proves that builder's intended `Payload(...)` and `DrivingKey(...)` semantics.

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