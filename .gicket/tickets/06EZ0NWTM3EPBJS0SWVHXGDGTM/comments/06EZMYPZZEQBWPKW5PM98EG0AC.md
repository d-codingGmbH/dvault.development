[gicket-bot] PO refinement contract

Summary
- Refined the ticket around the existing explicit save boundary: optional timestamp and record-source hook interfaces must preserve zero-config defaults, resolve values once per DataVaultSaveRequest, and behave identically across fallback and provider-specific save paths.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The zero-configuration `AddDVault()` path stays intact; timestamp and record-source hooks are additive opt-in extension points, not required configuration.
- The current caller-visible metadata boundary remains `IDataVaultSaveService` plus `DataVaultSaveRequest`; this ticket should refine behavior around that boundary instead of introducing a hidden `SaveChanges` path.
- For v1, one `DataVaultSaveRequest` continues to produce one effective load timestamp and one effective record source shared by its contained hub, link, and satellite operations; per-row overrides inside a single request are out of scope.
- Deterministic fallback means that an unset hook uses the current default behavior; a configured hook that produces null, empty, ambiguous, non-UTC, or otherwise invalid output must fail clearly rather than silently falling back.
- Timestamp hooks resolve logical UTC values, while persisted provider-specific representation continues to follow the existing provider capability profile and model value-format rules.
- Record source remains required lineage metadata; hook behavior may normalize or replace the supplied value deterministically, but it must not remove the requirement or inject a generic unknown fallback.

Scope In
- Introduce optional timestamp and record-source hook interfaces on the core DVault advanced-configuration surface.
- Apply one shared timestamp/record-source resolution pipeline across `DefaultDataVaultSaveService` and all registered `IDataVaultProviderSaveStrategy` implementations.
- Preserve and validate UTC timestamp semantics and required deterministic record-source semantics at the explicit save boundary.
- Add regression and hook-behavior tests for default inheritance, custom behavior, and invalid hook outputs.

Scope Out
- Provider behavior hook surface and provider-specific option matrices owned by ticket `06EZ0NX282R80VF5VBKS6ARFZC`.
- Broader advanced-hook documentation and examples owned by ticket `06EZ0NX9SVP7MSB1R4PJ50EHGW`.
- Naming-hook or hashing-hook changes beyond consuming the already-existing stable hash services as baseline dependencies.
- Per hub/link/satellite-row metadata overrides inside one `DataVaultSaveRequest`.
- A new hidden ambient write path, `SaveChanges` interception, or any change that removes the explicit save-service boundary.

Open questions
- none

Follow-up questions
- If future advanced scenarios need timestamp or record-source derivation from richer ingest context than the current `DataVaultSaveRequest` carries, should that arrive through a new save-context payload rather than per-row overrides?
- Should a later ticket explicitly separate deterministic test-time and wall-clock timestamp modes, as suggested in `docs/plans/optional-advanced-configuration-hooks.md`?

Risks
- Provider save strategies currently duplicate timestamp and record-source handling, so any hook implementation that is not centralized can drift between fallback and optimized paths.
- Oracle already persists load timestamps as text while other providers use different model CLR mappings; careless hook-output rules can break round-tripping or chronological satellite ordering.
- Expanding beyond request-level resolution in this ticket risks cascading API changes across `DataVaultSaveRequest`, provider strategies, and sibling hook tickets.

Split recommendations
- If implementation starts needing provider-specific option objects, native timestamp precision controls, or other adapter-only behavior, move that work to `06EZ0NX282R80VF5VBKS6ARFZC`.
- If the effort grows into end-user documentation, narrative examples, or failure-mode guides beyond code comments and test evidence, move that work to `06EZ0NX9SVP7MSB1R4PJ50EHGW`.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment