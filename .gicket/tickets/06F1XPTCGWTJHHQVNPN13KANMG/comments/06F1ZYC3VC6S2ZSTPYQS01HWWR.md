[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPTCGWTJHHQVNPN13KANMG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPTCGWTJHHQVNPN13KANMG`.
- Optimistic claim succeeded (`expectedRevision=06F1XTP3H7N62M188GGDTF09NM`, `currentRevision=06F1ZT7JNZQKGG3J57G1Q4HNY0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPTCGWTJHHQVNPN13KANMG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPTCGWTJHHQVNPN13KANMG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPTCGWTJHHQVNPN13KANMG-story-add-ef-migration-guardrails-for-data-vault' from source 'a50c3b10705f181e27def46f470cf19a9a6a2e79'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- EF Core providers can express equivalent schema changes through different MigrationOperation sequences, so guardrail coverage must stay high-confidence without creating noisy false positives or false negatives.
- PIT and bridge baselines are narrower and more opt-in than hubs/links/satellites; incorrect mapping of snapshot-reference columns, TraversalDepth, or bridge traversal indexes will create misleading findings.
- The current public diagnostics issue shape does not obviously carry remediation text, so exposing guidance for automation may require a careful API extension or adjacent report surface.
- The repository still contains older point-in-time terminology, so docs/examples must clearly distinguish DataVaultPitMetadata from legacy DataVaultPointInTimeMetadata to avoid adoption confusion.
- Split recommendation: Keep this story limited to the guardrail API/report contract, diagnostic taxonomy, PIT/bridge baseline coverage, and one minimal pre-apply usage snippet.
- Split recommendation: Route broader README/example/checklist work to existing docs story 06F1XQ2MB5Y9JW25W2CWVZZ9G4 and checklist task 06F1XQ3006JYSJT5EHT05GV1HG instead of growing this ticket.
- Split recommendation: If a standalone CLI package is still desired after the reusable API exists, split it as separate adoption tooling work rather than expanding this story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9395`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `085dc4d3e46342e0bd6c4cdbe55f9d6f`
- completed-at-utc: `<redacted>-13T06:15:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPTCGWTJHHQVNPN13KANMG/runs/20260513T061505995Z-085dc4d3e46342e0bd6c4cdbe55f9d6f.json`