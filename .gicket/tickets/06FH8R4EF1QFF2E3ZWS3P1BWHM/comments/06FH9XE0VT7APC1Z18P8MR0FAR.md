[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8R4EF1QFF2E3ZWS3P1BWHM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8R4EF1QFF2E3ZWS3P1BWHM`.
- Optimistic claim succeeded (`expectedRevision=06FH8SBR3EHAXFYJRH6RE8JPXC`, `currentRevision=06FH9V6378QPKA8YBW9W3QQ7ZC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8R4EF1QFF2E3ZWS3P1BWHM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8R4EF1QFF2E3ZWS3P1BWHM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8R4EF1QFF2E3ZWS3P1BWHM-task-add-net-8-sdk-analyzer-smoke-ci-and-package' from source '6363325b61e225ece55818a2cdaa3afae96111f0'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FH8R4EF1QFF2E3ZWS3P1BWHM-task-add-net-8-sdk-analyzer-smoke-ci-and-package` as `16f5b331a7ec`.

Open questions / Risiken
- Retargeting the analyzer package may surface hidden `net10.0` API usage and may require explicit companion assemblies for Workspaces or `System.Composition`; if those are missed, the package can restore yet still fail to load on the claimed host.
- The current verifier and docs are hard-coded to the `.NET 10 SDK`-only story, so partial updates across README, analyzer README, and packaged README checks could leave the repository internally inconsistent.
- A proof that still builds from project references instead of packed packages would give a false positive and would not actually protect the advertised pure `.NET 8 SDK` consumer story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9359`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `4d3c78337bd149ffbf4314d7c78b10a9`
- completed-at-utc: `<redacted>-29T19:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8R4EF1QFF2E3ZWS3P1BWHM/runs/20260629T195757839Z-4d3c78337bd149ffbf4314d7c78b10a9.json`