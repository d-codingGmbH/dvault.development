[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract is bounded, has no unresolved Open Questions, and is backed by direct repository evidence for the existing read-service and PIT metadata baselines.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Current branch is `ticket/06F0MEGYHADPVN575H64D56W2G-task-define-pit-backed-as-of-read-api-contract`; `git rev-parse HEAD` returned `18e1ee0eecce07d7a5d40516fc34c94d08eabd7c`, matching the supplied scratch-source-ref.
- `.gicket/tickets/06F0MEGYHADPVN575H64D56W2G/description.md:7-9` records PO Handoff decision `ready_for_po_critic`; lines 53-54 record `## Open Questions` as `- none`.
- `.gicket/tickets/06F0MEGYHADPVN575H64D56W2G/description.md:18-23` scopes the work to documenting a PIT-backed as-of request/response contract, raw PIT read-record shape, multi-satellite/missing-row behavior, diagnostics, examples, and fixture expectations.
- `.gicket/tickets/06F0MEGYHADPVN575H64D56W2G/description.md:25-30` explicitly scopes out PIT maintenance, provider-specific SQL/optimization, bridge traversal, link-based PIT parents, PIT over multi-active satellites, legacy PointInTime reconciliation, and a second public read service.
- `.gicket/relations/P0/2G/06F0MEDJC732GDD77H60R259P0--06F0MEGYHADPVN575H64D56W2G--blocks.json:3-5` shows an upstream blocks relation from `06F0MEDJC732GDD77H60R259P0`; that ticket is `done` in `.gicket/tickets/06F0MEDJC732GDD77H60R259P0/ticket.json:3-8`.
- `.gicket/relations/2G/XC/06F0MEGYHADPVN575H64D56W2G--06F0MEH660Y5QTNR5P8JPS2QXC--blocks.json:3-5` and `.gicket/relations/2G/G8/06F0MEGYHADPVN575H64D56W2G--06F0MEJ7NANHCP64VR1SH3S3G8--blocks.json:3-5` show this contract ticket blocks downstream implementation/strategy tasks.
- `src/DCoding.Data.DVault/IDataVaultReadService.cs:5-19` directly defines the existing provider-neutral `IDataVaultReadService` latest/as-of read boundary returning `DataVaultSatelliteReadRecord` from `ReadLatestSatelliteRowsAsync`.
- `src/DCoding.Data.DVault/DataVaultLatestSatelliteReadRequest.cs:20-35` directly defines the current latest/as-of request shape with `DateTimeOffset? asOf` normalized to UTC; lines 38-51 expose `Satellite`, `ParentHashKeys`, and `AsOf`.
- `src/DCoding.Data.DVault/DataVaultSatelliteReadRecord.cs:6-24` and lines 36-64 expose the current raw record fields: parent hash key, driving-key values, hash diff, load timestamp, record source, and payload values.
- `src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs:18-53` directly shows caller-owned projector delegates over `IDataVaultReadService`, supporting the ticket's request to extend the existing projector pattern rather than introduce reflection binding.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:896-959` directly defines public `DataVaultPitMetadata` with `Parent`, ordered `Satellites`, hash-key metadata, load-timestamp metadata, and technical metadata columns; public API snapshot lines 880-893 also list `DataVaultPitMetadata` and `DataVaultPitSatelliteReferenceMetadata`.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:450-548` directly rejects link-based PIT parents, empty satellite sets, duplicate satellite references, multi-active PIT references, and link-attached/wrong-hub satellites with deterministic `NotSupportedException` diagnostics.
- `docs/plans/deferred-data-vault-capabilities.md:53-55` states `DataVaultMetadataModel.Pits` carries `DataVaultPitMetadata` plus ordered satellite references and that supported PIT projection requires one declared hub plus unique non-multi-active hub-attached satellites; lines 95-101 document canonical PIT column order and unsupported cases.
- `README.md:165-208` documents typed latest/as-of satellite projections and raw row reads as the current read-service baseline; `README.md:337-339` keeps PIT-backed read APIs and bridge traversal helpers as future work.
- `docs/releases/v0.6.0.md:24-25` says v0.6.0 added typed latest/as-of helpers and preserved raw reads; lines 38-39 state typed satellite reads use caller-supplied delegates and `IDataVaultReadService` remains latest/as-of satellite only; line 46 says PIT-backed read APIs are not delivered in v0.6.0.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Keep the existing AC requiring at least one multi-satellite typed projection example and one missing-PIT-row example as a hard contract output.
- Fixture/API snapshot coverage should capture the exact PIT request type, raw PIT record type, ordered satellite segment exposure, and deterministic diagnostics for unsupported multi-active, link-based, bridge-driven, and out-of-baseline PIT declarations.
- Include a timestamp example proving callers use `DateTimeOffset` while provider storage modes remain internal.

Implementation watchouts
- Do not introduce a second public read service or reflection DTO binding; source evidence shows the existing pattern is `IDataVaultReadService` plus caller-owned projector delegates.
- Use newer `Pit` vocabulary and avoid treating legacy `DataVaultPointInTimeMetadata` / `DataVaultModelBuilder.PointInTime(...)` as the v1 contract surface.
- Make absence explicit: no PIT row means no projected parent result; a matched PIT row with an absent satellite snapshot should expose that satellite segment as absent and must not fall back to latest/as-of satellite reads.
- Preserve `DataVaultPitMetadata` satellite declaration order in raw PIT record/projection shape so multi-satellite projectors are deterministic.

Non-blocking notes
- none

Split recommendations
- No split recommended; the ticket is already bounded to public contract documentation plus fixtures/snapshots/examples, with PIT maintenance, provider-specific optimization, bridge traversal, and legacy naming reconciliation out of scope.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment