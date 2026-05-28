[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q93H60W6X8FJ88PWTR6NG4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q93H60W6X8FJ88PWTR6NG4`.
- Optimistic claim succeeded (`expectedRevision=06F6WFMC6A2XDGZYWYZASBTG6G`, `currentRevision=06F6WFXR3AKKZHS4PRJDWWCXKG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q93H60W6X8FJ88PWTR6NG4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q93H60W6X8FJ88PWTR6NG4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan' from source '236b73f1c76b214b81ffad70e9638fd0fcf695f1'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q93H60W6X8FJ88PWTR6NG4-task-update-v0-22-0-typed-read-and-hash-governan` as `43e5b7168f18`.

Open questions / Risiken
- The main PO risk is reintroducing unsupported evidence claims, especially analyzer-package public API snapshots or dedicated generator approval snapshots that are not present in the branch.
- Docs can become misleading if they imply repo-checked dvault.model.v1 or dvault.support-bundle.v1 baseline files instead of the visible consumer-owned artifact workflow.
- The generator boundary can be overstated if PIT or bridge helper emission, provider-specific SQL generation, or dynamic-request compilation is described as current behavior.
- If the new v0.22.0 note cites commands or evidence files outside the current repo surfaces, the docs will drift from the actual validation and approval baseline.
- Split recommendation: If the team wants new analyzer or generator snapshot infrastructure, split that into a separate quality or evidence ticket.
- Split recommendation: If release documentation needs PIT or bridge typed-helper coverage later, split that follow-up to the ticket that ships the actual implementation.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8639`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `47101bf7e12d492db2998a2d79741fd2`
- completed-at-utc: `<redacted>-28T11:10:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q93H60W6X8FJ88PWTR6NG4/runs/20260528T111027150Z-47101bf7e12d492db2998a2d79741fd2.json`