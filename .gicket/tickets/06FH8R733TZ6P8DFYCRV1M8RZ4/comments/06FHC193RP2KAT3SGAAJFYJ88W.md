[gicket-bot] Run report (outcome: po-critic-closure-only-ticket-closed)

Summary
- Closed closure-only ticket '06FH8R733TZ6P8DFYCRV1M8RZ4' because PO-critic verified that the ticket is already satisfied and no developer or tester execution remains.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8R733TZ6P8DFYCRV1M8RZ4`.
- Optimistic claim succeeded (`expectedRevision=06FHBZ9AKPX7SMN0TQAMHS33VR`, `currentRevision=06FHBZSQKNPRBG08JKB3X6GB64`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- PO-critic closure audit approved the closure-only ticket.
- Planned status transition path: 'todo' -> 'doing' -> 'done'.
- Planned label cleanup: removed 'bot/lease:hp-ai-<redacted>.1', 'critic-needed'.
- Planned assignee cleanup for closed closure-only ticket.
- Queued automatic integration via 'squash' after the closure writeback.
- Workflow transition side-effect diagnostic `BOT-SHARED-WORKFLOW-UNAVAILABLE` from `.gicket/workflow.json`: gicket-cli/gicket-mcp transition_ticket could not provide shared transition side effects for ticket '06FH8R733TZ6P8DFYCRV1M8RZ4' through 'doing -> done': usage: gicket ti...
- Workflow transition side-effect diagnostic `BOT-POLICY-COMPATIBILITY-SIDE-EFFECT` from `.gicket-bot/policy.json`: Queued Bot policy compatibility label or assignee behavior because no shared workflow side-effect rule was reported by the ticket application adapter. Remediation:...
- Materialized deferred runtime handoff comment before transactional git writeback.
- Materialized deferred runtime handoff field mutations before transactional git writeback.
- 3 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Role-specific execution pipeline is not part of this dispatch step.
- Follow-up role workflow should confirm whether additional ticket updates are required.

Next steps
- Automatic integration will run after the closure writeback is finalized.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9184`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `ddea842b02fd483f8048fef6797956cd`
- completed-at-utc: `<redacted>-30T00:54:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8R733TZ6P8DFYCRV1M8RZ4/runs/20260630T005421096Z-ddea842b02fd483f8048fef6797956cd.json`