[gicket-bot] PO refinement contract

Summary
- Refined multi-active PIT support into a bounded shared-driving-key expansion over the current hub-parent PIT baseline; no persistent planning writes were applied.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows current PIT translation, maintenance, read pipelines, README, and release guidance all reject multi-active PITs, so this story is a real contract expansion rather than a naming cleanup.
- The bounded v1 default is still hub-parent PIT only, but it now allows one shared canonical driving-key set across all referenced multi-active hub-parent satellites; ordinary hub-parent satellites may participate as parent-wide snapshots in the same PIT.
- A multi-active tuple contributes PIT history only after that tuple first becomes visible; from that point onward the existing distinct-timestamp and carry-forward PIT rule is applied per `(parentHashKey, drivingKeyTuple)` without collapsing tuple series.
- Live relation state was left unchanged in this run: the ticket is a child of epic `06F5Q90CSKMGK3NZZ25XTW6W4C`, still has an incoming `blocks` relation from done story `06F5Q90KC6JGQPSP285XQYSPK8`, and still blocks diagnostics story `06F5Q91DR1555RSBQT7KDST684`.
- No child tickets, relation cleanup, description updates, attachments, or planning documents were materialized in this refinement run.

Scope In
- Expand `DataVaultPitMetadata` support for hub-parent PITs that reference one or more multi-active hub-parent satellites sharing the same canonical driving-key names and order.
- Project PIT driving-key columns, expanded PIT primary-key and index shape, and tuple-aware snapshot-reference columns without regressing ordinary PIT tables.
- Support rebuild, targeted parent maintenance, and PIT-backed as-of reads for multi-active tuple history while preserving the current explicit caller-owned maintenance workflow.
- Add deterministic validation, typed projection exposure, explain and diagnostic updates, public API snapshot changes, and automated coverage for supported and rejected multi-active PIT shapes.

Scope Out
- Link-parent PITs.
- Multi-active PITs that would require more than one driving-key family, cross-product tuple expansion, or automatic reconciliation of incompatible driving-key sets.
- Automatic, scheduled, background, or `SaveChanges`-triggered PIT maintenance.
- Provider-specific PIT maintenance or PIT read optimization.
- New tuple-filter request parameters or broader artifact and governance changes beyond what existing PIT metadata can already express.

Open questions
- none

Follow-up questions
- Should a later ticket add explicit driving-key-tuple filters to `DataVaultPitAsOfReadRequest` for large parent fan-out cases, or is parent-only filtering sufficient beyond this bounded v1 baseline?
- If model-first governance needs explicit multi-active PIT examples or artifact-level tuple-shape diagnostics beyond satellite-driven inference, should that be handled in a separate documentation or schema ticket?
- If teams later need multi-active PITs spanning incompatible driving-key families or cross-product tuple semantics, that should be handled in a separate contract ticket rather than broadening this story.

Risks
- Supporting multi-active PITs is not just maintenance work: current translation, read records, typed projection helpers, diagnostics, and published guidance all assume at most one visible PIT row per parent hash key.
- The live ticket graph still contains an incoming `blocks` relation from done story `06F5Q90KC6JGQPSP285XQYSPK8`; because no relation cleanup was applied in this run, automation that trusts raw relation state may still treat it as a blocker.
- Tuple-aware PIT maintenance and read paths will increase row counts and in-memory grouping pressure for parents with high driving-key fan-out until a separate optimization ticket changes the current provider-neutral approach.

Split recommendations
- No additional split is recommended if this story is bounded to one shared canonical driving-key set across referenced multi-active satellites and keeps tuple filters, model-first follow-ons, and provider-specific optimization out of scope.
- If the release also needs explicit tuple-filter read requests or broader artifact-schema changes, split those into follow-up tickets instead of enlarging this story.

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