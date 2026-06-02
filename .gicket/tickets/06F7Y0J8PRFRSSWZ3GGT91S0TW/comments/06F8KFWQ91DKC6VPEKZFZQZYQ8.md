[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0J8PRFRSSWZ3GGT91S0TW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0J8PRFRSSWZ3GGT91S0TW`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0WMWCZG2Z8AZNVPNXD79W`, `currentRevision=06F8KDSDQPQFEWMHWREH2YYQMG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0J8PRFRSSWZ3GGT91S0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0J8PRFRSSWZ3GGT91S0TW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0J8PRFRSSWZ3GGT91S0TW-epic-provider-performance-and-schema-guardrails' from source '5b5d341a429ff515254964761730603d3f94a627'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The benchmark artifacts are observational, not SLA commitments; if later closure evidence drops the documented run-context caveats, adopters may overread the timing numbers.
- The live relation graph still shows two incoming blockers, so epic closure could stall operationally even though no additional PO clarification is required.
- Split recommendation: No further split recommended. The epic already has seven persisted child tickets and should remain a coordination/closure parent.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `87552`
- effective-cache-ratio: `0.7253`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b7a6b9261695445fb4f32a7182ceed22`
- completed-at-utc: `<redacted>-02T19:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0J8PRFRSSWZ3GGT91S0TW/runs/20260602T191135234Z-b7a6b9261695445fb4f32a7182ceed22.json`