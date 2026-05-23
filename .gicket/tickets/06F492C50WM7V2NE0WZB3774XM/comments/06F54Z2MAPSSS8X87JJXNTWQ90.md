[gicket-bot] PO refinement contract

Summary
- Contract corrected to remove unverified read-diagnostics type assumptions, anchor scope to visible `IDataVaultDiagnosticsService` evidence, and keep the ticket as one bounded additive diagnostics story.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract no longer claims a pre-existing `IDataVaultReadDiagnosticsService` or other unverified carrier types. It anchors to visible `IDataVaultDiagnosticsService` evidence and allows any missing projection or lookup carrier members or types to be added explicitly.
- critic-item-2: `answered` - The delivery contract now describes additive extension of the current request-bound diagnostics flow instead of preserving exact public types that are not visibly evidenced on this branch.
- critic-item-3: `answered` - The unsupported baseline claim is withdrawn. Visible evidence supports `IDataVaultDiagnosticsService` and an existing read-shape diagnostics scenario; exact carrier types such as `DataVaultReadShapeDiagnostics` or `DataVaultDiagnosticsResult.ReadShape` are treated as existing only if confirmed in source during implementation, otherwise they may be created additively.
- critic-item-4: `answered` - Compatibility is re-anchored to existing diagnostics service behavior and current observable read-shape semantics, not to the previously inferred `IDataVaultReadDiagnosticsService` or `DataVaultReadShapeDiagnostics` anchor.
- critic-item-5: `answered` - The risk is restated without assuming existing public records: preserve current request-bound diagnostics behavior and any already-shipped members; add new projection or PIT lookup surface only additively.

Clarifications
- Verified live relations remain unchanged: epic `06F492BTNHRPBC7D24E13ECFKM` is `parentOf` this ticket, and this ticket still `blocks` `06F492CAB2293R7BGJWMWMRKT4` and `06F492D05THPGQVT3B3K7853A0`.
- The baseline is now the source-visible `IDataVaultDiagnosticsService` plus the existing read-diagnostics path exercised by `ReadDiagnosticsPopulateReadShapeForExplicitRegistryPitAndBridgeRequests`; earlier unverified `IDataVaultReadDiagnosticsService` wording is removed.
- No child ticket, relation mutation, attachment, or planning document was materialized in this pass.

Scope In
- Add bounded query-shape performance facts for supported latest/current/as-of satellite reads, PIT reads, and bridge reads through the existing request-bound diagnostics implementation path in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs`.
- Add deterministic projected-column group output for satellite, PIT, and bridge read diagnostics.
- Add PIT `ReferencedSatelliteLookupCount` as a bounded diagnostic fact.
- Update tests and any existing public API compatibility snapshot coverage needed to prove the additive surface and preserved redaction and registry-backed behavior.

Scope Out
- No claimed pre-existing `IDataVaultReadDiagnosticsService`, and no second root diagnostics service when the visible existing `IDataVaultDiagnosticsService` surface can carry the additive facts.
- No raw SQL, execution plans, live schema inspection, automatic index creation, provider-specific physical-plan advice, or disclosure of request values such as parent hash keys or raw as-of timestamps.
- No broader join-count, predicate-decomposition, provider package, analyzer, save-diagnostics, telemetry, or release-automation changes in this story.

Open questions
- none

Follow-up questions
- If product later wants richer predicate decomposition, join-plan hints, or provider-specific tuning guidance beyond projected columns and PIT lookup counts, should that ship as a separate follow-up story?

Risks
- If implementation changes the current request-bound diagnostics payload shape or redaction behavior instead of adding bounded new facts, existing consumers may break.
- If projection role names vary by provider or request path, explicit and registry-backed diagnostics become harder to compare; keep role names deterministic and provider-neutral.
- If implementation creates a parallel read-diagnostics carrier instead of extending the source surface already used by current tests, consumers may see duplicate or inconsistent contracts.

Split recommendations
- No split is required after removing the unsupported type assumptions; the ticket remains one bounded additive diagnostics refinement.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment