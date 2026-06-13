[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSBWPN112S4CGP0239K0ZT8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSBWPN112S4CGP0239K0ZT8`.
- Optimistic claim succeeded (`expectedRevision=06FBSDAATPQ47KA7VASBZ7NWCG`, `currentRevision=06FC0231M0JK76WZTRGR8ACSYC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSBWPN112S4CGP0239K0ZT8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSBWPN112S4CGP0239K0ZT8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSBWPN112S4CGP0239K0ZT8-task-document-v0-37-dependency-and-analyzer-comp' from source 'a76bb7b4b4a23982274c2e4ddeb68d70d597433f'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- `README.md` currently labels `v0.36.0` as the current baseline and uses v0.36-specific section wording, so a partial update could leave competing current-baseline signals between README and the new v0.37 release record.
- Because the planning label is `v0.37.0` but current repo-visible consumer lines are still `8.36.0` / `10.36.0`, careless documentation could wrongly invent `8.37.0` / `10.37.0` or a consumer-facing `0.37.0` package version.
- If the v0.37 docs omit the explicit `.NET 10 SDK` analyzer build-host boundary, they will overstate compatibility beyond what the repository actually proves for net8-target consumers.
- The downstream release-checklist ticket `06FBSBWW414TE19KZT14CB7Y3R` remains blocked until this current-baseline documentation work lands.
- Split recommendation: No new split. Keep existing done tickets as prerequisite evidence and keep `06FBSBWW414TE19KZT14CB7Y3R` as the downstream checklist follow-up that consumes this ticket's finalized baseline.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9624`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `78905f4af5874936a9bfc20bae87f5f7`
- completed-at-utc: `<redacted>-13T08:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSBWPN112S4CGP0239K0ZT8/runs/20260613T082844898Z-78905f4af5874936a9bfc20bae87f5f7.json`