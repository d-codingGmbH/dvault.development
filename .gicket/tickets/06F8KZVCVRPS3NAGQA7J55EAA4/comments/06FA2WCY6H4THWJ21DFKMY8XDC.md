[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZVCVRPS3NAGQA7J55EAA4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZVCVRPS3NAGQA7J55EAA4`.
- Optimistic claim succeeded (`expectedRevision=06F9XD3P4741N0CRNV9CQZHB0G`, `currentRevision=06FA2RMJDTTDJWQ1C4071RSD3C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZVCVRPS3NAGQA7J55EAA4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZVCVRPS3NAGQA7J55EAA4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari' from source '6a6a701f0ad1eff721fcc723654f58389f842667'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari` as `05298fec4071`.

Open questions / Risiken
- The checked-in root benchmark triplet still shows optional external-provider rows as skipped when connection strings are unset, so this refinement must not be misread as completed live all-provider evidence.
- If future tickets omit matched-input diagnostics or hide skipped optional-provider rows, they will undermine the comparability rules this ticket is supposed to lock down.
- If provider-specific artifact work substitutes provider-side hashing, changes request ordering, or suppresses caller transaction ownership, it will violate the parity boundary already documented in current contracts and tests.
- Because the landed prototype is SQL Server-specific, teams may overgeneralize its workload facts unless this ticket keeps the one-provider/one-workload rule explicit.
- Split recommendation: No new split is justified; the current evidence/prototype/documentation/all-provider-baseline separation is sufficient.
- Split recommendation: Create separate follow-up tickets instead of widening this task if the team wants deployable sidecar SQL payloads, runtime invocation helpers, provider-specific cleanup validators, or multi-workload/provider-matrix parity suites.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9448`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `5b8a927d0caa451aadc7431efc446d16`
- completed-at-utc: `<redacted>-07T09:37:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZVCVRPS3NAGQA7J55EAA4/runs/20260607T093718379Z-5b8a927d0caa451aadc7431efc446d16.json`