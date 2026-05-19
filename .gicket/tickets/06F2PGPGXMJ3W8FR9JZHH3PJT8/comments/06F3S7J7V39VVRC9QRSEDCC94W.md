[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F2PGPGXMJ3W8FR9JZHH3PJT8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGPGXMJ3W8FR9JZHH3PJT8`.
- Optimistic claim succeeded (`expectedRevision=06F3S4FJDRVWF64XRQ00Y9MST4`, `currentRevision=06F3S6C5PDT58R2C56VB627FPC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service' from source '1f5936b7bca685a958aff861bc27b8ea0c82cc2b'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service` as `405e11b476b2`.

Open questions / Risiken
- Blocking finding: The contract does not define the authoritative TraversalDepth rule when multiple hierarchy paths produce the same ancestor/descendant pair. That is a real gap because the shipped hierarchy bridge shape stores only one row per ancestor/descendant pair, so deve...
- Blocking finding: The contract also does not state what incremental maintenance must do when newly ingested recursive-link data creates a shorter path for an ancestor/descendant pair that already exists in the bridge table. Without that rule, the idempotence and maximumDepth s...
- Required PO action: Add an explicit hierarchy-depth rule for duplicate ancestor/descendant pairs created by multiple paths, including which TraversalDepth value is persisted.
- Required PO action: Clarify incremental hierarchy behavior when later source-link ingestion creates a shorter alternate path for an already materialized ancestor/descendant pair: update existing depth, reject as unsupported, or preserve the original depth by contract.
- Required PO action: Promote that rule into acceptance criteria and test expectations so downstream query-API and documentation tickets inherit one deterministic hierarchy semantics baseline.
- Risky assumption: Assuming developers will infer a shortest-path rule from the current maximumDepth read behavior, even though no contract text states that rule.
- Risky assumption: Assuming recursive source-link data is acyclic or otherwise harmless without an explicit contract for cycles.
- Risky assumption: Assuming the v0.15.0 release-note delta can simply be created during implementation; docs/releases currently contains release-note files through v0.14.0 only.
- Split recommendation: No split recommended once the hierarchy TraversalDepth rule is clarified; the existing sibling tickets already isolate PIT maintenance, query API follow-up, provider optimization, and broader documentation work.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9117`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `76249978a03f48e28c106eab8fb16ba1`
- completed-at-utc: `<redacted>-18T19:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGPGXMJ3W8FR9JZHH3PJT8/runs/20260518T194426376Z-76249978a03f48e28c106eab8fb16ba1.json`