[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEJ7NANHCP64VR1SH3S3G8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEJ7NANHCP64VR1SH3S3G8`.
- Optimistic claim succeeded (`expectedRevision=06F0QH3WMH0XFFKQ0WR6BH1VGW`, `currentRevision=06F1GD4VGMX4KE220Q1ZCZYWS4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEJ7NANHCP64VR1SH3S3G8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEJ7NANHCP64VR1SH3S3G8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEJ7NANHCP64VR1SH3S3G8-task-add-provider-specific-read-strategy-selecti' from source '506423e389ba0714cea651f87eba4035ec503b15'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The dispatch layer touches shared read-service behavior, so regression coverage needs to prove fallback output remains stable for existing latest/as-of read scenarios.
- Diagnostics wording may drift from the save-strategy explain vocabulary unless implementation reuses the established conventions.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `25525`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0953`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `317bb0215a9148d8bb3a2991a77cc126`
- completed-at-utc: `<redacted>-11T18:05:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEJ7NANHCP64VR1SH3S3G8/runs/20260511T180547924Z-317bb0215a9148d8bb3a2991a77cc126.json`