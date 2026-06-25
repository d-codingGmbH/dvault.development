[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43PCN26C70DXX326B9VYA4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43PCN26C70DXX326B9VYA4`.
- Optimistic claim succeeded (`expectedRevision=06FF44PQR817MB5AZE8KSQCP78`, `currentRevision=06FFVVR3BEZ3F2373X2468XFMC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43PCN26C70DXX326B9VYA4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43PCN26C70DXX326B9VYA4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43PCN26C70DXX326B9VYA4-task-document-provider-native-encryption-caveats' from source 'c79f7aaefeb8681b776b1a11450eed33a5c6be4d'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43PCN26C70DXX326B9VYA4-task-document-provider-native-encryption-caveats` as `53860bc9e20e`.

Open questions / Risiken
- If the docs enumerate provider-native features too aggressively, readers may misread examples as tested support or runtime dispatch promises; wording needs to stay guidance-only.
- Loose MySQL or MariaDB wording could overstate supported-provider scope, because the current finite baseline exposes a MySQL profile rather than a separate MariaDB capability contract.
- Split recommendation: No split recommended; the repository already fixes a bounded baseline, and the remaining work is documentation alignment around that existing contract.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8398`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `5854277b6e474dd58fdb618643ca7cd0`
- completed-at-utc: `<redacted>-25T08:46:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43PCN26C70DXX326B9VYA4/runs/20260625T084603355Z-5854277b6e474dd58fdb618643ca7cd0.json`