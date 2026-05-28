[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q90CSKMGK3NZZ25XTW6W4C'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q90CSKMGK3NZZ25XTW6W4C`.
- Optimistic claim succeeded (`expectedRevision=06F5Q966PBA1943GD0FTVHYRDM`, `currentRevision=06F6QQTCVZ3CZC4A84GDYD51GM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q90CSKMGK3NZZ25XTW6W4C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q90CSKMGK3NZZ25XTW6W4C': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q90CSKMGK3NZZ25XTW6W4C-epic-pit-and-bridge-completeness' from source '0ee9a7ae21e68d8faeafa3a605d9c3aa08afa8e6'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q90CSKMGK3NZZ25XTW6W4C-epic-pit-and-bridge-completeness` as `6c88cb72e295`.

Open questions / Risiken
- This epic currently has a live incoming blocks relation from 06F5Q90718D21DN1N1Q2AP7YEM.
- The bounded v1 stance on multi-active and link-parent PIT behavior can be destabilized if delivery tries to absorb cross-product tuple semantics or other deferred variants under this epic.
- Bridge completion can drift if delete-aware maintenance, advanced hierarchy semantics, or broader traversal features are implicitly added instead of being tracked as separate deferred work.
- Split recommendation: No additional split is recommended now; the epic already has six persisted child tickets linked by parentOf relations.
- Split recommendation: If new asks emerge for provider-specific optimization or orchestration, create separate follow-up tickets instead of broadening this epic's v1 boundary.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `24615`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0988`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1b09453768724d8a9e8c762f38ec181f`
- completed-at-utc: `<redacted>-28T00:08:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q90CSKMGK3NZZ25XTW6W4C/runs/20260528T000833071Z-1b09453768724d8a9e8c762f38ec181f.json`