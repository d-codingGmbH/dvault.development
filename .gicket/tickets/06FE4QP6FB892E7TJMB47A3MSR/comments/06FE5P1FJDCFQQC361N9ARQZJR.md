[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4QP6FB892E7TJMB47A3MSR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QP6FB892E7TJMB47A3MSR`.
- Optimistic claim succeeded (`expectedRevision=06FE4QSXT6ETDZ78V4NKYJRY4C`, `currentRevision=06FE5K3D19WFB8VR2HCYNRJG2W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4QP6FB892E7TJMB47A3MSR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4QP6FB892E7TJMB47A3MSR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4QP6FB892E7TJMB47A3MSR-task-normalize-provider-benchmark-lanes-for-late' from source '00821be5ed2783e7a26e761c1e58b83276dcbf29'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FE4QP6FB892E7TJMB47A3MSR-task-normalize-provider-benchmark-lanes-for-late` as `9ae4b13793c6`.

Open questions / Risiken
- Because most optional-provider latest-satellite rows are intentionally skipped in the root triplet when local connection strings are unset, downstream work could overstate row visibility as measured timing unless the promotion boundary stays explicit.
- MySQL and Oracle already have provider-read parity context for PIT and bridge, but their latest-satellite timing still lacks completed provider-configured evidence; mixing those categories would blur the contract.
- DB2 remains narrower than the other providers; treating its latest-satellite row as comparable timing instead of diagnostics or skip guidance would widen scope beyond repository evidence.
- If benchmark code, tests, and matrix docs drift on placeholder token names or skip semantics, provider-specific follow-up tickets will start from inconsistent evidence inputs.
- Split recommendation: No additional split is needed. Shared latest-satellite lane normalization stays in this ticket, while provider-specific follow-up work is already materialized in 06FE4QPR8TF8R6PXNM3RMXN8JG, 06FE4QQ0YTHD7624MGVPKKK1C0, 06FE4QQ9VF7B74E60CXEHSS5XW, and 06FE4...
- Split recommendation: Keep DB2 promotion broadening separate from this task; sibling ticket 06FE4QPEZW97YR6YT7MQD1MXTG already carries the broader DB2 promotion-guardrail follow-up.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9533`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `15ed018760674ff68a1fd5591b4c268b`
- completed-at-utc: `<redacted>-20T02:24:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QP6FB892E7TJMB47A3MSR/runs/20260620T022439307Z-15ed018760674ff68a1fd5591b4c268b.json`