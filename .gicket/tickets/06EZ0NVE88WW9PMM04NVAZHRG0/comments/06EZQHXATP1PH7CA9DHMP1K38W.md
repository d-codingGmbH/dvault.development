[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EZ0NVE88WW9PMM04NVAZHRG0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NVE88WW9PMM04NVAZHRG0`.
- Optimistic claim succeeded (`expectedRevision=06EZQG939GRBHZVSHBGHCRPZV8`, `currentRevision=06EZQGK2NGEPCAJ9R16AW2MEJW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar' from source '6da0ded4758ee795b1952c9eeef86cc24bf6d1d5'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EZ0NVE88WW9PMM04NVAZHRG0-task-add-bridge-documentation-and-example-scenar` as `61e1d4bb328f`.

Open questions / Risiken
- Blocking finding: The ticket is a documentation child of parent story `06EZ0NTV4SVAKV98C418T8A3CC`, but that parent still carries `needs-po`; approving the child for dev would hand off work that depends on an upstream scope the repository still marks as not PO-ready.
- Blocking finding: The acceptance criteria require a minimal example aligned to generated bridge metadata/table shapes, but the current repo exposes no concrete bridge source/API/type contract and the governing decision record explicitly forbids inferring deferred-feature API n...
- Required PO action: Refine parent story `06EZ0NTV4SVAKV98C418T8A3CC` out of `needs-po`, or explicitly state on this child ticket that documentation work is blocked until that parent establishes the authoritative bridge surface.
- Required PO action: Update this ticket contract to point to the exact authoritative bridge artifact the docs must follow once available: the parent ticket contract, a concrete source path/type, or a follow-up relation.
- Required PO action: Add an explicit blocking relation, label, or equivalent ticket-level sequencing signal so this child does not route to dev before the bridge surface exists.
- Required PO action: Specify which single minimal example scenario is required: the many-to-many traversal case or the hierarchy-style traversal case.
- Risky assumption: Assuming the future bridge surface will reuse obvious hub/link naming without a direct source contract.
- Risky assumption: Assuming the docs task can start independently even though the parent bridge story is still `needs-po`.
- Risky assumption: Assuming the example can be authored now and only lightly adjusted later, despite the ticket's own risk note that the generated bridge shape may still change.
- Split recommendation: No split required.
- Split recommendation: Sequence this task behind parent story `06EZ0NTV4SVAKV98C418T8A3CC` with an explicit blocking dependency instead of treating it as immediately developer-ready.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8989`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `b0b3f0abd3fb44e291dd5c91399e2479`
- completed-at-utc: `<redacted>-06T05:34:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/runs/20260506T053419088Z-b0b3f0abd3fb44e291dd5c91399e2479.json`