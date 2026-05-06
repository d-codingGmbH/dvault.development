[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EZ0NV7KG94MTMNXMGVRYVW9C'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NV7KG94MTMNXMGVRYVW9C`.
- Optimistic claim succeeded (`expectedRevision=06EZQCPMCHN2SFGRJ045T87FZG`, `currentRevision=06EZQD0RKFPPD7C9WN0F371VA8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m' from source '30fc7790cf934a0413dae3776802b7d584628313'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EZ0NV7KG94MTMNXMGVRYVW9C-task-generate-provider-neutral-bridge-ef-model-m` as `0686eaaa54da`.

Open questions / Risiken
- Blocking finding: This ticket delegates bridge-shape validity to sibling ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4, but that sibling is still needs-po and unrefined. Because the repo has no bridge metadata or public API surface today, developers do not yet have a stable input contract...
- Blocking finding: The sequencing dependency between metadata definition and mapping implementation is not persisted. With only parentOf relations in .gicket/relations, the current ticket set can be misread as independently actionable even though this ticket says the metadata s...
- Blocking finding: The handoff still lacks concrete worked examples for the supported many-to-many and hierarchy bridge shapes, especially the exact projected table, column, primary-key, index, and translator-failure expectations needed for deterministic implementation and tests.
- Required PO action: Refine ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 to a durable ready-for-dev contract first, including the authoritative bridge metadata shapes, validation ownership, and any required public API additions.
- Required PO action: Persist sequencing between the sibling tickets, for example by making 06EZ0NV0Y81AE1Z1Q3223TX2S4 block 06EZ0NV7KG94MTMNXMGVRYVW9C or by stating the same dependency unambiguously in the ticket state/comment trail.
- Required PO action: Add or reference concrete examples for the supported baseline many-to-many and hierarchy shapes, including expected generated columns, primary key and index layout, annotations, and which unsupported cases belong to translator-time failure versus metadata v...
- Risky assumption: Bridge support can be added with only minimal public API expansion even though the current public API snapshot and modeling types contain no bridge surface.
- Risky assumption: The existing shared-type, no-navigation translator posture is sufficient for both supported bridge shapes without leaking new EF relationship semantics.
- Risky assumption: The current load-timestamp contract is enough for the baseline hierarchy case and does not hide a new effectivity or window concept.
- Risky assumption: ParentOf-only ticket structure is enough for execution sequencing.
- Split recommendation: Do not create another child ticket yet; keep the current split.
- Split recommendation: Add an explicit dependency or sequencing relation from metadata ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 to mapping ticket 06EZ0NV7KG94MTMNXMGVRYVW9C.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8636`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `c917e91c77c6491aab114feb6a7f7edb`
- completed-at-utc: `<redacted>-06T05:19:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NV7KG94MTMNXMGVRYVW9C/runs/20260506T051923853Z-c917e91c77c6491aab114feb6a7f7edb.json`