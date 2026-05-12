[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEHSH6S31ZE4K0Q3EKR784'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEHSH6S31ZE4K0Q3EKR784`.
- Optimistic claim succeeded (`expectedRevision=06F0QH35AQKCK34FBJ0Q36MJFW`, `currentRevision=06F1VS9CZ45V1SH3YNRQ824S0C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEHSH6S31ZE4K0Q3EKR784': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEHSH6S31ZE4K0Q3EKR784': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEHSH6S31ZE4K0Q3EKR784-story-add-provider-aware-read-optimization-follo' from source 'f8dbdf2d4cd984046041ccc4bc11c9f6f293d8d7'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Benchmark timings are machine-specific, so review should focus on reproducible commands and comparable before/after context rather than absolute numbers alone.
- Non-SQLite providers may have deterministic skip rows on machines without local configuration, leaving fewer measured optimization candidates.
- Docs can overstate advanced read behavior if they imply PIT refresh, bridge maintenance, or provider-specific PIT/bridge optimization; keep wording tied to implemented provider-neutral behavior.
- Public additive hook and diagnostics surfaces require compatibility review through existing API approval practices.
- Split recommendation: No additional split is recommended. The existing split is already materialized and done through 06F0MEJ0NE80R7CNS982S3PKVR, 06F0MEJ7NANHCP64VR1SH3S3G8, 06F0MEJE5WC51MFQ3CWDRATCWC, and 06F0MEJPGG7JBFEXD693BHY07W.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `60842`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0400`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `45079029074e42ccb32ce9481c6e9866`
- completed-at-utc: `<redacted>-12T20:37:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEHSH6S31ZE4K0Q3EKR784/runs/20260512T203743140Z-45079029074e42ccb32ce9481c6e9866.json`