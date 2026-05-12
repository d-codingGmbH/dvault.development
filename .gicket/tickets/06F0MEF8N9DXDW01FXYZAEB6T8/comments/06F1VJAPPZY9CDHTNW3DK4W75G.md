[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEF8N9DXDW01FXYZAEB6T8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEF8N9DXDW01FXYZAEB6T8`.
- Optimistic claim succeeded (`expectedRevision=06F0QH30B7VVAA6TSCNPQBZ1C0`, `currentRevision=06F1VHERNS7GSPF89F5QWREG54`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEF8N9DXDW01FXYZAEB6T8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEF8N9DXDW01FXYZAEB6T8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEF8N9DXDW01FXYZAEB6T8-story-add-model-export-and-drift-tooling' from source '6848218bcbd504765c77313020b044e41827dc06'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-LIMIT-EXCEEDED.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Rename detection may be limited when metadata lacks a stable identity that survives produced-name changes; report unmatched items as added and removed rather than guessing.
- Provider-specific EF metadata can vary by provider, so this story should keep v1 drift semantics grounded in DVault-owned provider-neutral annotations and documented logical metadata.
- PIT and bridge support depends on the current branch's available metadata surfaces; tests should pin the supported v1 shapes and report unsupported comparison gaps explicitly.
- Split recommendation: If implementation size grows, split into exporter implementation, drift report implementation, and documentation/examples as separate delivery slices while keeping this story's v1 contract unchanged.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `25717`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0946`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `c7e20ba4fe9a47a29125dde297e4b3de`
- completed-at-utc: `<redacted>-12T20:03:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEF8N9DXDW01FXYZAEB6T8/runs/20260512T200312940Z-c7e20ba4fe9a47a29125dde297e4b3de.json`