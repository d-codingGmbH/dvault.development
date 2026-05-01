[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7F6WNWSJJV14EXTPSFDRG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7F6WNWSJJV14EXTPSFDRG`.
- Optimistic claim succeeded (`expectedRevision=06EY3EQDRWZWKW44YSQRCM0HSR`, `currentRevision=06EY3EV383FCDK7AEDQG5H9EAG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc' from source 'c33836b9eaa6f590e3940edf7646b90c891fb077'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7F6WNWSJJV14EXTPSFDRG-epic-entity-framework-integration-and-persistenc` as `376a9df659e8`.

Open questions / Risiken
- Blocking finding: Because the parent is explicitly described as having no remaining developer-owned or tester-owned slice, sending it forward while those dev/test-blocking labels remain would hand off a closure/tracking epic with contradictory ticket-level routing metadata.
- Required PO action: Update the parent ticket's status/labels to an explicit post-review closure-safe state instead of leaving a no-implementation epic in a generic active backlog state with conflicting runtime labels.
- Required PO action: Re-handoff only after the live ticket fields, not just the description block, consistently show that no developer or tester execution remains on the parent epic.
- Risky assumption: Automation and humans will treat the contract block as authoritative even while the live ticket fields still advertise `blocked/dev` and `blocked/test`.
- Risky assumption: The legacy draft below the contract will not be mistaken for remaining executable parent scope.
- Risky assumption: A closure-only epic can safely move forward without first making its ticket-level routing metadata match its no-dev/no-test contract.
- Split recommendation: Do not reopen `06EXB7F6WNWSJJV14EXTPSFDRG` for first-class Postgres runtime/provider support, SaveChanges interception, or other convenience work; create separate follow-up tickets or a separate follow-up epic for those slices.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8818`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `638dcda9b7eb48719b0f6d742e3a8c56`
- completed-at-utc: `<redacted>-01T04:16:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7F6WNWSJJV14EXTPSFDRG/runs/20260501T041601053Z-638dcda9b7eb48719b0f6d742e3a8c56.json`