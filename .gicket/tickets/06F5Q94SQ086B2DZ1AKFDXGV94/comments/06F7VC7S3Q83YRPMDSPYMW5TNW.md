[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q94SQ086B2DZ1AKFDXGV94'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q94SQ086B2DZ1AKFDXGV94`.
- Optimistic claim succeeded (`expectedRevision=06F72ZYG548FHWYJ2WFZTWH6GW`, `currentRevision=06F7V8XH1HZNC6SQYWNK7D6K8W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q94SQ086B2DZ1AKFDXGV94': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q94SQ086B2DZ1AKFDXGV94': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q94SQ086B2DZ1AKFDXGV94-task-update-v0-23-0-tracing-and-performance-guid' from source 'e67eee4c083e8f1702df64e1e3312752f5c425cc'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- README.md and docs/production-adoption-checklist.md still reference v0.22.0 as the current baseline, so partial edits could leave public-versioning guidance inconsistent if the update is not completed across all touched docs.
- Activity tracing docs must reuse the closed contract vocabulary exactly where names matter; paraphrased span names, redaction lists, or telemetry relationships would create public inconsistencies with the landed contract.
- Performance claims are evidence-bound to the root benchmark artifacts and current run context; optional external-provider rows are skipped, so broad provider-specific performance wording would overstate the checked-in evidence.
- Because no dedicated link checker is visible, broken anchors or stale cross-links remain a manual-review risk unless each touched link is checked during the docs pass.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9229`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d9b78772ebff4546bd6e4f0f01b1926d`
- completed-at-utc: `<redacted>-31T11:00:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q94SQ086B2DZ1AKFDXGV94/runs/20260531T110010649Z-d9b78772ebff4546bd6e4f0f01b1926d.json`