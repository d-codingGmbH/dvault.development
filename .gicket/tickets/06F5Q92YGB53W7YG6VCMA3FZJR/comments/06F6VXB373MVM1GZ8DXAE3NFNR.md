[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q92YGB53W7YG6VCMA3FZJR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92YGB53W7YG6VCMA3FZJR`.
- Optimistic claim succeeded (`expectedRevision=06F6VSHTPWNCCF8K2CVJ6J2B04`, `currentRevision=06F6VSV8WNYRWTAHA7CQ04FP74`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q92YGB53W7YG6VCMA3FZJR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q92YGB53W7YG6VCMA3FZJR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q92YGB53W7YG6VCMA3FZJR-story-add-analyzers-and-code-fixes-for-typed-rea' from source '92430cf1e2b64622661589b4937a8df07f817372'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- If unsupported PIT, bridge, dynamic, model-first, or helper-skipped cases continue to drop out silently, consumers and the docs task cannot distinguish not implemented from misconfigured.
- Expanding this follow-up into runtime helper generation or speculative code fixes would reopen already-completed implementation slices.
- Documentation rollup remains blocked on this residual diagnostic surface until the current story lands.
- Split recommendation: No further split is recommended. The residual diagnostic-only follow-up is smaller than the completed generator slices and separate from the documentation rollup.
- Split recommendation: Keep the documentation rollup on 06F5Q93H60W6X8FJ88PWTR6NG4 rather than reabsorbing docs scope into this story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `52067`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0467`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `30749d24958f4fbdbe512e80109a9c3e`
- completed-at-utc: `<redacted>-28T09:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92YGB53W7YG6VCMA3FZJR/runs/20260528T094058803Z-30749d24958f4fbdbe512e80109a9c3e.json`