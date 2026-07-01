[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8RP1SBVZ7K3K48ERGZSMQC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RP1SBVZ7K3K48ERGZSMQC`.
- Optimistic claim succeeded (`expectedRevision=06FHQNAXFKBCC7199FGJEN7G8C`, `currentRevision=06FHWWVM6G3BEEQN3HDT6R4G0C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8RP1SBVZ7K3K48ERGZSMQC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8RP1SBVZ7K3K48ERGZSMQC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8RP1SBVZ7K3K48ERGZSMQC-task-update-v0-51-0-release-notes-and-package-va' from source '45ec919b2f569e360f0976f4eaff0851b8d6c892'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FH8RP1SBVZ7K3K48ERGZSMQC-task-update-v0-51-0-release-notes-and-package-va` as `6b57a40ae147`.

Open questions / Risiken
- Because three live incoming blocks relations currently target this ticket, workflow comments alone are not enough to prove dependency clearance; stale relations can keep the ticket artificially blocked.
- Partial version bumps across docs, scripts, verifier logic, and verifier tests can leave the release baseline internally inconsistent even though the underlying analyzer implementation is already settled.
- Any mixed-line guidance or consumer-facing 0.51.0 package claim would publish incorrect installation and approval instructions.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `35968`
- effective-cache-ratio: `0.2276`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `69880bb0a6874d7bae40e065b5489c3c`
- completed-at-utc: `<redacted>-01T16:20:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RP1SBVZ7K3K48ERGZSMQC/runs/20260701T162025739Z-69880bb0a6874d7bae40e065b5489c3c.json`