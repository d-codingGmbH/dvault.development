[gicket-bot] PO-critic review contract

Summary
- Ticket is close, but the developer still has to invent the public typed-read projection contract and its nullability/name-collision rules.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F0MECPFAVBFBNC5XMVDZRQ6M/description.md:57-58` shows `## Open Questions` followed by `- none`.
- `src/DCoding.Data.DVault/IDataVaultReadService.cs:8-19`, `src/DCoding.Data.DVault/DataVaultLatestSatelliteReadRequest.cs:14-35`, `src/DCoding.Data.DVault/DataVaultRegistryLatestSatelliteReadRequest.cs:15-40`, and `src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs:26-44` confirm the repo already has explicit-metadata and registry-backed latest/as-of reads on one provider-neutral read-service path.
- `src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:33-35` and `:87-148` show malformed or null/non-string values are skipped today because `TryCreateReadRecord(...)` returns `false` and the caller `continue`s.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:925-947` and `:<redacted>` already prove explicit and registry-backed raw latest/as-of reads, including `AsOf` cutoff behavior and normalized `LoadTimestamp` results.
- `docs/architecture/dvault-v1-typed-row-mapper-contract.md:8-24` pins the save-side typed API to concrete interfaces and request boundaries, but an `rg -n` search for `IDataVault.*Project|Projector|typed projection|Projection contract` across `docs src tests` returned no existing read-side projection contract surface.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:754-760,820-845` only forbids payload/driving-key overlap, while `src/DCoding.Data.DVault/Modeling/DefaultNamingPolicy.cs:52-68` separately reserves fixed technical produced-column names `HashDiff`, `LoadTimestamp`, and `RecordSource`; the read-side exact-name collision rule is not defined by the ticket.
- `.gicket/tickets/06F0MEB634X6CTBZ00W108G3FG/ticket.json:7-15` and `.gicket/tickets/06F0MEC7FEXAD069AJNYZW0DRM/ticket.json:7-15` are `done`, while `.gicket/tickets/06F0MEDJC732GDD77H60R259P0/ticket.json:7-15` remains `todo` as the downstream docs follow-up.

Blocking findings
- The ticket does not pin the actual public typed-read contract shape. `description.md:15,21,34-47,49-55` requires a `thin typed projection contract` plus public API/XML-doc/snapshot updates, but it never states whether callers use an interface, delegate, builder, or another explicit binder. A developer would still have to invent the core public API.
- Nullability behavior is acceptance-critical, but the contract does not say how required versus nullable fields are declared once ambient DTO inference and reflection-discovered binding are explicitly out of scope. `description.md:14-15,25,31,39,45,53` defines the failure behavior without defining where those nullability requirements come from.
- Exact-name collisions between technical fields and logical payload/driving-key names are unresolved. The contract wants one exact-name projection space over `ParentHashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, payloads, and driving keys, while current metadata validation only forbids driving-key/payload overlap. The developer would have to invent precedence or reservation rules.

Required PO actions
- Amend the ticket with one concrete v1 typed-read contract example and explicitly choose the baseline public surface for callers (for example interface-based, delegate-based, or builder-based), including how both explicit and registry-backed helper APIs consume it.
- State how the manual projection contract expresses required versus nullable fields, and what deterministic diagnostic shape is expected when a required value is missing or null.
- Define the collision rule for technical field names versus payload/driving-key names: either reserve/disallow those overlaps up front or specify deterministic precedence/aliasing.

Open issues ledger
- critic-item-1 [required-po-action] Amend the ticket with one concrete v1 typed-read contract example and explicitly choose the baseline public surface for callers (for example interface-based, delegate-based, or builder-based), including how both explicit and registry-backed helper APIs consume it.
- critic-item-2 [required-po-action] State how the manual projection contract expresses required versus nullable fields, and what deterministic diagnostic shape is expected when a required value is missing or null.
- critic-item-3 [required-po-action] Define the collision rule for technical field names versus payload/driving-key names: either reserve/disallow those overlaps up front or specify deterministic precedence/aliasing.
- critic-item-4 [blocking-finding] The ticket does not pin the actual public typed-read contract shape. `description.md:15,21,34-47,49-55` requires a `thin typed projection contract` plus public API/XML-doc/snapshot updates, but it never states whether callers use an interface, delegate, builder, or another explicit binder. A developer would still have to invent the core public API.
- critic-item-5 [blocking-finding] Nullability behavior is acceptance-critical, but the contract does not say how required versus nullable fields are declared once ambient DTO inference and reflection-discovered binding are explicitly out of scope. `description.md:14-15,25,31,39,45,53` defines the failure behavior without defining where those nullability requirements come from.
- critic-item-6 [blocking-finding] Exact-name collisions between technical fields and logical payload/driving-key names are unresolved. The contract wants one exact-name projection space over `ParentHashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, payloads, and driving keys, while current metadata validation only forbids driving-key/payload overlap. The developer would have to invent precedence or reservation rules.

Missing examples / edge cases
- A concrete registry-backed typed read example that shows the manual projection contract and the raw-record escape hatch side by side.
- A DTO example with one nullable payload, one required payload, and one required technical or driving-key field so the nullability contract is testable.
- A satellite whose logical payload or driving-key names collide with technical names such as `HashDiff`, `LoadTimestamp`, `RecordSource`, or the parent-hash-key exposure token.
- A link-parent multi-active typed projection example if the chosen contract shape makes series identity or ordering exposure non-obvious.

Risky assumptions
- Assuming the developer will infer the intended read-side public API from the save-side typed mapper pattern even though no read-side equivalent contract is currently documented in the repo.
- Assuming nullability can be derived from CLR DTO metadata without violating the ticket's explicit `no DTO CLR-type lookup / no reflection-discovered binder` boundary.
- Assuming no real satellite metadata will use logical names that collide with technical field names.

AC / test suggestions
- Add one explicit acceptance/example snippet that shows the exact public typed projection contract for both `DataVaultLatestSatelliteReadRequest` and `DataVaultRegistryLatestSatelliteReadRequest` entry points.
- Add an acceptance/test for the chosen collision rule between technical field names and logical payload/driving-key names.
- Add an acceptance/test that demonstrates how nullable versus required fields are declared and how the resulting diagnostic identifies both the satellite and the offending mapped name.

Implementation watchouts
- Do not layer typed projection only on top of `DataVaultSatelliteReadRecord`; `DefaultDataVaultReadService.cs:33-35,87-148` drops malformed/null rows before they become raw records.
- Reuse the existing batching, as-of cutoff, series grouping, and ordinal ordering behavior from `DefaultDataVaultReadService.cs:21-49` so typed and raw reads stay semantically aligned.
- Keep explicit and registry-backed entry points as companion helpers over the existing service rather than widening `IDataVaultReadService`, consistent with `DataVaultReadServiceRegistryExtensions.cs:8-45`.
- Preserve exact `StringComparer.Ordinal` name matching and current load-timestamp normalization across provider-default, ISO 8601 text, and UTC-ticks storage modes.

Non-blocking notes
- The ticket's formal open-question section is clean; the return is for missing contract precision, not unresolved Q and A.
- The prerequisite configuration and typed-mapper tickets are already done, and the downstream README/release-doc task remains correctly separated.
- No split is needed if the PO refines this same ticket with the missing API-contract, nullability, and name-collision rules.

Split recommendations
- No split recommended. Keep this as one ticket, but refine the same ticket with a concrete public projection contract example, a nullability-declaration rule, and a technical-name collision rule before dev handoff.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment