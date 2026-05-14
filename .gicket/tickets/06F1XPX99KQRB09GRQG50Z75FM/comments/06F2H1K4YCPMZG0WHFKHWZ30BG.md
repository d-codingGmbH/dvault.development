[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F1XPX99KQRB09GRQG50Z75FM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPX99KQRB09GRQG50Z75FM`.
- Optimistic claim succeeded (`expectedRevision=06F1XTPMDRKQ82ZFHQBXCVYEB4`, `currentRevision=06F2H052T2600JD1VKVZC92B8M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F1XPX99KQRB09GRQG50Z75FM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F1XPX99KQRB09GRQG50Z75FM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F1XPX99KQRB09GRQG50Z75FM-epic-read-and-runtime-performance-ergonomics' from source 'f4c81c8859d205102956c25df61e5ceb75602a88'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F1XPX99KQRB09GRQG50Z75FM-epic-read-and-runtime-performance-ergonomics` as `d71efb237f8f`.

Open questions / Risiken
- Performance evidence can mislead if it covers only one provider or only happy-path data volumes; the epic should keep claims scoped to the measured scenarios.
- Interceptor convenience can blur the explicit-save guidance unless docs clearly preserve explicit `IDataVaultSaveService` as the default path.
- Provider bulk hooks may create inconsistent expectations across providers unless unsupported and fallback behavior is explicit and tested.
- Split recommendation: No additional split is recommended now; execution should continue through the four existing child tickets already linked by `parentOf`.
- Split recommendation: If compiled-query and compiled-model proof grows into provider-by-provider certification instead of one supported baseline with explicit exceptions, split that certification work into a separate follow-up rather than expanding this epic.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `31052`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0783`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `75fece81b69742c6a5ebaea51059981f`
- completed-at-utc: `<redacted>-14T22:05:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPX99KQRB09GRQG50Z75FM/runs/20260514T220556685Z-75fece81b69742c6a5ebaea51059981f.json`