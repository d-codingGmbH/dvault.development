[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FGX5JXRVY9FXDW4D8242XSB4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX5JXRVY9FXDW4D8242XSB4`.
- Optimistic claim succeeded (`expectedRevision=06FGX6NGSKGYGQY8073FF3S5ZR`, `currentRevision=06FGZ2N1EDYVW1JJJBX2RPMV04`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FGX5JXRVY9FXDW4D8242XSB4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FGX5JXRVY9FXDW4D8242XSB4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FGX5JXRVY9FXDW4D8242XSB4-task-add-analyzer-package-verifier-and-sdk-host' from source '587ebcb1caf161f2305e8b7ae26c81c58c84d59f'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FGX5JXRVY9FXDW4D8242XSB4-task-add-analyzer-package-verifier-and-sdk-host` as `0772bb275204`.

Open questions / Risiken
- A deterministic negative pure `.NET 8 SDK` host lane may remain outside the current validation baseline, so unsupported-host proof may rely on verifier/documentation evidence instead of an executed failure test.
- The analyzer project still depends on SDK-local Roslyn/Workspaces/composition assemblies, which keeps future host-support expansion higher risk until those dependencies are normalized.
- Split recommendation: No split is needed for the current bounded verifier/smoke/documentation-alignment work.
- Split recommendation: If pure `.NET 8 SDK` analyzer-host support is later required, split it into one implementation ticket for retargeting or package-shape changes plus dependency normalization, then one validation/documentation/release-surface ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `53144`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0458`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6c3e33d2a9dc4aea9fb1f7d437d3b974`
- completed-at-utc: `<redacted>-28T18:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX5JXRVY9FXDW4D8242XSB4/runs/20260628T185204804Z-6c3e33d2a9dc4aea9fb1f7d437d3b974.json`