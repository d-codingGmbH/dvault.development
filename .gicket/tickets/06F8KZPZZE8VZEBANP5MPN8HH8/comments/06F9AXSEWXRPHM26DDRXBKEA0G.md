[gicket-bot] PO refinement contract

Summary
- Refined the ticket as a bounded typed-helper generator transition-test story with no split or description update required; queued stale blocker cleanup is the only materialized planning action from this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository baseline already fixes the v1 helper contract: typed helpers consume exactly one authoritative `dvault.support-bundle.v1`, satellite helpers remain in scope, and PIT/bridge helpers require reviewed request-bound `readShape` evidence from the support bundle.
- For this ticket, schema-version or other incompatible support-bundle input stays on the existing `DMV1960` source-boundary path, while metadata-source fingerprint drift stays on `DMV1961`.
- Documented skip behavior is bounded: unsupported PIT or bridge facts skip only the affected helper and preserve other supported satellite, PIT, or bridge helpers from the same bundle.
- A stale incoming `blocks` relation from `06F8KZPN02NWFGMRC2Q1PKYKDR` was already targeted for cleanup; the removal is queued for replay on that ticket's owner branch, so the latest live relation read still shows it until replay.

Scope In
- Add analyzer/source-generator regression tests for support-bundle freshness transitions across successive runs of the typed read-model generator.
- Cover satellite, PIT, and bridge helper generation/removal when authoritative support-bundle evidence is refreshed, becomes stale, or becomes incompatible.
- Assert generator behavior for fingerprint mismatch (`DMV1961`) and schema-version or other incompatible support-bundle input (`DMV1960`).
- Assert partial-generation skip behavior so unsupported PIT/bridge facts suppress only the affected helper while other supported helpers continue to generate.

Scope Out
- Changing the typed helper public contract, naming pattern, or helper method signatures documented for v1.
- Adding new runtime read semantics, PIT/bridge maintenance behavior, or provider-specific SQL/read-strategy features.
- Changing support-bundle export workflows, raw `dvault.model.v1` parsing, or broader metadata/import architecture.
- General analyzer diagnostic redesign outside the existing `DMV1960`/`DMV1961` boundary for these transition scenarios.

Open questions
- none

Follow-up questions
- After the queued stale-blocker replay completes on `06F8KZPN02NWFGMRC2Q1PKYKDR`, confirm that shared board or reporting views no longer surface the old incoming blocker edge.

Risks
- The stale incoming `blocks` relation from `06F8KZPN02NWFGMRC2Q1PKYKDR` remains visible in the latest live relation read until the queued replay runs on that ticket's owner branch.
- Ticket `06F8KZQAWZ7QRGB68KB21C9B0R` remains blocked by this story until the transition-test coverage is delivered.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment