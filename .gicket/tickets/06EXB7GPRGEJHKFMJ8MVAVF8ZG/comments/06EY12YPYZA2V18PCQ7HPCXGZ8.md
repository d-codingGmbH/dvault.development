[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7GPRGEJHKFMJ8MVAVF8ZG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7GPRGEJHKFMJ8MVAVF8ZG`.
- Optimistic claim succeeded (`expectedRevision=06EY11D9PM217D5D8F5B39CXSR`, `currentRevision=06EY11HJ7WYYGKNQ1WHGM73AZW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7GPRGEJHKFMJ8MVAVF8ZG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7GPRGEJHKFMJ8MVAVF8ZG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7GPRGEJHKFMJ8MVAVF8ZG-task-add-schema-and-migration-snapshot-tests' from source '4af8fb00d127afa95071f43a18ba1a9f99fdb439'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Snapshot coverage that stores raw provider DDL without enough canonicalization may become brittle across EF Core or SQLite version changes.
- The ticket title mentions migrations, so an implementer could accidentally expand scope into deferred provider-specific migration infrastructure unless the refined scope is followed.
- Replacing all structural assertions with a single opaque blob snapshot could make failures harder to diagnose if the canonical output is not kept focused and readable.
- Split recommendation: If migration-specific output requires Microsoft.EntityFrameworkCore.Design, committed model snapshots, or design-time services, split that work into a separate ticket after migration behavior is intentionally scoped.
- Split recommendation: If additional database providers need equivalent coverage later, create provider-specific snapshot tickets rather than widening this ticket beyond the current SQLite baseline.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9281`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `004166cd3d544397a1c461522a47ae6e`
- completed-at-utc: `<redacted>-30T22:39:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7GPRGEJHKFMJ8MVAVF8ZG/runs/20260430T223913541Z-004166cd3d544397a1c461522a47ae6e.json`