[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q91DR1555RSBQT7KDST684'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q91DR1555RSBQT7KDST684`.
- Optimistic claim succeeded (`expectedRevision=06F5Q98Z609QCA6PQ9BGPTAYBG`, `currentRevision=06F6NQ789QFN2YBCZHC1SXP7D4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q91DR1555RSBQT7KDST684': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q91DR1555RSBQT7KDST684': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q91DR1555RSBQT7KDST684-story-add-pit-and-bridge-diagnostics-and-benchma' from source 'a2615a58736e863c0966de652f78753194fa3f2d'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The current public diagnostics surface is read-oriented, so expanding this ticket into a new maintenance-diagnostics API would materially enlarge scope and compatibility risk.
- The current root benchmark matrix only covers ordinary PIT and bridge read scenarios; new rows or bundles must make fallback and strategy status explicit or the evidence could be misread as provider-specific optimization support.
- Broader README and release-note completeness work is intentionally downstream, so weak handoff between this evidence ticket and 06F5Q91M0PM17RP43ZQRPBDXP0 could leave published guidance behind the verified behavior.
- Split recommendation: No additional split is recommended; the remaining work is one cohesive evidence pass across the existing read-diagnostics surface and the established benchmark artifact contract.
- Split recommendation: If stakeholders want a new public registry-backed PIT read API or a dedicated maintenance-diagnostics API, split that into a separate additive API ticket instead of enlarging this evidence story.
- Split recommendation: If the broader README, architecture, or release-note completeness pass grows beyond narrowly necessary benchmark-surface wording, keep it on the existing downstream documentation task 06F5Q91M0PM17RP43ZQRPBDXP0.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9550`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `0ef26c4eafb243c39508f8490335d303`
- completed-at-utc: `<redacted>-27T19:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q91DR1555RSBQT7KDST684/runs/20260527T192843767Z-0ef26c4eafb243c39508f8490335d303.json`