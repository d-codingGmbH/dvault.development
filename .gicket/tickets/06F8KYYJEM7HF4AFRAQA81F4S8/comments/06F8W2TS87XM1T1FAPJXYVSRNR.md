[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KYYJEM7HF4AFRAQA81F4S8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KYYJEM7HF4AFRAQA81F4S8`.
- Optimistic claim succeeded (`expectedRevision=06F8KZWNFGHPRF1TJ4JNQXVB64`, `currentRevision=06F8W18KST4QYYRSAANMPP7P70`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KYYJEM7HF4AFRAQA81F4S8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KYYJEM7HF4AFRAQA81F4S8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KYYJEM7HF4AFRAQA81F4S8-epic-ef-core-lifecycle-analyzer-guardrails' from source '4ebdb13070dee94461f632c88aba09af02ba7911'.
- Interactive PO tool loop fell back to legacy planning after MODEL-TOOL-INVOCATION-RESULT-TOOL-CALL-ARGUMENTS-JSON-INVALID.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Because the analyzer intentionally skips ambiguous and indirect code paths, some real lifecycle misuse can remain undiagnosed until a future ticket broadens the contract.
- The safe pooled and compiled-model baselines still depend on consumer-owned model-cache-key discipline when model shape varies.
- This release note is a documentation baseline only; final package publication still depends on a separate approval record outside this ticket.
- Split recommendation: If delivery breadth re-expands, keep this epic limited to `DMV1912` through `DMV1914` plus bounded docs/tests, and move pooled-factory, helper-expansion, cross-assembly, or runtime-guard ideas into separate follow-on tickets.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8373`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d22c8e87cfbd4292ac1aa5b65058ce22`
- completed-at-utc: `<redacted>-03T15:12:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KYYJEM7HF4AFRAQA81F4S8/runs/20260603T151248955Z-d22c8e87cfbd4292ac1aa5b65058ce22.json`