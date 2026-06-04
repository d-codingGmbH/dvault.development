[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZNNS76TD9Z7ESB173FZ68'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZNNS76TD9Z7ESB173FZ68`.
- Optimistic claim succeeded (`expectedRevision=06F8M03FTX5XRA9W344W24PDPM`, `currentRevision=06F97VSEYKP5P46EDVW3X5QC4G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZNNS76TD9Z7ESB173FZ68': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZNNS76TD9Z7ESB173FZ68': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZNNS76TD9Z7ESB173FZ68-task-update-v0-29-0-provider-schema-guardrail-do' from source '4c1288cdb22c530b8fe9507fd11053bbcbc1b998'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZNNS76TD9Z7ESB173FZ68-task-update-v0-29-0-provider-schema-guardrail-do` as `7eb28f54b4c3`.

Open questions / Risiken
- Live relation and attachment state could not be re-verified through gicket because the provided ticket read tools were trust-policy blocked, so hidden coordination context may still exist outside the prompt snapshot.
- If provider capability names or guardrail diagnostic terms change before implementation lands, the documentation could drift from the current planning contract.
- The main content risk is overclaiming provider support or automatic remediation; reviewer attention should stay on keeping the guardrail guidance bounded to the existing supported profiles and review workflow.
- Split recommendation: No split recommended; current evidence keeps this as one bounded documentation ticket spanning the missing v0.29.0 release notes and coordinated public-doc updates.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `34430`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0706`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `aeddf42a198c49e1a25669390a26b155`
- completed-at-utc: `<redacted>-04T18:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZNNS76TD9Z7ESB173FZ68/runs/20260604T184645334Z-aeddf42a198c49e1a25669390a26b155.json`