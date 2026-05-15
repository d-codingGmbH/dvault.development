[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XQ1VWEX0WPAXE78FHSWJ8G'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ1VWEX0WPAXE78FHSWJ8G`.
- Optimistic claim succeeded (`expectedRevision=06F1XTQBSXCQ2VFXYJ908B8X90`, `currentRevision=06F2HQR9FTWNRJ69HJA8A7M7R4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XQ1VWEX0WPAXE78FHSWJ8G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XQ1VWEX0WPAXE78FHSWJ8G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XQ1VWEX0WPAXE78FHSWJ8G-story-add-testcontainers-integration-helpers-and' from source '6ea882105abbf315721fab07c6a64c951474a74a'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The story title can invite full provider-matrix scope; keep this pass bounded to the done PostgreSQL first-provider fixture and reusable pattern.
- Podman and Docker networking differ across hosts, so the sample must keep hostname and port overrides visible.
- Conditional provider restore can fail if the documented MSBuild marker property is omitted during opt-in test runs.
- Hardcoded ports can collide with local services; documentation should keep alternate host-port mapping explicit.
- Split recommendation: No new split is recommended now. The existing done child 06F1XQ25KK4VY4MYJSDG9V4BZM covers the first provider fixture sample.
- Split recommendation: If the product later requires a full external-provider fixture matrix, split MySQL, SQL Server, and Oracle into separate provider-specific tickets because images, licensing, authentication, and privilege setup differ.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9053`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d7b68538e59f4cc9b50108c171662e05`
- completed-at-utc: `<redacted>-14T23:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ1VWEX0WPAXE78FHSWJ8G/runs/20260514T235058713Z-d7b68538e59f4cc9b50108c171662e05.json`