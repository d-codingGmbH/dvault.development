[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZHZ27SDTNCFNMFDQRVCKM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZHZ27SDTNCFNMFDQRVCKM`.
- Optimistic claim succeeded (`expectedRevision=06F8M00SBBR38SMW9HF4C0Y5B0`, `currentRevision=06F8WASPP7JGFFABX0SA27B2XC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZHZ27SDTNCFNMFDQRVCKM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZHZ27SDTNCFNMFDQRVCKM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZHZ27SDTNCFNMFDQRVCKM-story-define-provider-read-strategy-evidence-con' from source '4c689db000ac0c3fbb41dceabea952c686f9784c'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Until queued mutation `mutation-bd0022c7b2e86e2d` replays on `ticket/06F8KZHAB717MJJNAWWK7S0A5W-task-update-v0-27-0-analyzer-and-ef-lifecycle-do`, some views may still show the stale blocker temporarily.
- If future provider work adds new fallback causes, status values, or benchmark-schema fields without coordinated contract updates, diagnostics/support-bundle consumers and typed-helper expectations can drift.
- Non-SQLite benchmark proof depends on provider-specific environment availability, so evidence may lag implementation even when the contract is settled.
- Split recommendation: No immediate ticket split is required; this ticket remains a bounded contract-definition story.
- Split recommendation: If downstream implementation work is created later, split by provider package and keep benchmark/verifier evidence in separate follow-up tickets so runtime work does not blur the contract baseline.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `88238`
- cached-tokens: `63104`
- effective-cache-ratio: `0.7152`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1c1e08096321491cb81a9c10199adca1`
- completed-at-utc: `<redacted>-03T16:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZHZ27SDTNCFNMFDQRVCKM/runs/20260603T161158635Z-1c1e08096321491cb81a9c10199adca1.json`