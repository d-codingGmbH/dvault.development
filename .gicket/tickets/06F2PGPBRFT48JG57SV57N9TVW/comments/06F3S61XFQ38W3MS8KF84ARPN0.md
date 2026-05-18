[gicket-bot] PO-critic review contract

Summary
- Approved for dev: the persisted contract is bounded, has no open questions, and matches the observed repository service and PIT-read boundaries.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGPBRFT48JG57SV57N9TVW/ticket.json:7-17` shows the ticket is still `todo`, carries `critic-needed` instead of `needs-po`, and has no assignees.
- `.gicket/tickets/06F2PGPBRFT48JG57SV57N9TVW/description.md:30-50` defines 5 acceptance-criteria items, 4 definition-of-done items, and `## Open Questions` is `- none`, so the persisted delivery contract has no unresolved PO questions.
- `.gicket/tickets/06F2PGPBRFT48JG57SV57N9TVW/comments/06F3S1K4J7M0XVN0HE5RZKBHKW.md:6-30` marks the ticket `ready_for_po_critic` and scopes PIT maintenance in/out; the other visible ticket comments are bot claim/lease/orchestration entries (`06F3RXV...`, `06F3RXW...`, `06F3S4NV...`, `06F3S4P5...`) with no human scope comments.
- `docs/plans/pit-maintenance-service-v1-contract.md:14-53` directly specifies the additive explicit maintenance service, deterministic row-generation rule, parent-scoped recomputation, no-op empty parent input, and separate service boundary for this ticket.
- `README.md:253-255` and `docs/releases/v0.7.0.md:53-55` already document PIT reads as consuming already materialized PIT tables and not maintaining or refreshing them implicitly, which matches the ticket's compatibility baseline.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-31` currently registers `IDataVaultSaveService` and `IDataVaultReadService`; `src/DCoding.Data.DVault/IDataVaultReadService.cs:21-31` already exposes PIT reads; `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:920-980` defines `DataVaultPitMetadata`.
- `src/DCoding.Data.DVault/DataVaultPitReadPipeline.cs:303-356` already rejects non-hub PIT parents, duplicate satellite references, and multi-active satellites for PIT reads, and the persisted ticket AC at `.gicket/tickets/06F2PGPBRFT48JG57SV57N9TVW/description.md:33-35` asks maintenance to preserve that supported-v1 shape.
- Relation files `.gicket/relations/VW/XC/06F2PGPBRFT48JG57SV57N9TVW--06F2PGPKXWRFXNPFA1JR0X67XC--blocks.json:1-10`, `.gicket/relations/VW/VG/06F2PGPBRFT48JG57SV57N9TVW--06F2PGPXVAYRBC94RQ7X5V4DVG--blocks.json:1-10`, `.gicket/relations/XC/6W/06F2PGPKXWRFXNPFA1JR0X67XC--06F2PGPRGN0EVGD6RY5KY9M56W--blocks.json:1-10`, and `.gicket/relations/6W/VG/06F2PGPRGN0EVGD6RY5KY9M56W--06F2PGPXVAYRBC94RQ7X5V4DVG--blocks.json:1-10` persist the intended delivery chain.
- `git rev-parse HEAD` matched the supplied scratch source ref `25b726f29f326dd1d52d791391844cd0532093be`, and `git show --name-only HEAD` touched only `.gicket/tickets/06F2PGPBRFT48JG57SV57N9TVW/*`; recent branch history is ticket/planning handoff metadata rather than implementation, which fits a pre-development gate.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete worked example for a targeted parent whose recomputation finds zero visible satellite rows would remove any doubt that existing PIT rows should be deleted and nothing reinserted.
- A small late-arriving correction example spanning two satellites would make review of the deterministic row-replacement rule faster, even though the acceptance criteria already require that behavior.
- The ticket mentions missing-satellite coverage, but it does not illustrate whether a satellite with no rows at all versus a satellite with gaps in history should produce the same null snapshot-column behavior.

Risky assumptions
- The implementation is expected to reuse the current PIT read baseline rather than redefine it; that is reasonable, but it assumes write-side validation can mirror the existing read-side guardrails without hidden gaps.
- Documentation follow-through is intentionally deferred to `06F2PGPXVAYRBC94RQ7X5V4DVG`; the current repo has release notes through `docs/releases/v0.14.0.md`, so the later doc task will need to decide how `v0.15.0` notes are created.
- Legacy `DataVaultPointInTimeMetadata` and `DataVaultModelBuilder.PointInTime(...)` remain in source, so the story assumes developers will keep the new maintenance work scoped to `DataVaultPitMetadata` and not broaden it into legacy PIT cleanup.

AC / test suggestions
- Keep one explicit test around duplicate parent hash-key inputs using ordinal comparison, since the durable planning contract specifies `StringComparer.Ordinal` for bounded maintenance inputs.
- Keep one explicit test that rebuild and targeted-parent maintenance both preserve current PIT read compatibility by producing the existing `ParentHashKey`, `LoadTimestamp`, and `<Satellite>LoadTimestamp` column shape.
- Keep one explicit test that validation fails before any write when the PIT parent is not a hub or when a participating satellite is multi-active or duplicated, to match the observed read-side baseline.

Implementation watchouts
- Do not hide PIT refresh behind `SaveChanges`, interceptors, or `IDataVaultReadService`; the current explicit-service boundary is central to the approved scope.
- Mirror the existing PIT read guardrails in `DataVaultPitReadPipeline.ValidatePitShape(...)` so maintenance does not accept shapes that PIT reads already reject.
- Preserve the generated PIT table and column contract already consumed by `ReadPitRowsAsync(...)`; downstream tickets are explicitly relying on stable row-population semantics rather than a new read API.

Non-blocking notes
- The authoritative ticket contract scopes user-facing README and release-note follow-through to `06F2PGPXVAYRBC94RQ7X5V4DVG`, even though the durable planning contract also mentions updating release-note task language; the ticket description should stay authoritative for dev handoff.

Split recommendations
- None; the current story is already split cleanly from downstream query/API, provider-optimization, and documentation follow-on work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment